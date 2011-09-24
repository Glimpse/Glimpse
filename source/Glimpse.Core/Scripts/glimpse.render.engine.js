renderEngine = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('render.data', function(message, data) { master.build(data.data, 0, true, data.metadata, 1); });
        },
        shouldUsePreview = function(length, level, forceFull, limit, forceLimit, tolerance) {
            if (!$.isNaN(forceLimit))  
                limit = forceLimit; 
            return !forceFull && ((level == 1 && length > (limit + tolerance)) || (level > 1 && (!forceLimit || length > (limit + tolerance))));
        }, 
        //Main
/*(import:glimpse.render.engine.master.js|2)*/,
/*(import:glimpse.render.engine.keyvalue.js|2)*/,
/*(import:glimpse.render.engine.table.js|2)*/,
/*(import:glimpse.render.engine.structed.js|2)*/,
/*(import:glimpse.render.engine.string.js|2)*/, 

        //Main 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()