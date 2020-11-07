using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Common.Configuration
{
    public static  class CustomServiceConfiguration
    {
        public static System.Configuration.Configuration GetConfiguration(string configFileName)
        {
            var filemap = new System.Configuration.ExeConfigurationFileMap();
            filemap.ExeConfigFilename = configFileName;

            System.Configuration.Configuration config =
            System.Configuration.ConfigurationManager.OpenMappedExeConfiguration
            (filemap,
             System.Configuration.ConfigurationUserLevel.None);

            return config;
        }

        public static string GetConfigFileName(string moduleFileName)
        {
            //if (string.IsNullOrEmpty(moduleFileName))
            //{
            //    return null;
            //}

            ////string configFilename = System.IO.Path.Combine(PhysicalPath,
            ////                       String.Format(@"bin\Modules\{0}.config", moduleFileName));
            //string configFilename = System.IO.Path.Combine(PhysicalPath,
            //                       String.Format(@"{0}.config", moduleFileName));

            //if (!string.IsNullOrEmpty(moduleFileName) && !System.IO.File.Exists(configFilename))
            //{
            //    throw new FileNotFoundException(configFilename);
            //}

            //return configFilename;
            if (string.IsNullOrEmpty(moduleFileName))
            {
                return null;
            }

            //string configFilename = System.IO.Path.Combine(PhysicalPath,
            //                       String.Format(@"bin\Modules\{0}.config", moduleFileName));
            string configFilename = System.IO.Path.Combine(PhysicalPath,
                                   String.Format(@"{0}.config", moduleFileName));

            if (!string.IsNullOrEmpty(moduleFileName) && !System.IO.File.Exists(configFilename))
            {
                configFilename = configFilename.Replace(@"\Modules", "");
                if (!System.IO.File.Exists(configFilename))
                {
                    configFilename = configFilename.Replace(".dll.config", ".Tests.dll.config");
                    if (!System.IO.File.Exists(configFilename))
                        throw new FileNotFoundException(configFilename);
                }
            }

            return configFilename;
        }

        //https://stackoverflow.com/questions/10376253/windows-service-get-current-directory
        private static string PhysicalPath
        {
            get
            {
                string physicalPath = null;
                // if hosted in IIS
                if (!string.IsNullOrEmpty(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath))
                    physicalPath = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, @"bin\Modules");
                else //if (string.IsNullOrEmpty(physicalPath))
                {
                    // for hosting outside of IIS
                    //physicalPath = System.IO.Directory.GetCurrentDirectory();
                    physicalPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Modules");
                }
                return physicalPath;
            }
        }
    }
}
