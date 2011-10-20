lazyloaderController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('action.plugin.lazyload', function(subject, payload) { fetch(payload); }); 
        },  
        
        //Main
        fetchFunc = {
            complete : function (key) { pubsub.publishAsync('action.tab.select', key); }
        },
        fetch = function (key) {
            data.retrievePlugin(key, fetchFunc); 
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()