using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IGestionDesguace" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IGestionDesguace
    {
        [OperationContract]
        int addDesguace(string nombre);

        [OperationContract]
        Desguace getDesguace(int id);

        [OperationContract]
        int putDesguace(Desguace d);

        [OperationContract]
        int deleteDesgauce(int id);

        /*[OperationContract]
        List<Solicitud> getSolicitudes();

        [OperationContract]
        int addOferta(Oferta o);

        [OperationContract]
        Oferta getOferta(int id);

        [OperationContract]
        int putOferta(Oferta o);

        [OperationContract]
        int deleteOferta(int id);

        [OperationContract]
        List<Oferta> getOfertas();

        [OperationContract]
        int resolveOferta(int id, bool acepted);*/

    }
}
