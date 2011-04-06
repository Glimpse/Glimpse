 
var glimpse; 
if (window.jQuery) { (function ($) {

    var glimpseCss = '.glimpse-open { position:fixed; right:0; bottom:0; height:27px; width:28px; border-left: 1px solid #ACA899; border-top: 1px solid #ACA899; background:#EEE; background:-moz-linear-gradient(top, #FFFFFF 0%, #EEEEEE 4%, #F3F5F7 8%, #E9E8DD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#FFFFFF), color-stop(4%,#EEEEEE), color-stop(8%,#F3F5F7), color-stop(100%,#E9E8DD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#FFFFFF\', endColorstr=\'#E9E8DD\',GradientType=0 ); }\n    .glimpse-icon { background:url(\x2FGlimpse\x2FglimpseSprite.png) 0px -16px; height:20px; width:20px; margin: 3px 4px 0; cursor:pointer; }\n    .glimpse-holder { display:none; height:0; position:fixed; bottom:0; left:0; width:100%; background-color:#fff; font-family:Lucida Grande,Tahoma,sans-serif; font-size:11px; }\n    .glimpse-bar { height:27px; border-top:1px solid #ACA899; background:#FFFFFF; background:-moz-linear-gradient(top, #FFFFFF 0%, #EEEEEE 4%, #F3F5F7 8%, #E9E8DD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#FFFFFF), color-stop(4%,#EEEEEE), color-stop(8%,#F3F5F7), color-stop(100%,#E9E8DD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#FFFFFF\', endColorstr=\'#E9E8DD\',GradientType=0 ); }\n    .glimpse-bar .glimpse-icon { margin-top:4px; float:left; cursor:default; }\n    .glimpse-buttons { float:right; height:17px; width:33px; padding:6px; }\n    .glimpse-title { margin:5px 0 0 15px; font-weight:bold; float:left; } \n    .glimpse-close, .glimpse-close:hover, .glimpse-terminate, .glimpse-terminate:hover { background-image:url(\x2FGlimpse\x2FglimpseSprite.png); background-repeat:no-repeat; height:14px; width:14px; margin-left:2px; display:inline-block; }\n    .glimpse-close { background-position:-1px -1px; }\n    .glimpse-close:hover { background-position:-17px -1px; }\n    .glimpse-terminate { background-position:-65px -1px; }\n    .glimpse-terminate:hover { background-position:-81px -1px; } \n    .glimpse-tabs { height:24px; font-weight:bold; border-bottom:1px solid #ACA899; border-top:1px solid #CDCABB; background:#B9B7AF; background:-moz-linear-gradient(top, #B9B7AF 0%, #DAD8C8 4%, #D7D4C5 10%, #E9E6D5 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#B9B7AF), color-stop(4%,#DAD8C8), color-stop(10%,#D7D4C5), color-stop(100%,#E9E6D5)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#B9B7AF\', endColorstr=\'#E9E6D5\',GradientType=0 );  }\n    .glimpse-tabs ul { margin:4px 0px 0 0; padding:0px; }\n    .glimpse-tabs li { display:inline; margin:0 2px 3px 2px; height:22px; padding:4px 9px 3px; color:#565656; cursor:pointer; border-radius: 0px 0px 3px 3px; -moz-border-radius: 0px 0px 3px 3px; -webkit-border-bottom-right-radius: 3px; -webkit-border-bottom-left-radius: 3px; }\n    .glimpse-tabs li.glimpse-active { padding:4px 8px 3px; color:#000; border-left:1px solid #A4A4A4; border-bottom:1px solid #A4A4A4; border-right:1px solid #A4A4A4; background:#F2F1EC; background:-moz-linear-gradient(top, #F2F1EC 0%, #F2F1EC 3%, #EFEEE9 7%, #E8E7E1 51%, #F7F6F1 92%, #F1F0EB 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#F2F1EC), color-stop(3%,#F2F1EC), color-stop(7%,#EFEEE9), color-stop(51%,#E8E7E1), color-stop(92%,#F7F6F1), color-stop(100%,#F1F0EB)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#F2F1EC\', endColorstr=\'#F1F0EB\',GradientType=0 ); }\n    .glimpse-tabs li.glimpse-hover { padding:4px 8px 3px; border-left:1px solid #BFBDB1; border-bottom:1px solid #BFBDB1; border-right:1px solid #BFBDB1; background:#BFBDB1; background:-moz-linear-gradient(top, #BFBDB1 0%, #DAD9CB 4%, #D8D5C9 8%, #E8E7E1 51%, #F0EEE4 92%, #EDEBE1 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#BFBDB1), color-stop(4%,#DAD9CB), color-stop(8%,#D8D5C9), color-stop(51%,#E8E7E1), color-stop(92%,#F0EEE4), color-stop(100%,#EDEBE1)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#BFBDB1\', endColorstr=\'#EDEBE1\',GradientType=0 ); }\n    .glimpse-tabs li.glimpse-disabled { color:#AAA; cursor:default; }\n    .glimpse-panel-holder {}\n    .glimpse-panel { display:none; overflow:auto; position:relative; } \n    .glimpse-panel-message { text-align:center; padding-top:40px; font-size:1.1em; color:#AAA; }\n    .glimpse-panel table { border-spacing:0; width:100%; }\n    .glimpse-panel table td, .glimpse-panel table th { padding:3px 4px; text-align:left; vertical-align:top; } \n    .glimpse-panel .glimpse-row-header-0 { height:19px; border-bottom:1px solid #9C9C9C; background:#C6C6C6; background:-moz-linear-gradient(top, #DEDEDE 0%, #BDBDBD 80%, #BBB 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#DEDEDE), color-stop(80%,#BDBDBD), color-stop(100%,#BBB)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#DEDEDE\', endColorstr=\'#BBB\',GradientType=0 ); }\n    .glimpse-panel .glimpse-row-header-0 th { border-left:1px solid #D9D9D9; border-right:1px solid #9C9C9C; }\n    .glimpse-panel table table { border:1px solid #D9D9D9; } \n    .glimpse-panel table table thead tr { height:17px; border-bottom:1px solid #9C9C9C; background:#C6C6C6; background:-moz-linear-gradient(top, #F1F1F1 0%, #DFDFDF 80%, #DDD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#F1F1F1), color-stop(80%,#DFDFDF), color-stop(100%,#DDD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#F1F1F1\', endColorstr=\'#DDD\',GradientType=0 ); }\n    .glimpse-panel table table thead tr th { border-left:1px solid #C6C6C6; border-right:1px solid #D9D9D9; padding:1px 4px 2px 4px; }\n    .glimpse-panel table table thead tr th:first-child { border-left:0px; }\n    .glimpse-panel table table thead tr th:last-child { border-right:0px; }\n    .glimpse-panel .even { background:#F4F4F4; }\n    .glimpse-panel .odd { background:#F9F9F9; }\n    .glimpse-panel table table tbody th { font-weight:normal; font-style:italic; }\n    .glimpse-panel table table thead th { font-weight:bold; font-style:normal; }\n    .glimpse-panel .glimpse-side-sub-panel { right:0; z-index:10; background-color:#F5F5F5; height:100%; width:25%; border-left:1px solid #ACA899; position:absolute; }\n    .glimpse-panel .glimpse-side-main-panel { position:relative; height:100%; width:75%; float:left; } \n    .glimpse-panel-holder .glimpse-active { display:block; }\n    .glimpse-resizer { height:4px; cursor:n-resize; width:100%; position:absolute; top:-1px; }\n    .glimpse-preview-object { color:#006400; } \n    .glimpse-preview-string { color:#006400 !important; font-weight:normal !important; } \n    .glimpse-preview-string span { padding-left:1px; }\n    .glimpse-preview-object span { font-weight:bold; color:#444; } \n    .glimpse-preview-object span.start { margin-right:5px; } \n    .glimpse-preview-object span.end { margin-left:5px; }\n    .glimpse-preview-object span.rspace { margin-right:4px; }\n    .glimpse-preview-object span.mspace { margin:0 4px; }\n    .glimpse-preview-object span.small { font-size:0.95em; } \n    .glimpse-expand { height:11px; width:11px; display:inline-block; float:left; margin:1px 0 0 -13px; cursor:pointer; background-image:url(\x2FGlimpse\x2FglimpseSprite.png); background-repeat:no-repeat; background-position:-96px 0; }\n    .glimpse-collapse { background-position:-96px -11px; }\n    .glimpse-preview-show { display:none; font-weight:normal !important; }\n    .glimpse-panel .quiet { color:#AAA; }\n    .glimpse-panel .suppress { color:#AAA; text-decoration:line-through; }\n    .glimpse-panel .selected { background-color:#FFFF99; color:#409B3B; }\n    .glimpse-panel .info .icon, .glimpse-panel .warn .icon, .glimpse-panel .loading .icon, .glimpse-panel .error .icon, .glimpse-panel .fail .icon { width:14px; height:14px; background-image:url(\x2FGlimpse\x2FglimpseSprite.png); background-repeat:no-repeat; display:inline-block; margin-right: 5px; } \n    .glimpse-panel .info .icon { background-position: -22px -22px; }\n    .glimpse-panel .warn .icon { background-position:-36px -22px; }\n    .glimpse-panel .loading .icon { background-position:-78px -22px; }\n    .glimpse-panel .error .icon { background-position:-50px -22px; } \n    .glimpse-panel .fail .icon { background-position:-64px -22px; }\n    .glimpse-panel .info { color:#067CE5; }\n    .glimpse-panel .warn { color:#FE850C; } \n    .glimpse-panel .error { color:#B40000; }\n    .glimpse-panel .fail { color:#D00; font-weight:bold; }\n    .glimpse-panelitem-XHReqests .loading .icon { float:right; }\n    .glimpse-panelitem-Remote .glimpse-side-sub-panel .loading, \n    .glimpse-panelitem-Remote .glimpse-side-main-panel .loading { position:absolute; bottom:5px; right:5px; color:#777; }';
    $('body').append($('<style type="text/css" class="glimpse-styles" />').append(glimpseCss));

    $.glimpse = {}; 
    $.glimpseProcessor = {};
    $.glimpseContent = {};
    $.glimpseResize = {}; 

	$.extend($.fn, {
        resizer : function() {
		    return this.each(function() { 
                var gr = $.glimpseResize;
		        gr.options.anchor = $(this).bind("mousedown", {el: $(this).parent()}, gr.startDrag); 
		    });
	    },
        sortElements : (function() { 
            var sort = [].sort; 
            return function(comparator, getSortable) { 
                getSortable = getSortable || function()  { return this; };
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
        htmlEncode : function(value) { 
            return !(value == undefined || value == null) ? $('<div/>').text(value).html() : ''; 
        },
        htmlDecode : function(value) { 
            return !(value == undefined || value == null) ? $('<div/>').html(value).text() : ''; 
        },
        lengthJson : function(data) {
            var count = 0;
            if ($.isPlainObject(data))
                $.each(data, function(k, v) { count++; });
            return count;
        },
        formatTime : function(d) {
            if (typeof d === 'number')
                d = new Date(d);
            var padding = function(t) { return t < 10 ? '0' + t : t; }
            return d.getHours() + ':' + padding(d.getMinutes()) + ':' + padding(d.getSeconds()) + ' ' +d.getMilliseconds();
        }
    });
     
    $.extend($.glimpseResize, {
        options : {
            anchor : null, 
            staticOffset : null, 
            lastMousePos : 0,
            min : 50, 
            endDragCallback : function(height) {} },
        startDrag : function(e) { 
            var gr = $.glimpseResize, o = gr.options; 
		    o.anchor = $(e.data.el);
	        o.lastMousePos = gr.mousePosition(e).y; 
	        o.staticOffset = o.anchor.height() + o.lastMousePos;
	        o.anchor.css('opacity', 0.50);
	        $(document).mousemove(gr.performDrag).mouseup(gr.endDrag);
	        return false;
        },
	    performDrag : function(e) {
            var gr = $.glimpseResize, o = gr.options; 
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
        endDrag : function(e) {
            var gr = $.glimpseResize, o = gr.options; 
	        $(document).unbind('mousemove', gr.performDrag).unbind('mouseup', gr.endDrag);
	        o.anchor.css('opacity', 1); 
	        o.anchor = null;
	        o.staticOffset = null;
	        o.lastMousePos = 0;
            o.endDragCallback();
        },
        mousePosition : function(e) {
            var d = document.documentElement;
	        return { x: e.clientX + d.scrollLeft, y: e.clientY + d.scrollTop };
        }
    });
    
    $.extend($.glimpseProcessor, { 
        layout : function(g, options, url) {
            var that = this, tabStrip = options.tabStrip(), panelHolder = options.panelHolder();
             
            var start = new Date().getTime();

            //Build Dynamic HTML
            for (var key in options.data) {
                if ($('.glimpse-tabitem-' + key, tabStrip).length == 0) {
                    that.addTab(tabStrip, options.data[key], key);
                    that.addTabBody(panelHolder, that.build(options.data[key], 0), key); 
                }
            }

            $('li', tabStrip).sortElements(); 
            $('.glimpse-panel', panelHolder).sortElements(); 

            console.log((new Date().getTime() - start) + ' ms render time');
             
            //Set Inital State - TODO: don't like how this works... need to review
            $('.info td:first-child, .warn td:first-child, tr.error td:first-child, .fail td:first-child, .loading td:first-child', $('.glimpse-panel')).not(':has(.icon)').prepend('<div class="icon"></div>');

            $('.glimpse-active', tabStrip).removeClass('glimpse-active').removeClass('glimpse-hover');
            $('li:first', tabStrip).addClass('glimpse-active');

            $('.glimpse-active', panelHolder).removeClass('glimpse-active');
            $('.glimpse-panel:first', panelHolder).addClass('glimpse-active');
             
            $('.glimpse-title').text(url);
        }, 
        addTab : function(container, data, key) {
            var disabled = (data === undefined || data === null) ? ' glimpse-disabled' : '';
            container.append('<li class="glimpse-tabitem-' + key + disabled + '" data-sort="' + key + '">' + key + '</li>');
        },
        addTabBody : function(container, content, key) {
            container.append('<div class="glimpse-panel glimpse-panelitem-' + key + '" data-sort="' + key + '">' + content + '</div>');
        },
        clearLayout : function(g, options) {
            var that = this, tabStrip = options.tabStrip(), panelHolder = options.panelHolder();
            
            that.removeTabs(tabStrip);
            that.removeTabBodies(panelHolder);
        },
        removeTabs : function(container) {
            $('li:not(.glimpse-permanent)', container).remove();
        },
        removeTabBodies : function(container) {
            $('.glimpse-panel:not(.glimpse-permanent)', container).remove();
        }, 
        build : function(data, level) { 
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
        buildKeyValueTable : function(data, level, forceFull) { 
            var that = this, limit = 2;
            if (((level > 0 && $.lengthJson(data) > (limit + 1)) || level > 1) && !forceFull) 
                return that.buildKeyValuePreview(data, limit); 
                
            var i = 1, html = '<table><thead><tr class="glimpse-row-header-' + level + '"><th>Key</th><th>Value</th></tr></thead>';   
            for (var key in data)
                html += '<tr class="' + (i++ % 2 ? 'odd' : 'even') + '"><th width="30%">' + $.glimpseContent.formatString(key) + '</th><td width="70%"> '+ that.build(data[key], level + 1) + '</td></tr>';
            html += '</table>';
            return html; 
        },
        buildCustomTable : function(data, level, forceFull) {
            var that = this, limit = 2; 
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
        buildString : function(data, level) {  
            return this.buildStringPreview(data, level);
        },
        buildKeyValuePreview : function(data, level) {    
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
        buildCustomPreview : function(data, level) {  
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
        buildStringPreview : function(data, level) { 
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
                content = '<span class="glimpse-preview-string" title="' + $.glimpseContent.trimFormatString(data, charMax * 2, charMax * 2.1, false, true)  + '">' + content + '</span>';
                if (level < 100)
                    content = '<span class="glimpse-expand"></span>' + content + '<span class="glimpse-preview-show">' + $.glimpseContent.formatString(data) + '</span>'; 
            }
            return content;
        },
        newItemSpacer : function(currentRow, rowLimit, dataLength) {
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
        formatStringTypes : {
            italics : {
                match : function(d) { return d.match(/^\_[\w\D]+\_$/) != null; },
                replace : function(d) { return '<u>' + $.htmlEncode($.glimpseContent.scrub(d)) + '</u>'; },
                trimmable : true },
            underline : {
                match : function(d) { return d.match(/^\\[\w\D]+\\$/) != null; },
                replace : function(d) { return '<em>' + $.htmlEncode($.glimpseContent.scrub(d)) + '</em>'; },
                trimmable : true },
            strong : {
                match : function(d) { return d.match(/^\*[\w\D]+\*$/) != null; },
                replace : function(d) { return '<strong>' + $.htmlEncode($.glimpseContent.scrub(d)) + '</strong>'; },
                trimmable : true },
            raw : {
                match : function(d) { return d.match(/^\![\w\D]+\!$/) != null; },
                replace : function(d) { return $.glimpseContent.scrub(d); },
                trimmable : false }
        },
        scrub : function(d) {
            return d.substr(1, d.length - 2);
        },
        formatString : function(data) {
            var that = this;
            return that.trimFormatString(data);
        },
        trimFormatString : function(data, charMax, charOuterMax, wrapEllipsis, skipEncoding) {
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

    var _persistHeight = function(g, options) {
        options.height = $('.glimpse-holder').height();
        //$.cookie('GlimpseSettings', true);
    }
   
    $.extend($.glimpse, {
        addProtocolListener : function(callback, onInitOnly) {
            $.glimpse._context.protocolListeners.push({ 'callback' : callback, 'onInitOnly' : onInitOnly });
        },
        _executeProtocolListeners : function(g, options, isInit) {
            var i = 0, listeners = g._context.protocolListeners, data = options.data; 
            for (; i < listeners.length; i++) {
                var listener = listeners[i];
                if (isInit || !listener.onInitOnly)
                    listener.callback(data)
            }
        },
        addLayoutListener : function(callback, onInitOnly) {
            $.glimpse._context.layoutListeners.push({ 'callback' : callback, 'onInitOnly' : onInitOnly });
        },
        _executeLayoutListeners : function(g, options, isInit) {
            var i = 0, listeners = g._context.layoutListeners, tabStrip = options.tabStrip(), panelHolder = options.panelHolder();
            for (; i < listeners.length; i++) {
                var listener = listeners[i];
                if (isInit || !listener.onInitOnly)
                    listener.callback(tabStrip, panelHolder)
            }
        },
        wireEvents : function(g, options) { 
            //Open/Close Holder
            $('.glimpse-open').live('click', g.open); 
            $('.glimpse-close').live('click', g.close);    
            $('.glimpse-terminate').live('click', g.terminate);
             
            //Tab Switching 
            $('.glimpse-tabs li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function (e) { 
                var item = $(this);
                if (e.type == 'mouseover') { item.addClass('glimpse-hover'); } else { item.removeClass('glimpse-hover'); }
            }); 
            $('.glimpse-tabs li:not(.glimpse-active, .glimpse-disabled)').live('click', function () {
                var item = $(this);

                //Setup Tabs
                $('.glimpse-tabs .glimpse-active').removeClass('glimpse-active').removeClass('glimpse-hover');
                item.addClass('glimpse-active');
                
                //Setup Panels 
                $('.glimpse-panel-holder .glimpse-active').removeClass('glimpse-active');
                $('.glimpse-panel-holder .glimpse-panel:eq(' + $('.glimpse-tabs ul li').index(item) + ')').addClass('glimpse-active');
            });

            //Resize
            $('.glimpse-resizer').resizer(options.height); 
             
            //Exspand/Collapse
            $('.glimpse-expand').live('click', function() {
                $(this).toggleClass('glimpse-collapse').next().toggle().next().toggle();  
            });
        },
        wireCallback : function(g, options) {
            //Remember height 
            $.glimpseResize.options.endDragCallback = function() {
                _persistHeight(g, options);  
                $('.glimpse-spacer').height(options.height);
                $('.glimpse-holder .glimpse-panel').height(options.height - 54); 
            }
        }, 
        open : function() {
            var g = $.glimpse, options = g.defaults;

            $('.glimpse-open').hide();
            $('.glimpse-holder').show().animate({ 'height': options.height }, 'fast'); 
            g.setHeight(g, options);
        },
        close : function() {
            $('.glimpse-holder').animate({ 'height': '0' }, 'fast', function () { 
                    $(this).hide(); 
                    $('.glimpse-open').show(); 
                });
            $('.glimpse-spacer').height('0');
        },
        terminate : function() {
            $('.glimpse-open, .glimpse-spacer').remove(); 
            $('.glimpse-holder').animate({ 'height': '0' }, 'fast', function () { 
                $(this).remove(); 
            });

            var t = new Date();
            t.setDate(t.getDate() + -1)
            var expires = t.toUTCString() 
            document.cookie = 'glimpseState=; path=/; expires=' + expires + ';';
            document.cookie = 'glimpseClientName=; path=/; expires=' + expires + ';';
            document.cookie = 'glimpseOptions=; path=/; expires=' + expires + ';';
        },
        setHeight : function(g, options) {
            $('.glimpse-spacer').height(options.height); 
            $('.glimpse-holder .glimpse-panel').height(options.height - 54); 
        },
        refresh : function(data, url) { 
            if (!data) return;

            var g = $.glimpse, options = g.defaults;
            options.data = data;
            
            g._executeProtocolListeners(g, options, false);

            $.glimpseProcessor.clearLayout(g, options);
            $.glimpseProcessor.layout(g, options, url);
            g.setHeight(g, options);

            g._executeLayoutListeners(g, options, false);
        },
        init : function(data) {
            if (!data) return;
            
            var g = $.glimpse, options = g.defaults;
            options.data = data;

            g._executeProtocolListeners(g, options, true);

            $('body').append(options.html.plugin); 

            g.wireEvents(g, options);
            g.wireCallback(g, options);

            $.glimpseProcessor.layout(g, options, window.location.pathname);

            g._executeLayoutListeners(g, options, true);

            //$('body').append('<style>' + options.css + '</style>');
            $('body').append('<div class="glimpse-spacer"></div>');
        },
        _context : {
            protocolListeners : [],
            layoutListeners : []
        },
        defaults : { 
            data : null,
            html : { plugin : '<div class="glimpse-open"><div class="glimpse-icon"></div></div><div class="glimpse-holder"><div class="glimpse-resizer"></div><div class="glimpse-bar"><div class="glimpse-icon"></div><div class="glimpse-title"></div><div class="glimpse-buttons"><a href="#" title="Shutdown/Terminate" class="glimpse-terminate"></a><a href="#" title="Close/Minimize" class="glimpse-close"></a></div></div><div class="glimpse-content"><div class="glimpse-tabs"><ul></ul></div><div class="glimpse-panel-holder"></div></div></div>' },
            css : '',
            height : 200,
            tabStrip : function() { return $('.glimpse-tabs ul'); },
            panelHolder : function() { return $('.glimpse-panel-holder'); }
        }
    });
      

    //Run glimpse 
    $(document).ready(function() {
	    $.glimpse.init(glimpse); 
    });

})(jQuery); }  






if (window.jQuery) { (function ($) {
 
    $.glimpseAjax = {};
    $.extend($.glimpseAjax, { 
        init : function() { 
            var ga = this;

            //Wire up plugin
            $.glimpse.addProtocolListener(ga.adjustProtocol, true);
            $.glimpse.addLayoutListener(ga.adjustLayout, true);
        },
        adjustProtocol : function(data) {
            var ga = $.glimpseAjax, options = ga.defaults; 
            data[options.key] = ''
        },
        adjustLayout : function(tabStrip, panelHolder) {
            var ga = $.glimpseAjax, options = ga.defaults;
            
            //Setup layout
            options.tab = $('.glimpse-tabitem-' + options.key, tabStrip);
            options.panel = $('.glimpse-panelitem-' + options.key, panelHolder);
            options.tab.addClass('glimpse-permanent').text(options.key); 
            options.panel.addClass('glimpse-permanent').html('<div class="glimpse-panel-message">No ajax calls have yet been detected</div>');

            //Wireevents 
            $('a', options.panel).live('click', function() {
                $('.selected', options.panel).removeClass('selected');
                $(this).parent().parent().addClass('selected');
            }); 
        },
        callStarted : function(ajaxSpy) {
            var g = $.glimpse, ga = $.glimpseAjax, options = ga.defaults, panelHolder = g.defaults.panelHolder(), panelItem = $('.glimpse-panelitem-' + options.key, panelHolder);

            if (ajaxSpy.url && ajaxSpy.url.length > 9 && ajaxSpy.url.substr(0, 9) == '/Glimpse/')
                return;

            //First time round we need to set everything up
            if ($('.glimpse-panel-message', panelItem).length > 0) {
                panelItem.html($.glimpseProcessor.build([ 
                    ['Request URL', 'Status', 'Date/Time', 'Druration', 'Is Async', 'Inspect'],
                    [window.location.pathname, '200', $.formatTime(new Date()), 'N/A', 'N/A', '!<a href="#">Reset</a>!']
                ], 0));

                $('a', options.panel).click(function(e) { 
                    $.glimpse.refresh(glimpse, window.location.pathname);
                    return false;  
                });
                $('tr:last', options.panel).addClass('selected');
            }
            
            //Add new row
            $('table', panelItem).append('<tr class="loading"><th><div class="icon"></div>' + ajaxSpy.url + '</th><td class="glimpse-ajax-status">Loading...</td><td>' + $.formatTime(ajaxSpy.startTime) + '</td><td class="glimpse-ajax-duration">--</td><td>' + ajaxSpy.async + '</td><td class="glimpse-ajax-inspect">N/A</td></tr>');
            
            //In theory I wouldn't need to do this every time but wanting to make sure that all rows are kept in sync
            $('table tbody tr:odd', panelItem).addClass('even');
            $('table tbody tr:even', panelItem).addClass('odd');

            ajaxSpy.logRow = $('tr:last', panelItem);
        },
        callFinished : function(ajaxSpy) {
            var row = ajaxSpy.logRow, glimpseResponse = ajaxSpy.responseHeaders['X-Glimpse-Debugger'];
            
            if (ajaxSpy.url && ajaxSpy.url.length > 9 && ajaxSpy.url.substr(0, 9) == '/Glimpse/')
                return;

            //Adjust layout
            row.removeClass('loading').addClass(!ajaxSpy.success ? 'error' : glimpseResponse ? 'ajax-loaded' : 'suppress');
            $('.glimpse-ajax-status', row).text(ajaxSpy.status);
            $('.glimpse-ajax-duration', row).text(ajaxSpy.duration + 'ms'); 
            if (glimpseResponse) {
                $('.glimpse-ajax-inspect', row).html('<a href="#">Launch</a>').children('a').click(function() {
                    $.glimpse.refresh(eval('(' + glimpseResponse + ')'), ajaxSpy.url); 
                    return false;
                }); 
            }
        },  
        defaults : {
            key : 'XHReqests',
            tab : null,
            panel : null
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseAjax.init();
     
})(jQuery); }  






if (window.jQuery) { (function ($) {
 
    $.glimpseRemote = {};
    $.extend($.glimpseRemote, { 
        init : function() { 
            var gr = this;

            //Wire up plugin
            $.glimpse.addProtocolListener(gr.adjustProtocol, true);
            $.glimpse.addLayoutListener(gr.adjustLayout, true);
        },
        adjustProtocol : function(data) {
            var gr = $.glimpseRemote, options = gr.defaults; 
            data[options.key] = ''
        },
        adjustLayout : function(tabStrip, panelHolder) {
            var gr = $.glimpseRemote, options = gr.defaults;
            
            //Setup layout
            options.tab = $('.glimpse-tabitem-' + options.key, tabStrip);
            options.panel = $('.glimpse-panelitem-' + options.key, panelHolder);
             
            //Make sure we stick round 
            options.tab.addClass('glimpse-permanent').text(options.key); 
            options.panel.addClass('glimpse-permanent'); 
            options.panel.prepend('<div class="glimpse-side-sub-panel"><div class="loading"><div class="icon"></div><span>Refreshing...</span></div><div class="glimpse-content"></div></div><div class="glimpse-side-main-panel"><div class="glimpse-initial glimpse-panel-message">No remote calls have yet been detected</div><div class="loading" style="display:none"><div class="icon"></div><span>Refreshing...</span></div><div class="glimpse-content"></div></div>');

            options.subPanel = $('.glimpse-side-sub-panel', options.panel);
            options.mainPanel = $('.glimpse-side-main-panel', options.panel);

            //Wireevents 
            $('a', options.panel).live('click', function() {
                $('.selected', $(this).parents('table:first')).removeClass('selected');
                $(this).parents('tr:first').addClass('selected');
            }); 
            $('a.glimpse-trigger', options.subPanel).live('click', function() {
                gr.getClientHistoryList($(this).data('client')); 
                return false;
            });
            $('a.glimpse-orignal', options.subPanel).live('click', function() {
                gr.reset();
                return false;
            });
            $('a', options.mainPanel).live('click', function() { 
                gr.activate($(this).data('client'), $(this).data('request'));
                return false;
            });
            options.tab.click(function() {
                gr.getClientList();
            });
        },
        reset : function() {
            var gr = $.glimpseRemote, options = gr.defaults; 

            $.glimpse.refresh(glimpse, window.location.pathname);  
            $('.glimpse-initial', options.mainPanel).show(); 
            $('.loading', options.mainPanel).hide(); 
            $('.glimpse-content', options.mainPanel).empty(); 
        },
        activate : function(client, request) {
            var gr = $.glimpseRemote, options = gr.defaults; 

            var request = options.result.Data[client][request];
            if (request.Data) 
                $.glimpse.refresh(eval('(' + request.Data + ')'), request.Url); 
        },
        getClientHistoryList : function(clientId) {
            var gr = $.glimpseRemote, options = gr.defaults, loading = $('.loading', options.mainPanel); 
             
            $.ajax({
                url: '/Glimpse/History',
                type: 'GET', 
                data : { 'ClientName' : clientId },
                contentType: 'application/json',  
                beforeSend: function () { 
                    $('span', loading).text('Refreshing...').parent().fadeIn(); 
                },
                success: function(result) {  
                    $('span', loading).text('Loaded...').parent().delay(1500).fadeOut(); 
                    gr.processClientHistoryList(result);
                }
            });  
        },
        getClientList : function() {
            var gr = $.glimpseRemote, options = gr.defaults, loading = $('.loading', options.subPanel); 

            $.ajax({
                url: '/Glimpse/Clients',
                type: 'GET', 
                contentType: 'application/json',  
                beforeSend: function () { 
                    $('span', loading).text('Refreshing...').parent().fadeIn(); 
                },
                success: function(result) {  
                    $('span', loading).text('Loaded...').parent().delay(1500).fadeOut();  
                    gr.processClientList(result);
                }
            });
        },
        processClientHistoryList : function(result) {
            var gr = $.glimpseRemote, options = gr.defaults; 

            if (options.result && result) {
            
                $.extend(true, options.result, result);
                 
                //Pull out the name of the client
                var ldata = options.result.Data || {}, rdata = result.Data || {}, rclientToken = ''; 
                for (var key in rdata) {
                    rclientToken = key;
                    break;
                }
                
                //As long as the client  
                if ($(".selected a[data-client='" + rclientToken + "']", options.subPanel).length > 0) {
                    var lclient = ldata[rclientToken], data = [ [ 'Client Name', 'Request Url', 'Browser', 'Date/Time', 'Is Ajax', 'Launch' ] ]; 
                    
                    for (var lclientRequestToken in lclient) {
                        var lclientRequest = lclient[lclientRequestToken];
                        if (lclientRequest.Data) 
                            data.push([ rclientToken, lclientRequest.Url, lclientRequest.Browser, lclientRequest.RequestTime, lclientRequest.IsAjax, '!<a href="#" data-request="' + lclientRequestToken + '" data-client="' + rclientToken + '">Launch</a>!' ]);
                    }

                    $('.glimpse-initial', options.mainPanel).hide();
                    $('.glimpse-content', options.mainPanel).html($.glimpseProcessor.build(data, 0)); 
                }
            } 
        },
        processClientList : function(result) {
            var gr = $.glimpseRemote, options = gr.defaults; 
            
            //Create the table we need first time round 
            if ($('table', options.subPanel).length == 0) 
                $('.glimpse-content', options.subPanel).html($.glimpseProcessor.build([ [ 'Client', 'Count', 'Launch'], [ '\\--this--\\', 1, '!<a href="#" class="glimpse-orignal">Reset</a>!', 'selected' ] ], 0));
 
            if (options.result && result) {
                var shouldTriggerHistoryRequest = false, selectedClientName = $(".selected a", options.subPanel).data('client');

                //Adjusts the returned data
                $.extend(true, options.result, result);

                //Need to do some work on this data 
                var ldata = options.result.Data || {}, rdata = result.Data || {}; 
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
                    var clientRow = $("tr:has(a[data-client='" + lclientToken + "'])", options.subPanel); 
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
                        $('table', options.subPanel).append('<tr><td>' + lclientToken + '</td><td>' + count + '</td><td><a href="#" class="glimpse-trigger" data-client="' + lclientToken + '">Launch</a></td></tr>')
                } 

                //Trigger a history request if we need to 
                if (shouldTriggerHistoryRequest)
                    gr.getClientHistoryList(selectedClientName); 

                //In theory I wouldn't need to do this every time but wanting to make sure that all rows are kept in sync
                $('table tbody tr', options.subPanel).removeClass('even').removeClass('odd');
                $('table tbody tr:odd', options.subPanel).addClass('even');
                $('table tbody tr:even', options.subPanel).addClass('odd');
            }

            //Trigger new fetch
            if (options.tab.hasClass('glimpse-active'))
                setTimeout(function() { gr.getClientList(); }, 5000);
        },
        defaults : {
            key : 'Remote',
            tab : null,
            panel : null, 
            subPanel : null,
            mainPanel : null,
            result : {},
            _count : 0
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseRemote.init();


})(jQuery); }   