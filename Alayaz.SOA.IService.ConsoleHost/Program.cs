 using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
 
 using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace Alayaz.SOA.IClientService.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceHost host = new ServiceHost(typeof(FileReaderService));

                host.Open();

                Console.WriteLine("Service Host Satrt...!");

                #region SignalR  Conn

                StartSgnlR();

                #endregion
              

             }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Program), string.Format("Message:{0};StackTrace{1}", ex, ex.StackTrace));
                if (ex.InnerException != null)
                {
                    var e = ex.InnerException;
                    LogHelper.WriteLog(typeof(Program), string.Format("Message:{0};StackTrace{1}", e.Message, e.StackTrace));
                }
                if (ex.InnerException != null&&ex.InnerException.InnerException != null)
                {
                    var e = ex.InnerException.InnerException;
                    LogHelper.WriteLog(typeof(Program), string.Format("Message:{0};StackTrace{1}", e.Message, e.StackTrace));
                }

            }

        
        }

        private static void StartSgnlR()
        {
            string url = "http://localhost:11111";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Server running at {0}", url);

                var input = Console.ReadLine();
                while (input == "hi")
                {
                    #region SignalR  BroadcastLog


                    // var hubConn = new PingHub();
                    // hubConn.BroadcastLog(  "Ping received at " + DateTime.Now.ToLongTimeString());
                    var context = GlobalHost.ConnectionManager.GetHubContext<PingHub>();
                    context.Clients.All.Update(  "Update received at " + DateTime.Now.ToLongTimeString());

                    //context. Clients.Others.Update(msg);


                    #endregion
                    input = Console.ReadLine();
                }

            }
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            //string url = "http://localhost:11111";
            //using (WebApp.Start(url))
            //{
            //    Console.WriteLine("Server running on {0}", url);
            //    Console.ReadLine();
            //}
        }
    }

}
