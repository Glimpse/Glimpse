glimpse.pubsub = (function() {
    var messages = {},
        lastUid = -1,
        throwException = function(ex) {
            return function() {
                throw ex;
            };
        },
        callSubscriber = function(subscriber, message, data) {
            try {
                subscriber(data, message);
            } catch(ex) {
                setTimeout(throwException(ex), 0);
            }
        },
        deliverMessage = function(originalMessage, matchedMessage, data) {
            var subscribers = messages[matchedMessage],
                i, j;

            if (!messages.hasOwnProperty(matchedMessage)) {
                return;
            }

            for (i = 0, j = subscribers.length; i < j; i++) {
                callSubscriber(subscribers[i].func, originalMessage, data);
            }
        },
        createDeliveryFunction = function(message, data) {
            return function() {
                var topic = String(message),
                    position = topic.lastIndexOf('.');

                // deliver the message as it is now
                deliverMessage(message, message, data);

                // trim the hierarchy and deliver message to each level
                while (position !== -1) {
                    topic = topic.substr(0, position);
                    position = topic.lastIndexOf('.');
                    deliverMessage(message, topic, data);
                }
            };
        },
        messageHasSubscribers = function(message) {
            var topic = String(message),
                found = messages.hasOwnProperty(topic),
                position = topic.lastIndexOf('.');

            while (!found && position !== -1) {
                topic = topic.substr(0, position);
                position = topic.lastIndexOf('.');
                found = messages.hasOwnProperty(topic);
            }

            return found;
        },
        publish = function(message, data, sync) {
            var deliver = createDeliveryFunction(message, data),
                hasSubscribers = messageHasSubscribers(message);

            if (!hasSubscribers) {
                return false;
            }

            if (sync === true) {
                deliver();
            } else {
                setTimeout(deliver, 0);
            }
            return true;
        };

    return {
        publishAsync: function(message, data) {
            return publish(message, data, false);
        },
        publish: function(message, data) {
            return publish(message, data, true);
        },
        subscribe: function(message, func) {
            // message is not registered yet
            if (!messages.hasOwnProperty(message)) {
                messages[message] = [];
            }

            // forcing token as String, to allow for future expansions without breaking usage
            // and allow for easy use as key names for the 'messages' object
            var token = String(++lastUid);
            messages[message].push({ token: token, func: func });

            // return token for unsubscribing
            return token;
        },
        unsubscribe: function(tokenOrFunction) {
            var isToken = typeof tokenOrFunction === 'string',
                key = isToken ? 'token' : 'func',
                succesfulReturnValue = isToken ? tokenOrFunction : true,
                result = false,
                m, i;

            for (m in messages) {
                if (messages.hasOwnProperty(m)) {
                    for (i = messages[m].length - 1; i >= 0; i--) {
                        if (messages[m][i][key] === tokenOrFunction) {
                            messages[m].splice(i, 1);
                            result = succesfulReturnValue;

                            // tokens are unique, so we can just return here
                            if (isToken) {
                                return result;
                            }
                        }
                    }
                }
            }

            return result;
        }
    };
})();