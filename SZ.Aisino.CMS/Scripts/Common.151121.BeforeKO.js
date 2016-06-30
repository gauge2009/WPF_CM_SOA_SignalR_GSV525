//"use strict";

//语言全局变量
var LangHelper = window.LangHelper || {};
(function (l) {
    l.Lang = "zh-CN";
    l["zh-CN"] = {
        OK: "确定",
        Close: "关闭",
        Cancel: "取消",
        AlertTitle: "页面提示信息"
    };
    l["en-US"] = {
        OK: "Sure",
        Close: "Close",
        Cancel: "Cancel",
        AlertTitle: "Page message"
    };
    l["zh-TW"] = {
        OK: "確定",
        Close: "關閉",
        Cancel: "取消",
        AlertTitle: "頁面提示信息"
    };
})(LangHelper);

/*重写window.alert方法*/
(function (_) {
    _.alert = function (message) {
        JR.ColorBox.Alert("tips", LangHelper[LangHelper.Lang].AlertTitle, message, null, false, null, LangHelper[LangHelper.Lang].Close);
    };


    _.doFunction = function (fun) {
        var args = [], i;
        for (i = 1; i < arguments.length; i++) {
            args.push(arguments[i]);
        }
        return function () {
            fun.apply(null, args);
        };
    };

})(window);

$.extend($.expr[':'], {
    regex: function (elem, index, match) {
        var matchParams = match[3].match(/(\w+?),(.+)/);
        validLabels = /^(data|css):/,
		attr = {
		    method: matchParams[1].match(validLabels) ? matchParams[1].split(':')[1] : 'attr',
		    property: matchParams[1]
		},
		regexOpts = matchParams[2].match(/\/(.+)?\/(\w*)/);
        regex = new RegExp(regexOpts[1], regexOpts.length == 3 ? regexOpts[2] : "");
        return regex.test(jQuery(elem)[attr.method](attr.property));
    }
});

(function ($) {
    //https://code.google.com/p/jquery-loadm
    $.fn.mask = function (delay) {
        $(this).each(function () {
            if (delay !== undefined && delay > 0) {
                var element = $(this);
                element.data("_mask_timeout", setTimeout(function () { $.maskElement(element, label) }, delay));
            } else {
                $.maskElement($(this));
            }
        });
    };

    $.fn.unmask = function () {
        $(this).each(function () {
            $.unmaskElement($(this));
        });
    };

    $.fn.isMasked = function () {
        return this.hasClass("masked");
    };

    $.maskElement = function (element) {

        if (element.data("_mask_timeout") !== undefined) {
            clearTimeout(element.data("_mask_timeout"));
            element.removeData("_mask_timeout");
        }

        if (element.isMasked()) {
            $.unmaskElement(element);
        }

        if (element.css("position") == "static") {
            element.addClass("masked-relative");
        }

        element.addClass("masked");

        var maskDiv = $('<div class="loadmask"></div>');

        //auto height fix for IE 6
        if (navigator.userAgent.toLowerCase().indexOf("msie 6") > -1) {
            maskDiv.height(element.height() + parseInt(element.css("padding-top")) + parseInt(element.css("padding-bottom")));
            maskDiv.width(element.width() + parseInt(element.css("padding-left")) + parseInt(element.css("padding-right")));

            element.bind("resize.mask", function () {
                maskDiv.height(element.height() + parseInt(element.css("padding-top")) + parseInt(element.css("padding-bottom")));
                maskDiv.width(element.width() + parseInt(element.css("padding-left")) + parseInt(element.css("padding-right")));
            });
        }

        //fix for z-index bug with selects in IE6
        if (navigator.userAgent.toLowerCase().indexOf("msie 6") > -1) {
            element.find("select").addClass("masked-hidden");
        }

        element.append(maskDiv);
        var maskMsgDiv = $('<div class="loadmask-msg" style="display:none;"></div>');
        element.append(maskMsgDiv);

        //maskMsgDiv.show();
        maskMsgDiv.fadeIn("slow");
    };

    $.unmaskElement = function (element) {
        //if this element has delayed mask scheduled then remove it
        if (element.data("_mask_timeout") !== undefined) {
            clearTimeout(element.data("_mask_timeout"));
            element.removeData("_mask_timeout");
        }

        element.unbind("resize.mask");

        element.find(".loadmask-msg,.loadmask").fadeOut("slow", function () {
            $(this).remove();
        })
        element.removeClass("masked");
        element.removeClass("masked-relative");
        element.find("select").removeClass("masked-hidden");
    };

})(jQuery);


