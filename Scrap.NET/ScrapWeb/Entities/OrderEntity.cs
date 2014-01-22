using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScrapWeb.Entities
{
    public class OrderEntity : AbstractEntity
    {
        public OrderEntity()
        {
            rawLines = new List<OrderLineEntity>();
        }

        public String sgId { get; set; }

        public DateTime deadline { get; set; }

        [DefaultValue("false")]
        public bool closed { get; set; }

        [JsonIgnore]
        [DefaultValue("false")]
        public bool deleted { get; set; }

        [JsonIgnore]
        [InverseProperty("order")]
        public ICollection<OrderLineEntity> rawLines { get; set; }

        public virtual ICollection<OrderLineEntity> lines 
        {
            get
            {
                return rawLines.Where(t => (t.offerLine != null && t.offerLine.acceptedOffer == null) || t.offerLine == null).ToList();
            }
        }

        public virtual OfferEntity offer
        {
            get
            {
                var orderline = lines.Where(t => t.offerLine != null).FirstOrDefault();
                if (orderline != null)
                    return orderline.offerLine.offer;
                return null;
            }
        }
    }
}