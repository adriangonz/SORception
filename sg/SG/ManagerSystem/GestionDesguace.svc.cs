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
        private managersystemEntities db_context;
        private RDesguace r_desguace;
        private RToken r_token;
        private ROferta r_oferta;

        public GestionDesguace()
        {
            init(new managersystemEntities());
        }

        public GestionDesguace(managersystemEntities context)
        {
            init(context);
        }

        private void init(managersystemEntities context)
        {
            db_context = context;
            r_desguace = new RDesguace(db_context);
            r_token = new RToken(db_context);
            r_oferta = new ROferta(db_context);
        }

        public TokenResponse signUp(ExpDesguace ed)
        {
            if (ed != null)
            {
                Desguace d = r_desguace.FromExposed(ed);
                d.active = false;

                Token t = r_token.getToken();
                d.Tokens.Add(t);

                r_desguace.InsertOrUpdate(d);
                r_desguace.Save();
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
                Token t = r_token.Find(token);
                if (t != null)
                {
                    if (t.is_valid)
                    {
                        Desguace d = r_desguace.Find(t.Desguace.Id);
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
                        new_token = r_token.RegenerateToken(t);
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
            
            o = new AMQOfertaMessage();

            o.desguace_id = "d1fb28d93a179cf2efeb146cc09099b02bbcbabc625bf7f69b5be1722ecf443d";
            o.oferta = new ExpOferta();
            o.oferta.id_en_desguace = 123;
            o.oferta.solicitud_id = 19;
            o.oferta.lineas = new List<ExpOferta.Line>();
            ExpOferta.Line linea = new ExpOferta.Line();
            linea.id_en_desguace = 123;
            linea.linea_solicitud_id = 10;
            linea.price = 123;
            linea.quantity = 100;
            linea.notes = "";
            o.oferta.lineas.Add(linea);
            
            Oferta of = processAMQMessage(o);
            GestionTaller gt = new GestionTaller(db_context);
            gt.checkAutoBuy(of);
        }

        public Oferta processAMQMessage(AMQOfertaMessage message)
        {
            Desguace d = r_desguace.Find(message.desguace_id);
            int id_en_desguace = message.oferta.id_en_desguace;
            Oferta o = null;
            switch (message.code)
            {
                case AMQOfertaMessage.Code.New:
                    o = r_oferta.FromExposed(message.oferta, d);
                    r_oferta.InsertOrUpdate(o);
                    r_oferta.Save();
                    break;
                case AMQOfertaMessage.Code.Update:
                    o = db_context.OfertaSet.First(of => of.id_en_desguace == id_en_desguace);
                    r_oferta.UpdateFromExposed(o, message.oferta);
                    r_oferta.InsertOrUpdate(o);
                    r_oferta.Save();
                    break;
                case AMQOfertaMessage.Code.Delete:
                    o = db_context.OfertaSet.First(of => of.id_en_desguace == id_en_desguace);
                    r_oferta.Delete(o.Id);
                    r_oferta.Save();
                    break;
            }
            return o;
        }
    }
}
