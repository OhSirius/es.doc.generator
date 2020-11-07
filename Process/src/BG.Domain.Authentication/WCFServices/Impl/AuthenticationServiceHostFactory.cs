using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.WCFServices.Impl
{
    public class AuthenticationServiceHostFactory : ServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new CustomServiceHost(serviceType, baseAddresses);
        }
    }
}
