if(!ProvinceCitySelect)
{
    var ProvinceCitySelect={};
}

ProvinceCitySelect.create = function(select_array, ext_opt_array, opt_array) {
    return new MultiSelect.create(select_array, opt_array || ProvinceCitySelect.DATA, ext_opt_array || []);
};

ProvinceCitySelect.DATA= 
[
    {t:"北京", v:"0001", opt_data_array:
        [
            {t:"北京市", v:"0101"}
        ]
    },
    {t:"上海", v:"0002", opt_data_array:
        [
            {t:"上海市", v:"0201"}
        ]
    },
    {t:"天津", v:"0003", opt_data_array:
        [
            {t:"天津市", v:"0301"}
        ]
    },
    {t:"重庆", v:"0004", opt_data_array:
        [
            {t:"重庆市", v:"0401"}
        ]
    },
    {t:"江苏省", v:"0008", opt_data_array:
        [
            {t:"南京市", v:"0801"},
            {t:"无锡市", v:"0802"},
            {t:"徐州市", v:"0803"},
            {t:"常州市", v:"0804"},
            {t:"苏州市", v:"0805"},
            {t:"南通市", v:"0806"},
            {t:"连云港市", v:"0807"},
            {t:"淮阴市", v:"0808"},
            {t:"盐城市", v:"0809"},
            {t:"扬州市", v:"0810"},
            {t:"镇江市", v:"0811"},
            {t:"泰州市", v:"0812"},
            {t:"宿迁市", v:"0813"},
            {t:"淮安市", v:"0814"}
        ]
    },
    {t:"广东省", v:"0009", opt_data_array:
        [
            {t:"广州市", v:"0901"},
            {t:"深圳市", v:"0902"},
            {t:"珠海市", v:"0903"},
            {t:"汕头市", v:"0904"},
            {t:"韶关市", v:"0905"},
            {t:"河源市", v:"0906"},
            {t:"梅州市", v:"0907"},
            {t:"惠州市", v:"0908"},
            {t:"汕尾市", v:"0909"},
            {t:"东莞市", v:"0910"},
            {t:"中山市", v:"0911"},
            {t:"江门市", v:"0912"},
            {t:"佛山市", v:"0913"},
            {t:"阳江市", v:"0914"},
            {t:"湛江市", v:"0915"},
            {t:"茂名市", v:"0916"},
            {t:"肇庆市", v:"0917"},
            {t:"云浮市", v:"0918"},
            {t:"清远市", v:"0919"},
            {t:"潮州市", v:"0920"},
            {t:"揭阳市", v:"0921"}
        ]
    },
    {t:"江西省", v:"0010", opt_data_array:
        [
            {t:"抚州市", v:"1001"},
            {t:"赣州市", v:"1002"},
            {t:"吉安市", v:"1003"},
            {t:"景德镇市", v:"1004"},
            {t:"九江市", v:"1005"},
            {t:"南昌市", v:"1006"},
            {t:"萍乡市", v:"1007"},
            {t:"上饶市", v:"1008"},
            {t:"新余市", v:"1009"},
            {t:"宜春市", v:"1010"},
            {t:"鹰潭市", v:"1011"}
        ]
    },
    {t:"湖北省", v:"0011", opt_data_array:
        [
            {t:"鄂州市", v:"1101"},
            {t:"恩施市", v:"1102"},
            {t:"黄石市", v:"1103"},
            {t:"荆门市", v:"1104"},
            {t:"武汉市", v:"1105"},
            {t:"咸宁市", v:"1106"},
            {t:"襄樊市", v:"1107"},
            {t:"黄冈市", v:"1108"},
            {t:"孝感市", v:"1109"},
            {t:"宜昌市", v:"1110"},
            {t:"十堰市", v:"1111"},
            {t:"荆州市", v:"1112"},
            {t:"随州市", v:"1113"},
            {t:"仙桃市", v:"1114"},
            {t:"潜江市", v:"1115"},
            {t:"神农架林区", v:"1116"},
            {t:"天门市", v:"1117"}
        ]
    },
    {t:"山西省", v:"0012", opt_data_array:
        [
            {t:"忻州市", v:"1201"},
            {t:"吕梁市", v:"1202"},
            {t:"临汾市", v:"1203"},
            {t:"晋中市", v:"1204"},
            {t:"运城市", v:"1205"},
            {t:"太原市", v:"1206"},
            {t:"大同市", v:"1207"},
            {t:"阳泉市", v:"1208"},
            {t:"长治市", v:"1209"},
            {t:"晋城市", v:"1210"},
            {t:"朔州市", v:"1211"}
        ]
    },
    {t:"山东省", v:"0013", opt_data_array:
        [
            {t:"滨州市", v:"1301"},
            {t:"德州市", v:"1302"},
            {t:"东营市", v:"1303"},
            {t:"菏泽市", v:"1304"},
            {t:"济南市", v:"1305"},
            {t:"济宁市", v:"1306"},
            {t:"莱芜市", v:"1307"},
            {t:"聊城市", v:"1308"},
            {t:"临沂市", v:"1309"},
            {t:"青岛市", v:"1310"},
            {t:"日照市", v:"1311"},
            {t:"泰安市", v:"1312"},
            {t:"威海市", v:"1313"},
            {t:"潍坊市", v:"1314"},
            {t:"烟台市", v:"1315"},
            {t:"枣庄市", v:"1316"},
            {t:"淄博市", v:"1317"}
        ]
    },
    {t:"河南省", v:"0014", opt_data_array:
        [
            {t:"郑州市", v:"1401"},
            {t:"开封市", v:"1402"},
            {t:"洛阳市", v:"1403"},
            {t:"平顶山市", v:"1404"},
            {t:"焦作市", v:"1405"},
            {t:"鹤壁市", v:"1406"},
            {t:"新乡市", v:"1407"},
            {t:"安阳市", v:"1408"},
            {t:"濮阳市", v:"1409"},
            {t:"许昌市", v:"1410"},
            {t:"漯河市", v:"1411"},
            {t:"三门峡市", v:"1412"},
            {t:"南阳市", v:"1413"},
            {t:"商丘市", v:"1414"},
            {t:"信阳市", v:"1415"},
            {t:"济源市", v:"1416"},
            {t:"周口市", v:"1417"},
            {t:"驻马店市", v:"1418"}
        ]
    },
    {t:"云南省", v:"0015", opt_data_array:
        [
            {t:"昆明市", v:"1501"},
            {t:"曲靖市", v:"1502"},
            {t:"玉溪市", v:"1503"},
            {t:"昭通市", v:"1504"},
            {t:"思茅市", v:"1505"},
            {t:"临沧市", v:"1506"},
            {t:"保山市", v:"1507"},
            {t:"丽江市", v:"1508"},
            {t:"文山州", v:"1509"},
            {t:"红河州", v:"1510"},
            {t:"西双版纳州", v:"1511"},
            {t:"楚雄州", v:"1512"},
            {t:"大理州", v:"1513"},
            {t:"德宏州", v:"1514"},
            {t:"怒江州", v:"1515"},
            {t:"迪庆州", v:"1516"}
        ]
    },
    {t:"吉林省", v:"0016", opt_data_array:
        [
            {t:"长春市", v:"1601"},
            {t:"吉林市", v:"1602"},
            {t:"四平市", v:"1603"},
            {t:"辽源市", v:"1604"},
            {t:"通化市", v:"1605"},
            {t:"白山市", v:"1606"},
            {t:"松原市", v:"1607"},
            {t:"白城市", v:"1608"},
            {t:"延边州", v:"1609"}
        ]
    },
    {t:"浙江省", v:"0017", opt_data_array:
        [
            {t:"杭州市", v:"1701"},
            {t:"宁波市", v:"1702"},
            {t:"温州市", v:"1703"},
            {t:"嘉兴市", v:"1704"},
            {t:"湖州市", v:"1705"},
            {t:"绍兴市", v:"1706"},
            {t:"金华市", v:"1707"},
            {t:"衢州市", v:"1708"},
            {t:"舟山市", v:"1709"},
            {t:"台州市", v:"1710"},
            {t:"丽水市", v:"1711"}
        ]
    },
    {t:"河北省", v:"0018", opt_data_array:
        [
            {t:"石家庄市", v:"1801"},
            {t:"邯郸市", v:"1802"},
            {t:"邢台市", v:"1803"},
            {t:"保定市", v:"1804"},
            {t:"张家口市", v:"1805"},
            {t:"承德市", v:"1806"},
            {t:"唐山市", v:"1807"},
            {t:"秦皇岛市", v:"1808"},
            {t:"沧州市", v:"1809"},
            {t:"廊坊市", v:"1810"},
            {t:"衡水市", v:"1811"}
        ]
    },
    {t:"西藏", v:"0019", opt_data_array:
        [
            {t:"拉萨市", v:"1901"},
            {t:"那曲市", v:"1902"},
            {t:"昌都市", v:"1903"},
            {t:"山南市", v:"1904"},
            {t:"日喀则市", v:"1905"},
            {t:"阿里市", v:"1906"},
            {t:"林芝市", v:"1907"}
        ]
    },
    {t:"贵州省", v:"0020", opt_data_array:
        [
            {t:"贵阳市", v:"2001"},
            {t:"六盘水市", v:"2002"},
            {t:"遵义市", v:"2003"},
            {t:"铜仁市", v:"2004"},
            {t:"毕节地区", v:"2005"},
            {t:"安顺市", v:"2006"},
            {t:"黔西市", v:"2007"},
            {t:"黔东市", v:"2008"},
            {t:"黔南州", v:"2009"},
            {t:"黔东南州", v:"2010"}
        ]
    },
    {t:"湖南省", v:"0021", opt_data_array:
        [
            {t:"长沙市", v:"2101"},
            {t:"株洲市", v:"2102"},
            {t:"湘潭市", v:"2103"},
            {t:"衡阳市", v:"2104"},
            {t:"邵阳市", v:"2105"},
            {t:"岳阳市", v:"2106"},
            {t:"常德市", v:"2107"},
            {t:"郴州市", v:"2108"},
            {t:"永州市", v:"2109"},
            {t:"怀化市", v:"2110"},
            {t:"娄底市", v:"2111"},
            {t:"益阳市", v:"2112"},
            {t:"湘西市", v:"2113"},
            {t:"张家界市", v:"2114"}
        ]
    },
    {t:"宁夏", v:"0022", opt_data_array:
        [
            {t:"银川市", v:"2201"},
            {t:"石嘴山市", v:"2202"},
            {t:"银南市", v:"2203"},
            {t:"吴忠市", v:"2204"},
            {t:"中卫市", v:"2205"},
            {t:"固原市", v:"2206"}
        ]
    },
    {t:"四川省", v:"0023", opt_data_array:
        [
            {t:"成都市", v:"2301"},
            {t:"自贡市", v:"2302"},
            {t:"攀枝花市", v:"2303"},
            {t:"泸州市", v:"2304"},
            {t:"德阳市", v:"2305"},
            {t:"绵阳市", v:"2306"},
            {t:"广元市", v:"2307"},
            {t:"遂宁市", v:"2308"},
            {t:"内江市", v:"2309"},
            {t:"乐山市", v:"2310"},
            {t:"南充市", v:"2311"},
            {t:"宜宾市", v:"2312"},
            {t:"广安市", v:"2313"},
            {t:"达州市", v:"2314"},
            {t:"巴中市", v:"2315"},
            {t:"雅安市", v:"2316"},
            {t:"眉山市", v:"2317"},
            {t:"资阳市", v:"2318"},
            {t:"阿坝州", v:"2319"},
            {t:"甘孜市", v:"2320"},
            {t:"凉山市", v:"2321"}
        ]
    },
    {t:"陕西省", v:"0024", opt_data_array:
        [
            {t:"西安市", v:"2401"},
            {t:"宝鸡市", v:"2402"},
            {t:"咸阳市", v:"2403"},
            {t:"渭南市", v:"2404"},
            {t:"延安市", v:"2405"},
            {t:"汉中市", v:"2406"},
            {t:"榆林市", v:"2407"},
            {t:"商洛市", v:"2408"},
            {t:"安康市", v:"2409"},
            {t:"铜川市",v:"2410"}
        ]
    },
    {t:"海南省", v:"0025", opt_data_array:
        [
            {t:"海口市", v:"2501"},
            {t:"三亚市", v:"2502"},
            {t:"乐东市", v:"2503"},
            {t:"陵水市", v:"2504"},
            {t:"东方市", v:"2505"},
            {t:"琼中市", v:"2506"},
            {t:"昌江市", v:"2507"},
            {t:"保亭市", v:"2508"},
            {t:"文昌市", v:"2509"},
            {t:"琼海市", v:"2510"}
        ]
    },
    {t:"广西", v:"0026", opt_data_array:
        [
            {t:"南宁市", v:"2601"},
            {t:"柳州市", v:"2602"},
            {t:"桂林市", v:"2603"},
            {t:"梧州市", v:"2604"},
            {t:"北海市", v:"2605"},
            {t:"防城港市", v:"2606"},
            {t:"钦州市", v:"2607"},
            {t:"贵港市", v:"2608"},
            {t:"玉林市", v:"2609"},
            {t:"南宁市", v:"2610"},
            {t:"柳州市", v:"2611"},
            {t:"贺州市", v:"2612"},
            {t:"百色市", v:"2613"},
            {t:"河池市", v:"2614"},
            {t:"崇左市", v:"2615"},
            {t:"来宾市", v:"2616"}
        ]
    },
    {t:"福建省", v:"0027", opt_data_array:
        [
            {t:"福州市", v:"2701"},
            {t:"厦门市", v:"2702"},
            {t:"三明市", v:"2703"},
            {t:"莆田市", v:"2704"},
            {t:"泉州市", v:"2705"},
            {t:"漳州市", v:"2706"},
            {t:"南平市", v:"2707"},
            {t:"宁德市", v:"2708"},
            {t:"龙岩市", v:"2709"}
        ]
    },
    {t:"辽宁省", v:"0028", opt_data_array:
        [
            {t:"沈阳市", v:"2801"},
            {t:"大连市", v:"2802"},
            {t:"鞍山市", v:"2803"},
            {t:"盘锦市", v:"2804"},
            {t:"抚顺市", v:"2805"},
            {t:"本溪市", v:"2806"},
            {t:"丹东市", v:"2807"},
            {t:"锦州市", v:"2808"},
            {t:"营口市", v:"2809"},
            {t:"葫芦岛市", v:"2810"},
            {t:"阜新市", v:"2811"},
            {t:"辽阳市", v:"2812"},
            {t:"铁岭市", v:"2813"},
            {t:"朝阳市", v:"2814"}
        ]
    },
    {t:"黑龙江省", v:"0029", opt_data_array:
        [
            {t:"哈尔滨市", v:"2901"},
            {t:"齐齐哈尔市", v:"2902"},
            {t:"鹤岗市", v:"2903"},
            {t:"双鸭山市", v:"2904"},
            {t:"鸡西市", v:"2905"},
            {t:"大庆市", v:"2906"},
            {t:"伊春市", v:"2907"},
            {t:"牡丹江市", v:"2908"},
            {t:"佳木斯市", v:"2909"},
            {t:"七台河市", v:"2910"},
            {t:"黑河市", v:"2911"},
            {t:"绥化市", v:"2912"},
            {t:"大兴安岭市", v:"2913"}
        ]
    },
    {t:"新疆", v:"0030", opt_data_array:
        [
            {t:"乌鲁木齐市", v:"3001"},
            {t:"克拉玛依市", v:"3002"},
            {t:"石河子市", v:"3003"},
            {t:"吐鲁番地区", v:"3004"},
            {t:"哈密地区", v:"3005"},
            {t:"昌吉州", v:"3006"},
            {t:"伊犁哈萨克自治州", v:"3007"},
            {t:"伊犁州", v:"3008"},
            {t:"塔城地区", v:"3009"},
            {t:"阿勒泰地区", v:"3010"},
            {t:"博尔塔拉州", v:"3011"},
            {t:"巴音郭楞州", v:"3012"},
            {t:"阿克苏地区", v:"3013"},
            {t:"克孜勒苏柯州", v:"3014"},
            {t:"喀什地区", v:"3015"},
            {t:"和田地区", v:"3016"},
            {t:"五家渠市", v:"3017"},
            {t:"阿拉尔市", v:"3018"}
        ]
    },
    {t:"青海省", v:"0031", opt_data_array:
        [
            {t:"西宁市", v:"3101"},
            {t:"海东市", v:"3102"},
            {t:"海北市", v:"3103"},
            {t:"黄南市", v:"3104"},
            {t:"海南市", v:"3105"},
            {t:"果洛市", v:"3106"},
            {t:"玉树市", v:"3107"},
            {t:"海西市", v:"3108"}
        ]
    },
    {t:"内蒙古", v:"0032", opt_data_array:
        [
            {t:"呼和浩特市", v:"3201"},
            {t:"包头市", v:"3202"},
            {t:"乌海市", v:"3203"},
            {t:"赤峰市", v:"3204"},
            {t:"乌兰察布市", v:"3205"},
            {t:"锡林郭勒市", v:"3206"},
            {t:"呼伦贝尔市", v:"3207"},
            {t:"哲里木市", v:"3208"},
            {t:"伊克昭市", v:"3209"},
            {t:"巴彦淖尔市", v:"3210"},
            {t:"通辽市", v:"3211"},
            {t:"兴安盟", v:"3212"},
            {t:"鄂尔多斯市", v:"3213"}
        ]
    },
    {t:"甘肃省", v:"0033", opt_data_array:
        [
            {t:"白银市", v:"3301"},
            {t:"定西市", v:"3302"},
            {t:"甘南市", v:"3303"},
            {t:"嘉峪关市", v:"3304"},
            {t:"金昌市", v:"3305"},
            {t:"酒泉市", v:"3306"},
            {t:"兰州市", v:"3307"},
            {t:"临夏市", v:"3308"},
            {t:"平凉市", v:"3309"},
            {t:"天水市", v:"3310"},
            {t:"武威市", v:"3311"},
            {t:"张掖市", v:"3312"},
            {t:"庆阳市", v:"3313"},
            {t:"陇南市", v:"3314"}
        ]
    },
    {t:"安徽省", v:"0034", opt_data_array:
        [
            {t:"安庆市", v:"3401"},
            {t:"蚌埠市", v:"3402"},
            {t:"巢湖市", v:"3403"},
            {t:"池州市", v:"3404"},
            {t:"滁州市", v:"3405"},
            {t:"阜阳市", v:"3406"},
            {t:"合肥市", v:"3407"},
            {t:"淮北市", v:"3408"},
            {t:"淮南市", v:"3409"},
            {t:"黄山市", v:"3410"},
            {t:"六安市", v:"3411"},
            {t:"马鞍山市", v:"3412"},
            {t:"宿州市", v:"3413"},
            {t:"铜陵市", v:"3414"},
            {t:"芜湖市", v:"3415"},
            {t:"宣城市", v:"3416"},
            {t:"亳州市", v:"3417"}
        ]
    },
    {t:"香港", v:"00005", opt_data_array:
        [
            {t:"香港", v:"501"}
        ]
    },
    {t:"澳门", v:"0006", opt_data_array:
        [
            {t:"澳门", v:"601"}
        ]
    },   
    {t:"台湾", v:"0007", opt_data_array:
        [
            {t:"台湾", v:"701"}
        ]
    }    
];

