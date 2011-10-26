var glimpseAjaxPlugin = (function ($, glimpse) {

/*(im port:glimpse.ajax.spy.js|2)*/ 
    
    var //Support
        isActive = false, 
        resultCount = 0,
        notice = undefined,
        wireListener = function () {  
            glimpse.pubsub.subscribe('data.elements.processed', wireDomListeners); 
            glimpse.pubsub.subscribe('action.plugin.deactive', function (topic, payload) { if (payload == 'Ajax') { deactive(); } }); 
            glimpse.pubsub.subscribe('action.plugin.active', function (topic, payload) {  if (payload == 'Ajax') { active(); } }); 
        },
        wireDomListeners = function () {
            glimpse.elements.holder.find('.glimpse-clear-ajax').live('click', function () { return false; });
            glimpse.elements.findPanel('Ajax').find('a').live('click', function () { return false; });
        },
        alterCurrent = function () {
            var data = glimpse.data.current().data,
                metadata = glimpse.data.currentMetadata().plugins;

            data.Ajax = { name : 'Ajax', data : 'No requests currently detected...', isPermanent : true };
            metadata.Ajax = { helpUrl : 'http://getglimpse.com/Help/Plugin/Ajax' };
        },
        active = function () {
            isActive = true;
            glimpse.elements.options.html('<div class="glimpse-clear"><a href="#" class="glimpse-clear-ajax">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            notice = new glimpse.objects.ConnectionNotice(glimpse.elements.options.find('.glimpse-notice')); 
            
            getData(); 
        },
        deactive = function () {
            isActive = false; 
            glimpse.elements.options.html('');
            notice = null;
        }, 
        getData = function () { 
            if (!isActive) { return; } 
 
            notice.prePoll(); 
            $.ajax({
                url: glimpsePath + 'Ajax',
                data: { 'glimpseId' : glimpseData.requestId },
                type: 'GET',
                contentType: 'application/json',
                complete : function(jqXHR, textStatus) {
                    notice.complete(textStatus);
                    if (textStatus != "Success")
                        setTimeout(getData, 1000);
                },
                success: function (result) {
                    if (resultCount != result.length)
                        processData(result);
                    resultCount = result.length; 
                    setTimeout(getData, 1000);
                }
            });
        },
        processData = function (result) { 
            var panel = glimpse.elements.findPanel('Ajax'); 
            if (resultCount == 0)
                panel.html(glimpse.render.build([['Request URL', 'Method', 'Duration', 'Date/Time', 'View']])).find('table').append('<tbody></tbody>');
            
            for (var x = result.length; --x >= resultCount;) {
                var item = result[x];
                panel.find('tbody').prepend('<tr class="' + (x % 2 == 0 ? 'even' : 'odd') + '"><td>' + item.url + '</td><td>' + item.method + '</td><td>' + item.duration + '<span class="glimpse-soft"> ms</span></td><td>' + item.requestTime + '</td><td><a href="#">Inspect</a></td></tr>');
            }
        },

        //Main 
        init = function () {
            wireListener();
            alterCurrent();
        };

    init();
}($Glimpse, glimpse));