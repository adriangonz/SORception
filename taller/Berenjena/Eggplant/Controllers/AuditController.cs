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
using Eggplant.Entity;
using Eggplant.DataAcces;

namespace Eggplant.Controllers
{
    [Authorize]
    public class AuditController : ApiController
    {
        private EggplantContext db = new EggplantContext();

        // GET api/Audit
        public IQueryable<Audit> GetAuditSet()
        {
            return db.AuditSet;
        }

        // GET api/Audit/5
        [ResponseType(typeof(Audit))]
        public IHttpActionResult GetAudit(int id)
        {
            Audit audit = db.AuditSet.Find(id);
            if (audit == null)
            {
                return NotFound();
            }

            return Ok(audit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuditExists(int id)
        {
            return db.AuditSet.Count(e => e.id == id) > 0;
        }
    }
}