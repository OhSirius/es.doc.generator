using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;
using BG.Infrastructure.Process.Process;

namespace BG.Infrastructure.Process.Configuration.Impl.Unity
{
    public class UnityBusinessProcessConfig<TBusinessProcess> : IBusinessProcessConfig<IUnityContainer, TBusinessProcess>
        where TBusinessProcess : IBusinessProcess
    {
        protected readonly List<Action<IUnityContainer>> regActions = new List<Action<IUnityContainer>>();

        public IBusinessProcessConfig<IUnityContainer, TBusinessProcess> Register(Action<IUnityContainer> actions)
        {
            Guard.Against<ArgumentNullException>(actions == null, "Ошибка составления конфигурации бизнес-процесса для тек. объекта: не задан делегат действий");

            regActions.Add(actions);
            return this;
        }

        public void Confugure(IUnityContainer container)
        {
            Guard.Against<ArgumentNullException>(container == null, "Ошибка составления конфигурации бизнес-процесса для тек. объекта: не задан Unity- контейнер");

            if (regActions.IsNullOrEmpty())
                return;

            regActions.ForEach(a => a(container));
        }
    }
}