(function () {

    Array.prototype.indexOf2 = function (ele) {
        /// <summary>
        /// 非严格等于
        /// IE下 数组没有 indexOf
        /// jquery.inArray 和 其它浏览器的 indexOf 是严格等于的
        /// </summary>
        /// <param name="ele"></param>

        for (var i = 0, length = this.length; i < length; i++) {
            if (this[i] == ele) {
                return i;
            }
        }
        return -1;
    };

    Array.prototype.intersection = function (arr) {
        var t1 = this, t2 = arr;
        if (t1.length > t2.length) {
            var t = t1;
            t1 = t2;
            t2 = t;
        }
        var reg = new RegExp("(," + t2.join(",)|(,") + ",)", "g");
        var ma = ("," + t1.join(",,") + ",").match(reg);
        var result = [];
        if (ma != null) {
            for (var i = 0; i < ma.length; i++) {
                result.push(ma[i].substr(1, ma[i].length - 2));
            }
        }
        return result.distinct();
    };

    Array.prototype.distinct = function () {
        /// <summary>
        /// 只接受数字和字符串数组的 distinct
        /// jquery 的 unique 在IE下对非DOM数组不起作用
        /// </summary>

        var o = {};
        for (var i = 0; i < this.length; i++) {
            var t = typeof (this[i]);
            if (t == "number" || t == "string") {
                o[this[i]] = 1;
            } else {
                throw new Exception("不支持非字母或数字数组的 Distinct");
            }
        }

        var arr = [];
        for (var oo in o) {
            arr.push(oo);
        }

        return arr;
    };


    String.prototype.GetLength = function () { return this.replace(/[^\x00-\xff]/g, 'xx').length; };
    String.prototype.trim = function () { return this.replace(/(^\s*)|(\s*$)/g, ""); };
    String.prototype.repeat = function (n) {
        var arr = new Array(n + 1);
        return arr.join(this);
    };
    String.prototype.padLeft = function (n, s) {
        // IE 不接受 substar 的第一个参数为负数的
        //return (s.repeat(n) + this).substr(-n);
        if ("000".substr(-2) != "000")
            return (s.repeat(n) + this).substr(-n);
        else
            return (s.repeat(n) + this).match(new RegExp("([\\s\\S]{" + n + "}$)"))[0];
    };
    String.prototype.padRight = function (n, s) {
        return (this + s.repeat(n)).substring(0, n);
    };

    String.prototype.toDateExact = function (fmt, defaultDate) {
        /// <summary>
        /// 按格式转换成日期
        /// </summary>
        /// <param name="fmt">日期格式</param>
        /// <param name="defaultDate"></param>

        defaultDate = defaultDate == undefined ? null : defaultDate;

        var ps = [];
        fmt = fmt.replace(/((yyyy)|(yy)|(MM)|(M)|(dd)|(d)|(HH)|(H)|(hh)|(h)|(mm)|(m)|(ss)|(s))/g, function (k) {
            ps.push(k.split("")[0]);
            if (k.length == 1) {
                return "(\\d{1,2})";
            } else {
                return "(\\d{" + k.length + "})";
            }
        });

        var year = month = day = hour = second = minute = 1;

        var reg = new RegExp(fmt);
        var ms = this.match(reg);
        if (ms != null) {
            for (var i = 1; i < ms.length; i++) {
                var p = ps[i - 1];
                var v = ms[i];
                switch (p) {
                    case "y":
                        year = v;
                        break;
                    case "M":
                        month = v;
                        break;
                    case "d":
                        day = v;
                        break;
                    case "H":
                        hour = v;
                        break;
                    case "m":
                        minute = v;
                        break;
                    case "s":
                        second = v;
                        break;
                }
            }

            //不能直接使用
            //return new Date(year,month,day,hour,minute,second);
            return (year + "/" + month + "/" + day).toDate(defaultDate);
        } else {
            return defaultDate;
        }
    };

    String.prototype.toDate = function (defaultDate) {
        /// <summary>
        /// 将给定的字符串转换为日期, defaultDate 默认可以不填,返回 null
        /// </summary>
        /// <param name="defaultDate" type="Date">转换失败时,要返回的默认值</param>
        /// <returns type="Date"></returns>
        if (defaultDate != null && !(defaultDate instanceof Date)) {
            defaultDate = defaultDate.toString().toDate();
        }

        if (defaultDate == undefined)
            defaultDate = null;

        var date = null;
        var arr = new Array();
        if (this.indexOf("-") != -1) {
            arr = this.toString().split("-");
        } else if (this.indexOf("/") != -1) {
            arr = this.toString().split("/");
        } else {
            return defaultDate;
        }

        if (arr.length != 3)
            return defaultDate;

        //yyyy-mm-dd || yyyy/mm/dd
        if (arr[0].length == 4) {
            date = new Date(arr[0], arr[1] - 1, arr[2]);
            if (date.getFullYear() == arr[0] && date.getMonth() == arr[1] - 1 && date.getDate() == arr[2]) {
                return date;
            }
        }
        //dd-mm-yyyy || dd/mm/yyyy
        if (arr[2].length == 4) {
            date = new Date(arr[2], arr[1] - 1, arr[0]);
            if (date.getFullYear() == arr[2] && date.getMonth() == arr[1] - 1 && date.getDate() == arr[0]) {
                return date;
            }
        }
        //mm-dd-yyyy || mm/dd/yyyy
        if (arr[2].length == 4) {
            date = new Date(arr[2], arr[0] - 1, arr[1]);
            if (date.getFullYear() == arr[2] && date.getMonth() == arr[0] - 1 && date.getDate() == arr[1]) {
                return date;
            }
        }

        return defaultDate;
    };

    String.prototype.isDate = function () {
        /// <summary>
        /// 给定的字符串是否可以转换为日期
        /// </summary>
        /// <returns type=""></returns>
        return this.toDate() != null;
    };

    String.prototype.asMoney = function (decimalDigits, defaultValue) {
        /// <summary>
        /// 数字转换为金额表示
        /// </summary>
        /// <param name="decimalDigits">小数位</param>
        /// <param name="defaultValue">转换失败时的默认值</param>
        /// <returns type=""></returns>
        if (defaultValue == null)
            defaultValue = "";

        if (isNaN(decimalDigits)) {
            decimalDigits = 2;
        }

        var reg = /^([+-]?)(\d*)\.?(\d*)$/;

        if (!reg.test(this.trim()))
            return defaultValue;

        var match = this.trim().match(reg);
        if (match[2] + match[3] == "")
            return defaultValue;

        var part3 = match[3];
        if (part3.length < decimalDigits) {
            part3 = part3 + "0".repeat(decimalDigits - match[3].length);
        } else {
            part3 = part3.substring(0, decimalDigits);
        }

        var sign = match[1];
        if (sign != "-")
            sign = "";

        if (match[2] != "")
            return sign + match[2].split('').reverse().join('').replace(/(.{3}(?=.))/g, "$1,").split('').reverse().join('') + (part3 != "" ? "." + part3 : "");
        else
            return sign + "0." + part3;
    };

    var __dateToString__ = Date.prototype.toString;
    /*仅支持以下格式：MM２长度月，yyyy四长度年，yy２长度年，dd２长日期, t / T AM or PM (小写返回小，大写返回大写)
	HH 2长度小时(24) hh2长度小时（12进制） mm 2长度分钟,ss 2长度秒
	M 1和2长度月, d 1或２长度日期 m,h,s 同上
	*/
    Date.prototype.toString = function (fmt) {
        var date = this;
        if (!fmt)
            return __dateToString__.apply(this);

        var month = date.getMonth() + 1;
        var year = date.getFullYear();

        fmt = fmt.replace(/((yyyy)|(yy)|(MM)|(M)|(dd)|(d)|(HH)|(H)|(hh)|(h)|(mm)|(m)|(ss)|(s)|(T)|(t))/g, function (key) {
            //var leng = ma.length;
            switch (key) {
                case "yyyy":
                    return year;
                    break;
                case "yy":
                    return year % 100;
                    break;
                case "MM":
                    return month.toString().padLeft(2, "0");
                    break;
                case "M":
                    return month;
                    break;
                case "dd":
                    return date.getDate().toString().padLeft(2, "0");
                    break;
                case "d":
                    return date.getDate();
                    break;
                case "HH":
                    return date.getHours().toString().padLeft(2, "0");
                    break;
                case "H":
                    return date.getHours();
                    break;
                case "hh":
                    return (date.getHours() % 12).toString().padLeft(2, "0");
                    break;
                case "h":
                    return date.getHours() % 12;
                    break;
                case "mm":
                    return date.getMinutes().toString().padLeft(2, "0");
                    break;
                case "m":
                    return date.getMinutes();
                    break;
                case "ss":
                    return date.getSeconds().toString().padLeft(2, "0");
                    break;
                case "s":
                    return date.getSeconds();
                    break;
                case "t":
                    return date.getHours() > 11 ? "pm" : "am";
                    break;
                case "T":
                    return date.getHours() > 11 ? "PM" : "AM";
                    break;
                default:
                    return key;
            }
        });
        return fmt;
    };

    Date.prototype.add = function (datePart, v) {
        /// <summary>
        /// 返回新值，並且不改變舊值 datePart yy:year, m:month, d: date,h:hour, mi:minute, s:second, ms:毫秒
        /// </summary>
        /// <param name="datePart"></param>
        /// <param name="v"></param>
        /// <returns type=""></returns>
        var reg = /^((yy)|(m)|(d)|(h)|(mi)|(s)|(ms))$/i;
        if (!reg.test(datePart)) {
            throw new Error("Invalid argument datePart, yy:year, m:month, d: date,h:hour, mi:minute, s:second, ms:millisecond");
        }

        if (isNaN(v)) {
            return this;
        }

        // Date 是地址引用
        var tmp = new Date(this.valueOf());

        var key = datePart.match(reg)[0].toUpperCase();
        var vv = this.valueOf();
        switch (key) {
            case "YY":
                vv = tmp.setFullYear(this.getFullYear() + v);
                break;
            case "M":
                vv = tmp.setMonth(this.getMonth() + v);
                break;
            case "D":
                vv = tmp.setDate(this.getDate() + v);
                break;
            case "H":
                vv = tmp.setHours(this.getHours() + v);
                break;
            case "MI":
                vv = tmp.setMinutes(this.getMinutes() + v);
                break;
            case "S":
                vv = tmp.setSeconds(this.getSeconds() + v);
                break;
            case "MS":
                vv = tmp.setMilliseconds(this.getMilliseconds() + v);
                break;
        }
        return new Date(vv);
    };

    //Trim
    String.prototype.trimStart = function (trimStr) {
        if (!trimStr) { return this; }
        var temp = this;
        while (true) {
            if (temp.substr(0, trimStr.length) != trimStr) {
                break;
            }
            temp = temp.substr(trimStr.length);
        }
        return temp;
    };
    String.prototype.trimEnd = function (trimStr) {
        if (!trimStr) { return this; }
        var temp = this;
        while (true) {
            if (temp.substr(temp.length - trimStr.length, trimStr.length) != trimStr) {
                break;
            }
            temp = temp.substr(0, temp.length - trimStr.length);
        }
        return temp;
    };

})();


