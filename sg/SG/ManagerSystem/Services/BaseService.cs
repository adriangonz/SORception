using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class BaseService : IDisposable
    {
        private UnitOfWork unit_of_work = null;
        protected UnitOfWork unitOfWork
        {
            get
            {
                if (this.unit_of_work == null)
                    this.unit_of_work = new UnitOfWork();
                return this.unit_of_work;
            }
        }

        public BaseService(UnitOfWork uOW = null)
        {
            this.unit_of_work = uOW;
        }

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}