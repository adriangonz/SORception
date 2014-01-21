using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public class GarageEntity : AbstractEntity
    {
        public string name { get; set; }

        [InverseProperty("garage")]
        public virtual ICollection<TokenEntity> tokens { get; set; }

        public virtual TokenEntity token
        {
            get
            {
                return tokens.First(t => t.status == TokenStatus.TEMPORAL || t.status == TokenStatus.VALID);
            }
        }

        [InverseProperty("garage")]
        public ICollection<OrderEntity> orders { get; set; }
    }
}