(function($, pubsub, util, elements, data, renderEngine) {
    var isActive = true,
        notice = undefined,
        resultCount = 0,
        
        wireListeners = function () {
            elements.holder().find('.glimpse-clear-ajax').live('click', clearLayout);
            
            var panel = elements.panel('ajax');
            panel.find('tbody a').live('click', function () { triggerLayoutSelection($(this)); }); 
            panel.find('.glimpse-head-message a').live('click', function() { pubsub.publish('action.data.context.reset', { type: 'ajax' }); }); //TODO: Need to check how this wires up
        },

        hidingPanel = function () {
            isActive = false; 
            
            elements.optionsHolder().html('');
            
            notice = null;
        },
        showingPanel = function () {
            isActive = true;
            
            var options = elements.optionsHolder().html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-ajax">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            notice = util.connectionNotice(options.find('.glimpse-notice')); 
            
            retreieveSummaryData();
        }, 
        dataUpdate = function(args) {
            var newPayload = args.newData,
                oldPayload = args.oldData,
                newId = newPayload.isAjax ? newPayload.parentId : newPayload.requestId,
                oldId = oldPayload.isAjax ? oldPayload.parentId : oldPayload.requestId,
                panel = elements.panel('ajax');

            if (oldId != newId) {
                panel.find('tbody').empty();
                resultCount = 0;
            }
        },
        setupData = function (input) { 
            input.data.ajax = { name: 'Ajax', data: 'No requests currently detected...', isPermanent: true };
            input.metadata.plugins.ajax = { documentationUri: 'http://getglimpse.com/Help/Plugin/Ajax' };
        },  
        
        retreieveSummaryData = function() { 
            if (!isActive) 
                return;

            var payload = data.currentData(),
                currentId = payload.isAjax ? payload.parentId : payload.requestId;

            //Poll for updated summary data
            notice.prePoll(); 
            $.ajax({
                url: util.uriTemplate(data.currentMetadata().resources.glimpse_ajax, { 'parentRequestId': currentId, 'ajaxResults': true }),
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
                    
                    tryProcessSummaryData(result);
                }
            });
        },
        tryProcessSummaryData = function(result) {
            if (resultCount != result.length) 
                processSummaryData(result); 
        },
        processSummaryData = function(result) {
            var panel = elements.panel('ajax');
            
            //Insert container table
            if (panel.find('table').length == 0) {
                var data = [['Request URL', 'Method', 'Duration', 'Date/Time', 'View']],
                    metadata = [[ { data : 0, key : true, width : '40%' }, { data : 1 }, { data : 2, width : '10%' },  { data : 3, width : '20%' },  { data : 4, width : '100px' } ]];
                panel.html(renderEngine.build(data, metadata)).find('table').append('<tbody></tbody>');
                panel.find('thead').append('<tr class="glimpse-head-message" style="display:none"><td colspan="6"><a href="#">Reset context back to starting page</a></td></tr>');
            }
            
            //Prepend results as we go 
            var panelBody = panel.find('tbody');
            for (var x = resultCount; x < result.length; x++) {
                var item = result[x];
                panelBody.prepend('<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '"><td>' + item.uri + '</td><td>' + item.method + '</td><td>' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.dateTime + '</td><td><a href="#" class="glimpse-ajax-link" data-requestId="' + item.requestId + '">Inspect</a></td></tr>');
            }
            
            resultCount = result.length; 
        },
        
        clearLayout = function () {
            elements.panel('ajax').html('<div class="glimpse-panel-message">No requests currently detected...</div>'); 
        },
        triggerLayoutSelection = function(item) {
            var requestId = item.attr('data-requestId');

            item.hide().parent().append('<div class="loading glimpse-ajax-loading" data-requestId="' + requestId + '"><div class="icon"></div>Loading...</div>');

            request(requestId);
        },
        processLayoutSelection = function (requestId) {
            var panel = elements.panel('ajax'),
                loading = panel.find('.glimpse-ajax-loading[data-requestId="' + requestId + '"]'),
                link = panel.find('.glimpse-ajax-link[data-requestId="' + requestId + '"]');

            panel.find('.glimpse-head-message').fadeIn();
            panel.find('.selected').removeClass('selected'); 
            loading.fadeOut(100).delay(100).remove(); 
            link.delay(100).fadeIn().parents('tr:first').addClass('selected');
        },
        reset = function (args) {
            var panel = elements.panel('ajax');
            
            panel.find('.glimpse-head-message').fadeOut();
            panel.find('.selected').removeClass('selected');
            
            if (args.type == 'ajax')
                data.retrieve(data.currentData().parentId);
        },
            
        request = function (requestId) { 
            data.retrieve(requestId, 'ajax');
        },
        requestCallback = function (args) {
            processLayoutSelection(args.requestId); 
        };
    
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('action.panel.hiding.ajax', hidingPanel); 
    pubsub.subscribe('action.panel.showing.ajax', showingPanel); 
    pubsub.subscribe('action.data.featched.ajax', requestCallback); 
    pubsub.subscribe('action.data.refresh.changed', dataUpdate); 
    pubsub.subscribe('action.data.initial.changed', setupData);
    pubsub.subscribe('action.data.context.reset', reset);
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.elements, glimpse.data, glimpse.render.engine)