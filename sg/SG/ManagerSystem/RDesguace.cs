using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace ManagerSystem
{
    public class RDesguace
    {
        private managersystemEntities db_context;
        private RToken r_token; 

        public RDesguace()
        {
            init(new managersystemEntities());
        }

        public RDesguace(managersystemEntities context)
        {
            init(context);
        }

        private void init(managersystemEntities context)
        {
            db_context = context;
            r_token = new RToken(db_context);
        }

        private Desguace Copy(Desguace tmp) {
            Desguace d = new Desguace();
            d.Id = tmp.Id;
            d.name = tmp.name;
            d.active = tmp.active;
            //d.Oferta = tmp.Oferta;
            return d;
        }

        public Desguace FromExposed(ExpDesguace ed) 
        {
            Desguace d = new Desguace();

            d.name = ed.name;

            return d;
        }

        public ExpDesguace ToExposed(Desguace d)
        {
            ExpDesguace ed = new ExpDesguace();

            ed.name = d.name;

            return ed;
        }

        public Desguace Find(int id)
        {
            return db_context.DesguaceSet.Find(id);
        }

        public Desguace Find(string token)
        {
            Token t = r_token.Find(token);
            if (t != null)
                return t.Desguace;
            return null;
        }

        public Desguace Sanitize(Desguace d)
        {
            return Copy(d);
        }


        public List<Desguace> FindAll()
        {
            List<Desguace> l = new List<Desguace>();

            var lq_l = from d in db_context.DesguaceSet select d;
            foreach (var singleDesguace in lq_l)
            {
                Desguace d = new Desguace();
                d.Id = singleDesguace.Id;
                d.active = singleDesguace.active;
                d.name = singleDesguace.name;

                l.Add(d);
            }
            return l;
        }

        public void InsertOrUpdate(Desguace desguace)
        {
            if (desguace.Id == default(int))
            {
                // New entity
                db_context.DesguaceSet.Add(desguace);
            }
            else
            {
                // Existing entity
                Desguace d = Find(desguace.Id);
                if (desguace.name != null)
                {
                    d.name = desguace.name;
                }
            }
        }

        public void Delete(int id)
        {
            Desguace d = db_context.DesguaceSet.Find(id);
            d.deleted = true;
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
