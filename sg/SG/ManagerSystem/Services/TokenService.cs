﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagerSystem.Entities;
using ManagerSystem.DataAccess;

namespace ManagerSystem.Services
{
    public class TokenService : BaseService
    {
        public TokenService(UnitOfWork uow = null) : base(uow) { }

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

            if (token.junkyard == null && token.garage == null)
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            if (token.status == TokenStatus.EXPIRED)
                return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);

            if (token.type == TokenType.FINAL)
                return new TokenResponse(token.token, TokenResponse.Code.CREATED);

            TokenResponse.Code code;

            token.status = TokenStatus.EXPIRED;
            TokenEntity new_token;
            if (token.junkyard != null && token.junkyard.status == JunkyardStatus.ACTIVE 
                || token.garage != null && token.garage.status == GarageStatus.ACTIVE)
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
            new_token.garage = token.garage;
            unitOfWork.TokenRepository.Insert(new_token);

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

        public bool isValid(string token)
        {
            return unitOfWork.TokenRepository.Get(t => t.token == token && t.status == TokenStatus.VALID).Any();
        }

        public GarageEntity getGarage(string token_string)
        {
            if (token_string == null)
                throw new ArgumentNullException();

            TokenEntity token;
            try
            {
                token = unitOfWork.TokenRepository.Get(t => t.token == token_string).First();
                if (token.garage == null)
                    throw new ArgumentNullException();
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException();
            }

            return token.garage;
        }
    }
}