using NCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Data
{
    public interface IUnitOfWorkScopeFactory
    {
        IUnitOfWorkScope Create(TransactionMode mode = TransactionMode.Default);
    }
}
