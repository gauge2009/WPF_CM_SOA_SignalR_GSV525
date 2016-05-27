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
using SZ.Aisino.WebCrawl.Views;
using SZ.Aisino.CMS.Service.ViewModel;
using System.ServiceModel;
using SZ.Aisino.CMS.IService;
using System.Threading;

namespace SZ.Aisino.WebCrawl.ViewModels {

    [Export("WebBox", typeof(WebBoxViewModel))]
    public class WebBoxViewModel : Screen
    {


        private static string s_startUri = "http://service.szhtxx.com:15922/Business/Invest";
        private static string s_targetUri = "http://service.szhtxx.com:15922/Business/Invest/Edit/1";
        private static string s_htmlFake = "N/A";
        private static string s_htmlFakeFilePath = "demo.txt";
        private   bool isStartUriOpenned = false;

        public string IsOffline { get; set; }
        public string HtmlFakeFilePath { get; set; }

        IList<int> start = new List<int>();
        public string parseResult { get; set; }
      
        
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
        #endregion

        private WebBrowser WebBrowserPtr = null;


        public WebBoxViewModel()
        {
          
            InitFromConfig();

            Init();

          
            // this.PageURI = "http://service.szhtxx.com:15817/Admin/Notify";
            // this.PageURI = "http://service.szhtxx.com:15922/Business/Invest/Edit/1";
        }
        private void InitFromConfig()
        {
            this.HtmlFakeFilePath = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("HtmlFakeFilePath")) ? s_htmlFakeFilePath : ConfigurationManager.AppSettings.Get("HtmlFakeFilePath");

            this.IsOffline = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("IsOffline")) ? "0" : ConfigurationManager.AppSettings.Get("IsOffline");

