using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public class ScrapyardEntity : AbstractEntity
    {
        public string name { get; set; }

        [InverseProperty("scrapyard")]
        public virtual ICollection<TokenEntity> tokens { get; set; }
    }
}