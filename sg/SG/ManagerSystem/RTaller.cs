using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class RTaller
    {
        public managersystemEntities db_context;

        public RTaller()
        {
            db_context = new managersystemEntities();
        }

        public RTaller(managersystemEntities context)
        {
            db_context = context;
        }

        private Taller Copy(Taller tmp)
        {
            Taller d = new Taller();
            d.Id = tmp.Id;
            d.name = tmp.name;
            d.active = tmp.active;
            return d;
        }

        public ExpTaller ToExposed(Taller t)
        {
            ExpTaller et = new ExpTaller();

            et.name = t.name;

            return et;
        }

        public Taller FromExposed(ExpTaller et)
        {
            Taller t = new Taller();

            t.name = et.name;

            return t;
        }

        public Taller Find(int id)
        {
            return db_context.TallerSet.Find(id);
        }

        public Taller Sanitize(Taller d)
        {
            return Copy(d);
        }


        public List<Taller> FindAll()
        {
            List<Taller> l = new List<Taller>();

            var lq_l = from d in db_context.TallerSet select d;
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

        public void InsertOrUpdate(Taller taller)
        {
            if (taller.Id == default(int))
            {
                // New entity
                db_context.TallerSet.Add(taller);
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

        public void Delete(int id)
        {
            Taller t = Find(id);
            t.deleted = true;
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