using Eggplant.DataAcces;
using Eggplant.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.Http.Filters;

namespace Eggplant.Filters
{
    public class WCFAuthorizationHeader : IClientMessageInspector
    {
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {

            using (EggplantContext c_bd = new EggplantContext())
            {
                Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
                Token t = c_bd.TokensSet.AsQueryable().FirstOrDefault(x => x.status == Token.ACTIVE);
                if (t != null)
                {
                    MessageHeader header = MessageHeader.
                           CreateHeader("Authorization", "http://sorception.azurewebsites.net/", t.token);
                    request.Headers.Add(header);
                }
            
            }
            return null;
        }
        
    }
}