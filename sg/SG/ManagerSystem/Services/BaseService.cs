using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class BaseService : IDisposable
    {
        private UnitOfWork unit_of_work = null;
        protected UnitOfWork unitOfWork
        {
            get
            {
                if (this.unit_of_work == null)
                    this.unit_of_work = new UnitOfWork();
                return this.unit_of_work;
            }
        }

        /*
         * Services offered by the application
         * */

        private TokenService token_service = null;
        protected TokenService tokenService
        {
            get
            {
                if (this.token_service == null)
                    this.token_service = new TokenService(unitOfWork);
                return this.token_service;
            }
        }

        private OfferService offer_service = null;
        protected OfferService offerService
        {
            get
            {
                if (this.offer_service == null)
                    this.offer_service = new OfferService(unitOfWork);
                return this.offer_service;
            }
        }

        private AuthorizationService authorization_service = null;
        protected AuthorizationService authorizationService
        {
            get
            {
                if (this.authorization_service == null)
                    this.authorization_service = new AuthorizationService(unitOfWork);
                return this.authorization_service;
            }
        }

        private GarageService garage_service = null;
        protected GarageService garageService
        {
            get
            {
                if (this.garage_service == null)
                    this.garage_service = new GarageService(unitOfWork);
                return this.garage_service;
            }
        }

        public BaseService(UnitOfWork uOW = null)
        {
            this.unit_of_work = uOW;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.unitOfWork.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}