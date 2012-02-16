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
        loadPageData: function (panelItem, data, structure) {
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

                var lastPageTop = firstPageRowSeparator.offset().top - panelItem.offset().top;
                panelItem.animate({ scrollTop: '+=' + lastPageTop + 'px' }, 500);
            }
        }
    };
} ()