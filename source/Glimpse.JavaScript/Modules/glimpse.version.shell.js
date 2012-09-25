(function($, pubsub, elements, util, data) {
    var wireListeners = function() {
            elements.holder().find('.glimpse-meta-update').click(function() { pubsub.publish('trigger.shell.version.info.show'); });
        },  
        retrieveStamp = function () {
            if (!settings.local('stamp'))
                settings.local('stamp', (new Date()).getTime()); 
            return settings.local('stamp');
        },
        generateVersionDetailAddress = function () {
            return util.uriTemplate(data.currentMetadata().resources.glimpse_version_detail, { stamp: retrieveStamp() });
        },
        show = function() {
            var metadata = data.currentMetadata();
            if (metadata.resources.glimpse_version_detail) { 
                elements.lightbox.find('.glimpse-lb-element').html('<div class="close">[close]</div><iframe src="' + generateVersionDetailAddress() + '"></iframe>');
                elements.lightbox.show();
            }
            else
                pubsub.publishAsync('trigger.notification.toast', { type: 'error', message: 'Version check isn\'t currently supported by your server implementation. Sorry :(' });
        };
    
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('trigger.shell.version.info.show', show);
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements, glimpse.util, glimpse.data);