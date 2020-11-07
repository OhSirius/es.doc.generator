using BG.Infrastructure.Process.Logging;
using NCommon;
using NCommon.Data;
using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.EF.Impl
{
    public class UnityEFUnitOfWorkFactory<TLoggingContext, TContext> : IUnitOfWorkFactory
        where TContext : DbContext
        where TLoggingContext : ILoggerDataContext<TContext>
    {
        UnityExEFSessionResolver<TLoggingContext, TContext> _resolver = new UnityExEFSessionResolver<TLoggingContext, TContext>();

        /// Registers a <see cref="Func{T}"/> of type <see cref="ObjectContext"/> provider that can be used
        /// to resolve instances of <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="contextProvider">A <see cref="Func{T}"/> of type <see cref="ObjectContext"/>.</param>
        public void RegisterObjectContextProvider(Func<ObjectContext> contextProvider)
        {
            Guard.Against<ArgumentNullException>(contextProvider == null,
                                                 "Invalid object context provider registration. " +
                                                 "Expected a non-null Func<ObjectContext> instance.");
            _resolver.RegisterObjectContextProvider(contextProvider);
        }

        public void RegisterObjectContextProvider(Func<TLoggingContext> contextProvider)
        {
            Guard.Against<ArgumentNullException>(contextProvider == null,
                                                 "Invalid object context provider registration. " +
                                                 "Expected a non-null Func<ObjectContext> instance.");

            _resolver.RegisterObjectContextProvider(contextProvider);
        }

        //public void RegisterObjectContextProvider(Func<ObjectContext> contextProvider, Func<ILoggingAdapter> loggingProvider)
        //{
        //    Guard.Against<ArgumentNullException>(contextProvider == null,
        //                                         "Invalid object context provider registration. " +
        //                                         "Expected a non-null Func<ObjectContext> instance.");
        //    Guard.Against<ArgumentNullException>(contextProvider == null,
        //                                         "Invalid logging provider registration. " +
        //                                         "Expected a non-null Func<ILoggingAdapter> instance.");
        //    _resolver.RegisterObjectContextProvider(contextProvider, loggingProvider);
        //}
        /// <summary>
        /// Creates a new instance of <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <returns>Instances of <see cref="EFUnitOfWork"/>.</returns>
        public IUnitOfWork Create()
        {
            Guard.Against<InvalidOperationException>(
               _resolver.ObjectContextsRegistered == 0,
               "No ObjectContext providers have been registered. You must register ObjectContext providers using " +
               "the RegisterObjectContextProvider method or use NCommon.Configure class to configure NCommon.EntityFramework " +
               "using the EFConfiguration class and register ObjectContext instances using the WithObjectContext method.");

            return new UnityExEFUnitOfWork(_resolver);
        }

    }
}
