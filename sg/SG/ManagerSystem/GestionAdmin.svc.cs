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
        public List<Desguace> getDesguaces()
        {
            return Desguace.FindAll();
        }

        public List<Taller> getTalleres()
        {
            return Taller.FindAll();
        }

        public int activeDesguace(int id, bool active)
        {
            Desguace d = Desguace.Find(id);
            if (d == null)
            {
                throw new WebFaultException<string>("Desguace not found", HttpStatusCode.NotFound);
            }
            d.active = active;
            Desguace.InsertOrUpdate(d);
            Desguace.Save();
            return 1;
        }

        public int activeTaller(int id, bool active)
        {
            Taller t = Taller.Find(id);
            if (t == null)
            {
                throw new WebFaultException<string>("Taller not found", HttpStatusCode.NotFound);
            }
            t.active = active;
            Taller.InsertOrUpdate(t);
            Taller.Save();
            return 1;
        }

        public int deleteTaller(int id)
        {
            Taller.Delete(id);
            Taller.Save();
            return 1;
        }

        public int deleteDesguace(int id)
        {
            Desguace.Delete(id);
            Desguace.Save();
            return 1;
        }
    }
}
