metadataController = function () {
    var //Support  
        metadataKey = 'GlimpseMetadata',
        wireListeners = function() {
            pubsub.subscribe('state.build.shell.modify', wireDomListeners);
            pubsub.subscribe('action.plugin.active', function(topic, payload) { activateHelp(payload); });
            pubsub.subscribe('action.metadata', metadata); 
        }, 
        wireDomListeners = function() {
            elements.holder.find('.glimpse-icon').click(function() { pubsub.publish('action.metadata'); });
        }, 
        
        //Main
        renderMetadata = function() {
            var html = '<div class="glimpse-panel glimpse-panelitem-' + metadataKey + '" data-glimpseKey="' + metadataKey + '">' + template.metadata + '</div>';
            return $(html).appendTo(elements.panelHolder);
        },
        metadata = function () {
            var panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + metadataKey + '"]');  
            if (panel.length == 0) {
                panel = renderMetadata();    
                pubsub.publish('action.plugin.created', metadataKey); 
            }
            
            //Switch style states
            elements.panelHolder.find('.glimpse-active').removeClass('glimpse-active'); 
            panel.addClass('glimpse-active');
            elements.tabHolder.find('.glimpse-active').removeClass('glimpse-active'); 
            
            pubsub.publish('action.plugin.active', metadataKey); 
        }, 
        activateHelp = function (key) { 
            var metaData = data.currentMetadata().plugins[key], 
                url = metaData && metaData.documentationUri;

            elements.holder.find('.glimpse-meta-help').toggle(url != undefined).attr('href', url); 
        },
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()