using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrapWeb.Entities
{
    /* This class is not persisted */
    public class SettingsEntity
    {
        public String name;
        public TokenEntity validToken;
        public IEnumerable<TokenEntity> tokenList;

        public AESPairEntity myAES { get; set; }
        public AESPairEntity sgAES { get; set; }
    }
}