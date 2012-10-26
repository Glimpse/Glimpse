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
            var panelHeight = height - 52;
            
            settings.local('height', height);
            settings.local('panelHeight', panelHeight);

            elements.pageSpacer().height(height);
            elements.panels().height(panelHeight);
            
            pubsub.publish('trigger.shell.resize', { height: height, panelHeight: panelHeight });
        },
        panelRendered = function (args) {
            var height = settings.local('height');
            
            elements.panel(args.key).height(height - 52);
        };
    
    pubsub.subscribe('action.panel.rendered', panelRendered);
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements, glimpse.settings, glimpse.util);