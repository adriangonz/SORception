using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.ServiceTaller
{
    public partial class ExpSolicitudLine
    {
        public static string CRITERIO_NONE = "NONE";
        public static string CRITERIO_FIRST = "FIRST";
        public static string CRITERIO_CHEAPEST = "CHEAPEST";
        public static string CRITERIO_NEWEST = "NEWEST";
        
        public static string castToFlag(string criterio)
        {
            switch (criterio)
            {
                case "1":
                    return CRITERIO_FIRST;
                case "2":
                    return CRITERIO_CHEAPEST;
                case "3":
                    return CRITERIO_NEWEST;
                default:
                    return CRITERIO_NONE;
            }
        }
    }
}