using BG.Infrastructure.Process.NCommon.Data;
using NCommon.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using BG.Domain.Authentication.Repositories.Impl;
using BG.LicenseDAL.Models;
using BG.Infrastructure.Process.NCommon.Extensions;
using BG.Extensions;
using Configuration = BG.Domain.Authentication.Bootstrap.Configuration;

namespace BG.Domain.Authentication.Tests
{
    [Category("AuthenticationTest")]
    public class AuthenticationProcessTests:BaseTest
    {
        [Test]
        public void Success_logon_for_guid()
        {
            var factory = _container.Resolve<IUnitOfWorkScopeFactory>();
            var rep = _container.Resolve<IRepository<Account>>();
            var accountRep = _container.Resolve<IRepository<Account>>();
            var licenseRep = _container.Resolve<IRepository<License>>();
            var logonRep = _container.Resolve<IRepository<LogonHistory>>();

            IAuthenticationProcess process = null;
            LogonHistory logon = null;
            using (var scope = factory.Create())
            {
                //Создаем УЗ
                var account = new Account() { CreateDate = DateTime.Now, Email = "test1@bg.ru", Guid = Guid.NewGuid() };
                var license1 = new License() { TypeId = 1, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license2 = new License() { TypeId = 2, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license3 = new License() { TypeId = 3, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };

                var account2 = new Account() { CreateDate = DateTime.Now, Email = "test2@bg.ru", Guid = Guid.NewGuid() };
                var license2_1 = new License() { TypeId = 1, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license2_2 = new License() { TypeId = 2, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license2_3 = new License() { TypeId = 3, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };

                accountRep.Add(account); licenseRep.Add(license1); licenseRep.Add(license2); licenseRep.Add(license3);
                accountRep.Add(account2); licenseRep.Add(license2_1); licenseRep.Add(license2_2); licenseRep.Add(license2_3);
                scope.Flush();

                //Проверяем доступ
                process = BG.Domain.Authentication.Bootstrap.Configuration.Configure(_container, true);
                process.Logon(license1.Guid, new LogonHistory() { CSDBuildNumber = "123" });
                scope.Flush();

                logon = logonRep.FirstOrDefault(l => l.AccountId == account.Id);
            }

            Assert.IsTrue(process != null, "Не удалось создать процесс");
            Assert.IsTrue(process.IsValid, process.ErrorMessage ?? "Ошибка");
            Assert.IsTrue(logon != null, "Не создался логон");
        }


        [Test]
        public void Fail_logon_without_license_guid()
        {
            var factory = _container.Resolve<IUnitOfWorkScopeFactory>();
            var rep = _container.Resolve<IRepository<Account>>();
            var accountRep = _container.Resolve<IRepository<Account>>();
            var licenseRep = _container.Resolve<IRepository<License>>();
            var logonRep = _container.Resolve<IRepository<LogonHistory>>();

            IAuthenticationProcess process = null;
            LogonHistory logon = null;
            using (var scope = factory.Create())
            {
                //Создаем УЗ
                var account = new Account() { CreateDate = DateTime.Now, Email = "test1@bg.ru", Guid = Guid.NewGuid() };
                var license1 = new License() { TypeId = 1, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license2 = new License() { TypeId = 2, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license3 = new License() { TypeId = 3, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };

                accountRep.Add(account); licenseRep.Add(license1); licenseRep.Add(license2); licenseRep.Add(license3);
                scope.Flush();

                //Проверяем доступ
                process = BG.Domain.Authentication.Bootstrap.Configuration.Configure(_container, true);
                process.Logon(null, new LogonHistory() { CSDBuildNumber = "123" });
                scope.Flush();

                logon = logonRep.FirstOrDefault();
            }

            Assert.IsTrue(process != null, "Не удалось создать процесс");
            Assert.IsTrue(!process.IsValid, process.ErrorMessage ?? "Ошибка");
            Assert.IsTrue(logon != null, "Не создался логон");
            Assert.IsTrue(!logon.AccountId.HasValue || logon.AccountId.Value <= 0, "УЗ должна отсутствовать");
        }


        [Test]
        public void Fail_logon_without_account()
        {
            var factory = _container.Resolve<IUnitOfWorkScopeFactory>();
            var rep = _container.Resolve<IRepository<Account>>();
            var accountRep = _container.Resolve<IRepository<Account>>();
            var licenseRep = _container.Resolve<IRepository<License>>();
            var logonRep = _container.Resolve<IRepository<LogonHistory>>();

            IAuthenticationProcess process = null;
            LogonHistory logon = null;
            using (var scope = factory.Create())
            {
                //Создаем УЗ
                var account = new Account() { CreateDate = DateTime.Now, Email = "test1@bg.ru", Guid = Guid.NewGuid() };
                var license1 = new License() { TypeId = 1, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license2 = new License() { TypeId = 2, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license3 = new License() { TypeId = 3, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };

                accountRep.Add(account); licenseRep.Add(license1); licenseRep.Add(license2); licenseRep.Add(license3);
                scope.Flush();

                //Проверяем доступ
                process = BG.Domain.Authentication.Bootstrap.Configuration.Configure(_container, true);
                process.Logon(Guid.NewGuid(), new LogonHistory() { CSDBuildNumber = "123" });
                scope.Flush();

                logon = logonRep.FirstOrDefault();
            }

            Assert.IsTrue(process != null, "Не удалось создать процесс");
            Assert.IsTrue(!process.IsValid, process.ErrorMessage ?? "Ошибка");
            Assert.IsTrue(logon != null, "Не создался логон");
            Assert.IsTrue(!logon.AccountId.HasValue || logon.AccountId.Value <= 0, "УЗ должна отсутствовать");
        }


        [Test]
        public void Fail_logon_without_success()
        {
            var factory = _container.Resolve<IUnitOfWorkScopeFactory>();
            var rep = _container.Resolve<IRepository<Account>>();
            var accountRep = _container.Resolve<IRepository<Account>>();
            var licenseRep = _container.Resolve<IRepository<License>>();
            var logonRep = _container.Resolve<IRepository<LogonHistory>>();

            IAuthenticationProcess process = null;
            LogonHistory logon = null;
            using (var scope = factory.Create())
            {
                //Создаем УЗ
                var account = new Account() { CreateDate = DateTime.Now, Email = "test1@bg.ru", Guid = Guid.NewGuid() };
                var license1 = new License() { TypeId = 1, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license2 = new License() { TypeId = 2, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = true, Count = 1, ApplicationId = 1, Account = account };
                var license3 = new License() { TypeId = 3, Guid = Guid.NewGuid(), CreateDate = DateTime.Now, ChangeDate = DateTime.Now, Access = false, Count = 1, ApplicationId = 1, Account = account };

                accountRep.Add(account); licenseRep.Add(license1); licenseRep.Add(license2); licenseRep.Add(license3);
                scope.Flush();

                //Проверяем доступ
                process = BG.Domain.Authentication.Bootstrap.Configuration.Configure(_container, true);
                process.Logon(license3.Guid, new LogonHistory() { CSDBuildNumber = "123" });
                scope.Flush();

                logon = logonRep.FirstOrDefault(l => l.AccountId == account.Id);
            }

            Assert.IsTrue(process != null, "Не удалось создать процесс");
            Assert.IsTrue(!process.IsValid, process.ErrorMessage ?? "Ошибка");
            Assert.IsTrue(logon != null, "Не создался логон");
            Assert.IsTrue(logon.AccountId.HasValue && logon.AccountId.Value > 0, "УЗ должна отсутствовать");
        }


    }
}
