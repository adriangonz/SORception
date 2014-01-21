using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IGestionDesguace" en el código y en el archivo de configuración a la vez.
    [ServiceContract(Namespace = Constants.Namespace)]
    public interface IGestionDesguace
    {
        [OperationContract]
        TokenResponse signUp(ExpDesguace d);

        [OperationContract]
        TokenResponse getState(string id);

        [OperationContract]
        void dummy(AMQSolicitudMessage s, AMQOfertaMessage o, AMQPedidoMessage p);

        Oferta processAMQMessage(AMQOfertaMessage message);
    }
}
