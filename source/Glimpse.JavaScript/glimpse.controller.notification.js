notificationController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('state.final', check); 
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
                elements.holder.find('.glimpse-meta-update').attr('title', 'Update: Glimpse ' + settings.newVersion + ' now available on nuget.org').css('display', 'inline-block');

            if (util.cookie('glimpseVersionCheck'))
                return;

            $.ajax({
                dataType: 'jsonp',
                url: 'http://getglimpse.com/Glimpse/CurrentVersion/',
                data: { stamp: retrieveStamp(), version: metadata.version },
                success: function (data) { 
                    settings.newVersion = data;
                    pubsub.publish('state.persist');

                    util.cookie('glimpseVersionCheck', 1, 1);  //Not sure if this should only be set on success 
                }
            }); 
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()