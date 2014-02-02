using Eggplant.Exceptions;
using Eggplant.ServiceTaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Eggplant.Services
{
    public class DebugSGService : ISGService
    {
        public TokenResponse signUp(string et)
        {
            return new TokenResponse();
            //throw new ApplicationLayerException(HttpStatusCode.BadRequest ,"signUpDebug no implementada");
        }

        public TokenResponse getState(string token)
        {
            var t = new TokenResponse();
            t.status = TokenResponseCode.CREATED;
            return t;
            //throw new ApplicationLayerException(HttpStatusCode.BadRequest, "getStateDebug no implementada");
        }

        public int addSolicitud(ExpSolicitud exSol)
        {
            return 0;
        }

        public ExpSolicitud getSolicitud(int id)
        {
            return new ExpSolicitud();
        }
    }
}