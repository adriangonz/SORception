using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;

namespace Eggplant.Controllers
{
    public class PedidoController : ApiController
    {
        static BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        static Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();

        // GET api/pedido
        public object Get()
        {
            TallerResponse tr = new TallerResponse();//TODO SG
            TallerResponse.SelectedLine sl = new TallerResponse.SelectedLine();
            sl.quantity = 5;//tr.selected_lines
            return new string[] { "value1", "value2" };
        }

        // GET api/pedido/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/pedido
        public void Post([FromBody]string value)
        {
        }

        // PUT api/pedido/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/pedido/5
        public void Delete(int id)
        {
        }
    }
}
