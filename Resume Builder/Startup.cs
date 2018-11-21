using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Resume_Builder.Startup))]
namespace Resume_Builder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
