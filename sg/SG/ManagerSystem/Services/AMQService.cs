using ActiveMQHelper;
using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;


namespace ManagerSystem.Services
{
    public class AMQService : BaseService
    {
        private List<TopicPublisher> subscribers;

        public AMQService(UnitOfWork uow = null)
            : base(uow)
        {
            this.subscribers = new List<TopicPublisher>();
        }

        public void publishOrder(ExpSolicitud order, AMQSolicitudMessage.Code code)
        {
            AMQSolicitudMessage msg = new AMQSolicitudMessage
            {
                code = code,
                solicitud = order
            };

            TopicPublisher publisher = TopicPublisher.MakeDefaultPublisher(Config.ActiveMQ.Topics.Orders);

            AESPairEntity aes_pair = configService.getAESPair();
            AMQSecureMessage secure_msg = this.getSecureMessage((object)msg, aes_pair);

            publisher.SendMessage((object)secure_msg);
        }

        public AMQSecureMessage getSecureMessage(object msg, AESPairEntity aes_pair, string junkyard_token = "")
        {
            string message = TopicPublisher.ToXML(msg);
            byte[] encrypted_msg = aesService.encryptMessage(message, aes_pair);
            return new AMQSecureMessage()
            {
                data = encrypted_msg,
                junkyard_token = junkyard_token
            };
        }

        public void publishOrderConfirmation(ExpPedido order_confirmation, JunkyardEntity junkyard)
        {
            AMQPedidoMessage msg = new AMQPedidoMessage
            {
                pedido = order_confirmation,
                desguace_id = junkyard.current_token
            };

            TopicPublisher publisher = TopicPublisher.MakeDefaultPublisher(Config.ActiveMQ.Topics.OfferConfirmations);

            AESPairEntity aes_pair = junkyard.aes_pair;
            AMQSecureMessage secure_msg = this.getSecureMessage((object)msg, aes_pair);

            publisher.SendMessage((object)secure_msg);
        }

        public void scheduleJob(JobEntity job)
        {
            AMQScheduledJob msg = new AMQScheduledJob
            {
                id_solicitud = job.order.id,
                xsrf_token = job.xsrf_token
            };
            TimeSpan delay = job.order.deadline - DateTime.Now;

            TopicPublisher publisher = TopicPublisher.MakeDefaultPublisher(Config.ActiveMQ.Topics.ScheduledJobs);

            //AESPairEntity aes_pair = configService.getAESPair();
            //AMQSecureMessage secure_msg = this.getSecureMessage((object)msg, aes_pair);

            publisher.SendMessage((object)msg, (long)delay.TotalMilliseconds);
        }

        public void createOfferSubscriber()
        {
            string subscription_name = this.generateSubscriptionName(System.Environment.MachineName);

            TopicSubscriber topicSubscriber = TopicSubscriber.MakeDefaultSubscriber(Config.ActiveMQ.Topics.Offers);
            topicSubscriber.OnMessageReceived += offerSubscriber_OnMessageReceived;

            topicSubscriber.Start(subscription_name);
            logService.Info("Connected to the Offer ActiveMQ queue");
        }

        public void createScheduledJobSubscriber()
        {
            string subscription_name = this.generateSubscriptionName(System.Environment.MachineName);

            TopicSubscriber topicSubscriber = TopicSubscriber.MakeDefaultSubscriber(Config.ActiveMQ.Topics.ScheduledJobs);
            topicSubscriber.OnMessageReceived += scheduledJobSubscriber_OnMessageReceived;

            topicSubscriber.Start(subscription_name);
            logService.Info("Connected to the Job ActiveMQ queue");
        }

        public void processOfferMessage(AMQSecureMessage msg)
        {
            logService.Info("Started processing offer message");
            JunkyardEntity junkyard = junkyardService.getJunkyardWithToken(msg.junkyard_token);
            AESPairEntity aes_pair = junkyard.aes_pair;

            string xml_msg = aesService.decryptMessage(msg.data, aes_pair);
            AMQOfertaMessage decoded_msg = (AMQOfertaMessage)TopicSubscriber.FromXML(xml_msg, (new AMQOfertaMessage()).GetType());
            logService.Info(xml_msg);
            //logService.Info("Decrypted offer message with token " + decoded_msg.desguace_id);
            authService.setJunkyardToken(decoded_msg.desguace_id);
            logService.Info(authService.getToken());
            authService.authenticateCall();

            offerService.current_junkyard = decoded_msg.desguace_id;
            switch (decoded_msg.code)
            {
                case AMQOfertaMessage.Code.New:
                    offerService.addOffer(decoded_msg.oferta);
                    break;
                case AMQOfertaMessage.Code.Update:
                    offerService.putOffer(decoded_msg.oferta);
                    break;
                case AMQOfertaMessage.Code.Delete:
                    offerService.deleteOffer(decoded_msg.oferta.id);
                    break;
            }
            logService.Info("Finished processing offer message");
        }

        private void offerSubscriber_OnMessageReceived(string message)
        {
            AMQSecureMessage msg = (AMQSecureMessage)TopicSubscriber.FromXML(message, (new AMQSecureMessage()).GetType());
            this.processOfferMessage(msg);
        }

        private void scheduledJobSubscriber_OnMessageReceived(string message)
        {
            AMQScheduledJob msg = (AMQScheduledJob)TopicSubscriber.FromXML(message, (new AMQScheduledJob()).GetType());
            this.processScheduledJob(msg);
        }

        private void processScheduledJob(AMQScheduledJob msg)
        {
            if (jobService.jobIsValid(msg.id_solicitud, msg.xsrf_token))
            {
                purchaseService.processOnDeadlinePurchases(msg.id_solicitud);
            }
        }

        private string generateSubscriptionName(string machine_name)
        {
            string token_string = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token_string = token_string.Replace("=", "");
            token_string = token_string.Replace("+", "");

            return machine_name + "(" + token_string + ")";
        }
    }
}