//"use strict";

if (window.GlobalDatas == undefined) {
    window.GlobalDatas = { RootCompanyID: 0 };
}

//语言全局变量
var LangHelper = window.LangHelper || {};
(function (l) {
    l.Lang = "zh-CN";
    l["zh-CN"] = {
        OK: "确定",
        Close: "关闭",
        Cancel: "取消",
        AlertTitle: "页面提示信息",
        Yes: "是",
        No: "否"
    };
    l["en-US"] = {
        OK: "Sure",
        Close: "Close",
        Cancel: "Cancel",
        AlertTitle: "Page message",
        Yes: "Yes",
        No: "No"
    };
    l["zh-TW"] = {
        OK: "確定",
        Close: "關閉",
        Cancel: "取消",
        AlertTitle: "頁面提示信息",
        Yes: "是",
        No: "否"
    };
})(LangHelper);



/*重写window.alert方法*/
(function (_) {

    _.__alert__ = window.alert;

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


    _.automap = function (data, type, writeNotExistsProperty, maps, callback, propertyWrap) {
        /// <summary>
        /// 将 Json 对象转换为目标类型的对象
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="type">目标类型</param>
        /// <param name="writeNotExistsProperty">如果目标类型中不存在对应的属性,是否写入,默认不写入</param>
        /// <param name="maps">属性映射列表,如{PropertyA:'PA',PropertyB:'B',...}</param>
        /// <returns type="">目标类型的数据,如果原数据是数组,结果目标类型的数组</returns>
        if (data == null)
            return null;

        if (data instanceof Array) {
            var item, result = [];
            for (var i = 0; item = data[i]; i++) {
                result.push(automap(item, type, writeNotExistsProperty, maps, callback, propertyWrap));
            }
            return result;
        } else if (data instanceof Object) {
            var result = new type();
            for (var k in data) {
                var targetProperty = k;
                var value = data[k];

                if (maps != null && maps[k] != undefined) {
                    if (typeof (maps[k]) == "string") {
                        //if (typeof (propertyWrap) == "function")
                        //    targetProperty = propertyWrap(maps[k])
                        //else
                        targetProperty = maps[k];
                    } else if (typeof (maps[k]) == "function") {
                        //TODO msWriteProfilerMark 是什么?
                        value = automap(value, maps[k], writeNotExistsProperty, null, callback, propertyWrap)
                    }
                } else if (typeof (propertyWrap) == "function") {
                    value = propertyWrap(value);
                }

                //不严格等于
                if (result[targetProperty] !== undefined || writeNotExistsProperty) {
                    result[targetProperty] = value;
                }
            }
            if (callback instanceof Function) {
                callback(result, data);
            }
            return result;
        }
    }

    _.ExtendTypeFromJson = function (json, type, propertyWrap) {
        for (var item in json) {
            var value = json[item];

            if (typeof (propertyWrap) == "function")
                type.prototype[item] = propertyWrap(value);
            else
                type.prototype[item] = value;
        }
    }

    _.ExtendConstrctorFromJson = function (json, type, propertyWrap) {

        var __type = type;
        type = function () {
            __type.call(this);


            for (var item in json) {
                var value = json[item];

                if (typeof (propertyWrap) == "function")
                    this[item] = propertyWrap(value);
                else
                    this[item] = value;
            }
        }

        return type;
    }


})(window);

(function () {
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
})();

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

        var year = 1, month = 1, day = 1, hour = 1, second = 1, minute = 1;

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
})();


