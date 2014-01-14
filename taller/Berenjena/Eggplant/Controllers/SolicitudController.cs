using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Eggplant.Controllers
{
    [RoutePrefix("api/solicitud")]
    public class SolicitudController : ApiController
    {
        static BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        static Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
        public static string DELETED = "DELETED";

        public static string LINEA_NEW = "NEW";
        public static string LINEA_UPDATED = "UPDATED";
        public static string LINEA_DELETE = "DELETED";
            public static string LINEA_NOEFECT = "NOEFECT";

        // GET api/solicitud
        public object Get()
        {
            var solicitudes = c_bd.SolicitudSet.AsQueryable().Where(x => x.status != DELETED).ToList();
            return solicitudes;
        }

        // GET api/solicitud/5
        public object Get(int id)
        {
            var solicitud = c_bd.SolicitudSet.AsQueryable().First(x => x.Id == id);
            return solicitud;
        }

        // POST api/solicitud
        public object Post([FromBody]JObject values)
        {
            // Consigo el token de la aplicacion para el id
            var tokens = c_bd.TokensSet.AsQueryable().ToList();
            //Si habia un token
            if (tokens.Count > 0)
            {
                ExposedSolicitud sol = new ExposedSolicitud();

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
                return new { id = resId };
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "El taller no esta dado de alta");
        }

        // PUT api/solicitud/5
        public void Put(int id, [FromBody]JObject values)
        {
            Solicitud solInterna = c_bd.SolicitudSet.FirstOrDefault(x => x.Id == id);
            if (solInterna != null)
            {
                ExposedSolicitud solExterna = svcTaller.getSolicitud(solInterna.sg_id);
                if (solExterna != null)
                {
                    //Creo las lineas de la solicitud desde los datos pasado por json
                    List<ExposedLineaSolicitud> lineas = new List<ExposedLineaSolicitud>();
                    foreach (JObject item in values["data"])
                    {
                        string efecto = item["update"].ToString();
                        //Si me envia datos que no sean totalmente inutiles que solo sirven para sobrecargar
                        if (efecto == LINEA_NEW || efecto == LINEA_UPDATED || efecto == LINEA_DELETE)
                        {
                            // Modificacion interna
                            if (efecto == LINEA_NEW)
                            {
                                LineaSolicitud linIn = new LineaSolicitud();
                                linIn.descripcion = item["descripcion"].ToString();
                                linIn.cantidad = int.Parse(item["cantidad"].ToString());
                                solInterna.LineaSolicitud.Add(linIn);
                            }
                            else if (efecto == LINEA_UPDATED)
                            {
                                int idToModify = int.Parse(item["id"].ToString());
                                LineaSolicitud linIn =
                                    c_bd.LineaSolicitudSet.FirstOrDefault(x => x.Id == idToModify);
                                linIn.descripcion = item["descripcion"].ToString();
                                linIn.cantidad = int.Parse(item["cantidad"].ToString());
                            }

                            if (efecto == LINEA_DELETE)
                            {
                                int idToDelete = int.Parse(item["id"].ToString());
                                LineaSolicitud linIn =
                                    c_bd.LineaSolicitudSet.FirstOrDefault(x => x.Id == idToDelete);
                                c_bd.LineaSolicitudSet.Remove(linIn);//.LineaSolicitud.Remove(linIn);
                            }
                            else
                            {
                                // Modificacion externa
                                ExposedLineaSolicitud lin = new ExposedLineaSolicitud();
                                lin.description = item["descripcion"].ToString();
                                lin.quantity = int.Parse(item["cantidad"].ToString());
                                lineas.Add(lin);
                            }

                            
                        }
                    }
                    c_bd.SaveChanges();

                    solExterna.lineas = lineas.ToArray();
                    svcTaller.putSolicitud(solExterna);
                }
            }
        }

        // DELETE api/solicitud/5
        public void Delete(int id)
        {
            Solicitud sol = c_bd.SolicitudSet.FirstOrDefault(x => x.Id == id);
            if (sol != null)
            {
                sol.status = DELETED;
                c_bd.SaveChanges();
                ExposedSolicitud sExt = svcTaller.getSolicitud(sol.sg_id);
                sExt.status = DELETED;
                svcTaller.putSolicitud(sExt);
            }
        }

        [Route("update")]
        public object GetUpdatedSolicitudes()
        {
            updateSolicitudes();
            return Get();
        }


        private void updateSolicitudes()
        {
            //Si no hay taller activo devuelve -1 y no encontrara nada digo yo
            var solicitudes = svcTaller.getSolicitudes().ToList();
            foreach (ExposedSolicitud solicitud in solicitudes)
            {
                Solicitud s = c_bd.SolicitudSet.FirstOrDefault(x => x.sg_id == solicitud.id);
                if (s != null)
                    s.status = solicitud.status;
            }

        }
        private void addSolicitudToLocalDB(int idSol)
        {
            ExposedSolicitud solExtern = svcTaller.getSolicitud(idSol);
            if (solExtern != null)
            {
                Solicitud s = new Solicitud();
                s.sg_id = solExtern.id;
                s.timeStamp = DateTime.Now;
                s.status = solExtern.status;
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
        /*
        private int getIdActive()
        {
            var tokenActive = c_bd.TokensSet.AsQueryable()
                .ToList()
                .FirstOrDefault(x => x.state == Berenjena.Controllers.SettingsController.ACTIVE);
            if (tokenActive != null)
            {
                int idTallerActual = svcTaller.getTaller(tokenActive.token).id;
                return idTallerActual;
            }
            return -1;

        }*/
    }
}
