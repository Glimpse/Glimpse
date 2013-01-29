glimpse.render.engine.util = (function($) {
    return {
        keyMetadata: function (key, metadata) {
            return metadata && metadata.layout === Object(metadata.layout) ? metadata.layout[key] : null;
        },
        shouldUsePreview: function(length, level, forceFull, limit, forceLimit, tolerance) {
            if ($.isNumeric(forceLimit))
                limit = forceLimit;
            return !forceFull && ((level == 1 && length > (limit + tolerance)) || (level > 1 && (!forceLimit || length > (limit + tolerance))));
        },
        newItemSpacer: function(currentRow, rowLimit, dataLength) {
            var html = '';
            if (currentRow > 1 && (currentRow <= rowLimit || dataLength > rowLimit))
                html += '<span class="rspace">,</span>';
            if (currentRow > rowLimit && dataLength > rowLimit)
                html += '<span class="small">length=' + dataLength + '</span>';
            return html;
        }          
    };
})(jQueryGlimpse); 