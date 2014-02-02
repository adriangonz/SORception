﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Entity
{
    public class Solicitud : AbstractEntity
    {
        public static string DELETED = "DELETED";

        public Solicitud()
        {
            rawLines = new List<LineaSolicitud>();
        }

        public int sg_id { get; set; }
        public string status { get; set; }
        public string user_id { get; set; }

        
        [JsonIgnore]
        public ICollection<LineaSolicitud> rawLines { get; set; }

        public virtual IEnumerable<LineaSolicitud> lines
        {
            get
            {
                return rawLines.Where(t => t.status != DELETED);
            }
        }
    }
}