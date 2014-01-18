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

        public class ActiveMQ
        {
            public const string Broker = "failover:tcp://sorceptionjava.cloudapp.net:61616?maximumConnections=1000";

            public class Solicitud
            {
                public const string Client_ID = "GestionTaller";
                public const string Topic = "Solicitudes";
            }

            public class Oferta
            {
                public const string Client_ID = "GestionTaller";
                public const string Consumer_ID = "SistemaGestor";
                public const string Topic = "Ofertas";
            }

            public class Pedido
            {
                public const string Topic = "Pedidos";
            }
        }

    }
}