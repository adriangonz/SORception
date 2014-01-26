using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScrapWeb.Entities
{
    public class OfferLineEntity : AbstractEntity
    {
        public int quantity { get; set; }

        public string notes { get; set; }

        public Double price { get; set; }


        public DateTime date;

        [JsonIgnore]
        [DefaultValue("false")]
        public bool deleted { get; set; }

        public int offerId { get; set; }

        [Required]
        [ForeignKey("offerId")]
        public virtual OfferEntity offer { get; set; }

        public int orderLineId { get; set; }

        [Required]
        [ForeignKey("orderLineId")]
        public virtual OrderLineEntity orderLine { get; set; }
        
        [InverseProperty("offerLine")]
        public virtual AcceptedOfferLineEntity acceptedOffer { get; set; }
    }
}