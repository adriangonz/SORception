using ActiveMQHelper;
using ManagerSystem.DataAccess;
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

        public AMQService(UnitOfWork uow = null) : base(uow) 
        {
            this.subscribers = new List<TopicPublisher>();
        }

        public void publishOrder(AMQSolicitudMessage msg)
        {
            TopicPublisher publisher = TopicPublisher.MakeDefaultPublisher(Config.ActiveMQ.Topics.Orders);

            publisher.SendMessage(TopicPublisher.ToXML((object) msg));
        }

        public void publishOrderConfirmation(AMQPedidoMessage msg)
        {
            TopicPublisher publisher = TopicPublisher.MakeDefaultPublisher(Config.ActiveMQ.Topics.OfferConfirmations);

            publisher.SendMessage(TopicPublisher.ToXML((object)msg));
        }

        public void scheduleJob(AMQScheduledJob msg, long delay)
        {
            TopicPublisher publisher = TopicPublisher.MakeDefaultPublisher(Config.ActiveMQ.Topics.ScheduledJobs);

            publisher.SendMessage(TopicPublisher.ToXML((object)msg), delay);
        }

        public void createOfferSubscriber()
        {
            string subscription_name = this.generateSubscriptionName(System.Environment.MachineName);

            TopicSubscriber topicSubscriber = TopicSubscriber.MakeDefaultSubscriber(Config.ActiveMQ.Topics.Offers);
            topicSubscriber.OnMessageReceived += offerSubscriber_OnMessageReceived;

            topicSubscriber.Start(subscription_name);
        }

        public void createScheduledJobSubscriber()
        {
            string subscription_name = this.generateSubscriptionName(System.Environment.MachineName);

            TopicSubscriber topicSubscriber = TopicSubscriber.MakeDefaultSubscriber(Config.ActiveMQ.Topics.ScheduledJobs);
            topicSubscriber.OnMessageReceived += scheduledJobSubscriber_OnMessageReceived;

            topicSubscriber.Start(subscription_name);
        }

        public void processOfferMessage(AMQOfertaMessage msg)
        {
            authorizationService.setJunkyardToken(msg.desguace_id);
            if (!authorizationService.isJunkyardAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            switch (msg.code)
            {
                case AMQOfertaMessage.Code.New:
                    offerService.addOffer(msg.oferta);
                    break;
                case AMQOfertaMessage.Code.Update:
                    offerService.putOffer(msg.oferta);
                    break;
                case AMQOfertaMessage.Code.Delete:
                    offerService.deleteOffer(msg.oferta.id);
                    break;
            }
        }

        private void offerSubscriber_OnMessageReceived(string message)
        {
            AMQOfertaMessage msg = (AMQOfertaMessage)TopicSubscriber.FromXML(message, (new AMQOfertaMessage()).GetType());
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