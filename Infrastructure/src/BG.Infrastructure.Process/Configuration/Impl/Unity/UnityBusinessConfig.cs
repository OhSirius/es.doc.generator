using Microsoft.Practices.Unity;
using NCommon.Configuration;
using NCommon.ContainerAdapter.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;
using Configure = NCommon.Configure;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Process.Extensions;
using NCommon.Data;
using BG.Infrastructure.Common.Unity.Injection;
using BG.Infrastructure.Common.Utility;

namespace BG.Infrastructure.Process.Configuration.Impl.Unity
{
    public class UnityBusinessConfig<TBusinessProcess> : IBusinessConfig<IUnityContainer, TBusinessProcess>
        where TBusinessProcess : IBusinessProcess
    {
        public UnityBusinessConfig(IUnityContainer container)
        {
            Container = container;
            BusinessOperationConfigs = BusinessOperationConfigCollection<IUnityContainer>.Create();
        }

        protected IUnityContainer Container { set; get; }

        protected Action<INCommonConfig> NCommonConfigActions { set; get; }

        protected Func<IUnityContainer, INCommonConfig> NCommonConfigGets { set; get; }

        protected BusinessOperationConfigCollection<IUnityContainer> BusinessOperationConfigs { set; get; }

        protected IBusinessProcessConfig<IUnityContainer,  TBusinessProcess> ProcessConfig { set; get; }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureNCommon(Func<IUnityContainer, INCommonConfig> getNCommonConfig, bool updateIfExist = false)
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации NCommon: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(getNCommonConfig == null, "Ошибка определения конфигурации NCommon: не определен метод получения конфигурации");
            //Guard.Against<ArgumentNullException>(NCommonConfigActions != null, "Ошибка определения конфигурации NCommon: конфигурация уже определена");
            //Guard.Against<ArgumentNullException>(!updateIfExist && (NCommonConfigGets != null || NCommonConfigActions != null), "Ошибка определения конфигурации NCommon: конфигурация уже определена");

            if (Container != null && Container.ExistsType<IUnitOfWorkScope>())//Если NCommon уже зарегистрирован, то регистрация отменяется
                return this;

            NCommonConfigGets = getNCommonConfig;
            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureNCommonActions(Action<INCommonConfig> actions, bool updateIfExist = false)
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации NCommon: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(actions == null, "Ошибка определения конфигурации NCommon: не определены действия конфигурации");
            Guard.Against<ArgumentNullException>(NCommonConfigGets != null, "Ошибка определения конфигурации NCommon: конфигурация уже определена");
            Guard.Against<ArgumentNullException>(!updateIfExist && NCommonConfigActions != null, "Ошибка определения конфигурации NCommon: конфигурация уже определена");

            if (Container != null && Container.ExistsType<IUnitOfWorkScope>())//Если NCommon уже зарегистрирован, то регистрация отменяется
                return this;

            NCommonConfigActions = actions;
            return this;
        }


        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation<T>(string operationName, Action<T> actions, bool updateIfExist) where T : IBusinessOperationConfig<IUnityContainer>, new()
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка определения конфигурации Operation: не определено название стратегии");
            Guard.Against<ArgumentNullException>(actions == null, "Ошибка определения конфигурации Operation: не определены действия конфигурации");

            var config = new T();
            actions(config);
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation(BusinessOperationConfigCollection<IUnityContainer> configs, bool updateIfExist)
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");

            if (configs.IsNullOrEmpty())
                return this;

