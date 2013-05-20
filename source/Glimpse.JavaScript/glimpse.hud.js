(function($, pubsub, data, elements, util) {
    var serverTime = 0,
        modify = function(options) {
            options.templates.css += '/*(import:glimpse.hud.css)*/';
        },
        loaded = function(args) {
            var html = '',
                tabData = args.newData.data.glimpse_hud,
                display = displayState.current();
            
            html += clientTimings.render(tabData.data, display[0]);
            html += mvcTimings.render(tabData.data, display[1]);
            html += sqlTimings.render(tabData.data, display[2]);
            html += ajaxRequests.render(tabData.data, display[3]);

            elements.opener().prepend('<div class="glimpse-hud">' + html + '</div>');

            adjustForAlerts(elements.opener().find('.glimpse-hud'), tabData.data);
            graph.setup(tabData.data, elements.opener().find('.glimpse-hud')); 
            displayState.setup();
        },
        displayState = (function() {
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
        clientTimings = (function() {
            var timingApi = (window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {}).timing,
                render = function(tabData, state) {
                    var html = '';

                    tabData.request = {};
                    
                    if (timingApi) {
                        var timingSum = 0,
                            timing = populate();
                        
                        for (var key in timing) {
                            timingSum += timing[key].duration;
                        }
                        tabData.request.requestTime = timingSum;
                        
                        var network = ((timing.network.duration / timingSum) * 100),
                            server = ((timing.server.duration / timingSum) * 100),
                            client = 100 - server - network;

                        html += '<div class="glimpse-hud-section glimpse-hud-section-request" data-maxValue="1000" data-warnValue="600">';
                        html += '<label class="glimpse-hud-title" for="glimpse-hud-section-request-input">Page</label><input type="checkbox" class="glimpse-hud-section-input" id="glimpse-hud-section-request-input"' + (state ? ' checked="checked"' : '') + ' />';
                        html += '<div class="glimpse-hud-section-inner">'; 
                        html += '<div class="glimpse-hud-main"><div class="glimpse-hud-value" data-maxName="hudPageRequestTime" data-maxValue="600" title="Total request time from click to dom ready">' + timingSum + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">Total Request</div></div>';
                        html += '<div class="glimpse-hud-content">';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" data-maxName="hudPageNetworkTime" data-maxValue="15" title="Total time on the network">' + timing.network.duration + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">Network</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" data-maxName="hudPageServerTime" data-maxValue="250" title="Total time on the server">' + timing.server.duration + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">Server</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" data-maxName="hudPageClientTime" data-maxValue="350" title="Total time once client kicks in">' + timing.browser.duration + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">Client</div></div>';
                        html += '<div class="glimpse-hud-bar glimpse-hud-tooltips-non"><div><div class="glimpse-hud-bar-item" style="width: 100%;background-color: #71b1d1;"></div><div class="glimpse-hud-bar-item" style="width: ' + (network + server) + '%;background-color: #AF78DD;"></div><div class="glimpse-hud-bar-item" style="width: ' + network + '%;background-color: #FDBF45;"></div></div></div>';
                        html += '</div>';
                        html += '</div>'; 
                        html += '</div>';
                        
                        serverTime = timing.server.duration;
                    }

                    return html;
                },
                populate = function() {
                    var result = { }, 
                        network = calculateTimings('navigationStart', 'requestStart') + calculateTimings('responseStart', 'responseEnd'),
                        server = calculateTimings('requestStart', 'responseStart'),
                        browser = calculateTimings('responseEnd', 'loadEventEnd'),
                        total = network + server + browser;
                      
                    result.network = { label: 'Network', categoryColor: '#FD4545', duration: network, percentage: (network / total) * 100 };
                    result.server = { label: 'Server', categoryColor: '#823BBE', duration: server, percentage: (server / total) * 100 };
                    result.browser = { label: 'Browser', categoryColor: '#5087CF', duration: browser, percentage: (browser / total) * 100 };

                    return result;
                },
                calculateTimings = function(startIndex, finishIndex) { 
                    return timingApi[finishIndex] - timingApi[startIndex];
                };

            return {
                render: render
            };
        })(),
        mvcTimings = (function() {
            var render = function(tabData, state) {
                    var html = '',
                        mvcData = tabData.mvc;

                    if (mvcData) { 
                        var viewIsDifferent = mvcData.actionName != mvcData.viewName;
                        html += '<div class="glimpse-hud-section glimpse-hud-section-mvc" data-maxValue="1500" data-warnValue="600">';
                        html += '<label class="glimpse-hud-title" for="glimpse-hud-section-mvc-input">MVC</label><input type="checkbox" class="glimpse-hud-section-input" id="glimpse-hud-section-mvc-input"' + (state ? ' checked="checked"' : '') + ' />';
                        html += '<div class="glimpse-hud-section-inner">'; 
                        html += '<div class="glimpse-hud-main">';
                        html += '<div class="glimpse-hud-value" title="How long root Action took to execute" data-maxName="hudMvcActionTime">' + Math.round(mvcData.actionExecutionTime) + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">Action</div>';
                        html += '</div>';
                        html += '<div class="glimpse-hud-content">';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="How long root View took to render" data-maxName="hudMvcViewTime">' + Math.round(mvcData.viewRenderTime) + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">View</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail glimpse-hud-detail-top"><div class="glimpse-hud-detailtitle"><span title="Root Controller">' + mvcData.controllerName + '</span>.<span title="Root Action' + (!viewIsDifferent ? ' & View' : '') + '">' + mvcData.actionName + '</span>(...)' + (viewIsDifferent ? ' - <span title="Root View Name">' + mvcData.viewName + '</span> ' : '') + '</div><div class="glimpse-hud-detailsubtitle" title="Name of matched route">' + mvcData.matchedRouteName + '</div></div>';
                        //html += '<div class="glimpse-hud-detail-divider"></div>';
                        //html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of child actions">' + mvcData.childActionCount + '</div></div>';
                        //html += '<div class="glimpse-hud-detail-divider">/</div>';
                        //html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of child views">' + mvcData.childViewCount + '</div></div>';
                        html += '</div>';
                        html += '</div>';  
                        html += '</div>'; 
                    }

                    return html;
                };

            return {
                render: render
            };
        })(),
        sqlTimings = (function() {
            var render = function(tabData, state) {
                    var html = '',
                        sqlData = tabData.sql;

                    if (sqlData && sqlData.queryExecutionTime && sqlData.connectionOpenTime) { 
                        html += '<div class="glimpse-hud-section glimpse-hud-section-sql" data-maxValue="1200" data-warnValue="300">';
                        html += '<label class="glimpse-hud-title" for="glimpse-hud-section-sql-input">SQL</label><input type="checkbox" class="glimpse-hud-section-input" id="glimpse-hud-section-sql-input"' + (state ? ' checked="checked"' : '') + ' />';
                        html += '<div class="glimpse-hud-section-inner">'; 
                        html += '<div class="glimpse-hud-main"><div class="glimpse-hud-value" data-maxValue="20" data-maxName="hudSqlQueryTime" title="How long all your quires took to execute">' + Math.round(sqlData.queryExecutionTime) + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">Execution</div></div>';
                        html += '<div class="glimpse-hud-content">';  
                        //html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value">' + limitValue(parseInt((sqlData.connectionOpenTime / serverTime) * 100)) + '</div><div class="glimpse-hud-postfix">%</div></div>';
                        //html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" data-maxValue="300" data-maxName="hudSqlConnectionTime" title="How long all your connections where open for">' + Math.round(sqlData.connectionOpenTime) + '</div><div class="glimpse-hud-postfix">ms</div><div class="glimpse-hud-tooltips">Open</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Total number of explicit transactions used" data-maxName="hudSqlTransactionCount">' + sqlData.transactionCount + '</div><div class="glimpse-hud-tooltips">Trx</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Total number of connections used" data-maxName="hudSqlConnectionCount">' + sqlData.connectionCount + '</div><div class="glimpse-hud-tooltips">Conn</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Total number of queries found" data-maxName="hudSqlQueryCount" data-maxValue="4">' + sqlData.queryCount + '</div><div class="glimpse-hud-tooltips">Exec</div></div>';
                        html += '</div>'; 
                        html += '</div>';
                        html += '</div>';
                    }

                    return html;
                },
                limitValue = function(value) {
                    return value > 100 ? 23 : value;
                }; 

            return {
                render: render
            };
        })(),
        ajaxRequests = (function() {
            var open = XMLHttpRequest.prototype.open, 
                count = 0, 
                graphRequestStack = [],
                runningRequestStack = [],
                timeSince = function(ms) {
                    var seconds = ms / 1000,
                        interval = Math.floor(seconds / 60);
                    if (interval >= 1) return [ interval, "m" ];

                    return [ Math.floor(seconds), "s" ];
                },
                render = function(tabData, state) {
                    var html = '<div class="glimpse-hud-section glimpse-hud-section-ajax">';
                        html += '<label class="glimpse-hud-title" for="glimpse-hud-section-for-input">Ajax</label><input type="checkbox" class="glimpse-hud-section-input" id="glimpse-hud-section-for-input"' + (state ? ' checked="checked"' : '') + ' />';
                        html += '<div class="glimpse-hud-section-inner">'; 
                        html += '<div class="glimpse-hud-main"><div class="glimpse-hud-value glimpse-hug-ajax-count" title="Total number of Ajax requests detected">0</div><div class="glimpse-hud-tooltips">Requests</div></div>';
                        html += '</div>'; 
                        html += '</div>';

                    return html;
                },
                update = function(method, url, duration) {
                    //Set the counter
                    $('.glimpse-hug-ajax-count').text(++count).addClass('glimpse-hud-value-update'); 
                    setTimeout(function() {
                        $('.glimpse-hug-ajax-count').removeClass('glimpse-hud-value-update');
                    }, 2000);
                    
                    //Add it when needed
                    if (count == 1) {
                        $('.glimpse-hud-section-ajax .glimpse-hud-section-inner').append('<div class="glimpse-hud-content"></div>');
                    }
                    
                    //Deal with the record details
                    track(method, url, duration);

                    //Update graph
                    graph.generate(graphRequestStack, elements.opener().find('.glimpse-hud .glimpse-hud-section-ajax'), duration, 100);
                },
                track = function(method, url, duration) {
                    //Setup tracker timer
                    var row = $('<div class="glimpse-hud-detail-top"><span class="quiet">' + method + '</span><span class="glimpse-hud-detail-divider"></span>' + url + '<span class="glimpse-hud-detail-divider"></span>' + duration + '<span class="super quiet">ms</span><span class="glimpse-hud-detail-divider"></span><span class="past"></span></div>').prependTo('.glimpse-hud-section-ajax .glimpse-hud-content'),
                        past = row.find('.past'),
                        record = { row: row, canceled: false },
                        next = 0,
                        total = 0, 
                        timer = function() {
                            setTimeout(function() {
                                var since = timeSince(total);
                                past.html(since[0] + '<span class="super quiet">' + since[1] + ' ago</span>');
                                
                                next = total < 10000 ? 1000 : total < 30000 ? 5000 : total < 60000 ? 10000 : 60000;
                                total += next;
                                 
                                if (!record.canceled)
                                    timer();
                            }, next);
                        };
                    timer();
                    
                    setTimeout(function() {
                        row.addClass('added');    
                    }, 1);
                    
                    
                    //Track state of the details
                    if (runningRequestStack.length >= 2) {
                        var item = runningRequestStack.shift();
                        item.canceled = true;
                        item.row.remove();
                    }
                    runningRequestStack.push(record);
                };

            XMLHttpRequest.prototype.open = function(method, url, async, user, pass) {
                var startTime = new Date().getTime();
                
                this.addEventListener("readystatechange", function() {
                        if (this.readyState == 4)  { 
                            update(method, url, new Date().getTime() - startTime);
                        }
                    }, false); 
                
                open.apply(this, arguments);
            }; 

            return {
                render: render
            };
        })(),
        adjustForAlerts = function(scope, tabData) { 
            for (var key in tabData.alert) {
                var item = $(".glimpse-hud-value[data-maxName='" + key + "']"),
                    maxValue = tabData.alert[key];
                
                if (parseInt(item.text()) > parseInt(maxValue)) { 
                    item.addClass('glimpse-hud-value-alert').attr('title', item.attr('title') + ' - Max allowed time \'' + tabData.alert[key] + '\'').attr('data-maxValue', maxValue);

                    item.closest('.glimpse-hud-section').addClass('glimpse-hud-section-alert');
                }
            }
        },
        graph = (function() {
            var setup = function(tabData, scope) {
                    var graphData = util.localStorage('glimpseHudGraph') || { requestTime: [], connectionOpenTime: [], actionExecutionTime: [] };
                 
                    graphData.requestTime.push({ capped: selectValue(tabData.request.requestTime, scope.find('.glimpse-hud-section-request').attr('data-maxValue')), raw: tabData.request.requestTime });
                    checkSize(graphData.requestTime);
                
                    graphData.actionExecutionTime.push({ capped: selectValue(tabData.mvc.actionExecutionTime, scope.find('.glimpse-hud-section-mvc').attr('data-maxValue')), raw: tabData.mvc.actionExecutionTime });
                    checkSize(graphData.actionExecutionTime); 
                
                    graphData.connectionOpenTime.push({ capped: selectValue(tabData.sql.connectionOpenTime, scope.find('.glimpse-hud-section-sql').attr('data-maxValue')), raw: tabData.sql.connectionOpenTime });
                    checkSize(graphData.connectionOpenTime);
                
                    scope.find('.glimpse-hud-section-request .glimpse-hud-main').prepend(build(graphData.requestTime, scope.find('.glimpse-hud-section-request')));
                    scope.find('.glimpse-hud-section-mvc .glimpse-hud-main').prepend(build(graphData.actionExecutionTime, scope.find('.glimpse-hud-section-mvc')));
                    scope.find('.glimpse-hud-section-sql .glimpse-hud-main').prepend(build(graphData.connectionOpenTime, scope.find('.glimpse-hud-section-sql')));

                    util.localStorage('glimpseHudGraph', graphData);
                },
                generate = function(graphItem, scope, value, maxValue) {
                    graphItem.push({ capped: selectValue(value, maxValue), raw: value});
                    checkSize(graphItem); 
                    
                    scope.find('.glimpse-hud-graph').remove();
                    scope.find('.glimpse-hud-main').prepend(build(graphItem, scope));
                },
                build = function(graphItem, scope) {
                    var min = graphItem[0].capped,
                        max = graphItem[0].capped,
                        difference = 0,
                        html = '<div class="glimpse-hud-graph glimpse-hud-tooltips-non">';
                    
                    for (var i = 0; i < graphItem.length; i++) { 
                        min = Math.min(min, graphItem[i].capped);
                        max = Math.max(max, graphItem[i].capped); 
                    }
                    difference = max - min;
                    
                    for (i = 0; i < graphItem.length; i++) { 
                        var height = (((((graphItem[i].capped - min) / difference) * 100) / 5) * 4) + 20,
                            maxValue = scope.attr('data-maxValue'),
                            warnClass = maxValue && graphItem[i].raw > maxValue ? ' glimpse-hud-graph-item-alert' : '';
                        html += '<div class="glimpse-hud-graph-item' + warnClass + '" style="left:' + (i * 3) + 'px;height:' + height + '%" title="' + graphItem[i].raw + ' ms"></div>';
                    }

                    return html + '</div>';
                },
                checkSize = function(graphItem) {
                    if (graphItem.length > 15)
                        graphItem.shift();
                },
                selectValue = function(current, max) {
                    return max ? Math.min(current, max) : current;
                };

            return {
                setup: setup,
                generate: generate
            };
        })();
        
        /*
        //Resource Spike 
        processResourceEntities = function() {
            var html = '';
            
            if (window.performance.getEntriesByType) { 
                var resourceList = window.performance.getEntriesByType("resource");
                for (var i = 0; i < resourceList.length; i++)
                { 
                    console.log("End to end resource fetch: " + resourceList[i].duration); 
                    for (var key in resourceList[i]) {
                        console.log('   ' + key + ' = ' + resourceList[i][key]);
                    }
                } 
            }

            return html;
        }
        */

    pubsub.subscribe('action.template.processing', modify); 
    pubsub.subscribe('action.data.initial.changed', function(args) { $(window).load(function() { setTimeout(function() { loaded(args); }, 0); }); }); 

})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements, glimpse.util);
