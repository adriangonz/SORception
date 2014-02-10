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
        public JunkyardService(UnitOfWork uow = null) : base(uow) { }

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

        public JunkyardEntity getJunkyard(int junkyard_id)
        {
            JunkyardEntity junkyard = unitOfWork.JunkyardRepository.GetByID(junkyard_id);
            if (junkyard == null)
                throw new ArgumentException();

            return junkyard;
        }

        public List<JunkyardEntity> getJunkyards()
        {
            List<JunkyardEntity> junkyards = unitOfWork.JunkyardRepository.GetAll().ToList();
            return junkyards;
        }

        public void activateJunkyard(int junkyard_id, bool is_active)
        {
            JunkyardEntity junkyard = unitOfWork.JunkyardRepository.GetByID(junkyard_id);
            if (junkyard == null)
                throw new ArgumentException();

            junkyard.status = is_active ? JunkyardStatus.ACTIVE : JunkyardStatus.CREATED;

            unitOfWork.JunkyardRepository.Update(junkyard);
            unitOfWork.Save();
        }

        public void removeJunkyard(int junkyard_id)
        {
            JunkyardEntity junkyard = unitOfWork.JunkyardRepository.GetByID(junkyard_id);

            if (junkyard == null)
                throw new ArgumentNullException();

            unitOfWork.JunkyardRepository.Delete(junkyard);
            unitOfWork.Save();
        }

        public JunkyardEntity getJunkyardWithToken(string token_string)
        {
            return tokenService.getJunkyard(token_string);
        }

        public bool existsJunkyardWithToken(string token_string)
        {
            try
            {
                this.getJunkyardWithToken(token_string);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}