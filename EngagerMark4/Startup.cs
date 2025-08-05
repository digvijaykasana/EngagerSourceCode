using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EngagerMark4.Startup))]
namespace EngagerMark4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
