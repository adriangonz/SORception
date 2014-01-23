using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQHelper
{
    public class AMQConfig
    {
        public static IConnection Connection { get; private set; }
        public static ISession Session { get; private set; }

        public static void StartUp(string endpoint)
        {
            var factory = new ConnectionFactory(endpoint);
            Connection = factory.CreateConnection();
            Connection.Start();
            Session = Connection.CreateSession();
        }
    }
}
