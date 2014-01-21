using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum GarageStatus
    {
        CREATED,
        ACTIVE
    }

    public class GarageEntity : BaseEntity
    {

        public GarageEntity()
        {
            this.tokens = new List<TokenEntity>();
        }

        public string name { get; set; }

        public GarageStatus status { get; set; }

        [InverseProperty("garage")]
        public virtual ICollection<TokenEntity> tokens { get; set; }

        public virtual string current_token
        {
            get
            {
                return tokens.First(t => t.status == TokenStatus.VALID).token;
            }
        }

        [InverseProperty("garage")]
        public virtual ICollection<OrderEntity> orders { get; set; }
    }
}