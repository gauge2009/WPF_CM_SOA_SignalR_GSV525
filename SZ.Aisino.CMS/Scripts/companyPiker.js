;
JR = window.JR || {};
JR.CompanyPiker = JR.CompanyPiker || {};
(function ($, obj, win) {

    win.hideCompTimeOut;

    obj.lastQueryStr = "";

    obj.init = function (inputObjId, type) {
        var inputObj = $("#" + inputObjId);
        if (inputObj[0].id.toLowerCase().indexOf("companycode") > -1) {
            inputObj.attr("companyPikerType", "CompanyCode");
        } else {
            inputObj.attr("companyPikerType", "CompanyName");
        }
        
        inputObj.wrap("<div class='input-group'></div>");

        //输入控件  弹出按钮
        inputObj.after("<span  id='" + inputObjId + "_Folder' class=\"input-group-btn\"><button type=\"button\" class=\"btn btn-default input-sm\"> " +
            "<span class=\"glyphicon glyphicon-folder-open\"></span></button></span>");

        
        obj.inputResize(inputObjId);
        var myFolder = $("#" + inputObjId + "_Folder");
        myFolder.click(function() {
            //obj.getPageData("", 5, 1, this.id + "_compPiker", inputObjId, "true");
            //var myInput = $(this).parent().prev('input');
            //var myInput = $("#" + inputObj);
            var aa = setTimeout(function () {
                obj.handleClientRequest(inputObj[0], "true");
                inputObj[0].focus();
            }, 250);
        });
        
        var $wind = $(window);
        $wind.resize(function () {
            obj.inputResize(inputObjId, type);
        });

        //inputObj.parent().resize(function (type) {
        //    obj.inputResize(inputObjId, type);
        //}(type));
        //弹出按钮

        inputObj.bind("keyup", function () {
            obj.handleClientRequest(this);
        });

       
        inputObj.bind("blur", function () {
            var pikerId = this.id + "_compPiker";
            JR.CompanyPiker.blurEvent(this, pikerId);
        });

        inputObj.bind("focus", function () {
            //var pikerId = this.id + "_compPiker";

            //if ($("#" + pikerId).length > 0 && $("#" + this.id).val().length > 1) {
            //    //if ($("#" + pikerId + "_content").html() != "") {
            //        var type = "";
            //        if (this.id.toLowerCase().indexOf("companycode") > -1) {
            //            type = "CompanyCode";
            //        } else {
            //            type = "CompanyName";
            //        }
            //        $("#" + pikerId).show();
            //        if (obj.lastQueryStr != $(this).val() && obj.lastQueryStr != "") {
            //            var queryAll = $.trim(obj.lastQueryStr) == "" ? "true" : "";
            //            obj.getPageData(type, 5, 1, pikerId, this.id, queryAll);
            //        }
            //    //}
            //}

        });
    };

    obj.blurEvent = function(e, pikerId) {
        clearTimeout(win.hideCompTimeOut);
        win.hideCompTimeOut = setTimeout(function() {
            if ($("#" + pikerId).length > 0) {
                $("#" + pikerId).hide();
            }
            if ($(e).val() != "") {
                $.ajax({
                    url: '/Company/Check_COMPANY_NAME_CN',
                    datetype: "json",
                    data: {
                        "BaseCompany.COMPANY_NAME_CN": $(e).val()
                    },
                    type: "post",
                    success: function(data) {
                        if (data == true) {
                            $(e).val("");
                        }
                        obj.BlurCallBack(e);
                    }
                });
            } else {
                obj.BlurCallBack(e);
            }
        }, 200);
    };
    
    obj.GetCompanyId = function (compId, inputerId, type, e) {
        var pikerId = inputerId + "_compPiker";
        $("#" + pikerId).hide();
        obj.SelectCallBack(compId, e);
    };

    obj.addRowClass = function (e) {
        $(e).addClass("compPiker_rowClass");
    };
    
    obj.removeRowClass = function (e) {
        $(e).removeClass("compPiker_rowClass");
    };

    //选中公司，回调函数
    obj.SelectCallBack = function(compId, e) {

    };

    //公司失去焦点回调
    obj.BlurCallBack = function(e) {

    };

    obj.inputResize = function (inputObjId, type) {
        //var inputObj = $("#" + inputObjId);
        //var myFolder = $("#" + inputObjId + "_Folder");

        //var inputHeight = inputObj.height() + 5;
        //var inputWidth = inputObj.parent().width();
        //inputObj.css({ "width": inputWidth - 20 + "px" });
        
        //if (type == "inline") {
        //    myFolder.css({ "top": "5px", "left": "10px", "cursor": "pointer" });
        //} else {
        //    myFolder.css({ "top": "0px", "left": "0px", "cursor": "pointer" });
        //}

        //if (type == "inline") {
        //    //myFolder.css({ "top": "5px", "left": "10px", "cursor": "pointer" });
        //} else {
        //    //myFolder.css({ "top": "-" + inputHeight + "px", "left": inputWidth - 15 + "px", "cursor": "pointer" });
        //}
        //重新设置选择器的位置
        obj.resetPickPosition(inputObjId);
    };

    obj.clearTimeout = function (inputerId) {
        $("#" + inputerId).focus();
        if (win.hideCompTimeOut > 0) {
            clearTimeout(win.hideCompTimeOut);
        }
    };

    obj.resetPickPosition = function(inputObjId) {
        var inputObj = $("#" + inputObjId);
        var pikerObj = $("#" + inputObjId + "_compPiker");
        if (pikerObj.length > 0) {
            var top = inputObj.offset().top + inputObj.outerHeight() + 1;
            var left = inputObj.offset().left;
            if (!document.all) {
                left += 7;
            }
            pikerObj.css({ "top": top + "px", "left": left + "px" });
        }
    };

    obj.gotoPage = function (type, pageSize, pageIndex, pikerId, inputerId) {
        var queryAll = $.trim(obj.lastQueryStr) == "" ? "true" : "";
        obj.getPageData(type, pageSize, pageIndex, pikerId, inputerId, queryAll);
    };

    obj.handleClientRequest = function (e, queryAll) {
        var pikerId = e.id + "_compPiker";
        var inputerId = e.id;

        if ($(e).val().length < 2 && !queryAll) {
            //$("#" + pikerId).remove();
            //return;
        }
        if ($("#" + pikerId).length == 0) {
            $(document.body).append("<div id='" + pikerId + "'  class='compPiker_main'><div id='" + pikerId + "_content'  class='compPiker_content'></div></div>");
            obj.resetPickPosition(e.id);
        }

        var type = "All";
        //if (e.id.toLowerCase().indexOf("companycode") > -1) {
        //    type = "CompanyCode";
        //} else {
        //    type = "CompanyName";
        //}
        obj.getPageData(type, 5, 1, pikerId, inputerId, queryAll);

        obj.lastQueryStr = !queryAll ? $("#" + inputerId).val() : "";
    };

    obj.findMatchText = function (inputText, forMatchText) {
        inputText = inputText.trim();
        var index = forMatchText.toLowerCase().indexOf(inputText.toLowerCase());
        if (inputText.length == 0 || index == -1) {
            return forMatchText;
        }
        return forMatchText.substring(0, index) + "<span class='compPiker_higthlight'>" + forMatchText.substring(index, index + inputText.length) + "</span>" + forMatchText.substring(index + inputText.length);
    };

    obj.getPageData = function (type, pageSize, pageIndex, pikerId, inputerId, queryAll) {

        pageSize = parseInt(pageSize);
        pageIndex = parseInt(pageIndex);

        $.ajax({
            url: '/Company/QueryCompany',
            datetype: "json",
            data: {
                fieldName: type,
                queryText: $("#" + inputerId).val(),
                pageSize: pageSize,
                pageIndex: pageIndex,
                queryAll: queryAll
            },
            type: "post",
            success: function (data) {
                var pageCount = data.pageCount;
                var contentObj = $("#" + pikerId + "_content");

                if (data.pageData.length > 0) {
                    var htmlStr = "";
                    htmlStr += "<table class='table compPiker_table gray_border' border='1'>";
                    htmlStr += "<tr class='compPiker_table_top' onclick=\"JR.CompanyPiker.clearTimeout('" + inputerId + "')\"><th>公司代码</th><th>公司名称</th><th>繁体名称</th><th>英文名称</th></tr>";
                    for (var i = 0; i < data.pageData.length; i++) {
                        htmlStr += "<tr onmouseover='JR.CompanyPiker.addRowClass(this)'  onmouseout='JR.CompanyPiker.removeRowClass(this)' onclick=\"JR.CompanyPiker.GetCompanyId("
                            + data.pageData[i].COMPANY_ID + ",'" + inputerId + "','" + type + "', this)\" >";
                        htmlStr += "<td style='width:100px;'>" + obj.findMatchText($("#" + inputerId).val(), data.pageData[i].COMPANY_CODE) + "</td>";
                        htmlStr += "<td>" + obj.findMatchText($("#" + inputerId).val(), data.pageData[i].COMPANY_NAME_CN) + "</td>";
                        htmlStr += "<td>" + obj.findMatchText($("#" + inputerId).val(), data.pageData[i].COMPANY_NAME_TW) + "</td>";
                        htmlStr += "<td>" + obj.findMatchText($("#" + inputerId).val(), data.pageData[i].COMPANY_NAME_EN) + "</td>";
                        htmlStr += "</tr>";
                    }
                    htmlStr += "</table>";

                    var prePageIndex = pageIndex - 1;
                    var nextPageIndex = pageIndex +1;

                    htmlStr += "<div class='compPiker_footer'  onclick=\"JR.CompanyPiker.clearTimeout('" + inputerId + "')\" >";
                    if (prePageIndex > 0) {
                        htmlStr += "<div class=\"compPikerPageBack\" onclick=\"JR.CompanyPiker.gotoPage('" + type + "','" + pageSize + "','" + prePageIndex + "','" + pikerId + "','" + inputerId + "')\"></div>";
                    } else {
                        htmlStr += "<div class=\"compPikerPageBack_disabled\" />";
                    }
                    htmlStr += "<div class='compPikerPageIndex' >" + pageIndex + "/" + pageCount + "</div>";
                    if (nextPageIndex <= pageCount) {
                        htmlStr += "<div class=\"compPikerPageFront\" onclick=\"JR.CompanyPiker.gotoPage('" + type + "','" + pageSize + "','" + nextPageIndex + "','" + pikerId + "','" + inputerId + "')\"></div>";
                    } else {
                        htmlStr += "<div class=\"compPikerPageFront_disabled\" />";
                    }
                    htmlStr += "</div>";
                    
                    contentObj.html(htmlStr);
                    contentObj.parent().show();
                } else {
                    contentObj.html("");
                    contentObj.parent().hide();
                }
            },
            beforeSend: function () {
                //alert("bef");
            },
            complete: function () {
                //alert("bef");
            }
        });
    };

})(jQuery, JR.CompanyPiker, window)
