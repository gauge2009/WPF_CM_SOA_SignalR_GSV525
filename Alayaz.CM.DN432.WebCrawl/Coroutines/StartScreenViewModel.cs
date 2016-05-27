using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading;
using System.Windows.Controls;
using Alayaz.CM.DN432.WebCrawl.Coroutines;

namespace Alayaz.CM.DN432.WebCrawl.ViewModels {

    //[Export("StartScreen", typeof(StartScreenViewModel))]
    [Export(  typeof(StartScreenViewModel))]

    public class StartScreenViewModel : Screen
    {
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

        public StartScreenViewModel()
        {

            this.ImgPath = "pack://application:,,,/Alayaz.CM.DN432.WebCrawl;Component/imgs/fpdk_szgs_step1.png";
        }

        public   void Next(Button btn)
        {
            if (this.ImgPath.Contains("step1")  )
            {
                this.ImgPath = this.ImgPath.Replace("step1", "step2");
            }
            else if (this.ImgPath.Contains("step2"))
            {
                this.ImgPath = this.ImgPath.Replace("step2", "step3");
            }
            else if (this.ImgPath.Contains("step3"))
            {
                this.ImgPath = this.ImgPath.Replace("step3", "step4");
                if (btn == null)
                    return;

                btn.Visibility = System.Windows.Visibility.Hidden;
            }
           

        }
        public IEnumerable<IResult> GoForward()
        {
            //this.IfShowContract = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("IfShowContract")) ? "1" : ConfigurationManager.AppSettings.Get("IfShowContract");
            //if (0 == string.Compare(this.IfShowContract, "1", System.StringComparison.InvariantCultureIgnoreCase))
            //{
            //    // ShowScreen("", typeof(StartScreenViewModel));
            //    yield return new ShowScreen("Contract");
            //}
            //else
            //{
                yield return Loader.Show("ÕýÔÚ¼ÓÔØ..");

                //yield return new LoadCatalog("Caliburn.Micro.Coroutines.External.xap");
                yield return new ShowScreen("WebBox");

                //yield return Loader.Hide();

                // yield return new ShowScreen("ExternalScreen");
            //}
        }

    

    }
}