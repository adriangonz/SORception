using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GestionAdmin" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GestionAdmin.svc or GestionAdmin.svc.cs at the Solution Explorer and start debugging.
    public class GestionAdmin : IGestionAdmin
    {
        public Desguace getDesguaces()
        {
            return null;
        }

        public Taller getTalleres()
        {
            return null;
        }

        public int activeDesguace(int id, bool active)
        {
            return 0;
        }

        public int activeTaller(int id, bool active)
        {
            return 0;
        }

        public int deleteTaller(int id)
        {
            return 0;
        }

        public int deleteDesguace(int id)
        {
            return 0;
        }
    }
}
