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
    }
}