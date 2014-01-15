using Eggplant;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;

namespace Berenjena.Controllers
{
    public class SettingsController : ApiController
    {
        BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
        public static string REQUESTED = "REQUESTED";
        public static string EXPIRED = "EXPIRED";
        public static string ACTIVE = "ACTIVE";
        
        // GET api/settings
        public object Get()
        {
            if (c_bd.TokensSet.Count() <= 0) return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se ha solicitado token");

            var tokens = c_bd.TokensSet.AsQueryable().ToList();
            //var tokens = c_bd.TokensSet.(from d in c_bd.TokensSet orderby d.timeStamp descending select d);

            return (new { name = "TUCARA", tokens });
            
        }

        // POST api/settings
        public object Post([FromBody]JObject value)
        {
            setLastTokenAsExpired();
            Tokens to = new Tokens();
            ExposedTaller extTaller = new ExposedTaller();
            extTaller.name = value["nombre"].ToString();
            to.token = svcTaller.signUp(extTaller).token;
            to.timeStamp = DateTime.Now;
            to.state = REQUESTED;
            c_bd.TokensSet.Add(to);
            c_bd.SaveChanges();

            return to;
        }

        // PUT api/settings
        public void Put([FromBody]JObject value)
        {
            var token = (from d in c_bd.TokensSet orderby d.timeStamp select d).First();
            ExposedTaller t = new ExposedTaller();
            t.name = value["nombre"].ToString();
            svcTaller.putTaller(t);
        }

        // DELETE api/settings
        public void Delete()
        {
            var token = (from d in c_bd.TokensSet orderby d.timeStamp select d).First();
            svcTaller.deleteTaller(token.token);
        }

        // GET api/settings/token
        [Route("api/settings/token")]
        public object GetToken() {
            if (c_bd.TokensSet.Count() <= 0) return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se ha solicitado token");
            var token = c_bd.TokensSet.OrderByDescending(x => x.timeStamp).FirstOrDefault();//(from d in c_bd.TokensSet orderby d.timeStamp descending select d).First();
            // Si el token esta pendiente de activacion
            if (token.state == REQUESTED)
            {

                TokenResponse tr = svcTaller.getState(token.token);
                   
                if (tr.status == TokenResponseCode.NON_AUTHORITATIVE)
                {
                    token.token = tr.token;
                    token.timeStamp = DateTime.Now;
                    c_bd.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.NotFound, "El token no ha sido activado");
                }
                else if (tr.status == TokenResponseCode.CREATED)
                {
                    token.state = ACTIVE;
                    token.token = tr.token;
                    token.timeStamp = DateTime.Now;
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

        private void setLastTokenAsExpired()
        {
            
            /*if (c_bd.TokensSet.Any())
            {*/
            try
            {
                var tokens = (from d in c_bd.TokensSet select d);
                foreach (var token in tokens)
                {
                    token.state = EXPIRED;
                }
                c_bd.SaveChanges();
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
