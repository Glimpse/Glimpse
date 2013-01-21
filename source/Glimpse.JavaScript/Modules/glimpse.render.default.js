(function(settings, pubsub, elements) { 
    var readyOpen = function () {
            var isOpen = settings.local('isOpen'); 
            if (isOpen) 
                pubsub.publish('trigger.shell.open', { isInitial: true }); 
        },
        readySelect = function () {
            var current = settings.local('view'); 
            if (!current)
                current = elements.tabHolder().find('li:not(.glimpse-active, .glimpse-disabled):first').attr('data-glimpseKey'); 
            pubsub.publish('trigger.tab.select.' + current, { key: current });
        },
        selected = function (options) {
            settings.local('view', options.key);
        };

    pubsub.subscribe('trigger.shell.ready', readyOpen);
    pubsub.subscribe('action.tab.rendered', readySelect);
    pubsub.subscribe('trigger.tab.select', selected);
})(glimpse.settings, glimpse.pubsub, glimpse.elements);