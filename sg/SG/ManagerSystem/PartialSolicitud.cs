using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace ManagerSystem
{
    public partial class Solicitud
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        static private Solicitud Copy(Solicitud tmp)
        {
            Solicitud s = new Solicitud();

            return s;
        }

        static public Solicitud Find(int id)
        {
            return ms_ent.SolicitudSet.Find(id);
        }

        static public Solicitud Sanitize(Solicitud s)
        {
            return Copy(s);
        }

        static public ExpSolicitud PrepareOutgoing(Solicitud s)
        {
            ExpSolicitud es = new ExpSolicitud();
            es.id = s.Id;
            es.id_en_taller = s.id_en_taller;
            es.status = s.status;
            es.deadline = s.deadline;
            es.lineas = new List<ExpSolicitud.Line>();
            foreach (var l in s.LineasSolicitud)
            {
                ExpSolicitud.Line el = new ExpSolicitud.Line();
                el.id = l.Id;
                el.id_en_taller = l.id_en_taller;
                el.description = l.description;
                el.quantity = l.quantity;
                el.status = l.status;
                el.flag = l.Flag.type;
                
                es.lineas.Add(el);
            }

            return es;
        }

        static public Solicitud GetIncoming(ExpSolicitud es)
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

        static private void UpdateLineaFromExposed(LineaSolicitud ls, ExpSolicitud.Line el)
        {
            ls.id_en_taller = el.id_en_taller;
            ls.quantity = el.quantity;
            ls.description = el.description;
            ls.Flag.type = el.flag;
        }

        static public void UpdateFromExposed(Solicitud s, ExpSolicitud es)
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
                        ls = ms_ent.LineasSolicitudSet.Find(el.id);
                        UpdateLineaFromExposed(ls, el);
                        ls.status = "UPDATED";
                        ls.description = el.description;
                        break;
                    case "DELETED":
                        ls = ms_ent.LineasSolicitudSet.Find(el.id);
                        s.LineasSolicitud.Remove(ls);
                        break;
                }
            }
        }

        static public List<Solicitud> FindAll()
        {
            List<Solicitud> l = new List<Solicitud>();

            var lq_l = from d in ms_ent.SolicitudSet select d;
            foreach (var singleDesguace in lq_l)
            {
                Solicitud s = Copy(singleDesguace);
                l.Add(s);
            }
            return l;
        }

        static public void InsertOrUpdate(Solicitud s)
        {
            if (s == null) return;

            if (s.Id == default(int))
            {
                // New entity
                ms_ent.SolicitudSet.Add(s);
            }
            else
            {
                // Existing entity

            }
        }

        static public void Delete(int id)
        {
            Solicitud s = ms_ent.SolicitudSet.Find(id);
            if (s == null)
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
            s.deleted = true;
        }

        static public void Save()
        {
            ms_ent.SaveChanges();
        }

        static public void Dispose()
        {
            ms_ent.Dispose();
        }
    }
}