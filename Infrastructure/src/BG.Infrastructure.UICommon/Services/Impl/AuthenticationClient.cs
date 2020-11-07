using BG.DAL.AuthModels;
using BG.Infrastructure.Common.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.UICommon.Configurations;
using System.ServiceModel;
using BG.Extensions;

namespace BG.Infrastructure.UICommon.Services.Impl
{
    public interface IAuthService
    {
        LogonResult Logon(LogonInfo info);
    }

    public interface IAuthServiceFactory<TService> where TService : IAuthService, ICommunicationObject
    {
        TService Create();
        void ShowErrorMessage(string error, bool isException = false, Exception exception = null);
    }


    public class AuthenticationClient<TServiceFactory, TService> : IAuthenticationClient 
        where TService: IAuthService, ICommunicationObject
        where TServiceFactory: IAuthServiceFactory<TService>, new()
    {
        readonly string _appName;
        readonly string _serverUrl;
        //readonly Func<LogonInfo, LogonResult> _getResult;
        readonly Guid _key;
        readonly string _version;

        public AuthenticationClient()//Func<LogonInfo, LogonResult> getResult)
        {
            _appName = Configuration.AppName;
            _version = Configuration.AppVersion;
            _serverUrl = Configuration.Url;
            _key = Configuration.LicenseGuid;
            //_getResult = getResult;
        }

        public bool Logon(Guid guid, out string errorMessage)
        {
            Guard.AssertNotEmpty(_appName, "Не определено название приложения");
            Guard.AssertNotEmpty(_version, "Не определена версия");
            Guard.AssertNotEmpty(_serverUrl, "не определен Url");
            Guard.Against<ArgumentException>(_key == Guid.Empty, "Не определен ключ");
            //Guard.AssertNotNull(_getResult, "Не определен прокси сервис");

            var info = new LogonInfo() { LicenseGuid = guid };
            var reg = RegistryHelper.Create();
            info.History = new LogonHistory()
            {
                RegisteredOwner = reg?.RegisteredOwner,
                CurrentBuild = reg?.CurrentBuild,
                CSDVersion = reg?.CSDVersion,
                CSDBuildNumber = reg?.CSDBuildNumber,
                ProductName = reg?.ProductName,
                ProductId = reg?.ProductId,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                UserDomainName = Environment.UserDomainName,
                HardwareId = HardwareHelper.GetId(),
                ServerUrl = _serverUrl,
                ApplicationName = _appName,
                CurrentVersion =_version

            };

            var res = Logon(info);
            errorMessage = res?.Message;
            return res?.Success ?? false;
        }

        LogonResult Logon(LogonInfo logonInfo)
        {
            LogonResult logonResult = null;
            try
            {
                new TServiceFactory().Create().Using(service =>
                {
                    logonResult = service.Logon(logonInfo);
                }, () => new TServiceFactory().Create());
            }
            catch (Exception e) //(FaultException<AuthenticationCrmServiceError> e)
            {
                //    XtraMessageBox.Show("Произошел сбой авторизации пользователя. \r\nПопробуйте повторить вызов тек. операции", "PKI.CRM", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                new TServiceFactory().ShowErrorMessage(null, true, exception:e);
            }

            return logonResult;
        }

    }
}
