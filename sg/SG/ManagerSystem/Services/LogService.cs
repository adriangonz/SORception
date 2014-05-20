using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class LogService : BaseService
    {
        public LogService(UnitOfWork uow = null) : base(uow) { }

        public void Log(string msg, LogLevel level)
        {
            LogEntity log = new LogEntity()
            {
                message = msg,
                level = level,
                user = authService.getCurrentUser()
            };
            unitOfWork.LogRepository.Insert(log);
            unitOfWork.Save();
        }

        public void Info(string msg)
        {
            this.Log(msg, LogLevel.INFO);
        }

        public void Warning(string msg)
        {
            this.Log(msg, LogLevel.WARNING);
        }

        public void Error(string msg)
        {
            this.Log(msg, LogLevel.ERROR);
        }

        public void Critial(string msg)
        {
            this.Log(msg, LogLevel.CRITICAL);
        }

    }
}