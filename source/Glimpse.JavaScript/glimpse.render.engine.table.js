table = function () {
    var //Main
        build = function (data, level, forceFull, forceLimit) { 
            var limit = !$.isNumeric(forceLimit) ? 3 : forceLimit;

            if (shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return buildPreview(data, level);

            var html = '<table><thead><tr class="glimpse-row-header-' + level + '">';
            if ($.isArray(data[0])) {
                for (var x = 0; x < data[0].length; x++)
                    html += '<th>' + rawString.process(data[0][x]) + '</th>';
                html += '</tr></thead>';
                for (var i = 1; i < data.length; i++) {
                    html += '<tr class="' + (i % 2 ? 'odd' : 'even') + (data[i].length > data[0].length ? ' ' + data[i][data[i].length - 1] : '') + '">';
                    for (var x = 0; x < data[0].length; x++)
                        html += '<td>' + master.build(data[i][x], level + 1) + '</td>';
                    html += '</tr>';
                }
                html += '</table>';
            }
            else {
                if (data.length > 1) {
                    html += '<th>Values</th></tr></thead>';
                    for (var i = 0; i < data.length; i++)
                        html += '<tr class="' + (i % 2 ? 'odd' : 'even') + '"><td>' + master.build(data[i], level + 1) + '</td></tr>';
                    html += '</table>';
                }
                else
                    html = master.build(data[0], level + 1);
            }
            return html;
        },
        buildPreview = function (data, level) {
            var isComplex = ($.isArray(data[0]) || $.isPlainObject(data[0]));
            
            if (isComplex && data.length == 1)
                return master.build(data[0], level);
            if (isComplex || data.length > 1) 
                return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + buildPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + build(data, level, true) + '</div></td></tr></table>';
            return string.build(data[0], level + 1); 
        },
        buildPreviewOnly = function (data, level) { 
            var isComplex = $.isArray(data[0]), 
                length = (isComplex ? data.length - 1 : data.length), 
                rowMax = 2, 
                columnMax = 3, 
                columnLimit = 1, 
                rowLimit = (rowMax < length ? rowMax : length), 
                html = '<span class="start">[</span>';

            if (isComplex) {
                columnLimit = ((data[0].length > columnMax) ? columnMax : data[0].length);
                for (var i = 1; i <= rowLimit + 1; i++) {
                    html += newItemSpacer(i, rowLimit, length);
                    if (i > length || i > rowLimit)
                        break;

                    html += '<span class="start">[</span>';
                    var spacer = '';
                    for (var x = 0; x < columnLimit; x++) {
                        html += spacer + '<span>\'</span>' + string.build(data[i][x], level, 12) + '<span>\'</span>';
                        spacer = '<span class="rspace">,</span>';
                    }
                    if (x < data[0].length)
                        html += spacer + '<span>...</span>';
                    html += '<span class="end">]</span>';
                }
            }
            else { 
                for (var i = 0; i <= rowLimit; i++) {
                    html += newItemSpacer(i + 1, rowLimit, length);
                    if (i >= length || i >= rowLimit)
                        break;
                    html += '<span>\'</span>' + string.build(data[i], level, 12) + '<span>\'</span>';
                } 
            }

            html += '<span class="end">]</span>';

            return html;
        };
 
    return {
        build : build,
        buildPreview : buildPreview,
        buildPreviewOnly : buildPreviewOnly
    }; 
} ()