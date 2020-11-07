using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Westwind.Utilities.Configuration;

namespace BG.Infrastructure.UICommon.Configurations
{
    public class LicenseConfiguration : AppConfiguration//, ICompanyMoratoriumScheduledConfiguration
    {
        public Guid Guid { set; get; }

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            var provider = new ConfigurationFileConfigurationProvider<LicenseConfiguration>();
            return provider;
        }
    }
}
