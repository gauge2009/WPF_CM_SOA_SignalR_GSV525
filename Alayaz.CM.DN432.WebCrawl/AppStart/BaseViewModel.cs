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

namespace Alayaz.CM.DN432.WebCrawl.ViewModels
{
    [Export(typeof(BaseViewModel))]
    public class BaseViewModel : Screen
    {
        #region  Traditional Prop
        internal log4net.ILog Log { get; set; }

        public string ShowWDforDebug { get; set; }

        /// <summary>
        ///  MESSAGEBOX / LOG  /  PERSIST 
        /// </summary>
        internal string InteractMode { get; set; }

        #endregion
        #region  Prop with NotifyOfPropertyChange
        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }

            set
            {
                windowTitle = value;
                NotifyOfPropertyChange(() => WindowTitle);
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
        private Visibility canVisal;
        public Visibility CanVisal
        {
            get { return canVisal; }

            set
            {
                canVisal = value;
                NotifyOfPropertyChange(() => CanVisal);
            }
        }
        #endregion

        public   BaseViewModel()
        {
            this.Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            // this.Log.Info(msg);

            InitFromConfig();
#if DEBUG

            if ("1" == this.ShowWDforDebug)
            {
                this.CanVisal = Visibility.Visible;
            }
            else
            {
                this.CanVisal = Visibility.Hidden;
            }
#else
             this.CanVisal = Visibility.Hidden;
#endif

           // Init();

 


        }
        private void InitFromConfig()
        {
           
             this.InteractMode = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("InteractMode")) ? "MESSAGEBOX" : ConfigurationManager.AppSettings.Get("InteractMode");

            this.ShowWDforDebug = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ShowWDforDebug")) ? "0" : ConfigurationManager.AppSettings.Get("ShowWDforDebug");

  

            this.TipInfo = "欢迎使用";
 

        }

        #region Common



        internal void Interact(string msg, bool ifLogThreadID)
        {
            if (ifLogThreadID)
                Interact(string.Format("Action={0} & {1} on Task{2}", msg, MethodInfo.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId.ToString()));
            else
                Interact(msg);
        }

        /// <summary>
        ///   UI交互模式   MESSAGEBOX / LOG  /  PERSIST   
        /// </summary>
        /// <param name="msg"></param>
        internal void Interact(string msg)
        {
            //this.InteractMode = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("InteractMode")) ? "MESSAGEBOX" : ConfigurationManager.AppSettings.Get("InteractMode");

            this.TipInfo = msg;

            switch (this.InteractMode)
            {
                case "PERSIST":
                    // SOAP +  DB
                    break;
                case "LOG":
                    this.Log.Info(msg);
                    break;
                default:
                case "MESSAGEBOX":
                    MessageBox.Show(msg);
                    break;


            }

        }

        public void ShowScreen(string name, Type screenType)
        {
            var screen = !string.IsNullOrEmpty(name)
                ? IoC.Get<object>(name)
                : IoC.GetInstance(screenType, null);

            ((IConductor)Parent).ActivateItem(screen);
        }



        #endregion

        #region Action
        public void SourceUpdatedHandler(NavigationEventArgs e, object view, WebBrowser wb)
        {
            Interact("Source Updated !");
        }

        #endregion

        #region MyRegion

        #endregion

        #region MyRegion

        #endregion

    }
    public enum CheckMode
    {
        CheckIsDataValidWhenLoginHtmlPartialUpdate,
        CheckIsDataValidWhenFpcxHtmlPartialUpdate,
        CheckIsDataValidWhenFphxHtmlPartialUpdate,


    }
}