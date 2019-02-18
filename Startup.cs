using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Builder;

[assembly: OwinStartup(typeof(Requerimientos.Startup))]

namespace Requerimientos
{
    public class Startup
    {
        public void Configuration(AppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
