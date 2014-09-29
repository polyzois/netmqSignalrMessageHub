using System;
using Microsoft.Owin.Hosting;
using System.Threading;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using NetMQ;
using NetMQ.Sockets;

namespace SampleServer
{

    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3) {
                Console.WriteLine ("usage: http://localhost:8080 tcp://127.0.0.1:5002 tcp://127.0.0.1:5003");
                return;
            }


            string serverUrl = "http://localhost:8080/";

            if (args.Length > 0) {
                serverUrl = args [0];
            }

            NetMQContext contex = NetMQContext.Create ();
            var pub = contex.CreatePublisherSocket ();

           
            var publishUrl = args [1];
            Console.WriteLine ("Will publish messages at " + publishUrl);
            pub.Bind (publishUrl);

            var sub = contex.CreateSubscriberSocket ();
            var subscriberUrl = args [2];
            Console.WriteLine ("Will recieve messages at "+subscriberUrl);
            sub.Connect (subscriberUrl);
            sub.Subscribe ("");

            var myBus = new netmqSignalrMessageHub.NetMqMessageBus (GlobalHost.DependencyResolver,new ScaleoutConfiguration(),pub,sub);


            GlobalHost.DependencyResolver.Register(typeof(IMessageBus),()=> myBus );

            using (WebApp.Start<Startup>(serverUrl))
            {
                Console.WriteLine("Server running at "+serverUrl);
                Console.ReadLine();
            }
        }
    }

}
