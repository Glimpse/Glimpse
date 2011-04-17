var XHRSpy = function () {
    this.requestHeaders = {};
    this.responseHeaders = {};
};

XHRSpy.prototype =
{
    method: null,
    url: null,
    href: null,
    async: null,
    xhrRequest: null,
    loaded: false,
    success: false,
    status: null,
    statusText: null,
    responseText: null,
    requestHeaders: null,
    responseHeaders: null,
    startTime: null,
    duration: null,
    logRow: null,
    send: function () {
        $.glimpseAjax.callStarted(this);
    },
    finish: function () {
        $.glimpseAjax.callFinished(this);
    }
};

var XMLHttpRequestWrapper = function (activeXObject) {
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper internal variables

    var xhrRequest = (typeof activeXObject != "undefined" ? activeXObject : new _XMLHttpRequest()),
        spy = new XHRSpy(), that = this;

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper internal methods

    var finishXHR = function () {
        var duration = new Date().getTime() - spy.startTime;
        var success = xhrRequest.status == 200;

        //Pull out the header information
        var responseHeadersText = xhrRequest.getAllResponseHeaders();
        var responses = responseHeadersText ? responseHeadersText.split(/[\n\r]/) : [];
        var reHeader = /^(\S+):\s*(.*)/;
        for (var i = 0, l = responses.length; i < l; i++) {
            var match = responses[i].match(reHeader);
            if (match)
                spy.responseHeaders[match[1]] = match[2];
        }

        //Trigger the finish a bit latter
        setTimeout(function () { spy.finish(); }, 200);

        //Get the rest of the information
        spy.success = success;
        spy.loaded = true;
        spy.status = xhrRequest.status;
        spy.statusText = xhrRequest.statusText;
        spy.responseText = xhrRequest.responseText;
        spy.duration = duration;
    };

    var handleStateChange = function () {
        that.readyState = xhrRequest.readyState;

        if (xhrRequest.readyState == 4) {
            that.statusText = xhrRequest.statusText;
            that.status = xhrRequest.status;
            that.response = xhrRequest.response;
            that.responseText = xhrRequest.responseText;
            that.responseType = xhrRequest.responseType;
            that.responseXML = xhrRequest.responseXML;

            finishXHR();
            xhrRequest.onreadystatechange = function () { };
        }
        that.onreadystatechange();
    };

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper public properties and handlers

    this.readyState = 0;

    this.onreadystatechange = function () { };

    this.response = null;
    this.responseText = null;
    this.responseType = null;
    this.responseXML = null;
    this.status = null;
    this.statusText = null;

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper public methods

    this.open = function (method, url, async) {
        if (spy.loaded)
            spy = new XHRSpy();
        spy.method = method;
        spy.url = url;
        spy.href = url;
        spy.async = async;
        spy.xhrRequest = xhrRequest;
        //spy.urlParams = parseURLParamsArray(url);

        if (!$.browser.msie && async)                                                 //TODO: Change over to jQuery
            xhrRequest.onreadystatechange = handleStateChange;

        // xhr.open.apply not available in IE
        if (xhrRequest.open.apply)                                              //TODO: Need to see if this applies
            xhrRequest.open.apply(xhrRequest, arguments)
        else
            xhrRequest.open(method, url, async);

        if ($.browser.msie && async)                                                 //TODO: Change over to jQuery
            xhrRequest.onreadystatechange = handleStateChange;

    };

    this.send = function (data) {
        spy.data = data;
        spy.startTime = new Date().getTime();

        try {
            xhrRequest.send(data);
        }
        catch (e) {
            throw e;
        }
        finally {
            spy.send();
            if (!spy.async) {
                that.readyState = xhrRequest.readyState;
                finishXHR();
            }
        }
    };

    this.setRequestHeader = function (header, value) {
        spy.requestHeaders[header] = value;
        xhrRequest.setRequestHeader(header, value);
    };

    this.getResponseHeader = function (header) {
        return xhrRequest.getResponseHeader(header);
    };

    this.getAllResponseHeaders = function () {
        return xhrRequest.getAllResponseHeaders();
    };

    this.abort = function () {
        return xhrRequest.abort();
    };

    return this;
};

// ************************************************************************************************
// Reguster XMLHttpRequest Wrapper / ActiveXObject Wrapper (IE6 only)

var _ActiveXObject;
if ($.browser.msie && $.browser.version == "6.0") {
    window._ActiveXObject = window.ActiveXObject;
    window.ActiveXObject = function (name) {
        var error = null;

        try {
            var activeXObject = new window._ActiveXObject(name);
        }
        catch (e) {
            error = e;
        }
        finally {
            if (!error) {
                var xhrObjects = " MSXML2.XMLHTTP.5.0 MSXML2.XMLHTTP.4.0 MSXML2.XMLHTTP.3.0 MSXML2.XMLHTTP Microsoft.XMLHTTP ";
                if (xhrObjects.indexOf(" " + name + " ") != -1)
                    return new XMLHttpRequestWrapper(activeXObject);
                else
                    return activeXObject;
            }
            else
                throw error.message;
        }
    };
}
else {
    var _XMLHttpRequest = XMLHttpRequest;
    window.XMLHttpRequest = function () {
        return new XMLHttpRequestWrapper();
    }
}















