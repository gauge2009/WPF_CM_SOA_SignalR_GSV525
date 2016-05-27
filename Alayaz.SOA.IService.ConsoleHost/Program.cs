 using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
 
 using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace Alayaz.SOA.IService.ConsoleHost
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

                StartSgnlR();

                Console.ReadLine();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Program), string.Format("Message:{0};StackTrace{1}", ex, ex.StackTrace));
                if (ex.InnerException != null)
                {
                    var e = ex.InnerException;
                    LogHelper.WriteLog(typeof(Program), string.Format("Message:{0};StackTrace{1}", e.Message, e.StackTrace));
                }
                if (ex.InnerException.InnerException != null)
                {
                    var e = ex.InnerException.InnerException;
                    LogHelper.WriteLog(typeof(Program), string.Format("Message:{0};StackTrace{1}", e.Message, e.StackTrace));
                }

            }

            #region SignalR

            StartSgnlR();

            #endregion
        }

        private static void StartSgnlR()
        {
            using (WebApp.Start<Startup>("http://localhost:11111"))
            {
                Console.WriteLine("Server running at http://localhost:11111");
                Console.ReadLine();
            }
        }
    }

}
