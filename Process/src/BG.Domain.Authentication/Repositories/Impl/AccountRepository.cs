using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.LicenseDAL.Models;
using NCommon.Data;
using BG.Infrastructure.Process.NCommon.Data;
using NCommon.Data.EntityFramework;

namespace BG.Domain.Authentication.Repositories.Impl
{
    public class AccountRepository : IAccountRepository
    {
        readonly IUnitOfWorkScopeFactory _scopeFactory;
        readonly IRepository<Account> _repository;

        public AccountRepository(IUnitOfWorkScopeFactory scopeFactory, IRepository<Account> repository)
        {
            _scopeFactory = scopeFactory;
            _repository = repository;
        }

        public Account GetByLicenseId(Guid guid)
        {
            Account account = null;
            using (var scope = _scopeFactory.Create())
            {
                _repository.FetchMany(a => a.Licenses).ThenFetch(l => l.Application);
                _repository.FetchMany(a => a.Licenses).ThenFetch(l => l.Type);
                account = _repository.FirstOrDefault(a => a.Licenses.Any(l => l.Guid == guid));
                scope.Commit();
            }

            return account;
        }
    }
}
