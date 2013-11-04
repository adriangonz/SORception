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
        public int addNewDesguace(string nombre)
        {
            if (nombre != "")
            {
                using (managersystemEntities ms_ent = new managersystemEntities())
                {
                    var d = ms_ent.DesguaceConjunto.Create();
                    d.name = nombre;
                    ms_ent.DesguaceConjunto.Add(d);
                    ms_ent.SaveChanges();
                    return d.id;
                }
            }
            return 0;// throw new WebFaultException<string>("Nombre can't be empty", HttpStatusCode.InternalServerError);
        }

        public Desguace getById(int desguaceId)
        {
            using (managersystemEntities ms_ent = new managersystemEntities()) {
                Desguace d = new Desguace();
                var desguace = (from db_d in ms_ent.DesguaceConjunto
                                where db_d.id == desguaceId
                                select db_d).First();

                d.id = desguace.id;
                d.name = desguace.name;
                d.active = desguace.active;

                return d;
            }
        }
        /*
        public List<Desguace> getAll()
        {
            using (managersystemEntities ms_ent = new managersystemEntities())
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
        }

        public int activateDesguace(int desguaceId)
        {
            using (managersystemEntities ms_ent = new managersystemEntities()) {
                var desguace = (from d in ms_ent.DesguaceConjunto
                                where d.id == desguaceId
                                select d).First();
                desguace.active = true;
                ms_ent.SaveChanges();
            }

            return 0;
        }*/
    }
}