var glimpse;
if (window.jQuery) {
    (function ($) {

    var glimpseCss = '.glimpse, .glimpse *, .glimpse a, .glimpse td, .glimpse th, .glimpse table { font-family:Lucida Grande,Tahoma,sans-serif; background-color:transparent; font-size:11px; line-height:14px; border:0px; color:#232323; text-align:left; }.glimpse a, .glimpse a:hover, .glimpse a:visited { color:#2200C1; text-decoration:underline; font-weight:normal; }.glimpse a:active { color:#c11; text-decoration:underline; font-weight:normal; }.glimpse th { font-weight:bold; }.glimpse-open { position:fixed; right:0; bottom:0; height:27px; width:28px; border-left: 1px solid #ACA899; border-top: 1px solid #ACA899; background:#EEE; background:-moz-linear-gradient(top, #FFFFFF 0%, #EEEEEE 4%, #F3F5F7 8%, #E9E8DD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#FFFFFF), color-stop(4%,#EEEEEE), color-stop(8%,#F3F5F7), color-stop(100%,#E9E8DD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#FFFFFF\', endColorstr=\'#E9E8DD\',GradientType=0 ); }.glimpse-icon { background:url(\x2FGlimpse\x2FglimpseSprite.png) 0px -16px; height:20px; width:20px; margin: 3px 4px 0; cursor:pointer; }.glimpse-holder { display:none; z-index:10000; height:0; position:fixed; bottom:0; left:0; width:100%; background-color:#fff; }.glimpse-bar { height:27px; border-top:1px solid #ACA899; background:#FFFFFF; background:-moz-linear-gradient(top, #FFFFFF 0%, #EEEEEE 4%, #F3F5F7 8%, #E9E8DD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#FFFFFF), color-stop(4%,#EEEEEE), color-stop(8%,#F3F5F7), color-stop(100%,#E9E8DD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#FFFFFF\', endColorstr=\'#E9E8DD\',GradientType=0 ); }.glimpse-bar .glimpse-icon { margin-top:4px; float:left; cursor:default; }.glimpse-buttons { float:right; height:17px; width:50px; padding:6px; }.glimpse-title { margin:5px 0 0 15px; font-weight:bold; display:inline-block; width:75%; overflow:hidden; }.glimpse-title span:first-child { display:inline-block; height:20px; }.glimpse-title span:last-child { font-weight:normal; font-style:italic; padding-left:10px; width:60%; white-space:nowrap; display:inline-block; height:20px; }.glimpse-close, .glimpse-close:hover, .glimpse-terminate, .glimpse-terminate:hover, .glimpse-popout, .glimpse-popout:hover { background-image:url(\x2FGlimpse\x2FglimpseSprite.png); background-repeat:no-repeat; height:14px; width:14px; margin-left:2px; display:inline-block; }.glimpse-close { background-position:-1px -1px; }.glimpse-close:hover { background-position:-17px -1px; }.glimpse-terminate { background-position:-65px -1px; }.glimpse-terminate:hover { background-position:-81px -1px; } .glimpse-popout { background-position:-1px -1px; }.glimpse-popout:hover { background-position:-17px -1px; } .glimpse-tabs { height:24px; font-weight:bold; border-bottom:1px solid #ACA899; border-top:1px solid #CDCABB; background:#B9B7AF; background:-moz-linear-gradient(top, #B9B7AF 0%, #DAD8C8 4%, #D7D4C5 10%, #E9E6D5 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#B9B7AF), color-stop(4%,#DAD8C8), color-stop(10%,#D7D4C5), color-stop(100%,#E9E6D5)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#B9B7AF\', endColorstr=\'#E9E6D5\',GradientType=0 );}.glimpse-tabs ul { margin:4px 0px 0 0; padding:0px; }.glimpse-tabs li { display:inline; margin:0 2px 3px 2px; height:22px; padding:4px 9px 3px; color:#565656; cursor:pointer; border-radius: 0px 0px 3px 3px; -moz-border-radius: 0px 0px 3px 3px; -webkit-border-bottom-right-radius: 3px; -webkit-border-bottom-left-radius: 3px; }.glimpse-tabs li.glimpse-active { padding:4px 8px 3px; color:#000; border-left:1px solid #A4A4A4; border-bottom:1px solid #A4A4A4; border-right:1px solid #A4A4A4; background:#F7F6F1; background:-moz-linear-gradient(top, #F2F1EC 0%, #F2F1EC 3%, #EFEEE9 7%, #E8E7E1 51%, #F7F6F1 92%, #F1F0EB 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#F2F1EC), color-stop(3%,#F2F1EC), color-stop(7%,#EFEEE9), color-stop(51%,#E8E7E1), color-stop(92%,#F7F6F1), color-stop(100%,#F1F0EB)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#EFEEE9\', endColorstr=\'#F7F6F1\',GradientType=0 ); } .glimpse-tabs li.glimpse-hover { padding:4px 8px 3px; border-left:1px solid #BFBDB1; border-bottom:1px solid #BFBDB1; border-right:1px solid #BFBDB1; background:#EEECE3; background:-moz-linear-gradient(top, #BFBDB1 0%, #DAD9CB 4%, #D8D5C9 8%, #E8E7E1 51%, #F0EEE4 92%, #EDEBE1 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#BFBDB1), color-stop(4%,#DAD9CB), color-stop(8%,#D8D5C9), color-stop(51%,#E8E7E1), color-stop(92%,#F0EEE4), color-stop(100%,#EDEBE1)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#D8D5C9\', endColorstr=\'#F0EEE4\',GradientType=0 ); }.glimpse-tabs li.glimpse-disabled { color:#AAA; cursor:default; }.glimpse-panel-holder {}.glimpse-panel { display:none; overflow:auto; position:relative; } .glimpse-panel-message { text-align:center; padding-top:40px; font-size:1.1em; color:#AAA; }.glimpse-panel table { border-spacing:0; width:100%; }.glimpse-panel table td, .glimpse-panel table th { padding:3px 4px; text-align:left; vertical-align:top; } .glimpse-panel .glimpse-row-header-0 { height:19px; border-bottom:1px solid #9C9C9C; background:#C6C6C6; background:-moz-linear-gradient(top, #DEDEDE 0%, #BDBDBD 80%, #BBB 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#DEDEDE), color-stop(80%,#BDBDBD), color-stop(100%,#BBB)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#DEDEDE\', endColorstr=\'#BBB\',GradientType=0 ); }.glimpse-panel .glimpse-row-header-0 th { border-left:1px solid #D9D9D9; border-right:1px solid #9C9C9C; }.glimpse-panel .glimpse-cell-key { width:30%; max-width:150px; }.glimpse-panel table table { border:1px solid #D9D9D9; } .glimpse-panel table table thead tr { height:17px; border-bottom:1px solid #9C9C9C; background:#C6C6C6; background:-moz-linear-gradient(top, #F1F1F1 0%, #DFDFDF 80%, #DDD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#F1F1F1), color-stop(80%,#DFDFDF), color-stop(100%,#DDD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#F1F1F1\', endColorstr=\'#DDD\',GradientType=0 ); }.glimpse-panel table table thead tr th { border-left:1px solid #C6C6C6; border-right:1px solid #D9D9D9; padding:1px 4px 2px 4px; }.glimpse-panel table table thead tr th:first-child { border-left:0px; }.glimpse-panel table table thead tr th:last-child { border-right:0px; }.glimpse-panel .even { background:#F4F4F4; }.glimpse-panel .odd { background:#F9F9F9; }.glimpse-panel table table tbody th { font-weight:normal; font-style:italic; }.glimpse-panel table table thead th { font-weight:bold; font-style:normal; }.glimpse-panel .glimpse-side-sub-panel { right:0; z-index:10; background-color:#F5F5F5; height:100%; width:25%; border-left:1px solid #ACA899; position:absolute; }.glimpse-panel .glimpse-side-main-panel { position:relative; height:100%; width:75%; float:left; } .glimpse-panel-holder .glimpse-active { display:block; }.glimpse-resizer { height:4px; cursor:n-resize; width:100%; position:absolute; top:-1px; }li.glimpse-permanent { font-style:italic; padding:4px 8px 3px; border-bottom:1px solid #ACA899; border-left:1px solid #CDCABB; border-right:1px solid #CDCABB; background:#B9B7AF; background:-moz-linear-gradient(top, #B9B7AF 0%, #DAD8C8 4%, #D7D4C5 10%, #E9E6D5 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#B9B7AF), color-stop(4%,#DAD8C8), color-stop(10%,#D7D4C5), color-stop(100%,#E9E6D5)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#B9B7AF\', endColorstr=\'#E9E6D5\',GradientType=0 ); }.glimpse-preview-object { color:#006400; } .glimpse-preview-string { color:#006400 !important; font-weight:normal !important; } .glimpse-preview-string span { padding-left:1px; }.glimpse-preview-object span { font-weight:bold; color:#444; } .glimpse-preview-object span.start { margin-right:5px; } .glimpse-preview-object span.end { margin-left:5px; }.glimpse-preview-object span.rspace { margin-right:4px; }.glimpse-preview-object span.mspace { margin:0 4px; }.glimpse-preview-object span.small { font-size:0.95em; } .glimpse-expand { height:11px; width:11px; display:inline-block; float:left; margin:1px 0 0 -13px; cursor:pointer; background-image:url(\x2FGlimpse\x2FglimpseSprite.png); background-repeat:no-repeat; background-position:-96px 0; }.glimpse-collapse { background-position:-96px -11px; }.glimpse-preview-show { display:none; font-weight:normal !important; }.glimpse-panel .quiet * { color:#AAA; }.glimpse-panel .suppress { text-decoration:line-through; }.glimpse-panel .suppress * { color:#AAA; }.glimpse-panel .selected { background-color:#FFFF99; }.glimpse-panel .selected * { color:#409B3B; }.glimpse-panel .info .icon, .glimpse-panel .warn .icon, .glimpse-panel .loading .icon, .glimpse-panel .error .icon, .glimpse-panel .fail .icon { width:14px; height:14px; background-image:url(\x2FGlimpse\x2FglimpseSprite.png); background-repeat:no-repeat; display:inline-block; margin-right: 5px; } .glimpse-panel .info .icon { background-position: -22px -22px; }.glimpse-panel .warn .icon { background-position:-36px -22px; }.glimpse-panel .loading .icon { background-position:-78px -22px; }.glimpse-panel .error .icon { background-position:-50px -22px; } .glimpse-panel .fail .icon { background-position:-64px -22px; }.glimpse-panel .info * { color:#067CE5; }.glimpse-panel .warn * { color:#FE850C; } .glimpse-panel .error * { color:#B40000; }.glimpse-panel .fail * { color:#D00; font-weight:bold; }.glimpse-panelitem-XHReqests .loading .icon { float:right; }.glimpse-panelitem-Remote .glimpse-side-sub-panel .loading, .glimpse-panelitem-Remote .glimpse-side-main-panel .loading { position:absolute; bottom:5px; right:5px; color:#777; }';
    $('<style type="text/css"> ' + glimpseCss + ' </style>').appendTo("head");      //http://stackoverflow.com/questions/1212500/jquery-create-css-rule-class-runtime

    $.glimpse = {};
    $.glimpseProcessor = {};
    $.glimpseContent = {};
    $.glimpseResize = {};

    $.extend($.fn, {
        resizer: function() {
            return this.each(function() {
                var gr = $.glimpseResize;
                gr.static.anchor = $(this).bind("mousedown", { el: $(this).parent() }, gr.startDrag);
            });
        },
        sortElements: (function() {
            var sort = [].sort;
            return function(comparator, getSortable) {
                getSortable = getSortable || function() { return this; };
                comparator = comparator || function(a, b) { return $(a).data('sort') > $(b).data('sort') ? 1 : -1; };
                var placements = this.map(function() {
                    var sortElement = getSortable.call(this), parentNode = sortElement.parentNode, nextSibling = parentNode.insertBefore(document.createTextNode(''), sortElement.nextSibling);
                    return function() {
                        if (parentNode === this) {
                            throw new Error("You can't sort elements if any one is a descendant of another.");
                        }
                        parentNode.insertBefore(this, nextSibling);
                        parentNode.removeChild(nextSibling);
                    };
                });
                return sort.call(this, comparator).each(function(i) {
                    placements[i].call(getSortable.call(this));
                });
            };
        })()
    });

    $.extend({
        htmlEncode: function(value) {
            return !(value == undefined || value == null) ? $('<div/>').text(value).html() : '';
        },
        htmlDecode: function(value) {
            return !(value == undefined || value == null) ? $('<div/>').html(value).text() : '';
        },
        lengthJson: function(data) {
            var count = 0;
            if ($.isPlainObject(data))
                $.each(data, function(k, v) { count++; });
            return count;
        },
        formatTime: function(d) {
            if (typeof d === 'number')
                d = new Date(d);
            var padding = function(t) { return t < 10 ? '0' + t : t; }
            return d.getHours() + ':' + padding(d.getMinutes()) + ':' + padding(d.getSeconds()) + ' ' + d.getMilliseconds();
        },
        cookie: function(key, value) {
            key = encodeURIComponent(key)
            //Set Cookie
            if (arguments.length > 1) {
                var t = new Date();
                t.setDate(t.getDate() + 1000);

                value = $.isPlainObject(value) ? JSON.stringify(value) : String(value);
                return (document.cookie = key + '=' + encodeURIComponent(value) + '; expires=' + t.toUTCString() + '; path=/');
            }

            //Get cookie 
            var result = new RegExp("(?:^|; )" + key + "=([^;]*)").exec(document.cookie);
            if (result) {
                result = decodeURIComponent(result[1]);
                if (result.substr(0, 1) == '{')
                    result = JSON.parse(result);
                return result;
            }
            return null;
        },
        popupExists : function(popup) { 
            var successfull = (popup && !popup.closed && typeof popup.closed != 'undefined');  
            if (!successfull) 
                alert("Glimpse Error: Glimpse popup was blocked.");  
            return successfull
        }
    }); 


    $.extend($.glimpseResize, {
        static: {
            anchor: null,
            staticOffset: null,
            lastMousePos: 0,
            min: 50,
            endDragCallback: function(height) { } 
        },
        startDrag: function(e) {
            var gr = $.glimpseResize, o = gr.static;
            o.anchor = $(e.data.el);
            o.lastMousePos = gr.mousePosition(e).y;
            o.staticOffset = o.anchor.height() + o.lastMousePos;
            o.anchor.css('opacity', 0.50);
            $(document).mousemove(gr.performDrag).mouseup(gr.endDrag);
            return false;
        },
        performDrag: function(e) {
            var gr = $.glimpseResize, o = gr.static;
            var mousePos = gr.mousePosition(e).y;
            var offsetMousePos = o.staticOffset - mousePos;
            if (o.lastMousePos >= mousePos) {
                offsetMousePos += 4;
            }
            offsetMousePos = Math.max(o.min, offsetMousePos);
            o.anchor.height(offsetMousePos + 'px');
            o.lastMousePos = mousePos;
            if (offsetMousePos < o.min) {
                gr.endDrag(e);
            }
            return false;
        },
        endDrag: function(e) {
            var gr = $.glimpseResize, o = gr.static;
            $(document).unbind('mousemove', gr.performDrag).unbind('mouseup', gr.endDrag);
            o.anchor.css('opacity', 1);
            o.anchor = null;
            o.staticOffset = null;
            o.lastMousePos = 0;
            o.endDragCallback();
        },
        mousePosition: function(e) {
            var d = document.documentElement;
            return { x: e.clientX + d.scrollLeft, y: e.clientY + d.scrollTop };
        }
    });

    $.extend($.glimpseProcessor, {
        layout: function(g, title) {
            var that = this, static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder();

            var start = new Date().getTime();

            //Build Dynamic HTML
            for (var key in static.data) {
                if ($('.glimpse-tabitem-' + key, tabStrip).length == 0) {
                    that.addTab(tabStrip, static.data[key], key);
                    that.addTabBody(panelHolder, that.build(static.data[key], 0), key);
                }
            }

            $('li', tabStrip).sortElements();
            $('.glimpse-panel', panelHolder).sortElements();
             
            //Set Inital State - TODO: don't like how this works... need to review
            $('.info td:first-child, .warn td:first-child, tr.error td:first-child, .fail td:first-child, .loading td:first-child', $('.glimpse-panel')).not(':has(.icon)').prepend('<div class="icon"></div>');

            //Select tab
            that.restoreTab(g);

            $('.glimpse-title').html(title);
        },
        addTab: function(container, data, key) {
            var disabled = (data === undefined || data === null) ? ' glimpse-disabled' : '';
            container.append('<li class="glimpse-tabitem-' + key + disabled + '" data-sort="' + key + '">' + key + '</li>');
        },
        addTabBody: function(container, content, key) {
            container.append('<div class="glimpse-panel glimpse-panelitem-' + key + '" data-sort="' + key + '">' + content + '</div>');
        },
        restoreTab : function(g) {
            var static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder(), activeTab = g.settings.activeTab; 
            
            $('.glimpse-active', tabStrip).removeClass('glimpse-active').removeClass('glimpse-hover'); 
            $((activeTab ? '.glimpse-tabitem-' + activeTab : 'li:first'), tabStrip).addClass('glimpse-active');

            $('.glimpse-active', panelHolder).removeClass('glimpse-active'); 
            $((activeTab ? '.glimpse-panelitem-' + activeTab : '.glimpse-panel:first'), panelHolder).addClass('glimpse-active'); 
        }, 
        clearLayout: function(g) {
            var that = this, static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder();

            that.removeTabs(tabStrip);
            that.removeTabBodies(panelHolder);
        },
        removeTabs: function(container) {
            $('li:not(.glimpse-permanent)', container).remove();
        },
        removeTabBodies: function(container) {
            $('.glimpse-panel:not(.glimpse-permanent)', container).remove();
        },
        build: function(data, level) {
            var that = this, result = '';

            if ($.isArray(data))
                result = that.buildCustomTable(data, level);
            else if ($.isPlainObject(data))
                result = that.buildKeyValueTable(data, level);
            else if (level == 0) {
                if (data === undefined || data === null || data === '')
                    result = '';
                else
                    result = '<div class="glimpse-panel-message">' + data + '</div>';
            }
            else
                result = that.buildString(data, level);

            return result;
        },
        buildHeading: function(url, clientName, type) {
            var clean = function(data) {
                return (data === undefined || data === null || data === "null") ? '' : data;
            }
            type = clean(type);
            clientName = clean(clientName);
            return '<span>' + clientName + ((type.length > 0) ? ' (' + type + ')' : '') + '&nbsp;</span><span>' + url + '</span>';
        },
        buildKeyValueTable: function(data, level, forceFull) {
            var that = this, limit = 3;
            if (((level > 0 && $.lengthJson(data) > (limit + 1)) || level > 1) && !forceFull)
                return that.buildKeyValuePreview(data, limit);

            var i = 1, html = '<table><thead><tr class="glimpse-row-header-' + level + '"><th class="glimpse-cell-key">Key</th><th class="glimpse-cell-value">Value</th></tr></thead>';
            for (var key in data)
                html += '<tr class="' + (i++ % 2 ? 'odd' : 'even') + '"><th width="30%">' + $.glimpseContent.formatString(key) + '</th><td width="70%"> ' + that.build(data[key], level + 1) + '</td></tr>';
            html += '</table>';
            return html;
        },
        buildCustomTable: function(data, level, forceFull) {
            var that = this, limit = 3;
            if (((level > 0 && data.length > (limit + 1)) || level > 1) && !forceFull)
                return that.buildCustomPreview(data, limit);

            var html = '<table><thead><tr class="glimpse-row-header-' + level + '">';
            for (var x = 0; x < data[0].length; x++)
                html += '<th>' + $.glimpseContent.formatString(data[0][x]) + '</th>';
            html += '</tr></thead>';
            for (var i = 1; i < data.length; i++) {
                html += '<tr class="' + (i % 2 ? 'odd' : 'even') + (data[i].length > data[0].length ? ' ' + data[i][data[i].length - 1] : '') + '">';
                for (var x = 0; x < data[0].length; x++)
                    html += '<td>' + that.build(data[i][x], level + 1) + '</td>';
                html += '</tr>';
            }
            html += '</table>';
            return html;
        },
        buildString: function(data, level) {
            return this.buildStringPreview(data, level);
        },
        buildKeyValuePreview: function(data, level) {
            var that = this, length = $.lengthJson(data), rowMax = 2, rowLimit = (rowMax < length ? rowMax : length), i = 1, html = '<span class="glimpse-expand"></span><span class="glimpse-preview-object"><span class="start">{</span>';
            for (var key in data) {
                html += that.newItemSpacer(i, rowLimit, length);
                if (i > length || i++ > rowLimit)
                    break;
                html += '<span>\'</span>' + that.buildStringPreview(key, level + 1) + '<span>\'</span><span class="mspace">:</span><span>\'</span>' + that.buildStringPreview(data[key], level + 99) + '<span>\'</span>';
            }
            html += '<span class="end">}</span></span><span class="glimpse-preview-show">' + that.buildKeyValueTable(data, level, true) + '</span>';
            return html;
        },
        buildCustomPreview: function(data, level) {
            var that = this, length = data.length - 1, rowMax = 2, columnMax = 3, columnLimit = ((data[0].length > columnMax) ? columnMax : data[0].length), rowLimit = (rowMax < length ? rowMax : length), html = '<span class="glimpse-expand"></span><span class="glimpse-preview-object"><span class="start">[</span>';

            for (var i = 1; i <= rowLimit + 1; i++) {
                html += that.newItemSpacer(i, rowLimit, length);
                if (i > length || i > rowLimit)
                    break;

                html += '<span class="start">[</span>';
                var spacer = '';
                for (var x = 0; x < columnLimit; x++) {
                    html += spacer + '<span>\'</span>' + that.buildStringPreview(data[i][x], level + 99) + '<span>\'</span>';
                    spacer = '<span class="rspace">,</span>';
                }
                if (x < data[0].length)
                    html += spacer + '<span>...</span>'
                html += '<span class="end">]</span>';
            }
            html += '<span class="end">]</span></span><span class="glimpse-preview-show">' + that.buildCustomTable(data, level, true) + '</span>';
            return html;
        },
        buildStringPreview: function(data, level) {
            if (data == undefined || data == null)
                return '--';
            if ($.isArray(data))
                return "[ ... ]";
            if ($.isPlainObject(data))
                return "{ ... }";

            var that = this,
                charMax = (level > 100 ? 12 : level > 1 ? 80 : 150),
                charOuterMax = (charMax * 1.2),
                content = $.glimpseContent.trimFormatString(data, charMax, charOuterMax, true);

            if (data.length > charOuterMax) {
                content = '<span class="glimpse-preview-string" title="' + $.glimpseContent.trimFormatString(data, charMax * 2, charMax * 2.1, false, true) + '">' + content + '</span>';
                if (level < 100)
                    content = '<span class="glimpse-expand"></span>' + content + '<span class="glimpse-preview-show">' + $.glimpseContent.formatString(data) + '</span>';
            }
            return content;
        },
        newItemSpacer: function(currentRow, rowLimit, dataLength) {
            var html = '';
            if (rowLimit != dataLength) {
                if (currentRow > 1)
                    html += '<span class="rspace">,</span>';
                if (currentRow > rowLimit)
                    html += '<span class="small">length=' + dataLength + '</span>';
            }
            return html;
        }
    });

    $.extend($.glimpseContent, {
        formatStringTypes: {
            italics: {
                match: function(d) { return d.match(/^\_[\w\D]+\_$/) != null; },
                replace: function(d) { return '<u>' + $.htmlEncode($.glimpseContent.scrub(d)) + '</u>'; },
                trimmable: true
            },
            underline: {
                match: function(d) { return d.match(/^\\[\w\D]+\\$/) != null; },
                replace: function(d) { return '<em>' + $.htmlEncode($.glimpseContent.scrub(d)) + '</em>'; },
                trimmable: true
            },
            strong: {
                match: function(d) { return d.match(/^\*[\w\D]+\*$/) != null; },
                replace: function(d) { return '<strong>' + $.htmlEncode($.glimpseContent.scrub(d)) + '</strong>'; },
                trimmable: true
            },
            raw: {
                match: function(d) { return d.match(/^\![\w\D]+\!$/) != null; },
                replace: function(d) { return $.glimpseContent.scrub(d); },
                trimmable: false
            }
        },
        scrub: function(d) {
            return d.substr(1, d.length - 2);
        },
        formatString: function(data) {
            var that = this;
            return that.trimFormatString(data);
        },
        trimFormatString: function(data, charMax, charOuterMax, wrapEllipsis, skipEncoding) {
            var that = this, trimmable = true, replace = function(d) { return $.htmlEncode(d); };

            if (data == undefined || data == null)
                return '--';
            data = $.trim(data);

            if (!skipEncoding) {
                for (var typeKey in that.formatStringTypes) {
                    var type = that.formatStringTypes[typeKey];
                    if (type.match(data)) {
                        replace = type.replace;
                        trimmable = type.trimmable;
                        break;
                    }
                }
            }

            if (trimmable && charOuterMax && data.length > charOuterMax)
                return replace(data.substr(0, charMax)) + (wrapEllipsis ? '<span>...</span>' : '...');
            return replace(data);
        }
    });

    $.extend($.glimpse, {
        _executeProtocolListeners: function(g, isInit) {
            var i = 0, listeners = g.plugins.protocolListeners, data = g.static.data;
            for (; i < listeners.length; i++) {
                var listener = listeners[i];
                if (isInit || !listener.onInitOnly)
                    listener.callback(data)
            }
        },
        _executeLayoutListeners: function(g, isInit) {
            var i = 0, listeners = g.plugins.layoutListeners, static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder();
            for (; i < listeners.length; i++) {
                var listener = listeners[i];
                if (isInit || !listener.onInitOnly)
                    listener.callback(tabStrip, panelHolder)
            }
        },
        _wireEvents: function(g) {
            var static = g.static, settings = g.settings;

            //Open/Close Holder
            $('.glimpse-open').live('click', function() { g.open(); });
            $('.glimpse-close').live('click', function() { g.close(); });
            $('.glimpse-terminate').live('click', function() { g.terminate(); });
            $('.glimpse-popout').live('click', function() { g.popout(); });

            //Tab Switching 
            $('.glimpse-tabs li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function(e) {
                var item = $(this);
                if (e.type == 'mouseover') { item.addClass('glimpse-hover'); } else { item.removeClass('glimpse-hover'); }
            });
            $('.glimpse-tabs li:not(.glimpse-active, .glimpse-disabled)').live('click', function() {
                var item = $(this);

                //Setup Tabs
                $('.glimpse-tabs .glimpse-active').removeClass('glimpse-active').removeClass('glimpse-hover');
                item.addClass('glimpse-active');

                //Setup Panels 
                $('.glimpse-panel-holder .glimpse-active').removeClass('glimpse-active');
                $('.glimpse-panel-holder .glimpse-panel:eq(' + $('.glimpse-tabs ul li').index(item) + ')').addClass('glimpse-active');

                //Save selection
                settings.activeTab = item.data('sort');
                g.persistState();
            });

            //Resize
            $('.glimpse-resizer').resizer(settings.height);

            //Exspand/Collapse
            $('.glimpse-expand').live('click', function() {
                $(this).toggleClass('glimpse-collapse').next().toggle().next().toggle();
            });

            //Resize panels if we are in popup
            if (static.isPopup) { 
                $(window).resize(function() { 
                    $('.glimpse-holder .glimpse-panel').height($(window).height() - 54);
                });
            }
        },
        _wireCallback: function(g) {
            //Remember height 
            $.glimpseResize.static.endDragCallback = function() {
                g.settings.height = $('.glimpse-holder').height();
                g.persistState();

                $('.glimpse-spacer').height(g.settings.height);
                $('.glimpse-holder .glimpse-panel').height(g.settings.height - 54);
            }
        },
        _adjustLayout: function(g) {
            $('.glimpse-spacer').height(g.settings.height);
            $('.glimpse-holder .glimpse-panel').height(g.settings.height - 54);
        },
        addProtocolListener: function(callback, onInitOnly) {
            $.glimpse.plugins.protocolListeners.push({ 'callback': callback, 'onInitOnly': onInitOnly });
        },
        addLayoutListener: function(callback, onInitOnly) {
            $.glimpse.plugins.layoutListeners.push({ 'callback': callback, 'onInitOnly': onInitOnly });
        },
        open: function(speed, dontPersist) {
            var g = $.glimpse;

            if (!dontPersist) {
                g.settings.open = true;
                g.persistState();
            }

            $('.glimpse-open').hide();
            $('.glimpse-holder').show().animate({ 'height': g.settings.height }, (speed === undefined ? 'fast' : speed));
            g._adjustLayout(g);
        },
        close: function(speed) {
            var g = $.glimpse;

            g.settings.open = false;
            g.persistState();

            $('.glimpse-holder').animate({ 'height': '0' }, (speed === undefined ? 'fast' : speed), function() {
                $(this).hide();
                $('.glimpse-open').show();
            });
            $('.glimpse-spacer').height('0');
        },
        terminate: function() {
            $('.glimpse-open, .glimpse-spacer').remove();
            $('.glimpse-holder').animate({ 'height': '0' }, 'fast', function() {
                $(this).remove();
            });

            $.cookie('glimpseState', null);
            $.cookie('glimpseClientName', null);
            $.cookie('glimpseOptions', null);
        },
        popout: function() {
            var g = this, static = g.static;

            if (!static.popup || static.popup.closed) {
                g.settings.popupOn = true;
                g.persistState();

                var url = static.popupUrl + '?glimpseRequestID=' + $('#glimpseData').data('glimpse-requestID');
                static.popup = window.open(url, 'GlimpsePopup', 'width=1000,height=600,0,status=0,scrollbars=1,dialog=1');

                if ($.popupExists(static.popup))
                    g.close(undefined, true); 
            } 
        },
        persistState: function() {
            var g = $.glimpse;
            $.cookie('glimpseOptions', g.settings);
        },
        restoreState: function() {
            var g = $.glimpse;
            if (g.settings.open)
                g.open(0);
        },
        refresh: function(data, title) {
            if (!data) return;

            var g = $.glimpse, static = g.static;
            static.data = data;

            g._executeProtocolListeners(g, false);

            $.glimpseProcessor.clearLayout(g);
            $.glimpseProcessor.layout(g, title);
            g._adjustLayout(g);

            g._executeLayoutListeners(g, false);
        },
        reset: function() {
            var g = this, static = g.static;
            //$('.glimpse').trigger('reset.refresh.glimpse');
            this.refresh(glimpse, $.glimpseProcessor.buildHeading(static.url, static.clientName, ''));
        },
        init: function(data) {
            var g = $.glimpse, static = g.static;
            static.isPopup = window.location.pathname.indexOf(this.static.popupUrl) > -1;

            if (!data) {
                if (static.isPopup && window.opener.glimpse) {
                    $.glimpse.static.url = window.opener.$.glimpse.static.url;
                    glimpse = data = window.opener.glimpse
                }
                else 
                    return;
            }

            g.static.data = data; 
            g.settings = $.extend(g.settings, $.cookie('glimpseOptions'));

            g._executeProtocolListeners(g, true);

            $('body').append(static.html.plugin);

            g._wireEvents(g);
            g._wireCallback(g);

            $.glimpseProcessor.layout(g, $.glimpseProcessor.buildHeading(static.url, static.clientName, ''));

            g._executeLayoutListeners(g, true);

            if (!static.isPopup)
                $('body').append('<div class="glimpse-spacer"></div>');

            g.restoreState();

            if (!static.isPopup && g.settings.popupOn)
                g.popout(); 
        },
        plugins: {
            protocolListeners : [],
            layoutListeners : []
        },
        settings: {
            open : false,
            height : 300,
            activeTab : 'Routes',
            popupOn : false
        },
        static: {
            data : null,
            url : window.location.href.replace(window.location.protocol + '//' + window.location.host, ''),
            clientName : $.cookie('glimpseClientName'),
            html : { plugin: '<div class="glimpse-open"><div class="glimpse-icon"></div></div><div class="glimpse-holder glimpse"><div class="glimpse-resizer"></div><div class="glimpse-bar"><div class="glimpse-icon"></div><div class="glimpse-title"></div><div class="glimpse-buttons"><a href="#" title="Pop Out" class="glimpse-popout"></a><a href="#" title="Shutdown/Terminate" class="glimpse-terminate"></a><a href="#" title="Close/Minimize" class="glimpse-close"></a></div></div><div class="glimpse-content"><div class="glimpse-tabs"><ul></ul></div><div class="glimpse-panel-holder"></div></div></div>' },
            tabStrip : function() { return $('.glimpse-tabs ul'); },
            panelHolder : function() { return $('.glimpse-panel-holder'); },
            isPopup : false,
            popupUrl : '/Glimpse/Popup',
            popup : null
        }
    });


    //Run glimpse 
    $(document).ready(function() {
        $.glimpse.init(glimpse);
    });












    $.glimpseAjax = {};
    $.extend($.glimpseAjax, {
        init: function() {
            var ga = this;

            //Wire up plugin
            $.glimpse.addProtocolListener(function(data) { ga.adjustProtocol(data); }, true);
            $.glimpse.addLayoutListener(function(tabStrip, panelHolder) { ga.adjustLayout(tabStrip, panelHolder); }, true);
        },
        adjustProtocol: function(data) {
            var ga = this;
            data[ga.static.key] = ''
        },
        adjustLayout: function(tabStrip, panelHolder) {
            var ga = this, static = ga.static;
             
            //Setup layout
            static.tab = $('.glimpse-tabitem-' + static.key, tabStrip);
            static.panel = $('.glimpse-panelitem-' + static.key, panelHolder);
            static.tab.addClass('glimpse-permanent').text(static.key);
            static.panel.addClass('glimpse-permanent').html('<div class="glimpse-panel-message">No ajax calls have yet been detected</div>');
        },
        callStarted: function(ajaxSpy) {
            var g = $.glimpse, ga = this, static = ga.static, panelHolder = g.static.panelHolder(), panelItem = $('.glimpse-panelitem-' + static.key, panelHolder);

            if (ajaxSpy.url && ajaxSpy.url.length > 9 && ajaxSpy.url.substr(0, 9) == '/Glimpse/')
                return;

            //First time round we need to set everything up
            if ($('.glimpse-panel-message', panelItem).length > 0) {
                panelItem.html($.glimpseProcessor.build([
                    ['Request URL', 'Status', 'Date/Time', 'Druration', 'Is Async', 'Inspect'],
                    [window.location.pathname, '200', $.formatTime(new Date()), 'N/A', 'N/A', '!<a href="#">Reset</a>!']
                ], 0));

                $('a', static.panel).click(function(e) {
                    ga._handelClick($(this));
                    $.glimpse.reset();
                    return false;
                });
                $('tr:last', static.panel).addClass('selected');
            }
            ajaxSpy.clientName = $.cookie('glimpseClientName');

            //Add new row
            $('table', panelItem).append('<tr class="loading"><th><div class="icon"></div>' + ajaxSpy.url + '</th><td class="glimpse-ajax-status">Loading...</td><td>' + $.formatTime(ajaxSpy.startTime) + '</td><td class="glimpse-ajax-duration">--</td><td>' + ajaxSpy.async + '</td><td class="glimpse-ajax-inspect">N/A</td></tr>');

            //In theory I wouldn't need to do this every time but wanting to make sure that all rows are kept in sync
            $('table tbody tr:odd', panelItem).addClass('even');
            $('table tbody tr:even', panelItem).addClass('odd');

            ajaxSpy.logRow = $('tr:last', panelItem);
        },
        callFinished: function(ajaxSpy) {
            var ga = this, static = ga.static, row = ajaxSpy.logRow, glimpseRequestId = ajaxSpy.responseHeaders['X-Glimpse-RequestID'];

            if (ajaxSpy.url && ajaxSpy.url.length > 9 && ajaxSpy.url.substr(0, 9) == '/Glimpse/')
                return;

            //Adjust layout
            row.removeClass('loading').addClass(!ajaxSpy.success ? 'error' : glimpseRequestId ? 'ajax-loaded' : 'suppress');
            $('.glimpse-ajax-status', row).text(ajaxSpy.status);
            $('.glimpse-ajax-duration', row).text(ajaxSpy.duration + 'ms');
            if (glimpseRequestId) {
                var linkPlaceholder = $('.glimpse-ajax-inspect', row).html('<div class="icon"></div>Loading...').addClass('loading');
                
                $.ajax({ 
                    url: static.historyLink,
                    type: 'GET', 
                    data : { 'ClientRequestID' : glimpseRequestId },
                    contentType: 'application/json',
                    success: function(result) {   
                        linkPlaceholder.html('<a href="#">Launch</a>').children('a').click(function() {
                            ga._handelClick($(this)); 
                            //$('.glimpse').trigger('ajax.refresh.glimpse');
                            $.glimpse.refresh(eval('(' + result.Data[glimpseRequestId].Data + ')'), $.glimpseProcessor.buildHeading(ajaxSpy.url, ajaxSpy.clientName, ga.static.key));
                            return false;
                        });
                    }
                });   
            }
        },
        static: {
            key : 'XHReqests',
            tab : null,
            panel : null, 
            historyLink : '/Glimpse/History' 
        },
        _handelClick: function(link) { 
            $('.selected', link.parents('table:first')).removeClass('selected');
            link.parents('tr:first').addClass('selected');
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseAjax.init();













    $.glimpseRemote = {};
    $.extend($.glimpseRemote, { 
        init : function() { 
            var gr = this;

            //Wire up plugin
            $.glimpse.addProtocolListener(function(data) { gr.adjustProtocol(data); }, true);
            $.glimpse.addLayoutListener(function(tabStrip, panelHolder) { gr.adjustLayout(tabStrip, panelHolder); }, true);  
        },
        adjustProtocol : function(data) {
            var gr = this; 
            data[gr.static.key] = ''
        },
        adjustLayout : function(tabStrip, panelHolder) {
            var gr = this, static = gr.static;
            
            //Setup layout
            static.tab = $('.glimpse-tabitem-' + static.key, tabStrip);
            static.panel = $('.glimpse-panelitem-' + static.key, panelHolder);
             
            //Make sure we stick round 
            static.tab.addClass('glimpse-permanent').text(static.key); 
            static.panel.addClass('glimpse-permanent'); 
            static.panel.prepend('<div class="glimpse-side-sub-panel"><div class="loading"><div class="icon"></div><span>Refreshing...</span></div><div class="glimpse-content"></div></div><div class="glimpse-side-main-panel"><div class="glimpse-initial glimpse-panel-message">No remote calls have yet been detected</div><div class="loading" style="display:none"><div class="icon"></div><span>Refreshing...</span></div><div class="glimpse-content"></div></div>');

            static.subPanel = $('.glimpse-side-sub-panel', static.panel);
            static.mainPanel = $('.glimpse-side-main-panel', static.panel);

            //Wireevents 
            $('a', static.panel).live('click', function() {
                $('.selected', $(this).parents('table:first')).removeClass('selected');
                $(this).parents('tr:first').addClass('selected');
            }); 
            $('a.glimpse-trigger', static.subPanel).live('click', function() {
                gr.getClientHistoryList($(this).data('client')); 
                return false;
            });
            $('a.glimpse-orignal', static.subPanel).live('click', function() {
                gr.reset();
                return false;
            });
            $('a', static.mainPanel).live('click', function() { 
                //$('.glimpse').trigger('remote.refresh.glimpse');
                gr.activate($(this).data('client'), $(this).data('request'));
                return false;
            });
            static.tab.click(function() {
                gr.getClientList();
            }); 
            //$('.glimpse').bind('reset.refresh.glimpse', function() { gr.reset(true); }); 

            if ($.glimpse.settings.activeTab == static.key)
                gr.getClientList(); 
        },
        reset : function() { 
            var gr = this, static = gr.static; 

            $.glimpse.reset();  

            $('.glimpse-initial', static.mainPanel).show(); 
            $('.loading', static.mainPanel).hide(); 
            $('.glimpse-content', static.mainPanel).empty(); 
        },
        activate : function(client, request) { 
            var gr = this, static = gr.static; 

            var request = static.result.Data[client][request];
            if (request.Data) 
                $.glimpse.refresh(eval('(' + request.Data + ')'), $.glimpseProcessor.buildHeading(request.Url, client, static.key)); 
        },
        getClientHistoryList : function(clientId) { 
            var gr = this, static = gr.static, loading = $('.loading', static.mainPanel); 
     
            $.ajax({
                url: static.historyLink,
                type: 'GET', 
                data : { 'ClientName' : clientId },
                contentType: 'application/json',  
                beforeSend: function() { 
                    $('span', loading).text('Refreshing...').parent().fadeIn(); 
                },
                success: function(result) {  
                    $('span', loading).text('Loaded...').parent().delay(1500).fadeOut(); 
                    gr.processClientHistoryList(result);
                }
            });  
        },
        getClientList : function() { 
            var gr = this, static = gr.static, loading = $('.loading', static.subPanel); 

            $.ajax({
                url: static.clientLink,
                type: 'GET', 
                contentType: 'application/json',  
                beforeSend: function() { 
                    $('span', loading).text('Refreshing...').parent().fadeIn(); 
                },
                success: function(result) {  
                    $('span', loading).text('Loaded...').parent().delay(1500).fadeOut();  
                    gr.processClientList(result);
                }
            });
        },
        processClientHistoryList : function(result) {
            var gr = this, static = gr.static; 

            if (static.result && result) {
            
                $.extend(true, static.result, result);
                 
                //Pull out the name of the client
                var ldata = static.result.Data || {}, rdata = result.Data || {}, rclientToken = ''; 
                for (var key in rdata) {
                    rclientToken = key;
                    break;
                }
                
                //As long as the client  
                if ($(".selected a[data-client='" + rclientToken + "']", static.subPanel).length > 0) {
                    var lclient = ldata[rclientToken], data = [ [ 'Client Name', 'Method', 'Request Url', 'Browser', 'Date/Time', 'Is Ajax', 'Launch' ] ]; 
                    
                    for (var lclientRequestToken in lclient) {
                        var lclientRequest = lclient[lclientRequestToken];
                        if (lclientRequest.Data) 
                            data.push([ rclientToken, lclientRequest.Method, lclientRequest.Url, lclientRequest.Browser, lclientRequest.RequestTime, lclientRequest.IsAjax, '!<a href="#" data-request="' + lclientRequestToken + '" data-client="' + rclientToken + '">Launch</a>!' ]);
                    }

                    $('.glimpse-initial', static.mainPanel).hide();
                    $('.glimpse-content', static.mainPanel).html($.glimpseProcessor.build(data, 0)); 
                }
            } 
        },
        processClientList : function(result) { 
            var gr = this, static = gr.static; 
            
            //Create the table we need first time round 
            if ($('table', static.subPanel).length == 0) 
                $('.glimpse-content', static.subPanel).html($.glimpseProcessor.build([ [ 'Client', 'Count', 'Launch'], [ '\\--this--\\', 1, '!<a href="#" class="glimpse-orignal">Reset</a>!', 'selected' ] ], 0));
 
            if (static.result && result) {
                var shouldTriggerHistoryRequest = false, selectedClientName = $(".selected a", static.subPanel).data('client');

                //Adjusts the returned data
                $.extend(true, static.result, result);

                //Need to do some work on this data 
                var ldata = static.result.Data || {}, rdata = result.Data || {}; 
                for (var lclientToken in ldata) {
                    var lclient = ldata[lclientToken], rclient = rdata[lclientToken], count = 0;
                 
                    //Lets go through the client requests 
                    for (var lclientRequestToken in lclient) {
                        var lclientRequest = lclient[lclientRequestToken];   

                        //Remove any tokens that we don't have cached locally and the server does't have 
                        if (!(lclientRequest.Data || (rclient && rclient[lclientRequestToken])))
                            delete lclient[lclientRequestToken];   
                        else 
                            count++;
                        
                        //Detect if any new requests new requests have arrived, since this client was selected
                        if (selectedClientName == lclientToken && !lclientRequest.Detected) 
                            shouldTriggerHistoryRequest = true;
                        lclientRequest.Detected = true;
                    }
                 
                    //Lets update the UI, by updateing the counter, adding a now or removing a row
                    var clientRow = $("tr:has(a[data-client='" + lclientToken + "'])", static.subPanel); 
                    if (clientRow.length > 0) { 
                        //If the client count is 0 then the server has no data and neither does the client
                        if (count == 0) {
                            delete ldata[lclientToken];
                            clientRow.remove(); 
                        }
                        else 
                            $('td:nth-child(2)', clientRow).text(count);
                    }
                    else 
                        $('table', static.subPanel).append('<tr><td>' + lclientToken + '</td><td>' + count + '</td><td><a href="#" class="glimpse-trigger" data-client="' + lclientToken + '">Launch</a></td></tr>')
                } 

                //Trigger a history request if we need to 
                if (shouldTriggerHistoryRequest)
                    gr.getClientHistoryList(selectedClientName); 

                //In theory I wouldn't need to do this every time but wanting to make sure that all rows are kept in sync
                $('table tbody tr', static.subPanel).removeClass('even').removeClass('odd');
                $('table tbody tr:odd', static.subPanel).addClass('even');
                $('table tbody tr:even', static.subPanel).addClass('odd');
            }

            //Trigger new fetch
            if (static.tab.hasClass('glimpse-active'))
                setTimeout(function() { gr.getClientList(); }, 5000);
        },
        static : {
            key : 'Remote',
            tab : null,
            panel : null, 
            subPanel : null,
            mainPanel : null,
            historyLink : '/Glimpse/History',
            clientLink : '/Glimpse/Clients',
            result : {},
            _count : 0
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseRemote.init();

})(jQuery); }   