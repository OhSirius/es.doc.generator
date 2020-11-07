using BG.Infrastructure.Process.Process;
using BG.LicenseDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication
{
    public interface IAuthenticationProcess : IBusinessProcess
    {
        bool IsValid { get; }
        string ErrorMessage { get; }

        void Logon(Guid? licenseGuid, LogonHistory history);
    }
}
