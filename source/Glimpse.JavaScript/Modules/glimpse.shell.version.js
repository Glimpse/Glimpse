glimpse.versionCheck = (function ($, pubsub, settings, elements) {
    var wireListeners = function () {
            //TODO: Need to wireup light box
        },
        retrieveStamp = function () {
            if (!settings.local('stamp'))
                settings.local('stamp', (new Date()).getTime()); 
            return settings.local('stamp');
        },
        ready = function() {
            var nextChecked = local('nextCheckedVersionTime'),
                hasNewerVersion = local('hasNewerVersion'),
                now = new Date();
            
            if (hasNewerVersion)
                elements.holder.find('.glimpse-meta-update').show();

            if (nextChecked) {
                var nextCheckedTickes = parseInt(nextChecked),
                    currentTimeTickes = now.getTime();
                if (nextCheckedTickes > currentTimeTickes)
                    return;
            }
            
            var metadata = data.currentMetadata(),
                url = util.uriTemplate(metadata.resources.glimpse_version_check, { stamp: retrieveStamp() });
            
            $.ajax({
                url: url, 
                type: 'GET',
                dataType: 'jsonp',
                crossDomain: true,
                jsonpCallback: 'glimpse.versionCheck.result'   //TODO: Need to setup a correct callback
            });
            
            local('nextCheckedVersionTime', now.setDate(now.getDate() + 1).getTime());
        },
        result = function(data) {
            local('hasNewerVersion', data.hasNewer);
        };
    
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners  );
    pubsub.subscribe('trigger.system.ready', ready);

    return {
        result: result
    };
})(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.elements)