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
        private static string generateSubscriptionName(string machine_name)
        {
            string token_string = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token_string = token_string.Replace("=", "");
            token_string = token_string.Replace("+", "");

            return machine_name + "(" + token_string + ")";
        }

        public static IConnection Connection { get; private set; }
        public static ISession Session { get; private set; }

        public static void StartUp(string endpoint, string client_id)
        {
            var factory = new ConnectionFactory(endpoint, generateSubscriptionName(client_id));
            Connection = factory.CreateConnection();
            Connection.Start();
            Session = Connection.CreateSession();
        }
    }
}
