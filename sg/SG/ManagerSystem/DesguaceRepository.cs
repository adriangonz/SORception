using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace ManagerSystem
{
    public class DesguaceRepository
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        static private Desguace Copy(Desguace tmp) {
            Desguace d = new Desguace();
            d.Id = tmp.Id;
            d.name = tmp.name;
            d.active = tmp.active;
            //d.Oferta = tmp.Oferta;
            return d;
        }

        static public Desguace FromExposed(ExposedDesguace ed) 
        {
            Desguace d = new Desguace();

            d.name = ed.name;

            return d;
        }

        static public ExposedDesguace ToExposed(Desguace d)
        {
            ExposedDesguace ed = new ExposedDesguace();

            ed.name = d.name;

            return ed;
        }

        static public Desguace Find(int id)
        {
            return ms_ent.DesguaceSet.Find(id);
        }

        static public Desguace Find(string token)
        {
            Token t = TokenRepository.Find(token);
            if (t != null)
                return t.Desguace;
            return null;
        }

        static public Desguace Sanitize(Desguace d)
        {
            return Copy(d);
        }


        static public List<Desguace> FindAll()
        {
            List<Desguace> l = new List<Desguace>();

            var lq_l = from d in ms_ent.DesguaceSet select d;
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

        static public void InsertOrUpdate(Desguace desguace)
        {
            if (desguace.Id == default(int))
            {
                // New entity
                ms_ent.DesguaceSet.Add(desguace);
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

        static public void Delete(int id)
        {
            Desguace d = ms_ent.DesguaceSet.Find(id);
            d.deleted = true;
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
