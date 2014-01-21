using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScrapWeb.Entities
{
    public class AcceptedOfferLineEntity
    {
        [Key]
        public int id { get; set; }

        public int quantity { get; set; }

        [Required]
        [ForeignKey("id")]
        public OfferLineEntity offerLine { get; set; }
    }
}