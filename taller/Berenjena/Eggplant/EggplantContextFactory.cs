using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant
{
    public class EggplantContextFactory
    {
        public static BDBerenjenaContainer getContext()
        {
            return new BDBerenjenaContainer();
        }
    }
}