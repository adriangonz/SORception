using ActiveMQHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace ManagerSystem
{
    public class GestionTaller : IGestionTaller
    {
        public TokenResponse signUp(ExposedTaller et)
        {
            if (et != null)
            {
                Taller tall = TallerRepository.FromExposed(et);
                tall.active = false;

                Token t = TokenRepository.getToken();
                tall.Tokens.Add(t);

                TallerRepository.InsertOrUpdate(tall);
                TallerRepository.Save();
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
                Token t = TokenRepository.Find(token);
                if (t != null)
                {
                    if (t.is_valid)
                    {
                        Taller tall = TallerRepository.Find(t.Taller.Id);
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
                        new_token = TokenRepository.RegenerateToken(t);
                    }
                    else
                    {
                        // El taller ha expirado
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

        /*public ExposedTaller getTaller(int id)
        {
            var tmp = TallerRepository.Find(id);
            ExposedTaller t = null;
            if (tmp != null)
            {
                t = TallerRepository.ToExposed(tmp);
            }
            return t;
        }*/

        public int putTaller(ExposedTaller et)
        {
            var auth = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

            if (et != null && auth != null)
            {
                Taller t = TallerRepository.FromExposed(et);
                TallerRepository.InsertOrUpdate(t);
                TallerRepository.Save();
            }

            MessageHeader header = MessageHeader.CreateHeader("Authorization", Constants.Namespace, "touken");
            OperationContext.Current.OutgoingMessageHeaders.Add(header);

            return 0;
        }

        public int deleteTaller(string token)
        {
            int id = int.Parse(token);
            TallerRepository.Delete(id);
            return 0;
        }

        /*public ExposedSolicitud getSolicitud(int id)
        {
            var tmp = SolicitudRepository.Find(id);
            ExposedSolicitud s = SolicitudRepository.ToExposed(tmp);
            return s;
        }*/

        public int addSolicitud(ExposedSolicitud es)
        {
            if (es != null)
            {
                Solicitud s = SolicitudRepository.FromExposed(es);
                SolicitudRepository.InsertOrUpdate(s);
                SolicitudRepository.Save();
                SendMessage(new AMQSolicitudMessage(es, AMQSolicitudMessage.Code.New));
                return s.Id;
            }
            return -1;
        }

        public int putSolicitud(ExposedSolicitud es)
        {
            if (es != null)
            {
                Solicitud s = SolicitudRepository.FromExposed(es);
                SolicitudRepository.InsertOrUpdate(s);
                SendMessage(new AMQSolicitudMessage(es, AMQSolicitudMessage.Code.Update));
            }
            return 0;
        }

        public int deleteSolicitud(int id)
        {
            SolicitudRepository.Delete(id);
            ExposedSolicitud es = new ExposedSolicitud();
            es.id = id;
            SendMessage(new AMQSolicitudMessage(es, AMQSolicitudMessage.Code.Delete));
            return 0;
        }

        /*public List<ExposedSolicitud> getSolicitudes()
        {
            List<ExposedSolicitud> l = new List<ExposedSolicitud>();
            foreach (var tmp in SolicitudRepository.FindAll())
            {
                l.Add(SolicitudRepository.ToExposed(tmp));
            }
            return l;
        }*/

        public List<ExposedOferta> getOfertas(int solicitud_id)
        {
            List<ExposedOferta> ofertas = new List<ExposedOferta>();

            Solicitud s = SolicitudRepository.Find(solicitud_id);

            if (s != null)
            {
                foreach (var oferta in s.Ofertas)
                {
                    ofertas.Add(OfertaRepository.ToExposed(oferta));
                }
            }

            return ofertas;
        }

        public int selectOferta(TallerResponse r) { return 1; }

        private void SendMessage(AMQSolicitudMessage sm)
        {
            TopicPublisher publisher = TopicPublisher.MakePublisher(
                    Constants.ActiveMQ.Broker, 
                    Constants.ActiveMQ.Solicitud.Client_ID, 
                    Constants.ActiveMQ.Solicitud.Topic);
            publisher.SendMessage(sm);
            publisher.Dispose();
        }
    }
}
