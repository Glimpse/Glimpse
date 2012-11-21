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
            var metadata = data.currentMetadata();

            if (settings.newVersion) 
                elements.holder.find('.glimpse-meta-update').attr('title', 'New Updates are available, take a look at what you are missing.').css('display', 'inline-block');

            if (util.cookie('glimpseVersionCheck'))
                return;

            if (metadata.resources.glimpse_version_check) {
                var url = util.replaceTokens(metadata.resources.glimpse_version_check, { stamp: retrieveStamp() });
                
                $.ajax({
                    url: url, 
                    type: 'GET',
                    dataType: 'jsonp',
                    crossDomain: true,
                    jsonpCallback: 'glimpse.versionCheck'
                });
            }
            util.cookie('glimpseVersionCheck', 1, 1);  
        }, 
        view = function () {
            var metadata = data.currentMetadata();
            if (version) {
                var url = util.replaceTokens(metadata.resources.glimpse_version_detail, { stamp: retrieveStamp() });

                elements.lightbox.find('.glimpse-lb-element').html('<div class="close">[close]</div><iframe src="' + url + '"></iframe>');
                elements.lightbox.show();
            }
        },
        response = function (payload) {
            settings.newVersion = payload.hasNewer;
            pubsub.publish('state.persist');
        },
        
        init = function () {
            wireListeners();
        };
    
    init();

    return {
        response: response
    };
} ()