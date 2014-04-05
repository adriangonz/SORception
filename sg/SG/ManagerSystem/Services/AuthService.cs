using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace ManagerSystem.Services
{
    public class AuthService : BaseService
    {
        public AuthService(UnitOfWork uow = null) : base(uow) { }

        private string current_junkyard_token = null;

        public string getCurrentGarageToken()
        {
            #if DEBUG
            return "XCVS5SB2qkuvXTN/u0T3rw";
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
            return "SDrMmE5BzEiq3fJeTCUwSw";
            #endif


            if (this.current_junkyard_token == null)
                throw new ArgumentNullException();

            return current_junkyard_token;
        }

        public bool isGarageAuthenticated()
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

        public bool isJunkyardAuthenticated()
        {
            string token_string = this.getCurrentJunkyardToken();
            try
            {
                bool junkyard_exists = junkyardService.existsJunkyardWithToken(token_string);
                bool token_is_valid = tokenService.isValid(token_string);
                return junkyard_exists && token_is_valid;
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

        public void forbidAccess(int garage_id, int junkyard_id)
        {
            if (!garageService.garageHasAccess(garage_id) &&
                !junkyardService.junkyardHasAccess(junkyard_id))
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public void forbidGarageAccess(int garage_id)
        {
            if (!garageService.garageHasAccess(garage_id))
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public void forbidJunkyardAccess(int junkyard_id)
        {
            if (!junkyardService.junkyardHasAccess(junkyard_id))
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}