glimpse.tab = (function($, pubsub, data) {
    var register = function(args) {
            //TODO: need to refactor this out as static plugins should not be stored in with the instance plugins    
            var currentData = data.currentData(),
                currentMetadata = data.currentMetadata();
            currentData.data[args.key] = args.payload;
            currentMetadata.plugins[args.key] = args.metadata;
            
            pubsub.publish('tigger.tab.insert', args); 
        };
    
    return {
            register: register
        };
})(jQueryGlimpse, glimpse.pubsub, glimpse.data);