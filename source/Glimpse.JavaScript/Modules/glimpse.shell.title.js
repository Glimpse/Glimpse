(function($, pubsub, data, elements) {
    var renderLayout = function() {
            var requestData = data.currentData(),
                name = requestData.clientName ? '"' + requestData.clientName + '"' : '';

            elements.titleHolder().find('.glimpse-snapshot-name').text(name);
            elements.titleHolder().find('.glimpse-uri').text(requestData.uri);
        };
    
    pubsub.subscribe('action.shell.loaded', renderLayout);
})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements);