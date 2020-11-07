using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Process.Identity;
using BG.Infrastructure.Common.MEF;

namespace BG.Infrastructure.Process.Configuration
{
    /// <summary>
    /// Обеспечивает повторную используемость конфигураций (по аналогии с PartialView в MVC)
    /// </summary>
    public interface IPartialBusinessProcessConfuguration
    {
        bool RegisterPartialBusinessProcess<TContainer, TBusinessProcess, TBusinessProcessConfig, TPartialBusinessProcess>(TBusinessProcessConfig config, string containerAlias, IUser currentUser, string entityName, EntityType entityType,
            string configurationName, BusinessProcessParams @params = null)
            where TBusinessProcessConfig : IBusinessConfig<TContainer, TBusinessProcess>
            where TContainer : class
            where TBusinessProcess : IBusinessProcess
;

    }
}
