using Eggplant.ServiceTaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Models
{
    public class SolicitudRepository : Solicitud
    {
        public List<LineaSolicitudRepository> lineas = new List<LineaSolicitudRepository>();

        public void fromObject(Solicitud s)
        {
            this.Id = s.Id;
            this.LineaSolicitud = s.LineaSolicitud;
            this.Pedidos = s.Pedidos;
            this.sg_id = s.sg_id;
            this.status = s.status;
            this.timeStamp = s.timeStamp;

            foreach (LineaSolicitud ls in this.LineaSolicitud)
            {
                LineaSolicitudRepository lsr = new LineaSolicitudRepository();
                lsr.fromObject(ls);
                lineas.Add(lsr);
            }
        }
    }

    public class LineaSolicitudRepository : LineaSolicitud
    {
        public List<ExposedLineaOferta> offers = new List<ExposedLineaOferta>();
        public void fromObject(LineaSolicitud ls)
        {
            this.Id = ls.Id;
            this.sg_id = ls.sg_id;
            this.Solicitud = ls.Solicitud;
            this.SolicitudId = ls.SolicitudId;
            this.cantidad = ls.cantidad;
            this.descripcion = ls.descripcion;
        }
    }
}