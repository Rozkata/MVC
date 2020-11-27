using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GuestBook2.Startup))]
namespace GuestBook2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
