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

namespace BG.Domain.Authentication.Tests
{
    [Category("AuthenticationTest")]
    public class AccountRepositoryTests : BaseTest
    {
        [Test]
        public void Find_account_by_license_guid()
        {
            var factory = _container.Resolve<IUnitOfWorkScopeFactory>();
            var rep = _container.Resolve<IRepository<Account>>();
            var accountRep = _container.Resolve<IRepository<Account>>();
            var licenseRep = _container.Resolve<IRepository<License>>();

            Account exAccount = null;
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

                accountRep.Add(account);licenseRep.Add(license1); licenseRep.Add(license2); licenseRep.Add(license3);
                accountRep.Add(account2);licenseRep.Add(license2_1); licenseRep.Add(license2_2); licenseRep.Add(license2_3);
                scope.Flush();

                var repository = new AccountRepository(factory, rep);
                exAccount = repository.GetByLicenseId(license1.Guid);
            }

            Assert.IsTrue(exAccount !=null, "Пользователь не найден");
            Assert.IsTrue(!exAccount.Licenses.IsNullOrEmpty(), "У пользователя отсутствуют лицензии");
            Assert.IsTrue(exAccount.Licenses.All(l=>l.Application.Id == 1 && new[] { 1, 2, 3 }.Contains(l.Type.Id)), "У пользователя неверные типы лицензий");
        }


    }
}
