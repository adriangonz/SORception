using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public partial class Oferta
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        static private Oferta Copy(Oferta tmp)
        {
            Oferta o = new Oferta();

            return o;
        }

        static public Oferta Find(int id)
        {
            return ms_ent.OfertaSet.Find(id);
        }

        static public Oferta Sanitize(Oferta o)
        {
            return Copy(o);
        }
        
        static public ExposedOferta ToExposed(Oferta s)
        {
            ExposedOferta eo = new ExposedOferta();

            eo.id = s.Id;
            eo.solicitud_id = s.SolicitudId;
            eo.lineas = new List<ExposedLineaOferta>();
            foreach (var l in s.LineasOferta)
            {
                ExposedLineaOferta lo = new ExposedLineaOferta();
                lo.id = l.Id;
                lo.linea_solicitud_id = l.LineaSolicitudId;
                lo.notes = l.notes;
                lo.price = l.price;
                lo.quantity = l.quantity;
                eo.lineas.Add(lo);
            }

            return eo;
        }

        static public Oferta FromExposed(ExposedOferta eo)
        {
            Oferta o = new Oferta();

            Desguace d;
            try
            { 
                d = Desguace.Find(eo.desguace_id);
            }
            catch (Exception e)
            {
                Logger.Error(String.Format("Exception thrown at Oferta.FromExposed with message: {0}", e.Message));
                throw;
            }
            o.DesguaceId = d.Id;

            o.SolicitudId = eo.solicitud_id;
            o.LineasOferta = new List<LineaOferta>();
            foreach (var elo in eo.lineas)
            {
                LineaOferta lo = new LineaOferta();
                lo.id_en_desguace = elo.id_en_desguace;
                lo.LineaSolicitudId = elo.id;
                lo.quantity = elo.quantity;
                lo.price = elo.price;
                lo.notes = elo.notes;
                o.LineasOferta.Add(lo);
            }
            o.id_en_desguace = eo.id;
            o.state = "NEW";
            o.date = DateTime.Now;

            return o;
        }

        static public List<Oferta> GetOfSolicitud(int solicitud_id)
        {
            return ms_ent.OfertaSet.AsQueryable().Where(o => o.SolicitudId == solicitud_id).ToList();
        }

        static public List<Oferta> FindAll()
        {
            return ms_ent.OfertaSet.ToList();
        }
        
        static public void InsertOrUpdate(Oferta s)
        {
            if (s.Id == default(int))
            {
                // New entity
                ms_ent.OfertaSet.Add(s);
            }
            else
            {
                // Existing entity
                //ms_ent.DesguaceConjunto.Attach(desguace);
            }
        }

        static public void Delete(int id)
        {
            Oferta o = ms_ent.OfertaSet.Find(id);
            o.deleted = true;
        }

        static public void Save()
        {
            ms_ent.SaveChanges();
        }

        static public void Dispose()
        {
            ms_ent.Dispose();
        }
    }
}