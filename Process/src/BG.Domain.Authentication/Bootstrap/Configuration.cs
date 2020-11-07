using BG.Domain.Authentication.Configurations;
using BG.Domain.Authentication.Configurations.Impl;
using BG.Domain.Authentication.Repositories;
using BG.Domain.Authentication.Repositories.Impl;
using BG.Infrastructure.Common.Logging;
using BG.Infrastructure.Process.Configuration.Impl.Unity;
using BG.Infrastructure.Process.Identity.Impl;
using BG.Infrastructure.Process.Logging;
using BG.Infrastructure.Process.NCommon.Data.Unity;
using BG.Infrastructure.Process.Process;
using BG.LicenseDAL.Models.Context;
using Microsoft.Practices.Unity;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.Bootstrap
{
    public static class Configuration
    {
        public static IAuthenticationProcess Configure(IUnityContainer rootContainer = null, bool isDegugMode = false)
        {
            //Guard.AssertNotEmpty(sourcePath, "Не определен путь источника");

            var currentUser = new IdentityUser();
            var config = new AuthenticationConfiguration() { DebugMode = isDegugMode };

            IUnityContainer container = rootContainer == null ? new UnityContainer() : rootContainer.CreateChildContainer();
            UnityBusinessConfigure.Using<IAuthenticationProcess>(container)
                .ConfigureNCommon(c => UnityDefaultNCommonConfig.GetEFWithTransaction<LicenseContext, DbContext>(c, currentUser,
                                     () => new LicenseContext())) //Устанавливаем NCommon
                .ConfigureSingletonParameter(GetLogger())
                .ConfigureSingletonParameter<IAuthenticationConfiguration>(config)
                .ConfigureOperation<IAccountRepository, AccountRepository>()
                .ConfigureOperation<ILogonHistoryRepository, LogonHistoryRepository>()
                .ConfigureProcess<IAuthenticationProcess, AuthenticationProcess>()
                .BuildConfiguration();

            return container.Resolve<IAuthenticationProcess>();
        }

        public static Logger GetLogger()
        {
            // var logger = CRM.ServerBusinessModule.CronNode.WorksSheetsGeneration.Logging.LogManager.Instance.GetCurrentClassLogger();
            var logger = NLogUtils.GetLoggerFromModuleConfig("Authentication");
            return logger;
        }

    }

}
