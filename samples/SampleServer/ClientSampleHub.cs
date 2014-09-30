using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.DemoHub
{
   
    public class ClientSampleHub : Hub
    {

        public void DisplayMessageCaller(string message){
            Clients.Caller.displayMessage (message);
        }

        public Task JoinGroup(string connectionId, string name){
        
            return Groups.Add(Context.ConnectionId, name);
          
        }
        public Task LeaveGroup(string connectionId, string name){

            return Groups.Remove(Context.ConnectionId, name);

        }


        public void DisplayMessageGroup( string groupName, string message){

            Clients.Group (groupName).displayMessage (message);
        
        }

        public void TraceMessageGroup(string groupName, TraceMessage message)
        {
            Console.WriteLine("TraceMessageGroup");
            message.AddPlace("Server Hub");
            Console.WriteLine(JsonConvert.SerializeObject(message));
            Clients.Group(groupName).displayTraceMessage(message);

        }

    }
}