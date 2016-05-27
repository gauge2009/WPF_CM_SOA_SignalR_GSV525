using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using hParser = Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using System.ServiceModel;
using System.Threading;
using Alayaz.CM.DN432.WebCrawl.Views;
using mshtml;
using Alayaz.CM.DN432.WebCrawl.Common;
using System.Windows.Input;
using System.ComponentModel;
using Alayaz.SOA.Service.ViewModel;
using Alayaz.SOA.IService;
using jaxws = Alayaz.CM.DN432.WebCrawl.ImpInvoiceServiceReference;
using Alayaz.Graph.WPF.Common;
using Alayaz.CM.DN432.WebCrawl.ServiceProxy;
//using Alayaz.CM.DN432.WebCrawl.ServiceProxy;

namespace Alayaz.CM.DN432.WebCrawl.ViewModels
{

    //  [Export("Confirm", typeof(ConfirmViewModel))]
    [Export(typeof(ConfirmViewModel))]
    public partial class ConfirmViewModel : BaseViewModel //Screen
    {


        private static string s_startUri = "https://fpdk.szgs.gov.cn/";
        private static string s_targetUri = "https://fpdk.szgs.gov.cn/fpcx.html";
        private static string s_chosenUri = "https://fpdk.szgs.gov.cn/fphx.html";
        private static string s_confirmUri = "https://fpdk.szgs.gov.cn/sbqr.html";
        private static string s_htmlFake = "N/A";
        private static string s_htmlFakeFilePath = "demo.txt";
        private bool isStartUriOpenned = false;
        #region Traditional Prop

        public string ChosenUri { get; set; }
        public string ConfirmUri { get; set; }
        public string IsOffline { get; set; }
        public string IfCallWS { get; set; }
        public string IfLog { get; set; }
        public string DeviceKey { get; set; }
        public string TaxCode { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public string RequestParamsSourceForConfirm { get; set; }
        ///// <summary>
        /////  MESSAGEBOX / LOG  /  PERSIST 
        ///// </summary>
        //public string InteractMode { get; set; }

        //private log4net.ILog Log { get; set; }

        public string HtmlFakeFilePath { get; set; }

        /// <summary>
        /// SOAP交互模式   IISATV / IIS / WINSVC
        /// </summary>
        private string SoapMode { get; set; }



        IList<int> start = new List<int>();
        public string parseResult { get; set; }

        #endregion
        #region Prop with NotifyOfPropertyChange
 
      

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

       
        #endregion

        public ConfirmViewModel() : base()
        {
             
            InitFromConfig();


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

            this.TaxCode = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TaxCode")) ? "" : ConfigurationManager.AppSettings.Get("TaxCode");

            this.RequestParamsSourceForConfirm = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("RequestParamsSourceForConfirm")) ? "" : ConfigurationManager.AppSettings.Get("RequestParamsSourceForConfirm");
            this.Begin = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("Begin")) ? "" : ConfigurationManager.AppSettings.Get("Begin");

            this.End = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("End")) ? "" : ConfigurationManager.AppSettings.Get("End");

