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
            d.id = tmp.id;
            d.name = tmp.name;
            d.active = tmp.active;
            d.Oferta = tmp.Oferta;
            return d;
        }

        static public Desguace Find(int id)
        {
            return ms_ent.DesguaceConjunto.Find(id);
        }

        static public Desguace Sanitize(Desguace d)
        {
            return Copy(d);
        }


        static public List<Desguace> FindAll()
        {
            List<Desguace> l = new List<Desguace>();

            var lq_l = from d in ms_ent.DesguaceConjunto select d;
            foreach (var singleDesguace in lq_l)
            {
                Desguace d = new Desguace();
                d.id = singleDesguace.id;
                d.active = singleDesguace.active;
                d.name = singleDesguace.name;

                l.Add(d);
            }
            return l;
        }

        static public void InsertOrUpdate(Desguace desguace)
        {
            if (desguace.id == default(int))
            {
                // New entity
                ms_ent.DesguaceConjunto.Add(desguace);
            }
            else
            {
                // Existing entity
                //ms_ent.DesguaceConjunto.Attach(desguace);
                ms_ent.Entry(desguace).State = EntityState.Modified;
            }
        }

        static public void Delete(int id)
        {
            var desguace = ms_ent.DesguaceConjunto.Find(id);
            ms_ent.DesguaceConjunto.Remove(desguace);
        }

        static public void Save()
        {
            ms_ent.SaveChanges();
        }

        static public void Dispose()
        {
            ms_ent.Dispose();
        }

        static public object ToXML(Desguace d)
        {
            using (MemoryStream memStr = new MemoryStream())
            {
                var serializer = new DataContractSerializer(d.GetType());
                serializer.WriteObject(memStr, d);

                memStr.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStr))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}
