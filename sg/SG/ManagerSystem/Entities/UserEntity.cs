using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum UserType
    {
        JUNKYARD,
        GARAGE,
        ADMIN,
        APP
    }

    public class UserEntity : BaseEntity
    {

        public DateTime last_access { get; set; }

        public UserType type;

        public int garage_id { get; set; }

        [ForeignKey("garage_id")]
        public virtual GarageEntity garage { get; set; }

        public int junkyard_id { get; set; }

        [ForeignKey("junkyard_id")]
        public virtual JunkyardEntity junkyard { get; set; }

        public virtual string name
        {
            get
            {
                return String.Format("{0}({1})", type.ToString(), id);
            }
        }
    }
}