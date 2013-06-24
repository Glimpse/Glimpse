(function($, util, engine, engineUtil) {
    var providers = engine._providers, 
        build = function (data, level, forceFull, metadata, forceLimit) {   
            var html = '<div class="glimpse-row-holder glimpse-row-holder-suppress"><div class="glimpse-row">';
            for (var key in data) {
                var value = data[key];
                html += '<div class="glimpse-header-item"><div class="glimpse-header">' + key + '</div>';
                if ($.isArray(value) || value === Object(value))
                    html += providers.master.build(value, 0, null, engineUtil.keyMetadata(key, metadata));
                else 
                    html += '<div class="glimpse-header-content">' + (value != null ? util.preserveWhitespace(value) : '<em>null</em>') + '</div>'; 
                html += '</div>';
            }
            html += '</div></div>';
            return html;
        }, 
        provider = {
            build : build
        }; 

    engine.register('heading', provider);
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util);
