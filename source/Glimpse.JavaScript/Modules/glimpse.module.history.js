(function($, pubsub, util, elements, data, renderEngine) {
    var isActive = true,
        notice = undefined,
        context = { resultCount : 0, clientName : '', requestId : '' },
        currentData = undefined,
        
        wireListeners = function () {
            elements.holder().find('.glimpse-clear-history').live('click', clearLayout);
            
            var panel = elements.panel('history');
            panel.find('.glimpse-col-main tbody a').live('click', function () { triggerLayoutSelection($(this)); }); 
            panel.find('.glimpse-col-side tbody a').live('click', function () { triggerLayoutSessionSelection($(this).attr('data-clientName')); }); 
            panel.find('.glimpse-col-main .glimpse-head-message a').live('click', function() { pubsub.publish('action.data.context.reset', { type: 'history' }); });
        },
        setupData = function (input) { 
            input.data.history = { name: 'History', data: 'No requests currently detected...', isPermanent: true };
            input.metadata.plugins.history = { documentationUri: 'http://getglimpse.com/Help/Plugin/History' };
        },  

        hidingPanel = function () {
            isActive = false; 
            
            elements.optionsHolder().html('');
            
            notice = null;
        },
        showingPanel = function () {
            isActive = true;
            
            var options = elements.optionsHolder().html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-history">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            notice = util.connectionNotice(options.find('.glimpse-notice')); 
            
            retreieveSummaryData();
        },
    
        renderLayout = function (panel) {
            panel.html('<div class="glimpse-col-main"><div class="glimpse-panel-message">No requests currently detected...</div></div><div class="glimpse-col-side"></div>');
            
            var main = panel.find('.glimpse-col-main'),
                summary = panel.find('.glimpse-col-side'),
                summaryData = [['Client', 'Count', 'View']],
                mainData = [['Request URL', 'Method', 'Duration', 'Date/Time', 'Is Ajax', 'View']],
                mainMetadata = [[ { data : 0, key : true, width : '30%' }, { data : 1 }, { data : 2, width : '10%' }, { data : 3, width : '20%' }, { data : 4, width : '10%' }, { data : 5, width : '100px' } ]];
                
            main.html(renderEngine.build(mainData, mainMetadata)).find('table').append('<tbody></tbody>');
            main.find('thead').append('<tr class="glimpse-head-message" style="display:none"><td colspan="6"><a href="#">Reset context back to starting page</a></td></tr>');
                
            summary.html(renderEngine.build(summaryData)).find('table').append('<tbody></tbody>'); 
        },
        
        triggerLayoutSessionSelection = function (clientName) {
            var panel = elements.panel('history'),
                item = panel.find('a[data-clientName="' + clientName + '"]'), 
                clientData = currentData[clientName];
            
            panel.find('.selected').removeClass('selected'); 
            item.parents('tr:first').addClass('selected');
            
            processSessionData(clientName, clientData);
        }, 
        triggerLayoutSelection = function (item) {
            var requestId = item.attr('data-requestId');

            item.hide().parent().append('<div class="loading glimpse-history-loading" data-requestId="' + requestId + '"><div class="icon"></div>Loading...</div>');

            request(requestId);
        },

        retreieveSummaryData = function () { 
            if (!isActive) 
                return; 

            //Poll for updated summary data
            notice.prePoll(); 
            $.ajax({
                url: util.uriTemplate(data.currentMetadata().resources.glimpse_history), 
                type: 'GET',
                contentType: 'application/json',
                complete : function(jqXHR, textStatus) {
                    if (!isActive) 
                        return; 
                    
                    notice.complete(textStatus); 
                    setTimeout(retreieveSummaryData, 1000);
                },
                success: function (result) {
                    if (!isActive)
                        return; 
                    
                    processSessionSummaryData(result);
                }
            });
        },
        processSessionSummaryData = function (result) { 
            var panel = elements.panel('history'),
                summary = panel.find('.glimpse-col-side'),
                didAutoSelect = false;
            
            //Store the current result
            currentData = result;
            
            //Insert container table
            if (summary.length == 0) 
                renderLayout(panel);
            
            //Prepend results as we go
            for (var recordName in result) {
                var summaryBody = summary.find('tbody'),
                    summaryRow = summaryBody.find('a[data-clientName="' + recordName + '"]').parents('tr:first'),
                    rowCount = summaryBody.find('tr').length;

                if (summaryRow.length == 0)
                    summaryRow = $('<tr class="' + (rowCount % 2 == 0 ? 'even' : 'odd') + '" data><td>' + recordName + '</td><td class="glimpse-history-count">1</td><td><a href="#" class="glimpse-Client-link" data-clientName="' + recordName + '">Inspect</a></td></tr>').prependTo(summaryBody);
                
                summaryRow.find('.glimpse-history-count').text(result[recordName].length);
                
                if (rowCount == 0) {
                    didAutoSelect = true;
                    triggerLayoutSessionSelection(recordName);
                }
            }  

            if (!didAutoSelect)
                tryProcessSessionData(result);
        },
        tryProcessSessionData = function (result) {
            var clientData = result[context.clientName];

            if (clientData && context.resultCount != result.length) 
                processSessionData(context.clientName, clientData); 
        },
        processSessionData = function (clientName, clientData) {
            var panel = elements.panel('history'),
                mainBody = panel.find('.glimpse-col-main tbody'),
                currentRequestId = data.currentData().requestId;
            
            if (context.clientName != clientName) {
                context.resultCount = 0;
                mainBody.empty();
            }

            var html = '';
            for (var x = context.resultCount; x < clientData.length; x++) {
                var item = clientData[x];
                html = '<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '" data-requestId="' + item.requestId + '"><td>' + item.uri + '</td><td>' + item.method + '</td><td>' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.dateTime + '</td><td>' + item.isAjax + '</td><td><a href="#" class="glimpse-history-link" data-requestId="' + item.requestId + '">Inspect</a></td></tr>' + html;
            }
            mainBody.prepend(html);
            
            if (currentRequestId)
                mainBody.find('tr[data-requestId="' + currentRequestId + '"]').addClass('selected');

            context.resultCount = clientData.length;
            context.clientName = clientName;
        },
        
        processLayoutSelection = function (requestId) {
            var panel = elements.panel('history'),
                main = panel.find('.glimpse-col-main'), 
                loading = panel.find('.glimpse-history-loading[data-requestId="' + requestId + '"]'),
                link = panel.find('.glimpse-history-link[data-requestId="' + requestId + '"]');
            
            main.find('.glimpse-head-message').fadeIn();
            main.find('.selected').removeClass('selected'); 
            loading.fadeOut(100).delay(100).remove(); 
            link.delay(100).fadeIn().parents('tr:first').addClass('selected');;
        },
        reset = function (args) {
            var panel = elements.panel('history');
            
            panel.find('.glimpse-head-message').fadeOut();
            panel.find('.selected').removeClass('selected');
            
            if (args.type == 'history')
                data.reset();
        },
            
        clearLayout = function () {
            elements.panel('history').html('<div class="glimpse-panel-message">No requests currently detected...</div>'); 
        },  
            
        request = function (requestId) { 
            data.retrieve(requestId, 'history');
        },
        requestCallback = function (args) {
            processLayoutSelection(args.requestId); 
        } 
    ;

    
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('action.data.featched.history', requestCallback); 
    pubsub.subscribe('action.panel.hiding.history', hidingPanel); 
    pubsub.subscribe('action.panel.showing.history', showingPanel); 
    pubsub.subscribe('action.data.initial.changed', setupData);
    pubsub.subscribe('action.data.context.reset', reset);
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.elements, glimpse.data, glimpse.render.engine);