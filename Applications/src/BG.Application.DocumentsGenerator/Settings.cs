using BG.Infrastructure.Common.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Application.DocumentsGenerator
{
    public static class Settings
    {

        static string _url;
        public static string Url
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_url))
                    _url = CryptographyHelper.DecryptStringAES("EAAAABqnzPk05XyV75PIJijCRxynA1VB0W0UNuZY6o8Y6uzLdiZ51F9Ir4HEVMjl/11Hgw==", "scstbbf");

                return _url;
            }
        }
    }
}