var CookieHelper = {};
(function ($) {

    $.getExpires = function (y, m, d, h, i, s, ms) {
        var date = new Date();
        y = isNaN(y) ? date.getFullYear() : y;
        m = isNaN(m) ? date.getMonth() : m - 1;
        d = isNaN(d) ? date.getDate() : d;

        h = isNaN(h) ? date.getHours() : h;
        i = isNaN(i) ? date.getMinutes() : i;
        s = isNaN(s) ? date.getSeconds() : s;
        ms = isNaN(ms) ? date.getMilliseconds() : ms;

        return new Date(y, m, d, h, i, s, ms).toUTCString();
    }

    $.getExpiresByUTCString = function (UTCString) {
        var s = new Date(UTCString).toUTCString();
        if (s == 'NaN' || s == 'Invalid Date')
            return null; // IE,Opera NaN , FF,Safari Invalid Date;
        else
            return s;
    }


    $.set = function (k, v, expires, path, domain, secure) {
        var cookie = k + '=' + encodeURIComponent(v);

        if (expires) cookie += ";expires=" + expires;
        if (path) cookie += ";path=" + path;
        if (domain) cookie += ";domain=" + domain;
        if (secure) cookie += ";secure";
        document.cookie = cookie;
    }

    $.get = function (k) {
        var cks = document.cookie.split(';');
        var t;
        for (var i = 0; i < cks.length; i++) {
            t = cks[i].split('=');
            if (k == t[0].trim()) return decodeURIComponent(t[1]);
        }
    }

    $.remove = function (k) {
        $.set(k, '', $.getExpires(new Date().getFullYear() - 1));
    }

    $.empty = function () {
        var cks = document.cookie.split(';');
        var t;
        for (var i = 0; i < cks.length; i++) {
            $.remove(cks[i].split('=')[0].trim());
        }
    }
})(CookieHelper);

