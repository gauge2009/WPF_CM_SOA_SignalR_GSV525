using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZ.Aisino.CMS.DbEntity
{

    public enum DeleteMarkEnum
    {
        [Description("未删除")]
        Normal = 0,
        [Description("已停用")]
        Forbid = 1,
        [Description("已删除")]
        Deleted = 2
    }

    //public enum CmsInfoType : byte
    //{
    //    [Description("公司简介")]
    //    CompanyProfile = 1 << 0,
    //    [Description("企业文化")]
    //    EnterpriseCulture = 1 << 1,
    //    [Description("组织机构")]
    //    Organization = 1 << 2,
    //    [Description("大事记")]
    //    ChronicleEvent = 1 << 3,
    //    [Description("人才招聘")]
    //    Recruitment = 1 << 4,
    //    [Description("总公司荣誉")]
    //    AisinoHonor = 1 << 5,
    //    [Description("分公司荣誉")]
    //    FilialeHonor = 1 << 6,
    //    [Description("企业来信")]
    //    EnterpriseLetter = 1 << 7,
    //    [Description("公司新闻")]
    //    CompanyNews = 1 << 8,
    //    [Description("业内新闻")]
    //    RelationNews = 1 << 9,
    //    [Description("税局通知")]
    //    TaxOfficeNotify = 1 << 10,
    //    [Description("公司通知")]
    //    CompanyNotify = 1 << 11,
    //    [Description("典型案例")]
    //    ClassicDemo = 1 << 12
    //}
        
    [Serializable]
    //[Flags]
    public enum CmsInfoType //: long
    {
        [Description("公司简介")]
        Company = 1 ,
        [Description("企业文化")]
        Culture = 2,
        [Description("组织机构")]
        Org  =4,
        [Description("发展历程")]
        Event = 8,
        [Description("人才招聘")]
        Recruitment = 16,
        [Description("总公司荣誉")]
        AisinoHonor = 32,
        [Description("企业荣誉")]
        SzHonor = 64,
        [Description("企业来信")]
        Letter = 128,

        [Description("公司新闻")]
        InnerNews = 256,
        [Description("业内新闻")]
        BusinessNews = 512,
        [Description("税局通知")]
        TaxOfficeNotify = 1024,
        [Description("公司通知")]
        CompanyNotify = 2048,

        [Description("典型案例")]
        ClassicDemo = 4096,
        [Description("信心服务")]
        Services = 8192
    }
    public enum ContentCatalogEnum
    {
        [Description("新闻")]
        News,
        [Description("企业文化")]
        Cultrue = 1,
        [Description("通知")]
        Notify = 2,
        [Description("信心服务")]
        ServiceBrand = 3,
    }


    public enum InvestCatalogEnum
    {
        [Description("其他")]
        Others,
        [Description("培训")]
        Training = 1,
        [Description("设备")]
        Device = 2,
        [Description("财税平台")]
        CaishuiPlatform = 3,
        [Description("软件产品")]
        Software = 4,
        [Description("互联网电子商务")]
        EBusiness = 5,
        [Description("公司内部")]
        Inner = 6,
        [Description("产品")]
        Product = 7,
        [Description("服务")]
        Service = 8
    }

    //public enum InvestCatalogEnum
    //{
    //    [Description("其他")]
    //    Others=1,
    //    [Description("培训")]
    //    Training = 2,
    //    [Description("设备")]
    //    Device = 4,
    //    [Description("财税平台")]
    //    CaishuiPlatform = 8,
    //    [Description("软件产品")]
    //    Software = 16,
    //    [Description("互联网电子商务")]
    //    EBusiness = 32,
    //    [Description("公司内部")]
    //    Inner = 64,
    //    [Description("产品")]
    //    Product = 128,
    //    [Description("服务")]
    //    Service = 256
    //}

    public enum InvestQuestionEnum
      {
          [Description("选择题")]
           SingleChoice,
          [Description("问答题")]
          EssayQuestion = 1,
        
      }

    public enum InvestDomainEnum
    {
        [Description("无条件")]
        NoCondition,
        [Description("培训")]
        Training = 1,
        [Description("公司内部")]
        Inner = 2,
        [Description("质监")]
        QA = 3,
    }

}
