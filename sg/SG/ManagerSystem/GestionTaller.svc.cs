using ActiveMQHelper;
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
        public static managersystemEntities db_context;
        private RTaller r_taller;
        private RSolicitud r_solicitud;
        private ROferta r_oferta;
        private RToken r_token;

        public GestionTaller()
        {
            init(new managersystemEntities());            
        }

        public GestionTaller(managersystemEntities context)
        {
            init(context);
        }

        private void init(managersystemEntities context)
        {
            db_context = context;
            r_taller = new RTaller(db_context);
            r_solicitud = new RSolicitud(db_context);
            r_oferta = new ROferta(db_context);
            r_token = new RToken(db_context);
        }

        private Taller getAuthorizedTaller()
        {
            string token_string;
            try
            {
                token_string = OperationContext.Current.IncomingMessageHeaders
                    .GetHeader<string>("Authorization", Constants.Namespace);
            }
            catch (Exception e)
            {
                token_string = "813f14e463abe904fa848fcd5bc4f2b3dfa6e61fea3192d7aaa6887677089dde";
                //throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }

            Token token = r_token.Find(token_string);
            if (token != null && token.is_valid && token.Taller != null)
            {
                return token.Taller;
            }
            else
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public TokenResponse signUp(ExpTaller et)
        {
            if (et != null)
            {
                Taller tall = r_taller.FromExposed(et);
                tall.active = false;

                Token t = r_token.getToken();
                tall.Tokens.Add(t);

                r_taller.InsertOrUpdate(tall);
                r_taller.Save();
                return new TokenResponse(t.token, TokenResponse.Code.ACCEPTED);
            }
            return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);
        }

        public TokenResponse getState(string token)
        {
            string new_token = "";
            TokenResponse.Code status;
            if (token != null && token != "")
            {
                Token t = r_token.Find(token);
                if (t != null)
                {
                    if (t.is_valid)
                    {
                        Taller tall = r_taller.Find(t.Taller.Id);
                        if (tall.active)
                        {
                            // El taller ya esta activo
                            status = TokenResponse.Code.CREATED;
                        }
                        else
                        {
                            // El taller no esta activo
                            status = TokenResponse.Code.NON_AUTHORITATIVE;
                        }
                        new_token = r_token.RegenerateToken(t);
                    }
                    else
                    {
                        // El token ha expirado
                        status = TokenResponse.Code.BAD_REQUEST;
                    }
                }
                else
                {
                    // El token no existe
                    status = TokenResponse.Code.NOT_FOUND;
                }
            }
            else
            {
                // No se le ha pasado un token
                status = TokenResponse.Code.BAD_REQUEST;
            }

            return new TokenResponse(new_token, status);
        }

        public int putTaller(ExpTaller et)
        {
            if (et == null)
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            Taller t = getAuthorizedTaller();

            if (et != null)
            {
                t = r_taller.FromExposed(et);
                r_taller.InsertOrUpdate(t);
                r_taller.Save();
            }

            return 0;
        }

        public int deleteTaller()
        {
            Taller t = getAuthorizedTaller();
            r_taller.Delete(t.Id);
            r_taller.Save();
            return 0;
        }

        public ExpSolicitud getSolicitud(int id)
        {
            Taller t = getAuthorizedTaller();

            Solicitud tmp = db_context.SolicitudSet.Find(id);
            if (tmp == null || tmp.deleted)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
            ExpSolicitud s = r_solicitud.PrepareOutgoing(tmp);
            return s;
        }

        public List<ExpSolicitud> getSolicitudes()
        {
            Taller t = getAuthorizedTaller();

            List<ExpSolicitud> solicitudes = new List<ExpSolicitud>();

            foreach (var solicitud in t.Solicitudes)
            {
                if (!solicitud.deleted)
                {
                    solicitudes.Add(r_solicitud.PrepareOutgoing(solicitud));
                }
            }

            return solicitudes;
        }

        public int addSolicitud(ExpSolicitud es)
        {
            if (es == null)
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            Taller t = getAuthorizedTaller();

            Solicitud s = r_solicitud.GetIncoming(es);
            s.TallerId = t.Id;
            r_solicitud.InsertOrUpdate(s);
            r_solicitud.Save();
            ScheduleJob(s);

            SendMessage(new AMQSolicitudMessage(r_solicitud.PrepareOutgoing(s), AMQSolicitudMessage.Code.New));
            return s.Id;
        }

        public int putSolicitud(ExpSolicitud es)
        {
            if (es == null)
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            Taller t = getAuthorizedTaller();

            Solicitud s = r_solicitud.Find(es.id);
            if (s == null || s.deleted)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);

            r_solicitud.UpdateFromExposed(s, es);
            r_solicitud.Save();
            SendMessage(new AMQSolicitudMessage(r_solicitud.PrepareOutgoing(s), AMQSolicitudMessage.Code.Update));

            return 0;
        }

        public int deleteSolicitud(int id)
        {
            Taller t = getAuthorizedTaller();

            r_solicitud.Delete(id);
            r_solicitud.Save();
            ExpSolicitud es = new ExpSolicitud();
            es.id = id;
            SendMessage(new AMQSolicitudMessage(es, AMQSolicitudMessage.Code.Delete));
            return 0;
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
    }
}
