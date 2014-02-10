using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem
{
    public class ROferta
    {
        public managersystemEntities db_context;

        public ROferta()
        {
            db_context = new managersystemEntities();
        }

        public ROferta(managersystemEntities context)
        {
            db_context = context;
        }

        private Oferta Copy(Oferta tmp)
        {
            Oferta o = new Oferta();

            return o;
        }

        public Oferta Find(int id)
        {
            return db_context.OfertaSet.Find(id);
        }

        public Oferta Sanitize(Oferta o)
        {
            return Copy(o);
        }
        
        public ExpOferta ToExposed(Oferta oferta)
        {
            ExpOferta out_oferta = new ExpOferta();

            out_oferta.id = oferta.Id;
            out_oferta.solicitud_id = oferta.SolicitudId;
            out_oferta.lineas = new List<ExpOferta.Line>();
            foreach (var l_oferta in oferta.LineasOferta)
            {
                ExpOferta.Line out_l_oferta = new ExpOferta.Line();
                out_l_oferta.id = l_oferta.Id;
                out_l_oferta.linea_solicitud_id = l_oferta.LineaSolicitudId;
                out_l_oferta.notes = l_oferta.notes;
                out_l_oferta.price = l_oferta.price;
                out_l_oferta.quantity = l_oferta.quantity;
                if (l_oferta.LineaOfertaSeleccionada != null)
                {/*
                    out_l_oferta.linea_solicitud = new ExpOferta.Line.RelatedSolicitud();
                    out_l_oferta.linea_solicitud.linea_solicitud_id = l_oferta.LineaSolicitudId;
                    out_l_oferta.linea_solicitud.quantity = l_oferta.LineaOfertaSeleccionada.quantity;*/
                }
                out_oferta.lineas.Add(out_l_oferta);
            }

            return out_oferta;
        }

        public void UpdateFromExposed(Oferta oferta, ExpOferta exp_oferta)
        {
            foreach (var l_oferta in oferta.LineasOferta)
            {
                l_oferta.status = "DELETED";
            }

            oferta.LineasOferta = new List<LineaOferta>();
            foreach (var exp_l_oferta in exp_oferta.lineas)
            {
                LineaOferta l_oferta = new LineaOferta();
                l_oferta.id_en_desguace = exp_l_oferta.id_en_desguace;
                l_oferta.LineaSolicitud = db_context.LineasSolicitudSet.Find(exp_l_oferta.linea_solicitud_id);
                l_oferta.quantity = exp_l_oferta.quantity;
                l_oferta.price = exp_l_oferta.price;
                l_oferta.notes = exp_l_oferta.notes;
                l_oferta.status = "UPDATED";
                oferta.LineasOferta.Add(l_oferta);
            }
            oferta.id_en_desguace = exp_oferta.id_en_desguace;
            oferta.status = "UPDATED";
        }

        public Oferta FromExposed(ExpOferta exp_oferta, Desguace d)
        {
            Oferta oferta = new Oferta();
            oferta.DesguaceId = d.Id;
            oferta.SolicitudId = exp_oferta.solicitud_id;
            oferta.LineasOferta = new List<LineaOferta>();
            foreach (var exp_l_oferta in exp_oferta.lineas)
            {
                LineaOferta l_oferta = new LineaOferta();
                l_oferta.id_en_desguace = exp_l_oferta.id_en_desguace;
                l_oferta.LineaSolicitud = db_context.LineasSolicitudSet.Find(exp_l_oferta.linea_solicitud_id);
                l_oferta.quantity = exp_l_oferta.quantity;
                l_oferta.price = exp_l_oferta.price;
                l_oferta.notes = exp_l_oferta.notes;
                l_oferta.status = "NEW";
                oferta.LineasOferta.Add(l_oferta);
            }
            oferta.id_en_desguace = exp_oferta.id_en_desguace;
            oferta.status = "NEW";
            oferta.date = DateTime.Now;

            return oferta;
        }

        public List<Oferta> GetOfSolicitud(int solicitud_id)
        {
            return db_context.OfertaSet.AsQueryable().Where(o => o.SolicitudId == solicitud_id).ToList();
        }

        public List<Oferta> FindAll()
        {
            return db_context.OfertaSet.ToList();
        }
        
        public void InsertOrUpdate(Oferta s)
        {
            if (s.Id == default(int))
            {
                // New entity
                db_context.OfertaSet.Add(s);
            }
            else
            {
                // Existing entity
                //ms_ent.DesguaceConjunto.Attach(desguace);
            }
        }

        public void Delete(int id)
        {
            Oferta o = db_context.OfertaSet.Find(id);
            o.deleted = true;
        }

        public void Save()
        {
            db_context.SaveChanges();
        }

        public void Dispose()
        {
            db_context.Dispose();
        }
    }
}