ProvinceCitySelect.PROVINCE_TYPE="1";
ProvinceCitySelect.CITY_TYPE="2";

ProvinceCitySelect.FindByKey = function(arr, val, key) {
    var data = arr;
    var len = data.length ? data.length : 0;
    for (var i = 0; i < len; i++) {
        if (data[i][key] == val) {
            return i;
        }
    }
    return -1;
};

ProvinceCitySelect.id2desc = function(sid, type) {
    var pdata = ProvinceCitySelect.DATA;
    if (type && type == ProvinceCitySelect.PROVINCE_TYPE) {
        var pidx = ProvinceCitySelect.FindByKey(pdata, Number(sid), "v");
        return pidx >= 0 ? pdata[pidx].t : "";
    } else {
        var re = /(\d{1,2})\d{2}$/img;
        var pid; //省份id
        if (!re.test(sid)) {
            return "";
        }
        pid = RegExp.$1;
        var pidx = ProvinceCitySelect.FindByKey(pdata, Number(pid), "v");
        if (pidx >= 0) {
            var cdata = pdata[pidx].opt_data_array;
            var cidx = ProvinceCitySelect.FindByKey(cdata, Number(sid), "v");
            return cidx >= 0 ? cdata[cidx].t : "";
        }
        return "";
    }
};

