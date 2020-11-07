using BG.DAL.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.UICommon.Services
{
    public interface IAuthenticationClient
    {
        bool Logon(Guid guid, out string errorMessage);
    }
}
