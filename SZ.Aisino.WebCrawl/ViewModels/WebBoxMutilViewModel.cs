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

namespace SZ.Aisino.WebCrawl.ViewModels {
    public class WebBoxMutilViewModel : Screen {


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




        public WebBoxMutilViewModel()
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
            this.BusyText = "正在加载页面...";
            if (webBrowser == null)
                return;
            if (!isStartUriOpenned)
            {
                webBrowser.Source = new Uri(this.StartUri);
                isStartUriOpenned = true;
            }
        }

        public void LoadCompletedHandler(NavigationEventArgs e)
        {
            this.IsBusy = false;
             if ((Uri)e.Uri == new Uri(this.TargetUri))
            {
 
                this.TipInfo = "加载已完成";
                MessageBox.Show("请先查询所需的进向数据");
                //StartCrawl();
            }
            

        }
        #endregion

        public void SetPageURI()
        {

              this.StartUri = this.TargetUri;
 

        }


        #region Start Crawl
        public void StartCrawl()//  private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
 
            this.parseResult = "";
            Uri uri = new Uri(this.TargetUri);
            #region 

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
                    parserTR(nodeList[i]);
                }
                MessageBox.Show(parseResult);
            }
            /*  parseResult = HtmlText(html);
             MessageBox.Show(parseResult);*/
            #endregion
        }

        #region parserTR 
        private hParser.Tags.TableRow getTagRow(hParser.INode node)
        {
            if (node == null)
                return null;
            return node is hParser.Tags.TableRow ? node as hParser.Tags.TableRow : null;
        }
        private void parserTR(hParser.INode node)
        {
            hParser.Tags.TableRow tagTR = getTagRow(node);

            //TD在子节点
            if (tagTR.Headers != null && tagTR.Headers.Count() > 0)
            {

                for (int i = 0; i < tagTR.Headers.Count(); i++)
                {
                    var header = tagTR.Headers[i] as hParser.Tags.TableHeader; // th

                    if (header.TagName == "TH" && !string.IsNullOrEmpty(header.StringText))
                    {
                        parseResult += header.TagName + ":\r\nStringText:" + header.StringText + " ChildrenHTML:" + header.ChildrenHTML
                  + " StartPosition:" + header.StartPosition.ToString() + " EndPosition:" + header.EndPosition.ToString() + "\r\n";

                    }

                }
            }
            if ((tagTR.Headers == null || tagTR.Headers.Count() == 0) && tagTR.ChildrenAsNodeArray != null && tagTR.ChildrenAsNodeArray.Count() > 0)
            {

                for (int i = 0; i < tagTR.ChildrenAsNodeArray.Count(); i++)
                {
                    var colum = tagTR.ChildrenAsNodeArray[i] as hParser.Tags.TableColumn; //td

                    if (colum != null && colum.TagName == "TD" && !string.IsNullOrEmpty(colum.StringText) && colum.StringText != "\n")
                    {
                        parseResult += colum.TagName + ":\r\nStringText:" + colum.StringText + " ChildrenHTML:" + colum.ChildrenHTML
                  + " StartPosition:" + colum.StartPosition.ToString() + " EndPosition:" + colum.EndPosition.ToString() + "\r\n";

                    }

                }
            }

        }
        #endregion 
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
       



    }
}
