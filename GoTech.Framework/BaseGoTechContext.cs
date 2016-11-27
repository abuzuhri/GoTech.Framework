using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GoTech.Framework
{
    public class BaseGoTechContext : DbContext
    {

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseGoTechEntity
        {
            return base.Set<TEntity>();
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry.Entity is BaseGoTechEntity)
            {
                var entity = (BaseGoTechEntity)entityEntry.Entity;
                if (entityEntry.State == EntityState.Added)
                {
                    entity.DateCreated = DateTime.Now;
                    entity.DateUpdated = DateTime.Now;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.DateUpdated = DateTime.Now;
                    Entry(entity).State = EntityState.Modified;
                }
                else if (entityEntry.State == EntityState.Deleted)
                {

                }
                //entity.DateCreated
            }
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
