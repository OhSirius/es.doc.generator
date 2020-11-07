using BG.Infrastructure.Process.Transactions;
using Microsoft.Practices.Unity;
using NCommon;
using NCommon.Data;
using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.EF.Impl
{
    public class UnityEFRepository<TEntity> : EFRepository<TEntity> where TEntity : class
    {
        readonly IUnityUnitOfWorkManager _manager;

        public UnityEFRepository(IUnityContainer container, IUnityUnitOfWorkManager manager)
        {
            Guard.Against<ArgumentNullException>(container == null, "Ошибка создания репозитория для EF: не задан исходный Unity-контейнер");
            Container = container;
            _manager = manager;
            var sessions = Container.ResolveAll<IEFSession>();
            if (sessions != null && sessions.Count() > 0)
                _privateSession = sessions.First();
        }

        protected IUnityContainer Container { set; get; }

        public override T UnitOfWork<T>()
        {
            //var currentScope = Container.Resolve<ITransactionManager>().CurrentUnitOfWork;
            var currentScope = _manager.CurrentUnitOfWork();
            Guard.Against<InvalidOperationException>(currentScope == null,
                                                     "No compatible UnitOfWork was found. Please start a compatible UnitOfWorkScope before " +
                                                     "using the repository.");

            Guard.TypeOf<T>(currentScope,
                                              "The current UnitOfWork instance is not compatible with the repository. " +
                                              "Please start a compatible unit of work before using the repository.");
            return ((T)currentScope);
        }

        protected override void Initialize()
        {
        }

        public override IQueryable<TEntity> For<TService>()
        {
            var strategy = Container.ResolveAll<IFetchingStrategy<TEntity, TService>>()
                                    .FirstOrDefault();

            if (strategy != null)
                return strategy.Define(this);
            return this;
        }

    }

}
