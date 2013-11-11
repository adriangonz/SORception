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
        public int addDesguace(string nombre)
        {
            if (nombre != "")
            {
                Desguace d = new Desguace();
                d.name = nombre;
                DesguaceRepository.InsertOrUpdate(d);
                DesguaceRepository.Save();
                return d.id;
            }
            throw new WebFaultException<string>("Nombre can't be empty", HttpStatusCode.InternalServerError);
        }

        public Desguace getDesguace(int id)
        {
            return DesguaceRepository.Find(id);;
        }

        public int putDesguace(Desguace d)
        {
            DesguaceRepository.InsertOrUpdate(d);
            DesguaceRepository.Save();
            return 0; //TODO
        }

        public int deleteDesgauce(int id)
        {
            DesguaceRepository.Delete(id);
            DesguaceRepository.Save();
            return 0; //TODO
        }
       
    }
}
