tollbarController = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('action.open', function(topic, payload) { open(payload); });
            pubsub.subscribe('action.minimize', function() { close(false); });
            pubsub.subscribe('action.close', function() { close(true); }); 
            pubsub.subscribe('state.build.shell.modify', wireDomListeners); 
            pubsub.subscribe('state.build.rendered', restore); 
        },
        wireDomListeners = function() {
            elements.scope.find('.glimpse-open').click(function () { pubsub.publish('action.open', false); return false; });
            elements.scope.find('.glimpse-minimize').click(function () { pubsub.publish('action.minimize'); return false; });
            elements.scope.find('.glimpse-close').click(function () { pubsub.publish('action.close'); return false; });
            elements.scope.find('.glimpse-popout').click(function () { pubsub.publish('action.popout'); return false; });
        }, 
            
        //Main 
        open = function (shortCircuitSlide) {
            settings.open = true;
            pubsub.publish('state.persist');

            elements.opener.hide(); 
            $.fn.add.call(elements.holder, elements.spacer).show().animate({ height : settings.height }, (shortCircuitSlide ? 0 : 'fast'));   
        },
        close = function (remove) {
            settings.open = false;
            pubsub.publish('state.persist');

            var panelElements = $.fn.add.call(elements.holder, elements.spacer).animate({ height : '0' }, 'fast', function() {
                    panelElements.hide();

                    if (remove) {
                        elements.scope.remove();
                        pubsub.publish('state.persist');
                    }
                    else
                        elements.opener.show(); 
                });
        },  
        restore = function () {
            var key = settings.activeTab,
                opened = settings.open,
                tab = elements.tabHolder.find('.glimpse-tab[data-glimpseKey="' + key + '"]');

            if (tab.length == 0 || tab.hasClass('glimpse-disabled'))
                key = elements.tabHolder.find('.glimpse-tab:not(.glimpse-disabled):first').attr('data-glimpseKey');

            pubsub.publish('action.tab.select', key);

            if (opened)
                pubsub.publish('action.open', true);
        },
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()