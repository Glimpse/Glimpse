(function($, util, engine, engineUtil, engineUtilTable) {
    var providers = engine._providers, 
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
                cellAttr = '',
                containsNestedData = $.isArray(metadataItem.data);
                
            //Cell Content
            if (containsNestedData) {
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
                if (!containsNestedData) { cellClass = 'glimpse-cell'; }
                
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
                factory = engineUtilTable.findFactory(data),
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
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util, glimpse.render.engine.util.table);
