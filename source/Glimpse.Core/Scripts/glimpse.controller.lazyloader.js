lazyloaderController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('action.plugin.lazyload', function(subject, payload) { fetch(payload); }); 
        },   
        retrievePlugin = function(key, callback) {  
            $.ajax({
                url : glimpsePath + 'History',
                type : 'GET',
                data : { 'ClientRequestID' : inner.requestId, 'PluginKey' : key },
                contentType : 'application/json',
                success : function (result, textStatus, jqXHR) { 
                    var currentData = data.current();
                    currentData.data[key].data = data;  

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