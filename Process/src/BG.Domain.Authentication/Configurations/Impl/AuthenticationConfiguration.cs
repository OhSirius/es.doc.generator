using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.Configurations.Impl
{
    public class AuthenticationConfiguration : IAuthenticationConfiguration
    {
        public bool DebugMode { set; get; }
    }
}