            this.InteractMode = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("InteractMode")) ? "MESSAGEBOX" : ConfigurationManager.AppSettings.Get("InteractMode");


            this.SoapMode = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("SoapMode")) ? "WINSVC" : ConfigurationManager.AppSettings.Get("SoapMode");

        

            GlobalData.TaxCode = this.TaxCode;
            //if( string.IsNullOrEmpty(this.TaxCode))
            //{
            //    Interact("税号未配置，请配置 TaxCode");
            //    return;
            //}



            this.TipInfo = "欢迎使用进项确认助手";

            this.ChosenUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ChosenUri")) ? s_chosenUri : ConfigurationManager.AppSettings.Get("ChosenUri");

            this.ConfirmUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ConfirmUri")) ? s_confirmUri: ConfigurationManager.AppSettings.Get("ConfirmUri");

 

        }


 
        #region  Methods for Action

 
        public void InitSource(WebBrowser wb, object view)
        {
 
            if (wb == null)
                return;
 
            if (!isStartUriOpenned)
            {
                wb.Source = new Uri(this.StartUri);
                isStartUriOpenned = true;
            }

            #region Busy start!
            this.IsBusy = true;
            this.BusyText = "正在加载资源，请稍后......";

            wb.Visibility = Visibility.Hidden;
            var vw = view as ConfirmView;
            if (vw != null)
            {
                ProgressBar pBar = vw.FindName("ProgBar") as ProgressBar;
                if (pBar != null)
                {
                    ConfigProgressBar(pBar, DispatcherPriority.SystemIdle, true);

                }

            }
            #endregion

            SetWebBrowserSilent(wb, true);

        }

        /// <summary>
        /// <N> 使用ActiveItem时不会触发
        /// </summary>
        /// <param name="e"></param>
        /// <param name="view"></param>
        /// <param name="wb"></param>
        public void NavigatingHandler(NavigationEventArgs e, object view, WebBrowser wb)
        {
            this.IsBusy = true;
            if (wb == null)
                return;
            var currentUrl = wb.Source.AbsoluteUri;
            if (currentUrl == this.StartUri)
            {
                this.BusyText = "正在执行身份验证，请稍后......";
            }
            else
            {
                this.BusyText = "正在加载数据资源，请稍后......";
            }
            this.TipInfo = currentUrl.Replace("https://fpdk.szgs.gov.cn/", "").Replace(".html", "") + "  ing...";

            //wb.Visibility = Visibility.Hidden;
            var vw = view as ConfirmView;
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
            //Interact("Loading......");

            //}
            #region WebBrowser控件不弹脚本错误提示框 
            SetWebBrowserSilent(wb, true);
            #endregion
        }

       private Stopwatch sw = Stopwatch.StartNew();

        /// <summary>
        /// AJAX不会触发LoadCompleted
        /// </summary>
        /// <param name="e"></param>
        /// <param name="view"></param>
        /// <param name="wb"></param>
        public void LoadCompletedHandler(NavigationEventArgs e, object view, WebBrowser wb)
        {
            if (string.IsNullOrEmpty(this.TaxCode))
            {
                string msg = "您尚未配置税号，请先配置TaxCode和DeviceKey节点再使用本软件";
                Interact(msg);
                LogHelper.WriteLog(typeof(ConfirmView), string.Format("Some Configuration Missing:{0} | TaxCode:{1}", msg, GlobalData.TaxCode));
                App.Current.Shutdown();
                //return;
            }
            var currentUrl = wb.Source.AbsoluteUri;
            this.TipInfo = currentUrl.Replace("https://fpdk.szgs.gov.cn/", "").Replace(".html", "") + " 准备就绪";
            if (wb == null)
                return;
              wb.Visibility = Visibility.Visible;

            var vw = view as ConfirmView;
            if (vw != null)
            {
                ProgressBar pBar = vw.FindName("ProgBar") as ProgressBar;
                if (pBar != null)
                {
                    pBar.Visibility = Visibility.Hidden;

                }

            }

            if (currentUrl.Contains("fpcx.html#"))
            {// 若查询后续流程基于异步事件通知实现，AutoCrawl模式下进入查询页面后直接抓取即可
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             //      StartCrawl(vw);
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            if (currentUrl == this.ChosenUri)
            {
                this.IsBusy = false;

                //this.TipInfo = "加载已完成";
                //Interact("请先查询所需的进向数据");

            }
            else if (currentUrl != this.StartUri)
            {//除首页登录页面外，都重定向到fpcx.html
                wb.Source = new Uri(this.ChosenUri);
            }

            bool isOffline = string.Compare(this.IsOffline, "1", StringComparison.InvariantCultureIgnoreCase) == 0 ? true : false;
            mshtml.IHTMLDocument2 doc2 = isOffline ? null : (mshtml.IHTMLDocument2)wb.Document;
            string html = isOffline ? s_htmlFake : doc2.body.innerHTML;

            #region 加迷彩   login_header  login_footer
            mshtml.IHTMLElement login_header = (mshtml.IHTMLElement)doc2.all.item("login_header", 0);
            mshtml.IHTMLElement login_footer = (mshtml.IHTMLElement)doc2.all.item("login_footer", 0);
            mshtml.IHTMLElement footer = (mshtml.IHTMLElement)doc2.all.item("footer", 0);
            mshtml.IHTMLElement header = (mshtml.IHTMLElement)doc2.all.item("header", 0);

            if (login_header != null)
            {
                // login_header.setAttribute("style", "display: none;");
                login_header.innerHTML = "";
            }
            if (login_footer != null)
            {
                // login_header.setAttribute("style", "display: none;");
                login_footer.innerHTML = "";
            }
            if (footer != null)
            {
                // login_header.setAttribute("style", "display: none;");
                footer.innerHTML = "";
            }
            if (header != null)
            {
                // login_header.setAttribute("style", "display: none;");
                header.innerHTML = "";
            }

            //屏蔽右键
            //(wb.Document as mshtml.HTMLDocumentEvents_Event).oncontextmenu += new mshtml.HTMLDocumentEvents_oncontextmenuEventHandler(ExtendFrameControl_oncontextmenu);
            //wb.ContextMenu = null;
            //wb.ContextMenuOpening += Wb_ContextMenuOpening;

            #endregion


            mshtml.IHTMLWindow2 win = (mshtml.IHTMLWindow2)doc2.parentWindow;

            #region Auto Login
            if (currentUrl == this.StartUri && !string.IsNullOrEmpty(this.DeviceKey))
            {
                if (string.IsNullOrEmpty(GlobalData.PWD))
                {
                    Interact("空密钥");
                    return;
                }
                else if (string.Compare(GlobalData.PWD, this.DeviceKey, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    Interact("检测到您输入的密码与惯常使用的密码不一致，登录失败");
                    ShowScreen("", typeof(LoginViewModel));
                }
              win.execScript(string.Format("Login('{0}', '', 1)", GlobalData.PWD), "javascript");
                //  win.execScript(string.Format("Login('{0}', '')", GlobalData.PWD), "javascript");

                // CPU轮询
                Interact("execScript Login()", true);

                //<N>JR:在UI线程处理轮询会阻塞此LoadCompleted的完成，导致UI阻塞，即使AJAX已经完成也不会更新wb的文档！！！
                ///必须在工作线程中轮询，然后借助同步上下文更新UI并触发后续工作流！
                ///
                sw.Reset();
                sw.Start();
                 var task = new Task(() => PollingCheck(vw, doc2, CheckMode.CheckIsDataValidWhenLoginHtmlPartialUpdate,sw));
                task.Start();

            }

            #endregion

            if (currentUrl == this.StartUri)
            {
                return;
            }
            // ShowScreen("", typeof(StartScreenViewModel));
            #region  Auto Search
            if (currentUrl == this.ChosenUri)
            {
                Button btn = vw.FindName("Chose") as Button;
                switch (this.RequestParamsSourceForConfirm)
                {
                    case "WS":
                        //dateBegin = GlobalData.ImpInvViewModel.Begin; // 2016-05-4
                        //dateEnd = GlobalData.ImpInvViewModel.End;
                        AutoCrawl(view, btn);
                        break;
                    case "CONFIG":
                        dateBegin = this.Begin; // 2016-05-4
                        dateEnd = this.End;
                   //     AutoCrawl(view, btn);
                        break;
                    default:
                    case "DB":
                        //RPC取值
                        var now = DateTime.Now;
                        ImportInvoiceDTO cond = new ImportInvoiceDTO
                        {
                            IsChosen = "1",
                            IsConfirmed = "0",

                            BeginDateTime = now.AddMonths(-1),
                            EndDateTime = now.AddDays(2),
                             TaxCode = this.TaxCode
                             
                         };
                        RPC_GetChosenUnconfirmedList_From_WinSrvHost(cond);

                        DealWithDate(  this.ChosenUnconfirmedList );

                        AutoCrawl(view, btn);

                        break;
                }
            }

            #endregion
        }
        private void DealWithDate(  ImportInvoiceListDTO list)
        {
            if (list.Result.Status > 0)
            {
                DateTime beginDateTime = list.List.Min(o => o.CreateDateTime);
                DateTime endDateTime = list.List.Max(o => o.CreateDateTime);
                dateBegin = beginDateTime.ToString("yyyy-MM-dd");  // 2016-05-4
                dateEnd = endDateTime.ToString("yyyy-MM-dd");
            }
        }
        private string dateBegin = "";
        private string dateEnd = "";
        private bool IsChoseChecked = true;

        public void AutoCrawl(object view, Button btn)//  private void BtnDownload_Click(object sender, 
        {
            Uri uri = new Uri(this.ChosenUri);
            Uri uri2 = new Uri(string.Format("{0}#", this.ChosenUri));
            WebBrowser wb = null;
            var vw = view as ConfirmView;
            if (vw != null)
            {
                wb = vw.FindName("webBox") as WebBrowser;
#if DEBUG
                mshtml.IHTMLDocument2 _doc2 = (mshtml.IHTMLDocument2)wb.Document;
                string html = _doc2.body.innerHTML;
                Debug.WriteLine(html);
#endif
                if (wb == null || (wb.Source != uri && wb.Source != uri2))
                {
                    Interact("网络异常，请确保您的网络连接正常后重新运行软件（没有适合的数据）");
                    return;
                }
            }
            #region 模拟点击已勾选 IsUnConfirmChecked  (hxzt0~未勾选 /  hxzt1~已勾选)  +  input注入  sjq  sjz
            var currentUrl = wb.Source.AbsoluteUri;
            if (currentUrl.Contains("#"))
            {//判断是查询后的页面
                currentUrl = currentUrl.Replace("#", "");
            }
            mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)wb.Document;

            if (currentUrl == this.ChosenUri)
            {
                #region 勾选标志 IsUnConfirmChecked   (hxzt0~未勾选 /  hxzt1~已勾选) 
                mshtml.IHTMLElement hxzt1 = (mshtml.IHTMLElement)doc2.all.item("hxzt1", 0);
                if (hxzt1 != null)
                {
                    IsChoseChecked = (bool)hxzt1.getAttribute("checked");
                    // login_header.setAttribute("style", "display: none;");
                    Debug.WriteLine(IsChoseChecked);
                }
                #endregion
                #region  input注入  sjq  sjz

                mshtml.IHTMLElement sjq = (mshtml.IHTMLElement)doc2.all.item("sjq", 0);
                mshtml.IHTMLElement sjz = (mshtml.IHTMLElement)doc2.all.item("sjz", 0);


                if (sjq == null || sjz == null)
                {
                    Interact("数据加载格式错误，未预期的sjq/sjz控件缺失");
                }
                sjq.setAttribute("value", dateBegin);
                sjz.setAttribute("value", dateEnd);

                #endregion
            }
            #endregion
            #region  触发查询 <A onclick=searchInfo(); id=search class="button button-blue" href="#" onFocus="undefined"><SPAN>查询</SPAN></A> 
            if (currentUrl == this.ChosenUri)
            {
                mshtml.IHTMLWindow2 win = (mshtml.IHTMLWindow2)doc2.parentWindow;

                win.execScript("searchInfo()", "javascript");
                //Thread.Sleep(4000); // too bad !
            }
            //法1：<N>JR:查询后续流程基于异步事件通知实现

            //法2：<N>JR:同步直接抓取
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //  btn.Visibility = Visibility.Hidden;


            //判断是否是查询后的页面(只有含有有效非空表格的才执行抓取)
            //<N>
            //对于click后AJAX局部更新的页面，处理方式有两种：
            //法1：脚本注入+js互操作C#是最好的方式
            //法2： CPU轮询！


#if DEBUG
            Interact("exec  查询-发票勾选状态", true);
 #endif

            //<N>JR:在UI线程处理轮询会阻塞此LoadCompleted的完成，导致UI阻塞，即使AJAX已经完成也不会更新wb的文档！！！
            //// PollingCheck(vw, doc2);
            ///必须在工作线程中轮询，然后借助同步上下文更新UI并触发后续工作流！
            ///
            sw.Reset();
            sw.Start();
             var task = new Task(() => PollingCheck(vw, doc2, CheckMode.CheckIsDataValidWhenFphxHtmlPartialUpdate, sw));
            //
            task.Start();
            //Task.WaitAll(task);
            #endregion



        }

        // public delegate void StartCrawlDelegate(ConfirmView vw);

           

        private void PollingCheck(ConfirmView vw, IHTMLDocument2 doc2, CheckMode checkmode, Stopwatch sw)
        {
             var hasValidData = false;
            switch (checkmode)
            {
                case CheckMode.CheckIsDataValidWhenFphxHtmlPartialUpdate:
                    hasValidData = CheckIsDataValidWhenFphxHtmlPartialUpdate(doc2);
                    break;
                default:
                case CheckMode.CheckIsDataValidWhenLoginHtmlPartialUpdate:

                    hasValidData =CheckIsDataValidWhenLoginHtmlPartialUpdate(doc2);
                    break;

            }


            if (hasValidData)
            {

                 if (checkmode == CheckMode.CheckIsDataValidWhenLoginHtmlPartialUpdate)
                {
                    Interact("login done!");
                }
                else if (checkmode == CheckMode.CheckIsDataValidWhenFphxHtmlPartialUpdate)
                {
                    // ui thread
                    Action<ConfirmView> action = StartAutoChosen;
                    vw.Dispatcher.Invoke(action, DispatcherPriority.Normal, new object[] {
                   vw
                     });
                    //vw.Dispatcher.Invoke(   new StartCrawlDelegate(StartCrawl), new object[] { vw });
                    // work thread
                    Interact("search chosen done!");
                }

            }
            else
            {

                if (sw.Elapsed <= TimeSpan.FromSeconds(20))
                {
                    //  CPU轮询！+ 线程休眠 （中庸之道）
                    Thread.Sleep(1000);
                    PollingCheck(vw, doc2, checkmode,sw);
                }
                else
                {
                    Interact("网络异常/票源服务器异常，重试失败！（无法获取进项数据）");
                }
            }

        }

         /// <summary>
         /// 有效数据才能自动翻页
         /// </summary>
         /// <param name="doc2"></param>
         /// <returns></returns>
        private bool CheckIsDataValidWhenFphxHtmlPartialUpdate(mshtml.IHTMLDocument2 doc2)
        {

            bool hasValidData = true;
            mshtml.HTMLTableClass table = IsChoseChecked ? (mshtml.HTMLTableClass)doc2.all.item("example1", 0) : (mshtml.HTMLTableClass)doc2.all.item("example", 0);
            if (table == null)
            {
                hasValidData = false;
                //throw new InvalidOperationException("无效table");
            }
            mshtml.HTMLTableSectionClass tbody = (mshtml.HTMLTableSectionClass)table.lastChild;
            if (tbody == null)
            {
                hasValidData = false;
                //throw new InvalidOperationException("无效tbody");
            }

            var tbodyHtml = tbody.innerHTML;
            //class=dataTables_empty
            if (0 == string.Compare(tbody.innerText, "没找到记录", StringComparison.InvariantCultureIgnoreCase))
            {
                hasValidData = false;
                //throw new InvalidOperationException("无效tbody");
            }
            Lexer lexer = new Lexer(tbodyHtml);
            hParser.Parser parser = new hParser.Parser(lexer);
            hParser.NodeFilter filter = new NodeClassFilter(typeof(Winista.Text.HtmlParser.Tags.TableRow));
            NodeList nodeList = parser.Parse(filter);
            if (nodeList.Count == 0)
            {
                hasValidData = false;
                // Interact("没有符合要求的节点");
            }
            return hasValidData;
        }

        private bool CheckIsDataValidWhenLoginHtmlPartialUpdate(mshtml.IHTMLDocument2 doc2)
        {
            bool hasValidData = true;
            #region 监听 _._alert 消息

            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            var warn = CheckHtmlSearchForAlert(doc2);
            if (!string.IsNullOrEmpty(warn))
            {
                Interact(warn);
                hasValidData = false;
            }
            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            #endregion
            return hasValidData;
        }

 
        private bool _IsLastPage = false;

        public void StartAutoChosen(ConfirmView vw)//  private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(this.ChosenUri);
            Uri uri2 = new Uri(string.Format("{0}#", this.ChosenUri));
#if DEBUG
            Interact(string.Format("Start Auto Chosen on Task{0}", Thread.CurrentThread.ManagedThreadId.ToString()));
            //this.TipInfo = string.Format("StartCrawl on Task{0}", Thread.CurrentThread.ManagedThreadId.ToString());

#endif  

            WebBrowser wb = null;
            if (vw != null)
            {

                wb = vw.FindName("webBox") as WebBrowser;
                if (wb == null || (wb.Source != uri && wb.Source != uri2))
                {
                    Interact("没有适合的数据");
                    return;
                }

            }


            bool isOffline = string.Compare(this.IsOffline, "1", StringComparison.InvariantCultureIgnoreCase) == 0 ? true : false;

            #region Auto pagenate ： id = example1_next  

            this.IsBusy = true;
            this.BusyText = "正在同步数据，请稍后......";


            ProgressBar pBar = vw.FindName("ProgBar") as ProgressBar;
            if (pBar != null)
            {
                ConfigProgressBar(pBar);
            }



            //下一页定总页数和边界条件
            var pNum = 1;
            bool hasValidData = true;
            do
            {
                #region Crawl Current Page
                //法1：<N>基于Httphelper，这样下载会要求程序自己实现验证授权
                //HttpHelper httpHelper = new HttpHelper();
                //HttpItem rq = new HttpItem();
                //rq.URL = uri.AbsoluteUri;
                //HttpResult html = httpHelper.GetHtml(rq);
                //Debug.WriteLine(html.Html);

                //法2：直接基于WebBrowser，授权是由用户手动实现的
                //////////////////////////////////////////////////
                //////////////////////////////////////////////////
                //////////////////////////////////////////////////
                ChosenInCurrentPage(wb, isOffline, IsChoseChecked, ref hasValidData);
                //////////////////////////////////////////////////
                //////////////////////////////////////////////////
                //////////////////////////////////////////////////
                #endregion
                if (isOffline)
                {

                    Interact("离线测试结束");
                    return;

                }
                if (hasValidData)
                {
                    this.IsBusy = true;

                    this.BusyText = string.Format("已同步第{0}页数据", (pNum++).ToString());
                    _IsLastPage = GotoNextPage(wb, _IsLastPage);

                }
                else
                {
                    this.IsBusy = false;
                    // wb.Visibility = Visibility.Visible;
                    _IsLastPage = true;
                }


            }
            while (!_IsLastPage);

            #endregion

            if (_IsLastPage && hasValidData)
            {
                this.IsBusy = false;
                //  wb.Visibility = Visibility.Visible;
                Interact("所有数据已反向同步完成");
              //////////////////  App.Current.Shutdown();
            }

        }



        private void ChosenInCurrentPage(WebBrowser wb, bool isOffline, bool IsChoseChecked, ref bool hasValidData)
        {
            mshtml.IHTMLDocument2 doc2 = isOffline ? null : (mshtml.IHTMLDocument2)wb.Document;
            string html = isOffline ? s_htmlFake : doc2.body.innerHTML;


            Debug.WriteLine(html);


            List<ImportInvoiceDTO> listFromPage = new List<ImportInvoiceDTO>();
            List<hParser.Tags.TableRow> validRowList = new List<hParser.Tags.TableRow>();
            //this.parseResult = "";

            #region  使用IHTMLDocument2提取HTML

            //mshtml.HTMLTableClass table = IsChoseChecked ? (mshtml.HTMLTableClass)doc2.all.item("example1", 0) : (mshtml.HTMLTableClass)doc2.all.item("example", 0);
            mshtml.HTMLTableClass table =   (mshtml.HTMLTableClass)doc2.all.item("example", 0);
            if (table == null)
            {
                hasValidData = false;
                //throw new InvalidOperationException("无效table");
                return;
            }
            mshtml.HTMLTableSectionClass tbody = (mshtml.HTMLTableSectionClass)table.lastChild;
            if (tbody == null)
            {
                hasValidData = false;
                //throw new InvalidOperationException("无效tbody");
                return;
            }

            var tbodyHtml = tbody.innerHTML;

            if (0 == string.Compare(tbody.innerText, "没找到记录", StringComparison.InvariantCultureIgnoreCase))
            {
                hasValidData = false;
                //throw new InvalidOperationException("无效tbody");
                return;
            }
            #region WPF WebBroswer交互源代码DOM元素总结

#if RESEARCH
           
            //HTMLDocument doc01 = wb.Document as HTMLDocument;
            ////IHTMLDocument2 doc02 = wb.Document as IHTMLDocument2;
            //Debug.WriteLine(doc01.body.innerHTML);


            ///读/写元素
            ///
            mshtml.IHTMLElement login_pass = (mshtml.IHTMLElement)doc2.all.item("login_pass", 0);
            mshtml.IHTMLElement password = (mshtml.IHTMLElement)doc2.all.item("password", 0);
            password.setAttribute("value", "12345678");
            login_pass.setAttribute("style", "");

            mshtml.IHTMLElement login_pass1 = (mshtml.IHTMLElement)doc2.all.item("login_pass1", 0);
            mshtml.IHTMLElement password1 = (mshtml.IHTMLElement)doc2.all.item("password1", 0);
            login_pass1.setAttribute("style", "display:none;");
            //password1.setAttribute("style", "width:1px");

            //IHTMLElement item = doc01.getElementById("ptmm");
            //item.innerHTML = "<INPUT id=\"pwd\" class=\"login_input password\" type=\"text\" value=\"\" />";

            ////  doc01.body.insertAdjacentHTML(,);
            //Interact(item.innerText);

            //wb.NavigateToString(doc01.body.innerHTML);

            /// Trigger event
            //点击确定按钮
            loginBT.click();


            /// script injection
            ///
            //Basic ds = new Basic();
            //wb.ObjectForScripting = ds;//该对象可由显示在WebBrowser控件中的网页所包含的脚本代码访问  
          
            ///Levarage JS
            ///
            mshtml.IHTMLWindow2 win = (mshtml.IHTMLWindow2)doc2.parentWindow;
            win.execScript("Login('12345678', '', 1)", "javascript");
            return;

#endif


            #endregion

            #endregion
            #region 使用HtmlParser提取tbodyHtml
            Lexer lexer = new Lexer(tbodyHtml);
            hParser.Parser parser = new hParser.Parser(lexer);
            hParser.NodeFilter filter = new NodeClassFilter(typeof(Winista.Text.HtmlParser.Tags.TableRow));
            NodeList nodeList = parser.Parse(filter);
            if (nodeList.Count == 0)
            {
                hasValidData = false;
                Interact("没有符合要求的节点");
            }
            else
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    //抓取一行
                    var tagTR = parserTR(nodeList[i]);

                    #region 充填有效行
                    if (tagTR != null)
                    {
                        validRowList.Add(tagTR);
                    }
                    #endregion

                }

                ParserValidTR_And_ChosenOrNot(validRowList, IsChoseChecked, ref listFromPage);

                #region listFromPage 用 ChosenUnconfirmedList 筛选， ChosenUnconfirmedList 中存在则将本页的该TR的checkBox自动勾上

                var todoList = (from r in listFromPage
                                from i in this.ChosenUnconfirmedList.List
                                where r.InvoiceCode == i.InvoiceCode && r.InvoiceNumber == i.InvoiceNumber
                                && i.IsConfirmed == "0" && !string.IsNullOrEmpty(i.IsChosen)
                                select r).ToList();
                Debug.WriteLine(todoList);

                // mshtml.HTMLTableSectionClass tbody
                if (todoList != null && todoList.Count > 0)
                {
                    CheckInCurrentPage(doc2, todoList);

                }


                #endregion

            }

            #endregion

            #region 日志 & 导出 & 持久化

            if (listFromPage == null || listFromPage.Count == 0)
            {
                Interact("该页面上没有检测到预期数据");
                hasValidData = false;
            }

            ImportInvoiceListDTO soap = new ImportInvoiceListDTO
            {
                List = listFromPage,
                Result = new ImportInvoiceResultDTO
                {
                    Message = "CALLBACK",
                    Status = 9
                }
            };

            Debug.Write(soap);
            #region Log
            if (this.IfLog == "1")
            {
                soap.List.ForEach(impinfo =>
                {
                    if (IsChoseChecked)
                        LogHelper.WriteLog(typeof(ConfirmView), string.Format("发票代码{0} 发票号码{1} 开票日期{2} 销方税号{3} 金额{4} 税额{5} 来源{6} 发票状态{7} 勾选标志{8} 操作时间{9}", impinfo.InvoiceCode, impinfo.InvoiceNumber, impinfo.CreateDate, impinfo.SalesTaxNumber, impinfo.Amount, impinfo.Tax, impinfo.From, impinfo.Status, impinfo.SelectTag, impinfo.OperationTime));
                    else
                        LogHelper.WriteLog(typeof(ConfirmView), string.Format("发票代码{0} 发票号码{1} 开票日期{2} 销方税号{3} 金额{4} 税额{5} 来源{6} 发票状态{7} 确认月份{8}", impinfo.InvoiceCode, impinfo.InvoiceNumber, impinfo.CreateDate, impinfo.SalesTaxNumber, impinfo.Amount, impinfo.Tax, impinfo.From, impinfo.Status, impinfo.SelectTag));

                });
            }
            #endregion
            //if (this.IfCallWS == "1")
            //{
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //    switch (this.SoapMode) {
            //        case "IISATV":
            //            CallWS(soap);
            //            break;
            //        case "IIS":
            //            CallWsEAP(soap);
            //            break;
            //        default:
            //        case "WINSVC":
            //            CallWsEAP_From_WinSrvHost(soap);
            //            break;

            //    }               
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //    /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
            //}
            Debug.Write("本页已反向同步完成，请点击下一页继续同步");
            //FakeBusy();

            #endregion
        }

        /// <summary>
        /// 基于当前页的todoList注入本地脚本实现勾选！
        /// </summary>
        /// <param name="tbody"></param>
        /// <param name="todoList"></param>
        private void CheckInCurrentPage(mshtml.IHTMLDocument2 doc2, List<ImportInvoiceDTO> todoList)
        {
            mshtml.HTMLTableClass table = (mshtml.HTMLTableClass)doc2.all.item("example", 0);
            if (table == null)
            {
                 return;
            }
            mshtml.HTMLTableSectionClass tbody = (mshtml.HTMLTableSectionClass)table.lastChild;
            if (tbody == null)
            {
                return;
            }

            var tbodyHtml = tbody.innerHTML;

            var rows = tbody.rows;
          


            foreach (var todo in todoList)
            {
                //mshtml.IHTMLElement login_header = (mshtml.IHTMLElement)doc2.all.item("login_header", 0);
                // if (login_header != null)
                //{
                //     login_header.setAttribute("style", "display: none;");
                //    // login_header.innerHTML = "";
                //}

                foreach (var row in rows)
                {
                    var tr = (mshtml.HTMLTableRowClass)row;
                    if (tr.innerHTML.Contains(todo.InvoiceCode)&& tr.innerHTML.Contains(todo.InvoiceNumber)){
                        var td1st = (mshtml.HTMLTableCellClass)tr.firstChild;
                        //tr.firstChild.hasChildNodes()
                        if (td1st != null && td1st.hasChildNodes())
                        {
                            // I LIKE  mshtml.IHTMLElementCollection , JUST LIKE doc2.all !!
                            mshtml.IHTMLElementCollection elementCollection = (mshtml.IHTMLElementCollection)td1st.getElementsByTagName("INPUT");//.item("checkbox1", 0);
                            if (elementCollection != null && elementCollection.length > 0)
                            {
                                mshtml.IHTMLElement checkbox = (mshtml.IHTMLElement)elementCollection.item("checkbox1",0);
                                checkbox.setAttribute("checked", "true");
                            }

 
                        }

                    }

                  }


            }


        }


        private bool GotoNextPage(WebBrowser wb, bool isLastPage)
        {
            mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)wb.Document;
            // string html =  doc2.body.innerHTML;
            var currentUrl = wb.Source.AbsoluteUri;
            currentUrl = currentUrl.Replace("#", "");
            if (currentUrl == this.ChosenUri)
            {
                mshtml.IHTMLElement example_next = (mshtml.IHTMLElement)doc2.all.item("example_next", 0);
                example_next.click();

                int pageCounter = 0;
                var pagenumStr = example_next.getAttribute("data-dt-idx");
                if (!int.TryParse(pagenumStr.ToString(), out pageCounter))
                {
                    Interact(string.Format("翻页失败，请手工执行"));
                }


                //边界条件
                var classstr = example_next.getAttribute("class");
                if (classstr == null)
                {
                    Interact(string.Format("example_next miss {0} attr,请手工点击[下一页]", "class"));
                }
                else
                {
                    if (classstr.ToString().Contains("disabled"))
                    {
                        isLastPage = true;
                    }
                }

                // class = paginate_button current 定



            }

            return isLastPage;
        }



        #endregion
  

        public void SetPageURI()
        {

            this.StartUri = this.TargetUri;

        }

        #region SOA

        ImportInvoiceListDTO ChosenUnconfirmedList = new ImportInvoiceListDTO { List=new List<ImportInvoiceDTO> (), Result=new ImportInvoiceResultDTO () };
        private void RPC_GetChosenUnconfirmedList_From_WinSrvHost(ImportInvoiceDTO cond)
        {
            ImportInvoiceListDTO list;
            //ImportInvoiceDTO cond = new ImportInvoiceDTO
            //{
            //    IsChosen = "1",
            //    IsConfirmed = "0",

            //};
            using (WinSvcHostServiceRef.SyncImportInvoiceServiceClient proxy = new WinSvcHostServiceRef.SyncImportInvoiceServiceClient())
            {
                //proxy.FetchListCompleted += Proxy_FetchListCompleted;
                list= proxy.FetchList(cond);
  
            }
            if (list.Result.Status == -1)
            {
                // 修改UI线程
                Interact(list.Result.Message);
                App.Current.Shutdown();
            }
            if (list.Result.Status == 0)
            {
                // 修改UI线程
                this.TipInfo = string.Format("{0},没有已勾选未确认的进项发票", list.Result.Message);
                Interact(this.TipInfo);
                //App.Current.Shutdown();
            }
            this.TipInfo = string.Format("{0},RPC:GetChosen UnconfirmedList From WinSrvHost is Completed", list.Result.Message);
            if (_IsLastPage)
            {
                Interact(this.TipInfo);
            }

           this. ChosenUnconfirmedList= list;

        }

         /// <summary>
        /// FIRE AND FORGOT
        /// </summary>
        /// <param name="soap"></param>
        private void CallWS(ImportInvoiceListDTO soap)
        {
            Task task = new Task(() =>
            {
                using (var factory = new ChannelFactory<Alayaz.SOA.IService.ISyncImportInvoiceService>("*"))
                {
                    var chl = factory.CreateChannel();
                    soap = chl.InjectList(soap);

                    if (soap.Result.Status == 0)
                    {
                          //重试
                          soap = chl.InjectList(soap);
                    }
                }

                if (soap.Result.Status == -1)
                {
                      // 修改UI线程
                      // Interact(soap.Result.Message);
                  }

            }, TaskCreationOptions.LongRunning);
            task.Start();
        }

        private void CallWsEAP_From_WinSrvHost(ImportInvoiceListDTO soap)
        {
            using (WinSvcHostServiceRef.SyncImportInvoiceServiceClient proxy = new WinSvcHostServiceRef.SyncImportInvoiceServiceClient())
            {
                proxy.InjectListCompleted += Proxy_InjectListCompleted;
                proxy.InjectListAsync(soap);


            }
        }
        private void Proxy_InjectListCompleted(object sender, WinSvcHostServiceRef.InjectListCompletedEventArgs e)
        {
            var rs = e.Result;
            //if (soap.Result.Status == 0)
            //{
            //    //重试
            //    soap = proxy.InjectList(soap);
            //}
            if (rs.Result.Status == -1)
            {
                // 修改UI线程
                Interact(rs.Result.Message);
            }
            this.TipInfo = string.Format("{0},所有数据已同步完成", rs.Result.Message);
            if (_IsLastPage)
            {
                Interact("所有同步任务已完成，确认后将关闭软件");
                App.Current.Shutdown();
            }

        }

        private void CallWsEAP(ImportInvoiceListDTO soap)
        {
            using (SyncImportInvoiceServiceClient proxy = new SyncImportInvoiceServiceClient())
            {
                proxy.InjectListCompleted += Proxy_InjectListCompleted;
                proxy.InjectListAsync(soap);


            }

        }

        private void Proxy_InjectListCompleted(object sender, InjectListCompletedEventArgs e)
        {
            var rs = e.Result;
            //if (soap.Result.Status == 0)
            //{
            //    //重试
            //    soap = proxy.InjectList(soap);
            //}
            if (rs.Result.Status == -1)
            {
                // 修改UI线程
                Interact(rs.Result.Message);
            }
            this.TipInfo = string.Format("{0},所有数据已同步完成", rs.Result.Message);
            if (_IsLastPage)
            {
                Interact("所有同步任务已完成，确认后将关闭软件");
                App.Current.Shutdown();
            }

        }

        public void CallSOATest()
        {
            jaxws.users usr = new jaxws.users
            {
                username = "gauge2009",
                password = "8888"

            };

            using (jaxws.SyncImportInvoiceServiceClient client = new jaxws.SyncImportInvoiceServiceClient("SyncImportInvoiceServicePort"))
            {
                var probe = client.Inject(usr);
                Debug.WriteLine(probe);
            }

        }

        #endregion

        #region Start Crawl Util


        //private void FakeBusy()
        //{
        //    this.IsBusy = true;
        //    this.BusyText = "正在加载资源，请稍后......";
        //    this.WebBrowserPtr.Visibility = Visibility.Hidden;
        //    Thread.Sleep(2000);
        //}



        #region parserTR 
        private hParser.Tags.TableRow getTagRow(hParser.INode node)
        {
            if (node == null)
                return null;
            return node is hParser.Tags.TableRow ? node as hParser.Tags.TableRow : null;
        }
        private hParser.Tags.TableRow parserTR(hParser.INode node)
        {
            hParser.Tags.TableRow tagTR = getTagRow(node);
            bool isValid = false;
            //TD在子节点
            //抓取th 
            if (tagTR.Headers != null && tagTR.Headers.Count() > 0)
            {

                for (int i = 0; i < tagTR.Headers.Count(); i++)
                {
                    var header = tagTR.Headers[i] as hParser.Tags.TableHeader; // th

                    if (header.TagName == "TH" && !string.IsNullOrEmpty(header.StringText))
                    {

                        //      parseResult += header.TagName + ":\r\nStringText:" + header.StringText + " ChildrenHTML:" + header.ChildrenHTML
                        //+ " StartPosition:" + header.StartPosition.ToString() + " EndPosition:" + header.EndPosition.ToString() + "\r\n";

                    }

                }
            }
            //抓取td
            if ((tagTR.Headers == null || tagTR.Headers.Count() == 0) && tagTR.ChildrenAsNodeArray != null && tagTR.ChildrenAsNodeArray.Count() > 0
                 )
            {

                for (int i = 0; i < tagTR.ChildrenAsNodeArray.Count(); i++)
                {
                    var colum = tagTR.ChildrenAsNodeArray[i] as hParser.Tags.TableColumn; //td

                    if (colum != null && colum.TagName == "TD" && !string.IsNullOrEmpty(colum.StringText) && colum.StringText != "\n"
                     //&&
                     //(0 == string.Compare(colum.StringText.Trim(), "底账", StringComparison.InvariantCultureIgnoreCase)
                     //|| 0 == string.Compare(colum.StringText.Trim(), "认证", StringComparison.InvariantCultureIgnoreCase)
                     //|| 0 == string.Compare(colum.StringText.Trim(), "全部", StringComparison.InvariantCultureIgnoreCase)
                     //)
                     )
                    {

                        isValid = true;

                    }
                    if (isValid)
                        break;
                }
            }
            return isValid ? tagTR : null;
        }

        /// <summary>
        /// per page
        /// </summary>
        /// <param name="validRowList"></param>
        /// <param name="IsChoseChecked"></param>
        /// <param name="list"></param>
        private void ParserValidTR_And_ChosenOrNot(List<hParser.Tags.TableRow> validRowList, bool IsChoseChecked, ref List<ImportInvoiceDTO> list)
        {
            for (int j = 0; j < validRowList.Count; j++)
            {
                ImportInvoiceDTO dto = new ImportInvoiceDTO();
                dto.TaxCode = this.TaxCode;
               
                hParser.Tags.TableRow tagTR = validRowList[j];
                 
                    //抓取td
                    //fphx查处的都是未确认的
                    for (int i = 0; i < tagTR.Columns.Count(); i++)
                    {
                        var colum = tagTR.Columns[i] as hParser.Tags.TableColumn; //td

                        switch (i)
                        {
                        case 0:
                            var isTrChosen= colum.StringText.Contains("CHECKED")|| colum.StringText.Contains("checked");
                            dto.IsChosen = isTrChosen ? "1" : "0";
                            break;
                        case 1:
                            dto.InvoiceCode = colum.StringText;
                            break;
                        case 2:
                                dto.InvoiceNumber = colum.StringText;
                                break;
                            case 3:
                                dto.CreateDate = colum.StringText;
                                break;
                            case 4:
                                dto.SalesTaxNumber = colum.StringText;
                                break;
                            case 5:
                                dto.Amount = ConvertToDecimal(colum.StringText);
                                break;
                            case 6:
                                dto.Tax = ConvertToDecimal(colum.StringText);
                                break;
                            case 7:
                                dto.From = colum.StringText;
                                break;
                            case 8:
                                dto.Status = colum.StringText;
                                break;
                            case 9:
                                dto.SelectTag = colum.StringText;//勾选状态
                                break;
                            case 10:
                                dto.OperationTime = colum.StringText;//勾选时间
                                break;
                        }


                        //      parseResult += colum.TagName + ":\r\nStringText:" + colum.StringText + " ChildrenHTML:" + colum.ChildrenHTML
                        //+ " StartPosition:" + colum.StartPosition.ToString() + " EndPosition:" + colum.EndPosition.ToString() + "\r\n";
                    }
                    dto.DeductionStatus = "2";
              
                list.Add(dto);
            }
        }

        #endregion 
        private decimal ConvertToDecimal(string str)
        {
            decimal rtn = 0.0M;

            if (string.IsNullOrEmpty(str))
                return 0.0M;

            if (!Decimal.TryParse(str.Trim(), out rtn))
                return 0.0M;

            return rtn;
        }


        #region paserData 递归
        private hParser.ITag getTag(hParser.INode node)
        {
            if (node == null)
                return null;
            return node is hParser.ITag ? node as hParser.ITag : null;
        }

        private void paserData(hParser.INode node)
        {
            hParser.ITag tag = getTag(node);
            if (tag != null && !tag.IsEndTag() && !start.Contains(tag.StartPosition))
            {
                object oId = tag.GetAttribute("ID");
                object oName = tag.GetAttribute("name");
                object oClass = tag.GetAttribute("class");
                parseResult += tag.TagName + ":\r\nID:" + oId + " Name:" + oName
                       + " Class:" + oClass + " StartPosition:" + tag.StartPosition.ToString() + "\r\n";
                start.Add(tag.StartPosition);
            }
            //子节点
            if (node.Children != null && node.Children.Count > 0)
            {
                paserData(node.FirstChild);
            }
            //兄弟节点
            hParser.INode siblingNode = node.NextSibling;
            while (siblingNode != null)
            {
                paserData(siblingNode);
                siblingNode = siblingNode.NextSibling;
            }
        }
        #endregion
        private string HtmlText(string sourceHtml)
        {
            hParser.Parser parser = hParser.Parser.CreateParser(sourceHtml.Replace(System.Environment.NewLine, ""), "utf-8");

            StringBuilder builderHead = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();


            hParser.NodeFilter html = new TagNameFilter("TR");
            hParser.INode nodes = parser.Parse(html)[0];
            builderHead.Append(nodes.Children[0].ToHtml());
            hParser.INode body = nodes.Children[1];
            hParser.INode div = body.Children[0];


            for (int i = 0; i < div.Children.Count; i++)
            {
                if (div.Children[i] is hParser.ITag)
                    builderBody.Append(div.Children[i].ToHtml());
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<html>");
            builder.Append(builderHead.ToString());
            builder.Append("<body>");
            builder.Append(string.Format("<{0}>", div.GetText()));
            builder.Append(builderBody.ToString());
            builder.Append("</div>");
            builder.Append("</body>");
            builder.Append("</html>");
            return builder.ToString();
        }

        #endregion


        #region ProcessBar

        //private void ConfigProgressBar(ProgressBar bar)
        //{
        //    bar.Maximum = 100;
        //    bar.Value = 0;

        //    for (int i = 0; i < 100; i++)
        //    {
        //        System.Action<DependencyProperty, Object> act = (d,v) => bar.SetValue(ProgressBar.ValueProperty, Convert.ToDouble(i + 1));
        //         Dispatcher.CurrentDispatcher.Invoke(act);
        //    }
        //}
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void ConfigProgressBar(ProgressBar pBar, DispatcherPriority dispatcherPriority = DispatcherPriority.Loaded, bool isAsync = false)
        {
            pBar.Visibility = Visibility.Visible;

            pBar.Maximum = 100;
            pBar.Value = 0;

            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pBar.SetValue);

            for (int i = 0; i < 100; i++)
            {
                //System.Threading.Thread.Sleep(1);

                if (isAsync)
                    Dispatcher.CurrentDispatcher.BeginInvoke(updatePbDelegate, dispatcherPriority, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(i + 1) });
                else
                    Dispatcher.CurrentDispatcher.Invoke(updatePbDelegate, dispatcherPriority, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(i + 1) });
            }
        }
        #endregion


        #region regedit
        /* void WINAPI WriteWebBrowserRegKey(LPCTSTR lpKey, DWORD dwValue)
         {
             HKEY hk;
             CString str = "Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
             str += lpKey;
             if (RegCreateKey(HKEY_LOCAL_MACHINE, str, &hk) != 0)
             {
                 MessageBox(NULL, "打开注册表失败!", "Error", 0);
                 ExitProcess(-1);
             }
             if (RegSetValueEx(hk, "你的exe名称.exe", NULL, REG_DWORD, (const byte*)&dwValue,4)!= 0)
     {
                 RegCloseKey(hk);
                 MessageBox(NULL, "写注册表失败!", "Error", 0);
                 ExitProcess(-1);
             }
             RegCloseKey(hk);
         }




          WriteWebBrowserRegKey("FEATURE_BROWSER_EMULATION",9000);
     //    WriteWebBrowserRegKey("FEATURE_ACTIVEX_REPURPOSEDETECTION",1);
         WriteWebBrowserRegKey("FEATURE_BLOCK_LMZ_IMG",1);
         WriteWebBrowserRegKey("FEATURE_BLOCK_LMZ_OBJECT",1);
         WriteWebBrowserRegKey("FEATURE_BLOCK_LMZ_SCRIPT",1);
         WriteWebBrowserRegKey("FEATURE_Cross_Domain_Redirect_Mitigation",1);
         WriteWebBrowserRegKey("FEATURE_ENABLE_SCRIPT_PASTE_URLACTION_IF_PROMPT",1);
         WriteWebBrowserRegKey("FEATURE_LOCALMACHINE_LOCKDOWN",1);
         WriteWebBrowserRegKey("FEATURE_GPU_RENDERING",1);

       */

        #endregion

        #region 设置浏览器静默
        /// <summary>
        /// 设置浏览器静默，不弹错误提示框
        /// </summary>
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>
        /// <param name="silent">是否静默</param>
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

        #endregion


        #region  监听 _._alert 消息
        /// <summary>
        ///  监听 _._alert 消息
        /// 关键异常要写日志  
        /// 网络异常:jQuery11020952770533509179_1463642005738({"key1":"06","key2":"??@@
        /// 打开设备--未插USBKEY(0xA7)
        /// 打开加密设备时验证用户口令--未知错误(0x3810002A)
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        private string CheckHtmlSearchForAlert(mshtml.IHTMLDocument2 doc2)
        {
            string msg = "";
            string html = doc2.body.innerHTML;
            Debug.WriteLine(html);
            mshtml.IHTMLElement popup_message = (mshtml.IHTMLElement)doc2.all.item("popup_message", 0);
            if (popup_message != null)
            {
                msg = popup_message.innerText;
            }
            //网络异常:jQuery11020952770533509179_1463642005738({"key1":"06","key2":"??@@
            //打开设备--未插USBKEY(0xA7)
            //打开加密设备时验证用户口令--未知错误(0x3810002A)
            if (msg.Contains("打开加密设备时验证用户口令")

                )
            {
                msg += " \n确认后助手程序将自动关闭";
                // 写登录失败日志
                LogHelper.WriteLog(typeof(ConfirmView), string.Format("登录失败:{0} | TaxCode:{1}", msg, GlobalData.TaxCode));
                Interact(string.Format("5次登录失败将导致金税盘/税控盘锁死，请检查您的密码配置（DeviceKey）{0}", msg));
                App.Current.Shutdown();
            }
            if (msg.Contains("网络异常")
                 || msg.Contains("会话超时")
                )
            {
                msg += " \n确认后助手程序将自动关闭";
                LogHelper.WriteLog(typeof(ConfirmView), string.Format("Network/Session invalid:{0} | TaxCode:{1}", msg, GlobalData.TaxCode));
                Interact(msg);
                App.Current.Shutdown();
            }
            if (msg.Contains("未插USBKEY")
               )
            {
                msg += ";请确保以证书及驱动已正确安装并以管理员身份执行本程序;\n确认后助手程序将自动关闭";
                LogHelper.WriteLog(typeof(ConfirmView), string.Format("无效的证书、进程权限或设备:{0} | TaxCode:{1}", msg, GlobalData.TaxCode));
                Interact(msg);
                App.Current.Shutdown();
            }
            return msg;


        }

        #endregion
    }

 }
