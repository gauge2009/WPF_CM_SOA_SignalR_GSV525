using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
  using System.ComponentModel.DataAnnotations;
namespace Alayaz.SOA.Service.ViewModel
{
   

    /* 
       [DataContract]
      public class SmartCondition : BaseQuery<VIMS_BIZ_INVOICE>
    {
        [DataMember, MapTo("ID", Opt = MapToOpts.Equal, IgnoreCase = true)]
        public int? ID { get; set; }

        [MapTo("FPDM", Opt = MapToOpts.Include, IgnoreCase = true)]
        [DataMember, Required]
        public string InvoiceCode
        {
            get;
            set;
        }
        [MapTo("FPHM", Opt = MapToOpts.Include, IgnoreCase = true)]
        [DataMember]
        public string InvoiceNumber
        {
            get;
            set;
        }
        [MapTo("KPRQ", Opt = MapToOpts.LtOrEqual )]
        [DataMember]
        public DateTime BeginDate
        {
            get;
            set;
        }
        [MapTo("KPRQ", Opt = MapToOpts.GtOrEqual)]
        [DataMember]
        public DateTime EndDate
        {
            get;
            set;
        }
        [MapTo("XFSH", Opt = MapToOpts.Equal, IgnoreCase = true)]
        [DataMember]
        public string SalesTaxNumber
        {
            get;
            set;
        }
 
        /// <summary>
        /// 认证状态 （页面：确认标志  ~ 未确认/已确认）
        /// </summary>
        [DataMember, Required]
        public string CertificateStatus
        {
            get;
            set;
        }

        /// <summary>
        /// DKZT （抵扣状态（1：已抵扣；2：未抵扣；3：已过期；）
        /// </summary>
        [DataMember, Required]
        public string DKStatus
        {
            get;
            set;
       
    } }*/
    [DataContract]
    public class Condition
    {
        [DataMember]
        public int? ID { get; set; }

        [DataMember, Required]
        public string InvoiceCode
        {
            get;
            set;
        }
        [DataMember, Required]
        public string InvoiceNumber
        {
            get;
            set;
        }
        [DataMember]
        public DateTime BeginDate
        {
            get;
            set;
        }
        [DataMember]
        public DateTime EndDate
        {
            get;
            set;
        }
        [DataMember]
        public string SalesTaxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 认证状态 （页面：确认标志  ~ 未确认/已确认）
        /// </summary>
        [DataMember, Required]
        public string CertificateStatus
        {
            get;
            set;
        }

        /// <summary>
        /// DKZT （抵扣状态（1：已抵扣；2：未抵扣；3：已过期；）
        /// </summary>
        [DataMember, Required]
        public string DKStatus
        {
            get;
            set;
        }
    }
}
