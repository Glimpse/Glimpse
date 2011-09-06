var glimpse = (function () {
    var //Private
        elements = {},
/*(import:glimpse.element.js)*/,
/*(import:glimpse.util.js)*/,

        //Public
/*(import:glimpse.pubsub.js)*/,
/*(import:glimpse.plugin.js)*/,
        init = function () { 
            //findElements();
            //plugin.startAllPlugins();
            
            pubsub.publish('state.init'); 
        };

/*(import:glimpse.state.js)*/
/*(import:glimpse.action.js)*/

    return { 
        init : init,
        pubsub : pubsub,
        plugin : plugin,
        elements : elements
    };
}());

