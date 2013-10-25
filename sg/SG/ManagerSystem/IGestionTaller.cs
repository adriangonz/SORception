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
        Taller addNewTaller(string nombre);

        
    }

    [DataContract]
    public class Taller
    {
        int id;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string nombre;

        [DataMember]
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
    }
}
