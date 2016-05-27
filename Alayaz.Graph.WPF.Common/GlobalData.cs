using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;

namespace Alayaz.Graph.WPF.Common
{

    public enum ViewModes
    {
        [Description("普通视图")]
        Normal,

        [Description("紧凑视图")]
        Small
    }

    public class ImpInvViewModel
    {
        /// <summary>
        /// 启动模式
        /// </summary>
        public string BootMode { get; set; }
        public string PWD { get; set; }
        public string TaxCode { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
 
        
    }
    public static class GlobalData
    {

        public static CompositionContainer MefContainer = null;
        public static string PWD = String.Empty;
        public static string TaxCode = String.Empty;
        public static ImpInvViewModel ImpInvViewModel = new ImpInvViewModel();


        //public static Lazy<IEnumerable<LogisticServices>> _logisticServices = new Lazy<IEnumerable<LogisticServices>>(() => {
        //    var biz = GlobalData.GetInstance<ILogisticsService>();
        //    return biz.GetAll();
        //});

        //public static IEnumerable<LogisticServices> LogisticService
        //{
        //    get
        //    {
        //        return _logisticServices.Value;
        //    }
        //}

        public static T GetInstance<T>()
        {
            return MefContainer.GetExportedValue<T>();
        }

        private static GlobalDataHolder instance;
        public static GlobalDataHolder Instance
        {
            get
            {
                if (instance == null)
                    instance = new GlobalDataHolder();
                return instance;
            }
        }




        public class GlobalDataHolder : INotifyPropertyChanged
        {

            internal GlobalDataHolder() { }

            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            private ViewModes viewMode;
            public ViewModes ViewMode
            {
                get
                {
                    return this.viewMode;
                }
                set
                {
                    if (this.viewMode != value)
                    {
                        this.viewMode = value;
                        this.NotifyPropertyChanged("ViewMode");
                    }
                }
            }
        }
    }
}
