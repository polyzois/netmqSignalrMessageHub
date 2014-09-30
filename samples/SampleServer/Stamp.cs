using System;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.DemoHub
{
    public class Stamp
    {
        public double Time { get; set; }
        public string Place { get; set; }

        public Stamp()
        {
        }

        public Stamp(string place)
        {
            Place = place;
            Time = DateTime.UtcNow
                           .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                           .TotalMilliseconds;
        }
    }
}