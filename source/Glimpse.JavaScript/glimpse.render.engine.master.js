(function($, engine) {
    var providers = engine._providers,
        provider = {
            build: function(data, level, forceFull, metadata, forceLimit) {
                var result = '',
                    isArray = $.isArray(data),
                    isObject = data === Object(data);

                if (metadata) {
                    if (metadata.engine && providers[metadata.engine])
                        result = providers[metadata.engine].build(data, level, forceFull, metadata, forceLimit);
                    else if (metadata.layout && isArray) 
                        result = providers.layout.build(data, level, forceFull, metadata, forceLimit);
                    else if (metadata.keysHeadings && isObject)
                        result = providers.heading.build(data, level, forceFull, metadata, forceLimit);
                }
                
                if (result === '') {
                    if (typeof data === 'function')
                        result = providers.func.build(data, level, forceFull, metadata, forceLimit);
                    else if (isArray)
                        result = providers.table.build(data, level, forceFull, metadata, forceLimit);
                    else if (isObject)
                        result = providers.keyValue.build(data, level, forceFull, metadata, forceLimit);  
                    else if (level == 0) 
                        result = providers.empty.build(data);
                    else 
                        result = providers.string.build(data, level, forceFull, forceLimit);
                }
                
                return result;
            }
        };

    engine.register('master', provider);
})(jQueryGlimpse, glimpse.render.engine);