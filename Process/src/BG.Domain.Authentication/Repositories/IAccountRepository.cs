using BG.LicenseDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.Repositories
{
    public interface IAccountRepository
    {
        Account GetByLicenseId(Guid guid);
    }
}
