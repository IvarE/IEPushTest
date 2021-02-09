using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeploymentVerificationTool.Startup))]
namespace DeploymentVerificationTool
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
