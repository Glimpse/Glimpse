glimpse.paging = (function($, pubsub, util, data, elements) {
    var process = function(args) {
            var key = args.key,
                metadata = data.currentMetadata().plugins[key];
        
            if (metadata && (metadata = metadata.pagingInfo)) {
                var panelItem = elements.panel(key); 
                
                panelItem.find('.glimpse-pager').remove();

                var pageIndex = metadata.pageIndex,
                    pageIndexLast = Math.floor((metadata.totalNumberOfRecords - 1) / metadata.pageSize),
                    pagerContainer = $('<div class="glimpse-pager"></div>').appendTo(panelItem); 

                pubsub.publish('trigger.paging.controls.render', { key: key, pagerContainer: pagerContainer, pagerKey: metadata.pagerKey, pagerType: metadata.pagerType, pageIndex: pageIndex, pageIndexLast: pageIndexLast });
            }
        };
    
    pubsub.subscribe('trigger.tab.select', process);  
    pubsub.subscribe('trigger.tab.paging.refresh', process); 

    return {};
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.data, glimpse.elements); 