ProvinceCitySelect.GetProvinceDesc = function(sid) {
    return ProvinceCitySelect.id2desc(sid, ProvinceCitySelect.PROVINCE_TYPE);
};

ProvinceCitySelect.GetCityDesc = function(sid) {
    return ProvinceCitySelect.id2desc(sid, ProvinceCitySelect.CITY_TYPE);
};


/////////////////////////////////////////////////////////////////////////////////
// MultiSelect
// 推荐使用MultiSelect.create()来生成对象，参数不变
// handle_array   [handle_select1, handle_select2, ...]
// opt_data_array [opt_data1, opt_data2, ... ]
// opt_data       {t:text, v:value, s:selected, opt_data_array:[opt_data_array] }
// custom_onchange_fun_array [customer_onchange_fun1, customer_onchange_fun2, ...] 参数可选
/////////////////////////////////////////////////////////////////////////////////
var MultiSelect = function(select_array, opt_data_array, ext_opt_data_array, custom_onchange_fun_array) {
    if (select_array instanceof Array && select_array.length > 0) {

        this.select = select_array[0];
        this.left_selects = [];
        for (var i = 1; i < select_array.length; ++i) {
            this.left_selects.push(select_array[i]);
        }

        this.opt_data_array = opt_data_array || [];
        this.ext_opt_data_array = ext_opt_data_array || [];

        if (!custom_onchange_fun_array) {
            custom_onchange_fun_array = [];
            for (var i = 0; i < select_array.length; ++i) {
                custom_onchange_fun_array.push(select_array[i].onchange || function() {
                });
            }
        }

        this.custom_onchange_fun = custom_onchange_fun_array[0];
        this.left_custom_funs = [];
        for (var i = 1; i < custom_onchange_fun_array.length; ++i) {
            this.left_custom_funs.push(custom_onchange_fun_array[i]);
        }

        this.init();
    }
};

