data = (function () {
    var //Support
        inner = {},  
        base = {},
        metadataBase = {},
    
        //Main 
        mergeMetadata = function () { 
            if (!inner.metadata)
                inner.metadata = {};  
            $.extend(true, inner.metadata, metadataBase);
        },
        update = function (data) {
            inner = data;
            mergeMetadata();
            pubsub.publish('action.data.update');
        },
        reset = function () {
            update(base);
        },
        retrieve = function (requestId, callback) { 
            if (callback && callback.start)
                callback.start(requestId);

            if (requestId != base.requestId) {
                $.ajax({
                    url : glimpsePath + 'History',
                    type : 'GET',
                    data : { 'ClientRequestID': requestId },
                    contentType : 'application/json',
                    success : function (result, textStatus, jqXHR) {   
                        if (callback && callback.success) { callback.success(requestId, result, inner, textStatus, jqXHR); }
                        update(result);  
                    }, 
                    complete : function (jqXHR, textStatus) {
                        if (callback && callback.complete) { callback.complete(requestId, jqXHR, textStatus); }
                    }
                });
            }
            else { 
                if (callback && callback.success) { callback.success(requestId, base, inner, 'Success'); }
                update(base);  
                if (callback && callback.complete) { callback.complete(requestId, undefined, 'Success'); } 
            }
        },

        current = function () {
            return inner;
        }, 
        currentMetadata = function () {
            return inner.metadata;
        },
        
        initData = function (input) { 
            inner = input; 
            base = input; 
            
            mergeMetadata(); 
        },
        initMetadata = function (input) {
            metadataBase = input;
        };
     
    return { 
        current : current, 
        currentMetadata : currentMetadata,
        update : update,
        retrieve : retrieve,
        reset : reset,
        initData : initData,
        initMetadata : initMetadata
    };
}())