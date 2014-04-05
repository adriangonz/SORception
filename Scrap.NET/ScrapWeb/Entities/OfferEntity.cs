using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity;

namespace ScrapWeb.Entities
{
    public class OfferEntity : AbstractEntity
    {
        public OfferEntity()
        {
            rawLines = new List<OfferLineEntity>();
           
        }

        public virtual string orderSgId
        {
            get
            {
                return lines.Count() > 0 ? lines.First().orderLine.order.sgId : "-1";
            }
        }


        [JsonIgnore]
        [DefaultValue("false")]
        public bool deleted { get; set; }

        [JsonIgnore]
        [InverseProperty("offer")]
        public ICollection<OfferLineEntity> rawLines { get; set; }

        public virtual IEnumerable<OfferLineEntity> lines 
        { 
            get 
            {
                return rawLines.Where(t => !t.deleted);
            } 
        }

        [JsonIgnore]
        public virtual IEnumerable<OfferLineEntity> accepted
        {
            get
            {
                return lines.Where(t => t.acceptedOffer != null);
            }
        }

        public virtual int? orderId
        {
            get
            {
                if (lines.Count() > 0 && lines.First().orderLine != null)
                    return lines.First().orderLine.order.id;
                return null;
            }
        }
    }
}
