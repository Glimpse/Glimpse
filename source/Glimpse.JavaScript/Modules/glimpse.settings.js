glimpse.settings = (function($, pubsub, util) {
    var globalSettings = {},
        localStorage = {},  
        init = function () {
            globalSettings = $.extend({}, util.cookie('glimpseOptions'));
            localStorage = $.extend({}, util.localStorage('glimpseOptions'));

            $(window).bind('storage', function(e) {
                if (e.originalEvent.key == 'glimpseOptions') 
                    localStorage = JSON.parse(e.originalEvent.newValue); 
            });
        };

    pubsub.subscribe('trigger.system.init', init);

    return {
        global: function (key, value) { 
            if (arguments.length == 1) 
                return globalSettings[key];
            
            globalSettings[key] = value;
            util.cookie('glimpseOptions', globalSettings); 
        },
        local: function (key, value) {
            if (arguments.length == 1) 
                return localStorage[key];
            
            localStorage[key] = value;
            util.localStorage('glimpseOptions', localStorage);  
        }
    };
})(jQueryGlimpse, glimpse.pubsub, glimpse.util);