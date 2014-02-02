using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Eggplant.Entity
{
    public class LineaSolicitud : AbstractEntity
    {
        public static string EFFECT_NEW = "NEW";
        public static string EFFECT_UPDATED = "UPDATED";
        public static string EFFECT_DELETE = "DELETED";
        public static string EFFECT_NOEFECT = "NOEFECT";

        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public int sg_id { get; set; }
        public string criterio { get; set; }
        public string status { get; set; }

        public int solicitudId { get; set; }

        
        [Required]
        [ForeignKey("solicitudId")]
        public virtual Solicitud solicitud { get; set; }
        
    }
}