using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Identity;
using Eggplant.Application;
using Eggplant.DTO;

namespace Eggplant.Controllers
{
    [Authorize]
    public class PedidoController : ApiController
    {

        private PedidoApplication pa;
        public PedidoController()
        {
            pa = new PedidoApplication();
        }

        // GET api/pedido
        public object Get()
        {
            return pa.getAll(User.Identity.GetUserId());

        }

        // GET api/pedido/5
        public object Get(int id)
        {
            return pa.getById(id, User.Identity.GetUserId());
        }

        // POST api/pedido
        public object Post(PedidoDTO values)
        {
            return pa.request(values);
           
        }

        
    }
}
