using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ManagerSystem
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GestionTaller" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GestionTaller.svc or GestionTaller.svc.cs at the Solution Explorer and start debugging.
    public class GestionTaller : IGestionTaller
    {
        public int addNewTaller(string nombre)
        {
            if (nombre != "")
            {
                try
                {
                    Taller tall = new Taller();
                    tall.name = nombre;
                    TallerRepository.InsertOrUpdate(tall);
                    TallerRepository.Save();
                    return 1;
                }
                catch (Exception e)
                {
                    
                    throw;
                }
            }
            return 0;
        }


    }
}
