using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BG.Application.Portal.Startup))]
namespace BG.Application.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
