using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public enum LogLevel { INFO, WARNING, ERROR, CRITICAL }

    public class LogEntity : BaseEntity
    {
        public string message { get; set; }

        public int? user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual UserEntity user { get; set; }

        public LogLevel level;
    }
}