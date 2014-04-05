using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum JunkyardStatus
    {
        CREATED,
        ACTIVE
    }

    public class JunkyardEntity : BaseEntity
    {

        public JunkyardEntity()
        {
            this.tokens = new List<JunkyardTokenEntity>();
        }

        public string name { get; set; }

        public JunkyardStatus status { get; set; }

        [InverseProperty("junkyard")]
        public virtual ICollection<JunkyardTokenEntity> tokens { get; set; }

        public virtual string current_token
        {
            get
            {
                return tokens.First(t => t.status == TokenStatus.VALID).token;
            }
        }

        [InverseProperty("junkyard")]
        public virtual ICollection<OfferEntity> offers { get; set; }
    }
}