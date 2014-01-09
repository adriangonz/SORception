using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Web;

namespace Eggplant.Controllers
{
    [RoutePrefix("api/solicitud")]
    public class SolicitudController : ApiController
    {
        BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();

        // GET api/solicitud
        public object Get()
        {
            var solicitudes = c_bd.SolicitudSet.AsQueryable().ToList();
            /*var message = JsonConvert.SerializeObject(solicitudes.ToList(), 
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            */
            return solicitudes;
        }

        // GET api/solicitud/5
        public string Get(int id)
        {
            return "Solicitud"+id;
        }

        // POST api/solicitud
        public object Post([FromBody]JObject values)
        {
            // Consigo el token de la aplicacion para el id
            var tokens = c_bd.TokensSet.AsQueryable().ToList();
            //Si habia un token
            if (tokens.Count > 0) { 
                //Consigo el taller para obtener su id
                ExposedTaller t = svcTaller.getTaller(tokens.First().token);

                ExposedSolicitud sol = new ExposedSolicitud();
                sol.taller_id = t.id;//el taller de la solicitud es el activo en la bd

                //Creo las lineas de la solicitud desde los datos pasado por json
                List<ExposedLineaSolicitud> lineas = new List<ExposedLineaSolicitud>();
                foreach (JObject item in values["data"])
                {
                    ExposedLineaSolicitud lin = new ExposedLineaSolicitud();
                    lin.description = item["descripcion"].ToString();
                    lin.quantity = int.Parse(item["cantidad"].ToString());
                    lineas.Add(lin);
                }
                sol.lineas = lineas.ToArray();

                //Lango la peticion de alta al sistema gestor
                int resId = svcTaller.addSolicitud(sol);

                //Si algo ha ido mal
                if (resId == -1)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "El sistema gestor no ha creado la solicitud");
                else//Si ha ido bien
                    addSolicitudToLocalDB(resId); //Guardo la solicitud en la base de datos local
                
                //Si todo ha ido bien devuelvo el id de la solicitud del sistema gestor
                return new {id = resId};
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "El taller no esta dado de alta");
        }

        // PUT api/solicitud/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/solicitud/5
        public void Delete(int id)
        {
        }

        public void addSolicitudToLocalDB(int idSol)
        {
            ExposedSolicitud solExtern = svcTaller.getSolicitud(idSol);
            if (solExtern != null) { 
                Solicitud s = new Solicitud();
                s.sg_id = solExtern.id;
                s = c_bd.SolicitudSet.Add(s);
                foreach (ExposedLineaSolicitud linSolicitudExtern in solExtern.lineas)
                {
                    LineaSolicitud lineLocal = new LineaSolicitud();
                    lineLocal.cantidad = linSolicitudExtern.quantity;
                    lineLocal.descripcion = linSolicitudExtern.description;
                    lineLocal.sg_id = linSolicitudExtern.id;
                    s.LineaSolicitud.Add(lineLocal);
                }
                c_bd.SaveChanges();
            }
        }
    }
}
