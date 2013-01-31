(function($, util, engine, engineUtil) {
    var providers = engine._providers,
        buildFormatString = function(content, data, indexs) {  
            for (var i = 0; i < indexs.length; i++) {
                var pattern = "\\\{\\\{" + indexs[i] + "\\\}\\\}", regex = new RegExp(pattern, "g"); 
                content = content.replace(regex, data[indexs[i]]);
            }
            return content;
        },
        buildCell = function(data, metadataItem, level, cellType, rowIndex, includeHeading) {
            var html = '', 
                cellContent = '', 
                cellClass = '', 
                cellStyle = '', 
                cellAttr = '',
                isHeadingRow = rowIndex == 0 && includeHeading;
                
            //Cell Content
            if ($.isArray(metadataItem.data)) {
                for (var i = 0; i < metadataItem.data.length; i++) 
                    cellContent += buildCell(data, metadataItem.data[i], level, 'div', rowIndex, includeHeading);
            }
            else { 
                if (!metadataItem.indexs && !$.isNumeric(metadataItem.data)) 
                    metadataItem.indexs = util.getTokens(metadataItem.data, data); 
                
                cellContent = metadataItem.indexs ? buildFormatString(metadataItem.data, data, metadataItem.indexs) : data[metadataItem.data];
                
                if (metadataItem.engine && !isHeadingRow) {
                    cellContent = providers.master.build(cellContent, level + 1, metadataItem.forceFull, metadataItem, isHeadingRow ? undefined : metadataItem.limit);
                }
                else {
                    //Get metadata for the new data 
                    var newMetadataItem = metadataItem.layout;
                    if ($.isPlainObject(newMetadataItem)) 
                        newMetadataItem = newMetadataItem[rowIndex];
                    if (newMetadataItem)
                        newMetadataItem = { layout: newMetadataItem };

                    //If minDisplay and we are in header or there is no data, we don't want to render anything 
                    if (metadataItem.minDisplay && (isHeadingRow || cellContent == undefined || cellContent == null))
                        return ""; 
                     
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



        build = function (data, level, forceFull, metadata, forceLimit) { 
            var limit = !$.isNumeric(forceLimit) ? 3 : forceLimit;

            if (engineUtil.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return buildPreview(data, level, metadata);
            return buildOnly(data, level, metadata);
        },
        buildOnly = function (data, level, metadata) {
            var html = '<table class="glimpse-row-holder">', 
                rowClass = '',
                layout = metadata.layout,
                includeHeading = engineUtil.includeHeading(metadata);
            for (var i = 0; i < data.length; i++) {
                rowClass = data[i].length > data[0].length ? (' ' + data[i][data[i].length - 1]) : '';
                html += (i == 0 && includeHeading) ? '<thead class="glimpse-row-header glimpse-row-header-' + level + '">' : '<tbody class="glimpse-row ' + (i % 2 ? 'odd' : 'even') + rowClass + '">';
                for (var x = 0; x < layout.length; x++) { 
                    var rowData = '';
                     
                    for (var y = 0; y < layout[x].length; y++) {
                        var metadataItem = layout[x][y], cellType = (i == 0 && includeHeading ? 'th' : 'td'); 
                        rowData += buildCell(data[i], metadataItem, level, cellType, i, includeHeading);
                    }
                     
                    if (rowData != '') { html += '<tr>' + rowData + '</tr>'; };
                }
                html += (i == 0) ? '</thead>' : '</tbody>';
            }
            html += '</table>'; 

            return html; 
        },
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
