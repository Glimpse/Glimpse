(function(pubsub) {
    var start = function() {
            // Trigger this as async 
            setTimeout(function() {
                pubsub.publish('trigger.system.init');
            
                pubsub.publish('action.system.starting'); 
                pubsub.publish('trigger.shell.init');
                pubsub.publish('action.system.started');
            
                pubsub.publish('trigger.system.ready');
            }, 0);
        },
        refresh = function() {
            pubsub.publish('trigger.shell.refresh'); 
        };

    pubsub.subscribe('action.data.refresh.changed', refresh); 
    pubsub.subscribe('action.data.initial.changed', start);
})(glimpse.pubsub);