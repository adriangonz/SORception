using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    [ServiceContract]
    public interface IGestionTaller
    {
        [OperationContract]
        TokenResponse signUp(ExpTaller et);

        [OperationContract]
        TokenResponse getState(string token);

        [OperationContract]
        int putTaller(ExpTaller t);

        [OperationContract]
        int deleteTaller();

        [OperationContract]
        ExpSolicitud getSolicitud(int id);

        [OperationContract]
        List<ExpSolicitud> getSolicitudes();

        [OperationContract]
        int addSolicitud(ExpSolicitud s);

        [OperationContract]
        int putSolicitud(ExpSolicitud s);

        [OperationContract]
        int deleteSolicitud(int id);

        [OperationContract]
        List<ExpOferta> getOfertas(int solicitud);

        [OperationContract]
        ExpOferta getOferta(int oferta);

        [OperationContract]
        int selectOferta(ExpPedido p);

        void runJob(AMQScheduledJob job);
    }
}
