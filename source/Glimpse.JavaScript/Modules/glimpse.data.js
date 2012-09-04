glimpse.data = (function($, glimpse) {
    var pubsub = glimpse.pubsub,
        innerBaseData = {},
        innerBaseMetadata = {},
        innerCurrentData = {},
        requestUrl = function (requestId) {
            return util.replaceTokens(currentMetadata().resources.glimpse_request, { 'requestId': requestId });
        },
        validateMetadata = function () { 
            // Make sure that out data has metadata
            if (!innerCurrentData.metadata)
                innerCurrentData.metadata = { plugins : {} };  
            
            // Merge metadata from the base metadata, with the request metadata
            var newMetadata = {};
            $.extend(true, newMetadata, innerBaseMetadata, innerCurrentData.metadata);
            innerCurrentData.metadata = newMetadata;
            
            // Make sure that every plugin has metadata object
            for (var key in innerCurrentData.data) {
                if (!innerCurrentData.metadata.plugins[key])
                    innerCurrentData.metadata.plugins[key] = {};
            }
        },
        baseData = function () {
            return innerBaseData;
        },
        currentData = function () {
            return innerCurrentData;
        }, 
        currentMetadata = function () {
            return innerCurrentData.metadata;
        },
        update = function (data) {
            var oldData = innerCurrentData;
            
            pubsub.publish('action.data.changing', data);
            pubsub.publish('action.data.refresh.changing', oldData, data);
            
            // Set the data as current
            innerCurrentData = data;
            
            // Make sure the metadata is correct 
            validateMetadata();
            
            pubsub.publish('action.data.refresh.changing', oldData, data);
            pubsub.publish('action.data.changed', data);
        },
        reset = function () {
            update(innerBaseData);
        },
        retrieve = function (requestId, callback) { 
            if (!callback)
                callback = {};
            if (callback.start)
                callback.start(requestId);

            // Only need to do to the server if we dont have the data
            if (requestId != innerBaseData.requestId) {
                pubsub.publish('action.data.featching', requestId);
                
                $.get({
                    url: requestUrl(requestId), 
                    contentType: 'application/json',
                    success: function (result, textStatus, jqXHR) {    
                        pubsub.publish('action.data.featched', requestId, innerCurrentData, result);
                        
                        if (callback.success) 
                            callback.success(requestId, result, innerCurrentData, textStatus, jqXHR);
                        
                        update(result);  
                    }, 
                    complete: function (jqXHR, textStatus) {
                        if (callback.complete)
                            callback.complete(requestId, jqXHR, textStatus); 
                    }
                });
            }
            else { 
                if (callback.success) 
                    callback.success(requestId, innerBaseData, innerCurrentData, 'success'); 
                
                update(innerBaseData);  
                
                if (callback.complete)  
                    callback.complete(requestId, undefined, 'success'); 
            }
        },
        initMetadata = function (input) {
            pubsub.publish('action.data.metadata.changing', input);
            
            innerBaseMetadata = input;
            
            pubsub.publish('action.data.metadata.changed', input);
        },
        initData = function (input) { 
            pubsub.publish('action.data.changing', input);
            pubsub.publish('action.data.initial.changing', input);
            
            innerCurrentData = input; 
            innerBaseData = input; 
            
            validateMetadata(); 
            
            pubsub.publish('action.data.initial.changed', input);
            pubsub.publish('action.data.changed', input);
        };

    return {
        baseData: baseData,
        currentData: currentData,
        currentMetadata: currentMetadata,
        update: update,
        reset: reset,
        retrieve: retrieve,
        initMetadata: initMetadata,
        initData: initData
    };
})(jQueryGlimpse, glimpse);