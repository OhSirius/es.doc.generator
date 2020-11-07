using BG.Infrastructure.Process.NCommon .Data.Unity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Transactions.Impl
{
    /// <summary>
    /// Обеспечивает получение транзакции для конкретного потока
    /// </summary>
    public class BusinessTransactionSubjectWrapper : IBusinessTransactionSubject
    {
        readonly IUnityUnitOfWorkManager _manager;

        public BusinessTransactionSubjectWrapper(IUnityUnitOfWorkManager manager)
        {
            _manager = manager;
        }

        public void Attach(object scope)
        {
            GetSubject().Attach(scope);
        }

        public void Detach(object scope)
        {
            GetSubject().Detach(scope);
        }

        public void Next(BusinessTransactionEvent @event)
        {
            GetSubject().Next(@event);
        }

        public IBusinessTransactionObservable Subscribe(Action<BusinessTransactionEvent> action)
        {
            return GetSubject().Subscribe(action);
        }

        IBusinessTransactionSubject GetSubject()
        {
            return _manager.CurrentSubject();
        }
    }
}
