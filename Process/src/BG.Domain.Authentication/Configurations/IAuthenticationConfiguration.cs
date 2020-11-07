using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.Configurations
{
    public interface IAuthenticationConfiguration
    {
        bool DebugMode { set; get; }
    }
}
