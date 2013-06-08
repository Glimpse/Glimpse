(function($, pubsub, data, elements, util) {
    var serverTime = 0,
        modify = function(options) {
            options.templates.css += '/*(import:glimpse.hud.css)*/';
        },
        loaded = function(args) {
            var html = '',
                details = args.newData.hud,
                opened = state.current();

            html += display.http.render(details, opened[0]);
            if (details.mvc)
                html += display.host.render(details, opened[1]);
            html += display.ajax.render(details, opened[2]);

            elements.opener().prepend('<div class="glimpse-hud">' + html + '</div>');
            state.setup();

            display.host.postRender();
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
                        shouldUse = function(isVisible, details) {
                            if (isVisible !== undefined && isVisible) {
                                var isFunction = $.isFunction(isVisible);
                                return (isFunction && isVisible(details)) || (!isFunction && isVisible);
                            }
                            return true;
                        }, 
                        popup = function(structure, details) {
                            return '<div class="glimpse-hud-popup" style="border-color:' + structure.color + ';"><div class="glimpse-hud-title">' + structure.title + '</div><div class="glimpse-hud-popup-inner">' + structure.popup.render(details) + '</div></div>';
                        },
                        section = function(structure, details, opened) {
                            var html = '<div class="glimpse-hud-section glimpse-hud-section-' + structure.id + '" style="border-color:' + structure.color + '">';
                            
                            html += '<label class="glimpse-hud-title" for="glimpse-hud-section-input-' + structure.id + '">' + structure.title + '</label><input type="checkbox" class="glimpse-hud-section-input" id="glimpse-hud-section-input-' + structure.id + '"' + (opened ? ' checked="checked"' : '') + ' />';
                            html += '<div class="glimpse-hud-section-inner">';  
                            for (var key in structure.layout.mini) {
                                html += item(structure.layout.mini[key], details);
                            }
                            html += '</div>';
                            
                            if (!structure.popup.suppress) { html += popup(structure, details); }
                            
                            return html + '</div>';
                        },
                        item = function(item, details) {
                            var html = '';
                            if (shouldUse(item.visible, details)) {
                                var title = '<div class="glimpse-hud-header">' + item.title + '</div>',
                                    postfix = item.postfix ? '<span class="glimpse-hud-postfix">' + item.postfix + '</span>' : '',
                                    value = item.getLayoutData ? item.getLayoutData(details) : '<span class="glimpse-hud-data">' + item.getData(details) + '</span>' + postfix,
                                    id = item.id ? ' ' + item.id : '';
                                
                                html += item.getLayout ? item.getLayout(details) : '<div class="glimpse-hud-detail glimpse-hud-detail-' + sizes[item.size] + ' glimpse-hud-detail-' + position[item.position] + ' glimpse-hud-detail-' + align[item.align] + id + '" title="' + item.description + '">' + (!item.position ? title : '') + '<div class="glimpse-hud-value">' + value + '</div>' + (item.position ? title : '') + '</div>';
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
                                render: function(details) {
                                    var requestDetails = details.request.data,
                                        html = '<div class="glimpse-hud-popup-header">Browser Request</div>';
                                    html += '<div>' + rendering.item(structure.layout.popup.request, details) + '</div>';
                                    html += '<div class="glimpse-hud-popup-clear"></div>';
                                    html += '<div class="glimpse-data-request-parts"><table><tr><td colspan="3"><div class="glimpse-hud-bar glimpse-hud-tooltips-non"><div><div class="glimpse-hud-bar-item" style="width: 100%;background-color: ' + requestDetails.browser.categoryColor + '"></div><div class="glimpse-hud-bar-item" style="width: ' + requestDetails.server.percentage + '%;background-color: ' + requestDetails.server.categoryColor + ';"></div><div class="glimpse-hud-bar-item" style="width: ' + requestDetails.network.percentage + '%;background-color: ' + requestDetails.network.categoryColor + ';"></div></div></div></td></tr><tr><td class="glimpse-data-wire-part">' + rendering.item(structure.layout.popup.wire, details) + '</td><td class="glimpse-data-server-part">' + rendering.item(structure.layout.popup.server, details) + '</td><td class="glimpse-data-client-part">' + rendering.item(structure.layout.popup.client, details) + '</td></tr></table></div>'; 

                                    return html;
                                }
                            },
                            defaults: {
                                request: { title: 'Request', description: 'Total request time from click to dom ready', visible: true, size: 1, position: 0, align: 0, postfix: 'ms', getData: function(details) { return details.request.data.total.duration; } },
                                wire: { title: 'Wire', description: 'Total time on the network', visible: true, size: 2, position: 0, align: 0, postfix: 'ms', getData: function(details) { return details.request.data.network.duration; } },
                                server: { title: 'Server', description: 'Total time on the server', visible: true, size: 2, position: 0, align: 0, postfix: 'ms', getData: function(details) { return details.request.data.server.duration; } },
                                client: { title: 'Client', description: 'Total time once client kicks in to dom ready', visible: true, size: 2, position: 0, align: 0, postfix: 'ms', getData: function(details) { return details.request.data.browser.duration; } }
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
                        processTimings = function(details) {
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
                         
                            details.request = { data: result, name: 'Request' };
                        },
                        calculateTimings = function(startIndex, finishIndex) { 
                            return timingsRaw[finishIndex] - timingsRaw[startIndex];
                        }, 
                        render = function(details, opened) {
                            var html = '';
                            if (timingsRaw) {
                                process.init(structure);
                                processTimings(details); 
                                html = rendering.section(structure, details, opened); 
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
                                render: function(details) {
                                    var hasTrivial = false,
                                        html = '<div class="glimpse-hud-popup-header">Server Side</div>';
                                    html += '<div><table class="glimpse-hud-summary glimpse-hud-summary-left"><tr><th>' + rendering.item(structure.layout.popup.server, details) + '</th></tr><tr><td>' + rendering.item(structure.layout.popup.controller, details) + '</td></tr></table>';
                                    html += '<table class="glimpse-hud-summary glimpse-hud-summary-right"><tr><td width="1">' + rendering.item(structure.layout.popup.action, details) + '</td>' + (details.sql ? '<td width="40"></td><td>' + rendering.item(structure.layout.popup.connections, details) + '</td>' : '') + '</tr><tr><td>' + rendering.item(structure.layout.popup.view, details) + '</td>' + (details.sql ? '<td></td><td>' + rendering.item(structure.layout.popup.queries, details) + '</td>' : '') + '</tr></table></div>';
                                    html += '<div class="glimpse-hud-popup-clear"></div>'; 
                                    html += '<table class="glimpse-hud-listing" style="table-layout:fixed;"><thead><tr><th></th><th class="glimpse-hud-listing-value glimpse-data-childless-duration">duration (ms)</th><th class="glimpse-hud-listing-value glimpse-data-childless-start-point">from start (ms)</th></tr></thead>';  
                                    for (var i = 0; i < details.timings.data.length; i++) {
                                        var item = details.timings.data[i],
                                            isTrivial = item.childlessDuration < 2;
                                        
                                        if (!item.suppress) {
                                            html += '<tbody' + (isTrivial ? ' class="glimpse-data-trivial"' : '') + '>';
                                            html += '<tr' + (isTrivial ? ' class="glimpse-hud-quite"' : '') + '><td class="glimpse-hud-listing-overflow" style="padding-left:' + (item.nesting * 15) + 'px;" title="' + item.description + '">' + item.description + '</td><td class="glimpse-hud-listing-value glimpse-data-childless-duration">' + item.childlessDuration + '</td><td class="glimpse-hud-listing-value glimpse-data-childless-start-point"><span class="glimpse-hud-prefix">+</span>' + item.startPoint + '</td></tr>';
                                            if (item.queries && item.queries.listing.length > 0) {
                                                html += '<tr><td style="padding-left:' + ((item.nesting * 15) + 20) + 'px;"><span class="glimpse-hud-prefix">➥</span>' + item.queries.listing.length + '<span class="glimpse-hud-postfix">queries</span> <span class="glimpse-hud-listing-value">' + item.queries.durationSum.toFixed(2) + '</span><span class="glimpse-hud-postfix">ms</span></td><td></td><td></td></tr>';
                                            }
                                            html += '</tbody>'
                                            if (isTrivial) { hasTrivial = true; }
                                        }
                                    }    
                                    html += '</table>';
                                    if (hasTrivial) {
                                        html += '<div class="glimpse-hud-controls"><span class="glimpse-control-trivial">Show Trivial</span><span class="glimpse-control-trivial" style="display:none">Hide Trivial</span></div>';
                                    }

                                    return html;
                                }
                            },
                            defaults: {
                                server: { title: 'Total Server Time', description: 'Total time on the server', visible: true, size: 0, position: 1, align: 1, postfix: 'ms', getData: function(details) { return details.request.data.server.duration; } },
                                action: { title: 'Action', description: 'How long root Action took to execute', visible: function(details) { return details.mvc; }, size: 1, position: 0, align: 0, postfix: 'ms', getData: function(details) { return parseInt(details.mvc.data.actionExecutionTime); } },
                                view: { title: 'View', description: 'How long root View took to render', visible: function(details) { return details.mvc; }, size: 1, position: 0, align: 0, postfix: 'ms', getData: function(details) { return parseInt(details.mvc.data.viewRenderTime); } },
                                controller: { title: 'Controller/Action', description: 'Name of the root Controller and Action', visible: function(details) { return details.mvc; }, size: 2, position: 0, align: 0, postfix: 'ms', getLayoutData: function(details) { return '<span class="glimpse-hud-data">' + details.mvc.data.controllerName + '</span><span class="glimpse-hud-plain">.</span><span class="glimpse-hud-data">' + details.mvc.data.actionName + '</span><span class="glimpse-hud-plain">(...)</span>'; } },
                                queries: { title: 'Queries', description: 'Total query duration and number of all SQL queries', visible: function(details) { console.log(details.sql); return details.sql; }, size: 1, position: 0, align: 0, getLayoutData: function(details) { return '<span class="glimpse-hud-data">' + parseInt(details.sql.data.queryExecutionTime) + '</span><span class="glimpse-hud-postfix">ms</span><span class="glimpse-hud-spacer">/</span><span class="glimpse-hud-data">'  + details.sql.data.queryCount + '</span>'; } },
                                connections: { title: 'Connections', description: 'Total query duration and number of all SQL queries', visible: function(details) { return details.sql; }, size: 1, position: 1, align: 1, getLayoutData: function(details) { return '<span class="glimpse-hud-data">' + parseInt(details.sql.data.connectionOpenTime) + '</span><span class="glimpse-hud-postfix">ms</span><span class="glimpse-hud-spacer">/</span><span class="glimpse-hud-data">'  + details.sql.data.connectionCount + '</span>'; } }
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
                        processEvents = function(details) { 
                            var eventStack = [], 
                                lastEvent = { startPoint : 0, duration : 0, childlessDuration : 0, endPoint : 0 },
                                lastControllerEvent = { },
                                rootDuration = details.request.data.server.duration,
                                rootChildlessDuration = rootDuration;
                            
                            for (var i = 0; i < details.timings.data.length; i += 1) {
                                var event = details.timings.data[i],
                                    topEvent = eventStack.length > 0 ? eventStack[eventStack.length - 1] : null, 
                                    left = (event.startPoint / rootDuration) * 100,  
                                    width = (event.duration / rootDuration) * 100, 
                                    stackParsed = false;

                                event.endPoint = parseFloat((event.startPoint + event.duration).toFixed(2));

                                //Work out how queries are to be parsed
                                if (event.category == "Controller") {
                                    lastControllerEvent = event;
                                    lastControllerEvent.queries = { durationSum: 0, listing: [] };
                                }
                                else if (event.category == "Command") {
                                    lastControllerEvent.queries.listing.push(event);
                                    lastControllerEvent.queries.durationSum += event.duration;
                                    event.suppress = true;
                                }

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
                                    else 
                                        stackParsed = true; 
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
                                event.description = event.title; 
                        
                                lastEvent = event;
                            }
                             
                            details.timings.data.unshift({
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
                                    nesting: 0
                                }); 
                        }, 
                        render = function(details, opened) {
                            var html = '';
                            if (details.mvc || details.sql) {
                                process.init(structure); 
                                processEvents(details);
                                html = rendering.section(structure, details, opened); 
                            }

                            return html;
                        },
                        postRender = function() {
                            $('.glimpse-hud .glimpse-control-trivial').click(function() { $('.glimpse-hud .glimpse-control-trivial, .glimpse-hud .glimpse-data-trivial').toggle(); });
                        };
                    
                    return {
                            render: render,
                            postRender: postRender
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
                                render: function(details) {
                                    var html = '<div class="glimpse-hud-popup-header">Ajax Requests</div>';
                                    html += '<div>' + rendering.item(structure.layout.popup.requests, details) + '</div>';
                                    html += '<div class="glimpse-hud-popup-clear"></div>';
                                    html += '<table style="table-layout:fixed;" class="glimpse-hud-listing glimpse-data-ajax-detail"><thead><tr><th class="glimpse-data-content-method"></th><th></th><th class="glimpse-hud-listing-value glimpse-data-duration">duration (ms)</th><th class="glimpse-hud-listing-value glimpse-data-size">size (kb)</th></tr></thead>';
                                    html += '</table>';

                                    return html;
                                }
                            },
                            defaults: {
                                requests: { title: 'Count', id: 'glimpse-data-ajax-count', description: 'Total Ajax requests detected on this page', visible: true, size: 1, position: 0, align: 0, getData: function(details) { return 0; } }
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
                        render = function(details, opened) {
                            process.init(structure);

                            return rendering.section(structure, details, opened);
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
                            recordItem('<div class="glimpse-hud-listing-row glimpse-hud-value' + rowClass + '"><div class="glimpse-hud-data glimpse-hud-quite glimpse-data-ajax-method">' + method + '</div><div class="glimpse-hud-data glimpse-hud-listing-overflow glimpse-data-ajax-uri" title="' + uri + '">' + uri + '</div><div class="glimpse-data-ajax-duration"><span class="glimpse-hud-data">' + duration + '</span><span class="glimpse-hud-postfix">ms</span></div></div>', '.glimpse-hud-section-ajax .glimpse-data-ajax-summary', summaryStack, 2);
                            recordItem('<tbody class="' + rowClass + '"><tr><td class="glimpse-hud-listing-overflow" title="' + uri + '" colspan="2">' + uri + '</td><td class="glimpse-hud-listing-value glimpse-data-duration">' + duration + '</td><td class="glimpse-hud-listing-value glimpse-data-size">' + (Math.round((size / 1024) * 10) / 10) + '</td></tr><tr><td class="glimpse-hud-quite glimpse-data-content-method">' + method + '</td><td class="glimpse-hud-quite glimpse-hud-listing-overflow">' + status + ' - ' + statusText + '</td><td class="glimpse-hud-quite glimpse-data-content-type glimpse-hud-listing-overflow" title="' + contentType + '">' + processContentType(contentType) + '</td><td class="glimpse-hud-quite glimpse-data-content-time">' + time.toTimeString().replace(/.*(\d{2}:\d{2}:\d{2}).*/, "$1") + '</td></tr></tbody>', '.glimpse-hud-section-ajax .glimpse-data-ajax-detail', detailStack, 6);
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
