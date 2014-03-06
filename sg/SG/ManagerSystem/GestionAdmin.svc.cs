using ManagerSystem.Entities;
using ManagerSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ManagerSystem
{
    public class GestionAdmin : IGestionAdmin
    {
        private JunkyardService junkyard_service = null;
        private JunkyardService junkyardService
        {
            get
            {
                if (this.junkyard_service == null)
                    this.junkyard_service = new JunkyardService();
                return this.junkyard_service;
            }
        }

        private GarageService garage_service = null;
        private GarageService garageService
        {
            get
            {
                if (this.garage_service == null)
                    this.garage_service = new GarageService();
                return this.garage_service;
            }
        }

        public List<JunkyardEntity> getDesguaces()
        {
            return junkyardService.getJunkyards();
        }

        public List<GarageEntity> getTalleres()
        {
            List<GarageEntity> garages = garageService.getGarages();
            return garages;
        }

        public int activeDesguace(int id, bool active)
        {
            junkyardService.activateJunkyard(id, active);
            return 0;
        }

        public int activeTaller(int id, bool active)
        {
            garageService.activateGarage(id, active);
            return 0;
        }

        public int deleteTaller(int id)
        {
            try
            {
                garageService.removeGarage(id);
                return 0;
            }
            catch (ArgumentNullException)
            {
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
            }
        }

        public int deleteDesguace(int id)
        {
            try 
	        {
                junkyardService.removeJunkyard(id);
                return 0;
	        }
            catch (ArgumentNullException)
            {
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
	        }
        }
    }
}
