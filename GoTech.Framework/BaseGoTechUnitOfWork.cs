using GoTech.Framework.Service;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GoTech.Framework
{
    public class BaseGoTechUnitOfWork : IDisposable
    {
        private static BaseGoTechContext context=null;
        private bool disposed;
        private Dictionary<string, object> repositories;

        private event EventHandler OnSavingChanges;
        private event EventHandler OnSavedChanges;
        private Transaction transaction=null;

        public BaseGoTechUnitOfWork(BaseGoTechContext context)
        {
            if(context==null)
                BaseGoTechUnitOfWork.context = context;
            this.OnSavingChanges += new EventHandler(context_onSavingChanges);
            this.OnSavedChanges += new EventHandler(context_onSavedChanges);
        }

        public DbRawSqlQuery SqlQuery(Type elementType,string sql,params object[] parameters)
        {
            return context.Database.SqlQuery(elementType, sql, parameters);
        }
        public DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return context.Database.SqlQuery<TElement>(sql, parameters);
        }
        public int ExecuteSqlCommand(string sql,params object[] parameters)
        {
           return context.Database.ExecuteSqlCommand(sql, parameters);
        }
        public int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            return context.Database.ExecuteSqlCommand(transactionalBehavior,sql, parameters);
        }

        public Transaction BeginDbTransaction()
        {
            transaction = new Transaction(context, OnSavedChanges);
            return transaction;
        }

        private static IBusinessRuleValidation GetBusinessRuleTransationValidation(object sender, Type type)
        {
            IBusinessRuleValidation validator = null;
            string strTypeName = type.BaseType.Name;

            try
            {
                string assemblyName = sender.GetType().AssemblyQualifiedName.Split(",".ToCharArray())[1].Trim();
                string typeName = assemblyName + ".BusinessRuleValidation." + strTypeName + "BusinessRule";
                string fullyQualifiedName = typeName + ", " + assemblyName;
                Type validatorType = Type.GetType(fullyQualifiedName);
                validator = Activator.CreateInstance(validatorType) as IBusinessRuleValidation;
            }
            catch{ }

            return validator;
        }
        private static IBusinessRuleValidation GetBusinessRuleValidation(object sender, DbEntityEntry dbEntityEntry)
        {
            IBusinessRuleValidation validator = null;

            string strTypeName = "";
            if (dbEntityEntry.State == EntityState.Added)
                strTypeName = dbEntityEntry.Entity.GetType().Name;
            else strTypeName = dbEntityEntry.Entity.GetType().BaseType.Name;

            try
            {
                string assemblyName = sender.GetType().AssemblyQualifiedName.Split(",".ToCharArray())[1].Trim();
                string typeName = assemblyName + ".BusinessRuleValidation." + strTypeName + "BusinessRule";
                string fullyQualifiedName = typeName + ", " + assemblyName;
                Type validatorType = Type.GetType(fullyQualifiedName);
                validator = Activator.CreateInstance(validatorType) as IBusinessRuleValidation;
            }
            catch { }

            return validator;
        }
        private void CheckingBusinessRule(object sender,ChangeState changeState)
        {
            if (context != null)
            {
                if (changeState == ChangeState.Saving)
                {
                    var changeEntities = context.ChangeTracker.Entries().Where(a => a.State == EntityState.Added || a.State == EntityState.Modified || a.State == EntityState.Deleted).ToList();
                    foreach(var entity in changeEntities)
                    {
                        if(transaction!=null && transaction.ChangedDBEntities != null)
                        {
                            transaction.AddEntity(new DbEntityEntryChanged(entity));
                        }
                        IBusinessRuleValidation validator = GetBusinessRuleValidation(sender, entity);
                        if(validator != null)
                        {
                            switch (entity.State)
                            {
                                case EntityState.Added:
                                    validator.OnCreating(entity);break;
                                case EntityState.Modified:
                                    validator.OnUpdating(entity); break;
                                case EntityState.Deleted:
                                    validator.OnDeleting(entity); break;

                            }
                        }
                    }
                }else if(changeState==ChangeState.Saved && transaction!=null && transaction.ChangedDBEntities != null)
                {
                    foreach(var ChangedObj in transaction.ChangedDBEntities)
                    {
                        IBusinessRuleValidation validator = GetBusinessRuleTransationValidation(sender, ChangedObj.entity.Entity.GetType());
                        if (validator != null)
                        {
                            switch (ChangedObj.state)
                            {
                                case EntityState.Added:
                                    validator.OnCreating(ChangedObj.entity); break;
                                case EntityState.Modified:
                                    validator.OnUpdating(ChangedObj.entity); break;
                                case EntityState.Deleted:
                                    validator.OnDeleting(ChangedObj.entity); break;

                            }
                        }
                    }
                }
            }
        }

        private void context_onSavingChanges(object sender,EventArgs e)
        {
            CheckingBusinessRule(sender, ChangeState.Saving);
        }
        private void context_onSavedChanges(object sender, EventArgs e)
        {
            CheckingBusinessRule(sender, ChangeState.Saved);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Save()
        {
            OnSavingChanges.Invoke(this, null);
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

        public void ClearBag<T>(ICollection<T> bag) where T : BaseGoTechEntity
        {
            if(bag!=null && bag.Count > 0)
            {
                ((DbSet<T>)context.Set<T>()).RemoveRange(bag);
            }
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



        public enum ChangeState
        {
            Saving,
            Saved
        }
    }
}
