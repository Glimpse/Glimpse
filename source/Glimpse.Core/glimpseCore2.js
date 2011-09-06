var glimpse = (function () {
    var //Private
        elements = {},
        findElements = (function () { 
            elements.scope = scope;
            elements.holder = elements.scope.find('.glimpse-holder');
            elements.opener = elements.scope.find('.glimpse-open');
            elements.spacer = elements.scope.find('.glimpse-spacer');  
        }),
        util = { 
            cookie : function (key, value, expiresIn) {
                key = encodeURIComponent(key);
                //Set Cookie
                if (arguments.length > 1) {
                    var t = new Date();
                    t.setDate(t.getDate() + expiresIn || 1000);

                    value = $.isPlainObject(value) ? JSON.stringify(value) : String(value);
                    return (document.cookie = key + '=' + encodeURIComponent(value) + '; expires=' + t.toUTCString() + '; path=/');
                }

                //Get cookie 
                var result = new RegExp("(?:^|; )" + key + "=([^;]*)").exec(document.cookie);
                if (result) {
                    result = decodeURIComponent(result[1]);
                    if (result.substr(0, 1) == '{') {
                        result = JSON.parse(result);
                    }
                    return result;
                }
                return null;
            }
        },

        //Public
        pubsub = (function () {
            var //Private
                registry = {},
                lastUid = -1,
                publishCore = function (message, data, sync) {
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
                            try {
                                subscribers[i].func(message, data);
                            } catch(e) {
                                setTimeout(throwException(e), 0);
                            }
                        }
                    };
        
                    if (sync === true) {
                        deliverMessage();
                    } else {
                        setTimeout(deliverMessage, 0);
                    }
                    return true;
                },
        
                //Public
                publish = function (message, data) {
                    return publishCore(message, data, false);
                },
                publishSync = function (message, data) {
                    return publishCore(message, data, true);
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
                publishSync : publishSync,
                subscribe : subscribe,
                unsubscribe : unsubscribe
            };
        }()),
        plugin = (function () {
            var //Private
                plugins = {}, 
                startPlugin = function (pluginId) {
                    pluginData[pluginId].instance = pluginData[pluginId].creator();   //TODO: What to pass the plugin 
                    pluginData[pluginId].instance.init();
                },
                stopPlugin = function (pluginId) {
                    var data = pluginData[pluginId];
                    if (data.instance) {
                        data.instance.destroy();
                        data.instance = null;
                    } 
                },

                //Public
                registerPlugin = function (pluginId, creator) {
                    plugins[pluginId] = { creator : creator, instance : null };
                },
                startAllPlugins = function () {
                    for (var pluginId in pluginData) { startPlugin(pluginId); }
                },
                stopAllPlugins = function () {
                    for (var pluginId in pluginData) { stopPlugin(pluginId); }
                };
    
            return {
                register : registerPlugin,
                startAllPlugins : startAllPlugins,
                stopAllPlugins : stopAllPlugins
            };
        }()),
        init = function () { 
            //findElements();
            //plugin.startAllPlugins();
            
            pubsub.publish('state.init'); 
        };

    //pubsub
    //util
    (function () {
        var //Public
            persist = function () { 
                util.cookie('glimpseOptions', settings);
            },
            restore = function () {
                $.extend(settings, util.cookie('glimpseOptions'));
            },
            terminate = function () {
                util.cookie('glimpseState', null);
            },
        
            //Private
            init = function () {
                pubsub.subscribe('state.persist', persist);
                pubsub.subscribe('state.restore', restore);
                pubsub.subscribe('state.terminate', terminate);
            };
    
        init(); 
    }());
    //settings
    //elements
    (function () {
        var //Public 
            open = function () {
                settings.open = true;
                pubsub.publish('state.persist');

                elements.opener.hide(); 
                $.fn.add.call(elements.holder, elements.spacer).show().animate({ height : settings.height }, 'fast');  
            },
            close = function (remove) {
                settings.open = false;
                pubsub.publish('state.persist');

                var panelElements = $.fn.add.call(elements.holder, elements.spacer).animate({ height : '0' }, 'fast', function() {
                        panelElements.hide();

                        if (remove) {
                            elements.scope.remove();
                            pubsub.publish('state.persist');
                        }
                        else
                            elements.opener.show(); 
                    });
            }, 
            resize = function (height) {
                settings.height = height;
                pubsub.publish('state.persist');
             
                elements.spacer.height(height);
                elements.holder.find('.glimpse-panel').height(height - 54); 
            },
        
            //Private
            _init = function () {
                pubsub.subscribe('action.open', open);
                pubsub.subscribe('action.close', close);
                pubsub.subscribe('action.terminate', function() { close(true); });
                pubsub.subscribe('action.resize', resize);
            };
    
        _init(); 
    }()); 

    return { 
        init : init,
        pubsub : pubsub,
        plugin : plugin,
        elements : elements
    };
}());

