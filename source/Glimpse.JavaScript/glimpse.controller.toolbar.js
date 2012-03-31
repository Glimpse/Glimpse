toolbarController = function () {
    var //Support
        isPopup = function() {
            return window.location.href.indexOf(data.currentMetadata().paths.popup) > -1;
        },
        wireListeners = function() {
            pubsub.subscribe('action.open', function(topic, payload) { open(payload); });
            pubsub.subscribe('action.minimize', function() { close(false); });
            pubsub.subscribe('action.close', function() { close(true); }); 
            pubsub.subscribe('action.popout', popout); 
            pubsub.subscribe('state.final', checkPopout); 
            pubsub.subscribe('state.build.shell.modify', wireDomListeners); 
            pubsub.subscribe('state.build.rendered', restore); 
        },
        wireDomListeners = function() {
            elements.scope.find('.glimpse-open').click(function () { pubsub.publish('action.open', false); return false; });
            elements.scope.find('.glimpse-minimize').click(function () { pubsub.publish('action.minimize'); return false; });
            elements.scope.find('.glimpse-close').click(function () { pubsub.publish('action.close'); return false; });
            elements.scope.find('.glimpse-popout').click(function () { pubsub.publish('action.popout'); return false; });

            if (settings.popupOn) {
                if (isPopup()) {
                    $(window).resize(function () {
                        elements.holder.find('.glimpse-panel').height($(window).height() - 54);
                    });
                } 
                $(window).unload(closePopup);
            }
        }, 
        openPopup = function () { 
            settings.popupOn = true;
            pubsub.publish('state.persist');

            util.cookie('glimpseKeepPopup', '1');

            //TODO !!!! This needs to be updated once we get going !!!!
            var url = data.currentMetadata().paths.popup; // + '&glimpseRequestID=' + $('#glimpseData').data('glimpse-requestID');
            window.open(url, 'GlimpsePopup', 'width=1100,height=600,status=no,toolbar=no,menubar=no,location=no,resizable=yes,scrollbars=yes');
        },
            
        //Main 
        open = function (shortCircuitSlide) {
            settings.open = true;
            pubsub.publish('state.persist');

            elements.opener.hide(); 
            $.fn.add.call(elements.holder, elements.spacer).show().animate({ height : settings.height }, (shortCircuitSlide ? 0 : 'fast'));   
        },
        close = function (remove, suppressPersist) {
            if (!suppressPersist) {
                settings.open = false;
                pubsub.publish('state.persist');
            }

            var panelElements = $.fn.add.call(elements.holder, elements.spacer).animate({ height : '0' }, 'fast', function() {
                    panelElements.hide();

                    if (remove) {
                        elements.scope.remove(); 
                        pubsub.publish('state.terminate');
                    }
                    else
                        elements.opener.show(); 
                });
        },  
        popout = function () { 
            openPopup();

            close(false, true);
        },
        checkPopout = function () {
            var pResult = isPopup();
            if (pResult)
                util.cookie('glimpseKeepPopup', '');

            if (settings.open && settings.popupOn && !pResult)
                openPopup(); 
        },
        closePopup = function () {
            if (isPopup() && !util.cookie('glimpseKeepPopup')) { 
                settings.popupOn = false;
                pubsub.publish('state.persist');
            }
            else
                util.cookie('glimpseKeepPopup', null);
        },
        restore = function () {
            var key = settings.activeTab,
                opened = settings.open,
                popupOn = settings.popupOn,
                tab = elements.tabHolder.find('.glimpse-tab[data-glimpseKey="' + key + '"]');

            if (tab.length == 0 || tab.hasClass('glimpse-disabled'))
                key = elements.tabHolder.find('.glimpse-tab:not(.glimpse-disabled):first').attr('data-glimpseKey');

            pubsub.publish('action.tab.select', key);

            if (opened && !popupOn)
                pubsub.publish('action.open', true);
        },
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()