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

            /*using (BDBerenjenaContainer c_bd = new BDBerenjenaContainer())
            {
                Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
                Tokens t = c_bd.TokensSet.AsQueryable().FirstOrDefault(x => x.state == "ACTIVE");
                string token = "";
                if (t != null) token = t.token;
                using (OperationContextScope scope = new OperationContextScope(svcTaller.InnerChannel))
                {
                    MessageHeader header = MessageHeader.
                        CreateHeader("Authorization", "http://sorception.azurewebsites.net/", token);
                    OperationContext.Current.OutgoingMessageHeaders.Add(header);

                }
            }*/
             MessageHeader header = MessageHeader.
                        CreateHeader("Authorization", "http://sorception.azurewebsites.net/", "COCAINUM");
             request.Headers.Add(header);
            /*
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;
            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                if (string.IsNullOrEmpty(httpRequestMessage.Headers["Authorization"]))
                {
                    httpRequestMessage.Headers["Authorization"] = "COAINUM";
                }
            }
            else
            {
                httpRequestMessage = new HttpRequestMessageProperty();
                httpRequestMessage.Headers.Add("Authorization", "COAINUM");
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            }*/
            return null;
        }
        /*
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            using (BDBerenjenaContainer c_bd = new BDBerenjenaContainer())
            {
                Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();
                Tokens t = c_bd.TokensSet.AsQueryable().FirstOrDefault(x => x.state == "ACTIVE");
                string token = "";
                if (t != null) token = t.token;
                using (OperationContextScope scope = new OperationContextScope(svcTaller.InnerChannel))
                {
                    MessageHeader header = MessageHeader.
                        CreateHeader("Authorization", "http://sorception.azurewebsites.net/", "token");
                    OperationContext.Current.OutgoingMessageHeaders.Add(header);

                }
            }
        }*/
    }
}