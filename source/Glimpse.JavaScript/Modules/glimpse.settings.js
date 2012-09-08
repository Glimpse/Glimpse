glimpse.settings = (function($, pubsub, util) {
    var globalSettings = {},
        localStorage = {},  
        restore = function () {
            globalSettings = $.extend({}, util.cookie('glimpseOptions'));
            localStorage = $.extend({}, util.localStorage('glimpseOptions'));
        };

    pubsub.subscribe('trigger.system.init', restore);

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