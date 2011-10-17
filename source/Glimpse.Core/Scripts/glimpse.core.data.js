data = (function () {
    var //Support
        inner = {},
    
        //Main
        update = function (data) {
            inner = data;
            pubsub.publish('action.data.update');
        },
        current = function () {
            return inner;
        },
        currentMetadata = function () {
            return inner.data._metadata;
        },
        init = function () {
            inner = glimpseData; 
        };
        
    init(); 
    
    return {
        current : current,
        currentMetadata : currentMetadata,
        update : update
    };
}())