using ActiveMQHelper;
using ManagerSystem.Services;
using ManagerSystem.Dispatchers;
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
    [AuthInjector]
    [ServiceBehavior(Namespace = Config.Namespace)]
    public class GestionTaller : IGestionTaller
    {
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
            garageService.putGarage(et);

            return 0;
        }

        public int deleteTaller()
        {
            garageService.deleteCurrentGarage();

            return 0;
        }

        public ExpSolicitud getSolicitud(int id)
        {
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
            return (from order in orderService.getOrders() select orderService.toExposed(order)).ToList(); ;
        }

        public int addSolicitud(ExpSolicitud es)
        {
            return orderService.addOrder(es);
        }

        public int putSolicitud(ExpSolicitud es)
        {
            orderService.putOrder(es);

            return 0;
        }

        public int deleteSolicitud(int order_id)
        {
            orderService.deleteOrder(order_id);

            return 0;
        }

        public ExpOferta getOferta(int oferta_id)
        {
            return offerService.toExposed(offerService.getOffer(oferta_id));
        }

        public List<ExpOferta> getOfertas(int solicitud_id)
        {
            return (from offer in offerService.getOffers(solicitud_id) select offerService.toExposed(offer)).ToList(); ;
        }

        public int selectOferta(ExpPedido r)
        {
            purchaseService.selectOffer(r);

            return 0;
        }
    }
}
