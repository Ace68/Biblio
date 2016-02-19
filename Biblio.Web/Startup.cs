using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Biblio.Web.Startup))]
namespace Biblio.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
