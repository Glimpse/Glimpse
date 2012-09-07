(function($) {
    var wireListeners = function () {
        
            elements.scope.find('.glimpse-open').click(function () { pubsub.publish('action.open', false); return false; });
            elements.scope.find('.glimpse-minimize').click(function () { pubsub.publish('action.minimize'); return false; });
            elements.scope.find('.glimpse-close').click(function () { pubsub.publish('action.close'); return false; });
        },
        open = function() {

        },
        minimize = function() {

        },
        close = function() {

        };
    
    pubsub.subscribe('trigger.shell.open', open);
    pubsub.subscribe('trigger.shell.minimize', minimize);
    pubsub.subscribe('trigger.shell.close', close);
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
})(jQueryGlimpse);