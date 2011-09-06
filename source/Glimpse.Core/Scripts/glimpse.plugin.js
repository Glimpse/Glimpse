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
        }())