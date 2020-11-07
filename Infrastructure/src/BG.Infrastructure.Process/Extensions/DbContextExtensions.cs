using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Extensions
{
    public static class DbContextExtensions
    {
        public static EntityKey GetEntityKey<T>(this DbContext context, T entity)
            where T : class
        {
            var oc = ((IObjectContextAdapter)context).ObjectContext;
            ObjectStateEntry ose;
            if (null != entity && oc.ObjectStateManager
                                    .TryGetObjectStateEntry(entity, out ose))
            {
                return ose.EntityKey;
            }
            return null;
        }

        public static EntityKey GetEntityKey<T>(this DbContext context
                                               , DbEntityEntry<T> dbEntityEntry)
            where T : class
        {
            if (dbEntityEntry != null)
            {
                return GetEntityKey(context, dbEntityEntry.Entity);
            }
            return null;
        }

        public static EntityKey GetEntityKey(this DbContext context
                                               , DbEntityEntry dbEntityEntry)
        {
            if (dbEntityEntry != null)
            {
                return GetEntityKey(context, dbEntityEntry.Entity);
            }
            return null;
        }
    }
}
