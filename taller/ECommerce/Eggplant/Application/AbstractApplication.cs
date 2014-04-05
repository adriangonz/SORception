using Eggplant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Application
{
    public class AbstractApplication
    {
        protected IDataService dataService;
        protected ISGService sgService;

        public AbstractApplication()
        {
            dataService = ServicesFactory.getDataService();
            sgService = ServicesFactory.getSGService();
        }

    }
}