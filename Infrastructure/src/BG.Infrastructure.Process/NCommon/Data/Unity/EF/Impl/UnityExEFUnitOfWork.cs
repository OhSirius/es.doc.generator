using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.EF.Impl
{
    public class UnityExEFUnitOfWork : EFUnitOfWork, IExUnitOfWork
    {
        public UnityExEFUnitOfWork(IEFSessionResolver resolver) : base(resolver) { }

        public void AfterFlushing()
        {
            _openSessions.Where(session=>session.Value is IExEFSession).ForEach(session => ((IExEFSession)session.Value).AfterSaveChanges());
        }
    }
}
