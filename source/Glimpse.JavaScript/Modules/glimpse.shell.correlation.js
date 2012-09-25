(function($, pubsub, data, elements) {
    var wireListeners = function() {
            elements.titleHolder().find('.glimpse-correlation a').live('click', function() { data.retrieve($(this).attr('data-requestId'), 'correlation'); });
            elements.titleHolder().find('.glimpse-correlation').dropdown();
        },
        buildHtml = function(request, requestMetadata) {
            var correlation = requestMetadata.correlation,
                html = '';

            if (correlation) {
                var currentUri = request.uri,
                    currentLeg = '';

                html = '<div>' + correlation.title + '</div>';
                for (var i = 0; i < correlation.legs.length; i++) {
                    var leg = correlation.legs[i];
                    if (leg.uri == currentUri) {
                        currentLeg = leg.uri;
                        html += currentLeg + ' - <strong>' + leg.method + '</strong> (Current)<br />';
                    } else
                        html += '<a title="Go to ' + leg.uri + '" href="#" data-requestId="' + leg.requestId + '" data-url="' + leg.uri + '">' + leg.uri + '</a> - <strong>' + leg.method + '</strong><br />';
                }
                html = '<span class="glimpse-drop">' + currentLeg + '<span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over">' + html + '<div class="loading"><span class="icon"></span><span>Loaded...</span></div></div>';
            }
            return html;
        },
        renderLayout = function() {
            var html = buildHtml(data.currentData(), data.currentMetadata());
            if (html) {
                elements.titleHolder().find('.glimpse-uri').hide(); //TODO will probably need to reshow this at some point
                elements.titleHolder().find('.glimpse-correlation').html(html);
            }
        },
        startingRetrieve = function() {
            elements.titleHolder().find('.glimpse-correlation .loading').fadeIn();
        },
        completedRetrieve = function() {
            elements.titleHolder().find('.glimpse-correlation .loading').fadeOut();
        },
        succeededRetrieve = function(options) {
            options.newData.metadata.correlation = options.oldData.metadata.correlation;
        };

    pubsub.subscribe('action.shell.rendering', renderLayout);
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('action.data.retrieve.starting.correlation', startingRetrieve);
    pubsub.subscribe('action.data.retrieve.completed.correlation', completedRetrieve);
    pubsub.subscribe('action.data.retrieve.succeeded.correlation', succeededRetrieve);
})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements);