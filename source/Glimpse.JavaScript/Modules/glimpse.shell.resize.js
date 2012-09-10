(function($, pubsub, elements, settings, util) {
    var wireListeners = function() {
            $.draggable({
                handelScope: elements.holder().find('.glimpse-resizer'),
                opacityScope: elements.holder(),
                resizeScope: elements.holder(),
                dragged: shellResized
            }); 
        },
        shellResized = function (args, height) {
            settings.local('height', height);

            elements.pageSpacer().height(height);
            elements.panels().height(height - 52);
        },
        panelRendered = function (args) {
            elements.panel(args.key).height(height - 52);
        };
    
    pubsub.subscribe('action.panel.rendered', panelRendered);
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements, glimpse.settings, glimpse.util);