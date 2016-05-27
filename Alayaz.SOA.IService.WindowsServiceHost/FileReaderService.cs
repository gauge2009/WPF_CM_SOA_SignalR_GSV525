using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
namespace Alayaz.SOA.IService.WindowsServiceHost
{
    public class FileReaderService : IFileReader
    {
        #region 日志
        private const string baseLocation = @"C:\AlayazLogs\";
        private FileStream _stream;
        private byte[] _buffer;

        public IAsyncResult BeginRead(string fileName, AsyncCallback userCallback, object stateObject)
        {
            _stream = new FileStream(baseLocation + fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            _buffer = new byte[this._stream.Length];
            return _stream.BeginRead(this._buffer, 0, _buffer.Length, userCallback, stateObject);
        }
        public string EndRead(IAsyncResult ar)
        {
            _stream.EndRead(ar);
            _stream.Close();
            return Encoding.ASCII.GetString(_buffer);
        }

        private FileStream _stream2;

        public void Write(string fileName, string content)
        {
            _stream2 = new FileStream(baseLocation + fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(_stream2);
            sw.WriteLine(content);
            sw.Close();
            _stream2.Close();
        }

        #endregion

        #region App 交互
        public static string AppName { get; set; }
 

        /// <summary>
        ///  JustStart("",   "chosen"  );  // chosen  /  crawl
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="param"></param>
        public void JustStart(string appName, string param )
        {
            if (!string.IsNullOrEmpty(appName))
            {
                 AppName = appName;
            }
            else
            {
                AppName = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("AppName")) ? @"C:\Alayaz\Debug\Alayaz.CM.DN432.WebCrawl.exe" : ConfigurationManager.AppSettings.Get("AppName");
              
            }
            StartApp( AppName, new string[] { param });// chosen  /  crawl

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
                var path =  AppName;
                var arg1 = (paramlist != null && paramlist.Length > 0) ? paramlist[0] : "";
                LogHelper.WriteLog(typeof(FileReaderService), string.Format("path:{0}\n arg1:{1}", path, arg1));

                Run(path, arg1);
            }catch(Exception ex)
            {
                LogHelper.WriteLog( typeof(FileReaderService) , ex.InnerException!=null? (ex.InnerException.InnerException!=null? ex.InnerException.InnerException: ex.InnerException) : ex.InnerException);
                throw ex;
            }
        }

        private static string Run(string appPath, string arg1)
        {
            Process p = new Process();
            p.StartInfo.FileName = appPath;
            p.StartInfo.Arguments = string.IsNullOrEmpty(arg1) ? "" : "/c " + arg1;
           
            // p.StartInfo.UseShellExecute = false;
            p.StartInfo.UseShellExecute = true;
            //p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = true;
           // p.StartInfo.CreateNoWindow = false;

            p.Start();
            // p.StandardInput.WriteLine("exit");

            return "ok"; // p.StandardOutput.ReadToEnd();
        }

 
        #endregion

    }
}
