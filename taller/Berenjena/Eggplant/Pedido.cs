//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eggplant
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedido
    {
        public Pedido()
        {
            this.LineaPedido = new HashSet<LineaPedido>();
        }
    
        public int Id { get; set; }
        public System.DateTime timeStamp { get; set; }
    
        public virtual ICollection<LineaPedido> LineaPedido { get; set; }
        public virtual Solicitud Solicitud { get; set; }
    }
}
