var glimpseHistoryPlugin = (function ($, glimpse) {

/*(im port:glimpse.History.spy.js|2)*/ 
    
    var //Support
        isActive = false,  
        notice = undefined, 
        currentData = undefined,
        currentRequestId = undefined,
        wireListener = function () {  
            glimpse.pubsub.subscribe('data.elements.processed', wireDomListeners); 
            glimpse.pubsub.subscribe('action.data.applied', setupData);  
            glimpse.pubsub.subscribe('action.plugin.deactive', function (topic, payload) { if (payload == 'History') { deactive(); } }); 
            glimpse.pubsub.subscribe('action.plugin.active', function (topic, payload) {  if (payload == 'History') { active(); } });  
            glimpse.pubsub.subscribe('action.data.context.reset', function (topic, payload) { reset(payload); });
        },
        wireDomListeners = function () {
            glimpse.elements.holder.find('.glimpse-clear-History').live('click', function () { clear(); return false; });
            
            var panel = glimpse.elements.findPanel('History');
            panel.find('.glimpse-col-main tbody a').live('click', function () { selected($(this)); return false; });
            panel.find('.glimpse-col-side tbody a').live('click', function () { selectedSession($(this).attr('data-clientName')); return false; });
            //panel.find('.glimpse-col-main .glimpse-head-message a').live('click', function() { reset(); return false; });
            panel.find('.glimpse-col-main .glimpse-head-message a').live('click', function() { glimpse.pubsub.publish('action.data.context.reset', 'History'); return false; });
        },
        setupData = function () {
            var payload = glimpse.data.current(),
                metadata = glimpse.data.currentMetadata();
            
            // Only load the tab if we have what we need to support it 
            if (metadata.resources.glimpse_history) {
                payload.data.History = { name: 'History', data: 'No requests currently detected...', isPermanent: true };
                metadata.plugins.History = { documentationUri: 'http://getglimpse.com/Help/Plugin/Remote' };
            }
        },
         
        active = function () {
            isActive = true;
            glimpse.elements.options.html('<div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            notice = new glimpse.util.connectionNotice(glimpse.elements.options.find('.glimpse-notice')); 
            
            retreieveSummary(); 
        },
        deactive = function () {
            isActive = false; 
            glimpse.elements.options.html('');
            notice = null;
        }, 
        
        retreieveSummary = function () { 
            if (!isActive) { return; }

            //Poll for updated summary data
            notice.prePoll(); 
            $.ajax({
                url: glimpse.util.replaceTokens(glimpse.data.currentMetadata().resources.glimpse_history), 
                type: 'GET',
                contentType: 'application/json',
                complete : function(jqXHR, textStatus) {
                    if (!isActive) { return; } 
                    notice.complete(textStatus); 
                    setTimeout(retreieveSummary, 1000);
                },
                success: function (result) {
                    if (!isActive) { return; } 
                    processSummary(result);
                }
            });
        },
        processSummary = function (result) { 
            var panel = glimpse.elements.findPanel('History'),
                didAutoSelect = false;
            
            //Store the current result
            currentData = result;
            
            //Insert container table
            if (panel.find('table').length == 0) 
                renderLayout(panel);
            
            //Prepend results as we go  
            var summary = panel.find('.glimpse-col-side');
            for (var recordName in result) {
                var summaryBody = summary.find('tbody'),
                    summaryRow = summaryBody.find('a[data-clientName="' + recordName + '"]').parents('tr:first'),
                    rowCount = summaryBody.find('tr').length;

                if (summaryRow.length == 0)
                    summaryRow = $('<tr class="' + (rowCount % 2 == 0 ? 'even' : 'odd') + '" data><td>' + recordName + '</td><td class="glimpse-history-count">1</td><td><a href="#" class="glimpse-Client-link" data-clientName="' + recordName + '">Inspect</a></td></tr>').prependTo(summaryBody);
                
                summaryRow.find('.glimpse-history-count').text(result[recordName].length);
                
                if (rowCount == 0) {
                    didAutoSelect = true;
                    selectedSession(recordName);
                }
            }  

            if (!didAutoSelect)
                tryProcessSession(result);
        },
        
        renderLayout = function (panel) {
            panel.html('<div class="glimpse-col-main"></div><div class="glimpse-col-side"></div>');
            
            var main = panel.find('.glimpse-col-main'),
                summary = panel.find('.glimpse-col-side'),
                summaryData = [['Client', 'Count', 'View']],
                mainData = [['Request URL', 'Method', 'Duration', 'Date/Time', 'Is Ajax', 'View']],
                mainMetadata = [[ { data : 0, key : true, width : '30%' }, { data : 1 }, { data : 2, width : '10%' }, { data : 3, width : '20%' }, { data : 4, width : '10%' }, { data : 5, width : '100px' } ]];
                
            main.html(glimpse.render.build(mainData, mainMetadata)).find('table').append('<tbody></tbody>');
            main.find('thead').append('<tr class="glimpse-head-message" style="display:none"><td colspan="6"><a href="#">Reset context back to starting page</a></td></tr>');
                
            summary.html(glimpse.render.build(summaryData)).find('table').append('<tbody></tbody>'); 
        },
        
        
        
        
        selectedSession = function (clientName) {
            var panel = glimpse.elements.findPanel('History'),
                item = panel.find('a[data-clientName="' + clientName + '"]'), 
                clientData = currentData[clientName];
            
            panel.find('.selected').removeClass('selected'); 
            item.parents('tr:first').addClass('selected');
            
            processSession(clientName, clientData);
        },
        processSession = function (clientName, clientData) {
            var panel = glimpse.elements.findPanel('History'),
                mainBody = panel.find('.glimpse-col-main tbody');
            
            if (context.clientName != clientName) {
                context.resultCount = 0;
                mainBody.empty();
            }

            var html = '';
            for (var x = context.resultCount; x < clientData.length; x++) {
                var item = clientData[x];
                html = '<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '" data-requestId="' + item.requestId + '"><td>' + item.url + '</td><td>' + item.method + '</td><td>' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.requestTime + '</td><td>' + item.isAjax + '</td><td><a href="#" class="glimpse-history-link" data-glimpseId="' + item.requestId + '">Inspect</a></td></tr>' + html;
            }
            mainBody.prepend(html);
            
            if (currentRequestId)
                mainBody.find('tr[data-requestId="' + currentRequestId + '"]').addClass('selected');

            context.resultCount = clientData.length;
            context.clientName = clientName;
        },
        tryProcessSession = function (result) {
            var clientData = result[context.clientName];

            if (clientData && context.resultCount != result.length) 
                processSession(context.clientName, clientData); 
        },
        
        
        context = { resultCount : 0, clientName : '', requestId : '' },
        
        
        
        selected = function (item) {
            var requestId = currentRequestId = item.attr('data-glimpseId');

            item.hide().parent().append('<div class="loading glimpse-history-loading" data-glimpseId="' + requestId + '"><div class="icon"></div>Loading...</div>');

            request(requestId);
        },
        request = function (requestId) { 
            glimpse.data.retrieve(requestId, {
                success : function () {
                    process(requestId);
                }
            });
        }, 
        process = function (requestId) {
            var panel = glimpse.elements.findPanel('History'),
                main = panel.find('.glimpse-col-main'), 
                loading = panel.find('.glimpse-history-loading[data-glimpseId="' + requestId + '"]'),
                link = panel.find('.glimpse-history-link[data-glimpseId="' + requestId + '"]');
            
            context.requestId = requestId;

            main.find('.glimpse-head-message').fadeIn();
            main.find('.selected').removeClass('selected'); 
            loading.fadeOut(100).delay(100).remove(); 
            link.delay(100).fadeIn().parents('tr:first').addClass('selected');
        },
        
        reset = function (type) {
            var panel = glimpse.elements.findPanel('History'),
                main = panel.find('.glimpse-col-main');

            currentRequestId = undefined;

            main.find('.glimpse-head-message').fadeOut();
            main.find('.selected').removeClass('selected');
             
            if (type == 'History')
                glimpse.data.reset();
        }, 

        //Main 
        init = function () { 
            wireListener(); 
        };

    init();
}(jQueryGlimpse, glimpse));