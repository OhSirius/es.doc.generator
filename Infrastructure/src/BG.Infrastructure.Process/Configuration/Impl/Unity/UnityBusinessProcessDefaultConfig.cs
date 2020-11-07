using BG.Infrastructure.Process.Configuration.Impl.Unity;
using BG.Infrastructure.Process.Description;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Common.Utility;
using Microsoft.Practices.Unity;
using NCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Configuration.Unity
{
    //public class UnityBusinessProcessDefaultConfiguration : IBusinessProcessConfuguration
    //{
    //    public TBusinessProcess CreateBusinessProcess<TEntity, TBusinessProcess>(Aetp.Common.DataBase.User currentUser, string entityName, EntityType entityType, string configurationName)
    //        where TEntity : class
    //        where TBusinessProcess : class, IEntityBusinessProcess<TEntity>
    //    {
    //        Guard.Against<ArgumentException>(typeof(TEntity) != typeof(BaseObject), string.Format("Ошибка создания бизнесс-процесса по умолчанию: тип {0} не поддерживается", typeof(TEntity)));

    //        IUnityContainer container = new UnityContainer();
    //        var configure = CreateConfiguration(container, currentUser, entityName);

    //        configure.BuildConfiguration();
    //        var process = container.Resolve<IDocumentBusinessProcess<BaseObject>>() as TBusinessProcess;
    //        return process;
    //    }

    //    protected virtual IBusinessEntityConfig<IUnityContainer, BaseObject, IDocumentBusinessProcess<BaseObject>> CreateConfiguration(IUnityContainer container, User currentUser, string entityName)
    //    {
    //        return UnityBusinessEntityConfigure.Using<BaseObject, IDocumentBusinessProcess<BaseObject>>(container)
    //                      .ConfigureSingletonParameter<User>(currentUser) 
    //                      .ConfigureSingletonParameter<IUser>(currentUser) 
    //                      .ConfigureOperation<ICreateDocumentOperation<BaseObject>, DefaultCreateDocumentOperation>(new ConstructorParameterCollection() { { "objectName", entityName } })
    //                      .ConfigureOperation<IDocumentInitialStateOperation, DefaultCreateDocumentOperation>(new ConstructorParameterCollection() { { "objectName", entityName } })
    //                      .ConfigureProcess<IDocumentBusinessProcess<BaseObject>, DefaultDocumentBusinessProcess<BaseObject>>();
    //    }


    //    #region Old
    //    //protected virtual IBusinessEntityConfig<IUnityContainer, TEntity, DefaultDocumentBusinessProcess<TEntity>> CreateConfiguration<TEntity>(IUnityContainer container, User currentUser, string entityName) where TEntity : class
    //    //{
    //    //    var @using = UnityBusinessEntityConfigure.Using<TEntity, DefaultDocumentBusinessProcess<TEntity>>(container);

    //    //    var configure = @using.ConfigureOperation<UnityBusinessEntityOperationConfig>(BusinessProcessDescription.aCreateDocument,
    //    //                                config =>
    //    //                                    config.Register(c =>
    //    //                                    {
    //    //                                        c.RegisterType<ICreateDocumentOperation<BaseObject>, DefaultCreateDocumentOperation>
    //    //                                            (
    //    //                                                BusinessProcessDescription.aCreateDocument,
    //    //                                                new InjectionConstructor(new InjectionParameter<string>(entityName),
    //    //                                                                        typeof(string),
    //    //                                                                        typeof(object),
    //    //                                                                        new InjectionParameter<User>(currentUser)
    //    //                                                                        )
    //    //                                            );
    //    //                                    }
    //    //                                                    ), false);

    //    //    configure = configure.ConfigureOperation<UnityBusinessEntityOperationConfig>(BusinessProcessDescription.aInitialState,
    //    //        config =>
    //    //            config.Register(c =>
    //    //            {
    //    //                c.RegisterType<IDocumentInitialStateOperation, DefaultCreateDocumentOperation>
    //    //                    (
    //    //                        BusinessProcessDescription.aInitialState,
    //    //                        new InjectionConstructor()//entityName, new InjectionParameter(typeof(string), null),
    //    //                    //           new InjectionParameter(typeof(object), null))

    //    //                    );
    //    //            }
    //    //                             ), false);

    //    //    configure = configure.ConfigureProcess<UnityBusinessEntityProcessConfig<TEntity, DefaultDocumentBusinessProcess<TEntity>>>
    //    //                            (
    //    //                                  config => config.Register(
    //    //                                                            c => c.RegisterType(typeof(IDocumentBusinessProcess<BaseObject>), typeof(DefaultDocumentBusinessProcess<TEntity>),
    //    //                                                                new InjectionFactory((UnCon, tp, s) => new BG.Infrastructure.Process.Process.DefaultDocumentBusinessProcess<TEntity>
    //    //                                                                                                        (
    //    //                                                                                                            (parentName, parentID) =>
    //    //                                                                                                            {
    //    //                                                                                                                var parameters = new ParameterOverrides();
    //    //                                                                                                                parameters.Add("parentName", new InjectionParameter<string>(parentName));
    //    //                                                                                                                parameters.Add("parentID", new InjectionParameter<object>(parentID));
    //    //                                                                                                                return UnCon.Resolve<ICreateDocumentOperation<TEntity>>(BusinessProcessDescription.aCreateDocument, parameters);
    //    //                                                                                                            },
    //    //                                                                                                            UnCon.Resolve<IDocumentInitialStateOperation>(BusinessProcessDescription.aInitialState)
    //    //                                                                                                        )
    //    //                                                                                    )
    //    //                                                                                )
    //    //                                                            )
    //    //                            );
    //    //    return configure;

    //    //}
    //    #endregion


    //}
}
