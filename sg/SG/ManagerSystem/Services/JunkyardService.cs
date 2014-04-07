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
                junkyard.aes_pair = aesService.createAESPair(e_junkyard.aes_key, e_junkyard.aes_iv);
                junkyard.tokens.Add(tokenService.createTemporalToken());

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
            JunkyardEntity junkyard = this.getJunkyard(junkyard_id);

            junkyard.status = is_active ? JunkyardStatus.ACTIVE : JunkyardStatus.CREATED;

            unitOfWork.JunkyardRepository.Update(junkyard);
            unitOfWork.Save();
        }

        public void removeJunkyard(int junkyard_id)
        {
            JunkyardEntity junkyard = this.getJunkyard(junkyard_id);

            unitOfWork.JunkyardRepository.Delete(junkyard);
            unitOfWork.Save();
        }

        public JunkyardEntity getJunkyardWithToken(string token_string)
        {
            return tokenService.getJunkyard(token_string);
        }

        public ExpDesguace toExposed(JunkyardEntity junkyard)
        {
            return new ExpDesguace
            {
                name = junkyard.name,
                id = junkyard.id,
                active = junkyard.status == JunkyardStatus.ACTIVE
            };
        }
    }
}