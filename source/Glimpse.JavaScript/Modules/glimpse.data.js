glimpse.data = (function($, pubsub, util) {
    var innerBaseMetadata = { plugins : {}, resources : {} },
        innerBaseData = { data : {}, metadata : innerBaseMetadata },
        innerCurrentData = innerBaseData,
        generateRequestAddress = function (requestId) {
            return util.uriTemplate(currentMetadata().resources.glimpse_request, { 'requestId': requestId });
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
        copyPermanentData = function (newData) {
            for (var key in innerBaseData.data) {
                var current = innerBaseData.data[key];
                if (current.isPermanent) {
                    newData.data[key] = current;
                    newData.metadata.plugins[key] = innerBaseData.metadata.plugins[key];
                }
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
        update = function (data, topic) {
            var oldData = innerCurrentData;
            
            pubsub.publish('action.data.refresh.changing', { oldData: oldData, newData: data, type: topic });
            pubsub.publish('action.data.changing', { newData: data });
             
            innerCurrentData = data;
             
            validateMetadata(); 
            copyPermanentData(data);
            
            pubsub.publish('action.data.changed', { newData: data });
            pubsub.publish('action.data.refresh.changed', { oldData: oldData, newData: data, type: topic });

            pubsub.publish('trigger.system.refresh');
        },
        reset = function () {
            update(innerBaseData);
        },
        retrieve = function (requestId, topic) { 
            var parsedTopic = topic ? '.' + topic : '';

            pubsub.publish('action.data.retrieve.starting' + parsedTopic, { requestId: requestId });

            // Only need to do to the server if we dont have the data
            if (requestId != innerBaseData.requestId) {
                pubsub.publish('action.data.fetching' + parsedTopic, requestId);
                
                $.ajax({
                    type: 'GET',
                    url: generateRequestAddress(requestId), 
                    contentType: 'application/json',
                    success: function (result) {    
                        pubsub.publish('action.data.fetched' + parsedTopic, { requestId: requestId, oldData: innerCurrentData, newData: result });
                        
                        pubsub.publish('action.data.retrieve.succeeded' + parsedTopic, { requestId: requestId, oldData: innerCurrentData, newData: result });
                        
                        update(result, topic);  
                    }, 
                    complete: function (jqXhr, textStatus) { 
                        pubsub.publish('action.data.retrieve.completed' + parsedTopic, { requestId: requestId, textStatus: textStatus });
                    }
                });
            }
            else { 
                pubsub.publish('action.data.retrieve.succeeded' + parsedTopic, { requestId: requestId, oldData: innerCurrentData, newData: innerBaseData });
                
                update(innerBaseData);  
                
                pubsub.publish('action.data.retrieve.completed' + parsedTopic, { requestId: requestId, textStatus: 'success' });
            }
        },
        initMetadata = function (input) {
            pubsub.publish('action.data.metadata.changing', { metadata: input });
            
            innerBaseMetadata = input;
            
            pubsub.publish('action.data.metadata.changed', { metadata: input });
        },
        initData = function (input) { 
            pubsub.publish('action.data.initial.changing', { newData: input });
            pubsub.publish('action.data.changing', { newData: input });
            
            innerCurrentData = input; 
            innerBaseData = input; 
            
            validateMetadata(); 
            
            pubsub.publish('action.data.changed', { newData: input });
            pubsub.publish('action.data.initial.changed', { newData: input });

            pubsub.publish('trigger.data.init', { isInitial: true });
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
})(jQueryGlimpse, glimpse.pubsub, glimpse.util);