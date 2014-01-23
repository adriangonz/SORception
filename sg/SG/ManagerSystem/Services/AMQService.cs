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
            subscribers = new List<TopicPublisher>();        
        }

        public void publishOrder(AMQSolicitudMessage msg)
        {
            TopicPublisher publisher = new TopicPublisher(
                AMQConfig.Session, AMQConfig.Connection, Config.ActiveMQ.Topics.Orders);

            publisher.SendMessage(TopicPublisher.ToXML((object) msg));
        }

        public void publishOrderConfirmation(AMQPedidoMessage msg)
        {
            TopicPublisher publisher = new TopicPublisher(
                AMQConfig.Session, AMQConfig.Connection, Config.ActiveMQ.Topics.OfferConfirmations);

            publisher.SendMessage(TopicPublisher.ToXML((object)msg));
        }

        public void scheduleJob(AMQScheduledJob msg, long delay)
        {
            TopicPublisher publisher = new TopicPublisher(
                AMQConfig.Session, AMQConfig.Connection, Config.ActiveMQ.Topics.ScheduledJobs);

            publisher.SendMessage(TopicPublisher.ToXML((object)msg), delay);
        }

        public void createOfferSubscriber()
        {
            TopicSubscriber topicSubscriber = new TopicSubscriber(
                AMQConfig.Session, Config.ActiveMQ.Topics.Offers);
            topicSubscriber.OnMessageReceived += offerSubscriber_OnMessageReceived;
            topicSubscriber.Start(System.Environment.MachineName);
        }

        public void createScheduledJobSubscriber()
        {
            TopicSubscriber topicSubscriber = new TopicSubscriber(
                AMQConfig.Session, Config.ActiveMQ.Topics.Offers);
            topicSubscriber.OnMessageReceived += scheduledJobSubscriber_OnMessageReceived;
            topicSubscriber.Start(System.Environment.MachineName);
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
    }
}