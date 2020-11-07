using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.Practices.Unity;


namespace BG.Infrastructure.Common.MEF
{
    public interface IModuleInstaller
    {
        void Setup();

        IEnumerable<RouteBase> GetRoutes();

        IEnumerable<Type> GetKnownTypes();

        //IEnumerable<ScriptsAndHtmlInfo> GetScriptsAndHtml();

    }

    //public interface IODataModuleInstaller
    //{
    //    IEnumerable<Action<ODataConventionModelBuilder>> GetODataConventionModelConfigureActions();

    //}

    public interface IUnityConfigurationModuleInstaller
    {
        IEnumerable<Func<IUnityContainer, IUnityContainer>> GetContainerConfigurationFuncs();

    }

}
