using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZ.Aisino.CMS.DbEntity
{

    public enum ApplyStatus
    {
        /// <summary>
        /// 已提交待审核
        /// </summary>
        [Description("已提交待审核")]
        Submitted = 0,
        /// <summary>
        /// 审核通过
        /// </summary>
         [Description("审核通过")]
        Approved = 1,
        /// <summary>
        /// 拒绝
        /// </summary>
        [Description("拒绝")]
        Refuse = 2,



    }
    public enum SelectListEnum
    {
        Provinces = 1,
        MenuCategory=2,
        Contents=3,
        lecturers = 4,
        peroids = 5,
        locations = 6,
        departments = 7,
        Seats=8,
        Classes//班次
 
    }

    public enum MuseumStatus 
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 推荐
        /// </summary>
        [Description("推荐")]
        Recommend = 10,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Freeze = 20

        ///// <summary>
        ///// 已删除
        ///// </summary>
        //[Description("已删除")]
        //Deleted = -1
    }
    public enum ExhibitionStatus//待认领、已认领
    {
        /// <summary>
        /// 待认领
        /// </summary>
        [Description("待认领")]
        WaitForClaim = 0,
        /// <summary>
        /// 已认领
        /// </summary>
        [Description("已认领")]
        Claimed = 1,
        /// <summary>
        /// 推荐
        /// </summary>
        [Description("推荐")]
        Recommend = 10,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Freeze = 20

        ///// <summary>
        ///// 已删除
        ///// </summary>
        //[Description("已删除")]
        //Deleted = -1
    }
    public enum HallStatus 
    {
        /// <summary>
        /// 待预约
        /// </summary>
        [Description("待预约")]
        WaitForBook = 0,
        /// <summary>
        /// 已预约
        /// </summary>
        [Description("已预约")]
        Booked = 1,
        /// <summary>
        /// 推荐
        /// </summary>
        [Description("推荐")]
        Recommend = 10,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Freeze = 20

        ///// <summary>
        ///// 已删除
        ///// </summary>
        //[Description("已删除")]
        //Deleted = -1
    }
    public enum InfoStatus
    {
        ///// <summary>
        ///// 已删除
        ///// </summary>
        //[Description("已删除")]
        //Deleted = -1,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 推荐
        /// </summary>
        [Description("推荐")]
        Recommend = 10
    }
  
      public enum ThumbnailSizeEnum
    {
        /// <summary>
        /// origin
        /// </summary>
        [Description("origin")]
         origin = 0,
        /// <summary>
        /// w76
        /// </summary>
        [Description("w76")]
        w76 = 76,
        /// <summary>
        /// w180
        /// </summary>
        [Description("w180")]
        w180 = 180,
        /// <summary>
        /// 政策
        /// </summary>
        [Description("w400")]
        w400 = 400,
        /// <summary>
        /// 精品
        /// </summary>
        [Description("w650")]
        w650 = 650,
  
    }
          public enum LawsSubType 
    {
        /// <summary>
        /// 国家法律法规
        /// </summary>
        [Description("国家法律法规")]
        National = 0,
        /// <summary>
        /// 文物出入境相关规章
        /// </summary>
        [Description("文物出入境相关规章")]
        EntryExit = 1,
        /// <summary>
        /// 国际公约
        /// </summary>
        [Description("国际公约")]
        International = 2,
         /// <summary>
        /// 其他国家（地区）相关法规
        /// </summary>
        [Description("其他国家（地区）相关法规")]
        Others = 3,

     }
         public enum StructureCategoryEnum
         {
             /// <summary>
             /// 类目
             /// </summary>
             [Description("类目")]
             Catagory = 0,
             /// <summary>
             /// 详情
             /// </summary>
             [Description("详情")]
             Detail = 1,
        
          }
 


         public enum ViewOrientation
         {
             /// <summary>
             /// 水平
             /// </summary>
             [Description("水平")]
             Horizontal = 0,
             /// <summary>
             /// 垂直
             /// </summary>
             [Description("垂直")]
             Vertical = 1
          }


         public enum DataOptEnum
         {
             /// <summary>
             /// 资讯
             /// </summary>
             [Description("资讯")]
             News = 1,
             /// <summary>
             /// 专题
             /// </summary>
             [Description("专题")]
             Focus = 2,
             /// <summary>
             /// 政策
             /// </summary>
             [Description("政策")]
             Laws = 3,
             /// <summary>
             /// 精品
             /// </summary>
             [Description("精品")]
             Selection = 4


         }

         public enum TabResourceType
         {
             /// <summary>
             /// 资讯
             /// </summary>
             [Description("资讯")]
             News = 1,
             /// <summary>
             /// 专题
             /// </summary>
             [Description("专题")]
             Focus = 2,
             /// <summary>
             /// 政策
             /// </summary>
             [Description("政策")]
             Laws = 3,
             /// <summary>
             /// 精品
             /// </summary>
             [Description("精品")]
             Selection = 4

         }


         /// <summary>
         /// 所属模块/数据库
         /// </summary>
         public enum PosterBelongToEnum
         {
             /// <summary>
             /// 内容
             /// </summary>
             [Description("Info")]
             Info = 1
 
         }
 }
