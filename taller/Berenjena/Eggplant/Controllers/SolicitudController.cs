﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Eggplant.Models;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Eggplant.Controllers
{
    [Authorize]
    [RoutePrefix("api/solicitud")]
    public class SolicitudController : ApiController
    {
        private BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        static Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
        public static string DELETED = "DELETED";

        public static string LINEA_NEW = "NEW";
        public static string LINEA_UPDATED = "UPDATED";
        public static string LINEA_DELETE = "DELETED";
        public static string LINEA_NOEFECT = "NOEFECT";

        public Microsoft.AspNet.Identity.UserManager<Microsoft.AspNet.Identity.EntityFramework.IdentityUser> UserManager { get; private set; }

        // GET api/solicitud
        public object Get()
        {
            var userId = User.Identity.GetUserId();
            var solicitudes = c_bd.SolicitudSet.AsQueryable().Where(x => x.status != DELETED && x.user_id == userId).ToList();
            return solicitudes;
        }

        // GET api/solicitud/5
        public object Get(int id)
        {
            var userId = User.Identity.GetUserId();
            var solicitud = c_bd.SolicitudSet.AsQueryable().First(x => x.Id == id && x.user_id == userId);
            if (solicitud == null) return Request.CreateResponse(HttpStatusCode.NotFound,"La solicitud "+id+" no existe o no es de el usuario actual");
            var ofertas = svcTaller.getOfertas(solicitud.sg_id).ToList();

            foreach (var item in solicitud.LineaSolicitud)
            {
                int sol_id_sg = item.sg_id;
                foreach(var oferta in ofertas)
                {
                    // las lineas de oferta que tienen un pedido asociado y no estan en la bd
                    var lineasQueNoEstanInternas = oferta.lineas.AsQueryable().
                        Where(lineaOferta => c_bd.LineaPedidoSet.AsQueryable().                         
                            Where(lineaPedido => lineaPedido.linea_oferta_id == lineaOferta.id && lineaOferta.linea_solicitud.quantity > 0).ToList().Count == 0).ToList();
                    if (lineasQueNoEstanInternas.Count > 0)// se anyaden a la bd interna
                    {
                        addLineasNoAgregadas(lineasQueNoEstanInternas,id);
                    }
                    item.offers.AddRange(oferta.lineas.AsQueryable().Where(x => x.linea_solicitud_id == sol_id_sg).ToList());
                }
            }
            return solicitud;
        }

        // POST api/solicitud
        public object Post([FromBody]JObject values)
        {
            var s = new Solicitud();
            s.timeStamp = DateTime.Now;
            s.status = "FAILED";
            s.user_id = User.Identity.GetUserId();

            var solResult = c_bd.SolicitudSet.Add(s);

            //Creo las lineas de la solicitud desde los datos pasado por json
            List<ExpSolicitudLine> lineas = new List<ExpSolicitudLine>();
            foreach (JObject item in values["data"])
            {
                LineaSolicitud linInt = new LineaSolicitud();
                linInt.cantidad = int.Parse(item["cantidad"].ToString());
                linInt.descripcion = item["descripcion"].ToString();
                solResult.LineaSolicitud.Add(linInt);

                ExpSolicitudLine expoLinSol = new ExpSolicitudLine();
                expoLinSol.description = linInt.descripcion;
                expoLinSol.quantity = linInt.cantidad;
                expoLinSol.flag = ExpSolicitudLine.castToFlag(item["criterio"]["code"].ToString());
                lineas.Add(expoLinSol);
            }
            c_bd.SaveChanges();

            //Pasamos los ids locales al sistema gestor
            for (int i = 0; i < s.LineaSolicitud.Count; i++)
            {
                var linSol = s.LineaSolicitud.ToList()[i];
                var expoLinSol = lineas[i];
                expoLinSol.id_en_taller = linSol.Id;
            } 
            ExpSolicitud sol = new ExpSolicitud();
            sol.id_en_taller = s.Id;
            sol.deadline = DateTime.Now.AddDays(14);
            sol.lineas = lineas.ToArray();
           

            //Lanzo la peticion de alta al sistema gestor
            int resId = svcTaller.addSolicitud(sol);
            //Si algo ha ido mal
            if (resId == -1)
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "El sistema gestor no ha creado la solicitud");
            else//Si ha ido bien
                addSolicitudToLocalDB(resId, solResult.Id); //Guardo la solicitud en la base de datos local

            //Si todo ha ido bien devuelvo el id de la solicitud del sistema gestor
            return new { id = resId };
        }

        // PUT api/solicitud/5
        public object Put(int id, [FromBody]JObject values)
        {
            Solicitud solInterna = c_bd.SolicitudSet.FirstOrDefault(x => x.Id == id);
            if (solInterna != null)
            {
                ExpSolicitud solExterna = svcTaller.getSolicitud(solInterna.sg_id);
                if (solExterna != null)
                {
                    //Creo las lineas de la solicitud desde los datos pasado por json
                    List<ExpSolicitudLine> lineas = new List<ExpSolicitudLine>();
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
                            else if (efecto == LINEA_DELETE)
                            {
                                int idToDelete = int.Parse(item["id"].ToString());
                                LineaSolicitud linIn =
                                    c_bd.LineaSolicitudSet.FirstOrDefault(x => x.Id == idToDelete);
                                c_bd.LineaSolicitudSet.Remove(linIn);//.LineaSolicitud.Remove(linIn);
                            }
                            // Modificacion externa
                            ExpSolicitudLine lin = new ExpSolicitudLine();
                            lin.description = item["descripcion"].ToString();
                            lin.quantity = int.Parse(item["cantidad"].ToString());
                            lin.action = efecto;
                            lineas.Add(lin);



                        }
                    }
                    c_bd.SaveChanges();

                    solExterna.lineas = lineas.ToArray();
                    svcTaller.putSolicitud(solExterna);
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "");
                }
                else return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Solicitud no encontrada en el sistema gestor");
            }
            else return Request.CreateErrorResponse(HttpStatusCode.NotFound, "La solicitud no existe en la db local");
        }

        // DELETE api/solicitud/5
        public void Delete(int id)
        {
            Solicitud sol = c_bd.SolicitudSet.FirstOrDefault(x => x.Id == id);
            if (sol != null)
            {
                sol.status = DELETED;
                c_bd.SaveChanges();
                ExpSolicitud sExt = svcTaller.getSolicitud(sol.sg_id);
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
            foreach (ExpSolicitud solicitud in solicitudes)
            {
                Solicitud s = c_bd.SolicitudSet.FirstOrDefault(x => x.sg_id == solicitud.id);
                if (s != null)
                    s.status = solicitud.status;
            }

        }
        private void addSolicitudToLocalDB(int idSol, int idInterno)
        {
            ExpSolicitud solExtern = svcTaller.getSolicitud(idSol);
            if (solExtern != null)
            {
                using (BDBerenjenaContainer c_bd_interna = new BDBerenjenaContainer())
                {
                    Solicitud s = c_bd_interna.SolicitudSet.FirstOrDefault(x => x.Id == idInterno);//TODO deberia ser taller_solicitud_id
                    if (s != null)
                    {
                        s.sg_id = solExtern.id;
                        s.timeStamp = DateTime.Now;
                        s.status = solExtern.status;
                        foreach (var linSolicitudExtern in solExtern.lineas)
                        {
                            var lineaInterna = c_bd_interna.LineaSolicitudSet.FirstOrDefault(x => x.Id == linSolicitudExtern.id_en_taller);
                            if (lineaInterna != null)
                            {
                                lineaInterna.sg_id = linSolicitudExtern.id;
                            }
                        }
                        c_bd_interna.SaveChanges();
                    }
                }
            }
        }

        private void addLineasNoAgregadas(List<ExpOfertaLine> lineas, int idSolicitud)
        {
            Pedido p = new Pedido();
            p.Solicitud = c_bd.SolicitudSet.FirstOrDefault(solicitud => solicitud.sg_id == idSolicitud);
            foreach (var lineaPedidoExterna in lineas)
            {
                LineaPedido lp = new LineaPedido();
                lp.quantity = lineaPedidoExterna.linea_solicitud.quantity;
                lp.linea_oferta_id = lineaPedidoExterna.id;
                lp.price = (decimal)lineaPedidoExterna.price;
                p.LineaPedido.Add(lp);
            }
            c_bd.PedidoSet.Add(p);
            c_bd.SaveChanges();
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
