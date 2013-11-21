using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace ActiveMQHelper
{
    public class TopicPublisher : IDisposable
    {
        private bool _disposed;
        private readonly ISession _session;
        private readonly ITopic _topic;

        public IMessageProducer Producer { get; private set; }
        public string DestinationName { get; private set; }

        public TopicPublisher(ISession session, string topicName)
        {
            _session = session;
            DestinationName = topicName;
            _topic = new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(DestinationName);
            Producer = session.CreateProducer(_topic);
        }

        public void SendMessage(string message)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            var textMessage = Producer.CreateTextMessage(message);
            //Producer.Send(textMessage, MsgDeliveryMode.Persistent, MsgPriority.Low, new TimeSpan(0,0,10));
            Producer.Send(textMessage);
        }

        public void Dispose()
        {
            if (_disposed) return;
            Producer.Close();
            Producer.Dispose();
            _disposed = true;
        }
    }
}