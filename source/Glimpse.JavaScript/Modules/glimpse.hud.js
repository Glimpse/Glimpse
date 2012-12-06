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
                network = timingApi.connectEnd - timingApi.navigationStart,
                server = timingApi.responseEnd - timingApi.requestStart,
                browser = timingApi.loadEventEnd - timingApi.unloadEventStart,
                total = network + server + browser;

            timings.network = { label: 'Network', categoryColor: '#FD4545', duration: network, percentage: (network / total) * 100 };
            timings.server = { label: 'Server', categoryColor: '#823BBE', duration: server, percentage: (server / total) * 100 };
            timings.browser = { label: 'Browser', categoryColor: '#5087CF', duration: browser, percentage: (browser / total) * 100 };
        };

    pubsub.subscribe('action.shell.loaded', loaded); 

})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements);