(function($, data, elements) {
    var wireListeners = function() {
            elements.titleHolder().find('.glimpse-url a').live('click', function() { data.retrieve($(this).attr('data-requestId'), 'prg'); });
            elements.titleHolder().find('.glimpse-url').dropdown();
        },
        buildHtml = function(request, requestMetadata) {
            var correlation = requestMetadata.correlation,
                html = request.uri;

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
        }.
        render = function() {
            var html = buildHtml(data.currentData(), data.currentMetadata());

            elements.titleHolder().find('.glimpse-url').html(html);
        },
        startingRetrieve = function() {
            elements.titleHolder().find('.glimpse-url .loading').fadeIn();
        },
        completedRetrieve = function() {
            elements.titleHolder().find('.glimpse-url .loading').fadeOut();
        };

    pubsub.subscribe('action.shell.loaded', render);
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('action.data.retrieve.starting.prg', startingRetrieve);
    pubsub.subscribe('action.data.retrieve.completed.prg', completedRetrieve);
})(jQueryGlimpse, glimpse.data, glimpse.elements);