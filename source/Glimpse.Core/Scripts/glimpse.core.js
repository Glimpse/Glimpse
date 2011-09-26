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
/*(import:glimpse.core.util.js|2)*/,
/*(import:glimpse.core.pubsub.js|2)*/,
/*(import:glimpse.core.plugin.js|2)*/,
/*(import:glimpse.core.data.js|2)*/,
/*(import:glimpse.controller.state.js|2)*/, 
/*(import:glimpse.controller.shell.js|2)*/,
/*(import:glimpse.controller.tollbar.js|2)*/, 
/*(import:glimpse.render.engine.js|2)*/, 
        init = function () {
            pubsub.publish('state.init');
            pubsub.publish('state.build'); 
        };
        

    return { 
        init : init,
        pubsub : pubsub,
        plugin : plugin,
        elements : elements,
        render : renderEngine
    };
}($Glimpse, $Glimpse(document)));

$Glimpse(document).ready(function() {
    glimpse.init();
});