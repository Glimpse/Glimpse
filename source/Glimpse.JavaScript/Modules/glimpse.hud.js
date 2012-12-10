(function($, pubsub, data, elements, util) {
    var loaded = function() {
            var html = '',
                tabData = data.currentData().data.glimpse_hud;

            html += clientTimings.render(tabData.data);
            html += mvcTimings.render(tabData.data);
            html += sqlTimings.render(tabData.data);
            html += ajaxRequests.render(tabData.data);

            elements.opener().prepend('<div class="glimpse-hud">' + html + '</div>');

            adjustForAlerts(elements.opener().find('.glimpse-hud'));
            graph.inject(tabData.data, elements.opener().find('.glimpse-hud')); 
        },
        clientTimings = (function() {
            var timingApi = window.performance.timing,
                render = function(tabData) {
                    var html = '';

                    tabData.request = {};
                    
                    if (timingApi) {
                        var timingSum = 0,
                            timing = populate();
                        
                        for (var key in timing) {
                            timingSum += timing[key].duration;
                        }
                        tabData.request.requestTime = timingSum;
                        
                        html += '<div class="glimpse-hud-section glimpse-hud-section-request" data-maxValue="1000">';
                        html += '<div class="glimpse-hud-main"><div class="glimpse-hud-value" title="Total request time" data-maxValue="600">' + timingSum + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '<div class="glimpse-hud-content">';
                        html += '<div class="glimpse-hud-title">Request</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Network timing" data-maxValue="15">' + timing.network.duration + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Server timing" data-maxValue="250">' + timing.server.duration + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Browser timing" data-maxValue="350">' + timing.browser.duration + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '</div>'; 
                        html += '</div>';
                    }

                    return html;
                },
                populate = function() {
                    var result = { },
                        timingOrder = ['navigationStart', 'redirectStart', 'redirectStart', 'redirectEnd', 'fetchStart', 'domainLookupStart', 'domainLookupEnd', 'connectStart', 'secureConnectionStart', 'connectEnd', 'requestStart', 'responseStart', 'responseEnd', 'unloadEventStart', 'unloadEventEnd', 'domLoading', 'domInteractive', 'msFirstPaint', 'domContentLoadedEventStart', 'domContentLoadedEventEnd', 'domContentLoaded', 'domComplete', 'loadEventStart', 'loadEventEnd'],
                        start = timingApi.navigationStart || 0,
                        network = safeCheck(calculateTimings(start, timingOrder, 'navigationStart', 'connectEnd')),
                        server = safeCheck(calculateTimings(start, timingOrder, 'requestStart', 'responseEnd')),
                        browser = safeCheck(calculateTimings(start, timingOrder, 'unloadEventStart', 'loadEventEnd')),
                        total = network + server + browser;

                    result.network = { label: 'Network', categoryColor: '#FD4545', duration: network, percentage: (network / total) * 100 };
                    result.server = { label: 'Server', categoryColor: '#823BBE', duration: server, percentage: (server / total) * 100 };
                    result.browser = { label: 'Browser', categoryColor: '#5087CF', duration: browser, percentage: (browser / total) * 100 };

                    return result;
                },
                calculateTimings = function(start, timingOrder, startIndex, finishIndex) {
                    var total = 0;
                    for (var i = timingOrder.indexOf(startIndex); i <= timingOrder.indexOf(finishIndex); i++) {
                        var value = timingApi[timingOrder[i]];
                        if (value && value > total) {
                            total = (value - start);
                        }
                    }
                    return total;
                },
                safeCheck = function(value) {
                    return value > 10000000 ? 24 : value;
                };

            return {
                render: render
            };
        })(),
        mvcTimings = (function() {
            var render = function(tabData) {
                    var html = '',
                        mvcData = tabData.mvc;

                    if (mvcData) { 
                        var viewIsDifferent = mvcData.actionName != mvcData.viewName;
                        html += '<div class="glimpse-hud-section glimpse-hud-section-mvc" data-maxValue="1500">';
                        html += '<div class="glimpse-hud-main">';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-detailtitle"><span title="MVC Controller">' + mvcData.controllerName + '</span>.<span title="MVC Action' + (!viewIsDifferent ? ' & View' : '') + '">' + mvcData.actionName + '</span>(...)' + (viewIsDifferent ? ' - <span title="View Name">' + mvcData.viewName + '</span> ' : '') + '</div><div class="glimpse-hud-detailsubtitle">' + mvcData.routeName + '</div></div>';
                        html += '<div class="glimpse-hud-value" title="Action execution time">' + mvcData.actionExecutionTime + '</div><div class="glimpse-hud-postfix">ms</div>';
                        html += '</div>';
                        html += '<div class="glimpse-hud-content">';
                        html += '<div class="glimpse-hud-title">MVC</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="View rendering time">' + mvcData.viewRenderTime + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of child actions">' + mvcData.childActionCount + '</div></div>';
                        html += '<div class="glimpse-hud-detail-divider">/</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of child views">' + mvcData.childViewCount + '</div></div>';
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
            var render = function(tabData) {
                    var html = '',
                        sqlData = tabData.sql;

                    if (sqlData) { 
                        html += '<div class="glimpse-hud-section glimpse-hud-section-sql" data-maxValue="200">';
                        html += '<div class="glimpse-hud-main"><div class="glimpse-hud-value" title="Total query time" data-maxValue="20">' + sqlData.queryExecutionTime + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '<div class="glimpse-hud-content">';
                        html += '<div class="glimpse-hud-title">SQL</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of transactions">' + sqlData.transactionCount + '</div></div>';
                        html += '<div class="glimpse-hud-detail-divider">/</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of connections">' + sqlData.connectionCount + '</div></div>';
                        html += '<div class="glimpse-hud-detail-divider">/</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of queries" data-maxValue="4">' + sqlData.queryCount + '</div></div>';
                        html += '</div>'; 
                        html += '</div>';
                    }

                    return html;
                };

            return {
                render: render
            };
        })(),
        ajaxRequests = (function() {
            var send = XMLHttpRequest.prototype.send,
                count = 0,
                render = function() {
                    var html = '<div class="glimpse-hud-section">';
                        html += '<div class="glimpse-hud-main"><div class="glimpse-hud-value glimpse-hug-ajax-count" title="Number of Ajax request">0</div></div>';
                        html += '<div class="glimpse-hud-content">';
                        html += '<div class="glimpse-hud-title">Ajax</div>';
                        html += '</div>'; 
                        html += '</div>';

                    return html;
                },
                update = function() {
                    $('.glimpse-hug-ajax-count').text(++count).addClass('glimpse-hud-value-update');
                    setTimeout(function() {
                        $('.glimpse-hug-ajax-count').removeClass('glimpse-hud-value-update');
                    }, 2000);
                };
             
            XMLHttpRequest.prototype.send = function() { 
                update();
                send.apply(this, arguments);
            };

            return {
                render: render
            };
        })(),
        adjustForAlerts = function(scope) {
            var items = scope.find('[data-maxValue]'),
                alert = false,
                alertDetails = '';
            items.each(function() {
                var item = $(this);
                if (parseInt(item.text()) > parseInt(item.attr('data-maxValue'))) {
                    alert = true;
                    item.addClass('glimpse-hud-value-alert').attr('title', item.attr('title') + ' - Max allowed time \'' + item.attr('data-maxValue') + '\'');
                    alertDetails += item.attr('title') + '\n';
                }
            });
            
            if (alert)
                scope.append('<div class="alert" title="' + alertDetails + '"><div class="icon"></div></div>');
        },
        graph = (function() {
            var inject = function(tabData, scope) {
                    var graphData = util.localStorage('glimpseHudGraph') || { requestTime: [], queryExecutionTime: [], actionExecutionTime: [] };
                 
                    graphData.requestTime.push(selectValue(tabData.request.requestTime, scope.find('.glimpse-hud-section-request').attr('data-maxValue')));
                    checkSize(graphData.requestTime);
                
                    graphData.queryExecutionTime.push(selectValue(tabData.sql.queryExecutionTime, scope.find('.glimpse-hud-section-sql').attr('data-maxValue')));
                    checkSize(graphData.queryExecutionTime);
                
                    graphData.actionExecutionTime.push(selectValue(tabData.mvc.actionExecutionTime, scope.find('.glimpse-hud-section-mvc').attr('data-maxValue')));
                    checkSize(graphData.actionExecutionTime); 
                
                    scope.find('.glimpse-hud-section-request .glimpse-hud-content').prepend(build(graphData.requestTime));
                    scope.find('.glimpse-hud-section-mvc .glimpse-hud-content').prepend(build(graphData.actionExecutionTime));
                    scope.find('.glimpse-hud-section-sql .glimpse-hud-content').prepend(build(graphData.queryExecutionTime));

                    util.localStorage('glimpseHudGraph', graphData);
                },
                build = function(graphItem, maxValue) {
                    var min = graphItem[0],
                        max = graphItem[0],
                        difference = 0,
                        html = '<div class="glimpse-hud-graph">';
                    
                    for (var i = 0; i < graphItem.length; i++) { 
                        min = Math.min(min, graphItem[i]);
                        max = Math.max(max, graphItem[i]); 
                    }
                    difference = max - min;
                    
                    for (i = 0; i < graphItem.length; i++) { 
                        var height = (((((graphItem[i] - min) / difference) * 100) / 5) * 4) + 20;
                        html += '<div class="glimpse-hud-graph-item" style="left:' + (i * 2) + 'px;height:' + height + '%"></div>';
                    }

                    return html + '</div>';
                },
                checkSize = function(graphItem) {
                    if (graphItem.length > 15)
                        graphItem.shift();
                },
                selectValue = function(current, max) {
                    return Math.min(current, max);
                };

            return {
                inject: inject
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
        ;

    pubsub.subscribe('action.shell.loaded', loaded); 

})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements, glimpse.util);
