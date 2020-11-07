 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Transactions.Impl
{
    public class BusinessTransactionSubjectFactory: IBusinessTransactionSubjectFactory
    {
        readonly Func<IBusinessTransactionSubject> _getSubject;

        public BusinessTransactionSubjectFactory(Func<IBusinessTransactionSubject> getSubject)
        {
            _getSubject = getSubject;
        }


        public IBusinessTransactionSubject Get()
        {
            Guard.AssertNotNull(_getSubject, "не определен делегат для получения subject");

            return _getSubject();
        }
    }
}
