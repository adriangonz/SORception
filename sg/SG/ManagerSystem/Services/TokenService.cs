using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagerSystem.Entities;
using ManagerSystem.DataAccess;
using System.Security.Cryptography;
using System.Text;

namespace ManagerSystem.Services
{
    public class TokenService : BaseService
    {
        public TokenService(UnitOfWork uow = null) : base(uow) { }

        public TokenResponse validateGarageToken(string token_string)
        {
            return validateToken(token_string, false);
        }

        public TokenResponse validateJunkyardToken(string token_string)
        {
            return validateToken(token_string, true);
        }

        public TokenResponse validateToken(string token_string, bool is_junkyard)
        {
            if (token_string == null || token_string == "")
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            TokenEntity token;
            try
            {
                token = this.getToken(token_string);
            }
            catch (ArgumentException)
            {
                return new TokenResponse("", TokenResponse.Code.NOT_FOUND);
            }

            if (token.status == TokenStatus.EXPIRED)
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            if (token.type == TokenType.FINAL)
                return new TokenResponse(token.token, TokenResponse.Code.CREATED);

            token.status = TokenStatus.EXPIRED;

            TokenResponse.Code code;
            TokenEntity new_token = new TokenEntity()
            {
                token = this.createTokenString(),
                junkyard = token.junkyard,
                garage = token.garage
            };

            if (token.junkyard != null && token.junkyard.status == JunkyardStatus.ACTIVE
                || token.garage != null && token.garage.status == GarageStatus.ACTIVE)
            {
                new_token.status = TokenStatus.VALID;
                new_token.type = TokenType.FINAL;
                code = TokenResponse.Code.CREATED;
            }
            else
            {
                new_token.status = TokenStatus.VALID;
                new_token.type = TokenType.TEMPORAL;
                code = TokenResponse.Code.NON_AUTHORITATIVE;
            }
            unitOfWork.TokenRepository.Insert(new_token);

            unitOfWork.Save();

            return new TokenResponse(new_token.token, code);

        }

        public TokenEntity createTemporalToken()
        {
            return new TokenEntity()
            {
                token = this.createTokenString(),
                type = TokenType.TEMPORAL
            };
        }

        private string createTokenString()
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Guid.NewGuid().ToByteArray();
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        public bool isValid(string token_string)
        {
            logService.Info("Checking if " + token_string + " is a valid token");
            return unitOfWork.TokenRepository.Get(t => t.token == token_string && t.status == TokenStatus.VALID).Any();
        }

        public TokenEntity getToken(string token_string)
        {
            if (token_string == null)
                throw new ArgumentNullException("token_string must not be null");

            try
            {
                return unitOfWork.TokenRepository.Get(t => t.token == token_string).First();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is InvalidOperationException)
                    throw new ArgumentException(token_string + " is not a valid token");
                throw;
            }
        }

        public GarageEntity getGarage(string token_string)
        {
            TokenEntity token = this.getToken(token_string);
            if (token.garage == null)
            {
                throw new ArgumentException(token_string + " is not a valid garage token");
            }

            return token.garage;
        }

        public JunkyardEntity getJunkyard(string token_string)
        {
            logService.Info("Trying to get junkyard with token " + token_string);
            TokenEntity token = this.getToken(token_string);
            if (token.junkyard == null)
            {
                throw new ArgumentException(token_string + " is not a valid junkyard token");
            }

            return token.junkyard;
        }
    }
}