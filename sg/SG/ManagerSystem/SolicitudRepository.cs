using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class SolicitudRepository
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        static private Solicitud Copy(Solicitud tmp)
        {
            Solicitud s = new Solicitud();

            return s;
        }

        static public Solicitud Find(int id)
        {
            return ms_ent.Solicituds.Find(id);
        }

        static public Solicitud Sanitize(Solicitud s)
        {
            return Copy(s);
        }

        static public ExposedSolicitud ToExposed(Solicitud s) {
            ExposedSolicitud es = new ExposedSolicitud();

            es.id = s.id;
            es.taller_id = s.TallerId;
            foreach (var l in s.LineasSolicitud)
            {
                ExposedLineaSolicitud ls = new ExposedLineaSolicitud();
                ls.id = l.id;
                ls.description = l.description;
                ls.quantity = l.quantity;
                es.lineas.Add(ls);
            }
            es.status = s.state;

            return es;
        }

        static public Solicitud FromExposed(ExposedSolicitud es)
        {
            Solicitud s = new Solicitud();

            s.TallerId = es.taller_id;
            s.LineasSolicitud = new List<LineaSolicitud>();
            foreach(var els in es.lineas) {
                LineaSolicitud ls = new LineaSolicitud();
                ls.id_en_taller = els.id;
                ls.description = els.description;
                ls.quantity = els.quantity;
                s.LineasSolicitud.Add(ls);
            }
            s.id_en_taller = es.taller_id;
            s.state = "NEW";
            s.date = DateTime.Now;

            return s;
        }

        static public List<Solicitud> FindAll()
        {
            List<Solicitud> l = new List<Solicitud>();

            var lq_l = from d in ms_ent.Solicituds select d;
            foreach (var singleDesguace in lq_l)
            {
                Solicitud s = Copy(singleDesguace);
                l.Add(s);
            }
            return l;
        }

        static public void InsertOrUpdate(Solicitud s)
        {
            if (s.id == default(int))
            {
                // New entity
                ms_ent.Solicituds.Add(s);
            }
            else
            {
                // Existing entity
                //ms_ent.DesguaceConjunto.Attach(desguace);
                ms_ent.Entry(s).State = EntityState.Modified;
            }
        }

        static public void Delete(int id)
        {
            var solicitud = ms_ent.Solicituds.Find(id);
            ms_ent.Solicituds.Remove(solicitud);
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