using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eggplant.Entity
{
    public class Pedido : AbstractEntity
    {
        public Pedido()
        {
            rawLines = new List<LineaPedido>();
        }

        [JsonIgnore]
        public ICollection<LineaPedido> rawLines { get; set; }


        public virtual IEnumerable<LineaPedido> lines
        {
            get
            {
                return rawLines.Where(t => t.status != LineaPedido.DELETED);
            }
        }

        public int solicitudId { get; set; }

        [Required]
        [ForeignKey("solicitudId")]
        public virtual Solicitud solicitud { get; set; }

    }
}