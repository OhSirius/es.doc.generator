using BG.DAL.AuthModels;
using BG.Domain.Authentication.Bootstrap;
using BG.LicenseDAL.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Authentication.WCFServices.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        Logger _logger = null;

        public AuthenticationService()
        {
            _logger = Configuration.GetLogger();
        }

        public LogonResult Logon(LogonInfo info)
        {
            LogonResult result = new LogonResult() { Success = false };

            try
            {
                var process = Configuration.Configure();
                process.Logon(info?.LicenseGuid, new LicenseDAL.Models.LogonHistory()
                {
                    MachineName = info?.History?.MachineName,
                    Login = info?.History?.UserName,
                    UserDomainName = info?.History?.UserDomainName,
                    HardwareId = info?.History?.HardwareId,
                    ApplicationVersion = info?.History?.CurrentVersion,
                    ApplicationName = info?.History?.ApplicationName,
                    ProductName = info?.History?.ProductName,
                    ProductId = info?.History?.ProductId,
                    CSDBuildNumber = info?.History?.CSDBuildNumber,
                    CSDVersion = info?.History?.CSDVersion,
                    CurrentBuild = info?.History?.CurrentBuild,
                    RegisteredOwner = info?.History?.RegisteredOwner,
                    ServerUrl = info?.History?.ServerUrl
                });

                return new LogonResult() { Success = process.IsValid, Message = process.ErrorMessage };
            }
            catch(Exception e)
            {
                _logger.Error(e.ToString());
                result.Message = "Произошла ошибка на сервере. Попробуйте позже";
            }

            return result;
        }


    }
}