var ImageHelper = {};
(function (_) {

    _.NotFound = function (type, obj) {
        var width = obj.width;
        var height = obj.height;
        var src;
        switch (type) {
            case 0:
                src = SITEROOT + "/Content/imgs/not_found_140x105.jpg";
                break;
            case 1:
                src = SITEROOT + "/Content/imgs/not_found_140x105.jpg";
                break;
            case 2:
                src = SITEROOT + "/Content/imgs/not_found_245x115.gif";
                break;
            case 3:
                src = SITEROOT + "/Content/imgs/not_found_70.jpg";
                break;
        }
        //曲线救国, Chrome 下,如果单纯的只是设置 obj.src ,是不会加载新图片的
        var img = new Image();
        img.src = src;
        img.width = width;
        img.height = height;
        $(obj).parent().append(img);
        $(obj).hide();
    };

    _.Reload = function (oldImg) {
        var src = oldImg.src;
        var img = new Image();
        img.src = src + (src.indexOf("?") == -1 ? "?" : "&") + "tmp=" + (new Date()).valueOf().toString(32);
        img.width = oldImg.width;
        img.height = oldImg.height;
        $(oldImg).parent().append(img);
        $(oldImg).remove();
        img.onclick = doFunction(ImageHelper.Reload, img);
    }

})(ImageHelper);


