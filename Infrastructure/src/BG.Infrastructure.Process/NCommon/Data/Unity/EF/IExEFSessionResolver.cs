using BG.Infrastructure.Process.Logging;
using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.NCommon.Data
{
    public interface IExEFSessionResolver<TLoggingContext,TContext> : IEFSessionResolver
        where TContext : DbContext
        where TLoggingContext : ILoggerDataContext<TContext>
    {
        void RegisterObjectContextProvider(Func<TLoggingContext> contextProvider);
    }
}
