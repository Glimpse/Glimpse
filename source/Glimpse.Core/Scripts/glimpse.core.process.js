process = function () {
    var //Support 
        findElements = function () { 
            elements.scope = scope;
            elements.holder = elements.scope.find('.glimpse-holder');
            elements.opener = elements.scope.find('.glimpse-open');
            elements.spacer = elements.scope.find('.glimpse-spacer');  
            elements.tabHolder = elements.scope.find('.glimpse-tabs ul');
            elements.panelHolder = elements.scope.find('.glimpse-panel-holder');
            elements.title = elements.holder.find('.glimpse-title');
            elements.findPanel = function(key) {
                return elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]');
            };

            pubsub.publish('data.elements.processed'); 
        },
        
        //Main
        init = function () { 
            pubsub.subscribe('state.build.shell', findElements); 
        };
    
    init(); 
} ()