(function($, pubsub, data, elements, util) {
    var serverTime = 0,
        modify = function(options) {
            options.templates.css += '/*(import:glimpse.hud.css)*/';
        },
        loaded = function(args) {
            var html = '',
                tabData = args.newData.data.glimpse_hud,
                opened = state.current();

            html += display.http.render(tabData.data, opened[0]);
            html += display.host.render(tabData.data, opened[1]);
            html += display.ajax.render(tabData.data, opened[2]);

            elements.opener().prepend('<div class="glimpse-hud">' + html + '</div>');
            state.setup(); 
        }, 
        state = (function() {
            return { 
                setup: function () {
                   var inputs = elements.opener().find('.glimpse-hud-section-input').change(function() {
                       var state = [];
                       inputs.each(function() { state.push(this.checked); });
                       util.localStorage('glimpseHudDisplay', state);
                   });
                },
                current: function () {
                    return util.localStorage('glimpseHudDisplay') || [];
                }
            };
        })(),
        display = function() {
            var rendering = (function() {
                    var sizes = [ 'extra-large', 'large', 'normal', 'small' ],
                        position = [ 'top', 'bottom', 'left', 'right' ],
                        align = [ 'left', 'right' ],
                        shouldUse = function(isVisible, tabData) {
                            if (isVisible !== undefined && isVisible) {
                                var isFunction = $.isFunction(isVisible);
                                return (isFunction && isVisible(tabData)) || (!isFunction && isVisible);
                            }
                            return true;
                        }, 
                        popup = function(structure, tabData) {
                            return '<div class="glimpse-hud-popup" style="border-color:' + structure.color + ';"><div class="glimpse-hud-title">' + structure.title + '</div><div class="glimpse-hud-popup-inner">' + structure.popup.render(tabData) + '</div></div>';
                        },
                        section = function(structure, tabData, opened) {
                            var html = '<div class="glimpse-hud-section glimpse-hud-section-' + structure.id + '" style="border-color:' + structure.color + '">';
                            
                            html += '<label class="glimpse-hud-title" for="glimpse-hud-section-input-' + structure.id + '">' + structure.title + '</label><input type="checkbox" class="glimpse-hud-section-input" id="glimpse-hud-section-input-' + structure.id + '"' + (opened ? ' checked="checked"' : '') + ' />';
                            html += '<div class="glimpse-hud-section-inner">';  
                            for (var key in structure.layout.mini) {
                                html += item(structure.layout.mini[key], tabData);
                            }
                            html += '</div>';
                            
                            if (!structure.popup.suppress) { html += popup(structure, tabData); }
                            
                            return html + '</div>';
                        },
                        item = function(item, tabData) {
                            var html = '';
                            if (shouldUse(item.visible, tabData)) {
                                var title = '<div class="glimpse-hud-header">' + item.title + '</div>',
                                    postfix = item.postfix ? '<span class="glimpse-hud-postfix">' + item.postfix + '</span>' : '',
                                    value = item.getLayoutData ? item.getLayoutData(tabData) : '<span class="glimpse-hud-data">' + item.getData(tabData) + '</span>' + postfix,
                                    id = item.id ? ' ' + item.id : '';
                                
                                html += item.getLayout ? item.getLayout(tabData) : '<div class="glimpse-hud-detail glimpse-hud-detail-' + sizes[item.size] + ' glimpse-hud-detail-' + position[item.position] + ' glimpse-hud-detail-' + align[item.align] + id + '" title="' + item.description + '">' + (!item.position ? title : '') + '<div class="glimpse-hud-value">' + value + '</div>' + (item.position ? title : '') + '</div>';
                            }

                            return html;
                        };
                
                    return {
                        section: section,
                        item: item,
                        popup: popup
                    };
                })(),
                process = (function() {
                    var item = function(layout, defaults) {
                        for (var key in layout) {
                            layout[key] = $.extend(true, {}, defaults[key], layout[key]);
                        }
                    };
                
                    return {
                        init: function(payload) {
                            item(payload.layout.mini, payload.defaults);
                            item(payload.layout.popup, payload.defaults);
                        }
                    };
                })();

            return {
                http: function() {
                    var timingsRaw = (window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {}).timing,
                        structure = {
                            title: 'HTTP',
                            id: 'http', 
                            color: '#e2875e',
                            popup: {
                                render: function(tabData) {
                                    var html = '<div class="glimpse-hud-popup-header">Browser Request</div>';
                                    html += '<div>' + rendering.item(structure.layout.popup.request, tabData) + '</div>';
                                    html += '<div class="glimpse-hud-popup-clear"></div>';
                                    html += '<div class="glimpse-data-request-parts"><table><tr><td colspan="3"><div class="glimpse-hud-bar glimpse-hud-tooltips-non"><div><div class="glimpse-hud-bar-item" style="width: 100%;background-color: ' + tabData.timings.browser.categoryColor + '"></div><div class="glimpse-hud-bar-item" style="width: ' + tabData.timings.server.percentage + '%;background-color: ' + tabData.timings.server.categoryColor + ';"></div><div class="glimpse-hud-bar-item" style="width: ' + tabData.timings.network.percentage + '%;background-color: ' + tabData.timings.network.categoryColor + ';"></div></div></div></td></tr><tr><td class="glimpse-data-wire-part">' + rendering.item(structure.layout.popup.wire, tabData) + '</td><td class="glimpse-data-server-part">' + rendering.item(structure.layout.popup.server, tabData) + '</td><td class="glimpse-data-client-part">' + rendering.item(structure.layout.popup.client, tabData) + '</td></tr></table></div>'; 

                                    return html;
                                }
                            },
                            defaults: {
                                request: { title: 'Request', description: 'Total request time from click to dom ready', visible: true, size: 1, position: 0, align: 0, postfix: 'ms', getData: function(tabData) { return tabData.timings.total.duration; } },
                                wire: { title: 'Wire', description: 'Total time on the network', visible: true, size: 2, position: 0, align: 0, postfix: 'ms', getData: function(tabData) { return tabData.timings.network.duration; } },
                                server: { title: 'Server', description: 'Total time on the server', visible: true, size: 2, position: 0, align: 0, postfix: 'ms', getData: function(tabData) { return tabData.timings.server.duration; } },
                                client: { title: 'Client', description: 'Total time once client kicks in to dom ready', visible: true, size: 2, position: 0, align: 0, postfix: 'ms', getData: function(tabData) { return tabData.timings.browser.duration; } }
                            },
                            layout: {
                                mini: {
                                    request: {},
                                    wire: {},
                                    server: {},
                                    client: {},
                                },
                                popup: {
                                    request: { title: 'Total Request Time', size: 0, position: 1, align: 1 },
                                    wire: { position: 1, align: 1 },
                                    server: { position: 1, align: 1 },
                                    client: { position: 1, align: 1 },
                                }
                            }
                        }, 
                        processTimings = function(tabData) {
                            var result = { },
                                networkPre = calculateTimings('navigationStart', 'requestStart'),
                                networkPost = calculateTimings('responseStart', 'responseEnd'),
                                network = networkPre + networkPost,
                                server = calculateTimings('requestStart', 'responseStart'),
                                browser = calculateTimings('responseEnd', 'loadEventEnd'),
                                total = network + server + browser;
                      
                            result.networkSending = { categoryColor: '#FDBF45', duration: networkPre, percentage: (networkPre / total) * 100 };
                            result.networkReceiving = { categoryColor: '#FDBF45', duration: networkPost, percentage: (networkPost / total) * 100 };
                            result.network = { categoryColor: '#FDBF45', duration: network, percentage: (network / total) * 100 };
                            result.server = { categoryColor: '#AF78DD', duration: server, percentage: (server / total) * 100 };
                            result.browser = { categoryColor: '#72A3E4', duration: browser, percentage: (browser / total) * 100 };
                            result.total = { categoryColor: '#10E309', duration: network + server + browser, percentage: 100 };
                         
                            tabData.timings = result;
                        },
                        calculateTimings = function(startIndex, finishIndex) { 
                            return timingsRaw[finishIndex] - timingsRaw[startIndex];
                        }, 
                        render = function(tabData, opened) {
                            var html = '';
                            if (timingsRaw) {
                                process.init(structure);
                                processTimings(tabData); 
                                html = rendering.section(structure, tabData, opened); 
                            }

                            return html;
                        };
                
                    return {
                            render: render
                        };
                }(),
                host: function() {
                    var structure = {
                            title: 'Host',
                            id: 'host', 
                            color: '#6161e0',
                            popup: {
                                render: function(tabData) {
                                    var html = '<div class="glimpse-hud-popup-header">Server Side</div>';
                                    html += '<div><table class="glimpse-hud-summary glimpse-hud-summary-left"><tr><th>' + rendering.item(structure.layout.popup.server, tabData) + '</th></tr><tr><td>' + rendering.item(structure.layout.popup.controller, tabData) + '</td></tr></table>';
                                    html += '<table class="glimpse-hud-summary glimpse-hud-summary-right"><tr><td width="1">' + rendering.item(structure.layout.popup.action, tabData) + '</td><td width="40"></td><td>' + rendering.item(structure.layout.popup.connections, tabData) + '</td></tr><tr><td>' + rendering.item(structure.layout.popup.view, tabData) + '</td><td></td><td>' + rendering.item(structure.layout.popup.queries, tabData) + '</td></tr></table></div>';
                                    html += '<div class="glimpse-hud-popup-clear"></div>'; 
                                    html += '<table class="glimpse-hud-listing" style="table-layout:fixed;"><thead><tr><th></th><th class="glimpse-hud-listing-value glimpse-data-childless-duration">duration (ms)</th><th class="glimpse-hud-listing-value glimpse-data-childless-start-point">from start (ms)</th></tr></thead>';  
                                    for (var i = 0; i < tabData.server.events.length; i++) {
                                        var item = tabData.server.events[i];
                                        html += '<tr ' + (item.childlessDuration < 2 ? 'class="glimpse-hud-quite glimpse-data-trivial"' : '') + '><td class="glimpse-hud-listing-overflow" style="padding-left:' + (item.executionIndex * 15) + 'px;" title="' + item.description + '">' + item.description + '</td><td class="glimpse-hud-listing-value glimpse-data-childless-duration">' + item.childlessDuration + '</td><td class="glimpse-hud-listing-value glimpse-data-childless-start-point"><span class="glimpse-hud-prefix">+</span>' + item.startPoint + '</td></tr>';
                                    }    
                                    html += '</table>';

                                    return html;
                                }
                            },
                            defaults: {
                                server: { title: 'Total Server Time', description: 'Total time on the server', visible: true, size: 0, position: 1, align: 1, postfix: 'ms', getData: function(tabData) { return tabData.timings.server.duration; } },
                                action: { title: 'Action', description: 'How long root Action took to execute', visible: function(tabData) { return tabData.mvc; }, size: 1, position: 0, align: 0, postfix: 'ms', getData: function(tabData) { return parseInt(tabData.mvc.actionExecutionTime); } },
                                view: { title: 'View', description: 'How long root View took to render', visible: function(tabData) { return tabData.mvc; }, size: 1, position: 0, align: 0, postfix: 'ms', getData: function(tabData) { return parseInt(tabData.mvc.viewRenderTime); } },
                                controller: { title: 'Controller/Action', description: 'Name of the root Controller and Action', visible: function(tabData) { return tabData.mvc; }, size: 2, position: 0, align: 0, postfix: 'ms', getLayoutData: function(tabData) { return '<span class="glimpse-hud-data">' + tabData.mvc.controllerName + '</span><span class="glimpse-hud-plain">.</span><span class="glimpse-hud-data">' + tabData.mvc.actionName + '</span><span class="glimpse-hud-plain">(...)</span>'; } },
                                queries: { title: 'Queries', description: 'Total query duration and number of all SQL queries', visible: function(tabData) { return tabData.sql; }, size: 1, position: 0, align: 0, getLayoutData: function(tabData) { return '<span class="glimpse-hud-data">' + parseInt(tabData.sql.queryExecutionTime) + '</span><span class="glimpse-hud-postfix">ms</span><span class="glimpse-hud-spacer">/</span><span class="glimpse-hud-data">'  + tabData.sql.queryCount + '</span>'; } },
                                connections: { title: 'Connections', description: 'Total query duration and number of all SQL queries', visible: function(tabData) { return tabData.sql; }, size: 1, position: 1, align: 1, getLayoutData: function(tabData) { return '<span class="glimpse-hud-data">' + parseInt(tabData.sql.connectionOpenTime) + '</span><span class="glimpse-hud-postfix">ms</span><span class="glimpse-hud-spacer">/</span><span class="glimpse-hud-data">'  + tabData.sql.connectionCount + '</span>'; } }
                            },
                            layout: {
                                mini: {
                                    action: {},
                                    view: {},
                                    controller: {},
                                    queries: {},
                                },
                                popup: {
                                    server: {},
                                    action: { position: 1, align: 1 },
                                    view: { position: 1, align: 1 },
                                    controller: { position: 1, align: 1 },
                                    queries: { position: 1, align: 1 },
                                    connections: {},
                                }
                            }
                        },
                        processData = function(tabData) {
                            tabData.server = { 
                                    events: [{
                                        title: 'Store.Browse',
                                        startTime: '06/03/2013 14:19:38',
                                        duration: 4.12,
                                        startPoint: 1.67,
                                        category: 'Controller'
                                    },
                                    {
                                        title: 'Browse',
                                        startTime: '06/03/2013 14:19:38',
                                        duration: 0.12,
                                        startPoint: 1.98,
                                        category: 'Find'
                                    },
                                    {
                                        title: 'Browse',
                                        startTime: '06/03/2013 14:19:42',
                                        duration: 1.9,
                                        startPoint: 4.98,
                                        category: 'Render'
                                    },
                                    {
                                        title: '_RSVPStatus',
                                        startTime: '06/03/2013 14:19:43',
                                        duration: 0.9,
                                        startPoint: 5.01,
                                        category: 'Find'
                                    },
                                    {
                                        title: '_RSVPStatus',
                                        startTime: '06/03/2013 14:19:44',
                                        duration: 1.2,
                                        startPoint: 6.21,
                                        category: 'Render partial'
                                    }]
                                };  
                        }, 
                        processEvents = function(tabData) { 
                            var eventStack = [], 
                                lastEvent = { startPoint : 0, duration : 0, childlessDuration : 0, endPoint : 0 },
                                executionIndex = 0,
                                rootDuration = tabData.timings.server.duration,
                                rootChildlessDuration = rootDuration;
                            
                            for (var i = 0; i < tabData.server.events.length; i += 1) {
                                var event = tabData.server.events[i],
                                    topEvent = eventStack.length > 0 ? eventStack[eventStack.length - 1] : null, 
                                    left = (event.startPoint / rootDuration) * 100,  
                                    width = (event.duration / rootDuration) * 100, 
                                    stackParsed = false;

                                event.endPoint = parseFloat((event.startPoint + event.duration).toFixed(2));

                                //Derive event nesting  
                                while (!stackParsed) {
                                    if (event.startPoint > lastEvent.startPoint && event.endPoint <= lastEvent.endPoint) { 
                                        eventStack.push(lastEvent); 
                                        stackParsed = true;
                                    }
                                    else if (topEvent != null && topEvent.endPoint < event.endPoint) {
                                        eventStack.pop(); 
                                        topEvent = eventStack.length > 0 ? eventStack[eventStack.length - 1] : null; 
                                        stackParsed = false;
                                    }
                                    else {
                                        stackParsed = true;
                                        executionIndex++;
                                    }
                                }
                        
                                //Work out childless timings 
                                var temp = eventStack.length > 0 ? eventStack[eventStack.length - 1] : undefined; 
                                if (temp) {
                                    temp.childlessDuration = parseFloat((temp.childlessDuration - event.duration).toFixed(2));
                                } 

                                //Work out root childless timings 
                                if (eventStack.length == 0)
                                    rootChildlessDuration -= event.duration;

                                //Save calculate data
                                event.childlessDuration = event.duration;
                                event.startPercent = left;
                                event.endPercent = left + width;
                                event.widthPercent = width;
                                event.nesting = eventStack.length;
                                event.executionIndex = executionIndex + eventStack.length;
                                event.description = event.category + ': ' + event.title;
                        
                                lastEvent = event;
                            }
                             
                            tabData.server.events.unshift({
                                    description: 'Request: ' + (window.location.pathname + window.location.search),
                                    title: (window.location.pathname + window.location.search),
                                    startTime: 'NOT SURE',
                                    duration: rootDuration,
                                    startPoint: '0.0',
                                    category: 'Request',
                                    childlessDuration: Math.round(rootChildlessDuration * 10) / 10,
                                    startPercent: 0,
                                    endPercent: 100,
                                    widthPercent: 100,
                                    nesting: 0,
                                    executionIndex: 0
                                }); 
                        }, 
                        render = function(tabData, opened) {
                            var html = '';
                            if (tabData.mvc || tabData.sql) {
                                process.init(structure);
                                processData(tabData);
                                processEvents(tabData);
                                html = rendering.section(structure, tabData, opened); 
                            }

                            return html;
                        };
                    
                    return {
                            render: render
                        };
                }(),
                ajax: function() {
                    var open = XMLHttpRequest.prototype.open,
                        count = 0,
                        summaryStack = [],
                        detailStack = [],
                        structure = {
                            title: 'Ajax',
                            id: 'ajax',
                            color: '#559fdf',
                            popup: {
                                suppress: true,
                                render: function(tabData) {
                                    var html = '<div class="glimpse-hud-popup-header">Ajax Requests</div>';
                                    html += '<div>' + rendering.item(structure.layout.popup.requests, tabData) + '</div>';
                                    html += '<div class="glimpse-hud-popup-clear"></div>';
                                    html += '<table style="table-layout:fixed;" class="glimpse-hud-listing glimpse-data-ajax-detail"><thead><tr><th class="glimpse-data-content-method"></th><th></th><th class="glimpse-hud-listing-value glimpse-data-duration">duration (ms)</th><th class="glimpse-hud-listing-value glimpse-data-size">size (kb)</th></tr></thead>';
                                    html += '</table>';

                                    return html;
                                }
                            },
                            defaults: {
                                requests: { title: 'Count', id: 'glimpse-data-ajax-count', description: 'Total Ajax requests detected on this page', visible: true, size: 1, position: 0, align: 0, getData: function(tabData) { return 0; } }
                            },
                            layout: {
                                mini: {
                                    requests: { }
                                },
                                popup: {
                                    requests: { title: 'Total Ajax Requests', size: 0, position: 1, align: 1 },
                                }
                            }
                        },
                        processContentType = function(type) {
                            return type.substring(0, type.indexOf(';'));
                        },
                        render = function(tabData, opened) {
                            process.init(structure);

                            return rendering.section(structure, tabData, opened);
                        },
                        update = function(method, uri, duration, size, status, statusText, time, contentType) {
                            //Add it when needed
                            if (count == 0) {
                                var section = $('.glimpse-hud-section-ajax');
                                section.find('.glimpse-hud-section-inner').append('<div class="glimpse-hud-detail glimpse-hud-detail-small glimpse-hud-listing glimpse-data-ajax-summary"></div>');
                                section.append(rendering.popup(structure, { }));
                            }

                            //Set the counter
                            var counter = $('.glimpse-data-ajax-count .glimpse-hud-data').text(++count).addClass('glimpse-hud-value-update');
                            setTimeout(function() {
                                counter.removeClass('glimpse-hud-value-update');
                            }, 2000);
                             
                            //Update data records
                            var rowClass = (status == 304 ? ' glimpse-hud-quite' : !(status >= 200 && status < 300) ? ' glimpse-hud-error' : '');
                            recordItem('<div class="glimpse-hud-listing-row glimpse-hud-value' + rowClass + '"><div class="glimpse-hud-data glimpse-hud-quite glimpse-data-ajax-method">' + method + '</div><div class="glimpse-hud-data glimpse-hud-listing-overflow glimpse-data-ajax-uri" title="' + uri + '">' + uri + test + '</div><div class="glimpse-data-ajax-duration"><span class="glimpse-hud-data">' + duration + '</span><span class="glimpse-hud-postfix">ms</span></div></div>', '.glimpse-hud-section-ajax .glimpse-data-ajax-summary', summaryStack, 2);
                            recordItem('<tbody class="' + rowClass + '"><tr><td class="glimpse-hud-listing-overflow" title="' + uri + '" colspan="2">' + uri + test + '</td><td class="glimpse-hud-listing-value glimpse-data-duration">' + duration + '</td><td class="glimpse-hud-listing-value glimpse-data-size">' + (Math.round((size / 1024) * 10) / 10) + '</td></tr><tr><td class="glimpse-hud-quite glimpse-data-content-method">' + method + '</td><td class="glimpse-hud-quite glimpse-hud-listing-overflow">' + status + ' - ' + statusText + '</td><td class="glimpse-hud-quite glimpse-data-content-type glimpse-hud-listing-overflow" title="' + contentType + '">' + processContentType(contentType) + '</td><td class="glimpse-hud-quite glimpse-data-content-time">' + time.toTimeString().replace(/.*(\d{2}:\d{2}:\d{2}).*/, "$1") + '</td></tr></tbody>', '.glimpse-hud-section-ajax .glimpse-data-ajax-detail', detailStack, 6);
                        },
                        recordItem = function(html, selector, stack, length) {
                            //Set row
                            var row = $(html).prependTo(selector);
                            setTimeout(function() {
                                row.addClass('added');
                            }, 1);

                            //Track state of the details
                            if (stack.length >= length)
                                stack.shift().remove();
                            stack.push(row);
                        };
                        /*
                        record = function(method, uri, duration) { 
                            /*
                            timeSince = function(ms) {
                                var seconds = ms / 1000,
                                    interval = Math.floor(seconds / 60);
                                if (interval >= 1) return [ interval, "m" ];

                                return [ Math.floor(seconds), "s" ];
                            },
                            var row = $('<div class="glimpse-hud-listing-row glimpse-hud-value"><div class="glimpse-hud-data glimpse-hud-quite glimpse-data-ajax-method">' + method + '</div><div class="glimpse-hud-data glimpse-hud-listing-overflow glimpse-data-ajax-uri" title="' + uri + '">' + uri + '</div><div class="glimpse-data-ajax-duration"><span class="glimpse-hud-data">' + duration + '</span><span class="glimpse-hud-postfix">ms</span></div><div class="glimpse-data-ajax-past"></div></div>').prependTo('.glimpse-hud-section-ajax .glimpse-hud-listing'),
                                item = { row: row, canceled: false },
                                past = row.find('.glimpse-data-ajax-past'),
                                next = 0,
                                total = 0;
                            //Work out the timing count up
                            var timer = function() {
                                    setTimeout(function() {
                                        var since = timeSince(total);
                                        past.html('<span class="glimpse-hud-data">' + since[0] + '</span><span class="glimpse-hud-postfix">' + since[1] + ' ago</span>');
                                
                                        next = total < 10000 ? 1000 : total < 30000 ? 5000 : total < 60000 ? 10000 : 60000;
                                        total += next;
                                 
                                        if (!item.canceled)
                                            timer();
                                    }, next);
                                };
                            timer(); 
                        };
                        */ 
                     
                    
                    XMLHttpRequest.prototype.open = function(method, uri, async, user, pass) {
                        var startTime = new Date().getTime();
                
                        this.addEventListener("readystatechange", function() {
                                if (this.readyState == 4)  { 
                                    update(method, uri, new Date().getTime() - startTime, this.getResponseHeader("Content-Length"), this.status, this.statusText, new Date(), this.getResponseHeader("Content-Type"));
                                }
                            }, false); 
                
                        open.apply(this, arguments);
                    }; 

                    return {
                            render: render
                        };
                }()
            };
            
        }();

    pubsub.subscribe('action.template.processing', modify); 
    pubsub.subscribe('action.data.initial.changed', function(args) { $(window).load(function() { setTimeout(function() { loaded(args); }, 0); }); }); 

})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements, glimpse.util);
