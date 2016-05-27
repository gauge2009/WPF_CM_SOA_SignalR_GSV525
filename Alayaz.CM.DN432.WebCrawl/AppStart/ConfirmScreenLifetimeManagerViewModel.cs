using Alayaz.Graph.WPF.Common;
using Alayaz.Graph.WPF.Controls;
using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;

namespace Alayaz.CM.DN432.WebCrawl.ViewModels
{
    //public class ShellViewModel : Screen, IShell
    [Export(typeof(IConfirmShell))]
    public class ConfirmScreenLifetimeManagerViewModel : Conductor<object>, IConfirmShell
    {
        readonly CrawlViewModel screen1;
        readonly LoginViewModel screen2;
        readonly ConfirmViewModel screen3;
        readonly Screen initialialScreen;
        readonly Stack<object> previous = new Stack<object>();
        bool goingBack;
        [ImportingConstructor]
        public ConfirmScreenLifetimeManagerViewModel(CrawlViewModel screen1, LoginViewModel screen2, ConfirmViewModel  screen3)
        {
            this.WindowTitle = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("WindowTitle")) ? this.GetType().FullName : ConfigurationManager.AppSettings.Get("WindowTitle");

            this.initialialScreen = !string.IsNullOrEmpty(GlobalData.PWD) ? (Screen)screen3 : (Screen)screen2;
            
        }
        private double widthVal;
        public double WidthVal
        {
            get { return widthVal; }

            set
            {
                widthVal = value;
                NotifyOfPropertyChange(() => WidthVal);
            }
        }
        private double heightVal;
        public double HeightVal
        {
            get { return heightVal; }

            set
            {
                heightVal = value;
                NotifyOfPropertyChange(() => HeightVal);
            }
        }


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
        public bool CanGoBack
        {
            get { return previous.Count > 0; }
        }

        /// <summary>
        /// “¿¿µ”⁄ StartScreenViewModel
        /// </summary>
        protected override void OnInitialize()
        {
            ActivateItem(initialialScreen);

            // new MainWindow().ShowDialog();
            base.OnInitialize();
        }

        protected override void ChangeActiveItem(object newItem, bool closePrevious)
        {
            if (ActiveItem != null && !goingBack)
                previous.Push(ActiveItem);

            NotifyOfPropertyChange(() => CanGoBack);

            base.ChangeActiveItem(newItem, closePrevious);
        }

        //WindowTitle


        public void GoBack()
        {
            goingBack = true;
            ActivateItem(previous.Pop());
            goingBack = false;
        }




        //public void InitHandler(object source)
        //{
        //    //if (win == null)
        //    //    return;
        //    //if (GlobalData.ImpInvViewModel.BootMode.ToLower() == "confirm")
        //    //{
        //    //    win.Width = 1050;
        //    //    win.Height = 700;
        //    //}
        //    //win.Activated
        //    //win.Initialized
        //    //win.LayoutUpdated

        //    if (string.IsNullOrEmpty(GlobalData.ImpInvViewModel.BootMode) || GlobalData.ImpInvViewModel.BootMode.ToLower() == "crawl")
        //    {
        //        this.WidthVal = 500;
        //        this.HeightVal = 100;
        //    }
        //    else if (GlobalData.ImpInvViewModel.BootMode.ToLower() == "confirm")
        //    {
        //        this.WidthVal = 1050;
        //        this.HeightVal = 700;
        //    }


        //}


    }
}