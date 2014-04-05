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

        public TokenResponse validateJunkyardToken(string token_string)
        {
            if (token_string == null || token_string == "")
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            JunkyardTokenEntity token;
            try
            {
                token = unitOfWork.JunkyardTokenRepository.Get(t => t.token == token_string).First();
            }
            catch (InvalidOperationException)
            {
                return new TokenResponse("", TokenResponse.Code.NOT_FOUND);
            }

            if (token.status == TokenStatus.EXPIRED)
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            if (token.type == TokenType.FINAL)
                return new TokenResponse(token.token, TokenResponse.Code.CREATED);

            TokenResponse.Code code;

            token.status = TokenStatus.EXPIRED;
            JunkyardTokenEntity new_token;
            if (token.junkyard.status == JunkyardStatus.ACTIVE)
            {
                new_token = this.createJunkyardToken(TokenType.FINAL);
                code = TokenResponse.Code.CREATED;
            }
            else
            {
                new_token = this.createJunkyardToken(TokenType.TEMPORAL);
                code = TokenResponse.Code.NON_AUTHORITATIVE;
            }
            new_token.junkyard = token.junkyard;
            unitOfWork.JunkyardTokenRepository.Insert(new_token);

            unitOfWork.Save();

            return new TokenResponse(new_token.token, code);
        }

        public TokenResponse validateGarageToken(string token_string)
        {
            if (token_string == null || token_string == "")
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            GarageTokenEntity token;
            try
            {
                token = unitOfWork.GarageTokenRepository.Get(t => t.token == token_string).First();
            }
            catch (InvalidOperationException)
            {
                return new TokenResponse("", TokenResponse.Code.NOT_FOUND);
            }

            if (token.status == TokenStatus.EXPIRED)
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            if (token.type == TokenType.FINAL)
                return new TokenResponse(token.token, TokenResponse.Code.CREATED);

            TokenResponse.Code code;

            token.status = TokenStatus.EXPIRED;
            GarageTokenEntity new_token;
            if (token.garage.status == GarageStatus.ACTIVE)
            {
                new_token = this.createGarageToken(TokenType.FINAL);
                code = TokenResponse.Code.CREATED;
            }
            else
            {
                new_token = this.createGarageToken(TokenType.TEMPORAL);
                code = TokenResponse.Code.NON_AUTHORITATIVE;
            }
            new_token.garage = token.garage;
            unitOfWork.GarageTokenRepository.Insert(new_token);

            unitOfWork.Save();

            return new TokenResponse(new_token.token, code);
        }

        public JunkyardTokenEntity createJunkyardToken(TokenType type = TokenType.TEMPORAL)
        {
            JunkyardTokenEntity new_token = new JunkyardTokenEntity();

            string token_string = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token_string = token_string.Replace("=", "");
            token_string = token_string.Replace("+", "");

            new_token.token = token_string;
            new_token.type = type;
            new_token.status = TokenStatus.VALID;

            return new_token;
        }

        public GarageTokenEntity createGarageToken(TokenType type = TokenType.TEMPORAL)
        {
            GarageTokenEntity new_token = new GarageTokenEntity();

            new_token.token = this.createToken();
            new_token.type = type;
            new_token.status = TokenStatus.VALID;

            return new_token;
        }

        private string createToken()
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Guid.NewGuid().ToByteArray();
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        public bool isValid(string token)
        {
            return unitOfWork.JunkyardTokenRepository.Get(t => t.token == token && t.status == TokenStatus.VALID).Any()
                || unitOfWork.GarageTokenRepository.Get(t => t.token == token && t.status == TokenStatus.VALID).Any();
        }

        public GarageEntity getGarage(string token_string)
        {
            if (token_string == null)
                throw new ArgumentNullException();

            GarageTokenEntity token;
            try
            {
                token = unitOfWork.GarageTokenRepository.Get(t => t.token == token_string).First();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is InvalidOperationException)
                    throw new ArgumentException();
                throw;
            }

            return token.garage;
        }

        public JunkyardEntity getJunkyard(string token_string)
        {
            if (token_string == null)
                throw new ArgumentNullException();

            JunkyardTokenEntity token;
            try
            {
                token = unitOfWork.JunkyardTokenRepository.Get(t => t.token == token_string).First();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is InvalidOperationException)
                    throw new ArgumentException();
                throw;
            }

            return token.junkyard;
        }
    }
}