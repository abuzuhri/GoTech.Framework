using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework.Service
{
    public interface IBusinessRuleValidation
    {
        void OnCreating(DbEntityEntry entityEntry);
        void OnUpdating(DbEntityEntry entityEntry);
        void OnDeleting(DbEntityEntry entityEntry);

        void OnCreated(DbEntityEntry entityEntry);
        void OnUpdated(DbEntityEntry entityEntry);
        void OnDeleted(DbEntityEntry entityEntry);
    }
}
