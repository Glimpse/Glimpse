(function($, pubsub, util, elements, data, renderEngine) {
    var context = { resultCount : 0, notice: undefined, isActive: false, contextRequestId: undefined },
        generateAjaxAddress = function () {
            return util.uriTemplate(data.currentMetadata().resources.glimpse_ajax, { 'parentRequestId': retrieveScopeId() });
        },
        retrieveScopeId = function () { 
            var payload = data.currentData();
                
            return payload.isAjax ? payload.parentId : payload.requestId;
        }, 
        wireListeners = function() {
            var panel = elements.panel('ajax');
            
            elements.optionsHolder().find('.glimpse-clear-ajax').live('click', function() { pubsub.publish('trigger.shell.clear.ajax'); });
            panel.find('tbody a').live('click', function() { pubsub.publish('trigger.data.context.switch', { requestId: $(this).attr('data-requestId'), type: 'ajax' }); }); 
            panel.find('.glimpse-head-message a').live('click', function() { pubsub.publish('trigger.data.context.reset', { type: 'ajax' }); }); 
        }, 
        setup = function(args) {  
            args.newData.data.ajax = { name: 'Ajax', data: 'No requests currently detected...', isPermanent: true };
            args.newData.metadata.plugins.ajax = { documentationUri: 'http://getglimpse.com/Help/Plugin/Ajax' };
        },
        activate = function() {
            context.isActive = true;
            
            var options = elements.optionsHolder().html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-ajax">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            context.notice = util.connectionNotice(options.find('.glimpse-notice')); 
            
            fetch();
        }, 
        deactivate = function() {
            context.isActive = false; 
            
            elements.optionsHolder().html(''); 
            context.notice = null;
        },
        contextSwitch = function(args) {
            var newPayload = args.newData,
                oldPayload = args.oldData,
                newId = newPayload.isAjax ? newPayload.parentId : newPayload.requestId,
                oldId = oldPayload.isAjax ? oldPayload.parentId : oldPayload.requestId;

            if (oldId != newId) {
                elements.panel('ajax').find('tbody').empty();
                context.resultCount = 0;
            }
            if (args.type != 'ajax')
                context.contextRequestId = newPayload.requestId;
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
                success: function (result) {
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
        layoutBuildShell = function () {
            var panel = elements.panel('ajax'),
                detailPanel = panel.find('table'); 
            
            if (detailPanel.length == 0) {
                var detailData = [['Request URL', 'Method', 'Duration', 'Date/Time', 'View']],
                    detailMetadata = [[ { data : 0, key : true, width : '40%' }, { data : 1 }, { data : 2, width : '10%' },  { data : 3, width : '20%' },  { data : 4, width : '100px' } ]];
                
                panel.html(renderEngine.build(detailData, detailMetadata)).find('table').append('<tbody></tbody>');
                panel.find('thead').append('<tr class="glimpse-head-message" style="display:none"><td colspan="6"><a href="#">Reset context back to starting page</a></td></tr>');
            }
        },
        layoutBuildContent = function (result) {
            var panel = elements.panel('ajax'),
                detailBody = panel.find('tbody'),
                html = '';
            
            for (var x = context.resultCount; x < result.length; x++) {
                var item = result[x];
                html = '<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '"><td>' + item.uri + '</td><td>' + item.method + '</td><td>' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.dateTime + '</td><td><a href="#" class="glimpse-ajax-link" data-requestId="' + item.requestId + '">Inspect</a></td></tr>' + html;
            }
            detailBody.prepend(html);
            
            context.resultCount = result.length; 
            
            if (context.contextRequestId)
                selectStart({ requestId: context.contextRequestId, suppressClear: true });
        }, 
        layoutClear = function () {
            elements.panel('ajax').html('<div class="glimpse-panel-message">No requests currently detected...</div>');
        }, 
        selectClear = function (args) {
            var panel = elements.panel('ajax'); 
            panel.find('.glimpse-head-message').hide();
            panel.find('.selected').removeClass('selected');
            
            context.contextRequestId = undefined;
            
            if (args.type == 'ajax')
                data.retrieve(data.currentData().parentId);
        },
        selectStart = function(args) {
            var link = elements.panel('ajax').find('.glimpse-ajax-link[data-requestId="' + args.requestId + '"]');
                
            if (link.length > 0) {
                context.contextRequestId = undefined;
                
                if (args.type == 'ajax') {
                    link.hide().parent().append('<div class="loading glimpse-ajax-loading" data-requestId="' + args.requestId + '"><div class="icon"></div>Loading...</div>');
            
                    data.retrieve(args.requestId, 'ajax');
                }
                else 
                    selectFinish($.extend({ suppressAnimation: true }, args));
            }
            else if (!args.suppressClear)
                selectClear(args);
        },
        selectFinish = function (args) {
            var panel = elements.panel('ajax'),
                loading = panel.find('.glimpse-ajax-loading[data-requestId="' + args.requestId + '"]'),
                link = panel.find('.glimpse-ajax-link[data-requestId="' + args.requestId + '"]');

            panel.find('.glimpse-head-message').show();
            panel.find('.selected').removeClass('selected'); 
            link.closest('tr').addClass('selected');
            if (!args.suppressAnimation) {
                loading.fadeOut(100).delay(100).remove(); 
                link.delay(100).fadeIn();
            }
        };
    
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('action.panel.hiding.ajax', deactivate); 
    pubsub.subscribe('action.panel.showing.ajax', activate); 
    pubsub.subscribe('action.data.featched.ajax', selectFinish); 
    pubsub.subscribe('action.data.refresh.changed', contextSwitch); 
    pubsub.subscribe('action.data.initial.changed', setup);
    pubsub.subscribe('trigger.data.context.reset', selectClear);
    pubsub.subscribe('trigger.shell.clear.ajax', layoutClear);
    pubsub.subscribe('trigger.data.context.switch', selectStart);
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.elements, glimpse.data, glimpse.render.engine);