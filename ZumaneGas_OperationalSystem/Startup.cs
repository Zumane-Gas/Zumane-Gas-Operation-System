using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZumaneGas_OperationalSystem.Startup))]
namespace ZumaneGas_OperationalSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
