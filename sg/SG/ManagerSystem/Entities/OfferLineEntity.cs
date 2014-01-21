using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum OfferLineStatus
    {
        NEW,
        SELECTED,
        COMPLETE
    }

    public class OfferLineEntity : AbstractEntity
    {
        public OfferLineStatus status { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }

        public int offer_id { get; set; }

        [Required]
        [ForeignKey("offer_id")]
        public OrderEntity order { get; set; }

        public int order_line_id { get; set; }

        [Required]
        [ForeignKey("order_line_id")]
        public OrderLineEntity order_line { get; set; }

        public int selected_ammount { get; set; }
    }
}