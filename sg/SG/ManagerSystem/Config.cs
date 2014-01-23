using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class Config
    {
        // Ensures consistency in the namespace declarations across services
        public const string Namespace = "http://sorception.azurewebsites.net/";

        public class ActiveMQ
        {
            public const string Broker = "failover:tcp://sorceptionjava.cloudapp.net:61616?maximumConnections=1000";
            public const string Client_ID = "SistemaGestor";

            public class Topics
            {
                public const string Orders = "Solicitudes";
                public const string Offers = "Ofertas";
                public const string OfferConfirmations = "Pedidos";
                public const string ScheduledJobs = "ScheduledJobs";
            }
        }

    }
}