    /*
    elements.scope = scope
    elements.holder = elements.scope.find('.glimpse-holder')
    elements.opener = elements.scope.find('.glimpse-open')
    elements.spacer = elements.scope.find('.glimpse-spacer')
    */

    //pubsub
    //util
    (function () {
        var //Public
            persist = function () { 
                util.cookie('glimpseOptions', settings);
            },
            restore = function () {
                $.extend(settings, util.cookie('glimpseOptions'));
            },
            terminate = function () {
                util.cookie('glimpseState', null);
            },
        
            //Private
            init = function () {
                pubsub.subscribe('state.persist', persist);
                pubsub.subscribe('state.restore', restore);
                pubsub.subscribe('state.terminate', terminate);
            };
    
        init(); 
    }());