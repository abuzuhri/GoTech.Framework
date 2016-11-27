using System;
using System.Collections.Generic;

namespace GoTech.Framework
{
    public class BaseGoTechUnitOfWork : IDisposable
    {
        private readonly BaseGoTechContext context;
        private bool disposed;
        private Dictionary<string, object> repositories;

        public BaseGoTechUnitOfWork(BaseGoTechContext context)
        {
            this.context = context;
        }

        public BaseGoTechUnitOfWork()
        {
            context = new BaseGoTechContext();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public BaseDataRepository<T> Repository<T>() where T : BaseGoTechEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseDataRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }
            return (BaseDataRepository<T>)repositories[type];
        }
    }
}
