(function($, data, elements, pubsub, renderEngine) {
    var render = function (key, pluginData, pluginMetadata) {
            pubsub.publish('action.panel.rendering', { key: key, pluginData: pluginData, pluginMetadata: pluginMetadata.constructor });
            
            var panelHolder = elements.panelHolder(),  
                html = '<div class="glimpse-panel glimpse-panelitem-' + key  + '" data-glimpseKey="' + key + '"><div class="glimpse-panel-message">Loading data, please wait...</div></div>',
                panel = $(html).appendTo(panelHolder);

            if (!pluginData.dontRender)
                renderEngine.insert(panel, pluginData.data, pluginMetadata.structure); 
            
            pubsub.publish('action.panel.rendered', { key: key, pluginData: pluginData, pluginMetadata: pluginMetadata, panelHolder: panelHolder });

            return panel;
        },
        selected = function(key) {
            var panel = elements.panel(key);

            // Only render the content when we need to
            if (panel.length == 0) 
                panel = render(key, data.currentData().data[key], data.currentMetadata().plugins[key]);  

            elements.panelHolder().find('.glimpse-active').removeClass('glimpse-active');
            panel.addClass('glimpse-active');
        };

    pubsub.subscribe('trigger.tab.select', selected);
})(jQueryGlimpse, glimpse.data, glimpse.elements, glimpse.pubsub, glimpse.render.engine);