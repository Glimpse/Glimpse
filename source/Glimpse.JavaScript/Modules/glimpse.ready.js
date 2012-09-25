(function(pubsub) {
    var ready = function() {
            pubsub.publish('trigger.system.init');
            
            pubsub.publish('action.system.starting'); 
            pubsub.publish('trigger.shell.render');
            pubsub.publish('action.system.started');
            
            pubsub.publish('trigger.shell.default.view');
            
            pubsub.publish('trigger.system.ready');
        },
        start = function() {
            // Trigger this as async 
            pubsub.publishAsync('trigger.system.start');
        };

    pubsub.subscribe('trigger.system.start', ready); 
    pubsub.subscribe('action.data.initial.changed', start);
})(glimpse.pubsub);