﻿using Eggplant.Entity;
using Eggplant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eggplant.Services
{
    public interface IDataService : IDisposable
    {
        TokenRepository Tokens { get; }

        SolicitudRespository Solicitudes { get; }
        GenericRepository<LineaSolicitud> LineasSolicitud { get; }

        PedidoRepository Pedidos { get; }
        GenericRepository<LineaPedido> LineasPedido { get; }

        int SaveChanges();
    }
}
