using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ManagerSystem.Services
{
    public class AMQService : BaseService
    {
        public void processIncommingOffer(AMQOfertaMessage message)
        {/*
            Desguace d = r_desguace.Find(message.desguace_id);
            int id_en_desguace = message.oferta.id_en_desguace;
            Oferta o = null;
            switch (message.code)
            {
                case AMQOfertaMessage.Code.New:
                    o = r_oferta.FromExposed(message.oferta, d);
                    r_oferta.InsertOrUpdate(o);
                    r_oferta.Save();
                    break;
                case AMQOfertaMessage.Code.Update:
                    o = db_context.OfertaSet.First(of => of.id_en_desguace == id_en_desguace);
                    r_oferta.UpdateFromExposed(o, message.oferta);
                    r_oferta.InsertOrUpdate(o);
                    r_oferta.Save();
                    break;
                case AMQOfertaMessage.Code.Delete:
                    o = db_context.OfertaSet.First(of => of.id_en_desguace == id_en_desguace);
                    r_oferta.Delete(o.Id);
                    r_oferta.Save();
                    break;
            }
            return o;*/
        }

        public void addOffer(ExpOferta e_offer)
        {

        }

        public void updateOffer(ExpOferta e_offer)
        {

        }

        public void deleteOffer(ExpOferta e_offer)
        {

        }
    }
}