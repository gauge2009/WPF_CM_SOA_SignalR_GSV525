using System;
using System.ServiceModel;
namespace Alayaz.SOA.IService.WindowsServiceHost
{
    [ServiceContract(Namespace = "http://www.alayaz.com/")]
    public interface IFileReader
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginRead(string fileName, AsyncCallback userCallback,object stateObject);
        string EndRead(IAsyncResult asynResult);

        [OperationContract(AsyncPattern = false)]
        void Write(string fileName, string content);


        [OperationContract(AsyncPattern = false)]
        void JustStart(string appName, string param);





    }
}
