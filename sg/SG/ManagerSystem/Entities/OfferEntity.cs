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
        public OfferStatus status { get; set; }

        public int corresponding_id { get; set; }

        public int junkyard_id { get; set; }

        [Required]
        [ForeignKey("junkyard_id")]
        public JunkyardEntity junkyard { get; set; }

        public int order_id { get; set; }

        [Required]
        [ForeignKey("order_id")]
        public JunkyardEntity order { get; set; }

    }
}