var HtmlHelper = {};
(function (o) {
    $.fn.smartFloat = function () {
        var position = function (element) {
            var top = element.offset().top, pos = element.css("position");
            $(window).scroll(function () {
                var scrolls = $(this).scrollTop();
                if (scrolls > top) {
                    if (window.XMLHttpRequest) {
                        element.css({ position: "fixed", top: 0 });
                    } else {
                        element.css({ top: scrolls });
                    }
                } else {
                    //element.css({position: pos,top: top});	
                    element.removeAttr("style");
                }
            });
        };
        return $(this).each(function () {
            position($(this));
        });
    };

    o.EnableSubmit = function () {
        $(":submit").each(function (idx, item) {
            var txt = $(item).attr("__txt");
            if (item.tagName == "BUTTON")
                $(item).attr("disabled", false).html(txt);
            else if (item.tagName == "INPUT")
                $(item).attr("disabled", false).val(txt);
        });
        $(document).unbind(".cancel");
    };

    o.DisableSubmit = function () {
        $(":submit").each(function (idx, item) {
            if (item.tagName == "BUTTON")
                $(item).attr({ "__txt": $(item).html(), disabled: true })
					.html("處理中,請稍候...");
            else if (item.tagName == "INPUT")
                $(item).attr({ "__txt": $(item).val(), disabled: true })
					.val("處理中,請稍候...");
        });
        $(document).bind("keydown.cancel", function (e) {
            if (e.keyCode == 27) {
                o.EnableSubmit();
            }
        });
    };

    o.EnableRegSubmit = function () {
        $("#btnSubmit").each(function (idx, item) {
            var txt = $(item).attr("__txt");
            if (item.tagName == "BUTTON")
                $(item).attr("disabled", false).html(txt);
            else if (item.tagName == "INPUT")
                $(item).attr("disabled", false).val(txt);
        });
        $(document).unbind(".cancel");
    };

    o.DisableRegSubmit = function () {
        $("#btnSubmit").each(function (idx, item) {
            if (item.tagName == "BUTTON")
                $(item).attr({ "__txt": $(item).html(), disabled: true })
					.html("處理中,請稍候...");
            else if (item.tagName == "INPUT")
                $(item).attr({ "__txt": $(item).val(), disabled: true })
					.val("處理中,請稍候...");
        });
        $(document).bind("keydown.cancel", function (e) {
            if (e.keyCode == 27) {
                o.EnableSubmit();
            }
        });
    };

    $(window).bind("beforeunload", function () {
        o.DisableSubmit();
    }).bind("unload", function () {
        o.EnableSubmit();
    });

    $('a').on('click', function (e) {
        //$('a').live('click', function (e) {
        var jslinkstart = "javascript:";
        var thisLinkHref = $(this).attr('href') || "";
        var thisLinkJs = thisLinkHref.substring(jslinkstart.length);
        if (thisLinkHref) {
            var thisLinkHrefStart = thisLinkHref.substring(0, jslinkstart.length);
            if (thisLinkHrefStart == jslinkstart) {
                e.preventDefault();
                eval(thisLinkJs);
            }
        }
    });

    var uniqueID = function () {
        return ((((new Date).valueOf() + Math.random()) * 1000000) | 0).toString(36);
    };

    var HideRadio = function (item) {
        item = $(item);
        var radioUid = item.attr("radioUid");
        if (!radioUid) {
            radioUid = uniqueID();
            item.attr("radioUid", radioUid);
        }
        $("<em class='selected' radioUid='" + radioUid + "'></em>").insertBefore(item);
        item.css({ display: "none" });
    };

    var GreenRadio = function (radio) {
        var name = $(radio).attr("name").replace(/\./g, "\\.")
        var radios = $("input[type='radio'][name='" + name + "']")
        radios.each(function (idx, item) {
            item = $(item);
            var radioUid = item.attr("radioUid");
            if (radioUid) {
                $("em[radioUid='" + radioUid + "']").remove();
                item.css("display", "");
            }
        });

        HideRadio(radio);
    };

    var ClickRadio = function (event) {
        GreenRadio(this);
    };

    o.SetRadio = function () {
        var radio = $("input[type='radio']:checked:visible");
        radio.each(function (idx, item) {
            HideRadio(item);
        });
        $("input[type='radio']").off("click", ClickRadio).on("click", ClickRadio);
    };

    o.ResetRadio = function () {
        //$("input[type='radio']:hidden:not(:checked) ~ em").remove();
        $("input[type='radio']:hidden:not(:checked)").css("display", "").each(function (idx, item) {
            var radioUid = $(item).attr("radioUid");
            if (radioUid) {
                $("em[radioUid='" + radioUid + "']").remove();
            }
        });
    };

})(HtmlHelper);


