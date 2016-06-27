using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Alayaz.SOA.IClientService.ConsoleHost
{
    public class PingHub : Hub
    {
        public Task BroadcastLog(string msg)
        {
            
            return Clients.Others.Update(msg);
        }
        public Task BroadcastClear()
        {
             return Clients.Others.SayGoobye();
        }


        public override Task OnConnected()
        {
            Console.WriteLine("[Hub] Client connected");
            return base.OnConnected();
        }

        public Task Ping()
        {
            Console.WriteLine("[Hub] Ping received");
            return Clients.Caller.Update(
                "Ping received at " + DateTime.Now.ToLongTimeString());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("[Hub] Client disconnected");
            return base.OnDisconnected(  stopCalled);
        }

     }
}