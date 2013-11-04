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
        int addNewDesguace(string nombre);

        [OperationContract]
        Desguace getById(int desguaceId);

        /*
        [OperationContract]
        List<Desguace> getAll();

        [OperationContract]
        int activateDesguace(int desguaceId);*/
    }
}
