(function($, pubsub, util, settings, elements, data, renderEngine) {
    var context = { resultCount : 0, clientName : '', requestId : '', currentData: undefined, notice: undefined, isActive: false, contextRequestId: undefined }, 
        generateHistoryAddress = function() {
            return util.uriTemplate(data.currentMetadata().resources.glimpse_history);
        },
        wireListeners = function() {
            var panel = elements.panel('history');
            
            elements.holder().find('.glimpse-clear-history').live('click', function() { pubsub.publish('trigger.shell.panel.clear.history'); });            
            panel.find('.glimpse-col-main tbody a').live('click', function() { pubsub.publish('trigger.data.context.switch', { requestId: $(this).attr('data-requestId'), type: 'history' }); }); 
            panel.find('.glimpse-col-side tbody a').live('click', function() { selectSession($(this).attr('data-clientName')); }); 
            panel.find('.glimpse-col-main .glimpse-head-message a').live('click', function() { pubsub.publish('trigger.data.context.reset', { type: 'history' }); });
        },
        setup = function(args) { 
            args.newData.data.history = { name: 'History', data: 'No requests currently detected...', isPermanent: true };
            args.newData.metadata.plugins.history = { documentationUri: 'http://getglimpse.com/Help/Plugin/History' };
        }, 
        activate = function() {
            context.isActive = true;
            
            var options = elements.optionsHolder().html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-history">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            context.notice = util.connectionNotice(options.find('.glimpse-notice')); 
            
            fetch();
        },
        deactivate = function() {
            context.isActive = false; 
            
            elements.optionsHolder().html(''); 
            context.notice = null;
        }, 
        fetch = function() { 
            if (!context.isActive) 
                return; 

            //Poll for updated summary data
            context.notice.prePoll(); 
            $.ajax({
                url: generateHistoryAddress(), 
                type: 'GET',
                cache: false,
                contentType: 'application/json',
                complete : function(jqXHR, textStatus) {
                    if (!context.isActive) 
                        return; 
                    
                    context.notice.complete(textStatus); 
                    setTimeout(fetch, 1000);
                },
                success: function(result) {
                    if (!context.isActive)
                        return; 
                    
                    layoutRender(result);
                }
            });
        },
        layoutRender = function(result) { 
            if ($.isEmptyObject(result))
                return;

            context.currentData = result;
            
            layoutBuildShell(); 
            layoutBuildContentMaster(result); 
            layoutBuildContentDetail(context.clientName, result[context.clientName]);
        },
        layoutBuildShell = function() {
            var panel = elements.panel('history'),
                masterPanel = panel.find('.glimpse-col-side');
            
            if (masterPanel.length == 0) {
                panel.html('<div class="glimpse-col-main"></div><div class="glimpse-col-side"></div>');
            
                masterPanel = panel.find('.glimpse-col-side');
                
                var detailPanel = panel.find('.glimpse-col-main'),
                    masterData = [ [ 'Client', 'Count', 'View' ] ],
                    detailData = [ [ 'Request URL', 'Method', 'Duration', 'Date/Time', 'Is Ajax', 'View' ] ],
                    detailMetadata = { layout: [ [ { data : 0, key : true, width : '30%' }, { data : 1 }, { data : 2, width : '10%', className : 'mono', align : 'right' }, { data : 3, width : '20%' }, { data : 4, width : '10%' }, { data : 5, width : '100px' } ] ] };
                
                detailPanel.html(renderEngine.build(detailData, detailMetadata)).find('table').append('<tbody></tbody>');
                detailPanel.find('thead').append('<tr class="glimpse-head-message" style="display:none"><td colspan="6"><a href="#">Reset context back to starting page</a></td></tr>');
                
                masterPanel.html(renderEngine.build(masterData)).find('table').append('<tbody></tbody>'); 
            }
        }, 
        layoutBuildContentDetail = function(clientName, clientData) {
            var panel = elements.panel('history'),
                detailBody = panel.find('.glimpse-col-main tbody'), 
                html = '';
            
            if (context.clientName != clientName) {
                context.resultCount = 0;
                detailBody.empty();
            }
            
            for (var x = context.resultCount; x < clientData.length; x++) {
                var item = clientData[x];
                html = '<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '" data-requestId="' + item.requestId + '"><td>' + item.uri + '</td><td>' + item.method + '</td><td class="mono" style="text-align:right">' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.dateTime + '</td><td>' + item.isAjax + '</td><td><a href="#" class="glimpse-history-link" data-requestId="' + item.requestId + '">Inspect</a></td></tr>' + html;
            }
            detailBody.prepend(html);
            
            context.resultCount = clientData.length;
            context.clientName = clientName;
            
            if (context.contextRequestId)
                selectStart({ requestId: context.contextRequestId, suppressClear: true });
        }, 
        layoutBuildContentMaster = function(result) {
            var selected = settings.local('historyClient'),
                firstFound = '',
                panel = elements.panel('history'),
                masterBody = panel.find('.glimpse-col-side tbody');
            
            for (var recordName in result) {
                var masterRow = masterBody.find('a[data-clientName="' + recordName + '"]').parents('tr:first'),
                    rowCount = masterBody.find('tr').length;

                if (masterRow.length == 0)
                    masterRow = $('<tr class="' + (rowCount % 2 == 0 ? 'even' : 'odd') + '"><td>' + recordName + '</td><td class="glimpse-history-count">1</td><td><a href="#" class="glimpse-Client-link" data-clientName="' + recordName + '">Inspect</a></td></tr>').prependTo(masterBody);
                
                masterRow.find('.glimpse-history-count').text(result[recordName].length);
                
                if (rowCount == 0) 
                    firstFound = recordName; 
            }
            
            context.clientName = result[selected] ? selected : firstFound;
            selectSession(context.clientName);
        },
        layoutClear = function() {
            pubsub.publish('trigger.data.context.reset', { type: 'history' });
            elements.panel('history').html('<div class="glimpse-panel-message">No requests currently detected...</div>');
        },  
        selectClear = function(args) { 
            var panel = elements.panel('history'),
                detailPanel = panel.find('.glimpse-col-main'), 
                row = detailPanel.find('.selected'); 
            
            if (row.length > 0) {
                panel.find('.glimpse-head-message').hide();
                row.removeClass('selected');
            
                if (args.type == 'history')
                    data.reset();
            }
        },
        selectStart = function(args) { 
            var link = elements.panel('history').find('.glimpse-history-link[data-requestId="' + args.requestId + '"]');
            
            context.contextRequestId = args.requestId;
            
            if (link.length > 0) { 
                if (args.type == 'history') {     
                    link.hide().parent().append('<div class="loading glimpse-history-loading" data-requestId="' + args.requestId + '"><div class="icon"></div>Loading...</div>');
            
                    data.retrieve(args.requestId, 'history');
                }
                else 
                    selectCore(args.requestId);
            }
            else 
                selectClear(args);
        },
        selectFinish = function(args) {
            var panel = elements.panel('history');
            
            panel.find('.glimpse-history-loading[data-requestId="' + args.requestId + '"]').fadeOut(100).delay(100).remove(); 
            panel.find('.glimpse-history-link[data-requestId="' + args.requestId + '"]').delay(100).fadeIn(); 
            
            selectCore(args.requestId);
        },
        selectCore = function(requestId) {
            var panel = elements.panel('history'),
                detailPanel = panel.find('.glimpse-col-main'), 
                row = detailPanel.find('tr[data-requestId="' + requestId + '"]');
            
            detailPanel.find('.glimpse-head-message').show();
            detailPanel.find('.selected').removeClass('selected');
            row.addClass('selected');
            
            context.contextRequestId = undefined; 
        }, 
        selectSession = function(clientName) {
            var panel = elements.panel('history')
                masterPanel = panel.find('.glimpse-col-side'),
                item = masterPanel.find('a[data-clientName="' + clientName + '"]'), 
                clientData = context.currentData[clientName];
            
            settings.local('historyClient', clientName),

            masterPanel.find('.selected').removeClass('selected'); 
            item.parents('tr:first').addClass('selected');
            
            layoutBuildContentDetail(clientName, clientData);
        };

    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
    pubsub.subscribe('action.panel.hiding.history', deactivate); 
    pubsub.subscribe('action.panel.showing.history', activate); 
    pubsub.subscribe('action.data.featched.history', selectFinish); 
    pubsub.subscribe('action.data.initial.changed', setup); 
    pubsub.subscribe('trigger.data.context.reset', selectClear);
    pubsub.subscribe('trigger.shell.panel.clear.history', layoutClear);
    pubsub.subscribe('trigger.data.context.switch', selectStart);
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.settings, glimpse.elements, glimpse.data, glimpse.render.engine);