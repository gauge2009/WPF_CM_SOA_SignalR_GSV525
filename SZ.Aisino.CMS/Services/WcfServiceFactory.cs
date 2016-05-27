using Microsoft.Practices.Unity;
using Unity.Wcf;
using Microsoft.Practices.Unity.Configuration;

namespace Alayaz.SOA 
{
    public class WcfServiceFactory : UnityServiceHostFactory {
        protected override void ConfigureContainer(IUnityContainer container) {

            container.LoadConfiguration("Service");

            //// register all your components with the container here
            // container
            //    .RegisterType<IService1, Service1>()
            //    .RegisterType<DataContext>(new HierarchicalLifetimeManager());
        }
    }
}