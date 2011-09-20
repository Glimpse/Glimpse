        shellController = function () {
            var //Support
            
                //Main
                build = function (data, level, forceFull, forceLimit) {  
                    if (data == undefined || data == null)
                        return '--';
                    if ($.isArray(data))
                        return '[ ... ]';
                    if ($.isPlainObject(data))
                        return '{ ... }';

                    var that = this, limit = $.isNaN(forceLimit) ? (level > 1 ? 80 : 150) : forceLimit;
                    if (that.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                        return buildPreview(data, level);
                
                    return $.glimpse.util.preserveWhitespace($.glimpseContent.formatString(data));
                }, 
                buildPreview = function (data, level, forceLimit) { 
                    if (data == undefined || data == null)
                        return '--';
                    if ($.isArray(data))
                        return '[ ... ]';
                    if ($.isPlainObject(data))
                        return '{ ... }';

                    return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td>' + buildPreviewOnly(data, level) + '<span class="glimpse-preview-show">' + build(data, level, true) + '</span></td></tr></table>';
                },
                buildPreview = function (data, level, forceLimit) {
                    if (data == undefined || data == null)
                        return '--';
                    if ($.isArray(data))
                        return '[ ... ]';
                    if ($.isPlainObject(data))
                        return '{ ... }';
                         
                    var that = this,
                        charMax = !$.isNaN(forceLimit) ? forceLimit : (level > 1 ? 80 : 150),
                        charOuterMax = (charMax * 1.2),
                        content = $.glimpseContent.trimFormatString(data, charMax, charOuterMax, true);

                    if (data.length > charOuterMax) {
                        content = '<span class="glimpse-preview-string" title="' + $.glimpseContent.trimFormatString(data, charMax * 2, charMax * 2.1, false, true) + '">' + content + '</span>';
                        if (charMax >= 15)
                            content = '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td>' + content + '<span class="glimpse-preview-show">' + $.glimpse.util.preserveWhitespace($.glimpseContent.formatString(data)) + '</span></td></tr></table>';
                    }
                    else 
                        content = $.glimpse.util.preserveWhitespace(content);  
              
                    return content;
                },
                buildPreviewOnly = function (data, level) {
                    return '<span class="glimpse-preview-string" title="' + $.glimpseContent.trimFormatString(data, charMax * 2, charMax * 2.1, false, true) + '">' + content + '</span>';
                };
        } ()


    $.extend($.glimpseProcessor, {
        build: function (data, level, forceFull, metadata, forceLimit) {
            var that = this, result = '';

            if ($.isArray(data)) {
                if (metadata)
                    result = that.buildStructuredTable(data, level, forceFull, metadata, forceLimit);
                else
                    result = that.buildCustomTable(data, level, forceFull, forceLimit);
            }
            else if ($.isPlainObject(data))
                result = that.buildKeyValueTable(data, level, forceFull, forceLimit);
            else if (level == 0) {
                if (data === undefined || data === null || data === '')
                    result = '';
                else {
                    var attr = '';
                    if (data.indexOf('http://') == 0) {
                        attr = ' data-glimpse-lazy-url="' + data + '"';
                        data = 'Loading data, please wait...';
                    } 
                    result = '<div class="glimpse-panel-message"' + attr + '>' + data + '</div>';
                }
            }
            else
                result = that.buildString(data, level, forceLimit);

            return result;
        },
        buildHeading: function (url, clientName, type) {
            var clean = function(data) {
                return (data === undefined || data === null || data === "null") ? '' : data;
            };
            type = clean(type);
            clientName = clean(clientName);
            return '<span class="glimpse-snapshot-type" data-clientName="' + clientName + '">' + clientName + ((type.length > 0) ? ' (' + type + ')' : '') + '&nbsp;</span><span><span class="glimpse-enviro"></span><span class="glimpse-url">' + url + '</span></span>';
        },
        buildKeyValueTable: function (data, level, forceFull, forceLimit) {  
            var that = this, limit = $.isNaN(forceLimit) ? 3 : forceLimit;
            if (that.shouldUsePreview($.glimpse.util.lengthJson(data), level, forceFull, limit, forceLimit, 1))
                return that.buildKeyValuePreview(data, level);
                
            var i = 1, html = '<table><thead><tr class="glimpse-row-header-' + level + '"><th class="glimpse-cell-key">Key</th><th class="glimpse-cell-value">Value</th></tr></thead>';
            for (var key in data)
                html += '<tr class="' + (i++ % 2 ? 'odd' : 'even') + '"><th width="30%">' + $.glimpseContent.formatString(key) + '</th><td width="70%"> ' + that.build(data[key], level + 1) + '</td></tr>';
            html += '</table>';
            return html;
        },
        buildCustomTable: function (data, level, forceFull, forceLimit) { 
            var that = this, limit = $.isNaN(forceLimit) ? 3 : forceLimit;
            if (that.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return that.buildCustomPreview(data, level);

            var html = '<table><thead><tr class="glimpse-row-header-' + level + '">';
            if ($.isArray(data[0])) {
                for (var x = 0; x < data[0].length; x++)
                    html += '<th>' + $.glimpseContent.formatString(data[0][x]) + '</th>';
                html += '</tr></thead>';
                for (var i = 1; i < data.length; i++) {
                    html += '<tr class="' + (i % 2 ? 'odd' : 'even') + (data[i].length > data[0].length ? ' ' + data[i][data[i].length - 1] : '') + '">';
                    for (var x = 0; x < data[0].length; x++)
                        html += '<td>' + that.build(data[i][x], level + 1) + '</td>';
                    html += '</tr>';
                }
                html += '</table>';
            }
            else {
                if (data.length > 1) {
                    html += '<th>Values</th></tr></thead>';
                    for (var i = 0; i < data.length; i++)
                        html += '<tr class="' + (i % 2 ? 'odd' : 'even') + '"><td>' + that.build(data[i], level + 1) + '</td></tr>';
                    html += '</table>';
                }
                else
                    html = that.build(data[0], level + 1);
            }
            return html;
        },
        buildStructuredTable: function (data, level, forceFull, metadata, forceLimit) { 
            var that = this, limit = $.isNaN(forceLimit) ? 3 : forceLimit;
            if (that.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return that.buildStructuredTablePreview(data, level, metadata);
            
            var html = '<table>', rowClass = '';
            for (var i = 0; i < data.length; i++) {
                rowClass = data[i].length > data[0].length ? (' ' + data[i][data[i].length - 1]) : '';
                html += (i == 0) ? '<thead class="glimpse-row-header-' + level + '">' : '<tbody class="' + (i % 2 ? 'odd' : 'even') + rowClass + '">';
                for (var x = 0; x < metadata.length; x++) { 
                    var rowData = '';
                     
                    for (var y = 0; y < metadata[x].length; y++) {
                        var metadataItem = metadata[x][y], cellType = (i == 0 ? 'th' : 'td'); 
                        rowData += that.buildStructuredTableCell(data[i], metadataItem, level, cellType, i);
                    }
                     
                    if (rowData != '') { html += '<tr>' + rowData + '</tr>'; };
                }
                html += (i == 0) ? '</thead>' : '</tbody>';
            }
            html += '</table>'; 

            return html;
        },
        buildStructuredTableCell : function(data, metadataItem, level, cellType, rowIndex) {
            var that = this, html = '', cellContent = '', cellClass = '', cellStyle = '', cellAttr = '';
                
            //Cell Content
            if ($.isArray(metadataItem.data)) {
                for (var i = 0; i < metadataItem.data.length; i++) 
                    cellContent += that.buildStructuredTableCell(data, metadataItem.data[i], level, 'div', rowIndex);
            }
            else { 
                if (!metadataItem.indexs && $.isNaN(metadataItem.data)) 
                    metadataItem.indexs = $.glimpse.util.getTokens(metadataItem.data, data); 
                
                //Get metadata for the new data 
                var newMetadataItem = metadataItem.structure;
                if ($.isPlainObject(newMetadataItem)) 
                    newMetadataItem = newMetadataItem[rowIndex];
                    
                cellContent = metadataItem.indexs ? that.buildFormatString(metadataItem.data, data, metadataItem.indexs) : data[metadataItem.data];
                
                //If minDisplay and we are in header or there is no data, we don't want to render anything 
                if (metadataItem.minDisplay && (rowIndex == 0 || cellContent == undefined || cellContent == null))
                    return ""; 
                     
                cellContent = that.build(cellContent, level + 1, metadataItem.forceFull, newMetadataItem, rowIndex == 0 ? undefined : metadataItem.limit);

                //Content pre/post
                if (rowIndex != 0) {
                    if (metadataItem.pre) { cellContent = '<span class="glimpse-soft">' + metadataItem.pre + '</span>' + cellContent; }
                    if (metadataItem.post) { cellContent = cellContent + '<span class="glimpse-soft">' + metadataItem.post + '</span>'; }
                }
            }
            
            if (rowIndex != 0) {
                cellClass = 'glimpse-cell';
                //Cell Class
                if (metadataItem.key === true) { cellClass += ' glimpse-cell-key'; }
                if (metadataItem.isCode === true) { cellClass += ' glimpse-code'; }
                if (metadataItem.className) { cellClass += ' ' + metadataItem.className; }
                //Cell Code 
                if (metadataItem.codeType) { cellAttr += ' data-codeType="' + metadataItem.codeType + '"'; };
            }
            if (cellClass) { cellAttr += ' class="' + cellClass + '"'; }; 
            //Cell Style  
            if (metadataItem.width) { cellStyle += 'width:' + metadataItem.width + ';'; };
            if (metadataItem.align) { cellStyle += 'text-align:' + metadataItem.align + ';'; };
            if (cellStyle) { cellAttr += ' style="' + cellStyle + '"'; };
            //Cell Span
            if (metadataItem.span) { cellAttr += ' colspan="' + metadataItem.span + '"'; };
             
            html += '<' + cellType + cellAttr + '>' + cellContent + '</' + cellType + '>';
            
            return html;
        }, 
        buildKeyValuePreview: function (data, level) {
            var that = this;
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + that.buildKeyValuePreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + that.buildKeyValueTable(data, level, true) + '</div></td></tr></table>';
        },
        buildKeyValuePreviewOnly: function (data, level) {
            var that = this, length = $.glimpse.util.lengthJson(data), rowMax = 2, rowLimit = (rowMax < length ? rowMax : length), i = 1, html = '<span class="start">{</span>';

            for (var key in data) {
                html += that.newItemSpacer(i, rowLimit, length);
                if (i > length || i++ > rowLimit)
                    break;
                html += '<span>\'</span>' + that.buildStringPreview(key, level + 1) + '<span>\'</span><span class="mspace">:</span><span>\'</span>' + that.buildStringPreview(data[key], level, 12) + '<span>\'</span>';
            }
            html += '<span class="end">}</span>';

            return html;
        },
        buildCustomPreview: function (data, level) {
            var that = this, isComplex = ($.isArray(data[0]) || $.isPlainObject(data[0]));
            
            if (isComplex && data.length == 1)
                return that.build(data[0], level);
            if (isComplex || data.length > 1) 
                return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + that.buildCustomPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + that.buildCustomTable(data, level, true) + '</div></td></tr></table>';
            return that.buildStringPreview(data[0], level + 1); 
        },
        buildCustomPreviewOnly: function (data, level) { 
            var that = this, isComplex = $.isArray(data[0]), length = (isComplex ? data.length - 1 : data.length), rowMax = 2, columnMax = 3, columnLimit = 1, rowLimit = (rowMax < length ? rowMax : length), html = '<span class="start">[</span>';

            if (isComplex) {
                columnLimit = ((data[0].length > columnMax) ? columnMax : data[0].length);
                for (var i = 1; i <= rowLimit + 1; i++) {
                    html += that.newItemSpacer(i, rowLimit, length);
                    if (i > length || i > rowLimit)
                        break;

                    html += '<span class="start">[</span>';
                    var spacer = '';
                    for (var x = 0; x < columnLimit; x++) {
                        html += spacer + '<span>\'</span>' + that.buildStringPreview(data[i][x], level, 12) + '<span>\'</span>';
                        spacer = '<span class="rspace">,</span>';
                    }
                    if (x < data[0].length)
                        html += spacer + '<span>...</span>';
                    html += '<span class="end">]</span>';
                }
            }
            else { 
                for (var i = 0; i <= rowLimit; i++) {
                    html += that.newItemSpacer(i + 1, rowLimit, length);
                    if (i >= length || i >= rowLimit)
                        break;
                    html += '<span>\'</span>' + that.buildStringPreview(data[i], level, 12) + '<span>\'</span>';
                } 
            }

            html += '<span class="end">]</span>';

            return html;
        },
        buildStructuredTablePreview : function(data, level, metadata) {
            var that = this;
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + that.buildCustomPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + that.buildStructuredTable(data, level, true, metadata) + '</div></td></tr></table>';
        },
        //TODO: these needs to be moved out 
        shouldUsePreview: function(length, level, forceFull, limit, forceLimit, tolerance) {
            if (!$.isNaN(forceLimit)) { limit = forceLimit;}
            return !forceFull && ((level == 1 && length > (limit + tolerance)) || (level > 1 && (!forceLimit || length > (limit + tolerance))));
        }, 
        buildFormatString : function(formatString, data, indexs) { //TODO: this needs to be moved out 
            for (var i = 0; i < indexs.length; i++) {
                var pattern = "\\\{\\\{" + indexs[i] + "\\\}\\\}", regex = new RegExp(pattern, "g"); 
                formatString = formatString.replace(regex, data[indexs[i]]);
            }
            return formatString;
        },
        newItemSpacer: function (currentRow, rowLimit, dataLength) { 
            var html = '';
            if (currentRow > 1 && (currentRow <= rowLimit || dataLength > rowLimit)) { html += '<span class="rspace">,</span>'; }
            if (currentRow > rowLimit && dataLength > rowLimit) { html += '<span class="small">length=' + dataLength + '</span>'; }
            return html;
        }
    }); 