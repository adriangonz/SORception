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
        //BDBerenjenaContainer c_bd = EggplantContextFactory.getContext();
        Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
        public static string REQUESTED = "REQUESTED";
        public static string EXPIRED = "EXPIRED";
        public static string ACTIVE = "ACTIVE";

        // GET api/settings
        public object Get()
        {
            GetToken();
            /*
            using (BDBerenjenaContainer c_bd = new BDBerenjenaContainer())
            {
                if (c_bd.TokensSet.Count() <= 0) return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se ha solicitado token");

                var tokens = c_bd.TokensSet.AsQueryable().ToList();
                //var tokens = c_bd.TokensSet.(from d in c_bd.TokensSet orderby d.timeStamp descending select d);

                return (new { name = "TUCARA", tokens });
            }*/
            return null;
        }

        // POST api/settings
        public object Post([FromBody]JObject value)
        {
            /*
            using (BDBerenjenaContainer c_bd = new BDBerenjenaContainer())
            {
                setLastTokenAsExpired();
                Tokens to = new Tokens();
                ExpTaller extTaller = new ExpTaller();
                extTaller.name = value["nombre"].ToString();
                to.token = svcTaller.signUp(extTaller).token;
                to.timeStamp = DateTime.Now;
                to.state = REQUESTED;
                c_bd.TokensSet.Add(to);
                c_bd.SaveChanges();

                return to;
            }*/
            return null;
        }

        // PUT api/settings
        public void Put([FromBody]JObject value)
        {
            /*
            using (BDBerenjenaContainer c_bd = new BDBerenjenaContainer())
            {
                var token = (from d in c_bd.TokensSet orderby d.timeStamp select d).First();
                ExpTaller t = new ExpTaller();
                t.name = value["nombre"].ToString();
                svcTaller.putTaller(t);
            }*/
        }

        // DELETE api/settings
        public void Delete()
        {
            svcTaller.deleteTaller();
        }

        // GET api/settings/token
        [Route("api/settings/token")]
        public object GetToken()
        {
            /*
            using (BDBerenjenaContainer c_bd = new BDBerenjenaContainer())
            {
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
                        return Request.CreateResponse(HttpStatusCode.NoContent, "El token no ha sido activado");
                    }
                    else if (tr.status == TokenResponseCode.CREATED)
                    {
                        token.state = ACTIVE;
                        token.token = tr.token;
                        token.timeStamp = DateTime.Now;
                        c_bd.SaveChanges();
                        return (new { Token = token.token, Status = token.state, Created = token.timeStamp });

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Algo ha ido mal en el SG: "+tr.status);
                    }
                }
                else if (token.state == ACTIVE)
                {
                    return (new { Token = token.token, Status = token.state, Created = token.timeStamp });
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden, "El token ha expirado");
            }*/
            return null;
        }

        private void setLastTokenAsExpired()
        {
            /*
            using (BDBerenjenaContainer c_bd = new BDBerenjenaContainer())
            {
            
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
                
            }*/
        }
    }
}
