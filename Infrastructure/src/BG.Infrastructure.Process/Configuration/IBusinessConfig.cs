using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Common.Utility;
using NCommon.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Configuration
{
    /// <summary>
    /// Предоставляет Fluent-синтаксис для задания конфигурации Бизнес-объекта при выполнении операции над объектом
    /// </summary>
    public interface IBusinessConfig<TContainer, TBusinessProcess>
        where TContainer : class
        where TBusinessProcess : IBusinessProcess
    {
        //NCommon
        IBusinessConfig<TContainer, TBusinessProcess> ConfigureNCommon(Func<TContainer, INCommonConfig> getNCommonConfig, bool updateIfExist = false);
        IBusinessConfig<TContainer, TBusinessProcess> ConfigureNCommonActions(Action<INCommonConfig> actions, bool updateIfExist = false);

        //Стратегии
        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation<T>(string operationName, Action<T> actions, bool updateIfExist)
            where T : IBusinessOperationConfig<TContainer>, new();

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation(BusinessOperationConfigCollection<TContainer> configs, bool updateIfExist);

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(string operationName, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(string operationName, ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(Func<TContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperation<TService, TImplementation>(string operationName, Func<TContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService;


        //Синглетон параметры
        IBusinessConfig<TContainer, TBusinessProcess> ConfigureSingletonParameter<TService>(TService implementation, bool updateIfExist = false);

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(string operationName, ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(Func<TContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureSingletonParameter<TService, TImplementation>(string operationParameter, Func<TContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService;


        //Параметры для стратегий
        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(string parameterName, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(string parameterName, ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(ConstructorParameterCollection constructorArguments, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(Func<TContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationParameter<TService, TImplementation>(string operationParameter, Func<TContainer, TImplementation> getImpl, bool updateIfExist = false) where TImplementation : TService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationGenericParameter(Type serviceType, Type implementationType, bool updateIfExist = false);

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureOperationGenericParameter(string parameterName, Type serviceType, Type implementationType, bool updateIfExist = false);

        //Процесс
        IBusinessConfig<TContainer, TBusinessProcess> ConfigureProcess<T>(Action<T> actions)
            where T : IBusinessProcessConfig<TContainer, TBusinessProcess>, new();

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureProcess<TBusinessProcessService, TBusinessProcessImplementation>(Func<TContainer,TBusinessProcessImplementation> getProcess)
            where TBusinessProcessImplementation : TBusinessProcessService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureProcess<TBusinessProcessService, TBusinessProcessImplementation>()
            where TBusinessProcessImplementation : TBusinessProcessService;

        IBusinessConfig<TContainer, TBusinessProcess> ConfigureProcess<TBusinessProcessService, TBusinessProcessImplementation>(ConstructorParameterCollection constructorArguments)
            where TBusinessProcessImplementation : TBusinessProcessService;

        void BuildConfiguration();
    }
}
