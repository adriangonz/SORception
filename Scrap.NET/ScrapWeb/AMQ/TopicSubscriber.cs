using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScrapWeb.AMQ
{
    public class TopicSubscriber
    {
        private readonly ISession session;
        private readonly ITopic topic;
        private readonly String destination;
        private bool disposed = false;

        public TopicSubscriber(ISession session, string destination)
        {
            this.session = session;
            this.destination = destination;
            topic = new ActiveMQTopic(this.destination);
        }

        public delegate void MessageReceivedDelegate(string message);
        public event MessageReceivedDelegate OnMessageReceived;

        public IMessageConsumer Consumer { get; private set; }

        public string ConsumerId { get; private set; }

        public void Start(string consumerId)
        {
            ConsumerId = consumerId;
            Consumer = session.CreateDurableConsumer(topic, consumerId, "2 > 1", false);
            Consumer.Listener += (message =>
            {
                var textMessage = message as ITextMessage;
                if (textMessage == null) throw new InvalidCastException();
                if (OnMessageReceived != null)
                {
                    OnMessageReceived(textMessage.Text);
                }
            });
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

        public object FromXML(string text, Type to_type)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(to_type);
                object o = deserializer.ReadObject(stream);
                return o;
            }
        }
    }
}