var ValidationHelper = {};
(function (o) {

    o.Invalidate = function (form, validator) {

    };

    var Helper = function (form) {

        var setting = null;
        var errorPlacement = null;
        var success = null;

        this.ready = function (mode) {
            $(document).ready(function () {
                try {
                    var obj = $.data(form, 'validator');
                    if (obj) {
                        setting = $.data(form, 'validator').settings;
                        setting.ignore = "input[type='hidden']";
                        errorPlacement = setting.errorPlacement;
                        success = setting.success;
                        setting.errorPlacement = $.proxy(mode3_error, form);
                        setting.success = $.proxy(mode3_success, form);

                        $(form).unbind("invalid-form.validate")
							.bind("invalid-form.validate", function () {
							    o.Invalidate(this, arguments[1]);
							});
                    }

                } catch (e) {
                }
            });
        };


        var mode3_error = function (error, inputElement) {
            $(inputElement).parent().addClass("has-error");
            $(inputElement).tooltip({ title: error, html: true });
        };

        var mode3_success = function (error, inputElement) {
            $(inputElement).tooltip('destroy');
            $(inputElement).parent().removeClass("has-error");
            $("span[for='" + inputElement.name + "']").parent().parent().remove();
        };
    };

    $("form").each(function (idx, form) {
        var helper = new Helper(form);
        helper.ready();
    });

    var checkDate = function (value, element, params) {
        var fmt = params["format"] || "";
        if (fmt.trim() != "") {
            return this.optional(element) || value.toDateExact(fmt) != null;
        } else
            return this.optional(element) || value.toDate() != null;
    };

    var mustChecked = function (value, element) {
        return $(element).is(':checked');
    };

    var dateRange = function (value, element, params) {
        if (this.optional(element)) {
            return true;
        }

        var startDate = (params["min"] || "0001/1/1").toDate(); //$(element).attr("data-val-dateRange-min").toDate();
        var endDate = (params["max"] || "9999/12/31").toDate(); //$(element).attr("data-val-dateRange-max").toDate();
        var format = params["format"];
        var enteredDate = format == "" ? value.toDate() : value.toDateExact(format);

        return ((startDate <= enteredDate) && (enteredDate <= endDate));
    };

    var requiredIf = function (value, element, params) {
        var prefix = getModelPrefix(element.name);
        var fullDependencyName = appendModelPrefix(params["dependency"], prefix);
        var dependency = $(element.form).find(":input[name='" + fullDependencyName + "']");
        if (dependency.length == 0)
            return true;

        var dependencyValue = params["dependencyvalue"];

        var acturalValue = null;
        if (dependency.attr("type") == "checkbox") {
            acturalValue = dependency.is(":checked") ? dependency.val().toLowerCase() : null;
        } else {
            acturalValue = dependency.val().toLowerCase();
        }

        eval("dvs = " + dependencyValue);
        if (dvs.indexOf2(acturalValue) >= 0) {
            return $.validator.methods.required.call(this, value, element, params);
        }

        return true;
    };

    var getModelPrefix = function (fieldName) {
        return fieldName.substr(0, fieldName.lastIndexOf(".") + 1);
    };

    var appendModelPrefix = function (value, prefix) {
        if (value.indexOf("*.") === 0) {
            value = value.replace("*.", prefix);
        }
        return value;
    };

    var dateAdapter = function (options) {
        options.rules['date'] = options.params;
        options.messages['date'] = "請輸入有效的日期";
    };

    var dateRangeAdapter = function (options) {
        options.rules['dateRange'] = options.params;
        options.messages['dateRange'] = options.message;
    };

    var mustCheckedAdapter = function (options) {
        options.rules['mustChecked'] = options.params;
        options.messages['mustChecked'] = options.message;
    };

    var requiredIfAdapter = function (options) {
        options.rules["requiredIf"] = options.params;
        options.messages["requiredIf"] = options.message;
    };

    if ($.validator != undefined) {
        var ms = $.validator.messages;
        ms["email"] = "請輸入有效的郵箱地址";
        ms["date"] = "請輸入有效的日期";

        $.validator.addMethod("date", checkDate);
        $.validator.addMethod("dateRange", dateRange);
        $.validator.addMethod('requiredIf', requiredIf);
        $.validator.addMethod('mustChecked', mustChecked);

        //$.validator.unobtrusive.adapters["date", { name: dateAdapter , params: ['format'] , adapt: dateAdapter }];
        $.validator.unobtrusive.adapters.add("date", ["format"], dateAdapter);
        $.validator.unobtrusive.adapters.add("dateRange", ['min', 'max', 'format'], dateRangeAdapter);
        $.validator.unobtrusive.adapters.add('requiredIf', ['dependency', 'dependencyvalue'], requiredIfAdapter);
        $.validator.unobtrusive.adapters.add('mustchecked', [], mustCheckedAdapter);
    }

})(ValidationHelper);


var PaginationHelper = {};
(function (p) {
    p.Set = function (name, page) {
        var hp = $(":hidden[name='" + name + "']");
        hp.val(page);
        hp.parents("form").submit();
        return false;
    }

    $(":submit").on("click", function () {
        $(":hidden[data-pager='true']").val("");
    });
})(PaginationHelper)


var PortHelper = {};
(function (p) {

    p.Company = 0;

    var render = function (ul, item) {
        return $("<li>")
			.data("item.autocomplete", item)
			.append("<a>" + item.Code + "<span class='badge pull-right'>" + item.NationNameCn + "</span></a>")
			.appendTo(ul);
    };

    var source = function (request, response) {
        $.ajax({
            url: "/" + LangHelper.Lang + "/Json/Port/" + p.Company,
            dataType: "jsonp",
            jsonpCallback: "callback",////////
            data: { q: request.term },
            cache: false
        }).done(function (data) {
            response(data);
        }).fail(function (httpRequest, textStatus, errorThrown) {
            if (console && console.log)
                console.log(errorThrown);
        });
    };


    $("input[data-xxy-port='True']")
		.each(function (idx, item) {
		    $(item).autocomplete({
		        source: source,
		        select: function (event, ui) {
		            $(item).val(ui.item.Code);
		            return false;
		        }
		    })
			.data("ui-autocomplete")._renderItem = render;
		});

})(PortHelper);


