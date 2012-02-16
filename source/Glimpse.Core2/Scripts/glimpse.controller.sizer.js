sizerController = function () {
    var //Support    
        wireListeners = function() {
            pubsub.subscribe('state.build.shell', setup); 

            pubsub.subscribe('action.plugin.created', function(topic, payload) { pluginCreated(payload); }); 
            pubsub.subscribe('action.resize', function(topic, payload) { shellResized(payload); }); 
        },
        shellResized = function (height) { 
            //Persist height 
            settings.height = height;
            pubsub.publish('state.persist');

            //Apply the current height
            elements.holder.find('.glimpse-spacer').height(height);
            elements.holder.find('.glimpse-panel').height(height - 52); 
        },
        pluginCreated = function (key) {
            var panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]'); 

            panel.height(settings.height - 52); 
        },
        
        //Main
        setup = function () { 
            //Bind the resizer
            util.resizer(elements.holder.find('.glimpse-resizer'), {
                    getValue : function(settings) { return settings.resizeScope.height(); },
                    setValue : function(settings, value) { return settings.resizeScope.height(value + 'px'); },
                    resizeScope : elements.holder,
                    opacityScope : elements.holder,
                    isUpDown : true, 
                    offset : -1,
                    endDragCallback: function () { pubsub.publish('action.resize', elements.holder.height()); }
                });
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()