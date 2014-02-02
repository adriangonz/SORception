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
            throw new ApplicationLayerException(HttpStatusCode.BadRequest ,"signUpDebug no implementada");
        }

        public TokenResponse getState(string token)
        {
            throw new ApplicationLayerException(HttpStatusCode.BadRequest, "getStateDebug no implementada");
        }
    }
}