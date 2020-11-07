using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Transactions
{
    public interface IBusinessTransactionSubject: IBusinessTransactionObservable, IBusinessTransactionSubscriber
    {
    }
}
