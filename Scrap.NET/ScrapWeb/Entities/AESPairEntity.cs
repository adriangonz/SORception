using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ScrapWeb.Entities
{
    public class AESPairEntity : AbstractEntity
    {
        public enum AESPairType
        {
            MINE,
            SG
        }
        public byte[] key { get; set; }
        public byte[] iv { get; set; }
        public AESPairType type { get; set; }

    }
}