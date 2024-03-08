using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Muse3.Startup))]
namespace Muse3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
