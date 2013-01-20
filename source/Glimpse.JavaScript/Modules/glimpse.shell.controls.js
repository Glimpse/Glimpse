(function($, pubsub, elements, settings) {
    var wireListeners = function () { 
            elements.opener().click(function () { pubsub.publish('trigger.shell.open', { isInitial: false }); });
            elements.barHolder().find('.glimpse-minimize').click(function () { pubsub.publish('trigger.shell.minimize'); });
            elements.barHolder().find('.glimpse-close').click(function () { pubsub.publish('trigger.shell.close'); });
        },  
        open = function(args) {
            if (!args.isInitial)
                settings.local('hidden', false);
            
            if (!settings.local('hidden') || args.force) {
                settings.local('isOpen', true);

                pubsub.publish('action.shell.opening', { isInitial: args.isInitial });

                var height = settings.local('height') || 300,
                    body = $.fn.add.call(elements.holder(), elements.pageSpacer()).show();
                
                settings.local('height', height);
                settings.local('panelHeight', height - 52);

                elements.opener().hide();
                if (args.isInitial)
                    body.height(height);
                else 
                    body.animate({ height: settings.local('height') }, 'fast');
                
                pubsub.publish('action.shell.opened', { isInitial: args.isInitial });
            }
            else
                pubsub.publish('trigger.shell.suppressed.open');
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
        hide = function () {
            settings.local('hidden', true);

            elements.holder().hide();
            elements.pageSpacer().hide();
            elements.opener().show(); 
        },
        close = function() {
            settings.local('isOpen', false);
            settings.local('hidden', false);
            settings.global('glimpsePolicy', null, -1);
            
            pubsub.publish('action.shell.closeing');

            elements.holder().remove();
            elements.pageSpacer().remove();
            elements.opener().remove(); 

            pubsub.publish('action.shell.closeed'); 
        };
    
    pubsub.subscribe('trigger.shell.open', open);
    pubsub.subscribe('trigger.shell.minimize', minimize);
    pubsub.subscribe('trigger.shell.close', close);
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners); 
    pubsub.subscribe('trigger.shell.hide', hide);
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements, glimpse.settings);