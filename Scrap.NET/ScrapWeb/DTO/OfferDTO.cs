using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrapWeb.DTO
{
    public class OfferPostDTO
    {
        public List<OfferLineDTO> lines { get; set; }
    }

    public class OfferLineDTO
    {
        public string status { get; set; }
        public string notes { get; set; }
        public Double price { get; set; }
        public int quantity { get; set; }
        public int orderLineId { get; set; }
        public int id { get; set; }
    }
}