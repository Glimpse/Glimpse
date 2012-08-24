lazyloaderController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('action.plugin.lazyload', function(subject, payload) { fetch(payload); }); 
        },   
        retrievePlugin = function(key) {   
            var currentData = data.current(),
                resources = data.currentMetadata().resources;
            
            if (resources.glimpse_tab) {
                $.ajax({
                    url: util.replaceTokens(resources.glimpse_tab, { 'requestId': currentData.requestId, 'pluginKey': key }),
                    type: 'GET',
                    contentType: 'application/json',
                    success: function(result) {
                        load(key, currentData, result);
                    }
                });
            }
            else 
                load(key, currentData, 'This feature is not supported');
        },
        load = function(key, currentData, payload) {
            var itemData = currentData.data[key];
            itemData.data = payload;
            itemData.isLazy = false;

            pubsub.publishAsync('action.tab.select', key);
        },
        
        //Main 
        fetch = function (key) {
            retrievePlugin(key); 
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()