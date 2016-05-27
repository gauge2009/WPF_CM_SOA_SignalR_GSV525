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
        /// 插件化开发，将 View 和 ViewMode 放到 DLL里去，提示找不到 View ，该怎么办？  我用的是 MEF，我的解决办法是在 Bootstrapper 里重写：
        /// </summary>
        //protected override void StartRuntime()
        //{
        //    base.StartRuntime();

        //    //用 Assembly.Instance.Add 可以用于解决加载不同DLL内的View
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