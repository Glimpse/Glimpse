pubsub = (function () {
    var //Support
        registry = {},
        lastUid = -1,
        publishCore = function (message, data, sync) {
            console.log('Publish', message, data, sync);
            // if there are no subscribers to this message, just return here
            if (!registry.hasOwnProperty(message)) {
                return false;
            }
        
            var deliverMessage = function () {
                var subscribers = registry[message];
                var throwException = function (e) {
                    return function () {
                        throw e;
                    };
                }; 
                for (var i = 0, j = subscribers.length; i < j; i++) {
                    //try {
                        subscribers[i].func(message, data);
                    //} catch(e) {
                    //    setTimeout(throwException(e), 0);
                    //}
                }
            };
        
            if (sync === true) {
                deliverMessage();
            } else {
                setTimeout(deliverMessage, 0);
            }
            return true;
        },
        
        //Main
        publish = function (message, data) {
            return publishCore(message, data, true);
        },
        publishAsync = function (message, data) {
            return publishCore(message, data, false);
        },
        subscribe = function (message, func) { 
            var token = (++lastUid).toString();

            if (!registry.hasOwnProperty(message)) {
                registry[message] = [];
            } 
            registry[message].push({ token : token, func : func });
         
            return token;
        },
        unsubscribe = function (token) {
            for (var m in registry) {
                if (registry.hasOwnProperty(m)) {
                    for (var i = 0, j = registry[m].length; i < j; i++) {
                        if (registry[m][i].token === token) {
                            registry[m].splice(i, 1);
                            return token;
                        }
                    }
                }
            }
            return false;
        };

    return {
        publish : publish,
        publishAsync : publishAsync,
        subscribe : subscribe,
        unsubscribe : unsubscribe
    };
}())