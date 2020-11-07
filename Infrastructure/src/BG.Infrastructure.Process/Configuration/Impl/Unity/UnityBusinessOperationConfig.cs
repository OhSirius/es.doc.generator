using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;

namespace BG.Infrastructure.Process.Configuration.Impl.Unity
{
    public class UnityBusinessOperationConfig : IBusinessOperationConfig<IUnityContainer>
    {
        protected readonly List<Action<IUnityContainer>> regActions = new List<Action<IUnityContainer>>();

        public IBusinessOperationConfig<IUnityContainer> Register(Action<IUnityContainer> actions)
        {
            Guard.Against<ArgumentNullException>(actions == null, "Ошибка составления конфигурации стратегии для тек. объекта: не задан делегат действий");

            regActions.Add(actions);
            return this;
        }

        public void Confugure(IUnityContainer container)
        {
            Guard.Against<ArgumentNullException>(container == null, "Ошибка составления конфигурации стратегии для тек. объекта: не задан Unity- контейнер");

            if (regActions.IsNullOrEmpty())
                return;

            regActions.ForEach(a => a(container));
        }

    }
}