MultiSelect.create = function(select_array, opt_data_array, ext_data_array, custom_onchange_fun_array) {
    var obj = new MultiSelect(select_array, opt_data_array, ext_data_array, custom_onchange_fun_array);
    MultiSelect["_OBJ_" + MultiSelect._OBJECT_NUM++] = obj;
    return obj;
};

MultiSelect._OBJECT_NUM = 0;

MultiSelect.prototype.init = function() {
    this._initOption();

    if (this.left_selects.length > 0) {
        this._initOnchangeHandler();
    }

    if (this.select.onchange) {
        this.select.onchange(0, 1);
    }
    return;
};

MultiSelect.prototype.getSelectByIndex = function(index) {
    if (index == 0) {
        return this;
    }
    if (this.left_selects.length == 0) {
        return null
    }
    return this.next.getSelectByIndex(index - 1);
};

MultiSelect.prototype.getSelectByHandle = function(select_handle) {
    if (select_handle == this.select) {
        return this;
    }
    if (this.left_selects.length == 0) {
        return null;
    }
    return this.next.getSelectByHandle(select_handle);
};

MultiSelect.prototype._initOption = function() {
    this.select.length = 0;

    //var opt_fragment = document.createDocumentFragment();
    //this._createOptionDom(this.ext_opt_data_array, opt_fragment);
    //this._createOptionDom(this.opt_data_array, opt_fragment);
    //this.select.appendChild(opt_fragment);

    this._createOption(this.ext_opt_data_array);
    this._createOption(this.opt_data_array);
};

