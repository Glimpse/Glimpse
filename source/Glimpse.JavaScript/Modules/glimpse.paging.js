glimpse.paging = (function($, pubsub, util, data, elements) {
    var process = function(args) {
            var key = args.key,
                pagingInfo = data.currentMetadata().plugins[key].pagingInfo,
                panelItem = elements.panel(key); 
        
            if (pagingInfo) {
                panelItem.find('.glimpse-pager').remove();

                var pageIndex = pagingInfo.pageIndex,
                    pageIndexLast = Math.floor((pagingInfo.totalNumberOfRecords - 1) / pagingInfo.pageSize),
                    pagerContainer = $('<div class="glimpse-pager"></div>').appendTo(panelItem); 

                pubsub.publish('trigger.paging.controls.render', { key: key, pagerContainer: pagerContainer, pagerKey: pagingInfo.pagerKey, pagerType: pagingInfo.pagerType, pageIndex: pageIndex, pageIndexLast: pageIndexLast });
            }
    };
    
    pubsub.subscribe('trigger.tab.select', process);  
    pubsub.subscribe('trigger.tab.paging.refresh', process); 

    return {};
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.data, glimpse.elements); 