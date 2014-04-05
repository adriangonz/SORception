using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.ServiceTaller
{
    public partial class ExpOfertaLine
    {
        public bool isPedida()
        {
            return (this.CantidadPedida > 0);
            //return (this.linea_solicitud != null && this.linea_solicitud.quantity > 0);
        }
    }
}