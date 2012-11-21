glimpse.elements = (function($) {
    var scope = $(document),
        holder, opener, pageSpacer, barHolder, panelHolder, tabHolder, tabInstanceHolder, tabPermanentHolder, titleHolder, notificationHolder, lightbox, optionsHolder;
    
    return {
        scope: function () {
            return scope;
        },
        holder: function () {
            return holder || (holder = scope.find('.glimpse-holder'));
        },
        opener: function () {
            return opener || (opener = scope.find('.glimpse-open'));
        },
        pageSpacer: function () {
            return pageSpacer || (pageSpacer = scope.find('.glimpse-spacer'));
        },
        barHolder: function () {
            return barHolder || (barHolder = scope.find('.glimpse-bar'));
        },
        titleHolder: function () {
            return titleHolder || (titleHolder = scope.find('.glimpse-title'));
        },
        tabHolder: function() {
             return tabHolder || (tabHolder = scope.find('.glimpse-tabs ul'));
        },
        tabInstanceHolder: function() {
             return tabInstanceHolder || (tabInstanceHolder = scope.find('.glimpse-tabs-instance ul'));
        },
        tabPermanentHolder: function() {
             return tabPermanentHolder || (tabPermanentHolder = scope.find('.glimpse-tabs-permanent ul'));
        },
        panelHolder: function() {
             return panelHolder || (panelHolder = scope.find('.glimpse-panel-holder'));
        },
        notificationHolder: function() {
            return notificationHolder || (notificationHolder = scope.find('.glimpse-notification-holder'));
        },
        lightbox: function() {
            return lightbox || (lightbox = scope.find('.glimpse-lightbox'));
        },
        panel: function(key) {
             return this.panelHolder().find('.glimpse-panel[data-glimpseKey="' + key + '"]');
        },
        tab: function(key) {
             return this.tabHolder().find('.glimpse-tab[data-glimpseKey="' + key + '"]');
        },
        panels: function() {
             return this.panelHolder().find('.glimpse-panel');
        },
        optionsHolder: function() {
            return optionsHolder || (optionsHolder = scope.find('.glimpse-options'));
        }
    };
})(jQueryGlimpse);