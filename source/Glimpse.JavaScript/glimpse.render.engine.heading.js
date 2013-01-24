(function($, util, engine, engineUtil) {
    var providers = engine._providers,
        build = function (data, level, forceFull, forceLimit) {   
            var i = 1, 
                html = '';
            for (var key in data) {
                var value = data[key];
                html += '<div class="glimpse-header">' + key + '</div>';
                if (typeof value !== "string")
                    html += providers.master.build(value, 0);
                else 
                    html += '<div class="glimpse-header-content">' + util.preserveWhitespace(value) + '</div>'; 
            }
            return html;
        }, 
        provider = {
            build : build
        }; 

    engine.register('heading', provider);
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util);