var ImageHelper = {};
(function (_) {

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

        var dvs = null;
        eval("dvs = " + dependencyValue);
        if (dvs.indexOf2(acturalValue) >= 0) {
            return $.validator.methods.required.call(this, value, element, params);
        }

        return true;
    };


    var mRemote = function (value, element, param) {
        //自定义 remote

        if (this.optional(element)) {
            return "dependency-mismatch";
        }

        var previous = this.previousValue(element),
            validator, data;

        if (!this.settings.messages[element.name]) {
            this.settings.messages[element.name] = {};
        }
        previous.originalMessage = this.settings.messages[element.name].remote;
        this.settings.messages[element.name].remote = previous.message;

        param = typeof param === "string" && { url: param } || param;

        if (previous.old === value) {
            return previous.valid;
        }

        previous.old = value;
        validator = this;
        this.startRequest(element);
        data = {};
        data[param.paramName] = value;
        $.ajax($.extend(true, {
            url: param,
            mode: "abort",
            port: "validate" + element.name,
            dataType: "json",
            data: data,
            context: validator.currentForm,
            success: function (response) {
                var valid = response === true || response === "true",
                    errors, message, submitted;

                validator.settings.messages[element.name].remote = previous.originalMessage;
                if (valid) {
                    submitted = validator.formSubmitted;
                    validator.prepareElement(element);
                    validator.formSubmitted = submitted;
                    validator.successList.push(element);
                    delete validator.invalid[element.name];
                    validator.showErrors();
                } else {
                    errors = {};
                    message = response || validator.defaultMessage(element, "remote");
                    errors[element.name] = previous.message = $.isFunction(message) ? message(value) : message;
                    validator.invalid[element.name] = true;
                    validator.showErrors(errors);
                }
                previous.valid = valid;
                validator.stopRequest(element, valid);
            }
        }, param));
        return "pending";
    }

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

    var splitAndTrim = function (value) {
        return value.replace(/^\s+|\s+$/g, "").split(/\s*,\s*/g);
    }

    var escapeAttributeValue = function (value) {
        // As mentioned on http://api.jquery.com/category/selectors/
        return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
    }

    var mRemoteAdapter = function (options) {
        var value = {
            url: options.params.url,
            type: options.params.type || "GET",
            data: {},
            paramName: options.params.paramname || ""
        },
            prefix = getModelPrefix(options.element.name);

        $.each(splitAndTrim(options.params.additionalfields || options.element.name), function (i, fieldName) {
            var paramName = appendModelPrefix(fieldName, prefix);
            value.data[paramName] = function () {
                return $(options.form).find(":input").filter("[name='" + escapeAttributeValue(paramName) + "']").val();
            };
        });

        //setValidationValues(options, "mRemote", value);

        options.rules["mRemote"] = value;
        if (options.message) {
            options.messages["mRemote"] = options.message;
        }
    };

    if ($.validator != undefined) {
        var ms = $.validator.messages;
        ms["email"] = "請輸入有效的郵箱地址";
        ms["date"] = "請輸入有效的日期";

        $.validator.addMethod("date", checkDate);
        $.validator.addMethod("dateRange", dateRange);
        $.validator.addMethod('requiredIf', requiredIf);
        $.validator.addMethod('mustChecked', mustChecked);
        $.validator.addMethod('mRemote', mRemote);

        //$.validator.unobtrusive.adapters["date", { name: dateAdapter , params: ['format'] , adapt: dateAdapter }];
        $.validator.unobtrusive.adapters.add("date", ["format"], dateAdapter);
        $.validator.unobtrusive.adapters.add("dateRange", ['min', 'max', 'format'], dateRangeAdapter);
        $.validator.unobtrusive.adapters.add('requiredIf', ['dependency', 'dependencyvalue'], requiredIfAdapter);
        $.validator.unobtrusive.adapters.add('mustchecked', [], mustCheckedAdapter);
        $.validator.unobtrusive.adapters.add("mRemote", ["url", "type", "additionalfields", "paramname"], mRemoteAdapter);
    }

    o.ManualCheck = function () {
        $("form").each(function (idx, form) {
            var helper = new Helper(form);
            helper.ready();
        });
    };

    o.ResetValidator = function () {
        //jquery.validate 的 validate 方法,大概在31行,会判断当前表单是 data('validator')是不是存在,如果存在,就不会用新的.
        $("form").each(function (idx, form) {
            $(form).data('validator', null);
            $.validator.unobtrusive.parse(form);
            new Helper(form).ready();
        })

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

    obj.ResetInputLayOut = function () {
        try {
            $("#panel_condition input, select").each(function () {
                if ($(this).attr("type") == "button") {
                    $(this).css({ "display": "inline" });
                } else {
                    $(this).css({ "width": "100px", "display": "inline" });
                }
                $(this).prev().removeClass("help-block");
                $(this).parent().removeClass("col-lg-4 col-md-4 col-xs-4").css({ "display": "inline", "margin-right": "20px" });
                if ($(this).parent().attr("class") == "input-group") {
                    $(this).parent().css({ "float": "left" });
                    $(this).parent().prev().css({ "float": "left", "padding-right": "5px" });
                    $(this).next().css({ "float": "left" });
                }
            });
        } catch (e) {
        }
    };

    obj.ManualValidate = function () {
        $("input[manualvalidate=true]").each(function () {
            $(this).blur(function () {
                obj.CheckOneData(this);
            });
        });
        obj.SubmitValidate();
    };

    obj.CheckOneData = function (e) {
        $(e).tooltip('destroy');
        $(e).parent().removeClass("has-error");
        //if ($.trim($(e).val()) == "") {
        //    $(e).parent().addClass("has-error");
        //    $(e).tooltip({ title: $(e).attr("data-val-required"), html: true });
        //} else
        if ($.trim($(e).val()) != "") {
            if (!(/^(\d{1,10})$/.test($(e).val()))) {
                $(e).parent().addClass("has-error");
                $(e).tooltip({ title: $(e).attr("data-val-number"), html: true });
            } else if ($.trim($(e).attr("maxCount") != "")) {
                var maxCount = parseInt($.trim($(e).attr("maxCount")));
                if (parseInt($(e).val()) > maxCount) {
                    $(e).parent().addClass("has-error");
                    $(e).tooltip({ title: $(e).attr("data-val-max"), html: true });

                }
            }
        }
    };

    obj.SubmitValidate = function () {
        $("form").on("submit", function () {
            $("input[manualvalidate=true]").each(function () {
                obj.CheckOneData(this);
            });
            if ($(".has-error").length > 0) {
                return false;
            } else {
                return true;
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

    //选择器的地址
    s.SelectorUrl = "";

    //
    s.Key = "id";

    s.ChoicedDatas = [];

    var Choiced = function () {
        /// <summary>
        /// 已选择的数据
        /// </summary>
        var result = [];
        var data = null;
        for (var i = 0; data = s.ChoicedDatas[i]; i++) {
            result.push(data[s.Key]);
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
        if (forName != undefined) {


            s.SelectorFor = $("[name='" + forName + "']");
            s.SelectorForAssist = $("#" + forName.replace(".", "_") + "_assist");
            if (s.SelectorForAssist.length == 0)
                s.SelectorForAssist = $("[name='" + forName + "_assist']");
        }
    }
}


var UserSelector = {};
(function (m) {
    BaseSelector.call(m);

    m.SelectorUrl = "/Selector/UserSelector";

    var openDialog = function () {
        m.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, m.SelectorUrl, null);
    }

    $("[data-xxy-selector='Member']")
		.on("click", $.proxy(openDialog, this));

    m.SetSelectorUrl = function (cmpID) {
        m.SelectorUrl = "/Selector/UserSelector?companyID=" + cmpID;
    }

})(UserSelector);


var CompanySelector = {};
(function (m) {
    BaseSelector.call(m);

    m.SelectorUrl = "/Company/Search";

    var openDialog = function () {
        m.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, m.SelectorUrl, null);
    }

    $("button[data-xxy-selector='Company']")
        .each(function (idx, ele) {
            var isReadonly = $("[name='" + $(ele).attr("data-xxy-selector-for") + "']").prop("readonly");
            if (!isReadonly)
                $(ele).on("click", $.proxy(openDialog, this));
        });


})(CompanySelector);

var PortSelector = {};
(function (p) {

    BaseSelector.call(p);
    p.SelectorUrl = "/Selector/PortSelector";

    var openDialog = function () {
        p.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 850, 500, p.SelectorUrl, null);
    }

    p.OnChoice = function (data) {
        p.SelectorFor.val(data.Code);
        JR.ColorBox.Close();
    }

    $("button[data-xxy-selector='Port']").on("click", $.proxy(openDialog, this));
})(PortSelector);


var RouteSelector = {};
(function (r) {

    BaseSelector.call(r);
    r.SelectorUrl = "/Selector/RouteSelector";

    var openDialog = function () {
        r.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, r.SelectorUrl, null);
    }

    r.OnChoice = function (data) {
        r.SelectorFor.val(data.SHIP_ROUTE_CODE);
        JR.ColorBox.Close();
    }

    $("button[data-xxy-selector='Route']").on("click", $.proxy(openDialog, this));

})(RouteSelector);


var FWCustomerSelector = {};
(function (r) {

    BaseSelector.call(r);
    r.SelectorUrl = "/Selector/FWCustomerSelector"

    var openDialog = function () {
        r.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, r.SelectorUrl, null);
    }

    r.OnChoice = function (data) {
        r.SelectorFor.val(data.CUSTOMER_CODE);
        JR.ColorBox.Close();
    }

    $("button[data-xxy-selector='FWCustomer']").on("click", $.proxy(openDialog, this));

})(FWCustomerSelector);

function BaseAutocomplete() {
    var s = this;

    //数据源, JSONP格式
    s.SourceUrl = "";

    s.RenderFormat = function (item) {
        /// <summary>
        /// 渲染格式
        /// </summary>
        /// <remark>
        /// eg: return "<a>" + item.Code + "<span class='badge pull-right'>" + item.NationNameCn + "</span></a>"
        /// </remark>
        /// <param name="item"></param>

        return "<a>未设置显示格式</a>"
    }

    var render = function (ul, item) {
        return $("<li>")
			.data("item.autocomplete", item)
			.append(s.RenderFormat(item))
			.appendTo(ul);
    };

    //s.RenderContainer = function (ul, items) {

    //}

    s.RenderContainer = null;

    var source = function (request, response) {
        $.ajax({
            url: s.SourceUrl,
            dataType: "jsonp",
            jsonpCallback: "callback",////////
            data: {
                q: request.term
            },
            cache: false
        }).done(function (data) {
            response(data);
        }).fail(function (httpRequest, textStatus, errorThrown) {
            if (console && console.log)
                console.log(errorThrown);
        });
    };

    s.OnChoice = function (sourceElement, event, ui) {
        /// <summary>
        /// 当选中时
        /// </summary>
        /// <param name="sourceElement">自动完成对应的那个Textbox</param>

        $(sourceElement).val(ui.item.Code);
    }

    s.Bind = function (elements) {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements">jQuery 选择的对象</param>

        elements.each(function (idx, item) {
            var data = $(item).autocomplete({
                source: source,
                select: function (event, ui) {
                    s.OnChoice(item, event, ui)
                    return false;
                }
            })
            .data("ui-autocomplete");
            var a = data.menu;
            data._renderItem = render;
            if (s.RenderContainer instanceof Function)
                data._renderMenu = s.RenderContainer;
        });
    }
}




//用户自动完成
var UserAutoComplete = {};
(function (u) {

    BaseAutocomplete.call(u);

    u.SourceUrl = "/Selector/User";

    u.RenderFormat = function (data) {
        return "<a>" + data.User.USERCODE + " | " + data.User.USERNAME + " | " + data.User.FULLNAME + "</a>";
    }

    u.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.User.USERNAME);
    }

    u.Bind($("input[data-xxy-selector='User']"));

})(UserAutoComplete);







