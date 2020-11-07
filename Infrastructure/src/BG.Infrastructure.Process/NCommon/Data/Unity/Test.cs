using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using NCommon.Configuration;

namespace BG.Infrastructure.Process.NCommon.Unity
{
    public static class Test
    {
        //public static void Use_ServiceLocator()
        //{
        //    var dataContext = new DataClasses1DataContext();
        //    var container = new UnityContainer();
        //    var adapter = new UnityContainerAdapter(container);
        //    NCommon.Configure.Using(adapter)
        //        .ConfigureState<DefaultStateConfiguration>()
        //        .ConfigureData<LinqToSqlConfiguration>(config => config.WithDataContext(() => dataContext))
        //        .ConfigureUnitOfWork<DefaultUnitOfWorkConfiguration>(config => config.AutoCompleteScope());

        //    var serviceLocator = new UnityServiceLocator(container);
        //    Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => serviceLocator);

        //    IRepository<Company> repository = container.Resolve<IRepository<Company>>();

        //    using (var scope = new UnitOfWorkScope())
        //    {
        //        var com = repository.FirstOrDefault();
        //        repository.Add(new Company() { Name = "Test", Comment = "1232131" });
        //        scope.Commit();
        //    }
        //}

        //public static void Use_Without_ServiceLocator()
        //{
        //    var dataContext = new DataClasses1DataContext();
        //    var container = new UnityContainer();
        //    var adapter = new UnityContainerAdapter(container);
        //    NCommon.Configure.Using(adapter)
        //        .ConfigureState<DefaultStateConfiguration>()
        //        .ConfigureData<UnityLinqToSqlConfiguration>(config => config.WithDataContext(() => dataContext))
        //        .ConfigureUnitOfWork<UnityUnitOfWorkConfiguration>(config =>
        //            config.WithTransactionManager(settings => new UnityTransactionManager(container, settings))
        //        /*.AutoCompleteScope()*/);

        //    //Fix ServiceLocator.Current - set null (It can avoid if Ritesh will commit my fix :))
        //    //Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => null);

        //    IUnitOfWorkScopeFactory scopeFactory = container.Resolve<IUnitOfWorkScopeFactory>();
        //    IRepository<Company> repository = container.Resolve<IRepository<Company>>();

        //    using (var scope = scopeFactory.Create())
        //    {
        //        var com = repository.FirstOrDefault();
        //        repository.Add(new Company() { Id = 101, Name = "Test", Comment = "1232131" });
        //        scope.Commit();
        //    }
        //}

    }

}
