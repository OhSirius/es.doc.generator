using BG.Infrastructure.Process.Process;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Configuration.Impl.Unity
{
    public static class UnityBusinessConfigure
    {
        public static IBusinessConfig<IUnityContainer, TBusinessProcess> Using<TBusinessProcess>(IUnityContainer container)
            where TBusinessProcess : IBusinessProcess
        {
            Guard.Against<ArgumentNullException>(container == null, "Ошибка составления конфигурации для тек. объекта: не задан Unity- контейнер ");

            var config = new UnityBusinessConfig<TBusinessProcess>(container);

            return config;
        }

    }
}
