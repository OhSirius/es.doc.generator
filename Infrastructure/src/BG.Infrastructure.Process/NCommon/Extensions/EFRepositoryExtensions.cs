 
using BG.Infrastructure.Process.NCommon.Data.Unity;
using NCommon.Data;
using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Extensions
{
    public static class EFRepositoryExtensions
    {
        //public static IEnumerable<TEntity> ExecuteSql<TEntity>(this IEFSession repository, string sql, params object[] parameters) where TEntity : class
        //{
        //    Guard.Against<ArgumentNullException>(repository == null, "Expected a non-null IEFSession instance.");
        //    return repository.Context.ExecuteStoreQuery<TEntity>(sql, parameters);
        //}
        public static IEnumerable<TEntityResult> ExecuteSql<TEntity, TEntityResult>(this IUnitOfWorkScope scope, string sql, params object[] parameters)
            where TEntity : class
        {
            Guard.Against<ArgumentNullException>(scope == null, "Expected a non-null IEFSession instance.");
            Guard.Against<ArgumentException>(!(scope is IExUnitOfWorkScope), "Не реализован интерфейс IExUnitOfWorkScope");

            return ((IExUnitOfWorkScope)scope).CurrentUnitOfWork<EFUnitOfWork>().GetSession<TEntity>().Context.ExecuteStoreQuery<TEntityResult>(sql, parameters);
        }

        /// <summary>
        /// Выполнение нативного sql через контекст EF.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="scope"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public static void ExecuteNonSql<TEntity>(this IUnitOfWorkScope scope, string sql, params object[] parameters)
            where TEntity : class
        {
            Guard.Against<ArgumentNullException>(scope == null, "Expected a non-null IEFSession instance.");
            Guard.Against<ArgumentException>(!(scope is IExUnitOfWorkScope), "Не реализован интерфейс IExUnitOfWorkScope");

            ((IExUnitOfWorkScope)scope).CurrentUnitOfWork<EFUnitOfWork>().GetSession<TEntity>().Context.ExecuteStoreQuery<object>(sql, parameters);
        }

        public static DbConnection GetConnection<TEntity>(this IUnitOfWorkScope scope)
        {
            Guard.Against<ArgumentNullException>(scope == null, "Expected a non-null IEFSession instance.");
            Guard.Against<ArgumentException>(!(scope is IExUnitOfWorkScope), "Не реализован интерфейс IExUnitOfWorkScope");

            return ((IExUnitOfWorkScope)scope).CurrentUnitOfWork<EFUnitOfWork>().GetSession<TEntity>().Context.Connection;

        }

        public static IUnitOfWorkScope WithTrackQueryORM<TEntity>(this IUnitOfWorkScope scope)
        {
            Guard.Against<ArgumentNullException>(scope == null, "Expected a non-null IEFSession instance.");
            Guard.Against<ArgumentException>(!(scope is IExUnitOfWorkScope), "Не реализован интерфейс IExUnitOfWorkScope");

            ((IExUnitOfWorkScope)scope).SetTrackQueryORM<TEntity>();
            return scope;
        }
        public static void Flush(this IUnitOfWorkScope scope)
        {
            Guard.Against<ArgumentNullException>(scope == null, "Expected a non-null IEFSession instance.");
            Guard.Against<ArgumentException>(!(scope is IExUnitOfWorkScope), "Не реализован интерфейс IExUnitOfWorkScope");

            ((IExUnitOfWorkScope)scope).CurrentUnitOfWork<IUnitOfWork>().Flush();
        }

    }
}
