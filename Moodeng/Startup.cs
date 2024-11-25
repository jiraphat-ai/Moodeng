using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Moodeng.Startup))]
namespace Moodeng
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
