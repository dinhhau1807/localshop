using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(localshop.Startup))]
namespace localshop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
