glimpse.elements = (function($) {
    var scope = $(document),
        panelHolder, tabHolder;
    
    return {
        tabHolder: function() {
             return tabHolder || (tabHolder = scope.find('.glimpse-tabs ul'));
        },
        panelHolder: function() {
             return panelHolder || (panelHolder = scope.find('.glimpse-panel-holder'));
        },
        panel: function(key) {
             return this.panelHolder().find('.glimpse-panel[data-glimpseKey="' + key + '"]');
        }
    };
})(jQueryGlimpse);