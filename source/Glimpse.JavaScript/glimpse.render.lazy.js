(function($, data, util, elements, pubsub, renderEngine) {
    var generateLazyAddress = function (key) {
            var currentMetadata = data.currentMetadata();
            return util.uriTemplate(currentMetadata.resources.glimpse_tab, { 'requestId': data.currentData().requestId, 'pluginKey': key, 'version': currentMetadata.version });
        },
        retrieveData = function(options) {
            var resources = data.currentMetadata().resources; 
            
            if (resources.glimpse_tab) {
                pubsub.publish('action.tab.lazy.fetching', { key: options.key });
                
                $.ajax({
                    url: generateLazyAddress(options.key),
                    type: 'GET',
                    contentType: 'application/json',
                    success: function(result) {
                        processData(options.key, options.pluginData, options.pluginMetadata, result);
                    },
                    complete: function(xhr, status) {
                        pubsub.publish('action.tab.lazy.fetched', { key: options.key, status: status });
                    }
                });
            }
            else
                pubsub.publishAsync('trigger.notification.toast', { type: 'error', message: 'Lazy loading isn\'t currently supported by your server implementation. Sorry :(' });
        },
        processData = function (key, pluginData, pluginMetadata, result) {
            pluginData.data = result;

            renderEngine.insert(elements.panel(key), pluginData.data, pluginMetadata.structure);  
        },
        rendering = function (options) {
            if (options.pluginData.isLazy)
                options.pluginData.dontRender = true;
        },
        rendered = function (options) {
            if (options.pluginData.isLazy)
                retrieveData(options);
        };
     
    pubsub.subscribe('action.panel.rendering', rendering);
    pubsub.subscribe('action.panel.rendered', rendered);
})(jQueryGlimpse, glimpse.data, glimpse.util, glimpse.elements, glimpse.pubsub, glimpse.render.engine);