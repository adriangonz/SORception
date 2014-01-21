using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

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
    }
}
