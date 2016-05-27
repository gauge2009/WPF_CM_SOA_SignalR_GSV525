using Alayaz.Graph.WPF.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Alayaz.CM.DN432.WebCrawl
{

    class Program
    {
        public static string s_BootArgs = string.Empty;

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
               // 从配置文件取
                    GlobalData.ImpInvViewModel.BootMode = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("BootMode")) ? "crawl" : ConfigurationManager.AppSettings.Get("BootMode");
             }

            if (args.Length == 2)
            {
                s_BootArgs = args[1];

                if (s_BootArgs == "crawl" || s_BootArgs == "confirm")
                {//先从启动参数取
                     GlobalData.ImpInvViewModel.BootMode = s_BootArgs;
                }
                else
                {//再从配置文件取
                    GlobalData.ImpInvViewModel.BootMode = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("BootMode")) ? "crawl" : ConfigurationManager.AppSettings.Get("BootMode");
                }
                //MessageBox.Show(s_BootArgs);
            }
            if (args.Length == 4)
            {// > C:\Alayaz\Alayaz.CM.DN432.WebCrawl.exe /c chosen 2016-05-01 2016-05-21
                GlobalData.ImpInvViewModel.Begin = args[2]; // 2016-05-01
                GlobalData.ImpInvViewModel.End = args[3]; // 2016-05-21

            }

            Alayaz.CM.DN432.WebCrawl.App app = new Alayaz.CM.DN432.WebCrawl.App();
            app.Run();
            Alayaz.CM.DN432.WebCrawl.App.Main();
        }
    }


    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {     
        public string DeviceKey { get; set; }

        public static string RegPath = @"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION";



        public App()
        {
            #region DatePicker日期格式自定义 
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            #endregion

            InitializeComponent();
            var bootstrpper = new CoroutinesBootstrapper();
            bootstrpper.Initialize();
            // var AppName =  "Alayaz.CM.DN432.WebCrawl";
            //ChangeDefaultBrowser(AppName);

            //证书密码获取
            //V3.1
            this.DeviceKey = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("DeviceKey")) ? "" : ConfigurationManager.AppSettings.Get("DeviceKey");
            GlobalData.PWD = this.DeviceKey;
            //V3.2先从开票软件取，再从本地配置取
            //TODO






        }
        /*
                 64 bit or 32 bit only machine:
        HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION
        Value Key: DWORD - YourApplication.exe

                 32 bit on 64 bit machine:
        HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION
        Value Key: DWORD YourApplication.exe
         */
        public static void ChangeDefaultBrowser(string appName)
        {
            RegistryKey key;
            var access = Registry.LocalMachine.GetAccessControl();
            key = Registry.LocalMachine.OpenSubKey(RegPath, true);
            //key = Registry.LocalMachine.CreateSubKey(RegPath);
            key.SetValue("Alayaz.CM.DN432.WebCrawl.exe", Convert.ToInt32("1f40", 16), RegistryValueKind.DWord);
        }




        public static string RegPath_Size = @"Software\MyApp\";

        public static void SaveSize(Window win)
        {

            RegistryKey key;
            key = Registry.CurrentUser.CreateSubKey(RegPath_Size + win.Name);

            key.SetValue("Bounds", win.RestoreBounds.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public static void SetSize(Window win)
        {
            RegistryKey key;
            key = Registry.CurrentUser.OpenSubKey(RegPath_Size + win.Name);

            if (key != null)
            {
                Rect bounds = Rect.Parse(key.GetValue("Bounds").ToString());

                win.Top = bounds.Top;
                win.Left = bounds.Left;

                // Only restore the size for a manually sized
                // window.
                if (win.SizeToContent == SizeToContent.Manual)
                {
                    win.Width = bounds.Width;
                    win.Height = bounds.Height;
                }
            }
        }
    }
}
