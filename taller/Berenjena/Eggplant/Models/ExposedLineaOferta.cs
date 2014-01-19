using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Eggplant.ServiceTaller
{
    public partial class ExposedLineaOferta 
    {
        private int cantidadPedida;

        [global::System.Runtime.Serialization.DataMemberAttribute()] 
        public int CantidadPedida
        {
            get { return cantidadPedida; }
            set { cantidadPedida = value; }
        }
    }
}