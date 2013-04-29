glimpse.paging.engine.util = (function($, pubsub, data, elements, util, renderEngine) {
    var generatePagingAddress = function(pagerKey, pageIndex) {
            return util.uriTemplate(data.currentMetadata().resources.glimpse_paging, { 'key': pagerKey, 'pageIndex': pageIndex });
        },
        requestStart = function(key) {
            var pager = elements.panel(key).find('.glimpse-pager');

            pager.html('<span class="glimpse-pager-message">Loading...</span>');
        },
        requestSuccess = function(key, pagerKey, pageIndex, method, result) {  
            var pagingInfo = data.currentMetadata().plugins[key].pagingInfo,
                content = renderEngine.build(result, data.currentMetadata().plugins[key].structure),
                scope = renderMethod[method](elements.panel(key), content);
            
            pagingInfo.pageIndex = pageIndex; 
            
            pubsub.publish('trigger.panel.render.style', { scope: scope });
            pubsub.publish('trigger.tab.paging.refresh', { key: key });
        }, 
        renderMethod = {
            insert: function(panelItem, content) {
                panelItem.find('> table').remove();
                
                return $(content).appendTo(panelItem); 
            },
            append: function (panelItem, content) {
                var table = panelItem.find('> table'),
                    nodes = $(content.substring(content.indexOf('>') + 1, content.lastIndexOf('<')))[1];

                table.find('> tbody:last-child > tr:last-child').addClass('glimpse-pager-separator');
                
                return $(nodes).appendTo(table); 
            }
        };

    return {   
        load: (function() {
            var isLoading = false;

            return function(key, pagerKey, pageIndex, method) {
                    if (!isLoading) {
                        isLoading = true; 
                
                        requestStart(key);
                
                        $.ajax({
                            url: generatePagingAddress(pagerKey, pageIndex),
                            type: 'GET', 
                            contentType: 'application/json', 
                            success: function(result) { 
                                requestSuccess(key, pagerKey, pageIndex, method, result);
                            },
                            complete: function () {
                                isLoading = false;
                            }
                        });
                    }
                };
        })()
    };
})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements, glimpse.util, glimpse.render.engine); 