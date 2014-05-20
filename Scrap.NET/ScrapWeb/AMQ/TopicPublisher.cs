using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using ScrapWeb.Entities;
using ScrapWeb.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScrapWeb.AMQ
{
    public class TopicPublisher
    {
        private bool _disposed;
        private readonly ISession _session;
        private readonly IConnection _connection;
        private readonly ITopic _topic;

        public IMessageProducer Producer { get; private set; }

        public TopicPublisher(ISession session, IConnection connection, string topicName)
        {
            _session = session;
            _connection = connection;
            _topic = new ActiveMQTopic(topicName);
            Producer = session.CreateProducer(_topic);
        }

        /*Creo que debo encryptar aqui*/
        public void SendMessage(object o, long delay = 0)
        {
            AESService aes_service = new AESService();
            TokenService tokenService = new TokenService();
            TokenEntity tokenEntity = tokenService.getValid();

            string message = ToXML(o);
            byte[] encryp_mesage = aes_service.encryptMessage_with_MyPair(message);

            Webservices.AMQSecureMessage secure_msg = new Webservices.AMQSecureMessage();

            secure_msg.data = encryp_mesage;
            secure_msg.junkyard_token = tokenEntity.token;

            if (_disposed) throw new ObjectDisposedException(GetType().Name);
            var textMessage = Producer.CreateTextMessage(ToXML(secure_msg));
            Producer.Send(textMessage);
        }

        public void Dispose()
        {
            if (_disposed) return;
            Producer.Close();
            Producer.Dispose();
            _session.Close();
            _connection.Close();
            _disposed = true;
        }

        public static string ToXML(object d)
        {
            using (MemoryStream memStr = new MemoryStream())
            {
                var serializer = new DataContractSerializer(d.GetType());
                serializer.WriteObject(memStr, d);

                memStr.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStr))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}