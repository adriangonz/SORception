using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Eggplant.Entity
{
    public class Audit : AbstractEntity
    {

        public static string INFO = "INFO";
        public static string ERROR = "ERROR";
        public static string WARNING = "WARNING";

        public string type { get; set; }

        public string description { get; set; }
    }
}