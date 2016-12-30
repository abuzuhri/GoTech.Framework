using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTech.Framework
{
    public class DbEntityEntryChanged
    {
        public DbEntityEntry entity;
        public EntityState state;

        public DbEntityEntryChanged(DbEntityEntry entity)
        {
            this.entity = entity;
            this.state = entity.State;
        }
    }
}
