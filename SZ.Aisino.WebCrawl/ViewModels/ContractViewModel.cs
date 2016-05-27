using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using SZ.Aisino.WebCrawl.Coroutines;

namespace SZ.Aisino.WebCrawl.ViewModels {

    [Export("Contract", typeof(ContractViewModel))]
    //[Export(  typeof(ContractViewModel))]

    public class ContractViewModel : BaseViewModel
    {

         private string contractContent;
        public string ContractContent
        {
            get { return contractContent; }

            set
            {
                contractContent = value;
                NotifyOfPropertyChange(() => ContractContent);
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
        public string ContractFilePath { get; set; }

        public ContractViewModel()
        {
            this.ContractFilePath = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ContractFilePath")) ? "contract.txt" : ConfigurationManager.AppSettings.Get("ContractFilePath");
            if (File.Exists(this.ContractFilePath))
            {
                this.ContractContent = File.ReadAllText(this.ContractFilePath);
            }
 
        }

        public IEnumerable<IResult> Agree(Button btn)
        {

          yield  return new ShowScreen("WebBox");
        }
        public IEnumerable<IResult> Refuse(Button btn)
        {
            yield return new ShowScreen(typeof(StartScreenViewModel));

        }

        


    }
}