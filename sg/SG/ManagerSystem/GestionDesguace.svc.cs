using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ManagerSystem
{
    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedDesguace
    {
        [DataMember]
        public string name;
    }

    [ServiceBehavior(Namespace = Constants.Namespace)]
    public class GestionDesguace : IGestionDesguace
    {
        public int signUp(ExposedDesguace ed)
        {
            if (ed != null)
            {
                Desguace d = DesguaceRepository.FromExposed(ed);
                d.active = false;
                DesguaceRepository.InsertOrUpdate(d);
                DesguaceRepository.Save();
                return d.id;
            }
            return -1;
        }

        public int getState(int id)
        {
            if (id == default(int))
                return 38;

            if (id >= 0)
            {
                var tmp = DesguaceRepository.Find(Convert.ToInt32(id));
                Desguace d = DesguaceRepository.Sanitize(tmp);
                if (d.active)
                    return id;
            }
            return -1;
        }
    }
}
