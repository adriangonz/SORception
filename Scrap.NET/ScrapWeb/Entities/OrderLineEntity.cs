using Newtonsoft.Json;
using ScrapWeb.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ScrapWeb.Entities
{
    public class OrderLineEntity : AbstractEntity
    {

        public OrderLineEntity()
        {
            offerLines = new List<OfferLineEntity>();
        }

        public String sgId { get; set; }
        
        public String description { get; set; }
        
        public int quantity { get; set; }

        public int orderId { get; set; }

        [JsonIgnore]
        [DefaultValue("false")]
        public bool deleted { get; set; }

        [JsonIgnore]
        [Required]
        [ForeignKey("orderId")]
        public virtual OrderEntity order { get; set; }

        [JsonIgnore]
        [InverseProperty("orderLine")]
        public virtual ICollection<OfferLineEntity> offerLines { get; set; }

        public virtual OfferLineEntity offerLine
        {
            get 
            {
                return offerLines.Where(t => t.deleted == false).FirstOrDefault();
            }
        }
    }
}
