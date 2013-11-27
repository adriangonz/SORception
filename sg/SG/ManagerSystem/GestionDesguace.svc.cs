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
        public string singUp(string nombre)
        {
            if (nombre != null && nombre != "")
            {
                Desguace d = new Desguace();
                d.name = nombre;
                DesguaceRepository.InsertOrUpdate(d);
                DesguaceRepository.Save();
                return d.id.ToString();
            }
            return "-1";
        }

        public bool getState(string id)
        {
            if (id != null && id != "")
            {
                var tmp = DesguaceRepository.Find(Convert.ToInt32(id));
                Desguace d = DesguaceRepository.Sanitize(tmp);
                return d.active;
            }
            return false;
        }
    }
}
