namespace SZ.Aisino.WebCrawl
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using SZ.Aisino.WebCrawl.ViewModels;
    using System.Windows.Controls;
    using System.Windows;
    public class AppBootstrapper : BootstrapperBase {

        private SimpleContainer container;

        public AppBootstrapper() {
            this.StartRuntime();
        }

        protected override void Configure() {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.PerRequest<IShell, ShellViewModel>();

            // ConventionManager.AddElementConvention<WebBrowser>(WebBrowser.ValueProperty, "Value", "ValueChanged");
            // ConventionManager.AddElementConvention<WebBrowser>(WebBrowser.DataContextProperty, "DataContext", "DataContextChanged");
         }

        protected override object GetInstance(Type service, string key) {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }


        /// <summary>
        /// ������������� View �� ViewMode �ŵ� DLL��ȥ����ʾ�Ҳ��� View ������ô�죿  ���õ��� MEF���ҵĽ���취���� Bootstrapper ����д��
        /// </summary>
        //protected override void StartRuntime()
        //{
        //    base.StartRuntime();

        //    //�� Assembly.Instance.Add �������ڽ�����ز�ͬDLL�ڵ�View
        //    var dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories);
        //    foreach (var dll in dllFiles)
        //    {
        //        try
        //        {
        //            var asm = Assembly.LoadFrom(dll);
        //            if (asm.GetTypes().Any(t =>
        //                t.GetInterfaces().Contains(typeof(IViewAware))
        //                || t.GetInterfaces().Contains(typeof(IScreen))
        //                ))
        //            {
        //                AssemblySource.Instance.Add(asm);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}

    }
}