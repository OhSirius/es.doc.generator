using BG.Infrastructure.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.WCFServices.Impl
{
    class CustomServiceHost : BaseServiceHost
    {
        public CustomServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        protected override string ModuleFileName
        {
            get { return CustomServiceSettings.ModuleFileName; }
        }
    }
}
