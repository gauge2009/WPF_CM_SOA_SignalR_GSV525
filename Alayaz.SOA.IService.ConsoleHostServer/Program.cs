using Alayaz.SOA.Service;
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
            ServiceHost host = new ServiceHost(typeof(SyncImportInvoiceService));

            host.Open();

            Console.WriteLine("SOAP Service Host Success and Satrt !  Do not Close this Winow!");
            Console.ReadLine();
        }
    }
}
