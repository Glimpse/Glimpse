var glimpseAjaxPlugin = (function ($, glimpse) {

/*(im port:glimpse.ajax.spy.js|2)*/ 
    
    var //Support
        isActive = false, 
        resultCount = 0,
        notice = undefined,
        wireListener = function () {  
            glimpse.pubsub.subscribe('action.plugin.deactive', function (topic, payload) { if (payload == 'Ajax') { deactive(); } }); 
            glimpse.pubsub.subscribe('action.plugin.active', function (topic, payload) {  if (payload == 'Ajax') { active(); } }); 
        },
        alterCurrent = function () {
            var data = glimpse.data.current().data,
                metadata = glimpse.data.currentMetadata().plugins;

            data.Ajax = { name : 'Ajax', data : 'No requests currently detected...', isPermanent : true };
            metadata.Ajax = { helpUrl : 'http://getglimpse.com/Help/Plugin/Ajax' };
        },
        active = function () {
            isActive = true;
            glimpse.elements.options.html('<div class="glimpse-clear"><a href="#">Clear</a></div><div class="glimpse-notice gdisconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            notice = new glimpse.objects.ConnectionNotice(glimpse.elements.options.find('.glimpse-notice')); 
            
            getData(); 
        },
        deactive = function () {
            isActive = false; 
            glimpse.elements.options.html('');
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
                    setTimeout(getData, 1000);
                },
                success: function (result) {
                    if (resultCount != result.length)
                        processData(result);
                    resultCount = result.length; 
                }
            });
        },
        processData = function (result) { 
            var panel = glimpse.elements.findPanel('Ajax'); 
            panel.html(glimpse.render.build(result));
        },

        //Main 
        init = function () {
            wireListener();
            alterCurrent();
        };

    init();
}($Glimpse, glimpse));