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
        
        static public ExpOferta ToExposed(Oferta s)
        {
            ExpOferta eo = new ExpOferta();

            eo.id = s.Id;
            eo.solicitud_id = s.SolicitudId;
            eo.lineas = new List<ExpOferta.Line>();
            foreach (var l in s.LineasOferta)
            {
                ExpOferta.Line lo = new ExpOferta.Line();
                lo.id = l.Id;
                lo.linea_solicitud_id = l.LineaSolicitudId;
                lo.notes = l.notes;
                lo.price = l.price;
                lo.quantity = l.quantity;
                eo.lineas.Add(lo);
            }

            return eo;
        }

        static public Oferta FromExposed(ExpOferta eo, Desguace d)
        {
            Oferta o = new Oferta();
            o.DesguaceId = d.Id;
            o.SolicitudId = eo.solicitud_id;
            o.LineasOferta = new List<LineaOferta>();
            foreach (var elo in eo.lineas)
            {
                LineaOferta lo = new LineaOferta();
                lo.id_en_desguace = elo.id_en_desguace;
                lo.LineaSolicitud = ms_ent.LineasSolicitudSet.Find(elo.id);
                lo.quantity = elo.quantity;
                lo.price = elo.price;
                lo.notes = elo.notes;
                o.LineasOferta.Add(lo);
            }
            o.id_en_desguace = eo.id_en_desguace;
            o.status = "NEW";
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