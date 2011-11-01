var glimpseAjaxPlugin = (function ($, glimpse) {

/*(im port:glimpse.ajax.spy.js|2)*/ 
    
    var //Support
        isActive = false, 
        resultCount = 0,
        notice = undefined,
        wireListener = function () {  
            glimpse.pubsub.subscribe('data.elements.processed', wireDomListeners); 
            glimpse.pubsub.subscribe('state.build.prerender', setupData); 
            glimpse.pubsub.subscribe('action.plugin.deactive', function (topic, payload) { if (payload == 'Ajax') { deactive(); } }); 
            glimpse.pubsub.subscribe('action.plugin.active', function (topic, payload) {  if (payload == 'Ajax') { active(); } }); 
        },
        wireDomListeners = function () {
            glimpse.elements.holder.find('.glimpse-clear-ajax').live('click', function () { clear(); return false; });
            
            var panel = glimpse.elements.findPanel('Ajax');
            panel.find('tbody a').live('click', function () { selected($(this)); return false; });
            panel.find('.glimpse-head-message a').live('click', function() { reset(); return false; });
        },
        setupData = function () {
            var payload = glimpse.data.current(),
                metadata = glimpse.data.currentMetadata().plugins;

            //If we are looking at an ajax request we want to keep
            if (!payload.isAjax) {
                payload.data.Ajax = { name: 'Ajax', data: 'No requests currently detected...' };
                metadata.Ajax = { helpUrl: 'http://getglimpse.com/Help/Plugin/Ajax' };
                
                togglePermanent(false);
            }
            else 
                togglePermanent(true);
        },
        
        togglePermanent = function (makePermanent) {
            if (glimpse.elements.findPanel) {
                if (makePermanent) {
                    glimpse.elements.findPanel('Ajax').addClass('glimpse-permanent');
                    glimpse.elements.findTab('Ajax').addClass('glimpse-permanent');
                }
                else {
                    glimpse.elements.findPanel('Ajax').removeClass('glimpse-permanent');
                    glimpse.elements.findTab('Ajax').removeClass('glimpse-permanent');
                }
            }
        },
        
        active = function () {
            isActive = true;
            glimpse.elements.options.html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-ajax">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            notice = new glimpse.objects.ConnectionNotice(glimpse.elements.options.find('.glimpse-notice')); 
            
            retreieveSummary(); 
        },
        deactive = function () {
            isActive = false; 
            glimpse.elements.options.html('');
            notice = null;
        }, 
        
        retreieveSummary = function () { 
            if (!isActive) { return; }
            var data = glimpse.data.current();
 
            //Poll for updated summary data
            notice.prePoll(); 
            $.ajax({
                url: glimpsePath + 'Ajax',
                data: { 'glimpseId' : data.requestId },
                type: 'GET',
                contentType: 'application/json',
                complete : function(jqXHR, textStatus) {
                    if (!isActive) { return; } 
                    notice.complete(textStatus); 
                    setTimeout(retreieveSummary, 1000);
                },
                success: function (result) {
                    if (!isActive) { return; } 
                    if (resultCount != result.length)
                        processSummary(result);
                    resultCount = result.length; 
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
            for (var x = result.length; --x >= resultCount;) {
                var item = result[x];
                panel.find('tbody').prepend('<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '"><td>' + item.url + '</td><td>' + item.method + '</td><td>' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.requestTime + '</td><td><a href="#" data-glimpseId="' + item.requestId + '">Inspect</a></td></tr>');
            }
        },
        
        clear = function () {
            glimpse.elements.findPanel('Ajax').html('<div class="glimpse-panel-message">No requests currently detected...</div>'); 
        },
        
        reset = function () {
            var panel = glimpse.elements.findPanel('Ajax');
            panel.find('.glimpse-head-message').fadeOut();
            panel.find('.selected').removeClass('selected');
            
            //TODO: when clearning from ajax should go back to the parent request 
        },
        
        selected = function (item) {
            var requestId = item.attr('data-glimpseId');

            item.hide().parent().append('<div class="loading"><div class="icon"></div>Loading...</div>');

            request(requestId);
        },
        request = function (requestId) { 
//            glimpse.data.retrieve(requestId, {
//                successApplied : function () {
//                    process();
//                }
//            });
        },
        process = function (link) {
            var panel = glimpse.elements.findPanel('Ajax');
            panel.find('.glimpse-head-message').fadeIn();
            panel.find('.selected').removeClass('selected');
            link.parents('tr:first').addClass('selected'); 
        },

        //Main 
        init = function () {
            wireListener();
            setupData();
        };

    init();
}($Glimpse, glimpse));