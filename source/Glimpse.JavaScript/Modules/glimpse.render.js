(function($, glimpse) {
    var data = glimpse.data,
        elements = glimpse.elements,
        
        renderTabs = function (pluginDataSet) {
            elements.tabHolder.append(constructTabs(pluginDataSet)); 
            util.sortElements(elements.tabHolder, elements.tabHolder.find('li'));
        },

        coreRender = function(isInitial) { 
            var plugins = data.current().data,
                tabHolder = elements.tabHolder();
            
            tabHolder.append(constructTabs(pluginDataSet)); 
            
            util.sortElements(elements.tabHolder, elements.tabHolder.find('li'));
        },
        rerender = function() {
            pubsub.publish('action.shell.rendering');
            pubsub.publish('action.shell.refresh.rendering');
            
            coreRender(false);
            
            pubsub.publish('action.shell.refresh.rendered');
            pubsub.publish('action.shell.rendered');
        },
        render = function() {
            pubsub.publish('action.shell.rendering');
            pubsub.publish('action.shell.initial.rendering');
            
            coreRender(true);
            
            pubsub.publish('action.shell.initial.rendered');
            pubsub.publish('action.shell.rendered');
        };

    glimpse.pubsub.subscribe('trigger.shell.render', render); 
})(jQueryGlimpse, glimpse);