MultiSelect.prototype._createOptionDom = function(opt_data_array, opt_fragment) {
    for (var i = 0; i < opt_data_array.length; ++i) {

        var opt_data = opt_data_array[i];
        var o = document.createElement("option");

        if (opt_data.t == undefined || opt_data.t == null) {
            opt_data.t = "";
        }

        if (opt_data.v == undefined || opt_data.v == null) {
            opt_data.v = opt_data.t;
        }
        o.setAttribute("value", opt_data.v);

        if (opt_data.s) {
            o.setAttribute("selected", true);
        }

        var t = document.createTextNode(opt_data.t);
        o.appendChild(t);
        opt_fragment.appendChild(o);
    }
};

MultiSelect.prototype._createOption = function(opt_data_array) {
    for (var i = 0; i < opt_data_array.length; ++i) {

        var opt_data = opt_data_array[i];

        if (opt_data.t == undefined || opt_data.t == null) {
            opt_data.t = "";
        }

        if (opt_data.v == undefined || opt_data.v == null) {
            opt_data.v = opt_data.t;
        }

        this.select.options[this.select.length] = new Option(opt_data.t, opt_data.v, false, (opt_data.s == true));
    }
};

MultiSelect.CALL_TYPE = {};
MultiSelect.CALL_TYPE.INIT = 0;     // 初始化调用
MultiSelect.CALL_TYPE.PROGRAM = 1;  // 页面中显式调用select.onchange()
MultiSelect.CALL_TYPE.BROWSER = 2;  // 用户触发的onchange事件时调用

