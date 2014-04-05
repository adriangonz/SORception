using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum OrderStatus
    {
        NEW,
        SENT,
        HAS_RESPONSE,
        CLOSED
    }

    public class OrderEntity : BaseEntity
    {
        public OrderEntity()
        {
            this.lines = new List<OrderLineEntity>();
            this.status = OrderStatus.NEW;
        }

        public OrderStatus status { get; set; }

        public int corresponding_id { get; set; }

        public DateTime deadline { get; set; }

        public int garage_id { get; set; }

        [Required]
        [ForeignKey("garage_id")]
        public virtual GarageEntity garage { get; set; }

        [InverseProperty("order")]
        public ICollection<OrderLineEntity> lines { get; set; }

        public List<OfferEntity> offers
        {
            get
            {
                HashSet<OfferEntity> offers = new HashSet<OfferEntity>();
                foreach (var line in this.lines)
                {
                    if (line.deleted)
                        continue;
                    foreach (var line_offer in line.offers)
                    {
                        offers.Add(line_offer.offer);
                    }
                }
                return offers.ToList();
            }
         }
    }
}