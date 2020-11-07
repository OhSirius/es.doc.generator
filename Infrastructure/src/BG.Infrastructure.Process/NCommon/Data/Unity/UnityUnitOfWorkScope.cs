using Common.Logging;
using Microsoft.Practices.Unity;
using NCommon;
using NCommon.Data;
using NCommon.Data.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.NCommon.Extensions;
using System.Data.Entity.Core.EntityClient;
using NCommon.Data.EntityFramework;
using BG.Infrastructure.Process.Transactions;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    /// <summary>
    /// Helper class that allows starting and using a unit of work like:
    /// <![CDATA[
    ///     using (UnitOfWorkScope scope = new UnitOfWorkScope()) {
    ///         //Do some stuff here.
    ///         scope.Commit();
    ///     }
    /// 
    /// ]]>
    /// </summary>
    public class UnityUnitOfWorkScope : IUnitOfWorkScope, IExUnitOfWorkScope
    {
        bool _disposed;
        bool _commitAttempted;
        bool _completed;
        readonly Guid _scopeId = Guid.NewGuid();
        readonly ILog _logger = LogManager.GetLogger<UnityUnitOfWorkScope>();
        readonly bool _autoCompleteScope = false;
        readonly IUnityUnitOfWorkManager _manager;

        //IUnityContainer Container { set; get; }

        /// <summary>
        /// Default Constuctor.
        /// Creates a new <see cref="UnitOfWorkScope"/> with the <see cref="System.Data.IsolationLevel.Serializable"/> 
        /// transaction isolation level.
        /// </summary>
        public UnityUnitOfWorkScope(IUnityUnitOfWorkManager manager, CustomUnitOfWorkSettings settings) 
            : this(manager, TransactionMode.Default, settings) { }

        /// <summary>
        /// Overloaded Constructor.
        /// Creates a new instance of the <see cref="UnitOfWorkScope"/> class.
        /// </summary>
        /// <param name="mode">A <see cref="TransactionMode"/> enum specifying the transation mode
        /// of the unit of work.</param>
        public UnityUnitOfWorkScope(IUnityUnitOfWorkManager manager, TransactionMode mode, CustomUnitOfWorkSettings settings)
        {
            //Guard.Against<ArgumentNullException>(container == null, "Ошибка создания UnityUnitOfWorkScope: не определен Unity-контейнер");
            Guard.Against<ArgumentNullException>(settings == null, "Ошибка создания UnityUnitOfWorkScope: не определены Settings");

            _manager = manager;
            //Container = container;
            _autoCompleteScope = settings.AutoCompleteScope;
            //UnitOfWorkManager.CurrentTransactionManager.EnlistScope(this, mode);
            //Container.Resolve<ITransactionManager>().EnlistScope(this, mode);
            _manager.CurrentTransactionManager().EnlistScope(this, mode);
        }

        bool _trackQueryORM = false;
        //public bool TrackQueryORM
        //{
        //    set
        //    {
        //        if (_trackQueryORM != value)
        //        {
        //            if (_trackQueryORM)
        //            {
        //                Aetp.Configuration.CloseConnectionWithoutRelease();
        //            }
        //            else
        //            {
        //                var connection = ((IExUnitOfWork)UnityUnitOfWorkManager.CurrentTransactionManager(Container).CurrentUnitOfWork)
        //                             .Resolver.GetDefaultObjectContext().Connection;
        //                Aetp.Configuration.OpenConnectionWithLock(((EntityConnection)connection).StoreConnection);
        //                //var connection = ((IExUnitOfWork)UnityUnitOfWorkManager.CurrentTransactionManager(Container).CurrentUnitOfWork)
        //                //             .Resolver.GetDefaultContext().Database.Connection;
        //                //Aetp.Configuration.OpenConnectionWithLock(connection);
        //            }

        //            _trackQueryORM = value;
        //        }
        //    }
        //    get
        //    {
        //        return _trackQueryORM;
        //    }
        //}

        /// <summary>
        /// Устанавливаем транзакцию для Query ORM
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public void SetTrackQueryORM<TEntity>()
        {
            var connection = CurrentUnitOfWork<EFUnitOfWork>().GetSession<TEntity>().Context.Connection;
            //Aetp.Configuration.OpenConnectionWithLock(((EntityConnection)connection).StoreConnection, _manager.CurrentSubject());
            _trackQueryORM = true;
        }


        /// <summary>
        /// Event fired when the scope is comitting.
        /// </summary>
        public event Action<IUnitOfWorkScope> ScopeComitting;

        /// <summary>
        /// Event fired when the scope is rollingback.
        /// </summary>
        public event Action<IUnitOfWorkScope> ScopeRollingback;


        /// <summary>
        /// Gets the unique Id of the <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <value>A <see cref="Guid"/> representing the unique Id of the scope.</value>
        public Guid ScopeId
        {
            get { return _scopeId; }
        }

        /// <summary>
        /// Gets the current unit of work that the scope participates in.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IUnitOfWork"/> to retrieve.</typeparam>
        /// <returns>A <see cref="IUnitOfWork"/> instance of type <typeparamref name="T"/> that
        /// the scope participates in.</returns>
        public T CurrentUnitOfWork<T>()
        {
            //var currentUow = UnitOfWorkManager.CurrentUnitOfWork;
            //var currentUow = Container.Resolve<ITransactionManager>().CurrentUnitOfWork;
            var currentUow = _manager.CurrentTransactionManager().CurrentUnitOfWork;
            Guard.Against<InvalidOperationException>(currentUow == null,
                                                     "No compatible UnitOfWork was found. Please start a compatible UnitOfWorkScope before " +
                                                     "using the repository.");

            Guard.TypeOf<T>(currentUow,
                            "The current unit of work is not compatible with expected type" + typeof(T).FullName +
                            ", instead the current unit of work is of type " + currentUow.GetType().FullName + ".");
            return (T)currentUow;
        }

        ///<summary>
        /// Commits the current running transaction in the scope.
        ///</summary>
        public void Commit()
        {
            Guard.Against<ObjectDisposedException>(_disposed,
                                                   "Cannot commit a disposed UnitOfWorkScope instance.");
            Guard.Against<InvalidOperationException>(_completed,
                                                     "This unit of work scope has been marked completed. A child scope participating in the " +
                                                     "transaction has rolledback and the transaction aborted. The parent scope cannot be commit.");


            _commitAttempted = true;
            OnCommit();
        }

        /// <summary>
        /// Marks the scope as completed.
        /// Used for internally by NCommon and should not be used by consumers.
        /// </summary>
        public void Complete()
        {
            _completed = true;

            //if (_trackQueryORM)
            //    Aetp.Configuration.CloseConnectionWithoutRelease();
        }

        /// <summary>
        /// 
        /// </summary>
        void OnCommit()
        {
            _logger.Info(x => x("UnitOfWorkScope {0} Comitting.", _scopeId));
            if (ScopeComitting != null)
                ScopeComitting(this);
        }

        /// <summary>
        /// 
        /// </summary>
        void OnRollback()
        {
            _logger.Info(x => x("UnitOfWorkScope {0} Rolling back.", _scopeId));
            if (ScopeRollingback != null)
                ScopeRollingback(this);
        }

        /// <summary>
        /// Disposes off the <see cref="UnitOfWorkScope"/> insance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and un-managed resources used.
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    if (_completed)
                    {
                        //Scope is marked as completed. Nothing to do here...
                        _disposed = true;
                        return;
                    }

                    if (!_commitAttempted && _autoCompleteScope)
                        //Scope did not try to commit before, and auto complete is switched on. Trying to commit.
                        //If an exception occurs here, the finally block will clean things up for us.
                        OnCommit();
                    else
                        //Scope either tried a commit before or auto complete is turned off. Trying to rollback.
                        //If an exception occurs here, the finally block will clean things up for us.
                        OnRollback();
                }
                finally
                {
                    ScopeComitting = null;
                    ScopeRollingback = null;
                    _disposed = true;

                    //if (_trackQueryORM)
                    //    Aetp.Configuration.CloseConnectionWithoutRelease();
                }
            }
        }
    }

}
