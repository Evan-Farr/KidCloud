using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KidCloudProject.Startup))]
namespace KidCloudProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
