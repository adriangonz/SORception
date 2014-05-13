using org.apache.juddi.v3.client;
using org.apache.juddi.v3.client.config;
using org.apache.juddi.v3.client.transport;
using org.uddi.apiv3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Eggplant.Repositories;
using Eggplant.Entity;
using Eggplant.ServiceTaller;

namespace Eggplant.Services
{
    public class UDDIService
    {
        UDDIClient clerkManager = null;
        Transport transport = null;
        UDDIClerk clerk = null;


        public void findEndpointAvailable()
        {

        }
    }
}