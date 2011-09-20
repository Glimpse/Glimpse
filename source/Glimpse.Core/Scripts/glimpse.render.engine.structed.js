        structured = function () {
            var //Support
                buildCell = function(data, metadataItem, level, cellType, rowIndex) {
                    var that = this, html = '', cellContent = '', cellClass = '', cellStyle = '', cellAttr = '';
                
                    //Cell Content
                    if ($.isArray(metadataItem.data)) {
                        for (var i = 0; i < metadataItem.data.length; i++) 
                            cellContent += buildCell(data, metadataItem.data[i], level, 'div', rowIndex);
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

                //Main
                build = function (data, level, forceFull, metadata, forceLimit) { 
                    var that = this, limit = $.isNaN(forceLimit) ? 3 : forceLimit;
                    if (that.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                        return buildPreview(data, level, metadata);
            
                    var html = '<table>', rowClass = '';
                    for (var i = 0; i < data.length; i++) {
                        rowClass = data[i].length > data[0].length ? (' ' + data[i][data[i].length - 1]) : '';
                        html += (i == 0) ? '<thead class="glimpse-row-header-' + level + '">' : '<tbody class="' + (i % 2 ? 'odd' : 'even') + rowClass + '">';
                        for (var x = 0; x < metadata.length; x++) { 
                            var rowData = '';
                     
                            for (var y = 0; y < metadata[x].length; y++) {
                                var metadataItem = metadata[x][y], cellType = (i == 0 ? 'th' : 'td'); 
                                rowData += buildCell(data[i], metadataItem, level, cellType, i);
                            }
                     
                            if (rowData != '') { html += '<tr>' + rowData + '</tr>'; };
                        }
                        html += (i == 0) ? '</thead>' : '</tbody>';
                    }
                    html += '</table>'; 

                    return html;
                },
                buildPreview = function(data, level, metadata) {
                    var that = this;
                    return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + buildPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + build(data, level, true, metadata) + '</div></td></tr></table>';
                },
                buildPreviewOnly = function (data, level) { 
                    return that.buildCustomPreviewOnly(data, level);
                };
 
            return {
                build : build,
                buildPreview : buildPreview,
                buildPreviewOnly : buildPreviewOnly
            }; 
        } ()
