using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGestionTaller" in both code and config file together.
    [ServiceContract]
    public interface IGestionTaller
    {
        [OperationContract]
        ExposedTaller getTaller(int id);

        [OperationContract]
        int addTaller(string nombre);

        [OperationContract]
        int putTaller(ExposedTaller t);

        [OperationContract]
        int deleteTaller(int id);
        
        [OperationContract]
        ExposedSolicitud getSolicitud(int id);

        [OperationContract]
        int addSolicitud(ExposedSolicitud s);

        [OperationContract]
        int putSolicitud(ExposedSolicitud s);

        [OperationContract]
        int deleteSolicitud(int id);

        [OperationContract]
        List<ExposedSolicitud> getSolicitudes();
        /*
        [OperationContract]
        int sendCompra(Respuesta r);*/

        
    }
}
