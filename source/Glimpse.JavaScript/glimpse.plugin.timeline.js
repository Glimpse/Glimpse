var glimpseTimelinePlugin = (function ($, glimpse) {

    /*(import:glimpse.plugin.timeline.core.js|1)*/ 
    
    var //Support  
        currentData = undefined,
        currentTimeline = undefined, 
        wireListener = function () {    
            glimpse.pubsub.subscribe('action.data.applied', contextChanged);
            glimpse.pubsub.subscribe('action.plugin.created', function (topic, payload) { if (payload == 'Timeline') { created(); } });
            glimpse.pubsub.subscribe('action.plugin.active', function (topic, payload) { if (payload == 'Timeline') { resize(); } }); 
            glimpse.pubsub.subscribe('action.resize', resize);
            glimpse.pubsub.subscribe('state.build.template.modify', function(topic, payload) { modify(payload); }); 
        }, 
          
        created = function () { 
            var panel = glimpse.elements.findPanel('Timeline'),
                payload = glimpse.data.current().data;
            
            glimpse.pubsub.publish('state.timeline.build.prerender', currentData);
            
            currentTimeline = glimpseTimeline(panel, currentData);
            currentTimeline.init(); 

            payload.Timeline.data = currentData;
        },
        resize = function () { 
            if (currentTimeline)
                setTimeout(function() { currentTimeline.support.containerResize(glimpse.settings.height - 54); }, 1);
        }, 
        contextChanged = function () {
            var payload = glimpse.data.current().data;

            if (payload.Timeline) {
                currentData = payload.Timeline.data;
                
                glimpse.pubsub.publish('action.timeline.data.updated', currentData);
                
                if (currentData)
                    payload.Timeline.data = 'Generating timeline, please wait...';
            }
        },
        modify = function (template) {
            template.css += '/*(import:glimpse.plugin.timeline.shell.css)*/';
        },

        //Main 
        init = function () {
            wireListener(); 
        };

    init();
}(jQueryGlimpse, glimpse));
