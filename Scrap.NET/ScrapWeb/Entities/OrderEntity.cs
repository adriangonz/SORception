using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScrapWeb.Entities
{
    public class OrderEntity : AbstractEntity
    {
        public OrderEntity()
        {
            lines = new List<OrderLineEntity>();
        }

        public String sgId { get; set; }

        public DateTime deadline { get; set; }

        [InverseProperty("order")]
        public ICollection<OrderLineEntity> lines { get; set; }
    }
}