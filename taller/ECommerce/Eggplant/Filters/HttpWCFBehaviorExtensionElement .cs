using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;

namespace Eggplant.Filters
{
    public class HttpWCFBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(HttpWCFEndpointBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new HttpWCFEndpointBehavior();
        }
    }
}