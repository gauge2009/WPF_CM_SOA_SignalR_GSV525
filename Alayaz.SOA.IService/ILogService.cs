using System.ServiceModel;

namespace Alayaz.SOA.IService
{

    [ServiceContract]
    public interface ILogService
    {

        [OperationContract]
        void LogInfo(string msg);

      

    }
}
