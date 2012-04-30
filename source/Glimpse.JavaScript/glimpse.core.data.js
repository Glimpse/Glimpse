data = (function () {
    var //Support
        inner = {},  
        baseInner = {},
        baseMetadata = {},
    
        //Main 
        mergeMetadata = function () { 
            if (!inner.metadata)
                inner.metadata = {};  
            $.extend(true, inner.metadata, baseMetadata);
            
            for (var key in inner.data) {
                if (!inner.metadata.plugins[key])
                    inner.metadata.plugins[key] = {};
            }
        },
        update = function (data) {
            inner = data;
            mergeMetadata();
            pubsub.publish('action.data.update');
        },
        reset = function () {
            update(baseInner);
        },
        retrieve = function (requestId, callback) { 
            if (callback && callback.start)
                callback.start(requestId);

            if (requestId != baseInner.requestId) {
                $.ajax({
                    url : currentMetadata().history,
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
                if (callback && callback.success) { callback.success(requestId, baseInner, inner, 'Success'); }
                update(baseInner);  
                if (callback && callback.complete) { callback.complete(requestId, undefined, 'Success'); } 
            }
        },
         
        base = function () {
            return baseInner;
        },
        current = function () {
            return inner;
        }, 
        currentMetadata = function () {
            return inner.metadata;
        },
        
        initData = function (input) { 
            inner = input; 
            baseInner = input; 
            
            mergeMetadata(); 
        },
        initMetadata = function (input) {
            baseMetadata = input;
        };
     
    return { 
        base : base,
        current : current, 
        currentMetadata : currentMetadata,
        update : update,
        retrieve : retrieve,
        reset : reset,
        initData : initData,
        initMetadata : initMetadata
    };
}())