using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;

namespace Alayaz.SOA.IService.WindowsServiceHost
{
    partial class HostingService : ServiceBase
    {
        public ServiceHost ServiceHost { get; private set; }

        public HostingService()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            this.ServiceHost = new ServiceHost(typeof(FileReaderService));

            this.ServiceHost.Open();

            //#region 测试windows服务中开启桌面应用程序
            JustStart("", "chosen");  // chosen  /  crawl
            //#endregion
        }
        protected override void OnStop()
        {
            this.ServiceHost.Close();
            this.ServiceHost = null;
        }

        #region App 交互
        public static string AppName { get; set; }


        /// <summary>
        ///  JustStart("",   "chosen"  );  // chosen  /  crawl
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="param"></param>
        public void JustStart(string appName, string param)
        {
            if (!string.IsNullOrEmpty(appName))
            {
                AppName = appName;
            }
            else
            {
                AppName = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("AppName")) ? @"C:\Alayaz\Debug\Alayaz.CM.DN432.WebCrawl.exe" : ConfigurationManager.AppSettings.Get("AppName");
            }
            StartApp(AppName, new string[] { param });// chosen  /  crawl

        }
        /// <summary>
        ///    StartApp("Alayaz.CM.DN432.WebCrawl.exe", new string[] { "chosen" });// chosen  /  crawl
        ///   
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="paramlist"></param>j
        public static void StartApp(string appName, string[] paramlist)
        {
            try
            {
                // var path = Directory.GetCurrentDirectory();// 寄宿在winservice中时为：C:\WINDOWS\system32\
                //  path = string.Format("{0}\\work\\{1}", path, appName);// 寄宿在winservice中时为： path= C:\WINDOWS\system32\work\Alayaz.CM.DN432.WebCrawl.exe
                var path = AppName;
                var arg1 = (paramlist != null && paramlist.Length > 0) ? paramlist[0] : "";
                LogHelper.WriteLog(typeof(FileReaderService), string.Format("path:{0}\n arg1:{1}", path, arg1));

                Run(path, arg1);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FileReaderService), ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException : ex.InnerException) : ex.InnerException);
                throw ex;
            }
        }

        private static void Run(string appPath, string arg1)
        {
            Process p = new Process();
            p.StartInfo.FileName = appPath;
            p.StartInfo.Arguments = string.IsNullOrEmpty(arg1) ? "" : "/c " + arg1;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
         }
        private static string Runit(string appPath, string arg1)
        {
            Process p = new Process();
            p.StartInfo.FileName = appPath;
            p.StartInfo.Arguments = string.IsNullOrEmpty(arg1) ? "" : "/c " + arg1;
            p.StartInfo.UseShellExecute = true;
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
