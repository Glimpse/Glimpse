continuous = function () {
    return {
        match: function (pagerType) {
            return pagerType == 1;
        },
        render: function (key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast) {
            var pagerMessage = $('<span class="glimpse-pager-message">Showing ' + (pageIndex + 1) + ' page(s) of ' + (pageIndexLast + 1) + ' pages(s).</span>');
            pagerContainer.append(pagerMessage);

            if (pageIndex < pageIndexLast) {
                var pagerNextPageLink = $('<a href="#" class="glimpse-pager-link">More</a>');
                pagerNextPageLink.one('click', function () { loadPage(key, pagerKey, pagerType, pageIndex + 1); return false; });
                pagerContainer.append(pagerNextPageLink);
            }
        },
        loadPageData: function (panelItem, data) { 
            panelItem.append(renderEngine.build(data, null));

            //TODO: Not sure if this will work with needed tables
            var pages = panelItem.find('table');
            pages.not(':first').find('thead').remove();
            pages.not(':last').addClass('glimpse-pager-separator');

            var lastPage = panelItem.find('table:last');
            if (lastPage.length > 0) {
                var lastPageTop = lastPage.offset().top - panelItem.offset().top;
                panelItem.animate({ scrollTop: '+=' + lastPageTop + 'px' }, 500);
            }
        }
    };
} ()