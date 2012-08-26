toolbarController = function () {
    var //Support
        isPopup = function() {
            var resources = data.currentMetadata().resources;
            if (resources.glimpse_popup) 
                return window.location.href.indexOf(util.replaceTokens(data.currentMetadata().resources.glimpse_popup)) > -1;
            return false;
        },
        wireListeners = function() {
            pubsub.subscribe('action.open', function(topic, payload) { open(payload); });
            pubsub.subscribe('action.minimize', function() { close(false); });
            pubsub.subscribe('action.close', function() { close(true); }); 
            pubsub.subscribe('action.popout', popout); 
            pubsub.subscribe('state.final', checkPopout); 
            pubsub.subscribe('state.build.shell.modify', wireDomListeners); 
            pubsub.subscribe('state.build.rendered', restore);
            pubsub.subscribe('action.close.lightbox', closeLightbox);
        },
        wireDomListeners = function() {
            elements.scope.find('.glimpse-open').click(function () { pubsub.publish('action.open', false); return false; });
            elements.scope.find('.glimpse-minimize').click(function () { pubsub.publish('action.minimize'); return false; });
            elements.scope.find('.glimpse-close').click(function () { pubsub.publish('action.close'); return false; });
            elements.lightbox.find('.close').live('click', function () { pubsub.publish('action.close.lightbox'); });
            
            // Determine whether we need to wireup up any popup events 
            var resources = data.currentMetadata().resources,
                popoutElement = elements.scope.find('.glimpse-popout');
            if (resources.glimpse_popup) {
                popoutElement.click(function () { pubsub.publish('action.popout'); return false; });

                if (settings.popupOn) {
                    if (isPopup()) {
                        $(window).resize(function () {
                            elements.holder.find('.glimpse-panel').height($(window).height() - 54);
                        });
                    } 
                    $(window).unload(closePopup);
                }
            }
            else
                popoutElement.hide();
        }, 
        
        openPopup = function () { 
            settings.popupOn = true;
            pubsub.publish('state.persist');

            util.cookie('glimpseKeepPopup', '1');

            var url = util.replaceTokens(data.currentMetadata().resources.glimpse_popup, { requestId: data.current().requestId });
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
        closeLightbox = function () {
            elements.scope.find('.glimpse-lb').hide();
        },

        init = function () {
            wireListeners();
        };
    
    init(); 
} ()