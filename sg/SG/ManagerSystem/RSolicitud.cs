using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace ManagerSystem
{
    public class RSolicitud
    {
        public managersystemEntities db_context;

        public RSolicitud()
        {
            db_context = new managersystemEntities();
        }

        public RSolicitud(managersystemEntities context)
        {
            db_context = context;
        }

        private Solicitud Copy(Solicitud tmp)
        {
            Solicitud s = new Solicitud();

            return s;
        }

        public Solicitud Find(int id)
        {
            return db_context.SolicitudSet.Find(id);
        }

        public Solicitud Sanitize(Solicitud s)
        {
            return Copy(s);
        }

        public ExpSolicitud PrepareOutgoing(Solicitud s)
        {
            ExpSolicitud es = new ExpSolicitud();
            es.id = s.Id;
            es.id_en_taller = s.id_en_taller;
            es.status = s.status;
            es.deadline = s.deadline;
            es.lineas = new List<ExpSolicitud.Line>();
            foreach (var l in s.LineasSolicitud)
            {
                if (l.status != "DELETED")
                {
                    ExpSolicitud.Line el = new ExpSolicitud.Line();
                    el.id = l.Id;
                    el.id_en_taller = l.id_en_taller;
                    el.description = l.description;
                    el.quantity = l.quantity;
                    el.status = l.status;
                    el.flag = l.flag;

                    es.lineas.Add(el);
                }
            }

            return es;
        }

        public Solicitud GetIncoming(ExpSolicitud es)
        {            
            Solicitud s = new Solicitud();
            s.id_en_taller = es.id_en_taller;
            s.status = "NEW";
            s.deadline = es.deadline;
            s.date = DateTime.Now;
            foreach (var el in es.lineas)
            {
                LineaSolicitud ls = new LineaSolicitud();
                UpdateLineaFromExposed(ls, el);
                ls.status = "NEW";
                s.LineasSolicitud.Add(ls);
            }

            return s;
        }

        private void UpdateLineaFromExposed(LineaSolicitud ls, ExpSolicitud.Line el)
        {
            ls.quantity = el.quantity;
            ls.description = el.description;
            ls.flag = el.flag;
        }

        public void UpdateFromExposed(Solicitud s, ExpSolicitud es)
        {
            s.deadline = es.deadline;
            foreach (var el in es.lineas)
            {
                LineaSolicitud ls = null;
                switch (el.action)
                {
                    case "NEW":
                        ls = new LineaSolicitud();
                        UpdateLineaFromExposed(ls, el);
                        ls.status = "NEW";
                        s.LineasSolicitud.Add(ls);
                        break;
                    case "UPDATED":
                        ls = db_context.LineasSolicitudSet.Find(el.id);
                        UpdateLineaFromExposed(ls, el);
                        ls.status = "UPDATED";
                        ls.description = el.description;
                        break;
                    case "DELETED":
                        ls = db_context.LineasSolicitudSet.Find(el.id);
                        ls.status = "DELETED";
                        break;
                }
            }
        }

        public List<Solicitud> FindAll()
        {
            List<Solicitud> l = new List<Solicitud>();

            var lq_l = from d in db_context.SolicitudSet select d;
            foreach (var singleDesguace in lq_l)
            {
                Solicitud s = Copy(singleDesguace);
                l.Add(s);
            }
            return l;
        }

        public void InsertOrUpdate(Solicitud s)
        {
            if (s == null) return;

            if (s.Id == default(int))
            {
                // New entity
                db_context.SolicitudSet.Add(s);
            }
            else
            {
                // Existing entity

            }
        }

        public void Delete(int id)
        {
            Solicitud s = db_context.SolicitudSet.Find(id);
            if (s == null)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
            s.deleted = true;
        }

        public void Save()
        {
            db_context.SaveChanges();
        }

        public void Dispose()
        {
            db_context.Dispose();
        }
    }
}