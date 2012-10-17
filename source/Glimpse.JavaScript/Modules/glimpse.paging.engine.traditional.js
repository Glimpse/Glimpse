(function($, util, engine, engineUtil) {
    var provider = {
            renderControls: function (key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast) {
                var pagerFirstPageLink = $('<span href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-firstPage"></span>'),
                    pagerPreviousPageLink = $('<span href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-previousPage"></span>'),
                    pagerMessage = $('<span class="glimpse-pager-message">' + (pageIndex + 1) + ' / ' + (pageIndexLast + 1) + '</span>'),
                    pagerNextPageLink = $('<span href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-nextPage"></span>'),
                    pagerLastPageLink = $('<span href="#" class="glimpse-button glimpse-pager-link glimpse-pager-link-lastPage"></span>');

                pagerContainer.append(pagerFirstPageLink);
                pagerContainer.append(pagerPreviousPageLink); 
                pagerContainer.append(pagerMessage); 
                pagerContainer.append(pagerNextPageLink);
                pagerContainer.append(pagerLastPageLink);

                if (pageIndex > 0) {
                    pagerFirstPageLink.one('click', function() { engineUtil.load(key, pagerKey, 0, 'insert'); });
                    pagerPreviousPageLink.one('click', function() { engineUtil.load(key, pagerKey, pageIndex - 1, 'insert'); });
                } else {
                    pagerFirstPageLink.addClass('glimpse-pager-link-firstPage-disabled');
                    pagerPreviousPageLink.addClass('glimpse-pager-link-previousPage-disabled');
                }

                if (pageIndex < pageIndexLast) {
                    pagerNextPageLink.one('click', function() { engineUtil.load(key, pagerKey, pageIndex + 1, 'insert'); });
                    pagerLastPageLink.one('click', function() { engineUtil.load(key, pagerKey, pageIndexLast, 'insert'); });
                } else {
                    pagerNextPageLink.addClass('glimpse-pager-link-nextPage-disabled');
                    pagerLastPageLink.addClass('glimpse-pager-link-lastPage-disabled');
                }
            }
        };
    
    engine.register('traditional', provider);
})(jQueryGlimpse, glimpse.util, glimpse.paging.engine, glimpse.paging.engine.util);