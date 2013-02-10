(function($, engine) {
    var provider = {
            build: function(data) { 
                if (data == null || data === '')
                    data = 'No data found for this plugin.';
                return '<div class="glimpse-panel-message">' + data + '</div>';
            }
        };

    engine.register('empty', provider);
})(jQueryGlimpse, glimpse.render.engine);