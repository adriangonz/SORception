using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private MSContext context = new MSContext();

        private GenericRepository<AppConfig> app_config_repository;
        private GenericRepository<UserEntity> user_repository;
        private GenericRepository<AESPairEntity> aes_pair_repository;
        private GenericRepository<TokenEntity> token_repository;
        private GenericRepository<GarageEntity> garage_repository;
        private GenericRepository<OrderEntity> order_repository;
        private GenericRepository<OrderLineEntity> order_line_repository;
        private GenericRepository<JunkyardEntity> junkyard_repository;
        private GenericRepository<OfferEntity> offer_repository;
        private GenericRepository<OfferLineEntity> offer_line_repository;
        private GenericRepository<JobEntity> job_repository;

        public GenericRepository<AppConfig> AppConfigRepository
        {
            get
            {
                if (this.app_config_repository == null)
                {
                    this.app_config_repository = new GenericRepository<AppConfig>(context);
                }
                return app_config_repository;
            }
        }

        public GenericRepository<UserEntity> UserRepository
        {
            get
            {
                if (this.user_repository == null)
                {
                    this.user_repository = new GenericRepository<UserEntity>(context);
                }
                return user_repository;
            }
        }

        public GenericRepository<AESPairEntity> AESPairRepository
        {
            get
            {
                if (this.aes_pair_repository == null)
                {
                    this.aes_pair_repository = new GenericRepository<AESPairEntity>(context);
                }
                return aes_pair_repository;
            }
        }

        public GenericRepository<TokenEntity> TokenRepository
        {
            get
            {
                if (this.token_repository == null)
                {
                    this.token_repository = new GenericRepository<TokenEntity>(context);
                }
                return token_repository;
            }
        }

        public GenericRepository<GarageEntity> GarageRepository
        {
            get
            {
                if (this.garage_repository == null)
                {
                    this.garage_repository = new GenericRepository<GarageEntity>(context);
                }
                return garage_repository;
            }
        }

        public GenericRepository<OrderEntity> OrderRepository
        {
            get
            {
                if (this.order_repository == null)
                {
                    this.order_repository = new GenericRepository<OrderEntity>(context);
                }
                return order_repository;
            }
        }

        public GenericRepository<OrderLineEntity> OrderLineRepository
        {
            get
            {
                if (this.order_line_repository == null)
                {
                    this.order_line_repository = new GenericRepository<OrderLineEntity>(context);
                }
                return order_line_repository;
            }
        }

        public GenericRepository<JunkyardEntity> JunkyardRepository
        {
            get
            {
                if (this.junkyard_repository == null)
                {
                    this.junkyard_repository = new GenericRepository<JunkyardEntity>(context);
                }
                return junkyard_repository;
            }
        }

        public GenericRepository<OfferEntity> OfferRepository
        {
            get
            {
                if (this.offer_repository == null)
                {
                    this.offer_repository = new GenericRepository<OfferEntity>(context);
                }
                return offer_repository;
            }
        }

        public GenericRepository<OfferLineEntity> OfferLineRepository
        {
            get
            {
                if (this.offer_line_repository == null)
                {
                    this.offer_line_repository = new GenericRepository<OfferLineEntity>(context);
                }
                return offer_line_repository;
            }
        }

        public GenericRepository<JobEntity> JobRepository
        {
            get
            {
                if (this.job_repository == null)
                {
                    this.job_repository = new GenericRepository<JobEntity>(context);
                }
                return job_repository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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