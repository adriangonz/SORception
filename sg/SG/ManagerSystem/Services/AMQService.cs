using ActiveMQHelper;
using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private void offerSubscriber_OnMessageReceived(string message)
        {
            AMQOfertaMessage msg = (AMQOfertaMessage)TopicSubscriber.FromXML(message, (new AMQOfertaMessage()).GetType());
            // TODO
        }

        private void scheduledJobSubscriber_OnMessageReceived(string message)
        {
            AMQScheduledJob msg = (AMQScheduledJob)TopicSubscriber.FromXML(message, (new AMQScheduledJob()).GetType());
            // TODO
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