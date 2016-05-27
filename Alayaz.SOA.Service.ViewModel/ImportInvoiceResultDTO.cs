using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
 
namespace Alayaz.SOA.Service.ViewModel
{
    [DataContract, Serializable]
    public class ImportInvoiceResultDTO
    {
        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime Begin { get; set; }

        [DataMember]
        public DateTime End { get; set; }



    }
}
