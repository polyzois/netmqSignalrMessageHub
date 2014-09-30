using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Samples.Hubs.DemoHub;
using Newtonsoft.Json;

namespace Microsoft.AspNet.SignalR.Client40.Samples
{
    public class Client
    {
        private TextWriter _traceWriter;
        private HubConnection hubConnection;
        private IHubProxy hubProxy;

        public Client(TextWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public void Run(string url)
        {
            try
            {
                RunHubConnectionAPI(url);
            }
            catch (Exception exception)
            {
                _traceWriter.WriteLine("Exception: {0}", exception);
                throw;
            }
        }       

        private void RunHubConnectionAPI(string url)
        {
            hubConnection = new HubConnection(url);
            hubConnection.TraceWriter = _traceWriter;
            hubConnection.TraceLevel = TraceLevels.StateChanges;

            hubProxy = hubConnection.CreateHubProxy("ClientSampleHub");
            hubProxy.On<string>("displayMessage", (data) => hubConnection.TraceWriter.WriteLine(data));
            
            hubConnection.Start().Wait();
            hubConnection.TraceWriter.WriteLine("transport.Name={0}", hubConnection.Transport.Name);

            hubProxy.Invoke("DisplayMessageCaller", "Hello Caller!").Wait();

            string joinGroupResponse = hubProxy.Invoke<string>("JoinGroup", hubConnection.ConnectionId, "CommonClientGroup").Result;
            hubConnection.TraceWriter.WriteLine("joinGroupResponse={0}", joinGroupResponse);
            
            hubProxy.Invoke("DisplayMessageGroup", "CommonClientGroup", "Hello Group Members!").Wait();

            string leaveGroupResponse = hubProxy.Invoke<string>("LeaveGroup", hubConnection.ConnectionId, "CommonClientGroup").Result;
            hubConnection.TraceWriter.WriteLine("leaveGroupResponse={0}", leaveGroupResponse);

            hubProxy.Invoke("DisplayMessageGroup", "CommonClientGroup", "Hello Group Members! (caller should not see this message)").Wait();

            hubProxy.Invoke("DisplayMessageCaller", "Hello Caller again!").Wait();

           joinGroupResponse = hubProxy.Invoke<string>("JoinGroup", hubConnection.ConnectionId, "CommonClientGroup").Result;
            hubConnection.TraceWriter.WriteLine("joinGroupResponse={0}", joinGroupResponse);
             hubConnection.TraceWriter.WriteLine();


             hubProxy.On<TraceMessage>("displayTraceMessage", (data) =>
                 { 
                     data.AddPlace("dest");
                     hubConnection.TraceWriter.WriteLine(JsonConvert.SerializeObject(data)); 
                 });
        }


        public void SendTrace(string strmsg)
        {
            var msg = new TraceMessage();
            msg.AddPlace("Origin");
            hubProxy.Invoke("TraceMessageGroup", "CommonClientGroup", msg).Wait();
          //  hubProxy.Invoke("DisplayMessageGroup","CommonClientGroup", msg).Wait();
        }
    }    
}

