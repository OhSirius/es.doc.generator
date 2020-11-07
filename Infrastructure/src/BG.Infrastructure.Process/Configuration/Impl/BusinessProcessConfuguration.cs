using BG.Infrastructure.Common.MEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using BG.Extensions;
using NCommon;
using Microsoft.Practices.Unity;
using BG.Infrastructure.Process.Process;
using System.Security.Authentication;
using BG.Infrastructure.Process.Configuration.Unity;
using BG.Infrastructure.Process.Description;
using BG.Infrastructure.Common.Processes;
using BG.Infrastructure.Process.Identity;

namespace BG.Infrastructure.Process.Configuration
{
    public partial class BusinessProcessConfuguration : IBusinessProcessConfuguration
    {
        public const string aDefault = DefautProcessDescription.aUserProcess;//"UserProcess";
        public const string aDefaultDocument = "DefaultDocument";
        public const string aDefaultWorksSheet = "DefaultWorksSheet";
        public const string aDefaultReport = "DefaultReport";
        public const string aNone = "None";

        #region .TemplateConfiguration
        static object locker = new object();
        static BusinessProcessConfuguration()
        {
            lock (locker)
            {
                @default = new BusinessProcessConfuguration();

                //Собираем внеш. модули в ExternalBusinessProcess;
                ModulesLoader.Default.Load(typeof(BusinessProcessConfuguration).Assembly, @default);
            }
        }

        private readonly ModulesMatcher<IBusinessProcessConfuguration> _matcher;

        public BusinessProcessConfuguration()
        {
//            _matcher = new ModulesMatcher<IBusinessProcessConfuguration>(() => ExternalBusinessProcess,
//new Dictionary<EntityType, string>() { { EntityType.Document, aDefaultDocument }, { EntityType.WorksSheet, aDefaultWorksSheet },  { EntityType.Report, aDefaultReport }, { EntityType.None, aNone } }, DefautProcessDescription.aUserProcess);

//            _partMatcher = new ModulesMatcher<IPartialBusinessProcessConfuguration>(() => ExternalPartialBusinessProcess,
//new Dictionary<EntityType, string>() { { EntityType.Document, aDefaultDocument }, { EntityType.WorksSheet, aDefaultWorksSheet },   { EntityType.Report, aDefaultReport }, { EntityType.None, aNone } }, DefautProcessDescription.aUserProcess);

        }
        #endregion

        #region Default
        static BusinessProcessConfuguration @default;
        public static BusinessProcessConfuguration Default
        {
            get
            {
                lock(locker)
                    return @default;
            }
        }
        #endregion

        [ImportMany]
        public IEnumerable<Lazy<IBusinessProcessConfuguration, IEntityMetaData>> ExternalBusinessProcess { set; get; }

        public IEnumerable<IEntityMetaData> GetModulesBusinessProcessMetaData()
        {
            if (ExternalPartialBusinessProcess.IsNullOrEmpty())
                return null;

            var metaData = ExternalBusinessProcess.Select(p => p.Metadata).ToArray();
            return metaData;
        }

        public TBusinessProcess CreateBusinessProcess<TBusinessProcess>(IUser currentUser, string entityName, EntityType entityType, string configurationName, BusinessProcessParams @params = null)
            where TBusinessProcess : IBusinessProcess
        {
            Guard.Against<AuthenticationException>(currentUser == null, string.Format("Ошибка создания объекта бизнес процесса для сущности {0} типа {1} и конфигурацией {2}: не определен текущий пользователь", entityName, entityType, configurationName));

            TBusinessProcess process = default(TBusinessProcess);

            process = _matcher.FindBestMatch(entityName, entityType, configurationName ?? aDefault, config => config.CreateBusinessProcess<TBusinessProcess>(currentUser, entityName, entityType, configurationName ?? aDefault, @params));

            Guard.Against<ArgumentNullException>(process == null, string.Format("Ошибка создания объекта бизнес процесса для сущности {0} типа {1} и конфигурацией {2}: не удалось подобрать подходящий", entityName, entityType, configurationName));
            
            return process;
        }

        public IEnumerable<IEntityMetaData> GetSignalBusinessProcessMetaData()
        {
            if (ExternalBusinessProcess.IsNullOrEmpty())
                return null;

            var metaData = ExternalBusinessProcess.Where(p=>p.Metadata.ProcessType == ProcessType.Signal).Select(p => p.Metadata).ToArray();
            return metaData;
        }

    }

    public partial class BusinessProcessConfuguration: IPartialBusinessProcessConfuguration
    {
        private readonly ModulesMatcher<IPartialBusinessProcessConfuguration> _partMatcher;


        [ImportMany]
        public IEnumerable<Lazy<IPartialBusinessProcessConfuguration, IEntityMetaData>> ExternalPartialBusinessProcess { set; get; }

        public IEnumerable<IEntityMetaData> GetModulesPartialBusinessProcessMetaData()
        {
            if (ExternalPartialBusinessProcess.IsNullOrEmpty())
                return null;

            var metaData = ExternalPartialBusinessProcess.Select(p => p.Metadata).ToArray();
            return metaData;
        }

        public bool RegisterPartialBusinessProcess<TContainer, TBusinessProcess, TBusinessProcessConfig, TPartialBusinessProcess>(
            TBusinessProcessConfig config, string containerAlias, IUser currentUser, string entityName, EntityType entityType, string configurationName,
            BusinessProcessParams @params = null) where TContainer : class where TBusinessProcess : IBusinessProcess where TBusinessProcessConfig : IBusinessConfig<TContainer, TBusinessProcess>
        {
            Guard.Against<AuthenticationException>(currentUser == null, string.Format("Ошибка регистрации частичного бизнес процесса для сущности {0} типа {1}, конфигурации {2} и процесса типа {3}: не определен текущий пользователь", entityName, entityType, configurationName, typeof(TPartialBusinessProcess)));

            bool existsPartials = false;

            existsPartials = _partMatcher.FindBestMatch(entityName, entityType, configurationName ?? aDefault, c => c.RegisterPartialBusinessProcess<TContainer, TBusinessProcess, TBusinessProcessConfig, TPartialBusinessProcess>(config, containerAlias, currentUser, entityName, entityType, configurationName ?? aDefault, @params));

            //Guard.Against<ArgumentException>(!existsPartials, string.Format("Ошибка регистрации частичного бизнес процесса для сущности {0} типа {1}, конфигурации {2} и процесса типа {3}: не удалось найти подходящую конфигурацию", entityName, entityType, configurationName, typeof(TPartialBusinessProcess)));

            return existsPartials;

        }         
    }


}