JR = window.JR || {};
JR.Common = JR.Common || {};
(function (obj, $) {
    //搜索框提示文字变化方法
    obj.SearchBoxContent = function (objId, objText) {
        //查询框提示文字
        if ($("#" + objId).val() == "" || $("#" + objId).val() == objText) {
            $("#" + objId).val(objText).addClass("inputGrayColor");
        } else {
            $("#" + objId).addClass("inputBlackColor");
        }
        $("#" + objId).focus(function () {
            if ($(this).val() == objText) {
                $(this).val("");
                $(this).removeClass("inputGrayColor").addClass("inputBlackColor");
            }
        });
        $("#" + objId).blur(function () {
            if ($(this).val() == "") {
                $(this).val(objText);
                $(this).addClass("inputGrayColor").removeClass("inputBlackColor");
            }
        });
    };

    obj.ValidateTabHelpler = function (res) {
        //判断页签内是否有异常
        $(".nav-tabs").each(function () {
            $(this).find("a").each(function () {
                var myId = $(this).attr("href").replace("#", "");
                for (var i = 0; i < res.errorList.length; i++) {
                    if ($("#" + res.errorList[i].element.id).closest("div[id=" + myId + "]").length > 0) {
                        if ($("#tips_" + myId).length == 0) {
                            if (!document.all) {
                                $(this).append("<div id='tips_" + myId + "' class=\"tipImageBg tipserror\"></div>");
                            } else {
                                $(document.body).append("<div id='tips_" + myId + "'  class='tips-main tipImageBg tipserror'></div>");
                                var top = $(this).offset().top + $(this).outerHeight() - 65;
                                var left = $(this).offset().left + 20;
                                if (!document.all) {
                                    left += 7;
                                }
                                $("#tips_" + myId).css({ "top": top + "px", "left": left + "px" });
                            }
                        }
                    }
                }
            });
        });
    };

    obj.GridTableCheck = function (id) {
        $("#" + id + " input[type=checkbox]").click(function () {
            if ($(this).attr("head") == "head") {
                if (this.checked) {
                    $("#" + id + " input[type=checkbox]").each(function () {
                        if (this.disabled == false) {
                            this.checked = true;
                        }
                    });
                } else {
                    $("#" + id + " input[type=checkbox]").each(function () {
                        this.checked = false;
                    });
                }
            } else {
                var allCheck = $("#" + id + " input[type=checkbox][head=head]");
                var checkedCount = 0;
                var childCount = 0;
                $("#" + id + " input[type=checkbox]").each(function () {
                    if (!$(this).attr("head")) {
                        if (this.disabled == false) {
                            if (this.checked == true) {
                                checkedCount++;
                            }
                            childCount++;
                        }
                    }
                });
                if (checkedCount == childCount) {
                    allCheck[0].checked = true;
                } else {
                    allCheck[0].checked = false;
                }
            }
        });

    };

})(JR.Common, jQuery);


JR.Common.zTree = JR.Common.zTree || {};
(function (obj, $) {

    var zTreeNodeClick = function (event, treeId, treeNode, clickFlag) {
        setTimeout(function () {
            if (JR.Common.zTree.nodeClickCallBack) {
                JR.Common.zTree.nodeClickCallBack(event, treeId, treeNode, clickFlag);
            }
        }, 10);
    };

    //jquery tree控件
    var setting = {
        view: {
            dblClickExpand: true,
            showLine: true,
            selectedMulti: false
        },
        data: {
            simpleData: {
                enable: true,
                idKey: "id",
                pIdKey: "pId",
                rootPId: ""
            }
        },
        callback: {
            onClick: zTreeNodeClick
        }
    };

    //jquery tree控件
    var settingCheckBox = {
        view: {
            dblClickExpand: true,
            showLine: true,
            selectedMulti: false
        },
        data: {
            simpleData: {
                enable: true,
                idKey: "id",
                pIdKey: "pId",
                rootPId: ""
            }
        },
        check: {
            enable: true,
            chkStyle: "checkbox",
            chkboxType: {
                "Y": "ps",
                "N": "ps"
            }
        },
        callback: {
            onClick: zTreeNodeClick
        }
    };

    obj.LoadTree = function (containerId, zNodes, type) {
        var checkType = setting;
        if (type == "checkbox") {
            checkType = settingCheckBox;
        }
        $.fn.zTree.init($("#" + containerId), checkType, zNodes);
    };



})(JR.Common.zTree, jQuery);





