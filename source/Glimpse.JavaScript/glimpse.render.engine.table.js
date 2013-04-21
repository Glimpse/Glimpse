(function($, util, engine, engineUtil, engineUtilTable) {
    var providers = engine._providers,
        build = function (data, level, forceFull, metadata, forceLimit) { 
            var limit = !$.isNumeric(forceLimit) ? 3 : forceLimit;

            if (engineUtil.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return buildPreview(data, level);
            return buildOnly(data, level, metadata);
        },
        buildOnly = function (data, level, metadata) {
            var html = '<table>', 
                factory = engineUtilTable.findFactory(data),
                headers = factory.getHeader(data); 
            
            if (engineUtil.includeHeading(metadata)) {
                html += '<thead><tr class="glimpse-row-header glimpse-row-header-' + level + '">';
                for (var x = 0; x < headers.length; x++)
                    html += '<th>' + engineUtil.raw.process(headers[x]) + '</th>';
                html += '</tr></thead>';
            }
            html += '<tbody class="glimpse-row-holder">';
            for (var i = factory.startingIndex(); i < data.length; i++) {
                html += '<tr class="glimpse-row' + factory.getRowClass(data, i) + '">';
                for (var x = 0; x < headers.length; x++)
                    html += '<td>' + providers.master.build(factory.getRowValue(data[i], x, headers), level + 1) + '</td>';
                html += '</tr>';
            }
            html += '</tbody></table>';
            return html;
        },
        buildPreview = function (data, level) {
            var isComplex = ($.isArray(data[0]) || $.isPlainObject(data[0]));
            
            if (isComplex && data.length == 1) //This exists to simplify visual layout if we only have one item 
                return providers.master.build(data[0], level);
            if (isComplex || data.length > 1) 
                return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + buildPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + buildOnly(data, level) + '</div></td></tr></table>';
            return providers.string.build(data[0], level + 1); 
        },
        buildPreviewOnly = function (data, level) { 
            var html = '<span class="start">[</span>', 
                factory = engineUtilTable.findFactory(data),
                headers = factory.getHeader(data),
                startingIndex = factory.startingIndex(),
                columnMax = 3, 
                columnLength = headers.length,
                columnLimit = columnMax < columnLength ? columnMax : columnLength, 
                rowMax = 2 + startingIndex, 
                rowLength = data.length - startingIndex,
                rowLimit = rowMax < rowLength ? rowMax : rowLength + startingIndex; 
            
            for (var i = startingIndex; i < rowLimit; i++) { 
                html += engineUtil.newItemSpacer(startingIndex, i, rowLimit, rowLength);
                if (headers.length > 1)
                    html += '<span class="start">[</span>';
                var spacer = '';
                for (var x = 0; x < columnLimit; x++) {
                    html += spacer + '<span>\'</span>' + providers.string.build(factory.getRowValue(data[i], x, headers), level + 1, false, 12) + '<span>\'</span>';
                    spacer = '<span class="rspace">,</span>';
                }
                if (headers.length > 1) {
                    if (x < headers.length)
                        html += spacer + '<span>...</span>';
                    html += '<span class="end">]</span>';
                }
            }
            html += engineUtil.newItemSpacer(startingIndex, ++i, rowLimit, rowLength);

            html += '<span class="end">]</span>';

            return html;
        },
        provider = {
            build : build,
            buildOnly : buildOnly,
            buildPreview : buildPreview,
            buildPreviewOnly : buildPreviewOnly
        }; 

    engine.register('table', provider);
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util, glimpse.render.engine.util.table);