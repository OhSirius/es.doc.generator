using BG.DAL.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.WCFServices
{


    [ServiceContract]
    interface IAuthenticationService
    {
        [OperationContract]
        LogonResult Logon(LogonInfo info);
    }
}