var Treegrid = {};
(function (t) {

    var self = this;

    var dealTree = function (tab) {
        var rows = tab.rows;
        var row;

        var treeObjs = {};
        var tmp = {};

        for (var i = 0; row = rows[i]; i++) {
            var id = $(row).attr("data-treegrid-id");
            var pId = $(row).attr("data-treegrid-parentid");
            var seq = $(row).attr("data-treegrid-seqence") || 0;

            if (id == undefined || pId == undefined)
                continue;

            if (!tmp[pId])
                tmp[pId] = { id: pId };

            if (!tmp[id])
                tmp[id] = { hasChild: false };

            tmp[id].id = id;
            tmp[id].pID = pId;
            tmp[id].sequence = seq;

            tmp[pId][id] = tmp[id];
            tmp[pId].hasChild = true;
        }

        for (var t in tmp) {
            if (tmp[t].pID == undefined) {
                treeObjs[t] = tmp[t];
            }
        }

        return treeObjs;
    };

    var getSubIds = function (treeObjs, arr) {
        for (var t in treeObjs) {
            if (typeof (treeObjs[t]) != "object")
                continue;

            arr.push(t);

            if (treeObjs[t].hasChild)
                getSubIds(treeObjs[t], arr);
        }
        return arr;
    };


    var sort = function (left, right) {
        /// <summary>
        /// 只支持按数字方式和字符方式排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns type=""></returns>
        var x = left.sequence, y = right.sequence;
        if (!isNaN(left.sequence) && !isNaN(right.sequence)) {
            x = parseInt(left.sequence);
            y = parseInt(right.sequence);
        }

        return x > y ? 1 : (x < y ? -1 : 0);
    }

    var sortTree = function (treeObjs) {

        var arr = [];
        for (var o in treeObjs) {
            if (typeof treeObjs[o] != "object")
                continue;

            arr.push(treeObjs[o]);
        }

        return arr.sort(sort);
    }

    var generate = function (treeObjs, tab, tbody, level) {

        var a = sortTree(treeObjs);

        //for (var t in treeObjs) {
        var node;
        for (var i = 0; node = a[i]; i++) {
            //var node = treeObjs[t];

            if (typeof (node) != "object")
                return;

            var t = node.id;

            var row = $(tab).find("tr[data-treegrid-id=" + t + "]");
            var td = row.find("td:eq(0)");
            var indents = new Array(level).join("<span class='treegridIndent'></span>");

            var htmls = [indents];

            if (node.hasChild) {
                var subIds = getSubIds(node, new Array());
                htmls.push("<span data-treegrid-subs='" + subIds.join(",") + "' class='glyphicon glyphicon-chevron-down'></span>")
            }
            htmls.push(td.html());

            td.html(htmls.join(""));
            tbody.append(row);
            generate(node, tab, tbody, level + 1);
        }
    };

    var init = function (tab) {
        if ($(tab).attr("data-treegrid-dealed") == true)
            return;

        var treeObjs = dealTree(tab);

        var tbody = $("<tbody>");

        generate(treeObjs, tab, tbody, 0);
        $(tab).append(tbody);
        $(tab).attr("data-treegrid-dealed", true);
    };

    $.fn.treegrid = function () {
        this.each(function () {
            init(this);
        });

        $("span[data-treegrid-subs]").on("click", function () {
            var subIds = $(this).attr("data-treegrid-subs").split(',');
            var explanded = $(this).data("data-treegrid-expandend");
            explanded = explanded == undefined ? true : explanded;
            var id;
            for (var i = 0; id = subIds[i]; i++) {
                var tr = $("tr[data-treegrid-id='" + id + "']");
                tr.toggle(!explanded);
            }
            $(this)
				.data("data-treegrid-expandend", !explanded)
				.removeClass("glyphicon-chevron-right").removeClass("glyphicon-chevron-down")
				.addClass(explanded ? "glyphicon glyphicon-chevron-right" : "glyphicon glyphicon-chevron-down");
        });
    };

})(Treegrid);






function BaseSelector() {
    var s = this;

    s.ChoicedDatas = [];

    var Choiced = function () {
        /// <summary>
        /// 已选择的数据
        /// </summary>

        var result = [];
        var data = null;
        for (var i = 0; data = s.ChoicedDatas[i]; i++) {
            result.push(data.id);
        }
        return result;
    }

    s.OnChoice = function (data) {
        /// <summary>
        /// 选中时,需要在页面内实现
        /// </summary>
        /// <param name="data"></param>
    }

    s.OnUnChoice = function (data) {
        /// <summary>
        /// 取消选择时,需要在页面内实现
        /// </summary>
        /// <param name="data"></param>
    }

    var observers = [];
    s.AddObserver = function (o) {
        observers.push(o);
        o.Notify(Choiced())
    }

    s.Notify = function () {
        var o;
        var choiced = Choiced();
        for (var i = 0; o = observers[i]; i++) {
            try {
                o.Notify(choiced);
            } catch (e) { }
        }
    }


    s.SelectorFor = null;
    s.SelectorForAssist = null;

    var reg = /^(id|name|type|value|class|data-val-number)$/i;
    //COPY属性
    $("[data-xxy-selector]").each(function (idx, ele) {

        var assist = $("[name='" + ele.name + "_assist']");
        var attr;
        for (var i = 0; attr = ele.attributes[i]; i++) {
            if (reg.test(attr.name))
                continue;

            assist.attr(attr.name, attr.value);
        }

    });

    s.BeforeOpenDialog = function (obj) {
        var forName = $(obj).attr("data-xxy-selector-for");
        s.SelectorFor = $("[name='" + forName + "']");
        s.SelectorForAssist = $("#" + forName.replace(".", "_") + "_assist");
        if (s.SelectorForAssist.length == 0)
            s.SelectorForAssist = $("[name='" + forName + "_assist']");
    }
}







var MemberSelector = {};
(function (m) {
    BaseSelector.call(m);

    var openDialog = function () {
        m.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, "/User/Search", null);
    }

    $("[data-xxy-selector='Member']")
		.on("click", $.proxy(openDialog, this));

})(MemberSelector);


