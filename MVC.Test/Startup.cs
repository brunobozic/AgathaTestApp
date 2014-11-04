using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC.Test.Startup))]
namespace MVC.Test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
