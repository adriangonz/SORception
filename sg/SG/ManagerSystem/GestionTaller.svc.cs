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
            if (!authorizationService.isConnectionAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            garageService.putGarage(et);

            return 0;
        }

        public int deleteTaller()
        {
            if (!authorizationService.isConnectionAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            garageService.deleteCurrentGarage();

            return 0;
        }

        public ExpSolicitud getSolicitud(int id)
        {
            if (!authorizationService.isConnectionAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            try
            {
                return orderService.getOrder(id);
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
            if (!authorizationService.isConnectionAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            return orderService.getOrders();
        }

        public int addSolicitud(ExpSolicitud es)
        {
            if (!authorizationService.isConnectionAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            return orderService.addOrder(es);
            /*
            if (es == null)
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            Taller t = getAuthorizedTaller();

            Solicitud s = r_solicitud.GetIncoming(es);
            s.TallerId = t.Id;
            r_solicitud.InsertOrUpdate(s);
            r_solicitud.Save();
            ScheduleJob(s);

            SendMessage(new AMQSolicitudMessage(r_solicitud.PrepareOutgoing(s), AMQSolicitudMessage.Code.New));
            return s.Id;*/
        }

        public int putSolicitud(ExpSolicitud es)
        {
            if (!authorizationService.isConnectionAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            orderService.putOrder(es);

            return 0;

            /*
            if (es == null)
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            Taller t = getAuthorizedTaller();

            Solicitud s = r_solicitud.Find(es.id);
            if (s.deleted)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);

            r_solicitud.UpdateFromExposed(s, es);
            r_solicitud.Save();
            SendMessage(new AMQSolicitudMessage(r_solicitud.PrepareOutgoing(s), AMQSolicitudMessage.Code.Update));

            return 0;*/
        }

        public int deleteSolicitud(int order_id)
        {
            if (!authorizationService.isConnectionAuthorized())
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);

            orderService.deleteOrder(order_id);

            return 0;

            /*
            Taller t = getAuthorizedTaller();

            r_solicitud.Delete(id);
            r_solicitud.Save();
            ExpSolicitud es = new ExpSolicitud();
            es.id = id;
            SendMessage(new AMQSolicitudMessage(es, AMQSolicitudMessage.Code.Delete));
            return 0;*/
        }

        public ExpOferta getOferta(int oferta_id)
        {
            Taller t = getAuthorizedTaller();

            Oferta o = r_oferta.Find(oferta_id);
            if (o == null || o.deleted)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);

            ExpOferta eo = r_oferta.ToExposed(o);
            return eo;
        }

        public List<ExpOferta> getOfertas(int solicitud_id)
        {
            Taller t = getAuthorizedTaller();

            Solicitud s = db_context.SolicitudSet.Find(solicitud_id);
            if (s == null || s.deleted)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);

            List<Oferta> ofertas_mias = s.Ofertas.ToList();
            List<ExpOferta> ofertas = new List<ExpOferta>();
            foreach (var oferta in ofertas_mias)
            {
                if (!oferta.deleted)
                {
                    ofertas.Add(r_oferta.ToExposed(oferta));
                }
            }

            return ofertas;
        }

        public int selectOferta(ExpPedido r)
        {
            Taller t = getAuthorizedTaller();

            Oferta o = r_oferta.Find(r.oferta_id);
            if (o.deleted)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);

            ExpPedido amq_pedido = new ExpPedido();
            amq_pedido.oferta_id = o.id_en_desguace;
            amq_pedido.lineas = new List<ExpPedido.Line>();

            foreach (var l in r.lineas)
            {
                LineaOferta lo = db_context.LineaOfertaSet.Find(l.linea_oferta_id);
                if (lo.LineaOfertaSeleccionada != null)
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
                /*if (!o.LineasOferta.Contains(lo))
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);*/
                    
                LineaOfertaSeleccionada los = new LineaOfertaSeleccionada();
                los.LineaOferta = lo;
                lo.status = "SELECTED";
                los.quantity = l.quantity;
                db_context.LineaOfertaSeleccionadaSet.Add(los);
                lo.LineaSolicitud.status = l.quantity >= lo.LineaSolicitud.quantity ? "COMPLETE" : "SELECTED";

                ExpPedido.Line lp = new ExpPedido.Line();
                lp.linea_oferta_id = lo.id_en_desguace;
                lp.quantity = l.quantity;
                amq_pedido.lineas.Add(lp);
            }
            db_context.SaveChanges();

            AMQPedidoMessage message = new AMQPedidoMessage();
            message.desguace_id = o.Desguace.Tokens.First(token => token.is_valid == true).token;
            message.pedido = amq_pedido;
            SendMessage(message);

            return 0;
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
            publisher.SendMessage(job, (long) delay.TotalMilliseconds);
            publisher.Dispose();
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
            db_context.SaveChanges();

            ExpPedido pedido = new ExpPedido();
            pedido.oferta_id = o.Id;
            pedido.lineas = new List<ExpPedido.Line>();
            foreach (var linea in pedidas_ahora)
            {
                ExpPedido.Line linea_ped = new ExpPedido.Line();
                linea_ped.quantity = linea.quantity;
                linea_ped.linea_oferta_id = linea.LineaOferta.Id;
                pedido.lineas.Add(linea_ped);
            }

            AMQPedidoMessage message = new AMQPedidoMessage();
            message.pedido = pedido;
            message.desguace_id = db_context.TokenSet.First(t => t.is_valid && t.DesguaceId == o.DesguaceId).token;
            SendMessage(message);
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
                    if (l_sol.status == "COMPLETE")
                        continue;

                    // Sort the offers
                    List<LineaOferta> ofertas_ordenadas;
                    switch (l_sol.flag)
                    {
                        case "CHEAPEST":
                            ofertas_ordenadas = l_sol.LineasOferta.OrderBy(lo => lo.price).ToList();
                            break;
                        case "NEWEST":
                            //ofertas_ordenadas = l_sol.LineasOferta.OrderBy(lo => lo.date).ToList();
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

                // For each offer with select lines
                foreach (var entry in pedidas_ahora)
                {
                    // Send a message to the Offer AMQTopic
                    ExpPedido pedido = new ExpPedido();
                    pedido.oferta_id = entry.Key;
                    pedido.lineas = new List<ExpPedido.Line>();
                    foreach (var linea in entry.Value)
                    {
                        ExpPedido.Line linea_ped = new ExpPedido.Line();
                        linea_ped.quantity = linea.quantity;
                        linea_ped.linea_oferta_id = linea.LineaOferta.Id;
                        pedido.lineas.Add(linea_ped);
                    }

                    AMQPedidoMessage message = new AMQPedidoMessage();
                    message.pedido = pedido;
                    message.desguace_id = db_context.OfertaSet.Find(entry.Key).Desguace.Tokens.First(t => t.is_valid).token;
                    SendMessage(message);
                }
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
    }
}
