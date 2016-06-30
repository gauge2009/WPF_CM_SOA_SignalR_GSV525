var JObj = {};
(function ($) {

    $.isObject = function (p) { return "object" == typeof (p) }
    $.isFunction = function (p) { return p instanceof Function; }


    $.Dom = {};
    (function ($, $$) {

        $$.$ = $.$ = function (p, doc) { return $$.isObject(p) ? p : (doc || document).getElementById(p); }
        $$.$c = $.$c = function (tag) { return document.createElement(tag); }
        $$.$tag = $.$tag = function (tag, node) { return $$.$(undefined == node || null == node ? document : node).getElementsByTagName(tag); }
        $$.$name = $.$name = function (name, node) { return $$.$(undefined == node || null == node ? document : node).getElementsByName(name); }

        $$.$class = $.$class = function (className, obj) {
            obj = $$.$(obj) || document;
            var objs = obj.all || obj.getElementsByTagName("*");
            var o, i, arr = [];
            var classNames = "";
            className = "," + className + ",";
            for (i = 0; o = objs[i]; i++) {
                classNames = "," + o.className.split(/\s+/).join(",") + ",";
                if (classNames.indexOf(className) >= 0) {
                    arr.push(o);
                }
            }
            delete objs;
            return arr;
        }

        $.getRuntimeStyle = function (obj, k, d) {
            var v = null;
            if (obj.currentStyle) {
                k = k.replace(/\-(\w)/ig, function () { return arguments[1].toUpperCase(); });
                if (k == 'float') k = 'styleFloat'; //ie,opera
                v = obj.currentStyle[k];
            } else {
                if (k == 'styleFloat') k = 'float';
                k = k.replace(/[A-Z]/g, function () { return '-' + arguments[0].toLowerCase(); });
                v = window.getComputedStyle(obj, null).getPropertyValue(k);
            }
            if ((v == 'auto' || v == '') && d != undefined) v = d;
            delete obj;
            return v;
        }

        $.getOpacity = function (obj) {
            if (JObj.Browser.ie) {
                var a = obj.style.filter.alpha;
                return a == undefined ? 100 : a;
            } else {
                return $.getRuntimeStyle(obj, 'opacity', 1) * 100;
            }
        }

    })($.Dom, $);


    $.Browser = {};
    (function ($, $$) {

        $.getFlashVersion = function () {
            var f = "-1", n = navigator;
            if (n.plugins && n.plugins.length) {
                for (var ii = 0; ii < n.plugins.length; ii++) {
                    if (n.plugins[ii].name.indexOf('Shockwave Flash') != -1) {
                        f = n.plugins[ii].description.split('Shockwave Flash ')[1];
                        break;
                    }
                }
            } else if (window.ActiveXObject) {
                for (var ii = 11; ii >= 2; ii--) {
                    try {
                        var fl = eval("new ActiveXObject('ShockwaveFlash.ShockwaveFlash." + ii + "');");
                        if (fl) {
                            f = ii + '.0';
                            break;
                        }
                    } catch (e) {
                    }
                }
            }

            if (f == "-1") return f;
            else return f.substring(0, f.indexOf(".") + 2)
        }

        var n_ = navigator;

        var b = n_.appName;
        var ua = n_.userAgent.toLowerCase();
        $.userAgent = n_.userAgent;
        $.name = "Unknow";
        $.safari = ua.indexOf("safari") > -1;  // always check for safari & opera
        $.chrome = ua.indexOf('chrome') > -1; // must under safari check;
        $.opera = ua.indexOf("opera") > -1;    // before ns or ie
        $.firefox = ua.indexOf('firefox') > -1; // check for gecko engine
        $.netscape = !$.firefox && !$.opera && !$.safari && (b == "Netscape");
        $.ie = !$.opera && (b == "Microsoft Internet Explorer");

        $.name = ($.ie ? "IE" : ($.firefox ? "Firefox" : ($.netscape ? "Netscape" : ($.opera ? "Opera" : ($.chrome ? "Chrome" : ($.safari ? 'Safari' : "Unknow"))))));

        switch ($.name) {
            case "Opera":
                $.fullVersion = n_.appVersion.split(" ")[0];
                $.os = n_.appVersion.split(";")[1];
                break;
            case "IE":
                $.fullVersion = ua.substr(ua.indexOf("msie") + 5).split(";")[0];
                break;
            case "Firefox":
                $.fullVersion = ua.substr(ua.indexOf("firefox") + 8);
                break;
            case "Safari":
                $.fullVersion = ua.substr(ua.indexOf("version") + 8).split(" ")[0];
                break;
            case 'Chrome':
                $.safari = false; ////
                $.fullVersion = ua.substr(ua.indexOf('chrome') + 7).split(' ')[0];
                break;
            case "Netscape":
                $.fullVersion = ua.substr(ua.indexOf("netscape") + 9);
                break;
            default:
                $.fullVersion = "-1";
        }
        $.version = parseFloat($.fullVersion);

        $.cookieEnabled = n_.cookieEnabled;
        $.javaEnabled = n_.javaEnabled();

        $.os = $.os || n_.platform;
        $.browserLang = n_.browserLange || n_.language;
        $.osLang = n_.language;

    })($.Browser, $);



    $.Xml = {};
    (function ($, $$) {

        var ACTIVEXOBJECT_XMLHTTP = null;
        var ACTIVEXOBJECT_DOMDOCUMENT = null;

        if (!$$.Browser.ie) {
            Element.prototype.__defineGetter__("xml", function () {
                return (new XMLSerializer).serializeToString(this);
            });
        }

        $.getXMLHttp = function () {
            var xmlHttp = null;
            //if($$.Browser.ie && $$.Browser.version < 7){ //用IE7内置的 XMLHttpRequest 对象，不能加载本地文件．
            if ($$.Browser.ie) {
                var v = [/*'MSXML2.XMLHTTP.8.0', 'MSXML2.XMLHTTP.7.0',*/'MSXML2.XMLHTTP.6.0', 'MSXML2.XMLHTTP.5.0', 'MSXML2.XMLHTTP.4.0', 'MSXML2.XMLHTTP.3.0', 'MSXML2.XMLHTTP.2.6', 'MSXML2.XMLHTTP', 'Microsoft.XMLHTTP'];
                if (typeof (ACTIVEXOBJECT_XMLHTTP) == "string")
                    v[0] = ACTIVEXOBJECT_XMLHTTP;

                var v_;
                for (var i = 0; v_ = v[i]; i++) {
                    try {
                        xmlHttp = new ActiveXObject(v_);
                        ACTIVEXOBJECT_XMLHTTP = v_;
                        break;
                    } catch (e) { }
                }

            } else {
                xmlHttp = new XMLHttpRequest();
            }

            if (xmlHttp == null) {
                alert("你的系统不支持AJAX");
            }
            return xmlHttp;
        }


        $.getXMLDoc = function () {
            //DOMParser document.implementation
            if ($$.Browser.ie) {
                var doc = null;
                var v = ["Msxml2.DOMDocument.6.0", "Msxml2.DOMDocument.5.0", "Msxml2.DOMDocument.4.0", "Msxml2.DOMDocument.3.0", "MSXML2.DOMDocument"];
                if (typeof (ACTIVEXOBJECT_DOMDOCUMENT) == "string")
                    v[0] = ACTIVEXOBJECT_DOMDOCUMENT;

                var v_;
                for (var i = 0; v_ = v[i]; i++) {
                    try {
                        doc = new ActiveXObject(v_);
                        ACTIVEXOBJECT_DOMDOCUMENT = v_;
                        break;
                    } catch (e) {
                    }
                }
            } else if (document.implementation && document.implementation.createDocument) {
                doc = document.implementation.createDocument("", "doc", null);
            }

            return doc;
        }

        $.loadXML = $.parseXML = function (source) {
            var doc;
            if (window.DOMParser) {
                var parser = new DOMParser();
                doc = parser.parseFromString(source, "text/xml");
            } else {
                doc = $.getXMLDoc();
                doc.loadXML(source);
            }
            return doc;
        }

        $.getNodeAtt = function (pNode, pAtt) {
            try {
                return pNode.attributes.getNamedItem(pAtt).nodeValue;
            } catch (e) {
                //alert("前台调试错误：\n"+e.message+"\n当前节点不存在: "+pAtt+"这个属性");
                return false;
            }
        }

        $.extractNodes = function (pNode) {
            if (pNode.nodeType == 3)
                return null;
            var node, nodes = new Array();
            for (var i = 0; node = pNode.childNodes[i]; i++) {
                if (node.nodeType == 1 || node.nodeType == 4)
                    nodes.push(node);
            }
            return nodes;
        }

        $.getXML = $.serialize = function (pNode) {
            if (!pNode) return null;
            if (pNode.xml)
                return pNode.xml;
            else if (window.XMLSerializer)
                return (new XMLSerializer()).serializeToString(pNode);
        }

    })($.Xml, $);


    /*----------------------------------------------------------------------------------
    Ajax
    JObj.Ajax
    ----------------------------------------------------------------------------------*/
    $.Ajax = {};
    (function ($, $$) {

        // AjaxObj
        var AjaxObj = function (rule, dataRule) {
            var $ = this;
            $.async = rule.async == undefined ? true : rule.async; //默认异步加载

            var prepareData = function () {
                if (dataRule == null) return null;

                var o;
                var s = [];
                for (o in dataRule) {
                    s.push(encodeURIComponent(o) + "=" + encodeURIComponent(dataRule[o]));
                    //必须要： encodeURIComponent,否则，post 不成功！
                }
                s = s.join('&');

                if ($.method.toUpperCase() == "GET") {
                    $.url += ($.url.indexOf("?") > -1 ? "&" : "?") + s;
                    return null;
                } else
                    return s;
            }

            var ready = function () {
                $.url = rule.url;
                $.method = rule.method || "GET";
                $.data = prepareData();
                $.onSuccess = rule.onSuccess;
                $.onUnsuccess = rule.onUnsuccess;
                $.onError = rule.onError;
                //$.onReady = rule.onReady;
                $$.isFunction(rule.onReady) ? rule.onReady() : null;
            }

            $.xmlHttp = $$.Xml.getXMLHttp();

            var xmlHttp_onreadystatechange = function () {
                var http = $.xmlHttp;
                if (http.readyState == 4) {
                    switch (http.status) {
                        case 0:
                        case 200:
                            $$.isFunction($.onSuccess) ? $.onSuccess(http, 200, rule, dataRule) : null;
                            break;
                        default:
                            $$.isFunction($.onUnsuccess) ? $.onUnsuccess(http, http.status, rule, dataRule) : null;
                    }
                } else
                    $$.isFunction($.onWait) ? $.onWait(http, http.readyState, rule, dataRule) : null;
            }

            if ($.async)//如果同步加载，就不要 onreadystatechange 了
                $.xmlHttp.onreadystatechange = xmlHttp_onreadystatechange;

            $.send = function () {
                ready();
                $.xmlHttp.open($.method, $.url, $.async);
                $.xmlHttp.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
                try {
                    $.xmlHttp.send($.data);
                } catch (e) {
                    $$.isFunction($.onError) ? $.onError(e) : alert(e.message);
                    return;
                }
                if (!$.async) {
                    xmlHttp_onreadystatechange();
                }
            }
        }

        $.send = function (rule, dataRule) {
            var ajax = new AjaxObj(rule, dataRule);
            ajax.send();
        }

    })($.Ajax, $)
})(JObj);




