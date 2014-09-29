using System.Diagnostics;
using System.Web.Cors;
using Microsoft.AspNet.SignalR;

using Microsoft.AspNet.SignalR.Tracing;
using Microsoft.Owin.Cors;
using Owin;
using SampleServer.Connections;

namespace SampleServer
{
    /// <summary>
    /// Copied from Microsoft.AspNet.SelfHost.Samples
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Properties.Add( "host.AppMode" ,"development");
            app.UseErrorPage(); 

            app.Map("/raw-connection", map =>
            {
                // Turns cors support on allowing everything
                // In real applications, the origins should be locked down
                map.UseCors(CorsOptions.AllowAll)
                   .RunSignalR<RawConnection>();
            });

            app.Map("/signalr", map =>
            {
                var config = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting this line
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };

                // Turns cors support on allowing everything
                // In real applications, the origins should be locked down
                map.UseCors(CorsOptions.AllowAll)
                   .RunSignalR(config);
            });

            // Turn tracing on programmatically
            GlobalHost.TraceManager.Switch.Level = SourceLevels.Information;
        }
    }
}
