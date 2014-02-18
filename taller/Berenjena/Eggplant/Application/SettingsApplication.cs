using Eggplant.Exceptions;
using Eggplant.Services;
using Eggplant.ServiceTaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Eggplant.Application
{
    public class SettingsApplication : AbstractApplication
    {
        public IEnumerable<Entity.Token> getAll()
        {
            updateTokensStatus();
            return dataService.Tokens.Get();
        }

        public int Request(string tallerName)
        {
            Entity.Token t = new Entity.Token();
            t.status = Entity.Token.REQUESTED;

            t.token = sgService.signUp(tallerName).token;

            setLastTokenAsExpired();

            dataService.Tokens.Insert(t);
            return dataService.SaveChanges();
        }

        private void updateTokensStatus()
        {
            var token = dataService.Tokens.Get().OrderByDescending(x => x.creationDate).FirstOrDefault();
            if (token != null)
            {
                if (token.status == Entity.Token.REQUESTED)
                {
                    TokenResponse tr = sgService.getState(token.token);
                    if (tr.status == TokenResponseCode.NON_AUTHORITATIVE)
                    {
                        token.token = tr.token;
                        dataService.SaveChanges();
                    }
                    else if (tr.status == TokenResponseCode.CREATED)
                    {
                        token.status = Entity.Token.ACTIVE;
                        token.token = tr.token;
                        dataService.SaveChanges();
                    }
                    else
                    {
                        throw new ApplicationLayerException(HttpStatusCode.BadRequest, "Algo ha ido mal en el SG al comprobar el estado del token: " + tr.status);
                    }
                }
            }
        }

        private void setLastTokenAsExpired()
        {
            dataService.Tokens.updateStatus(Entity.Token.EXPIRED);
            dataService.SaveChanges();
        }
    }
}