using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Process
{
    /// <summary>
    /// Обеспечивает реализацию CRUD интерфейса для сущности <see cref="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityBusinessProcess<TEntity> : IObjectBusinessProcess where TEntity : class
    {
        bool IsValidLastOperation { get; }

        string LastOperationErrorText { get; }

        TEntity Create();
        
        TEntity Save(TEntity entity);
        TEntity[] Save(TEntity[] entities);
        void Delete(TEntity entity);

        void Delete(TEntity[] entities);
    }
}
