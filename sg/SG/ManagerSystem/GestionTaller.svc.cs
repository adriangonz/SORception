using ActiveMQHelper;
using ManagerSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace ManagerSystem
{
    public class GestionTaller : IGestionTaller
    {
        // Esto va fuera
        public static managersystemEntities db_context;
        private RTaller r_taller;
        private RSolicitud r_solicitud;
        private ROferta r_oferta;
        private RToken r_token;

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

        private GarageService garage_service = null;
        private GarageService garageService
        {
            get
            {
                if (this.garage_service == null)
                    this.garage_service = new GarageService();
                return this.garage_service;
            }
        }

        private AuthorizationService authorization_service = null;
        private AuthorizationService authorizationService
        {
            get
            {
                if (this.authorization_service == null)
                    this.authorization_service = new AuthorizationService();
                return this.authorization_service;
            }
        }

        private OrderService order_service = null;
        private OrderService orderService
        {
            get
            {
                if (this.order_service == null)
                    this.order_service = new OrderService();
                return this.order_service;
            }
        }

        private OfferService offer_service = null;
        protected OfferService offerService
        {
            get
            {
                if (this.offer_service == null)
                    this.offer_service = new OfferService();
                return this.offer_service;
            }
        }

        private PurchaseService purchase_service = null;
        protected PurchaseService purchaseService
        {
            get
            {
                if (this.purchase_service == null)
                    this.purchase_service = new PurchaseService();
                return this.purchase_service;
            }
        }

        // Esto va fuera
        private Taller getAuthorizedTaller()
        {
            return null;
        }

        public TokenResponse signUp(ExpTaller et)
        {
            return garageService.createGarage(et);
        }

        public TokenResponse getState(string token_string)
        {
            return tokenService.validateGarageToken(token_string);
        }

        public int putTaller(ExpTaller et)
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            garageService.putGarage(et);

            return 0;
        }

        public int deleteTaller()
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            garageService.deleteCurrentGarage();

            return 0;
        }

        public ExpSolicitud getSolicitud(int id)
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            try
            {
                return orderService.toExposed(orderService.getOrder(id));
            }
            catch (ArgumentNullException)
            {
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound); ;
            }
            catch (ArgumentException)
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden); ;
            }
        }

        public List<ExpSolicitud> getSolicitudes()
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            return (from order in orderService.getOrders() select orderService.toExposed(order)).ToList(); ;
        }

        public int addSolicitud(ExpSolicitud es)
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            return orderService.addOrder(es);
        }

        public int putSolicitud(ExpSolicitud es)
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            orderService.putOrder(es);

            return 0;
        }

        public int deleteSolicitud(int order_id)
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            orderService.deleteOrder(order_id);

            return 0;
        }

        public ExpOferta getOferta(int oferta_id)
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            return offerService.toExposed(offerService.getOffer(oferta_id));
        }

        public List<ExpOferta> getOfertas(int solicitud_id)
        {
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            return (from offer in offerService.getOffers(solicitud_id) select offerService.toExposed(offer)).ToList(); ;
        }

        public int selectOferta(ExpPedido r)
        {
            purchaseService.selectOffer(r);

            return 0;
        }
    }
}
