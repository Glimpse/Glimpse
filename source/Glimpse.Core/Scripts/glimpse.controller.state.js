stateController = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('state.persist', persist);
            pubsub.subscribe('state.restore', restore);
            pubsub.subscribe('state.terminate', terminate);
        },
            
        //Main
        persist = function () { 
            util.cookie('glimpseOptions', settings);
        },
        restore = function () {
            $.extend(settings, util.cookie('glimpseOptions'));
        },
        terminate = function () {
            util.cookie('glimpseState', null);
        }, 
        init = function () {
            wireListeners();
        };
    
    init();  
} ()