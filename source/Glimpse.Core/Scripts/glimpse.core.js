/*(import:jquery-1.6.3.min.js)*/

var glimpse = (function ($, scope) {
    var //Private
        elements = {},
        template = {},
        config = {
            path : '' 
        }
        settings = {
            height : '250px'
        },
/*(import:glimpse.core.util.js)*/,
/*(import:glimpse.core.pubsub.js)*/,
/*(import:glimpse.core.plugin.js)*/,
/*(import:glimpse.core.data.js)*/,
/*(import:glimpse.controller.state.js)*/, 
/*(import:glimpse.controller.shell.js)*/,
/*(import:glimpse.controller.tollbar.js)*/, 
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