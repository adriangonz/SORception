//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManagerSystem
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    
    public partial class Oferta
    {
        public Oferta()
        {
            this.deleted = false;
            this.LineasOferta = new HashSet<LineaOferta>();
        }
    
        public int Id { get; set; }
        public int id_en_desguace { get; set; }
        public System.DateTime date { get; set; }
        public string status { get; set; }
        public int DesguaceId { get; set; }
        public int SolicitudId { get; set; }
        public bool deleted { get; set; }
    
        public virtual Desguace Desguace { get; set; }
        public virtual ICollection<LineaOferta> LineasOferta { get; set; }
        public virtual Solicitud Solicitud { get; set; }
    }
}