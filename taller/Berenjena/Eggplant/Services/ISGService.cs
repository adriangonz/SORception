using Eggplant.ServiceTaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Services
{
    public interface ISGService
    {
        TokenResponse signUp(string name);

        TokenResponse getState(string token);

        int addSolicitud(ExpSolicitud exSol);

        ExpSolicitud getSolicitud(int id);

        List<ExpOferta> getOfertas(int idSolicitud);

        int selectOferta(ExpPedido pedido);
    }
}