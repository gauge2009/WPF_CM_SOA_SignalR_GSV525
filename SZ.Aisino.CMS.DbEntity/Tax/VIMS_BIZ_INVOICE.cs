//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SZ.Aisino.CMS.DbEntity.Tax
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    
    
    /// <summary>
    /// 
    /// </summary>
    [Serializable,DataContract]
    public partial class VIMS_BIZ_INVOICE
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(36)]
    	public string ID { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(2)]
    	public string FPLB { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(12)]
    	public string FPDM { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(10)]
    	public string FPHM { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required]
    	public System.DateTime KPRQ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(20)]
    	public string JQBH { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(20)]
    	public string GFSH { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(20)]
    	public string XFSH { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(190)]
    	public string FPMW { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required]
    	public decimal FPJE { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required]
    	public decimal FPSE { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
    	public decimal? FPSL { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(2)]
    	public string QDBZ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(255)]
    	public string GFMC { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(255)]
    	public string XFMC { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(672)]
    	public string HZMW { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string XHQDBZ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string KJQY { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(11)]
    	public string SWJGDM { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(20)]
    	public string DKDWDM { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(3)]
    	public string JMBZ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember , Required, StringLength(3)]
    	public string RZJG { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(255)]
    	public string RZJGSM { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
    	public System.DateTime? RZSJ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string SHBZ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
    	public System.DateTime? SHRQ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string TAXHALLFLAG { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(50)]
    	public string USERNAME { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string TSBZ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(50)]
    	public string YWBZ { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(50)]
    	public string PZH { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string FPLY { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string DKZT { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string FPYT { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string RZLY { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(255)]
    	public string PZLB { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(50)]
    	public string RZSBYY { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember, StringLength(2)]
    	public string RZZT { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
    	public System.DateTime? UPDATETIME { get; set; }
    
    }
}
