using ActiveMQHelper;
using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ManagerSystem.Services
{
    public class AMQService : BaseService
    {

        public AMQService(UnitOfWork uow = null)
            : base(uow)
        {
            subscriber = new List<TopicSubscriber>();
            publisher = new List<TopicPublisher>();
        }



        public void createSubscriber(string broker, string client_id, string consumer_id, string topic)
        {
            TopicSubscriber subscriber = null;// new TopicSubscriber();




            subscribers.Add(subscriber);
        }

        public void publishMessage(/* */)
        {

        }

        public void processIncommingOffer(AMQOfertaMessage message)
        {
            /*
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
    }
}