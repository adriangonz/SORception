using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class ConfigService : BaseService
    {
        public ConfigService(UnitOfWork uow = null) : base(uow) { }

        public void setAESConfig()
        {
            try {
                unitOfWork.AppConfigRepository.GetAll().First();
            } catch {
                AESPairEntity aes_pair = aesService.generateAppAESPair();
                AppConfig config = new AppConfig();
                config.aes_pair = aes_pair;

                unitOfWork.AppConfigRepository.Insert(config);
                unitOfWork.Save();
            }
        }

        public TokenResponse addAESPair(TokenResponse response)
        {
            AppConfig config = unitOfWork.AppConfigRepository.GetAll().First();
            response.aes_key = config.aes_pair.key;
            response.aes_iv = config.aes_pair.iv;
            return response;
        }

        public AESPairEntity getAESPair()
        {
            AppConfig config = unitOfWork.AppConfigRepository.GetAll().First();
            return config.aes_pair;
        }
    }
}