notificationrController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('state.final', check); 
        },   

        //Main 
        check = function () {
            var metadata = data.currentMetadata(),
                newestVersion = util.cookie('glimpseLatestVersion'),
                currentVersion = '';

            if (newestVersion) {
                currentVersion = metadata.version;
                if (currentVersion < newestVersion)
                    elements.holder.find('.glimpse-meta-update').attr('title', 'Update: Glimpse ' + parseFloat(newestVersion).toFixed(2) + ' now available on nuget.org').css('display', 'inline-block');
                return;
            }

            util.cookie('glimpseLatestVersion', -1, 1); 
            $.ajax({
                dataType: 'jsonp',
                url: 'http://getglimpse.com/Glimpse/CurrentVersion/',
                success: function (data) {
                    util.cookie('glimpseLatestVersion', data, 1);
                }
            }); 
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()