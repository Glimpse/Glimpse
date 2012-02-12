pubsub = (function () {
    var //Support
        registry = {},
        pRegistry = {},
        lastUid = -1,

        //Main
        reset = function () {
            registry = {};
            pRegistry = {};
            lastUid = -1;
        },
        publish = function (message, data) { 
            if (!pRegistry[message])
                pRegistry[message] = [];
            pRegistry[message].push(true);

            var subscribers = registry[message];
            var throwException = function (e) {
                return function () {
                    throw e;
                };
            }; 
            if (subscribers) {
                for (var i = 0, j = subscribers.length; i < j; i++) { 
                    subscribers[i].func(message, data); 
                }
            }
            return true;
        },
        subscribe = function (message, func) { 
            var token = (++lastUid).toString();

            if (!registry.hasOwnProperty(message)) {
                registry[message] = [];
            } 
            registry[message].push({ token : token, func : func });
         
            return token;
        };

    return {
        publish : publish, 
        subscribe : subscribe,
        reset : reset,
        sRegistry : function () { return registry; },
        pRegistry : function () { return pRegistry; }
    };
}())