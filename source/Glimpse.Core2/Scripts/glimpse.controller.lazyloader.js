lazyloaderController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('action.plugin.lazyload', function(subject, payload) { fetch(payload); }); 
        },   
        retrievePlugin = function(key) {   
            var currentData = data.current();
            $.ajax({
                url : glimpsePath + 'History',
                type : 'GET',
                data : { 'ClientRequestID' : currentData.requestId, 'PluginKey' : key },
                contentType : 'application/json',
                success : function (result) {
                    var itemData = currentData.data[key];
                    itemData.data = result;  
                    itemData.isLazy = false;

                    pubsub.publishAsync('action.tab.select', key);
                }
            });
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