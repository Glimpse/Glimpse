(function($, pubsub, data, elements) {
    var loaded = function() {
            var plugin = data.currentData().data.glimpse_hud, 
                html = '';

            html += processClientTimings(); 
            
            elements.opener().find('tr').prepend('<td class="glimpse-hud">' + html + '</td>');
        },
        processClientTimings = function() {
            var html = '';
            
            if (window.performance.timing) {
                var timingSum = 0,
                    timing = {};
                    
                populateClientTimings(timing);

                html += '<div class="glimpse-hud-section"><span class="glimpse-hud-title">Timings</span><table class="glimpse-hud-graph-bar"><tr>';
                for (var key in timing) {
                    var category = timing[key];
                    
                    timingSum += category.duration; 
                    html += '<td style="background-color:' + category.categoryColor + ';width:' + category.percentage + '%" title="' + category.label + ' timing - ' + category.duration + ' ms"></td>';
                }
                
                html += '</tr></table><span class="glimpse-hud-details"><span title="Total request time">' + timingSum + ' ms</span> ';
                html += '(<span title="Network timing">' + timing.network.duration + ' ms</span> / <span title="Server timing">' + timing.server.duration + ' ms</span> / <span title="Browser timing">' + timing.browser.duration + ' ms</span>)';
                html += '</span></div>';
            }

            return html;
        },
        populateClientTimings = function(timings) {
            var timingApi = window.performance.timing,
                timingOrder = ['navigationStart', 'redirectStart', 'redirectStart', 'redirectEnd', 'fetchStart', 'domainLookupStart', 'domainLookupEnd', 'connectStart', 'secureConnectionStart', 'connectEnd', 'requestStart', 'responseStart', 'responseEnd', 'unloadEventStart', 'unloadEventEnd', 'domLoading', 'domInteractive', 'msFirstPaint', 'domContentLoadedEventStart', 'domContentLoadedEventEnd', 'domContentLoaded', 'domComplete', 'loadEventStart', 'loadEventEnd'],
                start = timingApi.navigationStart || 0,
                network = calculateClientTimings(start, timingApi, timingOrder, 'navigationStart', 'connectEnd'),
                server = calculateClientTimings(start, timingApi, timingOrder, 'requestStart', 'responseEnd'),
                browser = calculateClientTimings(start, timingApi, timingOrder, 'unloadEventStart', 'loadEventEnd'),
                total = network + server + browser;

            timings.network = { label: 'Network', categoryColor: '#FD4545', duration: network, percentage: (network / total) * 100 };
            timings.server = { label: 'Server', categoryColor: '#823BBE', duration: server, percentage: (server / total) * 100 };
            timings.browser = { label: 'Browser', categoryColor: '#5087CF', duration: browser, percentage: (browser / total) * 100 };
        },
        calculateClientTimings = function(start, timingApi, timingOrder, startIndex, finishIndex) {
            var total = 0; 
            for (var i = timingOrder.indexOf(startIndex); i <= timingOrder.indexOf(finishIndex); i++) {
                var value = timingApi[timingOrder[i]];
                if (value && value > total) { 
                    total = (value - start);
                }
            } 
            return total;
        }
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

})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements);