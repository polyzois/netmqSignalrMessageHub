using System.Collections.Generic;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.DemoHub
{
    public class TraceMessage
    {

        List<Stamp> places = new List<Stamp>();
 
        public void AddPlace(string name)
        {
            places.Add(new Stamp(name));
        }

        public List<Stamp> Places
        {
            get { return places; }
            set { places = value; }
        }
    }
}