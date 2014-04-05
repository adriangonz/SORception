using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Eggplant.Entity
{
    public class Solicitud : AbstractEntity
    {
        public static string DELETED = "DELETED";
        public static string FAILED = "FAILED";

        public Solicitud()
        {
            rawLines = new List<LineaSolicitud>();
        }

        public int sg_id { get; set; }
        public string status { get; set; }

        
        [JsonIgnore]
        [InverseProperty("solicitud")]
        public ICollection<LineaSolicitud> rawLines { get; set; }

        public virtual IEnumerable<LineaSolicitud> lines
        {
            get
            {
                return rawLines.Where(t => t.status != LineaSolicitud.DELETED);
            }
        }
    }
}