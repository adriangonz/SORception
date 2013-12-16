using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Berenjena.Controllers
{
    public class GestionController : ApiController
    {
        BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        ServiceTaller.GestionTallerClient svcTaller = new ServiceTaller.GestionTallerClient();
        string REQUESTED = "REQUESTED";
        string EXPIRED = "EXPIRED";
        string ACTIVE = "ACTIVE";
        
        // GET api/gestion
        public object Get()
        {
            if (c_bd.TokensSet.Count() <= 0) return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se ha solicitado token");
            var tokens = (from d in c_bd.TokensSet orderby d.timeStamp descending select d);

            ServiceTaller.Taller t = svcTaller.getTaller(tokens.First().token);
            return (new { id = t.Id, name = t.name, active = t.active, tokens });
            
        }

        // POST api/gestion
        public object Post([FromBody]JObject value)
        {
            setLastTokenAsExpered();
            Tokens to = new Tokens();
            to.token = svcTaller.addTaller(value["nombre"].ToString());
            to.timeStamp = DateTime.Now;
            to.state = REQUESTED;
            c_bd.TokensSet.Add(to);
            c_bd.SaveChanges();

            return to;
        }

        // PUT api/gestion
        public void Put([FromBody]JObject value)
        {
            var token = (from d in c_bd.TokensSet orderby d.timeStamp select d).First();
            ServiceTaller.Taller t = svcTaller.getTaller(token.token);
            t.name = value["nombre"].ToString();
            svcTaller.putTaller(t);
        }

        // DELETE api/gestion
        public void Delete()
        {
            var token = (from d in c_bd.TokensSet orderby d.timeStamp select d).First();
            svcTaller.deleteTaller(token.token);
        }

        // GET api/gestion/token
        [Route("api/gestion/token")]
        public object GetToken() {
            if (c_bd.TokensSet.Count() <= 0) return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se ha solicitado token");
            var token = (from d in c_bd.TokensSet orderby d.timeStamp descending select d).First();
            // Si el token esta pendiente de activacion
            if (token.state == REQUESTED)
            {
                int st = svcTaller.getState(token.token);
                if (st == -1)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "El token no ha sido activado");
                }
                else
                {
                    token.state = ACTIVE;
                    c_bd.SaveChanges();
                    return (new { Token = token.token, Status = token.state, Created = token.timeStamp });

                }
            }
            else if (token.state == ACTIVE)
            {
                return (new { Token = token.token, Status = token.state, Created = token.timeStamp });
            }
            return Request.CreateResponse(HttpStatusCode.Forbidden, "El token ha expirado");
        }

        private void setLastTokenAsExpered()
        {
            if (c_bd.TokensSet.Count() > 0)
            {
                var tokens = (from d in c_bd.TokensSet select d);
                foreach (var token in tokens)
                {
                    token.state = EXPIRED;
                }
                c_bd.SaveChanges();
            }
        }
    }
}
