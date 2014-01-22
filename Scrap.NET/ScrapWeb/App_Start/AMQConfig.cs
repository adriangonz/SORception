using Apache.NMS;
using Apache.NMS.ActiveMQ;
using ScrapWeb.AMQ;
using ScrapWeb.Config;
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
        public static IConnection Connection { get; set; }
        public static ISession Session { get; set; }

        public static String AmqUrl;

        AMQConfig()
        {
        }

        private static void createConsumer()
        {
            var amqService = new AMQService();
            amqService.createTopicSubscribers();
        }

        public static void RegisterActiveMQ()
        {
            Connection = null;
            Session = null;
            AmqUrl = ScrapSettingsConfiguration.Instance.ActiveMQURL;
            // Check if we can create a consumer
            createConsumer();
        }
    }
}