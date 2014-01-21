using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagerSystem.Entities;
using ManagerSystem.DataAccess;

namespace ManagerSystem.Services
{
    public class TokenService : IDisposable
    {

        private UnitOfWork unit_of_work = null;
        private UnitOfWork unitOfWork
        {
            get
            {
                if (this.unit_of_work == null)
                    this.unit_of_work = new UnitOfWork();
                return this.unit_of_work;
            }
        }

        public TokenService() { }

        public TokenService(UnitOfWork unitOfWork)
        {
            this.unit_of_work = unitOfWork;
        }

        public TokenResponse validateToken(string token_string)
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
                new_token = this.createToken(TokenType.FINAL);
                code = TokenResponse.Code.CREATED;
            }
            else
            {
                new_token = this.createToken(TokenType.TEMPORAL);
                code = TokenResponse.Code.NON_AUTHORITATIVE;
            }
            new_token.junkyard = token.junkyard;

            unitOfWork.Save();

            return new TokenResponse(token.token, code);
        }

        public TokenEntity createToken(TokenType type = TokenType.TEMPORAL)
        {
            TokenEntity new_token = new TokenEntity();

            string token_string = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token_string = token_string.Replace("=", "");
            token_string = token_string.Replace("+", "");

            new_token.token = token_string;
            new_token.type = type;
            new_token.status = TokenStatus.VALID;

            return new_token;
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}