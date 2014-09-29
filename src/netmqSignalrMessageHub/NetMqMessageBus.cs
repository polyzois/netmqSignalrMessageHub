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

    /// <summary>
    /// Replicate all activity in current SignalR Bus using NetMq.
    /// Purpose is to scale out using NetMq in a brokerless fashion. 
    /// Using one subscriver socket and one publisher socket.
    /// A single socket in NetMq can subscribe to many endpoints so this setup should suffice.
    /// 
    /// 
    /// </summary>
    public class NetMqMessageBus : ScaleoutMessageBus
    {

        long pid = 1;

        PublisherSocket pubSock;

        SubscriberSocket subScoket;


        public NetMqMessageBus(IDependencyResolver resolver, ScaleoutConfiguration configuration,PublisherSocket pubSock,SubscriberSocket sub):base(resolver,configuration){

            this.pubSock = pubSock;
            this.subScoket = sub;

            InitializeSubscriber ();
            Open (0);
        }

        /// <summary>
        /// Send to netmq and also send out to local SignalR hub. 
        /// The NetMq setup is fire and forget and we will not reveive anything back which is the reason for sending it locally as well.
        /// </summary>
        /// <param name="streamIndex">Stream index.</param>
        /// <param name="messages">Messages.</param>
        protected override Task Send(int streamIndex, IList<Message> messages)
        {


            Console.WriteLine ("xxx Send xxx");

            var y = (ulong) Interlocked.Increment(ref pid) ;

            var scaleoutMessage = new ScaleoutMessage (messages);

            pubSock.Send(scaleoutMessage.ToBytes());

            base.OnReceived (streamIndex,y, scaleoutMessage);

            return Task.FromResult (Type.Missing);



        }


        /// <summary>
        /// Fire of a background thread to use for the incoming messages.
        /// TODO is OnRecieved thread safe? We can have traffic from local host as well as from this subscriber socket.
        /// </summary>
        void InitializeSubscriber ()
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

