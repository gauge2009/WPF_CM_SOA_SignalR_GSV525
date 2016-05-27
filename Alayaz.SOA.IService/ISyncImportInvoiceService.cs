using Alayaz.SOA.Service.ViewModel;
using System.ServiceModel;

namespace Alayaz.SOA.IService
{

    [ServiceContract]
    public interface ISyncImportInvoiceService
    {
        #region Inject

        [OperationContract]
        // ImportInvoiceDTO PullImportInvoices(ImportInvoiceDTO soap);
        ImportInvoiceListDTO InjectList(ImportInvoiceListDTO soap); 
        #endregion

        #region Fetch
        [OperationContract]
        ImportInvoiceListDTO FetchList(ImportInvoiceDTO soap);
       // ImportInvoiceListDTO FetchList(Condition soap); 
        #endregion
    }
}
