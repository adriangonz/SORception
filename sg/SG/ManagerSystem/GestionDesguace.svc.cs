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
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "GestionDesguace" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione GestionDesguace.svc o GestionDesguace.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class GestionDesguace : IGestionDesguace
    {
        public int signUp(Desguace d)
        {
            if (d != null)
            {
                d.active = false;
                DesguaceRepository.InsertOrUpdate(d);
                DesguaceRepository.Save();
                return d.id;
            }
            return -1;
        }

        public int getState(int id)
        {
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
