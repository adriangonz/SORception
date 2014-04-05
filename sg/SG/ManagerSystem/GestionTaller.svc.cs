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
<<<<<<< HEAD
            if (es == null)
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            Taller t = getAuthorizedTaller();

            Solicitud s = r_solicitud.Find(es.id);
            if (s == null || s.deleted)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
=======
            if (!authorizationService.isGarageAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
>>>>>>> sg_code_first

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
<<<<<<< HEAD
            Taller t = getAuthorizedTaller();

            Dictionary<int, List<LineaOfertaSeleccionada>> pedidas_ahora = new Dictionary<int, List<LineaOfertaSeleccionada>>();

            foreach (var l in r.lineas)
            {
                LineaOferta lo = db_context.LineaOfertaSet.Find(l.linea_oferta_id);
                if (lo.LineaOfertaSeleccionada != null)
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
                    
                LineaOfertaSeleccionada los = new LineaOfertaSeleccionada();
                los.LineaOferta = lo;
                lo.status = "SELECTED";
                los.quantity = l.quantity;
                db_context.LineaOfertaSeleccionadaSet.Add(los);
                lo.LineaSolicitud.status = l.quantity >= lo.LineaSolicitud.quantity ? "COMPLETE" : "SELECTED";

                if (!pedidas_ahora.ContainsKey(lo.OfertaId))
                    pedidas_ahora[lo.OfertaId] = new List<LineaOfertaSeleccionada>();
                pedidas_ahora[lo.OfertaId].Add(los);
            }
            db_context.SaveChanges();

            foreach (var entry in pedidas_ahora)
            {
                SendPedido(entry.Key, entry.Value);
            }

            return 0;
        }

        private void SendPedido(int oferta_id, List<LineaOfertaSeleccionada> lineas)
        {
            if (lineas.Count > 0)
            {
                // Send a message to the Offer AMQTopic
                ExpPedido pedido = new ExpPedido();
                pedido.oferta_id = r_oferta.Find(oferta_id).id_en_desguace;
                pedido.lineas = new List<ExpPedido.Line>();
                foreach (var linea in lineas)
                {
                    ExpPedido.Line linea_ped = new ExpPedido.Line();
                    linea_ped.quantity = linea.quantity;
                    linea_ped.linea_oferta_id = linea.LineaOferta.id_en_desguace;
                    pedido.lineas.Add(linea_ped);
                }

                AMQPedidoMessage message = new AMQPedidoMessage();
                message.pedido = pedido;
                message.desguace_id = db_context.OfertaSet.Find(oferta_id).Desguace.Tokens.First(t => t.is_valid).token;
                SendMessage(message);
            }
         }

        public void checkAutoBuy(Oferta o)
        {
            if (o == null)
                return;

            List<LineaOfertaSeleccionada> pedidas_ahora = new List<LineaOfertaSeleccionada>();
            foreach (var linea in o.LineasOferta)
            {
                if (linea.LineaSolicitud.flag == "FIRST" && linea.LineaSolicitud.status != "COMPLETE")
                {
                    LineaOfertaSeleccionada selec = new LineaOfertaSeleccionada();

                    int ammount_left = linea.LineaSolicitud.quantity;
                    List<LineaOfertaSeleccionada> ya_hechas = db_context.LineaOfertaSeleccionadaSet.Where(l => l.LineaOferta.LineaSolicitudId == linea.LineaSolicitud.Id).ToList();
                    foreach (LineaOfertaSeleccionada l in ya_hechas)
                    {
                        ammount_left -= l.quantity;
                    }
                    if (ammount_left <= 0)
                        continue;

                    if (linea.quantity < ammount_left)
                    {
                        selec.quantity = linea.quantity;
                        linea.LineaSolicitud.status = "INCOMPLETE";
                    }
                    else
                    {
                        selec.quantity = ammount_left;
                        linea.LineaSolicitud.status = "COMPLETE";
                    }
                    linea.status = "SELECTED";
                    
                    selec.LineaOferta = linea;
                    pedidas_ahora.Add(selec);
                    db_context.LineaOfertaSeleccionadaSet.Add(selec);
                }
            }

            if (pedidas_ahora.Count > 0)
            {
                db_context.SaveChanges();
                SendPedido(o.Id, pedidas_ahora);
            }
        }

        public void runJob(AMQScheduledJob job)
        {
            if (ValidateJob(job))
            {
                Logger.Info(String.Format("Running job for {0}", job.id_solicitud));

                // Iterate over the offers for each line and select one according to the criteria of that line
                Solicitud s = db_context.SolicitudSet.Find(job.id_solicitud);

                Dictionary<int, List<LineaOfertaSeleccionada>> pedidas_ahora = new Dictionary<int, List<LineaOfertaSeleccionada>>();
                // Iterate over the lines
                foreach (LineaSolicitud l_sol in s.LineasSolicitud)
                {
                    if (l_sol.status == "COMPLETE" || l_sol.status == "DELETED")
                        continue;

                    // Sort the offers
                    List<LineaOferta> ofertas_ordenadas;
                    switch (l_sol.flag)
                    {
                        case "CHEAPEST":
                            ofertas_ordenadas = l_sol.LineasOferta.OrderBy(lo => lo.price).ToList();
                            break;
                        case "NEWEST":
                            ofertas_ordenadas = l_sol.LineasOferta.OrderByDescending(lo => lo.date).ToList();
                            break;
                        case "NONE":
                            continue;
                    }

                    int ammount_left = l_sol.quantity;
                    List<LineaOfertaSeleccionada> ya_hechas = db_context.LineaOfertaSeleccionadaSet.Where(l => l.LineaOferta.LineaSolicitudId == l_sol.Id).ToList();
                    foreach (LineaOfertaSeleccionada l in ya_hechas)
                    {
                        ammount_left -= l.quantity;
                    }

                    // Get offers until you have enough pieces
                    List<LineaOferta> ofertas = db_context.LineaOfertaSet.Where(l => l.LineaSolicitudId == l_sol.Id).ToList();
                    foreach (LineaOferta lo in ofertas)
                    {
                        if (ammount_left <= 0)
                            break;

                        LineaOfertaSeleccionada selec = new LineaOfertaSeleccionada();
                        selec.LineaOferta = lo;
                        if (lo.quantity < ammount_left)
                        {
                            selec.quantity = lo.quantity;
                            ammount_left -= lo.quantity;
                        }
                        else
                        {
                            selec.quantity = ammount_left;
                            ammount_left = 0;
                        }
                        if (!pedidas_ahora.ContainsKey(lo.OfertaId))
                            pedidas_ahora[lo.OfertaId] = new List<LineaOfertaSeleccionada>();
                        pedidas_ahora[lo.OfertaId].Add(selec);
                        lo.status = "SELECTED";

                        db_context.LineaOfertaSeleccionadaSet.Add(selec);
                    }

                    if (ammount_left <= 0)
                    {
                        l_sol.status = "COMPLETE";
                    }
                    else
                    {
                        l_sol.status = "INCOMPLETE";
                    }
                }
                db_context.SaveChanges();

                foreach (var entry in pedidas_ahora)
                {
                    SendPedido(entry.Key, entry.Value);
                }

                // Avisar a todos de que se cierra la solicitud
                AMQSolicitudMessage close_msg = new AMQSolicitudMessage();
                close_msg.code = AMQSolicitudMessage.Code.Closed;
                close_msg.solicitud = new ExpSolicitud
                {
                    id = s.Id
                };
                SendMessage(close_msg);
            }
        }

        private string GenerateCSRF(Solicitud s)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(s.date.ToString());
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        private bool ValidateJob(AMQScheduledJob job)
        {
            Solicitud s = db_context.SolicitudSet.Find(job.id_solicitud);
            if (s == null) {
                Logger.Error(String.Format("Job with invalid id_solicitud: {0}", job.id_solicitud));
                return false;
            }

            if (s.deadline != job.deadline) {
                Logger.Error(String.Format("Job for {0} is outdated: {1} != {2}", job.id_solicitud, job.deadline, s.deadline));
                return false;
            }

            if (job.csrf != GenerateCSRF(s)) {
                Logger.Error(String.Format("CSRF of job for {0} doesn't match", job.id_solicitud));
                return false;
            }

            return true;
        }

        private void SendMessage(AMQSolicitudMessage sm)
        {
            Taller t = getAuthorizedTaller();

            TopicPublisher publisher = TopicPublisher.MakePublisher(
                    Constants.ActiveMQ.Broker,
                    Constants.ActiveMQ.Client_ID,
                    Constants.ActiveMQ.Solicitudes_Topic);
            publisher.SendMessage(sm);
            publisher.Dispose();
        }

        private void SendMessage(AMQPedidoMessage sm)
        {
            Taller t = getAuthorizedTaller();

            TopicPublisher publisher = TopicPublisher.MakePublisher(
                    Constants.ActiveMQ.Broker,
                    Constants.ActiveMQ.Client_ID,
                    Constants.ActiveMQ.Pedidos_Topic);
            publisher.SendMessage(sm);
            publisher.Dispose();
        }

        private void ScheduleJob(Solicitud s)
        {
            AMQScheduledJob job = new AMQScheduledJob();
            job.deadline = s.deadline;
            job.id_solicitud = s.Id;
            job.csrf = GenerateCSRF(s);

            TopicPublisher publisher = TopicPublisher.MakePublisher(
                    Constants.ActiveMQ.Broker,
                    Constants.ActiveMQ.Client_ID,
                    Constants.ActiveMQ.Jobs_Topic);

            TimeSpan delay = s.deadline - DateTime.Now;
            publisher.SendMessage(job, (long)delay.TotalMilliseconds);
            publisher.Dispose();
        }
=======
            purchaseService.selectOffer(r);

            return 0;
        }
>>>>>>> sg_code_first
    }
}
