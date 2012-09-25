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
        selected = function(options) {
            var panel = elements.panel(options.key);

            // Only render the content when we need to
            if (panel.length == 0) 
                panel = render(options.key, data.currentData().data[options.key], data.currentMetadata().plugins[options.key]);  

            elements.panelHolder().find('.glimpse-active').removeClass('glimpse-active');
            panel.addClass('glimpse-active');
        },
        clear = function() {
            elements.panelHolder().empty();
        };

    pubsub.subscribe('trigger.tab.select', selected);
    pubsub.subscribe('trigger.shell.clear', clear);
})(jQueryGlimpse, glimpse.data, glimpse.elements, glimpse.pubsub, glimpse.render.engine);