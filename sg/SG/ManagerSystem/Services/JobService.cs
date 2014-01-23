using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class JobService : BaseService
    {
        public JobService(UnitOfWork uow = null) : base(uow) { }

    }
}