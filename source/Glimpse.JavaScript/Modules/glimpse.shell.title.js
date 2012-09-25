(function($, pubsub, data, elements) {
    var renderLayout = function() {
            var requestData = data.currentData(),
                name = requestData.clientName ? '"' + requestData.clientName + '"' : '&nbsp;';

            elements.titleHolder().find('.glimpse-snapshot-name').html(name);
            elements.titleHolder().find('.glimpse-uri').text(requestData.uri);
        };
    
    pubsub.subscribe('action.shell.rendering', renderLayout);
})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements);