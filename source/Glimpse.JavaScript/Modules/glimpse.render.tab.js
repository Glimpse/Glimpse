(function($, data, elements, util, pubsub) {
    var wireListeners = function () {
            var tabHolder = elements.tabHolder();
            
            tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('click', function () {
                var key = $(this).attr('data-glimpseKey');
                pubsub.publish('trigger.tab.select.' + key, { key: key });
            }); 
            tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function (e) {
                e.type == 'mouseover' ? $(this).addClass('glimpse-hover') : $(this).removeClass('glimpse-hover');
            }); 
        },
        generateHtml = function(pluginDataSet) {
            var html = { instance: '', permanent: '' };
            for (var key in pluginDataSet) {
                var pluginData = pluginDataSet[key],
                    disabled = (pluginData.data === undefined || pluginData.data === null) ? ' glimpse-disabled' : '',
                    permanent = pluginData.isPermanent ? ' glimpse-permanent' : '',
                    item = '<li class="glimpse-tab glimpse-tabitem-' + key + disabled + permanent + '" data-glimpseKey="' + key + '">' + pluginData.name + '</li>';
                
                if (!pluginData.isPermanent)
                    html.instance += item;
                else
                    html.permanent += item;
            }
            return html;
        },
        render = function(args) {
            pubsub.publish('action.tab.rendering');

            var currentData = data.currentData(),
                tabInstanceHolder = elements.tabInstanceHolder(),
                tabPermanentHolder = elements.tabPermanentHolder();
            
            // Make sure that the tabs are sorted
            currentData.data = util.sortTabs(currentData.data);

            // Add tabs to the dom
            var tabHtml = generateHtml(currentData.data);
            tabInstanceHolder.html(tabHtml.instance);
            if (args.isInitial) {
                tabPermanentHolder.append(tabHtml.permanent);
            }

            pubsub.publish('action.tab.rendered', elements.tabHolder());
        },
        selected = function(options) {
            var tabHolder = elements.tabHolder(),
                tab = elements.tab(options.key);

            tabHolder.find('.glimpse-active, .glimpse-hover').removeClass('glimpse-active').removeClass('glimpse-hover');
            tab.addClass('glimpse-active');
        },
        clear = function() {
            elements.tabInstanceHolder().empty();
        };
    
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('trigger.tab.render', render);
    pubsub.subscribe('trigger.tab.select', selected);
    pubsub.subscribe('trigger.shell.clear', clear);
})(jQueryGlimpse, glimpse.data, glimpse.elements, glimpse.util, glimpse.pubsub);