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