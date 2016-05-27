using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZ.Aisino.CMS.DbEntity
{
    public enum AuthorizeType
    {
        [Description("未处理")]
        Normal = 0,
        [Description("许可")]
        Allow = 1,
        [Description("阻止")]
        Forbid = -1,
    }
    public enum PowerType
    {
        [Description("已授权")]
        HasPower,
        [Description("未授权")]
        NoPower,
    }

    public enum DeleteMarkType
    {
        [Description("未删除")]
        Normal = 0,
        [Description("已停用")]
        Forbid = 1, 
        [Description("已删除")]
        Deleted = 2
    }

    public enum PlatformType
    {
        [Description("Web")]
        Web = 1,
        [Description("Client")]
        Client = 2,
        [Description("SmartClient")]
        SmartClient = 3,
        [Description("iOS(iPhone)")]
        iOS_iPhone = 4,
        [Description("iOS(iPad)")]
        iOS_iPad = 5,
        [Description("Android(Phone)")]
        Android_Phone = 6,
        [Description("Android(Pad)")]
        Android_Pad = 7,
        [Description("WindowsPhone")]
        WindowsPhone = 8,
        [Description("WindowsPad")]
        WindowsPad = 9
    }

    /// <summary>
    /// 模块类型
    /// 支持位运算
    /// </summary>
    [Flags]
     public enum ModuleType 
    {
        [Description("响应菜单的模块(页面)")]
        OpenByMenu = 0x01,
        [Description("响应按钮的模块(页面)")]
        OpenByButton = 0x02,
        [Description("其它方式调用的模块")]
        OpenByWhatever = 0x04,
        [Description("数据接口")]
        DataAPI = 0x08
    }
    [Flags]
    public enum ResourceType
    {
        [Description("菜单资源")]
        Page = 1,
        [Description("按钮资源")]
        Button = 2,
        [Description("其它资源")]
        Others = 4
    }  
    public enum ModuleLayerType
    {
        [Description("管理范畴")]
        AdminRegion = 1,
        [Description("产品分类")]
        ProductCatalog = 2,
        [Description("产品详情")]
        ProductDetail = 3,
    }
    
    public enum ButtonPositionType
    {

        [Description("工具栏")]
        Toolbar,
        [Description("数据列表(行)")]
        DataRow,
        [Description("其它")]
        Others
    }

    /// <summary>
    /// EventButton~模块上的事件按钮
    /// PageButton~模块上的页面按钮(每个页面按钮必通过RESPONSE_ID关联一个子模块)
    /// </summary>
    [Flags]
    public enum ModuleButtonType  
    {
        [Description("事件按钮")]
        EventButton = 0x01,
        [Description("页面按钮")]
        PageButton = 0x02
    }


//权限类型:菜单/按钮/模块/其它
 //1-响应菜单的模块(页面)
//2-响应按钮的模块(页面)
//3-其它方式调用的模块
//4-
//5-按钮(仅按钮显示)
//6-
//7-接口
//8-
//9-其它
    /// <summary>
    /// 当权限为按钮时，此处值为 5（16） ;当权限为非按钮时取模块类型MODULE_TYPE的值
    /// </summary>
    [Flags]
    public enum PermissionType    
    {
        [Description("响应菜单的模块(页面)")]
        ResponseToMenu = 1,
        [Description("响应页面按钮的模块(页面)")]
        ResponseToPageButton = 2,
        [Description("其它方式调用的模块")]
        ResponseToOthers = 4,
        [Description("接口")]
        ResponseAsAPI = 8,
        [Description("事件按钮(仅按钮显示)")]
        ResponseToEventButton = 16,
        [Description("其它")]
        Others = 32
    }

 


/*菜单分类
0-不分类
1-系统管理类
2-基础信息维护类
3-用户管理类
4-个人信息管理类
5-业务功能类
6-查询统计类
7-
8-
9-其它类*/
    public enum MenuClassType
    {

        [Description("不分类")]
        UnClassify,
        [Description("系统管理类")]
        SyatemAdmin,
        [Description("基础信息维护类")]
        BasicInfoMaintain,
        [Description("用户管理类")]
        UserAdmin,
        [Description("个人信息管理类")]
        PersonalInfoAdmin,
        [Description("业务功能类")]
        BizFunction,
        [Description("查询统计类")]
        SearchStats,
        [Description("其它类")]
        Others,
     }
    public enum MenuTargetType
    {

        [Description("Open")]
        Open = 1,
        [Description("Iframe")]
        Iframe,
        [Description("href")]
        Href,
        [Description("_blank")]
        Blank,
        [Description("_parent")]
        Parent,
        [Description("_top")]
        Top,
    }

     public enum RoleApplyToType
    {
        [Description("无限制")]
        Unlimited = -1,
        [Description("运维企业")]
        Operation = 0,
        [Description("客户公司")]
        CustomerOwner = 10
     }
    
}
