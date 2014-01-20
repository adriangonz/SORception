using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class Constants
    {
        // Ensures consistency in the namespace declarations across services
        public const string Namespace = "http://sorception.azurewebsites.net/";

        public static managersystemEntities context = new managersystemEntities();

        public class ActiveMQ
        {
            public const string Broker = "failover:tcp://sorceptionjava.cloudapp.net:61616?maximumConnections=1000";
            public const string Client_ID = "SistemaGestor";

            public const string Solicitudes_Topic = "Solicitudes";
            public const string Ofertas_Topic = "Ofertas";
            public const string Pedidos_Topic = "Pedidos";
            public const string Jobs_Topic = "ScheduledJobs";
        }

    }
}