using BG.Infrastructure.Common.Cryptography;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.UICommon.Configurations
{
    public static class Configuration
    {
        static LicenseConfiguration _conf = null;

        static Configuration()
        {
            _conf = new LicenseConfiguration();
            _conf.Initialize();
        }

        static string _url;
        /// <summary>
        /// Зашифрованный адрес сервера аутентификации
        /// </summary>
        public static string Url
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_url))
                    _url = CryptographyHelper.DecryptStringAES("EAAAABqnzPk05XyV75PIJijCRxynA1VB0W0UNuZY6o8Y6uzLdiZ51F9Ir4HEVMjl/11Hgw==", "scstbbf");

                return _url;
            }
        }

        static string _appName;

        /// <summary>
        /// Название приложения
        /// </summary>
        public static string AppName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_appName))
                    _appName = ConfigurationManager.AppSettings["AppName"];

                return _appName;
            }
        }


        static string _appVersion;
        /// <summary>
        /// Версия приложения
        /// </summary>
        public static string AppVersion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_appVersion))
                    _appVersion = ConfigurationManager.AppSettings["AppVersion"];

                return _appVersion;
            }
        }

        /// <summary>
        /// Гуид лицензии
        /// </summary>
        public static Guid LicenseGuid
        {
            get
            {
                return _conf.Guid;
            }
        }

        public static void SetLicenseGuid(Guid guid)
        {
            _conf.Guid = guid;
            _conf.Write();
        }

    }
}
