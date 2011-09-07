var glimpse = (function () {
    var //Private
        elements = {},
        data = {},
        settings = {},
/*(import:glimpse.util.js)*/,

        //Public
/*(import:glimpse.pubsub.js)*/,
/*(import:glimpse.plugin.js)*/,
        init = function () {
            pubsub.publish('state.init');
            pubsub.publish('state.build'); 
        };
        
/*(import:glimpse.init.js)*/
/*(import:glimpse.state.js)*/
/*(import:glimpse.action.js)*/ 
/*(import:glimpse.shell.js)*/

    return { 
        init : init,
        pubsub : pubsub,
        plugin : plugin,
        elements : elements
    };
}());

