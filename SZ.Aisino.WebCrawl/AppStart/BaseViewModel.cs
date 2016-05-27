using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;

namespace SZ.Aisino.WebCrawl.ViewModels
{
    [Export(typeof(BaseViewModel))]
    public class BaseViewModel : Screen
    {
       
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

        private void ShowScreen(string name, Type screenType)
        {
            var screen = !string.IsNullOrEmpty(name)
                ? IoC.Get<object>(name)
                : IoC.GetInstance(screenType, null);

            ((IConductor)Parent).ActivateItem(screen);
        }


    }
}