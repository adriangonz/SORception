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
using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using ManagerSystem.Services;

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
            init(null);
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
            using (var unitOfWork = new UnitOfWork())
            {
                if (ed != null)
                {
                    JunkyardEntity junkyard = new JunkyardEntity();
                    junkyard.name = ed.name;
                    junkyard.tokens.Add(TokenService.createToken(TokenType.TEMPORAL));

                    unitOfWork.JunkyardRepository.Insert(junkyard);

                    unitOfWork.Save();

                    return new TokenResponse(junkyard.current_token, TokenResponse.Code.ACCEPTED);
                }
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);
            }
        }

        public TokenResponse getState(string token_string)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                if (token_string == null || token_string == "")
                    return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

                TokenEntity token;
                try
                {
                    token = unitOfWork.TokenRepository.Get(t => t.token == token_string).First();
                }
                catch (InvalidOperationException)
                {
                    return new TokenResponse("", TokenResponse.Code.NOT_FOUND);
                }

                if (token.junkyard == null)
                    return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

                if (token.status == TokenStatus.EXPIRED)
                    return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

                if (token.type == TokenType.FINAL)
                    return new TokenResponse(token.token, TokenResponse.Code.CREATED);

                TokenResponse.Code code;

                token.status = TokenStatus.EXPIRED;
                TokenEntity new_token;
                if (token.junkyard.status == JunkyardStatus.ACTIVE)
                {
                    new_token = TokenService.createToken(TokenType.FINAL);
                    code = TokenResponse.Code.CREATED;
                }
                else
                {
                    new_token = TokenService.createToken(TokenType.TEMPORAL);
                    code = TokenResponse.Code.NON_AUTHORITATIVE;
                }
                new_token.junkyard = token.junkyard;

                unitOfWork.Save();

                return new TokenResponse(token.token, code);
            }
        }

        public void dummy(AMQSolicitudMessage s, AMQOfertaMessage o, AMQPedidoMessage p) {
            /*
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
            gt.checkAutoBuy(of);*/
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
