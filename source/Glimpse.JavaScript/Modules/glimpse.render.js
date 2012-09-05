glimpse.render = (function($, glimpse, pubsub) {
    var coreRender = function(isInitial, topic) {
            pubsub.publish(topic + '.rendering', isInitial);
            pubsub.publish('action.shell.rendering', isInitial); 
            pubsub.publish('trigger.tab.render', isInitial); 
            pubsub.publish('action.shell.rendered', isInitial);
            pubsub.publish(topic + '.rendered', isInitial);
        },
        rerender = function() { 
            coreRender(false, 'action.shell.refresh'); 
        },
        render = function() { 
            coreRender(true, 'action.shell.initial'); 
        };

    glimpse.pubsub.subscribe('trigger.shell.render', render); 
    
    return {};
})(jQueryGlimpse, glimpse, glimpse.pubsub);