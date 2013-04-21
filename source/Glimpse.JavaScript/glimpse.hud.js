(function($, pubsub, data, elements, util) {
    var serverTime = 0,
        modify = function(options) {
            options.templates.css += '/*(import:glimpse.hud.css)*/';
        },
        loaded = function(args) {
            var html = '',
                tabData = args.newData.data.glimpse_hud;

            html += clientTimings.render(tabData.data);
            html += mvcTimings.render(tabData.data);
            html += sqlTimings.render(tabData.data);
            html += ajaxRequests.render(tabData.data);

            elements.opener().prepend('<div class="glimpse-hud">' + html + '</div>');

            adjustForAlerts(elements.opener().find('.glimpse-hud'));
            graph.inject(tabData.data, elements.opener().find('.glimpse-hud')); 
        },
        clientTimings = (function() {
            var timingApi = (window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {}).timing,
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
                        
                        html += '<div class="glimpse-hud-section glimpse-hud-section-request" data-maxValue="1000" data-warnValue="600" data-leftPosition="65">';
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
                        
                        serverTime = timingSum;
                    }

                    return html;
                },
                populate = function() {
                    var result = { }, 
                        network = calculateTimings('navigationStart', 'requestStart'),
                        server = calculateTimings('requestStart', 'requestEnd'),
                        browser = calculateTimings('requestEnd', 'loadEventEnd'),
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
            var render = function(tabData) {
                    var html = '',
                        mvcData = tabData.mvc;

                    if (mvcData) { 
                        var viewIsDifferent = mvcData.actionName != mvcData.viewName;
                        html += '<div class="glimpse-hud-section glimpse-hud-section-mvc" data-maxValue="1500" data-warnValue="600" data-leftPosition="45">';
                        html += '<div class="glimpse-hud-main">';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-detailtitle"><span title="MVC Controller">' + mvcData.controllerName + '</span>.<span title="MVC Action' + (!viewIsDifferent ? ' & View' : '') + '">' + mvcData.actionName + '</span>(...)' + (viewIsDifferent ? ' - <span title="View Name">' + mvcData.viewName + '</span> ' : '') + '</div><div class="glimpse-hud-detailsubtitle">' + mvcData.matchedRouteName + '</div></div>';
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
                        html += '<div class="glimpse-hud-section glimpse-hud-section-sql" data-maxValue="1200" data-warnValue="300" data-leftPosition="40">';
                        html += '<div class="glimpse-hud-main"><div class="glimpse-hud-value">' + limitValue(parseInt((sqlData.connectionOpenTime / serverTime) * 100)) + '</div><div class="glimpse-hud-postfix">%</div></div>';
                        html += '<div class="glimpse-hud-content">';
                        html += '<div class="glimpse-hud-title">SQL</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Total connection open time" data-maxValue="300">' + sqlData.connectionOpenTime + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '<div class="glimpse-hud-detail-divider">/</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Total query time" data-maxValue="20">' + sqlData.queryExecutionTime + '</div><div class="glimpse-hud-postfix">ms</div></div>';
                        html += '<div class="glimpse-hud-detail-divider"></div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of transactions">' + sqlData.transactionCount + '</div></div>';
                        html += '<div class="glimpse-hud-detail-divider">/</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of connections">' + sqlData.connectionCount + '</div></div>';
                        html += '<div class="glimpse-hud-detail-divider">/</div>';
                        html += '<div class="glimpse-hud-detail"><div class="glimpse-hud-value" title="Number of queries" data-maxValue="4">' + sqlData.queryCount + '</div></div>';
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
                    var graphData = util.localStorage('glimpseHudGraph') || { requestTime: [], connectionOpenTime: [], actionExecutionTime: [] };
                 
                    graphData.requestTime.push({ capped: selectValue(tabData.request.requestTime, scope.find('.glimpse-hud-section-request').attr('data-maxValue')), raw: tabData.request.requestTime });
                    checkSize(graphData.requestTime);
                
                    graphData.actionExecutionTime.push({ capped: selectValue(tabData.mvc.actionExecutionTime, scope.find('.glimpse-hud-section-mvc').attr('data-maxValue')), raw: tabData.mvc.actionExecutionTime });
                    checkSize(graphData.actionExecutionTime); 
                
                    graphData.connectionOpenTime.push({ capped: selectValue(tabData.sql.connectionOpenTime, scope.find('.glimpse-hud-section-sql').attr('data-maxValue')), raw: tabData.sql.connectionOpenTime });
                    checkSize(graphData.connectionOpenTime);
                
                    scope.find('.glimpse-hud-section-request .glimpse-hud-content').prepend(build(graphData.requestTime, scope.find('.glimpse-hud-section-request')));
                    scope.find('.glimpse-hud-section-mvc .glimpse-hud-content').prepend(build(graphData.actionExecutionTime, scope.find('.glimpse-hud-section-mvc')));
                    scope.find('.glimpse-hud-section-sql .glimpse-hud-content').prepend(build(graphData.connectionOpenTime, scope.find('.glimpse-hud-section-sql')));

                    util.localStorage('glimpseHudGraph', graphData);
                },
                build = function(graphItem, scope) {
                    var min = graphItem[0].capped,
                        max = graphItem[0].capped,
                        difference = 0,
                        html = '<div class="glimpse-hud-graph" style="left:' + scope.attr('data-leftPosition') + 'px">';
                    
                    for (var i = 0; i < graphItem.length; i++) { 
                        min = Math.min(min, graphItem[i].capped);
                        max = Math.max(max, graphItem[i].capped); 
                    }
                    difference = max - min;
                    
                    for (i = 0; i < graphItem.length; i++) { 
                        var height = (((((graphItem[i].capped - min) / difference) * 100) / 5) * 4) + 20,
                            warnClass = graphItem[i].raw > scope.attr('data-warnValue') ? ' glimpse-hud-graph-item-alert' : '';
                        html += '<div class="glimpse-hud-graph-item' + warnClass + '" style="left:' + (i * 3) + 'px;height:' + height + '%" title="' + graphItem[i].raw + ' ms"></div>';
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

    pubsub.subscribe('action.template.processing', modify); 
    pubsub.subscribe('action.data.initial.changed', function(args) { $(window).load(function() { setTimeout(function() { loaded(args); }, 0); }); }); 

})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements, glimpse.util);
