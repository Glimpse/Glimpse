sizerController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('state.build.shell', setup); 
        },
        applyNewHeihgt = function () {
            var height = elements.holder.height();

            //Persist height
            settings.height = height;
            pubsub.publish('state.persist');

            //Apply the current height
            elements.holder.find('.glimpse-spacer').height(height);
            elements.holder.find('.glimpse-panel').height(height - 52);
                    
            pubsub.subscribe('action.resize', height - 52);
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
                    endDragCallback: function () { applyNewHeihgt(); }
                });
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()