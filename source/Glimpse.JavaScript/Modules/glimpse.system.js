glimpse = (function($) {
    var obj = {},
        ready = function() {
            obj.pubsub.publish('trigger.system.init');
            obj.pubsub.publish('action.system.starting');
            obj.pubsub.publish('trigger.shell.load');
            obj.pubsub.publish('trigger.shell.render');
            obj.pubsub.publish('action.system.started');
        };

    $(function() {
        obj.pubsub.subscribe('trigger.system.start', ready); 
        obj.pubsub.publishAsync('trigger.system.start');
    });

    return obj;
})(jQueryGlimpse);