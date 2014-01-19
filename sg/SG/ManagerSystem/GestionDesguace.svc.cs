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
    [ServiceBehavior(Namespace = Constants.Namespace)]
    public class GestionDesguace : IGestionDesguace
    {
        static managersystemEntities ms_ent = new managersystemEntities();

        public TokenResponse signUp(ExpDesguace ed)
        {
            if (ed != null)
            {
                Desguace d = Desguace.FromExposed(ed);
                d.active = false;

                Token t = Token.getToken();
                d.Tokens.Add(t);

                Desguace.InsertOrUpdate(d);
                Desguace.Save();
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
                Token t = Token.Find(token);
                if (t != null)
                {
                    if (t.is_valid)
                    {
                        Desguace d = Desguace.Find(t.Desguace.Id);
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
                        new_token = Token.RegenerateToken(t);
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

        public void dummy(AMQSolicitudMessage s, AMQOfertaMessage o) {
            processAMQMessage(o);
        }

        public void processAMQMessage(AMQOfertaMessage message)
        {
            Desguace d = Desguace.Find(message.desguace_id);
            switch (message.code)
            {
                case AMQOfertaMessage.Code.New:
                    Oferta.InsertOrUpdate(Oferta.FromExposed(message.oferta, d));
                    Oferta.Save();
                    break;
                case AMQOfertaMessage.Code.Update:
                    break;
                case AMQOfertaMessage.Code.Delete: 
                    int id_en_desguace = message.oferta.id_en_desguace;
                    Oferta o = ms_ent.OfertaSet.First(of => of.id_en_desguace == id_en_desguace);
                    Oferta.Delete(o.Id);
                    Oferta.Save();
                    break;
            }
        }
    }
}
