(function($, util, engine, engineUtil) {
    var providers = engine._providers,

        findFactory = function(data) {
            var match = null;
            for (var key in factories) {
                if (factories[key].isHandled(data)) {
                    match = factories[key];
                    break;
                }
            }
            return match;
        },
        factories = {
            array: {
                isHandled: function(data) {
                    return $.isArray(data[0]);
                },
                getHeader: function(data) {
                    return data[0];
                },
                getRowClass: function(data, rowIndex) {
                    return data[rowIndex].length > data[0].length ? ' ' + data[rowIndex][data[rowIndex].length - 1] : '';
                },
                getRowValue: function(dataRow, fieldIndex, header) {
                    return dataRow[fieldIndex];
                }, 
                startingIndex: function() {
                    return 1;
                }
            },
            object: {
                isHandled: function(data) {
                    return data[0] === Object(data[0]);
                },
                getHeader: function(data) { 
                    var result = [];
                    for (var key in data[0]) {
                        if (key != "_metadata") 
                            result.push(key);
                    } 
                    return result; 
                },
                getRowClass: function(data, rowIndex) {
                    return data[rowIndex]._metadata && data[rowIndex]._metadata.style ? ' ' + data[rowIndex]._metadata.style : ''; 
                },
                getRowValue: function(dataRow, fieldIndex, header) {
                    return dataRow[header[fieldIndex]];
                }, 
                startingIndex: function() {
                    return 0;
                }
            } 
        },

        buildFormatString = function(content, data, indexs, isHeadingRow) {  
            for (var i = 0; i < indexs.length; i++) {
                var pattern = "\\\{\\\{" + indexs[i] + "\\\}\\\}", regex = new RegExp(pattern, "g"),
                    value = isHeadingRow && !$.isNumeric(indexs[i]) ? indexs[i] : data[indexs[i]]; 
                content = content.replace(regex, value);
            }
            return content;
        },
        
        buildCell = function(data, metadataItem, level, cellType, rowIndex, isHeadingRow) {
            var html = '', 
                cellContent = '', 
                cellClass = '', 
                cellStyle = '', 
                cellAttr = '';
                
            //Cell Content
            if ($.isArray(metadataItem.data)) {
                for (var i = 0; i < metadataItem.data.length; i++) 
                    cellContent += buildCell(data, metadataItem.data[i], level, 'div', rowIndex, isHeadingRow);
            }
            else { 
                if (!metadataItem.indexs && util.containsTokens(metadataItem.data)) 
                    metadataItem.indexs = util.getTokens(metadataItem.data, data); 
                  
                cellContent = metadataItem.indexs ? buildFormatString(metadataItem.data, data, metadataItem.indexs, isHeadingRow) : (isHeadingRow && !$.isNumeric(metadataItem.data) ? metadataItem.data : data[metadataItem.data]);
                
                if (metadataItem.engine && !isHeadingRow) {
                    cellContent = providers.master.build(cellContent, level + 1, metadataItem.forceFull, metadataItem, isHeadingRow ? undefined : metadataItem.limit);
                }
                else {
                    //Get metadata for the new data 
                    var newMetadataItem = metadataItem.layout;
                    if ($.isPlainObject(newMetadataItem)) 
                        newMetadataItem = newMetadataItem[rowIndex];
                    if (newMetadataItem || metadataItem.suppressHeader)
                        newMetadataItem = { layout: newMetadataItem, suppressHeader: metadataItem.suppressHeader };

                    //If minDisplay and we are in header or there is no data, we don't want to render anything 
                    if (metadataItem.minDisplay && (isHeadingRow || cellContent == null))
                        return ""; 
                    
                    //Work out what title we want
                    if (isHeadingRow && metadataItem.title) 
                        cellContent = metadataItem.title;

                    cellContent = providers.master.build(cellContent, level + 1, metadataItem.forceFull, newMetadataItem, isHeadingRow ? undefined : metadataItem.limit);

                    //Content pre/post
                    if (!isHeadingRow) {
                        if (metadataItem.pre) { cellContent = '<span class="glimpse-soft">' + metadataItem.pre + '</span>' + cellContent; }
                        if (metadataItem.post) { cellContent = cellContent + '<span class="glimpse-soft">' + metadataItem.post + '</span>'; }
                    }
                }
            }
            
            if (!isHeadingRow) {
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
        buildCellRow = function (data, layout, level, cellType, rowIndex, isHeadingRow) {
            var html = '';
            
            for (var x = 0; x < layout.length; x++) { 
                var rowHtml = ''; 
                for (var y = 0; y < layout[x].length; y++) {
                    var metadataItem = layout[x][y];  
                    rowHtml += buildCell(data, metadataItem, level, cellType, rowIndex, isHeadingRow);
                }
                     
                if (rowHtml != '') { html += '<tr>' + rowHtml + '</tr>'; };
            }
            return html;
        },


        build = function (data, level, forceFull, metadata, forceLimit) { 
            var limit = !$.isNumeric(forceLimit) ? 3 : forceLimit;

            if (engineUtil.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return buildPreview(data, level, metadata);
            return buildOnly(data, level, metadata);
        },
        buildOnly = function (data, level, metadata) {
            var html = '<table class="glimpse-row-holder">', 
                layout = metadata.layout,
                factory = findFactory(data),
                headers = factory.getHeader(data); 
            
            if (engineUtil.includeHeading(metadata)) {
                html += '<thead class="glimpse-row-header glimpse-row-header-' + level + '">';
                html += buildCellRow(headers, layout, level, 'th', 0, true);
                html += '</thead>';
            } 
            for (var i = factory.startingIndex(); i < data.length; i++) {
                html += '<tbody class="glimpse-row' + factory.getRowClass(data, i) + '">';
                html += buildCellRow(data[i], layout, level, 'td', i, false);
                html += '</tbody>';
            }
            html += '</table>';
            return html;
        },
        /* 
        buildOnly = function (data, level, metadata) {
            var html = '<table class="glimpse-row-holder">', 
                rowClass = '',
                layout = metadata.layout,
                includeHeading = engineUtil.includeHeading(metadata);
            for (var i = includeHeading ? 0 : 1; i < data.length; i++) {
                rowClass = data[i].length > data[0].length ? (' ' + data[i][data[i].length - 1]) : ''; 
                html += (i == 0 && includeHeading) ? '<thead class="glimpse-row-header glimpse-row-header-' + level + '">' : '<tbody class="glimpse-row">'; 
                for (var x = 0; x < layout.length; x++) { 
                    var rowData = '';
                     
                    for (var y = 0; y < layout[x].length; y++) {
                        var metadataItem = layout[x][y], cellType = (i == 0 && includeHeading ? 'th' : 'td'); 
                        rowData += buildCell(data[i], metadataItem, level, cellType, i, includeHeading);
                    }
                     
                    if (rowData != '') { html += '<tr class="' + rowClass + '">' + rowData + '</tr>'; };
                }
                html += (i == 0 && includeHeading) ? '</thead>' : '</tbody>'; 
            }
            html += '</table>';

            return html; 
        },
        */
        buildPreview = function(data, level, metadata) { 
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + buildPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + buildOnly(data, level, metadata) + '</div></td></tr></table>';
        },
        buildPreviewOnly = function (data, level) { 
            return providers.table.buildPreviewOnly(data, level);
        },
        provider = {
            build : build,
            buildOnly : buildOnly,
            buildPreview : buildPreview,
            buildPreviewOnly : buildPreviewOnly
        }; 

    engine.register('layout', provider);
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util);
