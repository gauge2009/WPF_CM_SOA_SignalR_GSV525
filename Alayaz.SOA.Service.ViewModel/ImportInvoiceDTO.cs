using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Alayaz.SOA.Service.ViewModel
{

    [DataContract, Serializable]
    public class ImportInvoiceListDTO
    {
        [DataMember]
        public List<ImportInvoiceDTO> List
        {
            get;
            set;
        }


        [DataMember]
        public ImportInvoiceResultDTO Result
        {
            get;
            set;
        }
    }



    [DataContract, Serializable]
    public class ImportInvoiceDTO  //: BaseCondition
    {
        /// <summary>
        /// 发票代码
        /// </summary>
        [DataMember]
        public string InvoiceCode
        {
            get;
            set;
        }
        /// <summary>
        /// 发票号码
        /// </summary>
        [DataMember]
        public string InvoiceNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 开票时间
        /// </summary>
        [DataMember]
        public string CreateDate
        {
            get;
            set;
        } 
        /// <summary>
          /// 开票时间
          /// </summary>
        [DataMember]
        public DateTime CreateDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 销方税号
        /// </summary>
        [DataMember]
        public string SalesTaxNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public decimal Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 税额
        /// </summary>
        [DataMember]
        public decimal Tax

        {
            get;
            set;
        }

        /// <summary>
        /// 来源
        /// </summary>
        [DataMember]
        public string From
        {
            get;
            set;
        }
        /// <summary>
        /// 发票状态 (正常)
        /// </summary>
        [DataMember]
        public string Status
        {
            get;
            set;
        }
        /// <summary>
        /// 勾选标志 （未勾选）
        /// </summary>
        [DataMember]
        public string SelectTag
        {
            get;
            set;
        }
        /// <summary>
        /// 操作时间 （未操作 / DateTime）
        /// </summary>
        [DataMember]
        public string OperationTime
        {
            get;
            set;
        }
        /// <summary>
        /// 认证状态 （页面：确认标志  ~ 未确认/已确认）
        /// </summary>
        [DataMember]
        public string CertificateStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 抵扣状态 （抵扣状态（1：已抵扣；2：未抵扣；3：已过期；））
        /// </summary>
        [DataMember]
        public string DeductionStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 购方税号  
        /// </summary>
        [DataMember]
        public string TaxCode
        {
            get;
            set;
        }

        /// <summary>
        /// '勾选标志（已勾选、未勾选）';
        /// </summary>
        [DataMember]
        public string IsChosen
        {
            get;
            set;
        }
        /// <summary>
        /// '确认标志（已确认，未确认）';  
        /// </summary>
        [DataMember]
        public string IsConfirmed
        {
            get;
            set;
        }
        /// </summary>
        [DataMember]
        public DateTime? BeginDateTime
        {
            get;
            set;
        }
        /// </summary>
        [DataMember]
        public DateTime? EndDateTime
        {
            get;
            set;
        }



    }

}
