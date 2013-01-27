(function($, engine) {
    var providers = engine._providers,
        provider = {
            build: function(data, level, forceFull, metadata, forceLimit) {
                var result = '';

                if (metadata && metadata.engine) {
                    if (providers[metadata.engine])
                        result = providers[metadata.engine].build(data, level, forceFull, metadata, forceLimit);
                    else
                        result = 'Specified engine could not be found: ' + metadata.engine;
                }
                else if ($.isArray(data)) {
                    if (metadata && metadata.layout)
                        result = providers.layout.build(data, level, forceFull, metadata.layout, forceLimit);
                    else
                        result = providers.table.build(data, level, forceFull, forceLimit);
                } 
                else if (data === Object(data)) {
                    if (metadata && metadata.keysHeadings)
                        result = providers.heading.build(data, level, forceFull, forceLimit);
                    else
                        result = providers.keyValue.build(data, level, forceFull, forceLimit);
                }
                else if (level == 0) 
                    result = providers.empty.build(data);
                else 
                    result = providers.string.build(data, level, forceFull, forceLimit);

                return result;
            }
        };

    engine.register('master', provider);
})(jQueryGlimpse, glimpse.render.engine);