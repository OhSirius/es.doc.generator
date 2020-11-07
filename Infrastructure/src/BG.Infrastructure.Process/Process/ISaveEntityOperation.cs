using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Process
{
    public interface ISaveEntityOperation<TEntity> : IEntityOperation where TEntity : class
    {
        //TEntity Entity { get; }

        TEntity SaveEntity(TEntity entity);
    }

}
