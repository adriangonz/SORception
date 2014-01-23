using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class JunkyardService : BaseService
    {
        public TokenResponse createJunkyard(ExpDesguace e_junkyard)
        {
            if (e_junkyard != null)
            {
                JunkyardEntity junkyard = new JunkyardEntity();
                junkyard.name = e_junkyard.name;
                junkyard.tokens.Add(tokenService.createJunkyardToken(TokenType.TEMPORAL));

                unitOfWork.JunkyardRepository.Insert(junkyard);

                unitOfWork.Save();

                return new TokenResponse(junkyard.current_token, TokenResponse.Code.ACCEPTED);
            }
            return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);
        }
    }
}