var glimpse = (function () {
    var //Private

/*(import:glimpse.pubsub.js)*/,
/*(import:glimpse.plugin.js)*/,

        //Public
        init = function () { 
            glimpse.plugin.startAllPlugins();
        };

/*(import:glimpse.state.js)*/
/*(import:glimpse.action.js)*/

    return { 
        init : init,
        pubsub : pubsub,
        plugin : plugin
    };
}());

