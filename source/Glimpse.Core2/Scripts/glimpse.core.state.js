state = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('state.persist', persist);
            pubsub.subscribe('state.restore', restore);
            pubsub.subscribe('state.terminate', terminate);
            pubsub.subscribe('state.init', restore);  
        },
            
        //Main
        persist = function () { 
            util.cookie('glimpseOptions', settings);
        },
        restore = function () {
            settings = $.extend(settings, util.cookie('glimpseOptions'));
        },
        terminate = function () {
            util.cookie('glimpseState', null);
        }, 
        init = function () {
            wireListeners();
            restore();
        };
    
    init();  
} ()