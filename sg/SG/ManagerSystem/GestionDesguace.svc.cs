using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ManagerSystem
{
    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedDesguace
    {
        [DataMember]
        public string name;
    }

    [ServiceBehavior(Namespace = Constants.Namespace)]
    public class GestionDesguace : IGestionDesguace
    {
        public string signUp(ExposedDesguace ed)
        {
            if (ed != null)
            {
                Desguace d = DesguaceRepository.FromExposed(ed);
                d.active = false;

                Token t = TokenRepository.getToken();
                d.Tokens.Add(t);

                DesguaceRepository.InsertOrUpdate(d);
                DesguaceRepository.Save();
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Accepted;
                return t.token;
            }

            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
            return "";
        }

        public string getState(string token)
        {
            if (token != null && token != "")
            {
                Token t = TokenRepository.Find(token);
                Desguace d = t.Desguace;
                if (d.active)
                {
                    t.status = "CONSUMED";
                    Token new_token = TokenRepository.getToken();
                    d.Tokens.Add(new_token);
                    DesguaceRepository.Save();

                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created;
                    return new_token.token;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NonAuthoritativeInformation;
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            return "";
        }
    }
}
