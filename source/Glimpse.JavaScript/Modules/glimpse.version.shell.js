(function($, pubsub, elements, util, settings) {
    var wireListeners = function() {
            elements.holder().find('.glimpse-meta-update').click(function() { pubsub.publish('trigger.shell.version.info.show'); });
        elements.lightbox().find('.close').live('click', function() { pubsub.publish('trigger.shell.version.info.close'); });
        },  
        retrieveStamp = function () {
            if (!settings.local('stamp'))
                settings.local('stamp', (new Date()).getTime()); 
            return settings.local('stamp');
        },
        generateVersionDetailAddress = function () {
            return settings.local('versionViewUri') + '&stamp=' + retrieveStamp();
        },
        show = function() {
            var detailsUri = settings.local('versionViewUri');;
            if (detailsUri) { 
                elements.lightbox().find('.glimpse-lightbox-element').html('<div class="close">[close]</div><iframe src="' + generateVersionDetailAddress() + '"></iframe>');
                elements.lightbox().show();
            }
            else
                pubsub.publishAsync('trigger.notification.toast', { type: 'error', message: 'Version check isn\'t currently supported by your server implementation. Sorry :(' });
        },
        close = function() {
            elements.lightbox().find('.glimpse-lightbox-element').empty();
            elements.lightbox().hide();
        };
    
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
    pubsub.subscribe('trigger.shell.version.info.show', show);
    pubsub.subscribe('trigger.shell.version.info.close', close);
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements, glimpse.util, glimpse.settings);