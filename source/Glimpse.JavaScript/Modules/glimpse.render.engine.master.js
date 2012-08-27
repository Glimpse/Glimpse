(function(engine) {
    var providers = engine._providers,
        provider = {
            build: function(data, level, forceFull, metadata, forceLimit) {
                var result = '';

                if ($.isArray(data)) {
                    if (metadata)
                        result = providers.structured.build(data, level, forceFull, metadata, forceLimit);
                    else
                        result = providers.table.build(data, level, forceFull, forceLimit);
                } 
                else if ($.isPlainObject(data)) 
                    result = providers.keyValue.build(data, level, forceFull, forceLimit);
                else if (level == 0) 
                    result = providers.empty.build(data);
                else 
                    result = providers.string.build(data, level, forceLimit);

                return result;
            }
        };

    engine.register('master', provider);
})(glimpse.render.engine);