var PortAutoComplete = {};
(function (p) {

    BaseAutocomplete.call(p);

    //公司ID
    p.Company = GlobalDatas.RootCompanyID;


    p.SourceUrl = "/Selector/Port/" + p.Company;
    p.RenderFormat = function (data) {
        return "<a>" + data.Code + "<span class='badge pull-right'>" + data.NameCn + "</span></a>"
    }
    p.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.Code);
    }

    p.Bind($("input[data-xxy-selector='Port']"));
})(PortAutoComplete);


//港口/地点自动完成
var PortAndLocationAutoComplete = {};
(function (p) {
    BaseAutocomplete.call(p);
    p.Company = GlobalDatas.RootCompanyID;
    p.SourceUrl = "/Selector/PortAndLocation/" + p.Company;
    p.RenderFormat = function (data) {
        return "<a>" + data.Code + "<span class='badge pull-right'>" + data.NameCn + "</span></a>"
    }
    p.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.Code);
    }

    p.RenderContainer = function (ul, items) {
        var that = this;
        var currentCategory = "";
        $.each(items, function (index, item) {
            var li;
            if (item.category != currentCategory) {
                ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                currentCategory = item.category;
            }
            li = that._renderItemData(ul, item);
            if (item.category) {
                li.attr("aria-label", item.category + " : " + item.label);
            }
        });
    }

    p.Bind($("input[data-xxy-selector='PortAndLocation']"));
})(PortAndLocationAutoComplete);