if (typeof (JObj.Plugin) != "object") JObj.Plugin = {};
JObj.Plugin.JTree = {};

(function ($, $$) {

    var treeNode = function () {
        var self = this;
        this.obj = null; //指caption所在的标签：<span>。。</span>
        this.caption = null; //显示的文字
        this.level = null; //节点的层次
        this.value = null; //这个值暂时没有用到。预感到会有用，因为做Delphi的树时，就因为缺少相关的东东，不得不用其它的办法来取代

        this.xml = null;

        //----------------------------------
        this.treeNodes = new Array(); //子树集合

        this.parentTreeNode = null; 		//当前“树枝”父树枝，就像树叶和树枝的关系一样。
        this.expand = function (pFlag) {        //如果是树枝，就收缩或展开。要重定位一下要展开或收缩的对象。
            try {
                self.obj.parentNode.expand(pFlag); //pFlag只能为false或true
            } catch (e) {
            }
            ;
        }
        this.click = function () {
            if (self.obj.click)
                self.obj.click();
            else
                self.obj.onclick();
        }
        this.dblclick = function () {
            self.obj.ondblclick();
        }
    }

    var JTree = function (pParent) {
        //this.PICPATH = "JTree/"	//图片文件所在的文件夹，可见public，可改变。
        this.PICPATH = $$.path + "plugins/JTree/JTreePic/";

        var self = this; //相当于一个引用，指向自己。JTree.
        //-----------------------------------------------------------------------------

        var JOIN = this.PICPATH + "join.gif";
        var JOINBOTTOM = this.PICPATH + "joinbottom.gif";
        var MINUS = this.PICPATH + "minus.gif";
        var MINUSBOTTOM = this.PICPATH + "minusbottom.gif";
        var PLUS = this.PICPATH + "plus.gif";
        var PLUSBOTTOM = this.PICPATH + "plusbottom.gif";
        var EMPTY = this.PICPATH + "empty.gif";
        var LINE = this.PICPATH + "line.gif";

        var LEAFICON = this.PICPATH + "page.gif";
        var NODEICON = this.PICPATH + "folder.gif";

        var OPEN = new Array();
        OPEN[true] = MINUS;
        OPEN[false] = PLUS;

        var OPENBOTTOM = new Array();
        OPENBOTTOM[true] = MINUSBOTTOM;
        OPENBOTTOM[false] = PLUSBOTTOM;

        this.setPicPath = function (pPath) {
            self.PICPATH = pPath;

            JOIN = self.PICPATH + "join.gif";
            JOINBOTTOM = self.PICPATH + "joinbottom.gif";
            MINUS = self.PICPATH + "minus.gif";
            MINUSBOTTOM = self.PICPATH + "minusbottom.gif";
            PLUS = self.PICPATH + "plus.gif";
            PLUSBOTTOM = self.PICPATH + "plusbottom.gif";
            EMPTY = self.PICPATH + "empty.gif";
            LINE = self.PICPATH + "line.gif";

            OPEN[true] = MINUS;
            OPEN[false] = PLUS;

            OPENBOTTOM[true] = MINUSBOTTOM;
            OPENBOTTOM[false] = PLUSBOTTOM;

            LEAFICON = self.PICPATH + "page.gif";
            NODEICON = self.PICPATH + "folder.gif";
        }

        this.CAPTIONATT = "caption"; //标题属性是哪一个属性
        this.ICONATT = "icon"; //图标属性
        this.EXPANDALL = true; //是否全部扩展。

        //this.clickItem=new treeNode;
        //this.clickItem = new treeNode;//用于点击时，返回值。
        this.clickItem = null;
        this.selectNode = null; //同上
        //----------------------------------------------------
        this.treeNodes = new Array(); //树节点的集合。
        this.treeNodes.push(null);
        this.root = this.treeNodes[0] = new treeNode; //树的根

        this.onclick = null;
        this.onmouseover = null;
        this.onmouseout = null;
        this.ondblclick = null;
        //----------------------------------------------------------------------------

        //----------------------------------------------------------------------------
        //2006/12/05修改，目的是为了做全展开和全缩。
        var oOutLine = null; //轮廓
        var leafAreas = new Array();
        //-----------------------------------------------------------------------------
        this.body = JObj.$(pParent) || document.body;
        //-----------------------------------------------------------------------------

        var xmlDom = null;
        var DOMRoot = null;

        this.loadFromFile = function (xmlFile) {
            $$.Ajax.send({
                url: xmlFile,
                method: "GET",
                async: false,

                onSuccess: function (xmlHttp, status, rule, dataRule) {
                    xmlDom = xmlHttp.responseXML;
                    DOMRoot = xmlDom.documentElement;
                }
            });
        }

        this.loadFromString = function (xmlString) {
            xmlDom = $$.Xml.loadXML(xmlString);
            DOMRoot = xmlDom.documentElement;
        }

        //-----------------------------------------------------------------------------
        var createImg = function (pSrc) {
            var tmp = $$.$c("IMG");
            tmp.align = "absmiddle";
            tmp.src = pSrc;
            tmp.onerror = function () {
                try {
                    this.parentNode.removeChild(this);
                } catch (e) {
                }
            }
            return tmp;
        }

        var caption_onclick = function (evt) {
            evt = window.event || evt;
            try {
                self.clickItem.className = "caption";
            } catch (e) { }
            this.className = "captionHighLight";

            self.clickItem = this;
            self.selectNode = this.xmlNode;

            try {
                if (evt.type == "click")
                    self.onclick();
                else
                    self.ondblclick();
            } catch (e) {
            } //必须加上，如果self没有对onclick赋值的话，会引发错误。
        }

        var caption_mousemove = function () {
            if (this.className != "captionHighLight")
                this.className = "captionActive";
            try {
                self.onmouseover()
            } catch (e) { }
        }

        var caption_mouseout = function () {
            if (this.className != "captionHighLight")
                this.className = "caption";
            try {
                self.onmouseout()
            } catch (e) { }
        }

        var createCaption = function (pNode, pLevel) {
            var tmp = $$.$c("SPAN");
            tmp.xmlNode = pNode;
            tmp.level = pLevel;
            tmp.innerHTML = $$.Xml.getNodeAtt(pNode, self.CAPTIONATT);
            tmp.className = "caption";
            tmp.onmouseover = caption_mousemove;
            tmp.onmouseout = caption_mouseout;
            tmp.onclick = caption_onclick;
            tmp.ondblclick = caption_onclick;
            return tmp;
        }


        var childShowBtn_onclick = function () {
            var isExpand = this.parentNode.expand();

            if (!this.parentArea.isLastChild) {
                this.src = OPEN[isExpand];
            } else {
                this.src = OPENBOTTOM[isExpand];
            }
        }

        var createTreeLine = function (pNode, pParentArea) {
            var hasChildren = pNode.hasChildNodes(); //是否有孩子。
            for (var i = 0; i < pParentArea.level; i++) {
                var tmpArea = pParentArea;
                for (var j = pParentArea.level; j > i; j--) {
                    //tmpArea=tmpArea.parentNode;
                    tmpArea = tmpArea.parentNode.parentNode;
                }

                if (tmpArea.isLastChild)
                    appendTo(createImg(EMPTY), pParentArea);
                else
                    appendTo(createImg(LINE), pParentArea);
            }

            if (hasChildren) {//有孩子
                var childShowBtn;
                if (!pParentArea.isLastChild) {
                    childShowBtn = createImg(OPEN[true]);
                    appendTo(childShowBtn, pParentArea);
                } else {
                    childShowBtn = createImg(OPENBOTTOM[true]);
                    appendTo(childShowBtn, pParentArea);
                }
                childShowBtn.parentArea = pParentArea;
                childShowBtn.onclick = childShowBtn_onclick;
                pParentArea.expandBtn = childShowBtn; //新增的
            } else {//无孩子。
                if (!pParentArea.isLastChild)
                    appendTo(createImg(JOIN), pParentArea);
                else
                    appendTo(createImg(JOINBOTTOM), pParentArea);
            }
        }

        var createIcon = function (pNode, pParentArea) {
            var hasChildren = pNode.hasChildNodes(); //是否有孩子
            var tmpIcon = $$.Xml.getNodeAtt(pNode, self.ICONATT);
            //alert(NODEICON)
            if (tmpIcon == false) {
                if (hasChildren)
                    appendTo(createImg(NODEICON), pParentArea);
                else
                    appendTo(createImg(LEAFICON), pParentArea);
            } else {
                appendTo(createImg(tmpIcon), pParentArea);
            }
        }
        //-----------------------------------------------------------------------------
        //将指定OBJ追加到某个OBJ的最后面。
        var appendTo = function (pObj, pTargetObj) {
            try {
                pTargetObj.appendChild(pObj);
            } catch (e) {
                alert(e.message);
            }
        }
        //-----------------------------------------------------------------------------
        var isFirstChild = function (pNode) {
            //除了空白节点之外，是否是第一个节点
            var tmpNode = pNode.previousSibling;
            try {
                while (tmpNode.previousSibling != null && tmpNode.nodeType != 1)
                    tmpNode = tmpNode.previousSibling;
                if (tmpNode.nodeType == 3)//是空节点
                    return true;
                else
                    return false;
            } catch (e) {
                return true;
            }
        }
        var isLastChild = function (pNode) {
            var tmpNode = pNode.nextSibling;
            try {
                while (tmpNode.nextSibling != null && tmpNode.nodeType != 1)
                    tmpNode = tmpNode.nextSibling;
                if (tmpNode.nodeType == 3)//是空节点
                    return true;
                else
                    return false;
            } catch (e) {
                return true;
            }
        }
        //-----------------------------------------------------------------------------
        //循环绘制各节点。从下面这些起，这些节点具有收缩功能，所以，下面的这些不应该被oRoot所包含，而应该是oOutLine的孩子。
        var createSubTree = function (pNode, pLevel, pNodeArea, pTreeNode) {
            var subNode;
            for (var i = 0; subNode = pNode.childNodes[i]; i++) {
                if (subNode.nodeType != 1) continue; //由于默认了把空白也当着一个节点来处理，所以，这里要判断一下。

                var subNodeItem = $$.$c("DIV")

                if (subNode.hasChildNodes()) {
                    var subNodeSubArea = $$.$c("DIV");
                    leafAreas.push(subNodeSubArea);
                }

                subNodeItem.level = pLevel + 1;
                subNodeItem.isFirstChild = isFirstChild(subNode);
                subNodeItem.isLastChild = isLastChild(subNode);
                //subNodeItem.parentTreeNode	=pTreeNode;//新增属性				

                //下面的这个位置不能变动，因为createTreeLine里用到了它的parentNode
                appendTo(subNodeItem, pNodeArea);

                createTreeLine(subNode, subNodeItem);
                createIcon(subNode, subNodeItem);
                var subNodeCaption = createCaption(subNode, pLevel + 1);
                subNodeItem.caption = subNodeCaption.innerHTML;

                subNodeItem.tree = new treeNode();
                subNodeItem.tree.obj = subNodeCaption;
                subNodeItem.tree.caption = subNodeItem.caption;
                subNodeItem.tree.level = subNodeItem.level;
                subNodeItem.tree.xml = subNode;
                subNodeItem.tree.parentTreeNode = pTreeNode;

                pTreeNode.treeNodes.push(subNodeItem.tree);

                appendTo(subNodeCaption, subNodeItem);

                if (subNode.hasChildNodes()) {
                    //createSubTree(subNode,pLevel+1,subNodeItem);
                    appendTo(subNodeSubArea, subNodeItem);
                    createSubTree(subNode, pLevel + 1, subNodeSubArea, pTreeNode.treeNodes[pTreeNode.treeNodes.length - 1]);
                    subNodeItem.subNodeSubArea = subNodeSubArea;

                    subNodeItem.expand = function (pFlag) {
                        //如果状态是展开，返回真，否则返回假。
                        //this.subNodeSubArea.style.display=="" ? this.subNodeSubArea.style.display="none" : this.subNodeSubArea.style.display="";

                        if (pFlag == null) {
                            if (this.subNodeSubArea.style.display == "") {
                                this.subNodeSubArea.style.display = "none";
                                return false;
                            } else {
                                this.subNodeSubArea.style.display = "";
                                return true;
                            }
                        } else {
                            //alert(this.expandBtn.tagName);
                            if (pFlag)
                                this.subNodeSubArea.style.display = "";
                            else this.subNodeSubArea.style.display = "none";

                            if (!this.isLastChild)
                                this.expandBtn.src = OPEN[pFlag];
                            else
                                this.expandBtn.src = OPENBOTTOM[pFlag];

                        }

                    };
                }
            }
        }


        //--------------------------------------------------------------------------------
        //2006/12/05新增功能
        this.expandAll = function (pExpandAll) {
            var oLeafArea;
            for (i = 0; oLeafArea = leafAreas[i]; i++) {
                //oLeafArea.style.display = (pExpandAll == false ? "none" : "");
                oLeafArea.parentNode.expand(pExpandAll);
            }
        }
        //--------------------------------------------------------------------------------

        this.create = function () {
            //-----------------------------------------------------------------------------
            //绘制轮廓
            oOutLine = $$.$c("DIV");
            oOutLine.className = "JTree";
            appendTo(oOutLine, this.body);
            //oOutLine.onclick=this.onclick;
            //-----------------------------------------------------------------------------
            //绘制根。这个根不具备收缩的功能。
            var oRoot = $$.$c("DIV");

            oRoot.level = -1; //级别。根的级别为-1;

            var oRootIcon = createImg($$.Xml.getNodeAtt(DOMRoot, self.ICONATT));
            //var oRootCaption=createCaption($$.Xml.getNodeAtt(DOMRoot,self.CAPTIONATT),-1);
            var oRootCaption = createCaption(DOMRoot, -1);
            oRoot.caption = oRootCaption.innerHTML;

            //================================================
            //子树
            //================================================
            oRoot.tree = new treeNode();
            oRoot.tree.obj = oRootCaption;
            oRoot.tree.caption = oRoot.caption;
            oRoot.tree.level = oRoot.level;
            oRoot.tree.xml = DOMRoot;
            oRoot.tree.parentTreeNode = self.treeNodes[0];

            self.root = self.treeNodes[0] = oRoot.tree;

            appendTo(oRootIcon, oRoot);
            appendTo(oRootCaption, oRoot);
            appendTo(oRoot, oOutLine);
            //------------------------------------------------------------------------------		
            createSubTree(DOMRoot, -1, oOutLine, self.treeNodes[0]);
            self.expandAll(self.EXPANDALL);
        }
    }

    $.getInstance = function (pParent) {
        var jtree = new JTree(pParent);
        //jtree.setPicPath(JObj.path + "plugins/JTree/JTreePic/");
        //$$.Loader.loadCss(JObj.path + "plugins/JTree/JTree.css");
        return jtree;
    }

})(JObj.Plugin.JTree, JObj);