using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Transactions
{
    /// <summary>
    /// Реализует отправку события всем подписчикам
    /// </summary>
    public interface IBusinessTransactionSubscriber
    {
        void Attach(object scope);

        void Detach(object scope);

        void Next(BusinessTransactionEvent @event);
    }
}
