(function(settings, pubsub, elements) { 
    var start = function () {
            var current = settings.local('view'),
                isOpen = settings.local('isOpen');
        
            if (!current)
                current = elements.tabHolder().find('li:not(.glimpse-active, .glimpse-disabled):first').attr('data-glimpseKey'); 
            pubsub.publish('trigger.tab.select.' + current, { key: current });
        
            if (isOpen) 
                pubsub.publish('trigger.shell.open', { isInitial: true }); 
        },
        selected = function (options) {
            settings.local('view', options.key);
        };

    pubsub.subscribe('trigger.shell.present', start);
    pubsub.subscribe('trigger.tab.select', selected);
})(glimpse.settings, glimpse.pubsub, glimpse.elements);