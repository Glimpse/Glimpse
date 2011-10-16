metadataController = function () {
    var //Support  
        metadataKey = 'GlimpseMetadata',
        wireListeners = function() {
            pubsub.subscribe('state.build.shell.modify', wireDomListeners);
            pubsub.subscribe('action.metadata', metadata); 
        }, 
        wireDomListeners = function() {
            elements.holder.find('.glimpse-icon').click(function() { pubsub.publish('action.metadata'); });
        }, 
        
        //Main
        renderMetadata = function() {
            var html = '<div class="glimpse-panel glimpse-panelitem-' + metadataKey + '" data-glimpseKey="' + metadataKey + '">' + template.metadata + '</div>';
            return panel = $(html).appendTo(elements.panelHolder);
        }
        metadata = function () {
            var panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + metadataKey + '"]');  
            if (panel.length == 0) {
                panel = renderMetadata();    
                pubsub.publish('action.plugin.created', metadataKey); 
            }
            
            //Switch style states
            elements.panelHolder.find('.glimpse-active').removeClass('glimpse-active'); 
            panel.addClass('glimpse-active');
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()