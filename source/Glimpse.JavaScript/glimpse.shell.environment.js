(function($, pubsub, data, elements, util) {
    var wireListeners = function() {
            elements.titleHolder().find('.glimpse-enviro').dropdown();
        },
        buildHtml = function(requestMetadata) {
            var uris = requestMetadata.environmentUrls, 
                html = ''; 

            if (uris) {
                var currentName = 'Enviro', 
                    currentDomain = util.getDomain(unescape(window.location.href));

                for (var targetName in uris) {
                    if (util.getDomain(uris[targetName]) === currentDomain) {
                        currentName = targetName;
                        html += ' - ' + targetName + ' (Current)<br />';
                    }
                    else
                        html += ' - <a title="Go to - ' + uris[targetName] + '" href="' + uris[targetName] + '">' + targetName + '</a><br />';
                }
                html = '<span class="glimpse-drop">' + currentName + '<span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over"><div>Switch Servers</div>' + html + '</div>';
            }
            return html;
        },
        renderLayout = function() {
            var html = buildHtml(data.currentMetadata());

            elements.titleHolder().find('.glimpse-enviro').html(html);
        };

    pubsub.subscribe('action.shell.rendering', renderLayout);
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements, glimpse.util);