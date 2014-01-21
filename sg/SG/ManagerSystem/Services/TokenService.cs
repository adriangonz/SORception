using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagerSystem.Entities;
using ManagerSystem.DataAccess;

namespace ManagerSystem.Services
{
    public class TokenService
    {
        public static TokenEntity createToken(TokenType type = TokenType.TEMPORAL)
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
    }
}