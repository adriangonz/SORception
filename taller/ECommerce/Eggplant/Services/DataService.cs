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
        private SolicitudRespository solicitudes;
        private GenericRepository<LineaSolicitud> lineasSolicitud;
        PedidoRepository pedidos;
        private GenericRepository<LineaPedido> lineasPedido;
        private AuditsRepository audits;
        
        public DataService()
        {
            context = new EggplantContext();
        }

        public AuditsRepository Audits
        {
            get
            {
                if (audits == null)
                    audits = new AuditsRepository(context);
                return audits;
            }
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

        public SolicitudRespository Solicitudes
        {
            get
            {
                if (solicitudes == null)
                    solicitudes = new SolicitudRespository(context);
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

        public PedidoRepository Pedidos
        { 
            get
            {
                if (pedidos == null)
                    pedidos = new PedidoRepository(context);
                return pedidos;
            }
        }
        public GenericRepository<LineaPedido> LineasPedido
        {
            get
            {
                if (lineasPedido == null)
                    lineasPedido = new GenericRepository<LineaPedido>(context);
                return lineasPedido;
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