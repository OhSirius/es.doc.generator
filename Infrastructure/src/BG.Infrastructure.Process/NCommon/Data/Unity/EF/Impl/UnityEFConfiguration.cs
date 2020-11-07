using NCommon;
using NCommon.Configuration;
using NCommon.Data;
using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.Logging;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.EF.Impl
{
    /// <summary>
    /// Implementatio of <see cref="IDataConfiguration"/> that configured NCommon to use Linq To Sql
    /// </summary>
    public class UnityEFConfiguration<TLoggingContext,TContext> : IDataConfiguration
        where TContext : DbContext
        where TLoggingContext : ILoggerDataContext<TContext>
    {
        readonly UnityEFUnitOfWorkFactory<TLoggingContext,TContext> _factory = new UnityEFUnitOfWorkFactory<TLoggingContext,TContext>();

        /// <summary>
        /// Configures unit of work instances to use the specified <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="objectContextProvider">A <see cref="Func{T}"/> of type <see cref="ObjectContext"/>
        /// that can be used to construct <see cref="ObjectContext"/> instances.</param>
        /// <returns><see cref="EFConfiguration"/></returns>
        public UnityEFConfiguration<TLoggingContext,TContext> WithObjectContext(Func<ObjectContext> objectContextProvider)
        {
            Guard.Against<ArgumentNullException>(objectContextProvider == null,
                                                 "Expected a non-null Func<ObjectContext> instance.");
            _factory.RegisterObjectContextProvider(objectContextProvider);
            return this;
        }

        public UnityEFConfiguration<TLoggingContext,TContext> WithObjectContextAndLogger(Func<TLoggingContext> objectContextProvider)
        {
            Guard.Against<ArgumentNullException>(objectContextProvider == null,
                                                 "Expected a non-null Func<ObjectContext> instance.");
            _factory.RegisterObjectContextProvider(objectContextProvider);
            return this;
        }

        //public UnityEFConfiguration WithObjectContextAndLogger(Func<ObjectContext> objectContextProvider, Func<ILoggingAdapter> loggingProvider)
        //{
        //    Guard.Against<ArgumentNullException>(objectContextProvider == null,
        //                                         "Expected a non-null Func<ObjectContext> instance.");
        //    Guard.Against<ArgumentNullException>(loggingProvider == null,
        //                                         "Expected a non-null Func<ILoggingAdapter> instance.");
        //    _factory.RegisterObjectContextProvider(objectContextProvider, loggingProvider);
        //    return this;
        //}

        //public UnityEFConfiguration WithLoggerProvider(Func<DbContext, ILoggingAdapter> loggerProvider)
        //{
        //    this.loggerProvider = loggerProvider;
        //    return this;
        //}

        /// <summary>
        /// Called by NCommon <see cref="Configure"/> to configure data providers.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="IContainerAdapter"/> instance that allows
        /// registering components.</param>
        public void Configure(IContainerAdapter containerAdapter)
        {
            //Guard.Against<NotSupportedException>(loggerProvider==null, "Ошибка установки конфигурации EF: не определен логгер");
            //Guard.Against<NotSupportedException>(!(objectContextProvider() is ILoggerDataContext<DbContext>), "Ошибка установки конфигурации EF: дата контекст не поддерживает ILoggerDataContext");

            //(objectContextProvider() as ILoggerDataContext<DbContext>).LoggerProvider = loggerProvider;
            containerAdapter.RegisterInstance<IUnitOfWorkFactory>(_factory);
            containerAdapter.RegisterGeneric(typeof(IRepository<>), typeof(UnityEFRepository<>));
        }
    }
}
