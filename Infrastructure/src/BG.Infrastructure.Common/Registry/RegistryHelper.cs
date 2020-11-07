using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BG.Infrastructure.Common.Registry
{
    public class RegistryInfo
    {
        public string RegisteredOrganization { set; get; }

        public string ProductName { set; get; }

        public string ProductId { set; get; }

        public string CSDBuildNumber { set; get; }

        public string CSDVersion { set; get; }

        public string CurrentBuild { set; get; }

        public string RegisteredOwner { set; get; }
    }


    public static class RegistryHelper
    {
        public static RegistryInfo Create()
        {
            RegistryInfo info = null;

            using (RegistryKey rey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
            {
                if (rey != null)
                {
                    info = new RegistryInfo()
                    {
                        RegisteredOrganization = rey.GetValue("RegisteredOrganization") as string,
                        ProductName = rey.GetValue("ProductName") as string,
                        ProductId = rey.GetValue("ProductId") as string,
                        RegisteredOwner = rey.GetValue("RegisteredOwner") as string,
                        CSDBuildNumber = rey.GetValue("CSDBuildNumber") as string,
                        CSDVersion = rey.GetValue("CSDVersion") as string,
                        CurrentBuild = rey.GetValue("CurrentBuild") as string
                    };
                }
            }

            return info;
        }
    }
}
