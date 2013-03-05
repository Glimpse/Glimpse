(function($, util, engine, engineUtil) {
    var providers = engine._providers,
        build = function (data, level, forceFull, metadata, forceLimit) {  
            var limit = !$.isNumeric(forceLimit) ? 3 : forceLimit;

            if (engineUtil.shouldUsePreview(util.lengthJson(data), level, forceFull, limit, forceLimit, 1))
                return buildPreview(data, level);
            return buildOnly(data, level, metadata);
        }, 
        buildOnly = function(data, level, metadata) {
            var html = '<table>';
            if (engineUtil.includeHeading(metadata))
                html += '<thead><tr class="glimpse-row-header glimpse-row-header-' + level + '"><th class="glimpse-key">Key</th><th class="glimpse-cell-value">Value</th></tr></thead>';
            html += '<tbody class="glimpse-row-holder">';
            for (var key in data)
                html += '<tr class="glimpse-row"><th class="glimpse-key">' + engineUtil.raw.process(key) + '</th><td> ' + providers.master.build(data[key], level + 1, null, engineUtil.keyMetadata(key, metadata)) + '</td></tr>';
            html += '</tbody></table>';

            return html;
        },
        buildPreview = function (data, level) { 
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + buildPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + build(data, level, true) + '</div></td></tr></table>';
        },
        buildPreviewOnly = function (data, level) {
            var rowMax = 2, 
                rowLength = util.lengthJson(data), 
                rowLimit = (rowMax < rowLength ? rowMax : rowLength), 
                i = 0, 
                html = '<span class="start">{</span>';

            for (var key in data) {
                html += engineUtil.newItemSpacer(0, i, rowLimit, rowLength);
                if (i > rowLength || i++ > rowLimit)
                    break;
                html += '<span>\'</span>' + providers.string.build(key, level + 1, false, 12) + '<span>\'</span><span class="mspace">:</span><span>\'</span>' + providers.string.build(data[key], level + 1, false, 12) + '<span>\'</span>';
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
