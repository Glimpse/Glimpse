scrolling = function () {
    return {
        match : function (pagerType) {
            return pagerType == 2;
        },
        render : function (key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast) {
            var pagerMessage = $('<span class="glimpse-pager-message"></span');
            pagerMessage.html('Showing page 1 until page ' + (pageIndex + 1) + ' from a total of ' + (pageIndexLast + 1) + ' pages.');
            pagerContainer.append(pagerMessage);
                    
            if (pageIndex < pageIndexLast) { 
                var panelItem = elements.findPanel(key);
                if (panelItem.length > 0) {
                    if (panelItem[0].clientHeight >= panelItem.find(':last').position().top) {
                        loadPage(key, pagerKey, pagerType, pageIndex + 1);
                    } else {
                        var scrollingCallback = function () {
                            if (panelItem[0].clientHeight >= panelItem.find(':last').position().top) {
                                loadPage(key, pagerKey, pagerType, pageIndex + 1);
                                panelItem.unbind('scroll');
                            }
                        }; 
                        panelItem.bind('scroll', scrollingCallback);
                    }
                }
            }
        },
        loadPageData : function (panelItem, data, structure) {
            var content = renderEngine.build(data, structure);
            panelItem.append(content);

            var firstPage = panelItem.find('table:first');
            var lastPage = panelItem.find('table:last');
            if (firstPage.length > 0 && lastPage.length > 0) {
                var firstPageRowSeparator = firstPage.find('tr:last');
                firstPageRowSeparator.addClass('glimpse-pager-separator');

                var lastPageRows = lastPage.find('tbody tr');
                $.each(lastPageRows, function (index, row) {
                    firstPage.append($(row).clone());
                });

                lastPage.remove();
            }
        }
    };
} ()