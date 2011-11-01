pagingController = function () {
    var //Support 
        isLoading = false,
        wireListeners = function() { 
            pubsub.subscribe('action.plugin.active', function(subject, payload) { refresh(payload); }); 
        },
/*(import:glimpse.paging.engine.js|2)*/,
        refresh = function (key) {  
            var pagingInfo = data.currentMetadata().plugins[key].pagingInfo; 
            if (pagingInfo) {
                var panelItem = elements.findPanel(key);
                removePreviousPager(key, panelItem); 
                renderPager(key, panelItem, pagingInfo);
            }
        },
        removePreviousPager = function (key, panelItem) {
            var previousPager = panelItem.find('.glimpse-pager-' + key);
            previousPager.remove();
        },
        renderPager = function (key, panelItem, pagingInfo) {
            var pageIndex = parseInt(pagingInfo.pageIndex),
                pageIndexLast = Math.floor((parseInt(pagingInfo.totalNumberOfRecords) - 1) / parseInt(pagingInfo.pageSize));

            if (pageIndex <= pageIndexLast) { 
                var pagerContainer = $('<div class="glimpse-pager glimpse-pager-' + key + '"></div>'),
                    pagerType = pagingInfo.pagerType,
                    pagerEngine = pagingEngine.retrieve(pagerType);

                panelItem.append(pagerContainer);
                pagerEngine.render(key, pagerContainer, pagingInfo.pagerKey, pagerType, pageIndex, pageIndexLast);
            }
        }, 
        loadPage = function (key, pagerKey, pagerType, pageIndex) { 
            if (!isLoading) {
                isLoading = true; 
                showLoadingMessage(key);
                $.ajax({
                    url: glimpsePath + 'Pager',
                    type: 'GET',
                    data: { 'key': pagerKey, 'pageIndex': pageIndex },
                    contentType: 'application/json',
                    cache: false, 
                    success: function (data, textStatus, jqXHR) { 
                        loadPageData(key, pageIndex, pagerType, data);
                    },
                    complete: function (jqXHR, textStatus, errorThrown) {
                        isLoading = false;
                    }
                });
            }
        }, 
        loadPageData = function (key, pageIndex, pagerType, result) {
            var panelItem = elements.findPanel(key),
                pagerEngine = pagingEngine.retrieve(pagerType),
                metadata = data.currentMetadata().plugins[key],
                structure = metadata.structure,
                pagingInfo = metadata.pagingInfo;

            if (pagingInfo) 
                pagingInfo.pageIndex = pageIndex; 

            pagerEngine.loadPageData(panelItem, result, structure);

            refresh(key);
        },
        showLoadingMessage = function (key) {
            var panelItem = elements.findPanel(key),
                pager = panelItem.find('.glimpse-pager-' + key);

            pager.empty();
            pager.append('<span class="glimpse-pager-message">Loading...</span>');
        },
        
        //Main
        init = function () {    
            wireListeners();
        };

    init();
} ()