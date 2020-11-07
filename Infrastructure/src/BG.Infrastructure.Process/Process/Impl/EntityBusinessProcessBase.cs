using BG.Infrastructure.Process.Configuration;
using BG.Infrastructure.Process.Configuration.Impl;
using Microsoft.Practices.Unity;
using NCommon.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;
using BG.Infrastructure.Process.Process.Impl;

namespace BG.Infrastructure.Process.Process
{
    public abstract class EntityBusinessProcessBase<TEntity> : ObjectBusinessProcessBase, IEntityBusinessProcess<TEntity>
        where TEntity : class
    {
        public abstract TEntity Create();

        public abstract TEntity Save(TEntity entity);

        public abstract TEntity[] Save(TEntity[] entities);

        public abstract void Delete(TEntity entity);

        public abstract void Delete(TEntity[] entities);


    }
}