            this.StartUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("StartUri")) ? s_startUri : ConfigurationManager.AppSettings.Get("StartUri");


            this.TargetUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TargetUri")) ? s_targetUri : ConfigurationManager.AppSettings.Get("TargetUri");

            this.TipInfo = "欢迎使用进向同步软件";

        }



        private void Init()
        {
           // this.txtPageURI.Text = this.StartUri;
            //this.webBox.Source = new Uri(this.StartUri);
            //this.webBox.LoadCompleted += WebBox_LoadCompleted;

#if DEBUG
            if (File.Exists(this.HtmlFakeFilePath))
            {
                s_htmlFake = File.ReadAllText(this.HtmlFakeFilePath);
            }

#endif
            //this.webBox .SourceUpdated+= WebBox_SourceUpdated;
        }



        #region  Methods for Action
        /// <summary>
        /// 不能在“WebBrowser”类型的“Source”属性上设置“Binding”,Source不是依赖属性
        /// </summary>
        /// <param name="webBrowser"></param>
        public void InitSource(WebBrowser webBrowser)
        {
            this.IsBusy = true;
            this.BusyText = "正在加载资源，请稍后......";
            if (webBrowser == null)
                return;
            this.WebBrowserPtr = webBrowser;
            if (!isStartUriOpenned)
            {
                this.WebBrowserPtr.Source = new Uri(this.StartUri);
                isStartUriOpenned = true;
            }
        }

        public void NavigatingHandler(NavigationEventArgs e, object view, WebBrowser webBrowser)
        {
            if (webBrowser == null)
                return;
            this.WebBrowserPtr.Visibility = Visibility.Hidden;
            var vw = view as WebBoxView;
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

         }

        public void LoadCompletedHandler(NavigationEventArgs e, object view, WebBrowser webBrowser)
        {
            if (webBrowser == null)
                return;
            this.WebBrowserPtr.Visibility = Visibility.Visible;

            var vw = view as WebBoxView;
            if (vw != null)
            {
                ProgressBar pBar = vw.FindName("ProgBar") as ProgressBar;
                if (pBar != null)
                {
                    pBar.Visibility = Visibility.Hidden;

                }

            }

            this.IsBusy = false;
             if ((Uri)e.Uri == new Uri(this.TargetUri))
            {
 
                this.TipInfo = "加载已完成";
                MessageBox.Show("请先查询所需的进向数据");
                //StartCrawl();
                 
            }

           // ShowScreen("", typeof(StartScreenViewModel));
        }

        #endregion
        private void ShowScreen(string name, Type screenType)
        {
            var screen = !string.IsNullOrEmpty(name)
                ? IoC.Get<object>(name)
                : IoC.GetInstance(screenType, null);

            ((IConductor)Parent).ActivateItem(screen);
        }


        public void SetPageURI()
        {

              this.StartUri = this.TargetUri;
 

        }

          
        #region Start Crawl
        public void StartCrawl()//  private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            List<ImportInvoiceDTO> list = new List<ImportInvoiceDTO>();
            List<hParser.Tags.TableRow> validRowList = new List<hParser.Tags.TableRow>();
            this.parseResult = "";
            Uri uri = new Uri(this.TargetUri);
            #region <N>基于Httphelper，这样下载会要求程序自己实现验证授权

            //<N>基于Httphelper，这样下载会要求程序自己实现验证授权
            //HttpHelper httpHelper = new HttpHelper();
            //HttpItem rq = new HttpItem();
            //rq.URL = uri.AbsoluteUri;
            //HttpResult html = httpHelper.GetHtml(rq);
            //Debug.WriteLine(html.Html);

            //直接基于WebBrowser，授权是由用户手动实现的
            mshtml.IHTMLDocument2 doc2 = null;//(mshtml.IHTMLDocument2)webBox.Document;
            string html = string.Compare(this.IsOffline, "1", StringComparison.InvariantCultureIgnoreCase) == 0 ? s_htmlFake : doc2.body.innerHTML;
            Debug.WriteLine(html);
            #endregion

            #region 使用HtmlParser提取HTML
            Lexer lexer = new Lexer(html);
            hParser.Parser parser = new hParser.Parser(lexer);
            hParser.NodeFilter filter = new NodeClassFilter(typeof(Winista.Text.HtmlParser.Tags.TableRow));
            NodeList nodeList = parser.Parse(filter);
            if (nodeList.Count == 0)
                MessageBox.Show("没有符合要求的节点");
            else
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    //抓取一行
                    var tagTR = parserTR(nodeList[i]);

                    #region 充填有效行
                    if (tagTR != null)
                        validRowList.Add(tagTR);
                    #endregion

                }

                parserValidTR(validRowList, ref list);
#if DEBUG
               // MessageBox.Show(parseResult);
