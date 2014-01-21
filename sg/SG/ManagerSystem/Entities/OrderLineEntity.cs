using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum OrderLineStatus 
    {
        NEW,
        HAS_RESPONSE,
        INCOMPLETE,
        COMPLETE
    }

    public enum OrderLineFlag 
    {
        NONE,
        FIRST,
        CHEAPEST,
        NEWEST
    }

    public class OrderLineEntity : BaseEntity
    {
        public int quantity { get; set; }

        public string description { get; set; }

        public OrderLineStatus status { get; set; }

        public OrderLineFlag flag { get; set; }

        public int order_id { get; set; }

        [Required]
        [ForeignKey("order_id")]
        public OrderEntity order { get; set; }

        [InverseProperty("order_line")]
        public virtual ICollection<OfferLineEntity> offers;

        public virtual ICollection<OfferLineEntity> selected_offers
        {
            get
            {
                return offers.Where(o => o.status == OfferLineStatus.SELECTED || o.status == OfferLineStatus.COMPLETE).ToList();
            }
        }

        public virtual int selected_ammount
        {
            get
            {
                return selected_offers.Sum(o => o.selected_ammount);
            }
        }
    }
}