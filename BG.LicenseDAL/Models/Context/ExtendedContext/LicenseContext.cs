using BG;
using BG.Infrastructure.Process.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Models.Context
{
    public partial class LicenseContext : ILoggerDataContext<DbContext>
    {
        ILoggingAdapter logger;

        //public RostelecomEntities(String connectionString) : base(connectionString) { }

        public ILoggerDataContext<DbContext> RegisterLoggerProvider(Func<DbContext, ILoggingAdapter> loggerProvider)
        {
            Guard.Against<ArgumentException>(loggerProvider == null, "Ошибка регистрации логгера в DbContext: провайдер логгер не определен");
            Guard.Against<ArgumentException>(logger != null, "Ошибка регистрации логгера в DbContext: логгер уже определен");

            //if (logger == null)
            logger = loggerProvider(this);

            return this;
        }

        public ILoggingAdapter GetLogger()
        {
            Guard.Against<ArgumentNullException>(logger == null, "Ошибка получения логгера: логгер не зарегистрирован");

            return logger;
        }

        public DbContext GetContext()
        {
            return this;
        }
    }
}