MultiSelect.prototype._initOnchangeHandler = function() {
    var this_multi_select = this;
    var select_handle = this_multi_select.select;
    var custom_onchange_fun = this_multi_select.custom_onchange_fun;

    select_handle.onchange = function(event, init) {

        event = window.event || event;
        var call_type = MultiSelect.CALL_TYPE.INIT;

        if (!init) {
            if (!event) {
                call_type = MultiSelect.CALL_TYPE.PROGRAM;
            } else {
                call_type = MultiSelect.CALL_TYPE.BROWSER;
            }
        }

        var args = {
            event: event,
            select: select_handle,
            call_type: call_type,
            multi_select: this_multi_select
        };

        if (custom_onchange_fun(args) == false) {
            return;
        }

        this_multi_select.next = new MultiSelect(this_multi_select.left_selects,
            this_multi_select._getNextSelectOptArray(select_handle.value),
            this_multi_select._getNextExtSelectOptArray(select_handle.value),
            this_multi_select.left_custom_funs);
    };
};

MultiSelect.prototype._getNextSelectOptArray = function(value) {
    for (var i = 0; i < this.opt_data_array.length; ++i) {
        if (this.opt_data_array[i].v == value) {
            return this.opt_data_array[i].opt_data_array;
        }
    }
    return [];
};

