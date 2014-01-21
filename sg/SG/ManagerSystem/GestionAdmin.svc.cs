using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ManagerSystem
{
    public class GestionAdmin : IGestionAdmin
    {
        private managersystemEntities db_context;
        private RDesguace r_desguace;
        private RTaller r_taller;

        public GestionAdmin()
        {
            init(null);
        }

        public GestionAdmin(managersystemEntities context)
        {
            init(context);
        }

        private void init(managersystemEntities context)
        {
            db_context = context;
            r_desguace = new RDesguace(db_context);
            r_taller = new RTaller(db_context);
        }

        public List<Desguace> getDesguaces()
        {
            return r_desguace.FindAll();
        }

        public List<Taller> getTalleres()
        {
            return r_taller.FindAll();
        }

        public int activeDesguace(int id, bool active)
        {
            Desguace d = r_desguace.Find(id);
            if (d == null)
            {
                throw new WebFaultException<string>("Desguace not found", HttpStatusCode.NotFound);
            }
            d.active = active;
            r_desguace.InsertOrUpdate(d);
            r_desguace.Save();
            return 1;
        }

        public int activeTaller(int id, bool active)
        {
            Taller t = r_taller.Find(id);
            if (t == null)
            {
                throw new WebFaultException<string>("Taller not found", HttpStatusCode.NotFound);
            }
            t.active = active;
            r_taller.InsertOrUpdate(t);
            r_taller.Save();
            return 1;
        }

        public int deleteTaller(int id)
        {
            r_taller.Delete(id);
            r_taller.Save();
            return 1;
        }

        public int deleteDesguace(int id)
        {
            r_desguace.Delete(id);
            r_desguace.Save();
            return 1;
        }
    }
}
