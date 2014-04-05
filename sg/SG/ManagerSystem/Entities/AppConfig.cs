using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public class AppConfig : BaseEntity
    {
        public int aes_pair_id { get; set; }

        [Required]
        [ForeignKey("aes_pair_id")]
        public virtual AESPairEntity aes_pair { get; set; }
    }
}