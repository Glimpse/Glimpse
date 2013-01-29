(function(pubsub) {
    var start = function() { 
            pubsub.publish('trigger.system.init');
            
            pubsub.publish('action.system.starting'); 
            pubsub.publish('trigger.shell.init');
            pubsub.publish('action.system.started');
            
            pubsub.publish('trigger.system.ready');
        },
        dataStart = function() {
            pubsub.publish('trigger.tab.render', { isInitial: true });
            pubsub.publish('trigger.title.render');
        },
        update = function() {
            pubsub.publish('trigger.system.refresh');
            
            pubsub.publish('trigger.shell.refresh'); 
        },
        dataUpdate = function() {
            pubsub.publish('trigger.tab.render', { isInitial: false });
            pubsub.publish('trigger.title.render');
        };

    pubsub.subscribe('trigger.system.start', start);
    pubsub.subscribe('trigger.data.init', dataStart); 
    pubsub.subscribe('trigger.system.update', update);
    pubsub.subscribe('trigger.data.update', dataUpdate); 
})(glimpse.pubsub);