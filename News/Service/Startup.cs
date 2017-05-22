using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Service.Startup))]
namespace Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // biztosítanunk kell a süti alapú azonosítás lehetőségét
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login")
            });
        }
    }
}