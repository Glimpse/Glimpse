(function($, pubsub, elements, settings) {
    var coreOpen = function (isInit) {
             pubsub.publish('action.shell.opening', {isInit: isInit});
            
            elements.opener().hide(); 
            $.fn.add.call(elements.holder(), elements.pageSpacer())
                .show()
                .animate({ height : settings.local('height') ||300 }, (isInit ? 0 : 'fast'), function () {
                    pubsub.publish('action.shell.opened', {isInit: isInit});
                });            
        },
        wireListeners = function () { 
            elements.opener().click(function () { pubsub.publish('trigger.shell.open'); });
            elements.barHolder().find('.glimpse-minimize').click(function () { pubsub.publish('trigger.shell.minimize'); });
            elements.barHolder().find('.glimpse-close').click(function () { pubsub.publish('trigger.shell.close'); });
        }, 
        ready = function() { 
            if (settings.local('isOpen'))
                coreOpen(true);
        },  
        open = function() {
            settings.local('isOpen', true);
            coreOpen(false);
        },
        minimize = function() {
            settings.local('isOpen', false);
            
            pubsub.publish('action.shell.minimizing');

            var count = 0;
            $.fn.add.call(elements.holder(), elements.pageSpacer())
                .animate({ height : '0' }, 'fast', function() {
                    $(this).hide();
                
                    if (count++ == 1) {
                        elements.opener().show(); 
                        pubsub.publish('action.shell.minimized'); 
                    }
                });
            
        },
        close = function() {
            settings.local('isOpen', false);
            settings.global('glimpseState', null, -1);
            
            pubsub.publish('action.shell.closeing');

            elements.holder().remove();
            elements.pageSpacer().remove();
            elements.opener().remove(); 

            pubsub.publish('action.shell.closeed'); 
        };
    
    pubsub.subscribe('trigger.shell.open', open);
    pubsub.subscribe('trigger.shell.minimize', minimize);
    pubsub.subscribe('trigger.shell.close', close);
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('trigger.system.ready', ready);
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements, glimpse.settings);