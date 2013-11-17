using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGestionAdmin" in both code and config file together.
    [ServiceContract]
    public interface IGestionAdmin
    {
        [OperationContract]
        List<Desguace> getDesguaces();

        [OperationContract]
        List<Taller> getTalleres();

        [OperationContract]
        int activeDesguace(int id, bool active);

        [OperationContract]
        int activeTaller(int id, bool active);

        [OperationContract]
        int deleteTaller(int id);

        [OperationContract]
        int deleteDesguace(int id);


    }
}
