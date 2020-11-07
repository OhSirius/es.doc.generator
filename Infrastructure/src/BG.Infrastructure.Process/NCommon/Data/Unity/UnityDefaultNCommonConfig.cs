using BG.Infrastructure.Process.NCommon.Data.Unity.Test;
using NCommon.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Data.Entity.Core.Objects;
using NCommon.ContainerAdapter.Unity;
using NCommon.State.Impl;
using Configure = NCommon.Configure;
using System.Data.Entity;
using BG.Infrastructure.Process.Logging.Adapters.EF;
using BG.Infrastructure.Process.NCommon.Data.Unity.EF.Impl;
using BG.Infrastructure.Process.Logging;
using System.Data.Entity.Infrastructure;
using System.Transactions;
using BG.Infrastructure.Process.Identity;
using RT.Infrastructure.Process.Logging.Adapters.Stub;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    public static class UnityDefaultNCommonConfig
    {
        /// <summary>
        /// В качестве LocalState используется ThreadLocalState, чтобы обеспечить кэширование UnityContainer для пользователя и неконфликтности кэшей (для ITransactionManager) от параллельных запросов от одного пользователя.
        /// </summary>
        /// <typeparam name="TLoggingContext"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="container"></param>
        /// <param name="user"></param>
        /// <param name="contextProvider"></param>
        /// <returns></returns>
        public static INCommonConfig GetEFWithLogger<TLoggingContext, TContext>(IUnityContainer container, IUser user, Func<TLoggingContext> contextProvider, Func<TContext, ILoggingAdapter> loggerProvider)
            where TContext : DbContext
            where TLoggingContext : ILoggerDataContext<TContext>
        {
            return
            Configure.Using(new UnityContainerAdapter(container))
                     .ConfigureState<DefaultStateConfiguration>(state=>state.UseCustomLocalStateOf<ThreadLocalState>())
                     .ConfigureData<UnityEFConfiguration<TLoggingContext, TContext>>(dataConfig => dataConfig.WithObjectContextAndLogger(()=>(TLoggingContext)contextProvider().RegisterLoggerProvider(loggerProvider)))
                     .ConfigureUnitOfWork<UnityUnitOfWorkConfiguration>(uowConfig =>
                                                                          uowConfig//.WithTransactionManager((settings) => new UnityTransactionManager(container, settings))
                                                                                   .WithDefaultIsolation(IsolationLevel.ReadUncommitted)
                                                                                );
        }

        public static INCommonConfig GetEFWithoutLogger<TLoggingContext, TContext>(IUnityContainer container, IUser user, Func<TLoggingContext> contextProvider)
            where TContext : DbContext
            where TLoggingContext : ILoggerDataContext<TContext>
        {
            return
            Configure.Using(new UnityContainerAdapter(container))
                     .ConfigureState<DefaultStateConfiguration>(state => state.UseCustomLocalStateOf<ThreadLocalState>())
                     .ConfigureData<UnityEFConfiguration<TLoggingContext, TContext>>(dataConfig => dataConfig.WithObjectContextAndLogger(() => (TLoggingContext)contextProvider().RegisterLoggerProvider(c => new StubLoggingAdapter(c, user))))
                     .ConfigureUnitOfWork<UnityUnitOfWorkConfiguration>(uowConfig =>
                                                                          uowConfig//.WithTransactionManager((settings) => new UnityTransactionManager(container, settings))
                                                                                   .WithDefaultIsolation(IsolationLevel.ReadUncommitted)
                                                                                );
        }

        /// <summary>
        /// Конфигурация NCommon с поддержкой бизнес-транзакций
        /// </summary>
        /// <typeparam name="TLoggingContext"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="container"></param>
        /// <param name="user"></param>
        /// <param name="contextProvider"></param>
        /// <returns></returns>
        public static INCommonConfig GetEFWithTransaction<TLoggingContext, TContext>(IUnityContainer container, IUser user, Func<TLoggingContext> contextProvider)//, Func<TContext, ILoggingAdapter> loggerProvider)
            where TContext : DbContext
            where TLoggingContext : ILoggerDataContext<TContext>
        {
            return
            Configure.Using(new UnityContainerAdapter(container))
                     .ConfigureState<DefaultStateConfiguration>(state => state.UseCustomLocalStateOf<ThreadLocalState>())
                     .ConfigureData<UnityEFConfiguration<TLoggingContext, TContext>>(dataConfig => dataConfig.WithObjectContextAndLogger(() => (TLoggingContext)contextProvider().RegisterLoggerProvider(c=>new StubLoggingAdapter(c, user))))
                     .ConfigureUnitOfWork<UnityBusinessTransactionUnitOfWorkConfiguration>(uowConfig =>
                                                                          uowConfig//.WithTransactionManager((settings) => new UnityTransactionManager(container, settings))
                                                                                   .WithDefaultIsolation(IsolationLevel.ReadUncommitted)
                                                                                );
        }




        /// <summary>
        /// Конфигурация NCommon для тестирования. Для корректной инфициализации необходимо зарегистрировать IRepository<> и IUnitOfWorkFactory 
        /// через dataRegisteredActions
        /// </summary>
        /// <param name="container"></param>
        /// <param name="dataRegisteredActions"></param>
        /// <returns></returns>
        public static INCommonConfig GetTest(IUnityContainer container, Action<IContainerAdapter> dataRegisteredActions)
        {
            return
            Configure.Using(new UnityContainerAdapter(container))
                     .ConfigureState<DefaultStateConfiguration>()
                     .ConfigureData<TestDataConfiguration>(t => t.Register(dataRegisteredActions))
                     .ConfigureUnitOfWork<DefaultUnitOfWorkConfiguration>();
        }

        //public readonly static Func<IUnityContainer, User, DbContext, INCommonConfig> EFWithLogger = (container, user, dbContext) =>
        //{
        //    return
        //    Configure.Using(new UnityContainerAdapter(container))
        //             .ConfigureState<DefaultStateConfiguration>()
        //             .ConfigureData<UnityEFConfiguration>(dataConfig => dataConfig.WithObjectContextAndLogger(() => ((IObjectContextAdapter)dbContext).ObjectContext
        //                                                                                                    , () => ((ILoggerDataContext<DbContext>)dbContext).RegisterLoggerProvider(c=>new EFLoggingAdapter(c,user))
        //                                                                                                                                                      .GetLogger()))
        //             .ConfigureUnitOfWork<UnityUnitOfWorkConfiguration>(uowConfig =>
        //                                                                  uowConfig.WithTransactionManager((settings) => new UnityTransactionManager(container, settings))
        //                                                                           .WithDefaultIsolation(IsolationLevel.ReadUncommitted)
        //                                                                        );

        //};
    }
}
