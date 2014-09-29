using System;
using Microsoft.AspNet.SignalR.Messaging;
using NetMQ.Sockets;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using NetMQ;

namespace netmqSignalrMessageHub
{
    public class NetMqMessageBus : ScaleoutMessageBus
    {

        long pid = 1;

        PublisherSocket pubSock;

        SubscriberSocket subScoket;

        public NetMqMessageBus(IDependencyResolver resolver, ScaleoutConfiguration configuration,PublisherSocket pubSock,SubscriberSocket sub):base(resolver,configuration){

            this.pubSock = pubSock;
            this.subScoket = sub;

            Initialize ();
            Open (0);
        }

        protected override Task Send(int streamIndex, IList<Message> messages)
        {


            Console.WriteLine ("xxx Send xxx");

            var y = (ulong) Interlocked.Increment(ref pid) ;

            var scaleoutMessage = new ScaleoutMessage (messages);

           // OutgoingSocketExtensions.
            pubSock.Send(scaleoutMessage.ToBytes());

            base.OnReceived (streamIndex,y, scaleoutMessage);

            return Task.FromResult (Type.Missing);



        }



        void Initialize ()
        {
            Task.Factory.StartNew (() => {
                try{
                    while(true){
                        Console.WriteLine("Entering recieve");
                        var data = subScoket.Receive();
                        Console.WriteLine("Recieved msg");
                        var scaleoutMessage= ScaleoutMessage.FromBytes(data);
                        var y = (ulong) Interlocked.Increment(ref pid) ;
                        base.OnReceived (0,y, scaleoutMessage);

                    }
                }catch(Exception e){
                    Console.WriteLine("Caught excpetion in recieve "+e);
                }
            });
        }
    }
}

