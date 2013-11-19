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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GestionAdmin" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GestionAdmin.svc or GestionAdmin.svc.cs at the Solution Explorer and start debugging.
    public class GestionAdmin : IGestionAdmin
    {
        public List<Desguace> getDesguaces()
        {
            return DesguaceRepository.FindAll();
        }

        public List<Taller> getTalleres()
        {
            return TallerRepository.FindAll();
        }

        public int activeDesguace(int id, bool active)
        {
            Desguace d = DesguaceRepository.Find(id);
            if (d == null)
            {
                throw new WebFaultException<string>("Desguace not found", HttpStatusCode.NotFound);
            }
            d.active = active;
            DesguaceRepository.InsertOrUpdate(d);
            DesguaceRepository.Save();
            return 1;
        }

        public int activeTaller(int id, bool active)
        {
            Taller t = TallerRepository.Find(id);
            if (t == null)
            {
                throw new WebFaultException<string>("Taller not found", HttpStatusCode.NotFound);
            }
            t.active = active;
            TallerRepository.InsertOrUpdate(t);
            TallerRepository.Save();
            return 1;
        }

        public int deleteTaller(int id)
        {
            TallerRepository.Delete(id);
            TallerRepository.Save();
            return 1;
        }

        public int deleteDesguace(int id)
        {
            DesguaceRepository.Delete(id);
            DesguaceRepository.Save();
            return 1;
        }
    }
}
