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
            try
            {
                return OperationContext.Current.IncomingMessageHeaders
                        .GetHeader<string>("Authorization", Constants.Namespace);
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