using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading;
using System.Windows.Controls;
using Alayaz.CM.DN432.WebCrawl.Coroutines;
using System.Windows;
using Alayaz.CM.DN432.WebCrawl.Views;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Reflection;
using Alayaz.Graph.WPF.Common;

namespace Alayaz.CM.DN432.WebCrawl.ViewModels {

    //[Export("LoginScreen", typeof(LoginViewModel))]
    [Export(  typeof(LoginViewModel))]

    public class LoginViewModel : Screen
    {
        private static string s_startUri = "https://fpdk.szgs.gov.cn/";
        private static string s_targetUri = "https://fpdk.szgs.gov.cn/fpcx.html";
        private static string s_htmlFake = "N/A";
        private static string s_htmlFakeFilePath = "demo.txt";
        private bool isStartUriOpenned = false;
        public string IsOffline { get; set; }
        public string IfCallWS { get; set; }
        public string IfLog { get; set; }
        public string DeviceKey { get; set; }
        public string HtmlFakeFilePath { get; set; }
        public bool CanLoginRemote { get; set; }
        public bool LoginRemoteResult { get; set; }

        public string PassWd { get; set; }

        //private string passWd;
        //public string PassWd
        //{
        //    get { return passWd; }

        //    set
        //    {
        //        passWd = value;
        //        NotifyOfPropertyChange(() => PassWd);
        //    }
        //}
        private string startUri;
        public string StartUri
        {
            get { return startUri; }

            set
            {
                startUri = value;
                NotifyOfPropertyChange(() => StartUri);
            }
        }

        private string targetUri;
        public string TargetUri
        {
            get { return targetUri; }

            set
            {
                targetUri = value;
                NotifyOfPropertyChange(() => TargetUri);
            }
        }
        private string tipInfo;
        public string TipInfo
        {
            get { return tipInfo; }

            set
            {
                tipInfo = value;
                NotifyOfPropertyChange(() => TipInfo);
            }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }

            set
            {
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }
        private string busyText;
        public string BusyText
        {
            get { return busyText; }

            set
            {
                busyText = value;
                NotifyOfPropertyChange(() => BusyText);
            }
        }
        private string contract;
        public string Contract
        {
            get { return contract; }

            set
            {
                contract = value;
                NotifyOfPropertyChange(() => Contract);
            }
        }

        private string imgPath;
        public string ImgPath
        {
            get { return imgPath; }

            set
            {
                imgPath = value;
                NotifyOfPropertyChange(() => ImgPath);
            }
        }
        private string ifShowContract;
        public string IfShowContract
        {
            get { return ifShowContract; }

            set
            {
                ifShowContract = value;
                NotifyOfPropertyChange(() => IfShowContract);
            }
        }

        public LoginViewModel()
        {




            InitFromConfig();
            this.ImgPath = "pack://application:,,,/Alayaz.CM.DN432.WebCrawl;Component/imgs/user.png";
        }

        private void InitFromConfig()
        {
            this.HtmlFakeFilePath = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("HtmlFakeFilePath")) ? s_htmlFakeFilePath : ConfigurationManager.AppSettings.Get("HtmlFakeFilePath");

            this.IsOffline = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("IsOffline")) ? "0" : ConfigurationManager.AppSettings.Get("IsOffline");

