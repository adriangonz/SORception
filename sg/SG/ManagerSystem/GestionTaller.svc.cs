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
        private Taller getAuthorizedTaller()
        {
            string token_string = OperationContext.Current.IncomingMessageHeaders
                .GetHeader<string>("Authorization", Constants.Namespace);

            Token token = TokenRepository.Find(token_string);
            if (token != null && token.is_valid && token.Taller != null)
            {
                return token.Taller;
            }
            else
            {
                throw new WebFaultException(System.Net.HttpStatusCode.Forbidden);
            }
        }

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

        public int putTaller(ExposedTaller et)
        {
            Taller t = getAuthorizedTaller();

            if (et != null)
            {
                t = TallerRepository.FromExposed(et);
                TallerRepository.InsertOrUpdate(t);
                TallerRepository.Save();
            }

            return 0;
        }

        public int deleteTaller()
        {
            Taller t = getAuthorizedTaller();
            TallerRepository.Delete(t.Id);
            TallerRepository.Save();
            return 0;
        }

        public ExposedSolicitud getSolicitud(int id)
        {
            Taller t = getAuthorizedTaller();

            var tmp = SolicitudRepository.Find(id);
            ExposedSolicitud s = SolicitudRepository.PrepareOutgoing(tmp);
            return s;
        }

        public List<ExposedSolicitud> getSolicitudes()
        {
            Taller t = getAuthorizedTaller();

            List<ExposedSolicitud> solicitudes = new List<ExposedSolicitud>();

            foreach (var solicitud in SolicitudRepository.FindAll())
            {
                solicitudes.Add(SolicitudRepository.PrepareOutgoing(solicitud));
            }

            return solicitudes;
        }

        public int addSolicitud(ExposedSolicitud es)
        {
            Taller t = getAuthorizedTaller();

            if (es != null)
            {
                Solicitud s = SolicitudRepository.GetIncoming(es);
                SolicitudRepository.InsertOrUpdate(s);
                SolicitudRepository.Save();
                SendMessage(new AMQSolicitudMessage(SolicitudRepository.PrepareOutgoing(s), AMQSolicitudMessage.Code.New));
                return s.Id;
            }
            return -1;
        }

        public int putSolicitud(ExposedSolicitud es)
        {
            Taller t = getAuthorizedTaller();

            if (es != null)
            {
                Solicitud s = SolicitudRepository.Find(es.id);
                SolicitudRepository.UpdateFromExposed(s, es);
                SolicitudRepository.Save();
                SendMessage(new AMQSolicitudMessage(SolicitudRepository.PrepareOutgoing(s), AMQSolicitudMessage.Code.Update));
            }
            return 0;
        }

        public int deleteSolicitud(int id)
        {
            Taller t = getAuthorizedTaller();

            SolicitudRepository.Delete(id);
            SolicitudRepository.Save();
            ExposedSolicitud es = new ExposedSolicitud();
            es.id = id;
            SendMessage(new AMQSolicitudMessage(es, AMQSolicitudMessage.Code.Delete));
            return 0;
        }

        public List<ExposedOferta> getOfertas(int solicitud_id)
        {
            Taller t = getAuthorizedTaller();

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
            Taller t = getAuthorizedTaller();

            TopicPublisher publisher = TopicPublisher.MakePublisher(
                    Constants.ActiveMQ.Broker, 
                    Constants.ActiveMQ.Solicitud.Client_ID, 
                    Constants.ActiveMQ.Solicitud.Topic);
            publisher.SendMessage(sm);
            publisher.Dispose();
        }
    }
}
