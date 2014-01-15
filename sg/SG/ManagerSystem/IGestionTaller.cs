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
        TokenResponse signUp(ExposedTaller et);

        [OperationContract]
        TokenResponse getState(string token);

        [OperationContract]
        int putTaller(ExposedTaller t);

        [OperationContract]
        int deleteTaller(string token);

        [OperationContract]
        ExposedSolicitud getSolicitud(int id);

        [OperationContract]
        List<ExposedSolicitud> getSolicitudes();

        [OperationContract]
        int addSolicitud(ExposedSolicitud s);

        [OperationContract]
        int putSolicitud(ExposedSolicitud s);

        [OperationContract]
        int deleteSolicitud(int id);

        [OperationContract]
        List<ExposedOferta> getOfertas(int solicitud);

        [OperationContract]
        int selectOferta(TallerResponse r);                
    }
}
