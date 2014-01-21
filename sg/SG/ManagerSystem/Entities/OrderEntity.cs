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

    public class OrderEntity : AbstractEntity
    {
        public OrderStatus status { get; set; }

        public int corresponding_id { get; set; }

        public DateTime deadline { get; set; }

        public int garage_id { get; set; }

        [Required]
        [ForeignKey("garage_id")]
        public virtual GarageEntity garage { get; set; }


    }
}