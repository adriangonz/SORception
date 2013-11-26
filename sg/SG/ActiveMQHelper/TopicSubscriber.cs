using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace ActiveMQHelper
{
    public class TopicSubscriber : IDisposable
    {
        private bool disposed;
        private readonly ISession _session;
        private readonly ITopic _topic;
        private readonly string _destination;

        public IMessageConsumer Consumer { get; private set; }
        public string ConsumerId { get; private set; }

        public delegate void MessageReceivedDelegate(string message);
        public event MessageReceivedDelegate OnMessageReceived;

        public static TopicSubscriber MakeSubscriber(string broker, string client_id, string topic)
        {
            IConnectionFactory connectionFactory = new ConnectionFactory(broker, client_id);
            IConnection connection = connectionFactory.CreateConnection();
            connection.Start();
            ISession session = connection.CreateSession();

            TopicSubscriber subscriber = new TopicSubscriber(session, topic);
            return subscriber;
        }

        public TopicSubscriber(ISession session, string destination)
        {
            _session = session;
            _destination = destination;
            _topic = new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(this._destination);
        }

        public void Start(string consumerId)
        {
            ConsumerId = consumerId;
            Consumer = _session.CreateDurableConsumer(_topic, consumerId, null, false);
            Consumer.Listener += NewMessageHandler;
        }

        void NewMessageHandler(IMessage message)
        {
            var textMessage = message as ITextMessage;
            if (textMessage == null) throw new InvalidCastException();
            if (OnMessageReceived != null)
                OnMessageReceived(textMessage.Text);
            textMessage.Acknowledge();
        }

        public void Dispose()
        {
            if (disposed) return;
            if (Consumer != null)
            {
                Consumer.Close();
                Consumer.Dispose();
            }
            disposed = true;
        }
    }
}