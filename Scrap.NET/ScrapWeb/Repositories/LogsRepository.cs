using ScrapWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace ScrapWeb.Repositories
{
    public class LogsRepository : GenericRepository<LogEntity>
    {
        public LogsRepository(DbContext context)
            : base(context)
        {

        }
        public void create(string type, string description)
        {
            LogEntity log = new LogEntity();
            log.type = type;
            log.description = description;
            this.Insert(log);
        }
    }
}