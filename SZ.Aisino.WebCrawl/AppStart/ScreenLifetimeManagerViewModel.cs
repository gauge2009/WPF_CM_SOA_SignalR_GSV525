using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
 
namespace SZ.Aisino.WebCrawl.ViewModels
{
    //public class ShellViewModel : Screen, IShell
    [Export(typeof(IShell))]
    public class ScreenLifetimeManagerViewModel : Conductor<object>, IShell
     {
        readonly StartScreenViewModel initialialScreen;
        readonly Stack<object> previous = new Stack<object>();
        bool goingBack;
        [ImportingConstructor]
        public ScreenLifetimeManagerViewModel(StartScreenViewModel initialialScreen)
        {
            this.WindowTitle = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("WindowTitle")) ? this.GetType().FullName : ConfigurationManager.AppSettings.Get("WindowTitle");
            this.initialialScreen = initialialScreen;

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




    }
}