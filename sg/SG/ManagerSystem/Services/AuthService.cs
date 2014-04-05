using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
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

        public void authenticateCall()
        {
            if (!tokenService.isValid(this.getToken()))
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public string getToken()
        {
            return "f266a5de85f2ad36f5a5fe7ef583db957066429558d2215efae70764e07de5d3";
            try
            {
                return OperationContext.Current.IncomingMessageHeaders
                            .GetHeader<string>("Authorization", Config.Namespace);
            }
            catch (MessageHeaderException)
            {
                return this.current_junkyard_token;
            }
        }

        public void setJunkyardToken(string token)
        {
            current_junkyard_token = token;
        }

        public GarageEntity currentGarage()
        {
            string token = this.getToken();
            return garageService.getGarageWithToken(token);
        }

        public JunkyardEntity currentJunkyard()
        {
            string token = this.getToken();
            return junkyardService.getJunkyardWithToken(token);
        }

        public void restrictAccess(GarageEntity garage = null, JunkyardEntity junkyard = null)
        {
            if (this.currentGarage() != garage && this.currentJunkyard() != junkyard)
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}