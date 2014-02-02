using Eggplant.DTO;
using Eggplant.Entity;
using Eggplant.Exceptions;
using Eggplant.ServiceTaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Eggplant.Application
{
    public class SolicitudApplication : AbstractApplication
    {
        public IEnumerable<Entity.Solicitud> getByUser(string userId)
        {
            return dataService.Solicitudes.Get(x => x.user_id == userId);
        }

        public int Request(SolicitudPostDTO solPost, string userId)
        {
            Solicitud sol_interna = new Solicitud();
            sol_interna.status = Solicitud.FAILED;
            sol_interna.user_id = userId;
            sol_interna.rawLines = solPost.toEntity();
            dataService.Solicitudes.Insert(sol_interna);
            dataService.SaveChanges();

            List<ExpSolicitudLine> lineas_externas = new List<ExpSolicitudLine>();
            foreach (var lin_interna in sol_interna.rawLines)
            {
                ExpSolicitudLine lin_externa = new ExpSolicitudLine();
                lin_externa.id_en_taller = lin_interna.id;
                lin_externa.description = lin_interna.descripcion;
                lin_externa.quantity = lin_interna.cantidad;
                lin_externa.flag = ExpSolicitudLine.castToFlag(lin_interna.criterio);
                lineas_externas.Add(lin_externa);
            }
            ExpSolicitud sol_externa = new ExpSolicitud();
            sol_externa.id_en_taller = sol_interna.id;
            sol_externa.deadline = solPost.deadline;
            sol_externa.lineas = lineas_externas.ToArray();

            //Lanzo la peticion de alta al sistema gestor
            int resId = sgService.addSolicitud(sol_externa);
            //Si algo ha ido mal
            if (resId == -1)
                throw new ApplicationLayerException(HttpStatusCode.InternalServerError, "Algo ha ido mal en el SG al anyadir la nueva solicitud");


            addSolicitudToLocalDB(resId, sol_interna.id); //Guardo la solicitud en la base de datos local

            return resId;
        }


        private void addSolicitudToLocalDB(int idSol, int idInterno)
        {
            ExpSolicitud solExtern = sgService.getSolicitud(idSol);
            if (solExtern != null)
            {

                Solicitud s = dataService.Solicitudes.GetByID(idInterno);
                if (s != null)
                {
                    s.sg_id = solExtern.id;
                    s.status = solExtern.status;
                    foreach (var linSolicitudExtern in solExtern.lineas)
                    {
                        var lineaInterna = dataService.LineasSolicitud.GetByID(linSolicitudExtern.id_en_taller);
                        if (lineaInterna != null)
                        {
                            lineaInterna.sg_id = linSolicitudExtern.id;
                        }
                    }
                    dataService.SaveChanges();
                }

            }
        }
    }
}