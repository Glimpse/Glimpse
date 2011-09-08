/*(import:jquery-1.6.3.min.js)*/

var glimpse = (function ($, scope) {
    var //Private
        elements = {},
        template = {},
        settings = {
            path : ''
        },
/*(import:glimpse.util.js)*/,
/*(import:glimpse.pubsub.js)*/,
/*(import:glimpse.plugin.js)*/,
/*(import:glimpse.init.js)*/,
/*(import:glimpse.state.js)*/,
/*(import:glimpse.action.js)*/,
/*(import:glimpse.shell.js)*/, 
        init = function () {
            pubsub.publish('state.init');
            pubsub.publish('state.build'); 
        };
        

    return { 
        init : init,
        pubsub : pubsub,
        plugin : plugin,
        elements : elements
    };
}($Glimpse, $Glimpse(document)));

$Glimpse(document).ready(function() {
    glimpse.init();
});