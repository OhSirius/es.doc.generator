using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process
{
    public interface IDeleteEntityOperation<TEntity> : IEntityOperation where TEntity : class
    {
        void DeleteEntity(TEntity entity);
    }
}
