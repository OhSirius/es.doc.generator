using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Logging
{
    public interface ILoggerDataContext<TDataContext> where TDataContext : class
    {
        ILoggerDataContext<TDataContext> RegisterLoggerProvider(Func<TDataContext, ILoggingAdapter> loggerProvider);
        ILoggingAdapter GetLogger();
        TDataContext GetContext();
    }
}
