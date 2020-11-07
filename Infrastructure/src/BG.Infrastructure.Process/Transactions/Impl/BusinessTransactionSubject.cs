using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Transactions.Impl
{
    public class BusinessTransactionSubject : IBusinessTransactionSubject
    {
        readonly List<object> _transactions = new List<object>();

        readonly List<Action<BusinessTransactionEvent>> queues = new List<Action<BusinessTransactionEvent>>();

        public void Attach(object scope)
        {
            Guard.AssertNotNull(scope, "Не определен scope для привязки");

            if (!_transactions.Contains(scope))
                _transactions.Add(scope);
        }

        public void Detach(object scope)
        {
            Guard.AssertNotNull(scope, "Не определен scope для привязки");

            if (_transactions.Contains(scope))
                _transactions.Remove(scope);

            if (!_transactions.Any())
                queues.Clear();
        }


        public void Next(BusinessTransactionEvent @event)
        {
            Guard.AssertNotNull(@event, "Невозможно зафиксироватьподписку на транзакцию  - не определено событие");

            queues.ForEach(a => a(@event));
            queues.Clear();
        }

        public IBusinessTransactionObservable Subscribe(Action<BusinessTransactionEvent> action)
        {
            Guard.AssertNotNull(action, "Невозможно выполнить подписку на транзакцию  - не определен обработчик");

            if (!_transactions.Any())
                action(null);
            else
                queues.Add(action);

            return this;
        }

    }
}
