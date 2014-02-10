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

        private string current_junkyard_token = null;

        public string getCurrentGarageToken()
        {
            #if DEBUG
            return "kSN5lFhNYEKRs01SZmqSg";
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

        public string getCurrentJunkyardToken()
        {
            #if DEBUG
            return "8urBZfCziU2f4V248M/t2w";
            #endif


            if (this.current_junkyard_token == null)
                throw new ArgumentNullException();

            return current_junkyard_token;
        }

        public bool isGarageAuthorized()
        {
            string token_string = this.getCurrentGarageToken();

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

        public bool isJunkyardAuthorized()
        {
            string token_string = this.getCurrentJunkyardToken();

            try
            {
                return junkyardService.existsJunkyardWithToken(token_string)
                    && tokenService.isValid(token_string);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public void setJunkyardToken(string token)
        {
            current_junkyard_token = token;
        }
    }
}