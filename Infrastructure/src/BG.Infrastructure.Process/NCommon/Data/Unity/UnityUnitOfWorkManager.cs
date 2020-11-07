using System;
using Common.Logging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NCommon.Data;
using NCommon.Data.Impl;
using NCommon.State;
using BG.Infrastructure.Process.Transactions;
using System.Threading;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    public interface IUnityUnitOfWorkManager
    {
        ITransactionManager CurrentTransactionManager();
        IUnitOfWork CurrentUnitOfWork();

        IBusinessTransactionSubject CurrentSubject();
    }

    ///<summary>
    /// Gets an instances of <see cref="ITransactionManager"/>.
    ///</summary>
    public class UnityUnitOfWorkManager: IUnityUnitOfWorkManager
    {
        readonly IBusinessTransactionSubjectFactory _subjectFactory;
        readonly IState _state;
        readonly IUnitOfWorkGuid _guid;
        readonly IUnitOfWorkFactory _scopeFactory;
        readonly CustomUnitOfWorkSettings _settings;

        static Func<IUnityContainer, ITransactionManager> _provider;
        static readonly ILog Logger = LogManager.GetLogger(typeof(UnitOfWorkManager));
        private const string LocalTransactionManagerKey = "UnitOfWorkManager.LocalTransactionManager";

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="UnitOfWorkManager"/>.
        /// </summary>
        public UnityUnitOfWorkManager(IState state, IBusinessTransactionSubjectFactory subjectFactory, IUnitOfWorkGuid guid, IUnitOfWorkFactory scopeFactory, CustomUnitOfWorkSettings settings)
        {
            //_provider = DefaultTransactionManager;
            _subjectFactory = subjectFactory;
            _state = state;
            _guid = guid;
            _scopeFactory = scopeFactory;
            _settings = settings;
        }

        ///<summary>
        /// Sets a <see cref="Func{T}"/> of <see cref="ITransactionManager"/> that the 
        /// <see cref="UnitOfWorkManager"/> uses to get an instance of <see cref="ITransactionManager"/>
        ///</summary>
        ///<param name="provider"></param>
        //public static void SetTransactionManagerProvider(Func<IUnityContainer, ITransactionManager> provider)
        //{
        //    if (provider == null)
        //    {
        //        Logger.Debug(x => x("The transaction manager provide is being set to null. Using " +
        //                            " the transaction manager to the default transaction manager provider."));
        //        _provider = DefaultTransactionManager;
        //        return;
        //    }
        //    Logger.Debug(x => x("The transaction manager provider is being overriden. Using supplied" +
        //                        " trasaction manager provider."));
        //    _provider = provider;
        //}

        /// <summary>
        /// Gets the current <see cref="ITransactionManager"/>.
        /// </summary>
        public ITransactionManager CurrentTransactionManager()
        {
            return GetDefaultTransactionManager();
        }

        /// <summary>
        /// Gets the current <see cref="IUnitOfWork"/> instance.
        /// </summary>
        public IUnitOfWork CurrentUnitOfWork()
        {
            return GetDefaultTransactionManager().CurrentUnitOfWork;
        }

        public IBusinessTransactionSubject CurrentSubject()
        {
            return ((IExTransactionManager)GetDefaultTransactionManager()).Subject;
        }

        /// <summary>
        /// Позволяет исправить ошибку с доступом к локальному кэшу на основе ThreadLocalState при запросе ITransactionManager от разных пользователей
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        static string GetLocalTransactionManagerKey(Guid guid)
        {
            //return string.Format("{0}{1}", LocalTransactionManagerKey, guid);
            return string.Format("{0}{1}{2}", LocalTransactionManagerKey, Thread.CurrentThread.ManagedThreadId, guid);
        }


        ITransactionManager  GetDefaultTransactionManager()
        {
            Logger.Debug(x => x("Using default UnitOfWorkManager provider to resolve current transaction manager."));

            //var state = container.Resolve<IState>();
            //var guid = container.Resolve<IUnitOfWorkGuid>();
            //var subscriber = container.Resolve<IBusinessTransactionSubscriber>();

            //var transactionManager = state.Local.Get<ITransactionManager>(GetLocalTransactionManagerKey(guid.Guid));
            var transactionManager = _state.Application.Get<ITransactionManager>(GetLocalTransactionManagerKey(_guid.Guid));
            if (transactionManager == null)
            {
                Logger.Debug(x => x("No valid ITransactionManager found in Local state. Creating a new TransactionManager."));
                //transactionManager = new TransactionManager();
                transactionManager = new UnityTransactionManager(_scopeFactory, _settings, _subjectFactory.Get());
                //state.Local.Put(GetLocalTransactionManagerKey(guid.Guid), transactionManager);
                _state.Application.Put(GetLocalTransactionManagerKey(_guid.Guid), transactionManager);
            }
            return transactionManager;
        }


    }
}