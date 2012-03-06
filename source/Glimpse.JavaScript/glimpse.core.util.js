util = function () {
    var //Support
        resizer = {
            defaults : {
                staticOffset : null, 
                lastMousePos : 0,
                min : function() { return 50; },  
                max : undefined,
                preDragCallback : function(postion) { }, 
                endDragCallback : function(postion) { },
                isUpDown : false, 
                valueStyle : 'left', 
                offset : 1,
                getValue : function(settings) { return parseInt(settings.resizeScope.css(settings.valueStyle)); },
                setValue : function(settings, value) { return settings.resizeScope.css(settings.valueStyle, value + 'px'); }
            }, 
            init : function (scope, instSettings) { 
                var settings = $.extend(true, { opacityScope : scope, anchorScope : scope, resizeScope : scope }, resizer.defaults, instSettings);   
                 
                settings.mousePosition = instSettings.isUpDown ? function(e) { return e.clientY + document.documentElement.scrollTop; } : function(e) { return e.clientX + document.documentElement.scrollLeft; };

                scope.bind("mousedown", { settings : settings }, resizer.startDrag); 
            },
            startDrag : function (e) {
                var settings = e.data.settings;  

                resizer.min = settings.min();
                resizer.max = settings.max != undefined ? settings.max() : undefined;
             
                settings.preDragCallback(); 
                settings.lastMousePos = settings.mousePosition(e);
                settings.staticOffset = settings.getValue(settings) + (settings.lastMousePos * -1 * settings.offset);   
                settings.opacityScope.css('opacity', 0.50);

                $(document).bind('mousemove', { settings : settings }, resizer.performDrag).bind('mouseup', { settings : settings }, resizer.endDrag);

                return false;
            },
            performDrag : function (e) {
                var settings = e.data.settings, 
                    mousePos = settings.mousePosition(e), 
                    offsetMousePos = resizer.calculateOffset(mousePos, settings); 
                 
                settings.setValue(settings, offsetMousePos);  
                settings.lastMousePos = mousePos;

                if (offsetMousePos < resizer.min || (resizer.max && offsetMousePos > resizer.max)) { resizer.endDrag(e); }

                return false;
            },
            endDrag : function (e) {
                var settings = e.data.settings;

                $(document).unbind('mousemove', resizer.performDrag).unbind('mouseup', resizer.endDrag);

                settings.opacityScope.css('opacity', 1);
                settings.endDragCallback(settings.getValue(settings));
            },
            calculateOffset : function (mousePos, settings) {
                var offsetMousePos = settings.staticOffset + (mousePos * 1 * settings.offset); 

                if (settings.lastMousePos >= mousePos) { offsetMousePos += 4; } 
                if (resizer.min != undefined) { offsetMousePos = Math.max(resizer.min, offsetMousePos); }
                if (resizer.max != undefined) { offsetMousePos = Math.min(resizer.max, offsetMousePos); } 

                return offsetMousePos;
            }
        },
        connectionNotice = function (scope) {
            var that = (this === window) ? {} : this;
            that.scope = scope;
            that.text = scope.find('span');
            return that;
        }; 

    connectionNotice.prototype = {
        connected : false, 
        prePoll : function () {
            var that = this;
            if (!that.connected) { 
                that.text.text('Connecting...'); 
                that.scope.removeClass('gconnect').addClass('gdisconnect');
            }
        },
        complete : function (textStatus) {
            var that = this;
            if (textStatus != "Success") {
                that.connected = false;
                that.text.text('Disconnected...');
                that.scope.removeClass('gconnect').addClass('gdisconnect');
            }
            else {
                that.connected = true;
                that.text.text('Connected...');
                that.scope.removeClass('gdisconnect').addClass('gconnect');
            }
        }
    };
    
    return { 
        connectionNotice : connectionNotice,
        cookie : function (key, value, expiresIn) {
            key = encodeURIComponent(key);
            //Set Cookie
            if (arguments.length > 1) {
                var t = new Date();
                t.setDate(t.getDate() + expiresIn || 1000);

                value = $.isPlainObject(value) ? JSON.stringify(value) : String(value);
                return (document.cookie = key + '=' + encodeURIComponent(value) + '; expires=' + t.toUTCString() + '; path=/');
            }

            //Get cookie 
            var result = new RegExp("(?:^|; )" + key + "=([^;]*)").exec(document.cookie);
            if (result) {
                result = decodeURIComponent(result[1]);
                if (result.substr(0, 1) == '{') {
                    result = JSON.parse(result);
                }
                return result;
            }
            return null;
        },
        htmlEncode: function (value) {
            return !(value == undefined || value == null) ? $('<div/>').text(value).html() : '';
        },
        htmlDecode: function (value) {
            return !(value == undefined || value == null) ? $('<div/>').html(value).text() : '';
        },
        preserveWhitespace: function (value) {
            return value.replace(/\r\n/g, '<br />').replace(/\n/g, '<br />').replace(/\t/g, '&nbsp; &nbsp; ').replace(/  /g, '&nbsp; ');
        },
        lengthJson: function (data) {
            var count = 0;
            if ($.isPlainObject(data))
                $.each(data, function (k, v) { count++; });
            return count;
        }, 
        getTokens: function(formatString, data) { 
            var count = 0, working = '', result = [];
            for (var i = 0; i < formatString.length; i++) {
                var x = formatString[i];
                
                if (count <= 2) { 
                    if (x == '{')
                        count++;
                    else if (x == '}' && count > 0)
                        count--;
                    else if (count == 2) {
                        if ($.isNaN(x)) {
                            count = 0;
                            working = '';
                        }
                        else 
                            working += '' + x;
                    }
                    else {
                        count = 0;
                        working = '';
                    }

                    if (count == 0 && working != '') {
                        result.push(working);
                        working = '';
                    }
                } 
            }
            return result;
        }, 
        sortElements : function (container, containerItem) {
            containerItem.sort(function(a, b) {
                var compA = $(a).text().toUpperCase();
                var compB = $(b).text().toUpperCase();
                return (compA < compB) ? -1 : (compA > compB) ? 1 : 0;
            });
            $.each(containerItem, function(idx, itm) { container.append(itm); });
        },
        resizer : function (scope, settings) {
            resizer.init(scope, settings);
        }, 
        getDomain : function(url) {
            if (url.indexOf('://') > -1)
                url = url.split('://')[1];
            return url.split('/')[0];
        },
        timeConvert : function(value) {
            if (value < 1000)
                return value + 'ms';
            return Math.round(value / 10) / 100 + 's';
        }
    }; 
} ()