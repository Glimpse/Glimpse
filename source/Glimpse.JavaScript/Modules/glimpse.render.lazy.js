(function($, glimpse) {
    var data = glimpse.data,
        util = glimpse.util,
        elements = glimpse.elements,
        pubsub = glimpse.pubsub,
        renderEngine = glimpse.render.engine,
        generateResourceUri = function (currentData, key) {
            var resources = data.currentMetadata().resources;
            
            return util.replaceTokens(resources.glimpse_tab, { 'requestId': currentData.requestId, 'pluginKey': key });
        },
        retrieveData = function(key) {
            var resources = data.currentMetadata().resources;
            
            if (resources.glimpse_tab) {
                pubsub.publish('action.tab.lazy.fetching', { key: key });
                
                $.ajax({
                    url: generateResourceUri(),
                    type: 'GET',
                    contentType: 'application/json',
                    success: function(result) {
                        processData(key, result);
                    },
                    complete: function(xhr, status) {
                        pubsub.publish('action.tab.lazy.fetched', { key: key, status: status, result: xhr.responseText });
                    }
                });
            }
            else
                pubsub.publish('trigger.notification.toast', { type: 'error', message: 'Lazy loading isn\'t currently supported by your server implementation.' });
        },
        processData = function (key, result) {
            var panelHolder = elements.panelHolder(),
                panel = panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]');
            
            renderEngine.insert(panel, pluginData.data, pluginMetadata.structure); 

            
        },
        rendering = function (options) {
            if (options.plugin.isLazy)
                options.plugin.dontRender = true;
        },
        rendered = function (options) {
            if (options.plugin.isLazy)
                retrieveData(options.key);
        };
     
    pubsub.subscribe('action.panel.rendering', rendering);
    pubsub.subscribe('action.panel.rendered', rendered);
})(jQueryGlimpse, glimpse);