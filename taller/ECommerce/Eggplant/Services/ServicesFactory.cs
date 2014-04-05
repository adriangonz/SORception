//#define TESTING

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Services
{
    public static class ServicesFactory
    {
        public static ISGService getSGService()
        {
#if TESTING
            return (ISGService)new DebugSGService();
#else
            return (ISGService)new SGService();
#endif
        }

        public static IDataService getDataService()
        {
            return (IDataService)new DataService();
        }
    }
}