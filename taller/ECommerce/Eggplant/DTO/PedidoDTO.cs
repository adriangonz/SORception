using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.DTO
{
    public class PedidoDTO
    {
        public int solicitud { get; set; }
        public List<LineaPedidoDTO> lineas;
    }

    public class LineaPedidoDTO
    {
        public int id_linea_oferta { get; set; }
        public int id_linea_solcitud { get; set; }
        public int cantidad { get; set; }

    }
}