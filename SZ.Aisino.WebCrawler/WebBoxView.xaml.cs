using mshtml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SZ.Aisino.WebCrawler.Common;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;
using hParser = Winista.Text.HtmlParser.Tags;

namespace SZ.Aisino.WebCrawler
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WebBoxView : UserControl
    {
        private static string s_startUri = "http://service.szhtxx.com:15922/Business/Invest";
        private static string s_targetUri = "http://service.szhtxx.com:15922/Business/Invest/Edit/1";
        private static string s_htmlFake = "N/A";
        private static string s_htmlFakeFilePath = "demo.txt";



        IList<int> start = new List<int>();
        public string parseResult { get; set; }
        public string StartUri { get; set; }
        public string TargetUri { get; set; }
        public string IsOffline { get; set; }
        public string HtmlFakeFilePath { get; set; }
        public WebBoxView()
        {

            this.HtmlFakeFilePath = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("HtmlFakeFilePath")) ? s_htmlFakeFilePath : ConfigurationManager.AppSettings.Get("HtmlFakeFilePath");

            this.IsOffline = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("IsOffline")) ? "0" : ConfigurationManager.AppSettings.Get("IsOffline");

            this. StartUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("StartUri")) ? s_startUri : ConfigurationManager.AppSettings.Get("StartUri");


           this. TargetUri = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("TargetUri")) ? s_targetUri : ConfigurationManager.AppSettings.Get("TargetUri");

            InitializeComponent();

            Init();
        }

        private void Init()
        {
            this.txtPageURI.Text = this.StartUri;
            this.webBox.Source = new Uri(this.StartUri);
            this.webBox.LoadCompleted += WebBox_LoadCompleted;
            this.webBox.Navigated += WebBox_Navigated;
          
            this.btnDownload.Click += BtnDownload_Click;

#if DEBUG
            if (File.Exists(this.HtmlFakeFilePath)  )
            {
                s_htmlFake = File.ReadAllText(this.HtmlFakeFilePath);
            }
            
#endif
            //this.webBox .SourceUpdated+= WebBox_SourceUpdated;
        }

       /* private void WebBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            
        }*/

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            this.parseResult = "";
            Uri uri = this.webBox.Source;
            #region 

            //<N>基于Httphelper，这样下载会要求程序自己实现验证授权
            //HttpHelper httpHelper = new HttpHelper();
            //HttpItem rq = new HttpItem();
            //rq.URL = uri.AbsoluteUri;
            //HttpResult html = httpHelper.GetHtml(rq);
            //Debug.WriteLine(html.Html);

            //直接基于WebBrowser，授权是由用户手动实现的
            mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)webBox.Document;
           string html = string.Compare( this.IsOffline, "1", StringComparison.InvariantCultureIgnoreCase)==0? s_htmlFake: doc2.body.innerHTML;
            Debug.WriteLine(html);
            #endregion

            #region 使用HtmlParser提取HTML 
            Lexer lexer = new Lexer(html);
             Parser parser = new Parser(lexer);
             NodeFilter filter = new NodeClassFilter(typeof(Winista.Text.HtmlParser.Tags.TableRow));
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
        private hParser.TableRow getTagRow(INode node)
        {
            if (node == null)
                return null;
            return node is hParser.TableRow ? node as hParser.TableRow : null;
        }
        private void parserTR(INode node)
        {
            hParser.TableRow tagTR = getTagRow(node);

            //TD在子节点
            if (tagTR.Headers != null && tagTR.Headers.Count() > 0)
            {

                for (int i = 0; i < tagTR.Headers.Count(); i++)
                {
                    var header = tagTR.Headers[i] as hParser.TableHeader; // th

                    if (header.TagName == "TH"&&  !string.IsNullOrEmpty(header.StringText))
                    {
                        parseResult += header.TagName + ":\r\nStringText:" + header.StringText + " ChildrenHTML:" + header.ChildrenHTML
                  + " StartPosition:" + header.StartPosition.ToString() + " EndPosition:" + header.EndPosition.ToString() + "\r\n";

                    }

                }
            }
            if ((tagTR.Headers == null|| tagTR.Headers.Count()==0) && tagTR.ChildrenAsNodeArray!=null&& tagTR.ChildrenAsNodeArray.Count() > 0)
            {

                for (int i = 0; i < tagTR.ChildrenAsNodeArray.Count(); i++)
                {
                    var colum = tagTR.ChildrenAsNodeArray[i]  as hParser.TableColumn; //td

                    if (colum!=null&& colum.TagName == "TD" && !string.IsNullOrEmpty(colum.StringText)&& colum.StringText!="\n")
                    {
                        parseResult += colum.TagName + ":\r\nStringText:" + colum.StringText + " ChildrenHTML:" + colum.ChildrenHTML
                  + " StartPosition:" + colum.StartPosition.ToString() + " EndPosition:" + colum.EndPosition.ToString() + "\r\n";

                    }

                }
            }

        } 
        #endregion
        #region paserData 递归
        private ITag getTag(INode node)
        {
            if (node == null)
                return null;
            return node is ITag ? node as ITag : null;
        }

        private void paserData(INode node)
        {
            ITag tag = getTag(node);
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
            INode siblingNode = node.NextSibling;
            while (siblingNode != null)
            {
                paserData(siblingNode);
                siblingNode = siblingNode.NextSibling;
            }
        } 
        #endregion
        private string HtmlText(string sourceHtml)
        {
            Parser parser = Parser.CreateParser(sourceHtml.Replace(System.Environment.NewLine, ""), "utf-8");

            StringBuilder builderHead = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();


            NodeFilter html = new TagNameFilter("TR");
            INode nodes = parser.Parse(html)[0];
            builderHead.Append(nodes.Children[0].ToHtml());
            INode body = nodes.Children[1];
            INode div = body.Children[0];


            for (int i = 0; i < div.Children.Count; i++)
            {
                if (div.Children[i] is ITag)
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



        private void WebBox_Navigated(object sender, NavigationEventArgs e)
        {
            this.txtPageURI.Text = e.Uri.AbsoluteUri;
            //if (  e.Uri == new Uri(targetUri))
            //{
            //    //MessageBox.Show("请先查询所需的进向数据");

            //    //mshtml.HTMLDocumentClass htmldocument = ( HTMLDocumentClass)webBox.Document;

            //    //mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)webBox.Document;
            //    //mshtml.IHTMLElement btnCancel = (mshtml.IHTMLElement)doc.all.item("close", 0);
            //    //string btnCancelClass = (string)btnCancel.getAttribute("class");
            //    //MessageBox.Show(btnCancelClass);

            //    //var source =  webBox.Document.ToString();


            //}
        }

        private void WebBox_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if ( (Uri)e.Uri == new Uri(this.TargetUri))
            {
               

                //mshtml.HTMLDocumentClass htmldocument = ( HTMLDocumentClass)webBox.Document;

                //mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)webBox.Document;
                //mshtml.IHTMLElement btnCancel = (mshtml.IHTMLElement)doc.all.item("close", 0);
                //string btnCancelClass = (string)btnCancel.getAttribute("class");
                //MessageBox.Show(btnCancelClass);

                //var source =  webBox.Document.ToString();

                LoadCompletedHandler();
            }
           
        }
        public void LoadCompletedHandler()
        {

            this.txtTips.Text = "加载已完成";
            MessageBox.Show("请先查询所需的进向数据");
            StartCrawl();

        }

       

      

        private void StartCrawl()
        {




        }
      
       



    }
}