var NationAutoComplete = {};
(function (n) {

    BaseAutocomplete.call(n);
    n.SourceUrl = "/Selector/Nation";
    n.RenderFormat = function (data) {
        return "<a>" + data.Code + "<span class='badge pull-right'>" + data.NameCn + "</span></a>";
    }

    n.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.Code);
    }

    n.Bind($("input[data-xxy-selector='Nation']"));

})(NationAutoComplete);

//船舶自动完成
var VesselAutoComplete = {};
(function (v) {

    BaseAutocomplete.call(v);

    v.SourceUrl = "/Selector/Vessel";
    v.RenderFormat = function (data) {
        return "<a>" + data.VESSEL_CODE + "<span class='badge pull-right'>" + data.OWNER_CODE + "</span></a>"
    }

    v.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.VESSEL_CODE);
    }

    v.Bind($("input[data-xxy-selector='Vessel']"));

})(VesselAutoComplete);


//公司自动完成
var CompanyAutoComplete = {
};
(function (v) {

    BaseAutocomplete.call(v);

    v.SourceUrl = "/Selector/Company";
    v.RenderFormat = function (data) {
        return "<a>" + data.COMPANY_CODE + "<span class='badge pull-right'>" + data.COMPANY_SHORTNAME_CN + "</span></a>"
    }

    v.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.COMPANY_CODE);
    }

    v.Bind($("input[data-xxy-selector='Company']"));

})(CompanyAutoComplete);


