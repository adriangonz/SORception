﻿using System;
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
        private managersystemEntities db_context;
        private RDesguace r_desguace;
        private RToken r_token;
        private ROferta r_oferta;

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
            return tokenService.validateToken(token_string);
        }

        public void dummy(AMQSolicitudMessage s, AMQOfertaMessage o, AMQPedidoMessage p) {
            /*
            o = new AMQOfertaMessage();

            o.desguace_id = "d1fb28d93a179cf2efeb146cc09099b02bbcbabc625bf7f69b5be1722ecf443d";
            o.oferta = new ExpOferta();
            o.oferta.id_en_desguace = 123;
            o.oferta.solicitud_id = 19;
            o.oferta.lineas = new List<ExpOferta.Line>();
            ExpOferta.Line linea = new ExpOferta.Line();
            linea.id_en_desguace = 123;
            linea.linea_solicitud_id = 10;
            linea.price = 123;
            linea.quantity = 100;
            linea.notes = "";
            o.oferta.lineas.Add(linea);
            
            Oferta of = processAMQMessage(o);
            GestionTaller gt = new GestionTaller(db_context);
            gt.checkAutoBuy(of);*/
        }

        public Oferta processAMQMessage(AMQOfertaMessage message)
        {
            amqService.processIncommingOffer(message);
        }
    }
}
