using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ScrapWeb.Entities;
using ScrapWeb.DataAccess;

namespace ScrapWeb.Controllers
{
    public class LogsController : ApiController
    {
        private ScrapContext db = new ScrapContext();

        // GET api/Logs
        public IQueryable<LogEntity> GetLog()
        {
            return db.Log;
        }

        // GET api/Logs/5
        [ResponseType(typeof(LogEntity))]
        public IHttpActionResult GetLogEntity(int id)
        {
            LogEntity logentity = db.Log.Find(id);
            if (logentity == null)
            {
                return NotFound();
            }

            return Ok(logentity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogEntityExists(int id)
        {
            return db.Log.Count(e => e.id == id) > 0;
        }
    }
}