using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using SZ.Aisino.WebCrawl.ViewModels;
using System.Windows.Controls;
using AsNum.Common;
using AsNum.Xmj.Common;

namespace SZ.Aisino.WebCrawl
{

    /// <summary>
    ///   MEF ��
    /// </summary>
    public class CoroutinesBootstrapper : BootstrapperBase {
        //private CompositionContainer container;

        public CoroutinesBootstrapper()
        {
            Initialize();
        }
        protected override void Configure()
        {
            var catalog = MefHelper.SafeDirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);

            AssemblySource.Instance.Select(x => new AssemblyCatalog(x))
                .OfType<ComposablePartCatalog>()
                .ToList().ForEach((c) => {
                    catalog.Catalogs.Add(c);
                });

            GlobalData.MefContainer = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);//// container = CompositionHost.Initialize(catalog);

            var batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            //batch.AddExportedValue<IWindowManager>(new ScreenLifetimeManagerViewModel());
            //batch.AddExportedValue<IEventAggregator>(new WebBoxViewModel());
            batch.AddExportedValue(GlobalData.MefContainer);
            ////batch.AddExportedValue(container);
            ////batch.AddExportedValue(catalog);

            //this.Container.ComposeParts(this);
            GlobalData.MefContainer.Compose(batch);//// container.Compose(batch);


            //EFLogger.EFLogger.Init();


            Coroutine.Completed += (s, e) => {
                if (e.Error != null)
                    MessageBox.Show(e.Error.Message);
            };
        }

     

        protected override object GetInstance(Type service, string key) {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = GlobalData.MefContainer.GetExportedValues<Object>(contract);
            if (exports.Count() > 0)
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
            //return null;
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return GlobalData.MefContainer.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
        }

        protected override void BuildUp(object instance) {
            GlobalData.MefContainer.SatisfyImportsOnce(instance);
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