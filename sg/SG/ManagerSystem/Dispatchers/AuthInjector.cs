using ManagerSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;

namespace ManagerSystem.Dispatchers
{
    public class AuthInjector : IParameterInspector
    {
        private AuthService auth_service = null;
        private AuthService authService
        {
            get
            {
                if (this.auth_service == null)
                    this.auth_service = new AuthService();
                return this.auth_service;
            }
        }

        List<String> no_auth_methods = new List<String>() { "getState", "signUp" };

        public object BeforeCall(string operationName, object[] inputs)
        {
            if (!no_auth_methods.Contains(operationName))
            {
                authService.authenticateCall();
            }
            return null;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {

        }
    }
}