using BG.Infrastructure.Process.Transactions;
using BG.Infrastructure.Process.Transactions.Impl;
using NCommon.Configuration;
using NCommon.Data;
using NCommon.Data.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TransactionManager = NCommon.Data.Impl.TransactionManager;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    ///<summary>
    /// Implementation of <see cref="IUnitOfWorkConfiguration"/>.
    ///</summary>
    public class UnityUnitOfWorkConfiguration : IUnitOfWorkConfiguration
    {
        bool _autoCompleteScope = false;
        IsolationLevel _defaultIsolation = IsolationLevel.ReadCommitted;
        protected bool _useBusinessTransaction = false;
        //Func<CustomUnitOfWorkSettings, ITransactionManager> getTransactionManager;

        /// <summary>
        /// Configures <see cref="UnitOfWorkScope"/> settings.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="IContainerAdapter"/> instance.</param>
        public void Configure(IContainerAdapter containerAdapter)
        {
            var settings = new CustomUnitOfWorkSettings() { AutoCompleteScope = _autoCompleteScope, DefaultIsolation = _defaultIsolation };
            containerAdapter.RegisterInstance(settings);
            containerAdapter.RegisterSingleton<IUnityUnitOfWorkManager, UnityUnitOfWorkManager>();
            //containerAdapter.RegisterInstance<ITransactionManager>(getTransactionManager(settings));
            containerAdapter.RegisterInstance<IUnitOfWorkGuid>(new UnitOfWorkGuid());
            containerAdapter.RegisterSingleton<IUnitOfWorkScopeFactory, UnityUnitOfWorkScopeFactory>();
            containerAdapter.Register<IUnitOfWorkScope, UnityUnitOfWorkScope>();

            //UnitOfWorkSettings.AutoCompleteScope = _autoCompleteScope;
            //UnitOfWorkSettings.DefaultIsolation = _defaultIsolation;
            if (_useBusinessTransaction)
            {
                containerAdapter.RegisterSingleton<IBusinessTransactionObservable, BusinessTransactionSubjectWrapper>();
                containerAdapter.RegisterSingleton<IBusinessTransactionSubscriber, BusinessTransactionSubjectWrapper>();
                containerAdapter.RegisterInstance<IBusinessTransactionSubjectFactory>(new BusinessTransactionSubjectFactory(() => new BusinessTransactionSubject()));
            }
            else
            {
                var subject = new StubTransactionSubject();
                containerAdapter.RegisterInstance<IBusinessTransactionObservable>(subject);
                containerAdapter.RegisterInstance<IBusinessTransactionSubscriber>(subject);
                containerAdapter.RegisterInstance<IBusinessTransactionSubjectFactory>(new BusinessTransactionSubjectFactory(() => subject));
            }



            //IBusinessTransactionSubject subject = !_useBusinessTransaction? new StubTransactionSubject(): new BusinessTransactionSubjectWrapper();
            //containerAdapter.RegisterInstance<IBusinessTransactionSubjectFactory>(new BusinessTransactionSubjectFactory(!_useBusinessTransaction ? () => subject : () => new BusinessTransactionSubject()));
        }

        /// <summary>
        /// Sets <see cref="UnitOfWorkScope"/> instances to auto complete when disposed.
        /// </summary>
        public IUnitOfWorkConfiguration AutoCompleteScope()
        {
            _autoCompleteScope = true;
            return this;
        }

        /// <summary>
        /// Sets the default isolation level used by <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <param name="isolationLevel"></param>
        public IUnitOfWorkConfiguration WithDefaultIsolation(IsolationLevel isolationLevel)
        {
            _defaultIsolation = isolationLevel;
            return this;
        }

        public IUnitOfWorkConfiguration WithBusinessTransaction()
        {
            _useBusinessTransaction = true;
            return this;
        }

        ///// <summary>
        ///// Sets transaction manager <see cref="UnitOfWorkScope"/>.
        ///// </summary>
        ///// <param name="isolationLevel"></param>
        //public UnityUnitOfWorkConfiguration WithTransactionManager(Func<CustomUnitOfWorkSettings, ITransactionManager> getTransactionManager)
        //{
        //    this.getTransactionManager = getTransactionManager;
        //    return this;
        //}
    }

    public class UnityBusinessTransactionUnitOfWorkConfiguration: UnityUnitOfWorkConfiguration
    {
        public UnityBusinessTransactionUnitOfWorkConfiguration()
        {
            _useBusinessTransaction = true;
        }
    }

}
