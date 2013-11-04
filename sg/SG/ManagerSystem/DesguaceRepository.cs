using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class DesguaceRepository
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        static public Desguace Find(int id)
        {
            return ms_ent.DesguaceConjunto.Find(id);
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
    }
}
