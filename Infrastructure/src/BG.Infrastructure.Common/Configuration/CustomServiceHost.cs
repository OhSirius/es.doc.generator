using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Common.Configuration
{
    public abstract class BaseServiceHost : System.ServiceModel.ServiceHost
    {
        //public BaseServiceHost()
        //{
        //}

        public BaseServiceHost(Type serviceType, params Uri[] baseAddresses) 
            : base(serviceType, baseAddresses)
        {
        }

        //public BaseServiceHost(object singeltonInstance, params Uri[] baseAddresses)
        //    : base(singeltonInstance, baseAddresses)
        //{
        //}

        protected abstract string ModuleFileName { get; }

        protected override void ApplyConfiguration()
        {

            // generate the name of the custom configFile, from the service name:
            string configFilename = CustomServiceConfiguration.GetConfigFileName(this.ModuleFileName);

            if (string.IsNullOrEmpty(configFilename) || !System.IO.File.Exists(configFilename))
                base.ApplyConfiguration();
            else
                LoadConfigFromCustomLocation(configFilename);

            // also make the generated WSDL "flat"": 
            InjectFlatWsdlExtension();
        }

        // workaround for passing file/string to the override ApplyConfiguration
        //CallContext.SetData("_config", config.Config);

        private void LoadConfigFromCustomLocation(string configFilename)
        {
            System.Configuration.Configuration config =
                CustomServiceConfiguration.GetConfiguration(configFilename);

            var serviceModel = System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(config);

            bool loaded = false;
            foreach (System.ServiceModel.Configuration.ServiceElement se in serviceModel.Services.Services)
            {
                if (!loaded)
                    if (se.Name == this.Description.ConfigurationName)
                    {
                        base.LoadConfigurationSection(se);
                        loaded = true;
                    }
            }
            if (!loaded)
                throw new ArgumentException("ServiceElement doesn't exist");
        }


        private void InjectFlatWsdlExtension()
        {
            foreach (ServiceEndpoint endpoint in this.Description.Endpoints)
            {
                endpoint.Behaviors.Add(new FlatWsdl());
            }
        }
    }
}
