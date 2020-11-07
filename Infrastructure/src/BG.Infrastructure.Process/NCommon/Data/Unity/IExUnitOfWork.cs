using NCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    public interface IExUnitOfWork : IUnitOfWork
    {
        void AfterFlushing();
    }
}
