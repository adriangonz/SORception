using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum OfferStatus 
    {
        NEW,
        HAS_RESPONSE,
        CLOSED
    }

    public class OfferEntity : BaseEntity
    {
        public OfferEntity()
        {
            lines = new List<OfferLineEntity>();
            this.status = OfferStatus.NEW;
        }

        public OfferStatus status { get; set; }

        public int corresponding_id { get; set; }

        public int junkyard_id { get; set; }

        [Required]
        [ForeignKey("junkyard_id")]
        public JunkyardEntity junkyard { get; set; }

        [InverseProperty("offer")]
        public List<OfferLineEntity> lines { get; set; }

        public virtual OrderEntity order
        {
            get
            {
                if (lines.Count > 0)
                {
                    return lines[0].order_line.order;
                }
                return null;
            }
        }
    }
}