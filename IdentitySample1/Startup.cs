using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentitySample1.Startup))]
namespace IdentitySample1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
