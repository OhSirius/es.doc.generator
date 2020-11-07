using BG.Application.DocumentsGenerator.ServiceReference;
using BG.Application.DocumentsGenerator.Services;
using BG.Infrastructure.UICommon.Services.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Configuration = BG.Infrastructure.UICommon.Configurations.Configuration;

namespace BG.Application.DocumentsGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// https://www.c-sharpcorner.com/UploadFile/07c1e7/create-splash-screen-in-wpf/
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ////Введение лицензионного ключа
            //if(Configuration.LicenseGuid == Guid.Empty)
            //{
            //    var key = new LicenseKey();
            //    if(key.ShowDialog() != true)
            //    {
            //        Current.Shutdown();
            //    }
            //}

            ////Аутентификация клиента на сервере
            //string error = null;
            //var service = new AuthenticationClient<AuthenticationServiceClientFactory, AuthenticationServiceClient>();
            //if(!service.Logon(Configuration.LicenseGuid, out error))
            //{
            //    MessageBox.Show(error, "Ошибка");
            //    Current.Shutdown();
            //}

            base.OnStartup(e);
        }


    }
}