#endif
            }
            /*  parseResult = HtmlText(html);
             MessageBox.Show(parseResult);*/
            #endregion

            #region 同步

            if (list == null || list.Count == 0)
            {
                MessageBox.Show("该页面上没有检测到预期数据");
                return;
            }

            ImportInvoiceListDTO soap = new ImportInvoiceListDTO
            {
                List = list,
                Result = new ImportInvoiceResultDTO
                {
                    Message = "CALLBACK",
                    Status = 9
                }
            };
            //using (var factory = new ChannelFactory<ISyncImportInvoiceService>("*"))
            //{
            //    var chl = factory.CreateChannel();
            //    soap = chl.PullImportInvoices(soap);

            //    if (soap.Result.Status == 0)
            //    {
            //        //重试
            //        soap = chl.PullImportInvoices(soap);
            //    }
            //}

            //if (soap.Result.Status == -1)
            //{
            //    // 修改UI线程
            //      MessageBox.Show(soap.Result.Message);
            //}
            CallWS(soap);
            MessageBox.Show("本页已同步完成，请点击下一页继续同步");
            //FakeBusy();

            #endregion

        }

        private void FakeBusy()
        {
            this.IsBusy = true;
            this.BusyText = "正在加载资源，请稍后......";
            this.WebBrowserPtr.Visibility = Visibility.Hidden;
            Thread.Sleep(2000);
         }

        private   void CallWS(ImportInvoiceListDTO soap)
        {
            Task task = new Task(() =>
            {
                using (var factory = new ChannelFactory<ISyncImportInvoiceService>("*"))
                {
                    var chl = factory.CreateChannel();
                    soap = chl.PullImportInvoices(soap);

                    if (soap.Result.Status == 0)
                    {
                        //重试
                        soap = chl.PullImportInvoices(soap);
                    }
                }

                if (soap.Result.Status == -1)
                {
                    // 修改UI线程
                    // MessageBox.Show(soap.Result.Message);
                }

            }, TaskCreationOptions.LongRunning);
            task.Start();
         }
       

        #region parserTR 
        private hParser.Tags.TableRow getTagRow(hParser.INode node)
        {
            if (node == null)
                return null;
            return node is hParser.Tags.TableRow ? node as hParser.Tags.TableRow : null;
        }
        private hParser.Tags.TableRow parserTR(hParser.INode node )
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

                    if (colum != null && colum.TagName == "TD" && !string.IsNullOrEmpty(colum.StringText) && colum.StringText != "\n" &&
                        (0 == string.Compare(colum.StringText.Trim(), "底账", StringComparison.InvariantCultureIgnoreCase) 
                        ||0==string.Compare( colum.StringText.Trim(), "认证",StringComparison.InvariantCultureIgnoreCase)  
                        ||0==string.Compare( colum.StringText.Trim(), "全部",StringComparison.InvariantCultureIgnoreCase)   
                        ))
                    {
                 
                        isValid = true;

                    }
                    if (isValid)
                        break;
                }
            }
            return isValid ? tagTR : null;
        }


        private void parserValidTR(List<hParser.Tags.TableRow> validRowList, ref List<ImportInvoiceDTO> list)
        {
            for (int j = 0; j < validRowList.Count; j++)
            {
                ImportInvoiceDTO dto = new ImportInvoiceDTO();
                hParser.Tags.TableRow tagTR = validRowList[j];

                //抓取td
                for (int i = 0; i < tagTR.Columns.Count(); i++)
                {
                    var colum = tagTR.Columns[i] as hParser.Tags.TableColumn; //td

                    switch (i)
                    {
                        case 0:
                            dto.InvoiceCode = colum.StringText;
                            break;
                        case 1:
                            dto.InvoiceNumber = colum.StringText;
                            break;
                        case 2:
                            dto.CreateDate = colum.StringText;
                            break;
                        case 3:
                            dto.SalesTaxNumber = colum.StringText;
                            break;
                        case 4:
                            dto.Amount = ConvertToDecimal(colum.StringText);
                            break;
                        case 5:
                            dto.Tax = ConvertToDecimal(colum.StringText);
                            break;
                        case 6:
                            dto.From = colum.StringText;
                            break;
                        case 7:
                            dto.Status = colum.StringText;
                            break;
                        case 8:
                            dto.SelectTag = colum.StringText;
                            break;
                        case 9:
                            dto.OperationTime = colum.StringText;
                            break;
                    }


                    parseResult += colum.TagName + ":\r\nStringText:" + colum.StringText + " ChildrenHTML:" + colum.ChildrenHTML
              + " StartPosition:" + colum.StartPosition.ToString() + " EndPosition:" + colum.EndPosition.ToString() + "\r\n";
                }
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
            hParser.Parser parser = hParser. Parser.CreateParser(sourceHtml.Replace(System.Environment.NewLine, ""), "utf-8");

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
        private void ConfigProgressBar(ProgressBar bar)
        {
            bar.Maximum = 100;
            bar.Value = 0;

            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(bar.SetValue);

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(10);
                Dispatcher.CurrentDispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(i + 1) });
            }
        }
        #endregion
    }
}
