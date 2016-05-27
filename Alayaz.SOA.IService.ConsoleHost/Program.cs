using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

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
        }
    }
}
