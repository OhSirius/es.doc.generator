using BG.Infrastructure.Process.Transactions;
using NCommon;
using NCommon.Data;
using NCommon.Data.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using BG.Extensions;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{

    public class UnityUnitOfWorkTransaction : UnitOfWorkTransaction
    {
        readonly IBusinessTransactionSubscriber _subscriber;

        public UnityUnitOfWorkTransaction(IExUnitOfWork unitOfWork, TransactionScope transaction, IBusinessTransactionSubscriber subscriber) 
            : base(unitOfWork, transaction)
        {
            _subscriber = subscriber;
            _subscriber.Attach(this);
        }




        protected override void OnScopeCommitting(IUnitOfWorkScope scope)
        {
            Guard.Against<ObjectDisposedException>(_disposed,
                                                   "The transaction attached to the scope has already been disposed.");

            _logger.Info(x => x("Commit signalled by scope {0} on transaction {1}.", scope.ScopeId, _transactionId));
           if (!_attachedScopes.Contains(scope))
           {
               Dispose();
               throw new InvalidOperationException("The scope being comitted is not attached to the current transaction.");
           }
            scope.ScopeComitting -= OnScopeCommitting;
            scope.ScopeRollingback -= OnScopeRollingBack;
            scope.Complete();
            _attachedScopes.Remove(scope);
            if (_attachedScopes.Count == 0)
            {
                _logger.Info(x => x("All scopes have signalled a commit on transaction {0}. Flushing unit of work and comitting attached TransactionScope.", _transactionId));
                try
                {
                    _unitOfWork.Flush();
                    _transaction.Complete();
                    _transaction.Dispose();
                    ((IExUnitOfWork)_unitOfWork).AfterFlushing();

                    _subscriber.Next(new BusinessTransactionEvent());
                    _subscriber.Detach(this);
                }
                finally
                {
                    Dispose(); //Dispose the transaction after comitting.
                }
            }

            
        }

        protected override void OnScopeRollingBack(IUnitOfWorkScope scope)
        {
            base.OnScopeRollingBack(scope);

            if (_attachedScopes.IsNullOrEmpty())
                _subscriber.Detach(this);
        }
    }
}
