using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace ManagerSystem.Services
{
    public class AuthorizationService : BaseService
    {
        public AuthorizationService(UnitOfWork uow = null) : base(uow) { }

        public string getCurrentToken()
        {
            #if DEBUG
            return "Nv8nAmhXoEmmEQMWbCDD/w";
            #endif

            try
            {
                return OperationContext.Current.IncomingMessageHeaders
                        .GetHeader<string>("Authorization", Config.Namespace);
            }
            catch (MessageHeaderException)
            {
                return null;
            }
        }

        public bool isConnectionAuthorized()
        {
            string token_string = this.getCurrentToken();

            try
            {
                return garageService.existsGarageWithToken(token_string)
                    && tokenService.isValid(token_string);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}