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
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
            return 0;
        }


    }
}
