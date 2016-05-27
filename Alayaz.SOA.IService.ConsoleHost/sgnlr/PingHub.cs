using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Alayaz.SOA.IService.ConsoleHost
{
    public class PingHub : Hub
    {
        public override Task OnConnected()
        {
            Console.WriteLine("[Hub] Client connected");
            return base.OnConnected();
        }

        public Task Ping()
        {
            Console.WriteLine("[Hub] Ping received");
            return Clients.Caller.Message(
                "Ping received at " + DateTime.Now.ToLongTimeString());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("[Hub] Client disconnected");
            return base.OnDisconnected(  stopCalled);
        }
    }
}