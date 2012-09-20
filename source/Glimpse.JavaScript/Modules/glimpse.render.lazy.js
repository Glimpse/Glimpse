(function($, data, util, elements, pubsub, renderEngine) {
    var generateLazyAddress = function (key) {
            return util.uriTemplate(data.currentMetadata().resources.glimpse_tab, { 'requestId': data.currentData().requestId, 'pluginKey': key });
        },
        retrieveData = function(options) {
            var resources = data.currentMetadata().resources; 
            
            if (resources.glimpse_tab) {
                pubsub.publish('action.tab.lazy.fetching', { key: options.key });
                
                $.ajax({
                    url: generateLazyAddress(key),
                    type: 'GET',
                    contentType: 'application/json',
                    success: function(result) {
                        processData(options.key, options.pluginData, options.pluginMetadata, result);
                    },
                    complete: function(xhr, status) {
                        pubsub.publish('action.tab.lazy.fetched', { key: options.key, status: status, result: xhr.responseText });
                    }
                });
            }
            else
                pubsub.publishAsync('trigger.notification.toast', { type: 'error', message: 'Lazy loading isn\'t currently supported by your server implementation. Sorry :(' });
        },
        processData = function (key, pluginData, pluginMetadata, result) {
            options.pluginData.data = result;

            renderEngine.insert(elements.panel(key), pluginData.data, pluginMetadata.structure);  
        },
        rendering = function (options) {
            if (options.pluginData.isLazy)
                options.plugin.dontRender = true;
        },
        rendered = function (options) {
            if (options.pluginData.isLazy)
                retrieveData(options);
        };
     
    pubsub.subscribe('action.panel.rendering', rendering);
    pubsub.subscribe('action.panel.rendered', rendered);
})(jQueryGlimpse, glimpse.data, glimpse.util, glimpse.elements, glimpse.pubsub, glimpse.render.engine);