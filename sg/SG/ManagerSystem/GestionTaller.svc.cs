using ActiveMQHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    [DataContract]
    public class AMQSolicitudMessage
    {
        public enum Code { New, Update, Delete };

        [DataMember]
        public Code code;

        [DataMember]
        public ExposedSolicitud solicitud = null;

        [DataMember]
        public int solicitud_id = -1;

        public AMQSolicitudMessage(ExposedSolicitud s, Code c)
        {
            code = c;
            solicitud = s;
        }

        public AMQSolicitudMessage(int id, Code c)
        {
            code = c;
            solicitud_id = id;
        }
        
    }

    [DataContract]
    public class ExposedLineaSolicitud 
    {
        [DataMember]
        public int id;
        
        [DataMember]
        public string description;

        [DataMember]
        public int quantity;
    }

    [DataContract]
    public class ExposedSolicitud
    {
        [DataMember]
        public int id;

        [DataMember]
        public int taller_id;

        [DataMember]
        public List<ExposedLineaSolicitud> lineas;
    }

    [DataContract]
    public class ExposedTaller
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GestionTaller" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GestionTaller.svc or GestionTaller.svc.cs at the Solution Explorer and start debugging.
    public class GestionTaller : IGestionTaller
    {
        TopicPublisher _publisher = null;

        public ExposedTaller getTaller(int id)
        {
            var tmp = TallerRepository.Find(id);
            ExposedTaller t = null;
            if (tmp != null)
            {
                t = TallerRepository.ToExposed(tmp);
            }
            return t;
        }

        public int addTaller(string nombre)
        {
            if (nombre != "" && nombre != null)
            {
                try
                {
                    Taller tall = new Taller();
                    tall.name = nombre;
                    TallerRepository.InsertOrUpdate(tall);
                    TallerRepository.Save();
                    return 1;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return 0;
        }

        public int putTaller(ExposedTaller et)
        {
            if (et != null)
            {
                Taller t = TallerRepository.FromExposed(et);
                TallerRepository.InsertOrUpdate(t);
                TallerRepository.Save();
            }
            return 0;
        }

        public int deleteTaller(int id)
        {
            TallerRepository.Delete(id);
            return 0;
        }

        public ExposedSolicitud getSolicitud(int id)
        {
            var tmp = SolicitudRepository.Find(id);
            ExposedSolicitud s = SolicitudRepository.ToExposed(tmp);
            return s;
        }

        public int addSolicitud(ExposedSolicitud es)
        {
            if (es != null)
            {
                Solicitud s = SolicitudRepository.FromExposed(es);
                SolicitudRepository.InsertOrUpdate(s);
                SolicitudRepository.Save();
                SendMessage(new AMQSolicitudMessage(es, AMQSolicitudMessage.Code.New));
                return 0;
            }
            return 1;
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
            SendMessage(new AMQSolicitudMessage(id, AMQSolicitudMessage.Code.Delete));
            return 0;
        }

        public List<ExposedSolicitud> getSolicitudes()
        {
            List<ExposedSolicitud> l = new List<ExposedSolicitud>();
            foreach (var tmp in SolicitudRepository.FindAll())
            {
                l.Add(SolicitudRepository.ToExposed(tmp));
            }
            return l;
        }

        private void SendMessage(AMQSolicitudMessage sm)
        {
            if (_publisher == null)
                _publisher = TopicPublisher.MakePublisher(Constants.ActiveMQ.Broker, Constants.ActiveMQ.Solicitud.Client_ID, Constants.ActiveMQ.Solicitud.Topic);
            _publisher.SendMessage(sm);
        }
    }
}
