using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Microsoft.AspNet.SignalR.Client40.Samples
{
    class Program
    {
        static void Main(string[] args)
        {

            string serverUrl = "http://localhost:8080/";

            if (args.Length > 0) {
                serverUrl = args [0];
            }

            var writer = Console.Out;
            var client = new Client(writer);

           

            client.Run(serverUrl);

            while (true)
            {
              var key=  Console.ReadKey();
               
                client.SendTrace(""+key.KeyChar);
            }
            
        }
    }
}
