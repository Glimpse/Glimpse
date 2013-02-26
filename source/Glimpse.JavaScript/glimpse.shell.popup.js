(function($, pubsub, settings, data, elements, util) {
    var wireListeners = function() {
            elements.holder().find('.glimpse-popout').click(function() { pubsub.publish('trigger.shell.popup'); });
        },
        dataLoaded = function(args) {
            var uri = args.metadata.resources.glimpse_popup;
            if (uri) {
                elements.barHolder().find('.glimpse-popout').removeClass('glimpse-hidden');
                tryOpenPopup();
            } 

            if (settings.local('popupOn')) { 
                $(window).unload(windowUnloading);
            }
        },
        registerSuppressedOpen = function() {
            registerSuppressedOpen = true;
        },
        generatePopupAddress = function() {
            var currentMetadata = data.currentMetadata();
            return util.uriTemplate(currentMetadata.resources.glimpse_popup, { 'requestId': data.currentData().requestId, 'version': currentMetadata.version });
        },
        isPopup = function() {
            return data.currentMetadata().resources.glimpse_popup ? window.location.href.indexOf('n=glimpse_popup') > -1 : false;
        },
        openPopup = function() { //WHEN USE CLICKS OPEN BUTTON 
            settings.local('popupOn', true);   //Indicates popup should be used
            settings.local('popupKeep', true); //This second item is used to detect difference between user closing the page and a redirect 
             
            window.open(generatePopupAddress(), 'GlimpsePopup', 'width=1100,height=600,status=no,toolbar=no,menubar=no,location=no,resizable=yes,scrollbars=yes');

            pubsub.publish('trigger.shell.hide');
        },
        tryOpenPopup = function() { //WHEN PARENT WINDOW IS LOADING AND DETECTS THAT IT HAS BEEN SUPPRESSED, IF POPUP ON, REDIRECT THE POPUP OR FORCE THE WINDOW OPEN  
            if (settings.local('popupOn')) {
                if (!isPopup()) {
                    pubsub.publish('trigger.shell.popup');
                }
                else {
                    pubsub.publish('trigger.shell.open', { isInitial: true, force: true, fullScreen: true });
                     
                    settings.local('popupKeep', false);
                }
            }
        },
        windowUnloading = function() { //WHEN USER CLOSES POPUP WE WANT TO TURN EVERYTHING OFF 
            if (isPopup() && !settings.local('popupKeep'))
                settings.local('popupOn', false);
        },
        terminate = function() { //WHEN USER CLOSES OR OPENS SHELL IN THE MAIN WINDOW LETS KILL POPUP
            if (!isPopup()) {
                settings.local('popupOn', false);
                settings.local('popupKeep', false);
            }
        };

    pubsub.subscribe('trigger.shell.popup', openPopup);
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
    pubsub.subscribe('action.shell.closed', terminate);
    pubsub.subscribe('action.shell.opening', terminate); 
    pubsub.subscribe('action.data.initial.changed', dataLoaded);
    pubsub.subscribe('trigger.shell.suppressed.open', registerSuppressedOpen);
})(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.data, glimpse.elements, glimpse.util);