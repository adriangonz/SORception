﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Eggplant.Entity
{
    public class LineaPedido : AbstractEntity
    {
        public static string DELETED = "DELETED";
        public static string AUTOGENERATED = "AUTOGENERATED";
        public static string FAILED = "FAILED";
        public static string SENT = "SENT";

        public string status { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public int sg_linea_oferta_id { get; set; }
        public int sg_lina_solicitud_id { get; set; }

        public int pedidoId { get; set; }

        [Required]
        [ForeignKey("pedidoId")]
        public virtual Pedido pedido { get; set; }

        [NotMapped]
        public string description { get; set; }
    }
}