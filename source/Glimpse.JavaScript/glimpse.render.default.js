(function(settings, pubsub, elements) { 
    var readyOpen = function () {
            var isOpen = settings.local('isOpen'); 
            if (isOpen) 
                pubsub.publish('trigger.shell.open', { isInitial: true }); 
        },
        readySelect = function () {
            var current = settings.local('view'),
                tabElement = elements.tab(current),
                forced = current != null;
            
            if (!current || tabElement.length == 0) {
                tabElement = elements.tabHolder().find('li:not(.glimpse-active, .glimpse-disabled):first'); 
                current = tabElement.attr('data-glimpseKey');
            }
             
            if (tabElement.length > 0 && !tabElement.hasClass('glimpse-active'))
                pubsub.publish('trigger.tab.select.' + current, { key: current, forced: forced });
        },
        selected = function (args) {
            if (!args.forced)
                settings.local('view', args.key);
        };

    pubsub.subscribe('trigger.shell.ready', readyOpen);
    pubsub.subscribe('action.tab.inserted', readySelect);
    pubsub.subscribe('trigger.tab.select', selected);
})(glimpse.settings, glimpse.pubsub, glimpse.elements);