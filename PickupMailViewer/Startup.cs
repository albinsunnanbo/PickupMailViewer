using Microsoft.Owin;
using Owin;
using PickupMailViewer;

[assembly: OwinStartupAttribute(typeof(PickupMailViewer.Startup))]
namespace PickupMailViewer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}