using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System;
using System.Collections;

namespace Alayaz.CM.DN432.WebCrawl.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {

        public Screen VM
        {
            get;
            set;
        }

        //WindowTitle
        private string windowTitle;

        public event EventHandler<ActivationProcessedEventArgs> ActivationProcessed;

        public string WindowTitle
        {
            get { return windowTitle; }

            set
            {
                windowTitle = value;
                NotifyOfPropertyChange(() => WindowTitle);
            }
        }




        public ShellViewModel()
        {
            //this.VM = new EditorViewModel();
            this.WindowTitle = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("WindowTitle")) ? this.GetType().FullName : ConfigurationManager.AppSettings.Get("WindowTitle");

            //����Э��ʵ�ֵĶ���ͼ
            this.VM = new WebBoxViewModel();

            //����ʵ�ֵĶ���ͼ
            //this.VM = new WebBoxMutilViewModel();


        }

        public void ActivateItem(object item)
        {
            throw new NotImplementedException();
        }

        public void DeactivateItem(object item, bool close)
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}