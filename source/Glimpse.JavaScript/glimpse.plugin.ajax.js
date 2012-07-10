var glimpseAjaxPlugin = (function ($, glimpse) {

/*(im port:glimpse.plugin.ajax.spy.js|2)*/ 
    
    var //Support
        isActive = false, 
        resultCount = 0,
        notice = undefined, 
        currentId = undefined,
        wireListener = function () {  
            glimpse.pubsub.subscribe('data.elements.processed', wireDomListeners); 
            glimpse.pubsub.subscribe('action.data.applied', setupData);
            glimpse.pubsub.subscribe('action.data.applied', contextChanged);
            glimpse.pubsub.subscribe('action.plugin.deactive', function (topic, payload) { if (payload == 'Ajax') { deactive(); } }); 
            glimpse.pubsub.subscribe('action.plugin.active', function (topic, payload) {  if (payload == 'Ajax') { active(); } }); 
            glimpse.pubsub.subscribe('action.data.context.reset', function (topic, payload) { reset(payload); });
        },
        wireDomListeners = function () {
            glimpse.elements.holder.find('.glimpse-clear-ajax').live('click', function () { clear(); return false; });
            
            var panel = glimpse.elements.findPanel('Ajax');
            panel.find('tbody a').live('click', function () { selected($(this)); return false; });
            //panel.find('.glimpse-head-message a').live('click', function() { reset(); return false; });
            panel.find('.glimpse-head-message a').live('click', function() { glimpse.pubsub.publish('action.data.context.reset', 'Ajax'); return false; });
        },
        
        setupData = function () {
            var payload = glimpse.data.current(),
                metadata = glimpse.data.currentMetadata().plugins;
                 
            payload.data.Ajax = { name: 'Ajax', data: 'No requests currently detected...', isPermanent : true };
            metadata.Ajax = { documentationUri: 'http://getglimpse.com/Help/Plugin/Ajax' }; 
        }, 
        
        active = function () {
            isActive = true;
            glimpse.elements.options.html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-ajax">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            notice = new glimpse.util.connectionNotice(glimpse.elements.options.find('.glimpse-notice')); 
            
            retreieveSummary(); 
        },
        deactive = function () {
            isActive = false; 
            glimpse.elements.options.html('');
            notice = null;
        }, 
        contextChanged = function () {
            var payload = glimpse.data.current(),
                newId = payload.isAjax ? payload.parentId : payload.requestId,
                panel = glimpse.elements.findPanel('Ajax');

            if (currentId != newId)
                panel.find('tbody').empty();
            
            currentId = newId;
        },
        
        retreieveSummary = function () { 
            if (!isActive) { return; }

            //Poll for updated summary data
            notice.prePoll(); 
            $.ajax({
                url: glimpse.data.currentMetadata().paths.ajax,
                data: { 'requestID': currentId },
                type: 'GET',
                contentType: 'application/json',
                complete : function(jqXHR, textStatus) {
                    if (!isActive) { return; } 
                    notice.complete(textStatus); 
                    setTimeout(retreieveSummary, 1000);
                },
                success: function (result) {
                    if (!isActive) { return; } 
                    tryProcessSummary(result);
                }
            });
        },
        processSummary = function (result) { 
            var panel = glimpse.elements.findPanel('Ajax');
            
            //Insert container table
            if (panel.find('table').length == 0) {
                var data = [['Request URL', 'Method', 'Duration', 'Date/Time', 'View']],
                    metadata = [[ { data : 0, key : true, width : '40%' }, { data : 1 }, { data : 2, width : '10%' },  { data : 3, width : '20%' },  { data : 4, width : '100px' } ]];
                panel.html(glimpse.render.build(data, metadata)).find('table').append('<tbody></tbody>');
                panel.find('thead').append('<tr class="glimpse-head-message" style="display:none"><td colspan="6"><a href="#">Reset context back to starting page</a></td></tr>');
            }
            
            //Prepend results as we go 
            var panelBody = panel.find('tbody');
            for (var x = result.length; --x >= resultCount;) {
                var item = result[x];
                panelBody.prepend('<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '"><td>' + item.url + '</td><td>' + item.method + '</td><td>' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.requestTime + '</td><td><a href="#" class="glimpse-ajax-link" data-glimpseId="' + item.requestId + '">Inspect</a></td></tr>');
            }
            
            resultCount = result.length; 
        }, 
        tryProcessSummary = function (result) {
            if (resultCount != result.length)
                processSummary(result);
        },
        
        clear = function () {
            glimpse.elements.findPanel('Ajax').html('<div class="glimpse-panel-message">No requests currently detected...</div>'); 
        },
        
        reset = function (type) {
            var panel = glimpse.elements.findPanel('Ajax');
            panel.find('.glimpse-head-message').fadeOut();
            panel.find('.selected').removeClass('selected');
             
            if (type == 'Ajax')
                glimpse.data.retrieve(currentId);
        },
        
        selected = function (item) {
            var requestId = item.attr('data-glimpseId');

            item.hide().parent().append('<div class="loading glimpse-ajax-loading" data-glimpseId="' + requestId + '"><div class="icon"></div>Loading...</div>');

            request(requestId);
        },
        request = function (requestId) { 
            glimpse.data.retrieve(requestId, {
                success : function (requestId) { process(requestId); }
            });
        },
        process = function (requestId) {
            var panel = glimpse.elements.findPanel('Ajax'),
                loading = panel.find('.glimpse-ajax-loading[data-glimpseId="' + requestId + '"]'),
                link = panel.find('.glimpse-ajax-link[data-glimpseId="' + requestId + '"]');

            panel.find('.glimpse-head-message').fadeIn();
            panel.find('.selected').removeClass('selected'); 
            loading.fadeOut(100).delay(100).remove(); 
            link.delay(100).fadeIn().parents('tr:first').addClass('selected');
        },

        //Main 
        init = function () {
            wireListener(); 
        };

    init();
}($Glimpse, glimpse));