(function($, util, engine, engineUtil) {
    var providers = engine._providers,
        build = function (data, level, forceFull, metadata, forceLimit) {  
            var limit = !$.isNumeric(forceLimit) ? 3 : forceLimit;

            if (engineUtil.shouldUsePreview(util.lengthJson(data), level, forceFull, limit, forceLimit, 1))
                return buildPreview(data, level);
            return buildOnly(data, level, metadata);
        }, 
        buildOnly = function(data, level, metadata) { 
            var i = 1, 
                html = '<table><thead><tr class="glimpse-row-header glimpse-row-header-' + level + '"><th class="glimpse-cell-key">Key</th><th class="glimpse-cell-value">Value</th></tr></thead>';
            for (var key in data)
                html += '<tr class="' + (i++ % 2 ? 'odd' : 'even') + '"><th>' + engineUtil.raw.process(key) + '</th><td> ' + providers.master.build(data[key], level + 1, null, engineUtil.keyMetadata(key, metadata)) + '</td></tr>';
            html += '</table>';

            return html;
        },
        buildPreview = function (data, level) { 
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + buildPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + build(data, level, true) + '</div></td></tr></table>';
        },
        buildPreviewOnly = function (data, level) {
            var length = util.lengthJson(data), 
                rowMax = 2, 
                rowLimit = (rowMax < length ? rowMax : length), i = 1, 
                html = '<span class="start">{</span>';

            for (var key in data) {
                html += engineUtil.newItemSpacer(i, rowLimit, length);
                if (i > length || i++ > rowLimit)
                    break;
                html += '<span>\'</span>' + providers.string.build(key, level + 1) + '<span>\'</span><span class="mspace">:</span><span>\'</span>' + providers.string.build(data[key], level, 12) + '<span>\'</span>';
            }
            html += '<span class="end">}</span>';

            return html;
        },
        provider = {
            build : build,
            buildOnly : buildOnly,
            buildPreview : buildPreview,
            buildPreviewOnly : buildPreviewOnly
        }; 

    engine.register('keyValue', provider);
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util);
