using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public class AESPairEntity : BaseEntity
    {
        public byte[] key { get; set; }
        public byte[] iv { get; set; }
    }
}