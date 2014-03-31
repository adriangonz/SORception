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
        public String action { get; set; }

        public LogEntity(String msg)
        {
           action = msg;
        }
    }
}
