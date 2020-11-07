using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Transactions
{
    /// <summary>
    /// Реализует подписку на транзакцию
    /// </summary>
    public interface IBusinessTransactionObservable
    {
        IBusinessTransactionObservable Subscribe(Action<BusinessTransactionEvent> action);
    }
}
