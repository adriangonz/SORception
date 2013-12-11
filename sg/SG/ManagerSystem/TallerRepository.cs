using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class TallerRepository
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        static private Taller Copy(Taller tmp)
        {
            Taller d = new Taller();
            d.Id = tmp.Id;
            d.name = tmp.name;
            d.active = tmp.active;
            return d;
        }

        static public ExposedTaller ToExposed(Taller t)
        {
            ExposedTaller et = new ExposedTaller();

            et.id = t.Id;
            et.name = t.name;

            return et;
        }

        static public Taller FromExposed(ExposedTaller et)
        {
            Taller t = new Taller();

            t.Id = et.id;
            t.name = et.name;

            return t;
        }

        static public Taller Find(int id)
        {
            return ms_ent.Tallers.Find(id);
        }

        static public Taller Sanitize(Taller d)
        {
            return Copy(d);
        }


        static public List<Taller> FindAll()
        {
            List<Taller> l = new List<Taller>();

            var lq_l = from d in ms_ent.Tallers select d;
            foreach (var singleTaller in lq_l)
            {
                Taller d = new Taller();
                d.Id = singleTaller.Id;
                d.active = singleTaller.active;
                d.name = singleTaller.name;

                l.Add(d);
            }
            return l;
        }

        static public void InsertOrUpdate(Taller taller)
        {
            if (taller.Id == default(int))
            {
                // New entity
                ms_ent.Tallers.Add(taller);
            }
            else
            {
                // Existing entity
                Taller t = Find(taller.Id);
                if (taller.name != null)
                {
                    t.name = taller.name;
                }
            }
        }

        static public void Delete(int id)
        {
            var taller = ms_ent.Tallers.Find(id);
            ms_ent.Tallers.Remove(taller);
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