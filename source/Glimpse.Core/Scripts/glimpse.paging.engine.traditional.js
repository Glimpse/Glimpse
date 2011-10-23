traditional = function () {
    return {
        match: function (pagerType) {
            return pagerType == 0;
        },
        render: function (key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast) {
            var pagerFirstPageLink = $('<a href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-firstPage"></a>');
            var pagerPreviousPageLink = $('<a href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-previousPage"></a>');
            pagerContainer.append(pagerFirstPageLink);
            pagerContainer.append(pagerPreviousPageLink);

            var pagerMessage = $('<span class="glimpse-pager-message"></span');
            pagerMessage.html((pageIndex + 1) + ' / ' + (pageIndexLast + 1));
            pagerContainer.append(pagerMessage);

            var pagerNextPageLink = $('<a href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-nextPage"></a>');
            var pagerLastPageLink = $('<a href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-lastPage"></a>');
            pagerContainer.append(pagerNextPageLink);
            pagerContainer.append(pagerLastPageLink);

            if (pageIndex > 0) {
                pagerFirstPageLink.one('click', function () { loadPage(key, pagerKey, pagerType, 0); return false; });
                pagerPreviousPageLink.one('click', function () { loadPage(key, pagerKey, pagerType, pageIndex - 1); return false; });
            } else {
                pagerFirstPageLink.addClass('glimpse-pager-link-firstPage-disabled');
                pagerPreviousPageLink.addClass('glimpse-pager-link-previousPage-disabled');
            }

            if (pageIndex < pageIndexLast) {
                pagerNextPageLink.one('click', function () { loadPage(key, pagerKey, pagerType, pageIndex + 1); return false; });
                pagerLastPageLink.one('click', function () { loadPage(key, pagerKey, pagerType, pageIndexLast); return false; });
            } else {
                pagerNextPageLink.addClass('glimpse-pager-link-nextPage-disabled');
                pagerLastPageLink.addClass('glimpse-pager-link-lastPage-disabled');
            }
        },
        loadPageData: function (panelItem, data, structure) {
            var content = renderEngine.build(data, structure);
            panelItem.html(content);
        }
    };
} ()