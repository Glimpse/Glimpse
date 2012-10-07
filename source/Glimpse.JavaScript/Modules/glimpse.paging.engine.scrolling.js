(function($, pubsub, util, engine, engineUtil, elements, data) {
    var provider = {
            renderControls: function (key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast, suppressRecheck) {
                var panelItem = elements.panel(key),
                    clientHeight = panelItem[0].clientHeight;
                 
                if (clientHeight == 0 && !suppressRecheck) {
                    var that = this;
                    //TODO: BAD CODE - Problem is that the height isn't avaiable until the panel is visable, this code runs before that time
                    setTimeout(function() {
                        that.renderControls(key, pagerContainer, pagerKey, pagerType, pageIndex, pageIndexLast, true);
                    }, 300);
                    return;
                }

                var pagerMessage = $('<span class="glimpse-pager-message">Showing ' + (pageIndex + 1) + ' page(s) of ' + (pageIndexLast + 1) + ' pages(s).</span>');
                pagerContainer.append(pagerMessage);
                    
                if (pageIndex < pageIndexLast) { 
                    if (clientHeight >= panelItem.find(':last').position().top) 
                        engineUtil.load(key, pagerKey, pageIndex + 1, 'append');
                    else {
                        var scrollingCallback = function() {
                            if (panelItem[0].clientHeight >= panelItem.find(':last').position().top) {
                                engineUtil.load(key, pagerKey, pageIndex + 1, 'append');
                                panelItem.unbind('scroll');
                            }
                        };
                        panelItem.bind('scroll', scrollingCallback);
                    } 
                }
            }
        };
    
    engine.register('scrolling', provider);
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.paging.engine, glimpse.paging.engine.util, glimpse.elements, glimpse.data);