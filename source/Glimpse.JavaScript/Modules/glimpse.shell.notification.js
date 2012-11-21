(function($, pubsub, elements) {
    var toast = function(options) {
            var toast  = $('<div class="glimpse-notification glimpse-notification-' + options.type + '">' + options.message + '</div>').appendTo(elements.notificationHolder());
            setTimeout(function() {
                 toast.fadeOut(250, function() {
                      toast.remove();
                 });
            }, 5000);
        };

    pubsub.subscribe('trigger.notification.toast', toast);
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements)