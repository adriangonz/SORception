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
        SELECTED
    }

    public class OfferLineEntity : BaseEntity
    {
        public OfferLineEntity()
        {
            this.status = OfferLineStatus.NEW;
            this.selected_ammount = 0;
        }

        public OfferLineStatus status { get; set; }

        public int corresponding_id { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }

        public string notes { get; set; }

        public DateTime date { get; set; }

        public int offer_id { get; set; }

        [Required]
        [ForeignKey("offer_id")]
        public virtual OfferEntity offer { get; set; }

        public int order_line_id { get; set; }
        
        [Required]
        [ForeignKey("order_line_id")]
        public virtual OrderLineEntity order_line { get; set; }
        
        public int selected_ammount { get; set; }
    }
}