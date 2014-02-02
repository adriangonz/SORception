using Eggplant.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.DTO
{
    public class SolicitudPostDTO
    {
        public List<SolicitudLineDTO> data { get; set; }
        public DateTime deadline { get; set; }
        public List<LineaSolicitud> toEntity()
        {
            List<LineaSolicitud> solicitudes = new List<LineaSolicitud>();
            foreach (var sol in this.data)
            {
                solicitudes.Add(sol.toEntity());
            }
            return solicitudes;
        }
    }
    public class SolicitudLineDTO {
        public int cantidad { get; set; }
        public SolicitudLineCriterioDTO criterio { get; set; }
        public string descripcion { get; set; }
        public string update { get; set; }

        public LineaSolicitud toEntity()
        {
            LineaSolicitud linSol = new LineaSolicitud();
            linSol.cantidad = this.cantidad;
            linSol.criterio = this.criterio.name;
            linSol.descripcion = this.descripcion;
            linSol.status = LineaSolicitud.FAILED;

            return linSol;
        }
    }
    public class SolicitudLineCriterioDTO
    {
        public int code { get; set; }
        public string name { get; set; }
    }
}