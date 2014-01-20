using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrapWeb.Entities
{
    public enum TokenStatus
    {
        VALID,
        EXPIRED,
        TEMPORAL,
        REQUESTED
    }

    public class TokenEntity : AbstractEntity
    {
        public string token { get; set; }

        public TokenStatus status { get; set; }
    }
}