using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace ManagerSystem.Dispatchers
{
    public class AuthInjectorAttribute : Attribute, IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            IParameterInspector parameterInspector = new AuthInjector();

            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in dispatcher.Endpoints)
                {
                    DispatchRuntime dispatchRuntime = endpointDispatcher.DispatchRuntime;
                    IEnumerable<DispatchOperation> dispatchOperations = dispatchRuntime.Operations;

                    foreach (DispatchOperation dispatchOperation in dispatchOperations)
                    {
                        dispatchOperation.ParameterInspectors.Add(parameterInspector);
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}