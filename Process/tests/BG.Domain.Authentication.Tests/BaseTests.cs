using BG.Domain.Authentication.Tests.Database.LocalDB;
using BG.Extensions;
using BG.Infrastructure.Process.NCommon.Data;
using BG.Infrastructure.Process.NCommon.Extensions;
using BG.LicenseDAL.Models;
using Microsoft.Practices.Unity;
using NCommon.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.Tests
{
    [TestFixture]
    public abstract class BaseTest
    {

        //-------------------------------------------------------------------------------------------------------------

        protected IUnityContainer _container;
        protected LocalDB _localdb;

        //-------------------------------------------------------------------------------------------------------------

        [OneTimeSetUp]
        public virtual void FixtureSetup()
        {
            _localdb = new LocalDB();
            //_localdb.ClearTables();
            //_localdb.FillTables();
            _localdb.ReseedTables();

            _container = Configuration.Configure();
        }

        [OneTimeTearDown]
        public virtual void FixtureCleanup()
        {
            //_localdb.ClearTables();
            //_localdb.FillTables();
        }

        [SetUp]
        public void Init()
        {
            //_localdb.ClearTables();
            //_localdb.FillTables();

            var appRepository = _container.Resolve<IRepository<Application>>();
            var typeRepository = _container.Resolve<IRepository<LicenseType>>();

            using (var scope = _container.Resolve<IUnitOfWorkScopeFactory>().Create())
            {
                var application = CreateApplication();
                appRepository.Add(application);
                CreateLicenseTypes(application).ForEach(l => typeRepository.Add(l));
                scope.Commit();
            }
        }

        [TearDown]
        public void Cleanup()
        {
            //_localdb.ClearTables();
            //_localdb.FillTables();

            var appRepository = _container.Resolve<IRepository<Application>>();
            var typeRepository = _container.Resolve<IRepository<LicenseType>>();

            using (var scope = _container.Resolve<IUnitOfWorkScopeFactory>().Create())
            {
                typeRepository.ToArray().ForEach(l => typeRepository.Delete(l));
                appRepository.ToArray().ForEach(a => appRepository.Delete(a));
                scope.Commit();
            }

            _localdb.ReseedTables();
        }

        //-------------------------------------------------------------------------------------------------------------

        protected Application CreateApplication()
        {
            return new Application() { Name = "DocumentGenerator" };
        }

        protected IEnumerable<LicenseType> CreateLicenseTypes(Application application)
        {
            yield return new LicenseType() { Name = "Lite", Application = application };
            yield return new LicenseType() { Name = "Basis", Application = application };
            yield return new LicenseType() { Name = "Pro", Application = application };
        }

    }

}
