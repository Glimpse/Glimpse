notificationController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('state.final', check); 
            pubsub.subscribe('state.build.shell.modify', wireDomListeners); 
            pubsub.subscribe('action.update.view', view);
        },   
        wireDomListeners = function() { 
            elements.scope.find('.glimpse-meta-update').click(function (event) {
                 event.preventDefault(); event.stopPropagation(); pubsub.publish('action.update.view');
            });
        }, 

        //Main 
        retrieveStamp = function () {
            if (!settings.stamp) {
                settings.stamp = (new Date()).getTime();
                pubsub.publish('state.persist');
            }
            return settings.stamp;
        },
        check = function () {
            var metadata = data.currentMetadata(),
                version = metadata.version;

            if (settings.newVersion) 
                elements.holder.find('.glimpse-meta-update').attr('title', 'New Updates are available, take a look at what you are missing.').css('display', 'inline-block');

            if (util.cookie('glimpseVersionCheck'))
                return;

            if (version) {
                var payload = buildVersionPayload(version),
                    url = util.replaceTokens(metadata.resources.glimpse_version_check, payload);
                
                $.ajax({
                    dataType: 'jsonp',
                    type: 'GET',
                    url: url,
                    success: function(data) {
                        settings.newVersion = data;
                        pubsub.publish('state.persist');
                    },
                    complete: function () {
                        util.cookie('glimpseVersionCheck', 1, 1); //Not sure if this should only be set on success 
                    }
                });
            }
        }, 
        view = function () {
            var metadata = data.currentMetadata(),
                version = metadata.version;
            if (version) {
                var payload = buildVersionPayload(version),
                    url = util.replaceTokens(metadata.resources.glimpse_version_detail, payload);

                elements.lightbox.find('.glimpse-lb-element').html('<div class="close">[close]</div><iframe src="' + url + '"></iframe>');
                elements.lightbox.show();
            }
        },
        
        buildVersionPayload = function (version) {
            return {
                packages: version.map(function(x) { return x.name; }).join(','),
                versions: version.map(function(x) { return x.version; }).join(','),
                stamp: retrieveStamp()
            };
        },
        
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()