            this.IfCallWS = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("IfCallWS")) ? "1" : ConfigurationManager.AppSettings.Get("IfCallWS");
            this.IfLog = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("IfLog")) ? "1" : ConfigurationManager.AppSettings.Get("IfLog");
            this.DeviceKey = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("DeviceKey")) ? "" : ConfigurationManager.AppSettings.Get("DeviceKey");

            this.StartUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("StartUri")) ? s_startUri : ConfigurationManager.AppSettings.Get("StartUri");


            this.TargetUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TargetUri")) ? s_targetUri : ConfigurationManager.AppSettings.Get("TargetUri");

            this.TipInfo = "特别提醒：密码错误3次以上会导致锁盘，请您谨慎操作";

            this.LoginRemoteResult = false;
            this.CanLoginRemote = true;
        }
 
        
        public IEnumerable<IResult> Login(object view, Button btn)
        {
               PasswordBox pb = null;
             var vw = view as LoginView;
             if (vw != null)
             {
                 pb = vw.FindName("PassWd") as PasswordBox;
                  if (pb == null ||string.IsNullOrEmpty( pb.Password))
                 {
                     this.CanLoginRemote = false;
                     MessageBox.Show("密码不能为空");
                 }
                 else
                 {
                     this.CanLoginRemote = true;
                    this.PassWd = pb.Password;
                 }
             }
             
 
            if (this.CanLoginRemote)
            {

                WebBrowser wb = vw.FindName("webLogin") as WebBrowser;
                if (wb == null)
                {
                    MessageBox.Show("网络异常");
                    yield return null;
                }
               
                wb.Source = new Uri(this.StartUri);

                 //<N>JR:验证后续流程基于异步事件通知实现
                //this.LoginRemoteResult =  LoginRemote(this.PassWd);

            }
            //if (this.LoginRemoteResult)
            // {//登录成功
 
            //    yield return Loader.Show("正在加载..");

            //     yield return new ShowScreen("WebBox");
            // }
            //else
            //{
            //    MessageBox.Show("登录失败");
            //}
            
        }

        //private bool LoginRemote(string pwd)
        //{

        //    MessageBox.Show(pwd);

        //    return true;
        //}


        public void NavigatingHandler(NavigationEventArgs e, object view, WebBrowser wb)
        {
            this.IsBusy = true;
            this.BusyText = "正在验证身份信息......";
            if (wb == null)
                return;
            wb.Visibility = Visibility.Hidden;
            var vw = view as LoginView;
            if (vw != null)
            {
                ProgressBar pBar = vw.FindName("ProgBar") as ProgressBar;
                if (pBar != null)
                {
                    ConfigProgressBar(pBar);

                }

            }

            //if ((Uri)e.Uri == new Uri(this.TargetUri))
            //{
            //this.TipInfo = "Loading......";
            //MessageBox.Show("Loading......");

            //}
            #region WebBrowser控件不弹脚本错误提示框 
           SetWebBrowserSilent(wb, true);
            #endregion
        }

        private void SetWebBrowserSilent(WebBrowser webBrowser, bool silent)
        {
            FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(webBrowser);
                if (browser != null)
                    browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
            }
        }
        public void LoadCompletedHandler(NavigationEventArgs e, object view, WebBrowser wb)
        {
            var currentUrl = wb.Source.AbsoluteUri;
            if (wb == null)
                return;
 
            var vw = view as LoginView;
            if (vw != null)
            {
                ProgressBar pBar = vw.FindName("ProgBar") as ProgressBar;
                if (pBar != null)
                {
                    pBar.Visibility = Visibility.Hidden;

                }

            }

            this.IsBusy = false;
           /* if (currentUrl.Contains("#"))
            {

                currentUrl = currentUrl.Replace("#", "");
            }
            
  
            if (currentUrl == this.TargetUri)
            {
                this.TipInfo = "连接授权进行中...";
                //MessageBox.Show("请先查询所需的进向数据");
                // StartCrawl();
            }
            else if (currentUrl != this.StartUri)
            {//除首页登录页面外，都重定向到fpcx.html
                //<N>JR:这里依赖于浏览器缓存实现两个wb共享Cookies
                ShowScreen("Crawl", typeof(CrawlViewModel));
                //wb.Source = new Uri(this.TargetUri);
                return;
            }
            */

            bool isOffline = string.Compare(this.IsOffline, "1", StringComparison.InvariantCultureIgnoreCase) == 0 ? true : false;
            mshtml.IHTMLDocument2 doc2 = isOffline ? null : (mshtml.IHTMLDocument2)wb.Document;
            string html = isOffline ? s_htmlFake : doc2.body.innerHTML;


            #region Auto Login  放在Crawl中！！！
            GlobalData.PWD = this.PassWd;
            ShowScreen("Crawl", typeof(CrawlViewModel));
           
            /*
            mshtml.IHTMLWindow2 win = (mshtml.IHTMLWindow2)doc2.parentWindow;
            if ( !string.IsNullOrEmpty(this.PassWd)&&this.PassWd.ToLower()!="go")
            {
               
                win.execScript("Login('"+ this.PassWd + "', '', 1)", "javascript");
                return;

            }
            else if (this.PassWd.ToLower() == "go"&&!string.IsNullOrEmpty(this.DeviceKey))
            {
                win.execScript("Login('" + this.DeviceKey + "', '', 1)", "javascript");
            }
            else
            {
                MessageBox.Show("密钥缺失");

            }
            */
            #endregion


            // ShowScreen("", typeof(StartScreenViewModel));
        }


        #region ProcessBar
 
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void ConfigProgressBar(ProgressBar pBar, DispatcherPriority dispatcherPriority = DispatcherPriority.Loaded, bool isAsync = false)
        {
            pBar.Visibility = Visibility.Visible;

            pBar.Maximum = 100;
            pBar.Value = 0;

            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pBar.SetValue);

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(10);

                if (isAsync)
                    Dispatcher.CurrentDispatcher.BeginInvoke(updatePbDelegate, dispatcherPriority, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(i + 1) });
                else
                    Dispatcher.CurrentDispatcher.Invoke(updatePbDelegate, dispatcherPriority, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(i + 1) });
            }
        }
        #endregion
        private void ShowScreen(string name, Type screenType)
        {
            var screen = !string.IsNullOrEmpty(name)
                ? IoC.Get<object>(name)
                : IoC.GetInstance(screenType, null);

            ((IConductor)Parent).ActivateItem(screen);
        }

    }
}