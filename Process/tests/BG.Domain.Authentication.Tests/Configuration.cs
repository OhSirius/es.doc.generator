using BG.Infrastructure.Common.Logging;
using BG.Infrastructure.Process.Configuration.Impl.Unity;
using BG.Infrastructure.Process.Identity.Impl;
using BG.Infrastructure.Process.Logging;
using BG.Infrastructure.Process.NCommon.Data.Unity;
using BG.LicenseDAL.Models.Context;
using Microsoft.Practices.Unity;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.Tests
{
    public static class Configuration
    {
        public static IUnityContainer Configure()
        {
            var currentUser = new IdentityUser();

            IUnityContainer container = new UnityContainer();
            UnityBusinessConfigure.Using<IAuthenticationProcess>(container)
                .ConfigureNCommon(c => UnityDefaultNCommonConfig.GetEFWithTransaction<LicenseContext, DbContext>(c, currentUser,
                                     () => new LicenseContext())) //Устанавливаем NCommon
                //.ConfigureSingletonParameter(GetLogger())
                .BuildConfiguration();

            return container;
        }

        //public static Logger GetLogger()
        //{
        //    // var logger = CRM.ServerBusinessModule.CronNode.WorksSheetsGeneration.Logging.LogManager.Instance.GetCurrentClassLogger();
        //    var logger = NLogUtils.GetLoggerFromModuleConfig("Authentication");
        //    return logger;
        //}


    }
}
