using Eggplant.DataAcces;
using Eggplant.Entity;
using Eggplant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Services
{
    public class DataService : IDataService
    {
        private EggplantContext context;
        private TokenRepository tokens;
        private GenericRepository<Solicitud> solicitudes;
        private GenericRepository<LineaSolicitud> lineasSolicitud;
        
        public DataService()
        {
            context = new EggplantContext();
        }

        public TokenRepository Tokens
        {
            get
            {
                if (tokens == null)
                    tokens = new TokenRepository(context);
                return tokens;
            }
        }

        public GenericRepository<Solicitud> Solicitudes
        {
            get
            {
                if (solicitudes == null)
                    solicitudes = new GenericRepository<Solicitud>(context);
                return solicitudes;
            }
        }

        public GenericRepository<LineaSolicitud> LineasSolicitud
        {
            get
            {
                if (lineasSolicitud == null)
                    lineasSolicitud = new GenericRepository<LineaSolicitud>(context);
                return lineasSolicitud;
            }
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        
    }
}