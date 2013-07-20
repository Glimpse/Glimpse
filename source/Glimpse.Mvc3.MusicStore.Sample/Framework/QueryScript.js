(function ($, pubsub, util, data, renderEngine) {
    var generateAddress = function(query) {
            var currentMetadata = data.currentMetadata();
            return util.uriTemplate(currentMetadata.resources.mvcmusicstore_framework_queryresource, { 'query': query });
        },
        setup = function (args) {
            args.newData.data.query = { name: 'Query', data: '', isPermanent: true };
            args.newData.metadata.plugins.query = {}; 
        },
        build = function(args) {
            args.panel.html('<div class="glimpse-header">Input</div><div class="glimpse-header-content"><textarea class="glimpse-query-input"></textarea><input type="button" class="glimpse-query-submit" value="Query?"></div><div class="glimpse-header">Output</div><div class="glimpse-query-output"><div class="glimpse-header-content"><em>No results currently found...</em></div></div>');

            args.panel.find('.glimpse-query-submit').click(function () {
                $.ajax({
                    url: generateAddress($('.glimpse-query-input').val()),
                    type: 'GET',
                    contentType: 'application/json',
                    success: function (result) { 
                        layout(args.panel, result);
                    }
                });
            });
        },
        layout = function (panel, result) {
            renderEngine.insert(panel.find('.glimpse-query-output'), result, null);
        };
    
    pubsub.subscribe('action.data.initial.changed', setup);
    pubsub.subscribe('action.panel.rendered.query', build);
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.data, glimpse.render.engine);