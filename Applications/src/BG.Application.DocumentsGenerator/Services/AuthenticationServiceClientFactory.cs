using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Application.DocumentsGenerator.ServiceReference;
using System.ServiceModel;
using BG.Extensions;
using BG.DAL.AuthModels;
using BG.Infrastructure.UICommon.Configurations;
using BG.Infrastructure.UICommon.Services.Impl;
using System.Windows;
using WpfExceptionViewer;

namespace BG.Application.DocumentsGenerator.Services
{
    public partial class AuthenticationServiceClientFactory: IAuthServiceFactory<AuthenticationServiceClient>
    {
        public AuthenticationServiceClient Create()
        {
            AuthenticationServiceClient proxy = null;
            try
            {
                var bindingConf = "WsPlain";
#if (Basis || Pro || Lite)
                string serviceUrl = string.Format("{0}/BGAuth", Configuration.Url);
                var endpointAddress = new EndpointAddress(serviceUrl);
                proxy = new AuthenticationServiceClient(bindingConf, endpointAddress);
#endif
#if (Debug)
                proxy = new AuthenticationServiceClient(bindingConf);
#endif
                //((ICommunicationObject)CrmDataManager).Faulted += ProxyServiceFactory_Faulted;

            }
            catch (Exception ex)
            {
                //Aetp.Windows.Forms.ExceptionsHandler.Exception(MainForm.Console, ex);
                throw ex;
            }

            return proxy;
        }

        public void ShowErrorMessage(string error, bool isException = false, Exception exception = null)
        {
            if (isException)
            {
                var ev = new ExceptionViewer("Произошло исключение.", exception);
                ev.ShowDialog();
            }
            else
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
