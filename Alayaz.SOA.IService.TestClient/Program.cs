using Alayaz.SOA.IService.TestClient.ServiceProxy;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
namespace Alayaz.SOA.IService.TestClient
{ 
   public class Program
    {
        static void Main(string[] args)
        {
            // TestService();
            //  StartApp("Alayaz.CM.DN432.WebCrawl.exe", new string[] { "chosen" });// chosen  /  crawl


            // TestJustStartServiceHostByConsole();


            // TestJustStartServiceHostByWinService();



            //Test_SyncImportInvoiceService_HostByWinService();


            // http://localhost:12000/alayazsoa/mex
            //Test_SyncImportInvoiceService_HostByConsole();

            // http://localhost:13000/alayazsoa/mex
            Test_SyncImportInvoiceService_HostByWindowsService();


        }

        private static void Test_SyncImportInvoiceService_HostByWindowsService()
        {
            using (RemoteServiceHostByWinServiceRef.SyncImportInvoiceServiceClient proxy = new RemoteServiceHostByWinServiceRef.SyncImportInvoiceServiceClient())
            {

                var soap = new Service.ViewModel.ImportInvoiceListDTO
                {
                    List = new System.Collections.Generic.List<Service.ViewModel.ImportInvoiceDTO>
                    {
                         new Service.ViewModel.ImportInvoiceDTO {
                              InvoiceCode = "1234567890",
                               InvoiceNumber = "87654321",
                                CertificateStatus="1",
                                 DeductionStatus="1",
                                  Amount=1000,
                                   Tax=170,
                                    SalesTaxNumber="110101000000000",
                                     TaxCode="110101999999999"
                         }
                    },
                    Result = new Service.ViewModel.ImportInvoiceResultDTO
                    {
                        Message = "ok",
                        Status = 0
                    }


                };
                proxy.InjectList(soap);
            }
            Console.Read();
        }

        private static void Test_SyncImportInvoiceService_HostByConsole()
        {
            using (RemoteServiceHostByConsoleRef.SyncImportInvoiceServiceClient proxy = new RemoteServiceHostByConsoleRef.SyncImportInvoiceServiceClient())
            {

                var soap = new Service.ViewModel.ImportInvoiceListDTO
                {
                    List = new System.Collections.Generic.List<Service.ViewModel.ImportInvoiceDTO>
                    {
                         new Service.ViewModel.ImportInvoiceDTO {
                              InvoiceCode = "1234567890",
                               InvoiceNumber = "12345678",
                                CertificateStatus="1",
                                 DeductionStatus="1",
                                  Amount=1000,
                                   Tax=170,
                                    SalesTaxNumber="110101000000000",
                                     TaxCode="110101999999999"
                         }
                    },
                    Result = new Service.ViewModel.ImportInvoiceResultDTO
                    {
                        Message = "ok",
                        Status = 0
                    }


                };
                proxy.InjectList(soap);
            }
            Console.Read();
        }

        private static void TestJustStartServiceHostByConsole()
        {
            //using (Alayaz.SOA.IService.TestClient.ServiceRefConsole. FileReaderClient proxy = new Alayaz.SOA.IService.TestClient.ServiceRefConsole.FileReaderClient())
            //{
            //    proxy.JustStart("", "chosen");
            //}
            using (Alayaz.SOA.IService.TestClient.ServiceRefConsole.FileReaderClient proxy = new Alayaz.SOA.IService.TestClient.ServiceRefConsole.FileReaderClient("BasicHttpBinding_IFileReader"))
            {
                proxy.JustStart("", "chosen");
            }
            

            Console.Read();
        }
        private static void Test_SyncImportInvoiceService_HostByWinService()
        {
            using (ImpInvServiceProxy.SyncImportInvoiceServiceClient proxy = new ImpInvServiceProxy.SyncImportInvoiceServiceClient())
            {

                var soap = new Service.ViewModel.ImportInvoiceListDTO
                {
                    List = new System.Collections.Generic.List<Service.ViewModel.ImportInvoiceDTO>
                    {
                         new Service.ViewModel.ImportInvoiceDTO {
                              InvoiceCode = "1234567890",
                               InvoiceNumber = "12345678",
                                CertificateStatus="1",
                                 DeductionStatus="1",
                                  Amount=1000,
                                   Tax=170,
                                    SalesTaxNumber="110101000000000",
                                     TaxCode="110101999999999"
                         }
                    },
                    Result = new Service.ViewModel.ImportInvoiceResultDTO
                    {
                        Message = "ok",
                        Status = 0
                    }


                };
                proxy.InjectList(soap);
            }
            Console.Read();
        }

        private static void TestJustStartServiceHostByWinService()
        {
            using (FileReaderClient proxy = new FileReaderClient())
            {
                proxy.JustStart("", "crawl");
            }
            Console.Read();
        }
        private static void TestService()
        {
            using (FileReaderClient proxy = new FileReaderClient())
            {
                string addPatchStr = proxy.Read("demo.txt");
                Console.WriteLine(DateTime.Now.ToShortDateString() + "： Account + ：{0}", addPatchStr);
                double addPatch = 0.00;
                if (double.TryParse(addPatchStr, out addPatch))
                {
                    int days = ((DateTime.Now - new DateTime(2012, 10, 1)).Days);
                    string content = (days * addPatch).ToString();
                    proxy.Write("demo.txt", content);
                }

            }
            Console.Read();
        }

        #region App 交互测试
        public static string AppName { get; set; }

        /// <summary>
        ///    StartApp("Alayaz.CM.DN432.WebCrawl.exe", new string[] { "chosen" });// chosen  /  crawl
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="paramlist"></param>
        public static void StartApp(string appName, string[] paramlist)
        {

            //var path = Directory.GetCurrentDirectory();
            //path = string.Format("{0}\\work\\{1}", path, appName);
            AppName = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("AppName")) ? @"C:\Alayaz\Debug\Alayaz.CM.DN432.WebCrawl.exe" : ConfigurationManager.AppSettings.Get("AppName");
            var arg1 = (paramlist != null && paramlist.Length > 0) ? paramlist[0] : "";
            Run(AppName, arg1);
        }

        private static string Run(string appPath, string arg1)
        {
            Process p = new Process();
            p.StartInfo.FileName = appPath;
            p.StartInfo.Arguments = "/c " + arg1;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.StandardInput.WriteLine("exit");

            return p.StandardOutput.ReadToEnd();
        }
        #endregion


    }
}
