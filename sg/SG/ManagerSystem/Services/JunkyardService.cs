using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class JunkyardService : IDisposable
    {
        private UnitOfWork unit_of_work = null;
        private UnitOfWork unitOfWork {
            get {
                if (this.unit_of_work == null)
                    this.unit_of_work = new UnitOfWork();
                return this.unit_of_work;
            }
        }

        private TokenService token_service = null;
        private TokenService tokenService {
            get {
                if (this.token_service == null)
                    this.token_service = new TokenService(unit_of_work);
                return this.token_service;
            }
        }

        public JunkyardService() { }

        public JunkyardService(UnitOfWork uOW)
        {
            this.unit_of_work = uOW;
        }

        public TokenResponse createJunkyard(ExpDesguace e_junkyard)
        {
            if (e_junkyard != null)
            {
                JunkyardEntity junkyard = new JunkyardEntity();
                junkyard.name = e_junkyard.name;
                junkyard.tokens.Add(tokenService.createToken(TokenType.TEMPORAL));

                unitOfWork.JunkyardRepository.Insert(junkyard);

                unitOfWork.Save();

                return new TokenResponse(junkyard.current_token, TokenResponse.Code.ACCEPTED);
            }
            return new TokenResponse("", TokenResponse.Code.BAD_REQUEST);
        }

        //public TokenResponse 

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}