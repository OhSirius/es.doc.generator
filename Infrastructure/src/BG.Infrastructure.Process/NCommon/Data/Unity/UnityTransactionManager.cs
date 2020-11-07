using Common.Logging;
using NCommon.Data;
using NCommon.Data.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCommon.Extensions;
using Microsoft.Practices.Unity;
using NCommon;
using System.Transactions;
using BG.Infrastructure.Process.Transactions;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    public interface IExTransactionManager
    {
        IBusinessTransactionSubject Subject { get; }
    }


    /// <summary>
    /// Default implementation of <see cref="ITransactionManager"/> interface.
    /// </summary>
    public class UnityTransactionManager : ITransactionManager, IDisposable, IExTransactionManager
    {
        bool _disposed;
        readonly Guid _transactionManagerId = Guid.NewGuid();
        readonly ILog _logger = LogManager.GetLogger<UnityTransactionManager>();
        readonly LinkedList<UnitOfWorkTransaction> _transactions = new LinkedList<UnitOfWorkTransaction>();
        readonly IsolationLevel _isolationLevel = IsolationLevel.Unspecified;
        //IUnityContainer Container { set; get; }
        readonly IBusinessTransactionSubject _subject;
        readonly IUnitOfWorkFactory _scopeFactory;

        /// <summary>
        /// Default Constructor.
        /// Creates a new instance of the <see cref="TransactionManager"/> class.
        /// </summary>
        public UnityTransactionManager(IUnitOfWorkFactory scopeFactory, CustomUnitOfWorkSettings settings, IBusinessTransactionSubject subject)
        {
            _logger.Debug(x => x("New instance of TransactionManager with Id {0} created.", _transactionManagerId));
            //Guard.Against<ArgumentNullException>(container == null, "Ошибка создания UnityTransactionManager: не задан тек. Unity-контейнер");
            Guard.Against<ArgumentNullException>(settings == null, "Ошибка создания UnityTransactionManager: не задан Settings");
            //Container = container;
            _scopeFactory = scopeFactory;
            this._isolationLevel = settings.DefaultIsolation;
            _subject = subject;
        }

        public IBusinessTransactionSubject Subject => _subject;

        /// <summary>
        /// Gets the current <see cref="IUnitOfWork"/> instance.
        /// </summary>
        public IUnitOfWork CurrentUnitOfWork
        {
            get
            {
                return CurrentTransaction == null ? null : CurrentTransaction.UnitOfWork;
            }
        }

        /// <summary>
        /// Gets the current <see cref="UnitOfWorkTransaction"/> instance.
        /// </summary>
        public UnitOfWorkTransaction CurrentTransaction
        {
            get
            {
                return _transactions.Count == 0 ? null : _transactions.First.Value;
            }
        }

        /// <summary>
        /// Enlists a <see cref="UnitOfWorkScope"/> instance with the transaction manager,
        /// with the specified transaction mode.
        /// </summary>
        /// <param name="scope">The <see cref="IUnitOfWorkScope"/> to register.</param>
        /// <param name="mode">A <see cref="TransactionMode"/> enum specifying the transaciton
        /// mode of the unit of work.</param>
        public void EnlistScope(IUnitOfWorkScope scope, TransactionMode mode)
        {
            _logger.Info(x => x("Enlisting scope {0} with transaction manager {1} with transaction mode {2}",
                                scope.ScopeId,
                                _transactionManagerId,
                                mode));

            var uowFactory = _scopeFactory; //Container.Resolve<IUnitOfWorkFactory>();
            if (_transactions.Count == 0 ||
                mode == TransactionMode.New ||
                mode == TransactionMode.Supress)
            {
                _logger.Debug(x => x("Enlisting scope {0} with mode {1} requires a new TransactionScope to be created.", scope.ScopeId, mode));
                var txScope = TransactionScopeHelper.CreateScope(_isolationLevel, mode);
                var unitOfWork = uowFactory.Create();
                Guard.Against<ArgumentException>(!(unitOfWork is IExUnitOfWork), "Ошибка создания UnitOfWork: тек. объект не реализует IExUnitOfWork");
                var transaction = new UnityUnitOfWorkTransaction((IExUnitOfWork)unitOfWork, txScope, _subject);
                transaction.TransactionDisposing += OnTransactionDisposing;
                transaction.EnlistScope(scope);
                _transactions.AddFirst(transaction);
                return;
            }
            CurrentTransaction.EnlistScope(scope);
        }

        /// <summary>
        /// Handles a Dispose signal from a transaction.
        /// </summary>
        /// <param name="transaction"></param>
        void OnTransactionDisposing(UnitOfWorkTransaction transaction)
        {
            _logger.Info(x => x("UnitOfWorkTransaction {0} signalled a disposed. Unregistering transaction from TransactionManager {1}",
                                    transaction.TransactionId, _transactionManagerId));

            transaction.TransactionDisposing -= OnTransactionDisposing;
            var node = _transactions.Find(transaction);
            if (node != null)
                _transactions.Remove(node);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Internal dispose.
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _logger.Info(x => x("Disposing off transction manager {0}", _transactionManagerId));
                if (_transactions != null && _transactions.Count > 0)
                {
                    _transactions.ForEach(tx =>
                    {
                        tx.TransactionDisposing -= OnTransactionDisposing;
                        tx.Dispose();
                    });
                    _transactions.Clear();
                }
            }
            _disposed = true;
        }
    }

}
