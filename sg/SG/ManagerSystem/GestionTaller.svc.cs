﻿using ActiveMQHelper;
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

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GestionTaller" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GestionTaller.svc or GestionTaller.svc.cs at the Solution Explorer and start debugging.
    public class GestionTaller : IGestionTaller
    {
        TopicPublisher _publisher = null;

        public Taller getTaller(string token)
        {
            int id = int.Parse(token);
            var tmp = TallerRepository.Find(id);
            Taller t = null;
            if (tmp != null)
            {
                t = TallerRepository.Sanitize(tmp);
            }
            return t;
        }

        public string addTaller(string nombre)
        {
            if (nombre != "" && nombre != null)
            {
                try
                {
                    Taller tall = new Taller();
                    tall.name = nombre;
                    TallerRepository.InsertOrUpdate(tall);
                    TallerRepository.Save();
                    return tall.Id.ToString();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return "";
        }

        public int getState(string token)
        {
            int id = int.Parse(token);
            try
            {
                var tmp = TallerRepository.Find(id);
                Taller t = null;
                if (tmp != null)
                {
                    t = TallerRepository.Sanitize(tmp);
                    if (t.active)
                        return t.Id;
                }
                
            }
            catch (Exception e)
            {

                throw;
            }
            return -1;
        }

        public int putTaller(Taller t)
        {
            if (t != null)
            {
                TallerRepository.InsertOrUpdate(t);
            }
            return 0;
        }

        public int deleteTaller(string token)
        {
            int id = int.Parse(token);
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
                _publisher = TopicPublisher.MakePublisher("tcp://MartinLaptop:61616", "GestionTaller", "Solicitudes");
            _publisher.SendMessage(sm);
        }
    }
}
