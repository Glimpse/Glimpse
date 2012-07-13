/*(import:jquery-1.6.3.min.js)*/

var glimpse = (function ($, scope) {
    if (!console) { (console = {}).log = function () {}; }

    var //Private
        elements = {},
        template = {},
        settings = {
            height : 250,
            activeTab: 'Routes'
        }, 
/*(import:glimpse.core.util.js|2)*/,
/*(import:glimpse.core.pubsub.js|2)*/,
/*(import:glimpse.core.state.js|2)*/,
/*(import:glimpse.core.data.js|2)*/,
/*(import:glimpse.process.elements.js|2)*/, 
/*(import:glimpse.process.template.js|2)*/,
/*(import:glimpse.controller.render.js|2)*/,
/*(import:glimpse.controller.shell.js|2)*/,
/*(import:glimpse.controller.sizer.js|2)*/,
/*(import:glimpse.controller.toolbar.js|2)*/, 
/*(import:glimpse.controller.metadata.js|2)*/,
/*(import:glimpse.controller.title.js|2)*/, 
/*(import:glimpse.controller.lazyloader.js|2)*/, 
/*(import:glimpse.controller.notification.js|2)*/,
/*(import:glimpse.controller.paging.js|2)*/,
/*(import:glimpse.render.engine.js|2)*/, 
        init = function () { 
            var start = new Date().getTime();
            
            pubsub.publish('state.init'); 
            pubsub.publish('state.build');  
            pubsub.publish('state.render'); 
            pubsub.publish('state.final'); 
            
            var end = new Date().getTime(); 
            console.log('Total execution time: ' + (end - start));
        };
    
    return { 
        init : init,
        pubsub : pubsub, 
        elements : elements,
        render : renderEngine,
        data : data,
        util : util,
        settings : settings
    };
}($Glimpse, $Glimpse(document)));

$Glimpse(document).ready(function() {
    glimpse.init();
});

/*(import:glimpse.plugin.ajax.js)*/
/*(import:glimpse.plugin.history.js)*/
/*(import:glimpse.plugin.timeline.js)*/
/*(import:google-code-prettify.js)*/