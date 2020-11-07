 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Transactions.Impl
{
    public class StubTransactionSubject : IBusinessTransactionSubject
    {

        public void Attach(object scope)
        {

        }

        public void Detach(object scope)
        {

        }


        public void Next(BusinessTransactionEvent @event)
        {

        }

        public IBusinessTransactionObservable Subscribe(Action<BusinessTransactionEvent> action)
        {
            Guard.AssertNotNull(action, "Невозможно выполнить подписку на транзакцию  - не определен обработчик");

            action(null);
            return this;
        }

    }
}
