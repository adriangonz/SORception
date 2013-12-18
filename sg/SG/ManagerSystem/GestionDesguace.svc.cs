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

    [DataContract(Namespace = Constants.Namespace)]
    public class TokenResponse
    {
        public enum Code : int { CREATED = 201, ACCEPTED = 202, NON_AUTHORITATIVE = 203, BAD_REQUEST = 400, NOT_FOUND = 404 };

        [DataMember]
        public string token;

        [DataMember]
        public Code status;

        public TokenResponse(string token, TokenResponse.Code status)
        {
            this.token = token;
            this.status = status;
        }
    }

    [ServiceBehavior(Namespace = Constants.Namespace)]
    public class GestionDesguace : IGestionDesguace
    {
        public TokenResponse signUp(ExposedDesguace ed)
        {
            if (ed != null)
            {
                Desguace d = DesguaceRepository.FromExposed(ed);
                d.active = false;

                Token t = TokenRepository.getToken();
                d.Tokens.Add(t);

                DesguaceRepository.InsertOrUpdate(d);
                DesguaceRepository.Save();
                return new TokenResponse(t.token, TokenResponse.Code.ACCEPTED);
            }
            return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);
        }

        public TokenResponse getState(string token)
        {
            string new_token = "";
            TokenResponse.Code status;
            if (token != null && token != "")
            {
                Token t = TokenRepository.Find(token);
                if (t != null)
                {
                    if (t.is_valid)
                    {
                        Desguace d = t.Desguace;
                        if (d.active)
                        {
                            // El desgauce ya esta activo
                            status = TokenResponse.Code.CREATED;
                        }
                        else
                        {
                            // EL desguace no esta activo
                            status = TokenResponse.Code.NON_AUTHORITATIVE;
                        }
                        new_token = TokenRepository.RegenerateToken(t);
                    }
                    else
                    {
                        // EL token ha expirado
                        status = TokenResponse.Code.BAD_REQUEST;
                    }
                }
                else
                {
                    // El token no existe
                    status = TokenResponse.Code.NOT_FOUND;
                }
            }
            else
            {
                // No se le ha pasado un token
                status = TokenResponse.Code.BAD_REQUEST;
            }

            return new TokenResponse(new_token, status);
        }
    }
}
