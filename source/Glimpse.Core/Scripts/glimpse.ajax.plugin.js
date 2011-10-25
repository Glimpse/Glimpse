var glimpseAjaxPlugin = (function ($, glimpse) {

/*(im port:glimpse.ajax.spy.js|2)*/

    var //Support
        isActive = false, 
        resultCount = 0;
        wireListener = function () { 
            //glimpse.pubsub.subscribe('hook.render.engine', function (topic, payload) { registerEngine(payload) }); 

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
            getData(); 
        },
        deactive = function () {
            isActive = false; 
        },

        getData = function () { 
            if (!isActive) { return; }

            var panel = glimpse.elements.findPanel('Ajax'),
                loading = panel.find('.loading');

            if (loading.length == 0) 
                panel.append('<div class="glimpse-clear"><a href="#">Clear</a></div><div class="loading"><div class="icon"></div><span>Refreshing...</span></div>');
            loading.find('span').text('Refreshing...').parent().fadeIn();

            $.ajax({
                url: glimpsePath + 'Ajax',
                data: { 'glimpseId' : glimpseData.requestId },
                type: 'GET',
                contentType: 'application/json',
                complete : function() {
                    loading.find('span').text('Loaded...').parent().delay(500).fadeOut();
                },
                success: function (result) {
                    if (resultCount != result.length)
                        processData(result, panel);
                    resultCount = result.length;
                    
                    setTimeout(getData, 1000);
                }
            });
        },
        processData = function (result, panel) { 
            panel.html(glimpse.render.build(result))
        },

        //Main 
        init = function () {
            wireListener();
            alterCurrent();
        };

    init();
}($Glimpse, glimpse));