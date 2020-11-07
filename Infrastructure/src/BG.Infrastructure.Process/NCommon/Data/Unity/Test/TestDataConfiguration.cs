using NCommon;
using NCommon.Configuration;
using NCommon.Data;
using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.Logging;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.Test
{
    /// <summary>
    /// Implementatio of <see cref="IDataConfiguration"/> that configured NCommon to use Linq To Sql
    /// </summary>
    public class TestDataConfiguration : IDataConfiguration
    {
        private Action<IContainerAdapter> _registeredActions = null;

        public TestDataConfiguration Register(Action<IContainerAdapter> registeredActions)
        {
            _registeredActions = registeredActions;
            return this;
        }

        /// <summary>
        /// Called by NCommon <see cref="Configure"/> to configure data providers.
        /// </summary>
        /// <param name="containerAdapter">The <see cref="IContainerAdapter"/> instance that allows
        /// registering components.</param>
        public void Configure(IContainerAdapter containerAdapter)
        {
            Guard.Against<ArgumentNullException>(_registeredActions == null, "Не определен контейнер");
            //Guard.Against<NotSupportedException>(loggerProvider==null, "Ошибка установки конфигурации EF: не определен логгер");
            //Guard.Against<NotSupportedException>(!(objectContextProvider() is ILoggerDataContext<DbContext>), "Ошибка установки конфигурации EF: дата контекст не поддерживает ILoggerDataContext");

            //(objectContextProvider() as ILoggerDataContext<DbContext>).LoggerProvider = loggerProvider;

            //containerAdapter.RegisterInstance<IUnitOfWorkFactory>(_factory);
            //containerAdapter.RegisterGeneric(typeof(IRepository<>), typeof(UnityEFRepository<>),);
            _registeredActions(containerAdapter);

        }
    }
}
