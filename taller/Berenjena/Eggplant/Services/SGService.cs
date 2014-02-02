using Eggplant.ServiceTaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Services
{
    public class SGService : ISGService
    {
        private GestionTallerClient svcTaller;
        public SGService()
        {
            svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
        }

        public TokenResponse signUp(string name)
        {
            ExpTaller et = new ExpTaller();
            et.name = name;
            return svcTaller.signUp(et);
        }

        public TokenResponse getState(string token)
        {
            return svcTaller.getState(token);
        }

    }
}