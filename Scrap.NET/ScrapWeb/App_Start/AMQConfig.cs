using Apache.NMS;
using Apache.NMS.ActiveMQ;
using ScrapWeb.AMQ;
using ScrapWeb.Entities;
using ScrapWeb.Exceptions;
using ScrapWeb.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ScrapWeb
{
    public class AMQConfig
    {
        public static IConnection Connection { get; private set; }
        public static ISession Session { get; private set; }

        private static readonly String amqUrl = "tcp://sorceptionjava.cloudapp.net:61616";

        AMQConfig()
        {
        }

        private static void createConsumer()
        {
            var amqService = new AMQService();
            amqService.createTopicSubscriber();
        }

        internal static void RegisterActiveMQ()
        {
            var factory = new ConnectionFactory(amqUrl);
            Connection = factory.CreateConnection();
            Connection.Start();
            Session = Connection.CreateSession();
            // Check if we can create a consumer
            createConsumer();
        }
    }
}