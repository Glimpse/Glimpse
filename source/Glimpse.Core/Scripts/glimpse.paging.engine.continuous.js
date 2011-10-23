continuous = function () {
    return {
        match: function (pagerType) {
            return pagerType == 1;
        },
        render: function (key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast) {
            var pagerMessage = $('<span class="glimpse-pager-message"></span');
            pagerMessage.html('Showing page 1 until page ' + (pageIndex + 1) + ' from a total of ' + (pageIndexLast + 1) + ' pages.');
            pagerContainer.append(pagerMessage);

            if (pageIndex < pageIndexLast) {
                var pagerNextPageLink = $('<a href="#" class="glimpse-pager-link">Load the next page</a>');
                pagerNextPageLink.one('click', function () { loadPage(key, pagerKey, pagerType, pageIndex + 1); return false; });
                pagerContainer.append(pagerNextPageLink);
            }
        },
        loadPageData: function (panelItem, data) {
            var content = renderEngine.build(data, null);
            panelItem.append(content);

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