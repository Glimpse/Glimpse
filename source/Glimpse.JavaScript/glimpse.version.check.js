glimpse.versionCheck = (function($, pubsub, settings, elements, data, util) {
    var retrieveStamp = function() {
            if (!settings.local('stamp'))
                settings.local('stamp', (new Date()).getTime());
            return settings.local('stamp');
        },
        generateVersionCheckAddress = function() {
            var currentMetadata = data.currentMetadata();
            return util.uriTemplate(currentMetadata.resources.glimpse_version_check, { stamp: retrieveStamp(), 'hash': currentMetadata.hash });
        },
        tryShow = function () {
            var hasNewerVersion = settings.local('hasNewerVersion'),
                versionCheckUri = settings.local('versionCheckUri') || '',
                currentHash = settings.local('hash');
            if (hasNewerVersion && versionCheckUri.indexOf('hash=' + currentHash) > -1)
                elements.holder().find('.glimpse-meta-update').show();
        },
        ready = function() {
            var nextChecked = settings.local('nextCheckedVersionTime'),
                now = new Date();

            tryShow();

            // Only check if we need to
            if (nextChecked) {
                var nextCheckedTicks = parseInt(nextChecked),
                    currentTimeTicks = now.getTime();
                if (nextCheckedTicks > currentTimeTicks)
                    return;
            }

            // Get the check uri and store checkUri for verification purposes
            var versionCheckUri = generateVersionCheckAddress();
            settings.local('versionCheckUri', versionCheckUri); 
            
            $.ajax({
                url: versionCheckUri,
                type: 'GET',
                dataType: 'jsonp',
                crossDomain: true,
                jsonpCallback: 'glimpse.versionCheck.result' 
            });

            settings.local('nextCheckedVersionTime', now.setDate(now.getDate() + 1));
        },
        result = function(data) {
            settings.local('hasNewerVersion', data.hasNewer);
            settings.local('versionViewUri', data.viewLink); 
            
            tryShow();
        };

    pubsub.subscribe('trigger.data.init', ready);

    return {
        result: result
    };
})(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.elements, glimpse.data, glimpse.util);