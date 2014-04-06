using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum TokenStatus
    {
        VALID,
        EXPIRED
    }

    public enum TokenType
    {
        TEMPORAL,
        FINAL
    }

    public class TokenEntity : BaseEntity
    {
        public string token { get; set; }

        public TokenStatus status { get; set; }

        public TokenType type { get; set; }
        public int garage_id { get; set; }

        [ForeignKey("garage_id")]
        public virtual GarageEntity garage { get; set; }
        public int junkyard_id { get; set; }

        [ForeignKey("junkyard_id")]
        public virtual JunkyardEntity junkyard { get; set; }
    }
}