using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using ManagerSystem.Services;

namespace ManagerSystem
{
    [ServiceBehavior(Namespace = Constants.Namespace)]
    public class GestionDesguace : IGestionDesguace
    {
        private JunkyardService junkyard_service = null;
        private JunkyardService junkyardService
        {
            get
            {
                if (this.junkyard_service == null)
                    this.junkyard_service = new JunkyardService();
                return this.junkyard_service;
            }
        }

        private TokenService token_service = null;
        private TokenService tokenService
        {
            get
            {
                if (this.token_service == null)
                    this.token_service = new TokenService();
                return this.token_service;
            }
        }

        public TokenResponse signUp(ExpDesguace ed)
        {
            return junkyardService.createJunkyard(ed);
        }

        public TokenResponse getState(string token_string)
        {
            return tokenService.validateToken(token_string);
        }

        public void dummy(AMQSolicitudMessage s, AMQOfertaMessage o, AMQPedidoMessage p) { }
    }
}
