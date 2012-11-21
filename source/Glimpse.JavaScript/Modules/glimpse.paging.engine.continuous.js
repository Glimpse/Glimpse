(function($, util, engine, engineUtil) {
    var provider = {
            removeExistingResults: false,
            renderControls: function (key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast) {
                var pagerMessage = $('<span class="glimpse-pager-message">Showing ' + (pageIndex + 1) + ' page(s) of ' + (pageIndexLast + 1) + ' pages(s).</span>');
                pagerContainer.append(pagerMessage);

                if (pageIndex < pageIndexLast) {
                    var pagerNextPageLink = $('<span href="#" class="glimpse-pager-link">More</span>');
                    pagerNextPageLink.one('click', function() { engineUtil.load(key, pagerKey, pageIndex + 1, 'append'); });
                    pagerContainer.append(pagerNextPageLink);
                }
            }
        };
    
    engine.register('continuous', provider);
})(jQueryGlimpse, glimpse.util, glimpse.paging.engine, glimpse.paging.engine.util);