//航线自动完成
var RouteAutoComplete = {
};
(function (r) {

    BaseAutocomplete.call(r);
    r.SourceUrl = "/Selector/Route";
    r.RenderFormat = function (data) {
        return "<a>" + data.SHIP_ROUTE_CODE + "<span class='badge pull-right'>" + data.SHIP_ROUTE_NAME + "</span></a>";
    }
    r.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.SHIP_ROUTE_CODE);
    }

    r.Bind($("input[data-xxy-selector='Route']"));

})(RouteAutoComplete);


//船期自动完成
var ScheduleAutoComplete = {};
(function (r) {
    BaseAutocomplete.call(r);
    r.SourceUrl = "/Selector/Schedule/" + GlobalDatas.RootCompanyID;
    r.SetSourceUrl = function (routeCode, shipCode, carrierID) {
        r.SourceUrl = "/Selector/Schedule/" + (carrierID != undefined ? carrierID : GlobalDatas.RootCompanyID) + "?routeCode=" + routeCode + "&shipCode=" + shipCode;
    }
    r.RenderFormat = function (data) {
        return "<a>" + data.VESSEL_CODE + "/" + data.VOYAGE + "<span class='badge pull-right'>" + data.SHIP_ROUTE_CODE + "</span></a>";
    }
    r.OnChoice = function (sourceElement, event, ui) {
        $(sourceElement).val(ui.item.SCHEDULE_ID);
    }

    r.Bind($("input[data-xxy-selector='Schedule']"));

})(ScheduleAutoComplete);


var ScheduleShipVoyageAutoComplete = {};
(function (r) {
    BaseAutocomplete.call(r);
    r.SourceUrl = "/Selector/Schedule/" + GlobalDatas.RootCompanyID;
    r.SetSourceUrl = function (routeCode, shipCode, carrierID) {
        r.SourceUrl = "/Selector/Schedule/" + (carrierID != undefined ? carrierID : GlobalDatas.RootCompanyID) + "?routeCode=" + (routeCode || "") + "&shipCode=" + (shipCode || "");
    }

    r.RenderFormat = function (data) {
        return "<a>" + data.VESSEL_CODE + "/" + data.VOYAGE + "<span class='badge pull-right'>" + data.SHIP_ROUTE_CODE + "</span></a>";
    }

    r.OnChoice = function (sourceElement, event, ui) {
        var forEle = $(sourceElement).attr("data-xxy-selector-for");
        $(sourceElement).parent().find("[data-xxy-selector-assist='ShipCode']").val(ui.item.VESSEL_CODE);
        $(sourceElement).parent().find("[data-xxy-selector-assist='Voyage']").val(ui.item.VOYAGE);
        $("#" + forEle).val(ui.item.SCHEDULE_ID);
    }

    r.Bind($("input[data-xxy-selector='ScheduleShipVoyage']"));

    $("input[data-xxy-selector='ScheduleShipVoyage']").on("change", function () {
        if ($(this).val().trim() == "") {
            var forEle = $(this).attr("data-xxy-selector-for");
            $("#" + forEle).val("");
            $("[data-xxy-selector-for='" + forEle + "']").val("");
        }
    });

})(ScheduleShipVoyageAutoComplete);

var CountryAutoComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseCountry/GetCountryList";

    v.RenderFormat = function (data) {
        return "<a>" + data.COUNTRY_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.COUNTRY_NAME + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.COUNTRY_NAME_CN + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.UNCODE + "<input type='hidden' value='" + data.BASE_COUNTRY_ID + "' /><input type='hidden' value='" + data.COUNTRY_FULLNAME + "' />" + "</a>"
    }

    //v.OnChoice = function (sourceElement, event, ui) {

    //    var a = sourceElement;
    //    $(sourceElement).val(ui.item.Country_Name);
    //}

    //v.Bind($("input[name='COUNTRY_CODE']"));
    //v.Bind($("input[name='COUNTRY_NAME']"));
})(CountryAutoComplete);



var ProvinceAutoComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseProvinces/GetProvinceList";
    v.RenderFormat = function (data) {
        return "<a>" + data.PROVINCE_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.PROVINCE_NAME + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.PROVINCE_NAME_CN + "<input type='hidden' value='" + data.BASE_PROVINCE_ID + "' />" + "</a>"
    }
})(ProvinceAutoComplete);


var CityAutoComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseCitys/GetCitysList";
    v.RenderFormat = function (data) {
        return "<a>" + data.CITY_NAME + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.CITY_NAME_CN + "<input type='hidden' value='" + data.BASE_CITY_ID + "' />" + "</a>"
    }
})(CityAutoComplete);


var FeeTypeAutoComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseFeetype/GetFeeTypeList";
    v.RenderFormat = function (data) {
        return "<a>" + data.FEE_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.FEE_NAME + "<input type='hidden' value='" + data.BASE_FEETYPE_ID + "' />" + "</a>"
    }
})(FeeTypeAutoComplete);


var LocationComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseLocation/GetLocationList";
    v.RenderFormat = function (data) {
        return "<a>" + data.LOCATION_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.LOCATION_NAME + "<input type='hidden' value='" + data.BASE_LOCATION_ID + "' />" + "</a>"
    }
})(LocationComplete);


var FeeTypeComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseFeetype/GetFeeTypeList";
    v.RenderFormat = function (data) {
        return "<a>" + data.FEE_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.FEE_NAME + "<input type='hidden' value='" + data.BASE_FEETYPE_ID + "' />" + "</a>"
    }
})(FeeTypeComplete);


var CurrencyComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseCurrency/GetCurrentyList";
    v.RenderFormat = function (data) {
        return "<a>" + data.CURRENCY_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.CURRENCY_NAME + "<input type='hidden' value='" + data.BASE_CURRENCY_ID + "' />" + "</a>"
    }
})(CurrencyComplete);

var CurrencyComplete_exp = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseCurrency/GetCurrentyList";
    v.RenderFormat = function (data) {
        return "<a>" + data.CURRENCY_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.CURRENCY_NAME + "<input type='hidden' value='" + data.BASE_CURRENCY_ID + "' />" + "</a>"
    }
})(CurrencyComplete_exp);

var PortComplete = {
};
(function (v) {
    BaseAutocomplete.call(v);
    v.SourceUrl = "/BaseLocation/GetPortList";
    v.RenderFormat = function (data) {
        return "<a>" + data.LOCATION_CODE + "&nbsp;&nbsp;&nbsp;&nbsp;" + data.LOCATION_NAME + "<input type='hidden' value='" + data.BASE_LOCATION_ID + "' />" + "</a>"
    }
})(PortComplete);


var ChoiceSpeLoad = {};
(function (m) {
    BaseSelector.call(m);

    m.SelectorUrl = "/Dict/ChoiceSpeLoad";

    var openDialog = function () {
        m.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, m.SelectorUrl, null);
    };
    $("[data-xxy-selector='ChoiceSpeLoad']").on("click", $.proxy(openDialog, this));

})(ChoiceSpeLoad);

var ChoiceSpeRemark = {};
(function (m) {
    BaseSelector.call(m);

    m.SelectorUrl = "/Dict/ChoiceSpeRemark";

    var openDialog = function () {
        m.BeforeOpenDialog(this);
        JR.ColorBox.AutoPage("选择", 800, 500, m.SelectorUrl, null);
    };
    $("[data-xxy-selector='ChoiceSpeRemark']").on("click", $.proxy(openDialog, this));

})(ChoiceSpeRemark);

$(".sidebarCloser").on("click", function () {
    $(".sidebar").switchClass("", "sidebarMini");
    $(".mainContent").switchClass("", "mainContentMax");
    $(".sidebarMini").switchClass("sidebarMini", "");
    $(".mainContentMax").switchClass("mainContentMax", "");
});



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
