renderEngine = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('render.data', function(message, data) { master.build(data.data, 0, true, data.metadata, 1); });
        },
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