            if (updateIfExist)
                configs.ForEach(config => BusinessOperationConfigs.Update(config.Key, config.Value));
            else
                configs.ForEach(config => BusinessOperationConfigs.New(config.Key, config.Value));

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(string operationName, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка определения конфигурации Operation: не определено название стратегии");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(operationName));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(string operationName, ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка определения конфигурации Operation: не определено название стратегии");
            Guard.Against<ArgumentNullException>(constructorArguments.IsNullOrEmpty(), "Ошибка определения конфигурации Operation: не определены аргументы конструктора");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(operationName, new SmartConstructor(constructorArguments)));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>());
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(constructorArguments.IsNullOrEmpty(), "Ошибка определения конфигурации Operation: не определены аргументы конструктора");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(new SmartConstructor(constructorArguments)));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }


        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureSingletonParameter<TService>(TService implementation, bool updateIfExist = false)
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(implementation==null, "Ошибка определения конфигурации Operation: не определен объект implementation");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterInstance<TService>(implementation));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(bool updateIfExist = false) where TImplementation:TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            //Guard.Against<ArgumentNullException>(implementation==null, "Ошибка определения конфигурации Operation: не определен объект implementation");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(new ContainerControlledLifetimeManager()));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer,  TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation:TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            //Guard.Against<ArgumentNullException>(implementation==null, "Ошибка определения конфигурации Operation: не определен объект implementation");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(new ContainerControlledLifetimeManager(), new SmartConstructor(constructorArguments)));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer,  TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(string operationName, ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation:TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            //Guard.Against<ArgumentNullException>(implementation==null, "Ошибка определения конфигурации Operation: не определен объект implementation");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(operationName, new ContainerControlledLifetimeManager(),  new SmartConstructor(constructorArguments)));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(Func<IUnityContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.AssertNotNull(getImpl, "");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(typeof(TService), typeof(TImplementation), new ContainerControlledLifetimeManager(), new InjectionFactory((UnCon, tp, s) => getImpl(UnCon))));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);
            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(string operationName, Func<IUnityContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.AssertNotNull(getImpl, "");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(typeof(TService), typeof(TImplementation), operationName, new ContainerControlledLifetimeManager(), new InjectionFactory((UnCon, tp, s) => getImpl(UnCon))));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);
            return this;
        }


        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(Func<IUnityContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.AssertNotNull(getImpl, "");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(typeof(TService), typeof(TImplementation), new TransientLifetimeManager(), new InjectionFactory((UnCon, tp, s) => getImpl(UnCon))));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);
            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(string operationName, Func<IUnityContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.AssertNotNull(getImpl, "");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(typeof(TService), typeof(TImplementation), operationName, new InjectionFactory((UnCon, tp, s) => getImpl(UnCon))));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);
            return this;
        }

        #region Operation parameters

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationGenericParameter(Type serviceType, Type implementationType, bool updateIfExist = false)
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            //Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка определения конфигурации Operation: не определено название стратегии");

            string operationName = serviceType.FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(serviceType,implementationType));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationGenericParameter(string parameterName, Type serviceType, Type implementationType,
            bool updateIfExist = false)
        {
            Guard.AssertNotEmpty(parameterName, "Не определено имя параметра");
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            //Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка определения конфигурации Operation: не определено название стратегии");

          
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(serviceType, implementationType, parameterName, new TransientLifetimeManager()));
            if (updateIfExist)
                BusinessOperationConfigs.Update(parameterName, config);
            else
                BusinessOperationConfigs.New(parameterName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(string operationName, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка определения конфигурации Operation: не определено название стратегии");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(operationName));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(string operationName, ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка определения конфигурации Operation: не определено название стратегии");
            Guard.Against<ArgumentNullException>(constructorArguments.IsNullOrEmpty(), "Ошибка определения конфигурации Operation: не определены аргументы конструктора");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(operationName, new SmartConstructor(constructorArguments)));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>());
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");
            Guard.Against<ArgumentNullException>(constructorArguments.IsNullOrEmpty(), "Ошибка определения конфигурации Operation: не определены аргументы конструктора");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType<TService, TImplementation>(new SmartConstructor(constructorArguments)));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);

            return this;
        }



        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(Func<IUnityContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.AssertNotNull(getImpl, "");

            string operationName = typeof(TService).FullName;
            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(typeof(TService), typeof(TImplementation), new InjectionFactory((UnCon, tp, s) => getImpl(UnCon))));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);
            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(string operationName, Func<IUnityContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService
        {
            Guard.AssertNotNull(getImpl, "");

            var config = new UnityBusinessOperationConfig();
            config.Register(c => c.RegisterType(typeof(TService), typeof(TImplementation), operationName, new InjectionFactory((UnCon, tp, s) => getImpl(UnCon))));
            if (updateIfExist)
                BusinessOperationConfigs.Update(operationName, config);
            else
                BusinessOperationConfigs.New(operationName, config);
            return this;
        }

        #endregion


        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureProcess<T>(Action<T> actions) where T : IBusinessProcessConfig<IUnityContainer, TBusinessProcess>, new()
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка определения конфигурации Operation: не определен Unity контейнер");

            var config = new T();
            actions(config);
            ProcessConfig = config;
            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureProcess<TBusinessProcessService, TBusinessProcessImplementation>(Func<IUnityContainer, TBusinessProcessImplementation> getProcess)
            where TBusinessProcessImplementation : TBusinessProcessService
        {
            Guard.Against<ArgumentNullException>(getProcess == null, "Ошибка определения конфигурации: не определен метод создания процесса");

            var config = new UnityBusinessProcessConfig<TBusinessProcess>();
            config.Register(c => c.RegisterType(typeof(TBusinessProcessService), typeof(TBusinessProcessImplementation), new InjectionFactory((UnCon, tp, s) => getProcess(UnCon))));
            ProcessConfig = config;

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureProcess<TBusinessProcessService, TBusinessProcessImplementation>()
            where TBusinessProcessImplementation : TBusinessProcessService
        {
            var config = new UnityBusinessProcessConfig<TBusinessProcess>();
            config.Register(c => c.RegisterType<TBusinessProcessService,TBusinessProcessImplementation>());
            ProcessConfig = config;

            return this;
        }

        public IBusinessConfig<IUnityContainer, TBusinessProcess> ConfigureProcess<TBusinessProcessService, TBusinessProcessImplementation>(ConstructorParameterCollection constructorArguments)
            where TBusinessProcessImplementation : TBusinessProcessService
        {
            Guard.Against<ArgumentNullException>(constructorArguments.IsNullOrEmpty(), "Ошибка определения конфигурации Operation: не определены аргументы конструктора");

            var config = new UnityBusinessProcessConfig<TBusinessProcess>();
            config.Register(c => c.RegisterType<TBusinessProcessService, TBusinessProcessImplementation>(new SmartConstructor(constructorArguments)));
            ProcessConfig = config;

            return this;
        }

        public void BuildConfiguration()
        {
            Guard.Against<ArgumentNullException>(Container == null, "Ошибка построения конфигурации: не определен Unity - контейнер");
            Guard.Against<ArgumentNullException>(NCommonConfigActions != null && NCommonConfigGets != null, "Ошибка построения конфигурации: определено две взаимоисключающие конфигурации NCommon");

            try
            {

                if (NCommonConfigActions != null)
                {
                    var adapter = new UnityContainerAdapter(Container);
                    NCommonConfigActions(Configure.Using(adapter));
                }
                else if (NCommonConfigGets != null)
                    NCommonConfigGets(Container);

                if (!BusinessOperationConfigs.IsNullOrEmpty())
                    BusinessOperationConfigs.ForEach(config => config.Value.Confugure(Container));

                if (ProcessConfig != null)
                    ProcessConfig.Confugure(Container);

                //Validate
                var validStr = Container.GetMappingAsString();
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
