using Microsoft.Practices.Unity;
using NCommon;
using NCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    public class UnityUnitOfWorkScopeFactory : IUnitOfWorkScopeFactory
    {
        readonly IUnityContainer _container;

        public UnityUnitOfWorkScopeFactory(IUnityContainer container)
        {
            Guard.Against<ArgumentNullException>(container == null, "Ошибка создания фабрики UnityUnitOfWorkScopeFactory: не задан Unity - контейнер");

            this._container = container;
        }

        public IUnitOfWorkScope Create(TransactionMode mode = TransactionMode.Default)
        {
            return _container.Resolve<IUnitOfWorkScope>(new ParameterOverride("mode", mode));
        }
    }
}
