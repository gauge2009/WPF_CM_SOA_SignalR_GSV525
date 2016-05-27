using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Microsoft.VisualStudio.TestTools.UnitTesting;
 using Alayaz.SOA.Service.ViewModel;
using Alayaz.SOA.Service;
namespace Alayaz.SOA.Service.Tests
{

    [TestClass()]
    public class SyncImportInvoiceServiceTests
    {
        [TestMethod()]
        public void SyncTest()
        {

            ImportInvoiceDTO model = new ImportInvoiceDTO
            {
                InvoiceCode = "4444444444",
                InvoiceNumber = "44444444",
                CreateDate = "2016-05-16",
                SalesTaxNumber = "110101000000000",
                Amount = 1000,
                Tax = 30,
                From = "底帐",
                Status = "正常",
                SelectTag = "未勾选",
                OperationTime = "未操作"
            };
            ImportInvoiceListDTO soap = new ImportInvoiceListDTO();
            soap.List = new List<ImportInvoiceDTO> { model };

            var service = new SyncImportInvoiceService();

            soap = service.InjectList(soap);


            Assert.AreNotEqual(0, soap.Result.Status);
        }


   
        [TestMethod()]
        public void FetchListTest()
        {
            ImportInvoiceDTO condition = new ImportInvoiceDTO
            {
                InvoiceCode = "3600153130",
                InvoiceNumber = "04444798",
                //CreateDate = "2016-05-16",
                //SalesTaxNumber = "110101000000000",
                //Amount = 1000,
                //Tax = 30,
                //From = "底帐",
                //Status = "正常",
                //SelectTag = "未勾选",
                //OperationTime = "未操作"
            };
            
            var service = new SyncImportInvoiceService();

            ImportInvoiceListDTO rs = service.FetchList(condition);

            Assert.IsNotNull( rs.List);
        }
    }
}

 
