(function($, pubsub, util, elements, data, renderEngine) {
    var context = { resultCount : 0, notice: null, isActive: false, contextRequestId: null, isSelected: false },
        generateAjaxAddress = function() {
            var currentMetadata = data.currentMetadata();
            return util.uriTemplate(currentMetadata.resources.glimpse_ajax, { 'parentRequestId': retrieveScopeId(), 'hash': currentMetadata.hash });
        },
        retrieveScopeId = function() { 
            var payload = data.currentData();
                
            return payload.isAjax ? payload.parentRequestId : payload.requestId;
        }, 
        wireListeners = function() {
            var panel = elements.panel('ajax');
            
            elements.optionsHolder().find('.glimpse-clear-ajax').live('click', function() { pubsub.publish('trigger.shell.panel.clear.ajax'); });
            panel.find('tbody a').live('click', function() { pubsub.publish('trigger.data.context.switch', { requestId: $(this).attr('data-requestId'), type: 'ajax' }); }); 
            panel.find('.glimpse-head-message a').live('click', function() { pubsub.publish('trigger.data.context.reset', { type: 'ajax' }); }); 
        }, 
        setup = function(args) {  
            args.newData.data.ajax = { name: 'Ajax', data: 'No requests currently detected...', isPermanent: true };
            args.newData.metadata.plugins.ajax = { documentationUri: 'http://getglimpse.com/Help/Ajax-Tab' };
        },
        activate = function() {
            var options = elements.optionsHolder().html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-ajax">Clear</a></div><div class="glimpse-notice glimpse-disconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            context.notice = util.connectionNotice(options.find('.glimpse-notice')); 
             
            context.isSelected = true;
            
            listenStart();
        }, 
        deactivate = function() {
            elements.optionsHolder().html('');  
            context.notice = null;

            listenStop();
             
            context.isSelected = false;
        }, 
        listenStart = function() {
            if (context.isSelected) { 
                context.isActive = true;
                fetch();
            }
        },
        listenStop = function() {
            if (context.isSelected) {
                context.isActive = false;
            }
        }, 
        contextSwitch = function(args) {
            var newPayload = args.newData,
                oldPayload = args.oldData,
                newId = newPayload.isAjax ? newPayload.parentRequestId : newPayload.requestId,
                oldId = oldPayload.isAjax ? oldPayload.parentRequestId : oldPayload.requestId;

            if (oldId != newId) {
                elements.panel('ajax').find('tbody').empty();
                context.resultCount = 0;
            }
        }, 
        fetch = function() { 
            if (!context.isActive) 
                return;

            //Poll for updated summary data
            context.notice.prePoll(); 
            $.ajax({
                url: generateAjaxAddress(),
                type: 'GET', 
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
            if (context.resultCount == result.length)
                return;
            
            layoutBuildShell();
            layoutBuildContent(result);
        }, 
        layoutBuildShell = function() {
            var panel = elements.panel('ajax'),
                detailPanel = panel.find('table'); 
            
            if (detailPanel.length == 0) {
                var detailData = [['Request URL', 'Method', 'Duration', 'Date/Time', 'View']],
                    detailMetadata = { layout: [ [ { data : 0, key : true, width : '40%' }, { data : 1 }, { data : 2, width : '10%', className : 'mono', align : 'right' },  { data : 3, width : '20%' },  { data : 4, width : '100px' } ] ] };
                
                panel.html(renderEngine.build(detailData, detailMetadata)).find('table').append('<tbody class="glimpse-row-holder"></tbody>');
                panel.find('table').addClass('glimpse-ellipsis').find('thead').append('<tr class="glimpse-head-message" style="display:none"><td colspan="5"><a href="#">Reset context back to starting page</a></td></tr>');
            }
        },
        layoutBuildContent = function(result) {
            var panel = elements.panel('ajax'),
                detailBody = panel.find('tbody'),
                html = '';
            
            for (var x = context.resultCount; x < result.length; x++) {
                var item = result[x];
                html = '<tr data-requestId="' + item.requestId + '" class="glimpse-row"><td><div class="glimpse-ellipsis" title="' + item.uri + '">' + item.uri + '</div></td><td>' + item.method + '</td><td class="mono" style="text-align:right">' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.dateTime + '</td><td><a href="#" class="glimpse-ajax-link" data-requestId="' + item.requestId + '">Inspect</a></td></tr>' + html;
            }
            detailBody.prepend(html);
            
            context.resultCount = result.length; 
            
            if (context.contextRequestId)
                selectStart({ requestId: context.contextRequestId });
        }, 
        layoutClear = function() {
            pubsub.publish('trigger.data.context.reset', { type: 'ajax' });
            elements.panel('ajax').html('<div class="glimpse-panel-message">No requests currently detected...</div>');
        }, 
        selectClear = function(args) {
            var panel = elements.panel('ajax'),
                row = panel.find('.selected'); 
            
            if (row.length > 0) {
                panel.find('.glimpse-head-message').hide();
                row.removeClass('selected');
            
                if (args.type == 'ajax')
                    data.retrieve(data.currentData().parentRequestId);
            }
        },
        selectStart = function(args) {
            var link = elements.panel('ajax').find('.glimpse-ajax-link[data-requestId="' + args.requestId + '"]');
            
            context.contextRequestId = args.requestId;
            
            if (link.length > 0) { 
                if (args.type == 'ajax') {
                    link.hide().parent().append('<div class="loading glimpse-ajax-loading" data-requestId="' + args.requestId + '"><div class="icon"></div>Loading...</div>');
            
                    data.retrieve(args.requestId, 'ajax');
                }
                else 
                    selectCore(args.requestId);
            }
            else 
                selectClear(args);
        },
        selectFinish = function(args) {
            var panel = elements.panel('ajax');
            
            panel.find('.glimpse-ajax-loading[data-requestId="' + args.requestId + '"]').fadeOut(100).delay(100).remove(); 
            panel.find('.glimpse-ajax-link[data-requestId="' + args.requestId + '"]').delay(100).fadeIn(); 
            
            selectCore(args.requestId);
        },
        selectCore = function(requestId) {
            var panel = elements.panel('ajax'),
                row = panel.find('tr[data-requestId="' + requestId + '"]');
            
            panel.find('.glimpse-head-message').show();
            panel.find('.selected').removeClass('selected');
            row.addClass('selected');
            
            context.contextRequestId = null;
        };
    
    var send = XMLHttpRequest.prototype.send;
    XMLHttpRequest.prototype.send = function() { 
        this.setRequestHeader("Glimpse-Parent-RequestID", data.baseData().requestId);
        
        pubsub.publish('trigger.ajax.request.send');
        
        send.apply(this, arguments);
    };
    
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
    pubsub.subscribe('action.panel.hiding.ajax', deactivate); 
    pubsub.subscribe('action.panel.showing.ajax', activate); 
    pubsub.subscribe('action.data.fetched.ajax', selectFinish); 
    pubsub.subscribe('action.data.refresh.changed', contextSwitch); 
    pubsub.subscribe('action.data.initial.changed', setup);
    pubsub.subscribe('trigger.data.context.reset', selectClear);
    pubsub.subscribe('trigger.shell.panel.clear.ajax', layoutClear);
    pubsub.subscribe('trigger.data.context.switch', selectStart);
    pubsub.subscribe('action.shell.opening', listenStart);
    pubsub.subscribe('action.shell.closeing', listenStop); 
    pubsub.subscribe('action.shell.minimizing', listenStop); 
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.elements, glimpse.data, glimpse.render.engine);