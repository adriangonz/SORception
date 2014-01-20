using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ScrapWeb.Entities
{
    public class OrderLineEntity : AbstractEntity
    {
        public String sgId { get; set; }
        
        public String description { get; set; }
        
        public int quantity { get; set; }

        public int orderId { get; set; }

        [ForeignKey("orderId")]
        public virtual OrderEntity order { get; set; }
    }
}
