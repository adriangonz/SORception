using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Entity
{
    public class Token : AbstractEntity
    {
        public static string REQUESTED = "REQUESTED";
        public static string EXPIRED = "EXPIRED";
        public static string ACTIVE = "ACTIVE";

        public string status { get; set; }
        public string token { get; set; }
    }
}