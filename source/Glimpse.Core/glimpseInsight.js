// glimpse.insight.js
var glimpse = glimpse || {};   
    glimpse.extensions = glimpse.extensions || []; 
(function(glimpse) { 
    var timerStart = new Date().getTime(),
        init = function($, pubsub, settings, util) {
                var report = (function() {
                        var _token = '80914090f5830ed4b25accb9f1658cb3',
                            _ = (function() {
                                var userAgent = navigator.userAgent;

                                return {
                                    base64Encode: function(data) {
                                        var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                                        var o1, o2, o3, h1, h2, h3, h4, bits, i = 0, ac = 0, enc = "", tmp_arr = [];

                                        if (!data) {
                                            return data;
                                        }

                                        data = _.utf8Encode(data);

                                        do { // pack three octets into four hexets
                                            o1 = data.charCodeAt(i++);
                                            o2 = data.charCodeAt(i++);
                                            o3 = data.charCodeAt(i++);

                                            bits = o1 << 16 | o2 << 8 | o3;

                                            h1 = bits >> 18 & 0x3f;
                                            h2 = bits >> 12 & 0x3f;
                                            h3 = bits >> 6 & 0x3f;
                                            h4 = bits & 0x3f;

                                            // use hexets to index into b64, and append result to encoded string
                                            tmp_arr[ac++] = b64.charAt(h1) + b64.charAt(h2) + b64.charAt(h3) + b64.charAt(h4);
                                        } while (i < data.length);

                                        enc = tmp_arr.join('');

                                        switch (data.length % 3) {
                                        case 1:
                                            enc = enc.slice(0, -2) + '==';
                                            break;
                                        case 2:
                                            enc = enc.slice(0, -1) + '=';
                                            break;
                                        }

                                        return enc;
                                    },
                                    utf8Encode: function(string) {
                                        string = (string + '').replace(/\r\n/g, "\n").replace(/\r/g, "\n");

                                        var utftext = "",
                                            start,
                                            end;
                                        var stringl = 0,
                                            n;

                                        start = end = 0;
                                        stringl = string.length;

                                        for (n = 0; n < stringl; n++) {
                                            var c1 = string.charCodeAt(n);
                                            var enc = null;

                                            if (c1 < 128) {
                                                end++;
                                            } else if ((c1 > 127) && (c1 < 2048)) {
                                                enc = String.fromCharCode((c1 >> 6) | 192, (c1 & 63) | 128);
                                            } else {
                                                enc = String.fromCharCode((c1 >> 12) | 224, ((c1 >> 6) & 63) | 128, (c1 & 63) | 128);
                                            }
                                            if (enc !== null) {
                                                if (end > start) {
                                                    utftext += string.substring(start, end);
                                                }
                                                utftext += enc;
                                                start = end = n + 1;
                                            }
                                        }

                                        if (end > start) {
                                            utftext += string.substring(start, string.length);
                                        }

                                        return utftext;
                                    },
                                    includes: function(str, needle) {
                                        return str.indexOf(needle) !== -1;
                                    },
                                    uuid: (function() {
                                        // Time/ticks information
                                        // 1*new Date() is a cross browser version of Date.now()
                                        var T = function() {
                                            var d = 1 * new Date(), i = 0;

                                            // this while loop figures how many browser ticks go by
                                            // before 1*new Date() returns a new number, ie the amount
                                            // of ticks that go by per millisecond
                                            while (d == 1 * new Date()) {
                                                i++;
                                            }

                                            return d.toString(16) + i.toString(16);
                                        };

                                        // Math.Random entropy
                                        var R = function() {
                                            return Math.random().toString(16).replace('.', '');
                                        };

                                        // User agent entropy
                                        // This function takes the user agent string, and then xors
                                        // together each sequence of 8 bytes.  This produces a final
                                        // sequence of 8 bytes which it returns as hex.
                                        var UA = function(n) {
                                            var ua = userAgent, i, ch, buffer = [], ret = 0;

                                            function xor(result, byte_array) {
                                                var j, tmp = 0;
                                                for (j = 0; j < byte_array.length; j++) {
                                                    tmp |= (buffer[j] << j * 8);
                                                }
                                                return result ^ tmp;
                                            }

                                            for (i = 0; i < ua.length; i++) {
                                                ch = ua.charCodeAt(i);
                                                buffer.unshift(ch & 0xFF);
                                                if (buffer.length >= 4) {
                                                    ret = xor(ret, buffer);
                                                    buffer = [];
                                                }
                                            }

                                            if (buffer.length > 0) {
                                                ret = xor(ret, buffer);
                                            }

                                            return ret.toString(16);
                                        };

                                        return function() {
                                            var se = (screen.height * screen.width).toString(16);
                                            return (T() + "-" + R() + "-" + UA() + "-" + se + "-" + T());
                                        };
                                    })(),
                                    info: {
                                        browser: function() {
                                            var ua = userAgent,
                                                vend = navigator.vendor || ''; // vendor is undefined for at least IE9
                                            if (window.opera) {
                                                if (_.includes(ua, "Mini")) {
                                                    return "Opera Mini";
                                                }
                                                return "Opera";
                                            } else if (/(BlackBerry|PlayBook|BB10)/i.test(ua)) {
                                                return 'BlackBerry';
                                            } else if (_.includes(ua, "Chrome")) {
                                                return "Chrome";
                                            } else if (_.includes(vend, "Apple")) {
                                                if (_.includes(ua, "Mobile")) {
                                                    return "Mobile Safari";
                                                }
                                                return "Safari";
                                            } else if (_.includes(ua, "Android")) {
                                                return "Android Mobile";
                                            } else if (_.includes(ua, "Konqueror")) {
                                                return "Konqueror";
                                            } else if (_.includes(ua, "Firefox")) {
                                                return "Firefox";
                                            } else if (_.includes(ua, "MSIE")) {
                                                return "Internet Explorer";
                                            } else if (_.includes(ua, "Gecko")) {
                                                return "Mozilla";
                                            } else {
                                                return "";
                                            }
                                        },
                                        os: function() {
                                            var a = userAgent;
                                            if (/Windows/i.test(a)) {
                                                if (/Phone/.test(a)) {
                                                    return 'Windows Mobile';
                                                }
                                                return 'Windows';
                                            } else if (/(iPhone|iPad|iPod)/.test(a)) {
                                                return 'iOS';
                                            } else if (/Android/.test(a)) {
                                                return 'Android';
                                            } else if (/(BlackBerry|PlayBook|BB10)/i.test(a)) {
                                                return 'BlackBerry';
                                            } else if (/Mac/i.test(a)) {
                                                return 'Mac OS X';
                                            } else if (/Linux/.test(a)) {
                                                return 'Linux';
                                            } else {
                                                return '';
                                            }
                                        },
                                        device: function() {
                                            var a = userAgent;
                                            if (/iPhone/.test(a)) {
                                                return 'iPhone';
                                            } else if (/iPad/.test(a)) {
                                                return 'iPad';
                                            } else if (/iPod/.test(a)) {
                                                return 'iPod Touch';
                                            } else if (/(BlackBerry|PlayBook|BB10)/i.test(a)) {
                                                return 'BlackBerry';
                                            } else if (/Windows Phone/i.test(a)) {
                                                return 'Windows Phone';
                                            } else if (/Android/.test(a)) {
                                                return 'Android';
                                            } else {
                                                return '';
                                            }
                                        },
                                        properties: function() {
                                            return {
                                                '$os': _.info.os(),
                                                '$browser': _.info.browser(),
                                                '$device': _.info.device()
                                            };
                                        }
                                    },
                                    chunk: function(array, chunkSize) {
                                        return [].concat.apply([],
                                            array.map(function(elem, i) {
                                                return i % chunkSize ? [] : [array.slice(i, i + chunkSize)];
                                            })
                                        );
                                    }
                                };
                            })(),
                            _data = {
                                fixed: { token: _token },
                                global: _.info.properties()
                            };
                                    
                        return {  
                            settings: (function() {
                                var data = $.extend({}, util.localStorage('glimpseEventOptions'));

                                return function(key, value) {
                                    if (arguments.length == 1)
                                        return data[key];

                                    data[key] = value;
                                    util.localStorage('glimpseEventOptions', data);
                                    return value;
                                };
                            })(),
                            init: function() { 
                                var id = report.settings('uuid'),
                                    initRun = false;
                                if (!id) {
                                    id = report.settings('uuid', _.uuid()); 
                                    initRun = true;
                                }

                                _data.global['distinct_id'] = id;
                                _data.global['page_id'] = _.uuid();
                         
                                $(function() {
                                    setTimeout(report.batch, 1000);
                                });

                                return initRun;
                            },
                            register: function(data) { 
                                $.extend(_data.global, data);
                            },
                            track: function(event_name, properties) {
                                var data = {
                                        'event': event_name, 
                                        'properties': $.extend(
                                                { time: parseInt(new Date().getTime().toString() / 1000) }, 
                                                _data.fixed,
                                                _data.global,
                                                properties
                                            )
                                    },
                                    events = util.localStorage('glimpseEvents') || [];
                         
                                pubsub.publish('trigger.insight.event', data);

                                //Push data onto the stack
                                events.push(data);

                                //Save off the state
                                util.localStorage('glimpseEvents', events);
                            },
                            batch: function() {
                                //Grab events and rip through them
                                var events = util.localStorage('glimpseEvents'); 
                                util.localStorage('glimpseEvents', null);

                                if (events && events.length > 0) {
                                    //Break up data into chunks to send off 
                                    var chunkedData = _.chunk(events, 45);
                                    for (var i = 0; i < chunkedData.length; i++) {
                                        var encoded_data  = _.base64Encode(JSON.stringify(chunkedData[i]));
                               
                                        $.post('//api.mixpanel.com/track/', 'data=' + encoded_data);
                                    } 
                                }  

                                //We want to try and send off our data every 5min or so
                                setTimeout(function() {
                                    report.batch();
                                //}, 300000);
                                }, 30000);
                            }
                        }; 
                    })(); 

                var timerInsightStart = new Date().getTime(),
                    staticCounter = function(key, property) { 
                        if (arguments.length == 1)
                            return report.settings(key, (report.settings(key) || 0) + 1);

                        var data = report.settings(key) || {};
                        data[property] = (data[property] || 0) + 1;
                        report.settings(key, data);

                        return data[property];
                    },
                    parseQueryString = function(url, backlist) {
                        var params = {}; 

                        if (url && url.indexOf('?') > -1) { 
                            var queries = url.substring(url.indexOf('?') + 1).split("&");
                            for (var i = 0; i < queries.length; i++) {
                                var temp = queries[i].split('=');
                                if (!backlist || backlist.indexOf(temp[0]) == -1)
                                    params[temp[0]] = temp[1];
                            }
                        }

                        return params;
                    };

                var events = {
                        startup: function() {
                            var initRun = report.init();
                            if (initRun)
                                report.track('$signup');
                        },
                        pageLoaded: function() {
                            report.track('page-load', { perfOffsetClientScriptLoad: timerInsightStart - timerStart, globalCountLoad: staticCounter('countLoad') });
                        },
                        systemInit: function() { 
                            var timerSystemInitStart;
                            pubsub.subscribe('trigger.system.init', function() {
                                timerSystemInitStart = new Date().getTime();
                            }, true);
                            pubsub.subscribe('trigger.system.ready', function() {
                                var duration = new Date().getTime() - timerSystemInitStart,
                                    offset = timerSystemInitStart - timerStart,
                                    defaultTab = settings.local('view') || '',
                                    isOpen = settings.local('isOpen') || false, 
                                    popupOn = settings.local('popupOn') || false,
                                    height = settings.local('height') || -1,
                                    width = $(window).width();
             
                                report.track('system-init', { perfDurationSystemInit: duration, perfOffsetSystemInitStart: offset, isOpen: isOpen, popupOn: popupOn, height: height, width: width, defaultTab: defaultTab });
                            });
                        },
                        systemHudInit: function() { 
                            var timerHudInitStart;
                            pubsub.subscribe('trigger.hud.init', function() {
                                timerHudInitStart = new Date().getTime();
                            }, true);
                            pubsub.subscribe('trigger.hud.ready', function() {
                                var duration = new Date().getTime() - timerHudInitStart,
                                    offset = timerHudInitStart - timerStart,
                                    sections = $.map($('.glimpse .glimpse-hud-section > .glimpse-hud-title'), function(val, i) { return $(val).text(); }); 
              
                                report.track('system-hud-init', { perfDurationHudInit: duration, perfOffsetHudInitStart: offset, sections: sections });
             
                                // fact - hud focus event
                                var countHudHover = 1;
                                $('.glimpse').on('transitionend webkitTransitionEnd oTransitionEnd MSTransitionEnd', '.glimpse-hud-popup', function() { 
                                    var element = $(this); 
                                    if (element.height() > 0)
                                        element.data('start-time', new Date().getTime());
                                    else {
                                        var startTime = element.data('start-time');
                                        if (startTime != null) {
                                            element.data('start-time', null);
                            
                                            var duration = new Date().getTime() - startTime,
                                                offset = startTime - glimpse.timerStart,
                                                section = element.find('.glimpse-hud-title').text();
                         
                                            report.track('system-hud-focus', { pageDurationHudFocus: duration, pageCountHudFocus: countHudHover++, pageOffsetHudFocus: offset, section: section }); 
                                        }
                                    }    
                                });
                            });
                        },
                        systemDataLoaded: function() { 
                            var timerDataInitStart;
                            pubsub.subscribe('action.data.initial.changing', function() {
                                timerDataInitStart = new Date().getTime();
                            }, true);
                            pubsub.subscribe('action.data.initial.changed', function(data) {
                                var duration = new Date().getTime() - timerDataInitStart,
                                    offset = timerDataInitStart - timerStart
                                    tabs = [],
                                    version = glimpse.data.currentMetadata().version || '',
                                    oldVersion = report.settings('version');
                 
                                report.register({ version: version });
                 
                                if (version != oldVersion) {
                                    report.settings('version', version);

                                    report.track('system-version-change', { oldVersion: (oldVersion || '0.0.0') });
                                }

                                for (var key in data.newData.data)
                                    tabs.push(key);

                                report.track('system-data-init', { perfDurationDataInit: duration, perfOffsetDataInitStart: offset, tabs: tabs });
                            });
                        },
                        systemOpened: function() {
                            var countShellOpened = 1;
                            pubsub.subscribe('action.shell.opened', function(data) {
                                var offset = new Date().getTime() - timerStart,
                                    tab = settings.local('view') || '';
                 
                                report.track('system-shell-opened', { pageOffsetOpened: offset, pageCountOpen: countShellOpened++, globalCountOpen: staticCounter('countOpen'), isInitial: data.isInitial, tab: tab });
                            });
                        },
                        systemMinimized: function() {
                            var countShellMinimized = 1;
                            pubsub.subscribe('action.shell.minimizing', function() {
                                var offset = new Date().getTime() - timerStart,
                                    tab = settings.local('view') || '';

                                report.track('system-shell-minimizing', { pageOffsetMinimized: offset, pageCountMinimized: countShellMinimized++, globalCountMinimized: staticCounter('countMinimized'), tab: tab });
                            });
                        },
                        systemClose: function() { 
                            pubsub.subscribe('action.shell.closed', function() {
                                var offset = new Date().getTime() - timerStart,
                                    tab = settings.local('view') || '';

                                report.track('system-shell-closed', { pageOffsetClosed: offset, globalCountClosed: staticCounter('countClosed') });
                            });
                        },
                        systemTabRendered: function() {
                            var timerTabRenderStart;
                            pubsub.subscribe('action.panel.rendering', function() {
                                timerTabRenderStart = new Date().getTime();
                            }, true);
                            pubsub.subscribe('action.panel.rendered', function(data) {
                                var duration = new Date().getTime() - timerTabRenderStart,
                                    offset = timerTabRenderStart - timerStart,
                                    key = data.key || '';
                  
                                report.track('system-tab-rendered', { perfDurationTabRender: duration, pageOffsetTabRenderStart: offset, key: key });
                            });

                        },
                        systemTabSelected: function() {
                            var countTabSelect = 1,
                                countTabInstanceSelect = {};
                            pubsub.subscribe('trigger.tab.select', function(data) {
                                var offset = new Date().getTime() - timerStart,
                                    tab = data.key || '',
                                    oldTab = data.oldKey || '', 
                                    countTabInstanceSelectVal = (countTabInstanceSelect[tab] = (countTabInstanceSelect[tab] || 0) + 1);
                  
                                report.track('system-tab-selected', {  pageOffsetTabSelect: offset, pageCountTabSelect: countTabSelect++, pageCountTabInstanceSelect: countTabInstanceSelectVal, globalCountTabSelect: staticCounter('countTabSelect'), globalCountTabInstanceSelect: staticCounter('countTabInstanceSelect', tab), tab: tab, oldTab: oldTab });
                            });
                        },
                        tabFocus: function() {
                            var timerTabFocus,
                                argsTabFocusStart;
                            pubsub.subscribe('action.tab.focus.start', function(args) {
                                timerTabFocus = new Date().getTime();
                                argsTabFocusStart = args;
                            }, true);
                            pubsub.subscribe('action.tab.focus.stop', function(data) {
                                var duration = new Date().getTime() - timerTabFocus,
                                    tab = data.key || '',
                                    startCause = argsTabFocusStart ? (argsTabFocusStart.isOpening ? 'ConsoleOpening' : 'TabSwitch') : '',
                                    stopCause = data.userTermination ? 'ConsoleClosed' : data.switchingTab ? 'TabSwitch' : 'PageRedirect';
                 
                                report.track('system-tab-focused', { pageDurationTabFocus: duration, tab: tab, startCause: startCause, stopCause: stopCause });
                            });
                        },
                        pluginVersionChange: function() { 
                            var plugins = report.settings('plugins'),
                                newPlugins = settings.local('versionCheckUri');
                            if ((plugins || newPlugins) && plugins != newPlugins) {
                                report.settings('plugins', newPlugins);

                                var blacklist = [ 'stamp', 'callback' ],
                                    newPluginsList = parseQueryString(newPlugins, blacklist),
                                    pluginsList = parseQueryString(plugins, blacklist);

                                for (var key in pluginsList) {
                                    pluginsList[key] = pluginsList[key] + '|' + (newPluginsList[key] || '0.0.0');
                                    delete newPluginsList[key];
                                }

                                for (key in newPluginsList) {
                                    pluginsList[key] = '0.0.0|' + newPluginsList[key];
                                }
   
                                report.track('system-plugin-change', pluginsList); 
                            }
                        }
                    };
                     
                for (var key in events) 
                    events[key](); 
            };

    glimpse.extensions.push(function() {
        init(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.util);
    }); 
})(glimpse);