MultiSelect.prototype._getNextExtSelectOptArray = function(value) {
    for (var i = 0; i < this.ext_opt_data_array.length; ++i) {
        if (this.ext_opt_data_array[i].v == value) {
            return this.ext_opt_data_array[i].opt_data_array;
        }
    }

    if (this.ext_opt_data_array.length <= 0) {
        return [];
    }
    return this.ext_opt_data_array[0].opt_data_array || [];
};

MultiSelect = window.MultiSelect || {};
(function(obj, $) {
    obj.SetSelectOption = function(objId1, desObjId1, objId2, desObjId2) {
        if ($("#" + objId1).val() && $("#" + objId2).val()) {
            $("#" + desObjId1).html("<option>请选择省份</option>");
            for (var i = 0; i < ProvinceCitySelect.DATA.length; i++) {
                if (ProvinceCitySelect.DATA[i].t == $("#" + objId1).val()) {
                    $("#" + desObjId1).append("<option selected='selected' value='" + ProvinceCitySelect.DATA[i].v + "'>" + ProvinceCitySelect.DATA[i].t + "</option>");
                    for (var j = 0; j < ProvinceCitySelect.DATA[i].opt_data_array.length; j++) {

                        if (ProvinceCitySelect.DATA[i].opt_data_array[j].t == $("#" + objId2).val()) {
                            $("#" + desObjId2).append("<option selected='selected' value='" + ProvinceCitySelect.DATA[i].opt_data_array[j].v + "'>" + ProvinceCitySelect.DATA[i].opt_data_array[j].t + "</option>");
                        } else {
                            $("#" + desObjId2).append("<option value='" + ProvinceCitySelect.DATA[i].opt_data_array[j].v + "'>" + ProvinceCitySelect.DATA[i].opt_data_array[j].t + "</option>");
                        }
                    }
                } else {
                    $("#" + desObjId1).append("<option value='" + ProvinceCitySelect.DATA[i].v + "'>" + ProvinceCitySelect.DATA[i].t + "</option>");
                }
            }
        }
    };

})(MultiSelect, $);

