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
using ManagerSystem.Dispatchers;

namespace ManagerSystem
{
    [AuthInjector]
    [ServiceBehavior(Namespace = Config.Namespace)]
    public class GestionDesguace : IGestionDesguace
    {
        private ConfigService config_service = null;
        protected ConfigService configService
        {
            get
            {
                if (this.config_service == null)
                    this.config_service = new ConfigService();
                return this.config_service;
            }
        }

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

        private AMQService amq_service = null;
        private AMQService amqService
        {
            get
            {
                if (this.amq_service == null)
                    this.amq_service = new AMQService();
                return this.amq_service;
            }
        }

        public TokenResponse signUp(ExpDesguace ed)
        {
            return junkyardService.createJunkyard(ed);
        }

        public TokenResponse getState(string token_string)
        {
            TokenResponse response = tokenService.validateJunkyardToken(token_string);
            if (response.status == TokenResponse.Code.CREATED)
            {
                response = configService.addAESPair(response);
            }
            return response;
        }

        public void dummy(AMQSolicitudMessage s, AMQOfertaMessage o, AMQPedidoMessage p) 
        {
            o = new AMQOfertaMessage
            {
                code = AMQOfertaMessage.Code.New,
                desguace_id = "SDrMmE5BzEiq3fJeTCUwSw",
                oferta = new ExpOferta
                {
                    id_en_desguace = 200,
                    solicitud_id = 4,
                    lineas = new List<ExpOferta.Line>()
                }
            };
            o.oferta.lineas.Add(new ExpOferta.Line
            {
                id_en_desguace = 201,
                linea_solicitud_id = 1,
                notes = "una patata",
                price = 1,
                quantity = 5
            });
            
            amqService.processOfferMessage(o);
        }
    }
}