var CompanySelector = {};
(function (m) {
    BaseSelector.call(m);

    var openDialog = function () {
        m.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, "/Company/QueryCompanyNotExistsAdmin", null);
    }

    $("[data-xxy-selector='Company']")
        .each(function (idx, ele) {
            var isReadonly = $("[name='" + $(ele).attr("data-xxy-selector-for") + "']").prop("readonly");
            if (!isReadonly)
                $(ele).on("click", $.proxy(openDialog, this));
        });
		

})(CompanySelector);




//异步上传（单文件）

function BaseSingleUploader() {
    var u = this;

    //m.UploadUrl = '@Url.Action("UploadFile", "Exhibition", new { area = "Museum", exhId = exhid })';
    //m.CancelUploadUrl = '@Url.Action("DeleteFileByName", "Exhibition", new { area = "Museum", exhId = exhid })';

    //m.UploadDom = $('.uploadTrigger');
    //m.ResultDom = $('#tbl');
    u.BtnText = null;
    u.MaxFileSize = 5242880;//Max 5Mb file 1kb=1024字节

    u.UploadUrl = null;
    u.CancelUploadUrl = null;

    u.Sequence = 0;

    u.UploadDom = null;
    u.ResultDom = null;

    u.Bind = function (cur) {
        //隐藏显示图片的表格
        u.ResultDom.hide();

        u.UploadDom.JSAjaxFileUploader({
            uploadUrl: u.UploadUrl,
            formData: { sequence: u.Sequence },
            inputText: u.BtnText,
            maxFileSize: u.MaxFileSize,//Max 5Mb file 1kb=1024字节
            allowExt: 'gif|jpg|jpeg|png',
            zoomPreview: true,
            zoomWidth: 800,
            zoomHeight: 600,
            beforesend: function (file) {
                if (u.ResultDom.find('.imgName').text() != "") {
                    deleteImg();
                    u.ResultDom.hide();
                }
            },
            success: function (data) {
                $('.file_name').html(data.name);
                $('.file_type').html(data.type);
                $('.file_size').html(data.size);
                $('.file_path').html(data.path);
                $('.file_msg').html(data.msg);
                $('.modal').modal({ backdrop: false, show: true });
                setTimeout(function () {
                    $('.modal').modal("hide");
                }, 1500)

                u.createTableTr(u.ResultDom);

                u.ResultDom.show();

                u.ResultDom.find('.showImg').attr("src", data.path);
                u.ResultDom.find('.imgName').text(data.name);
            },
            error: function (data) {
                alert(data.msg);
            }
        });

        //点击删除链接删除刚上传图片
        u.ResultDom.on("click", ".delImg", function () {
            u.deleteImg(u.ResultDom);
            //window.location.reload();
        });
    }
    u.createTableTr = function (table) {
        table.append("<tr><td><img class='showImg img-thumbnail'/></td><td colspan='2'><span class='imgName'></span></td><td><a class='delImg btn btn-danger'  href='javascript:void(0)'>删除</a></td></tr>");
     }
    u.deleteImg = function (table) {
        $.ajax({
            cache: false,
            url: u.CancelUploadUrl,
            type: "POST",
            data: { filename: table.find('.imgName').text(), sequence: u.Sequence },
            success: function (data) {
                if (data.msg) {
                    //alert("图片删除成功");
                    table.find('.delImg').parent().parent().remove();
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert("出错了 '" + jqXhr.status + "' (状态: '" + textStatus + "', 错误为: '" + errorThrown + "')");
            }
        });

    }
 
    u.BeforeOpenDialog = function (obj) {

    }
}

//var SingleUploader = {};
//(function (m) {
//    BaseSingleUploader.call(m);

//    m.UploadUrl = '';
//    m.CancelUploadUrl = '';

//    m.UploadDom ;
//    m.ResultDom ;

//    // m.OnChoice

//})(SingleUploader);






////jquery 插件，支持标签级别onresize
//(function($, h, c) {
//    var a = $([]), e = $.resize = $.extend($.resize, {}), i, k = "setTimeout", j = "resize", d = j + "-special-event", b = "delay", f = "throttleWindow";
//    e[b] = 250;
//    e[f] = true;
//    $.event.special[j] = {
//        setup: function() {
//            if (!e[f] && this[k]) {
//                return false;
//            }
//            var l = $(this);
//            a = a.add(l);
//            $.data(this, d, { w: l.width(), h: l.height() });
//            if (a.length === 1) {
//                g();
//            }
//        },
//        teardown: function() {
//            if (!e[f] && this[k]) {
//                return false;
//            }
//            var l = $(this);
//            a = a.not(l);
//            l.removeData(d);
//            if (!a.length) {
//                clearTimeout(i);
//            }
//        },
//        add: function(l) {
//            if (!e[f] && this[k]) {
//                return false;
//            }
//            var n;

//            function m(s, o, p) {
//                var q = $(this), r = $.data(this, d);
//                r.w = o !== c ? o : q.width();
//                r.h = p !== c ? p : q.height();
//                n.apply(this, arguments);
//            }

//            if ($.isFunction(l)) {
//                n = l;
//                return m;
//            } else {
//                n = l.handler;
//                l.handler = m;
//            }
//        }
//    };

//    function g() {
//        i = h[k](function() {
//            a.each(function() {
//                var n = $(this), m = n.width(), l = n.height(), o = $.data(this, d);
//                if (m !== o.w || l !== o.h) {
//                    n.trigger(j, [o.w = m, o.h = l]);
//                }
//            });
//            g();
//        }, e[b]);
//    }
//})(jQuery, this);