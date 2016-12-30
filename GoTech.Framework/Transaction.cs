using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework
{
    public class Transaction : IDisposable
    {
        public IList<DbEntityEntryChanged> ChangedDBEntities;

        private readonly BaseGoTechContext context;
        private readonly EventHandler OnSavedChanges;
        private bool isCommited = false;
        
        public Transaction(BaseGoTechContext context, EventHandler OnSavedChanges)
        {
            this.context = context;
            this.OnSavedChanges = OnSavedChanges;
            this.context.Database.BeginTransaction();
            this.ChangedDBEntities = new List<DbEntityEntryChanged>();
        }
        public void AddEntity(DbEntityEntryChanged entity)
        {
            ChangedDBEntities.Add(entity);
        }
        public void Commit()
        {
            OnSavedChanges.Invoke(this, null);
            ChangedDBEntities.Clear();
            context.Database.CurrentTransaction.Commit();
            isCommited = true;
        }
        public void RollbackChangeTracker()
        {
            var changedEntities = context.ChangeTracker.Entries().Where(a => a.State != System.Data.Entity.EntityState.Unchanged).ToList();
            foreach(var entity in changedEntities.Where(a => a.State == System.Data.Entity.EntityState.Modified))
            {
                entity.CurrentValues.SetValues(entity.OriginalValues);
                entity.State = System.Data.Entity.EntityState.Unchanged;
            }
            foreach (var entity in changedEntities.Where(a => a.State == System.Data.Entity.EntityState.Added))
            {
                entity.State = System.Data.Entity.EntityState.Detached;
            }
            foreach (var entity in changedEntities.Where(a => a.State == System.Data.Entity.EntityState.Detached))
            {
                entity.State = System.Data.Entity.EntityState.Unchanged;
            }

        }
        public void Rollback()
        {
            ChangedDBEntities.Clear();
            RollbackChangeTracker();
            if (context.Database.CurrentTransaction != null)
                context.Database.CurrentTransaction.Rollback();
        }
        public void Dispose()
        {
            ChangedDBEntities.Clear();
            if (!isCommited)
            {
                Rollback();
            }
            if (context.Database.CurrentTransaction != null)
                context.Database.CurrentTransaction.Dispose();
        }
    }
}
