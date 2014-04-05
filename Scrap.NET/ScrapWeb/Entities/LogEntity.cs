using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity;

namespace ScrapWeb.Entities
{
    public class LogEntity : AbstractEntity
    {
        public static string INFO = "INFO";
        public static string ERROR = "ERROR";
        public static string WARNING = "WARNING";

        public String description { get; set; }
        public String type { get; set; }

    }
}
