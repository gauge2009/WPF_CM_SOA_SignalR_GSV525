using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Alayaz.SOA.IService.ConsoleHost
{
    [ServiceContract(Namespace = "http://www.alayaz.com/")]

    public interface IFileReader
    {
        

        [OperationContract(AsyncPattern = false)]
         void JustStart(string appName, string param);





    }
}
