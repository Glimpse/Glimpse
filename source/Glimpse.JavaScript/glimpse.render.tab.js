(function($, data, elements, util, pubsub, settings) {
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
        generateHtmlItem = function(key, pluginData) {
            if (!pluginData.suppressTab) {
                var disabled = (pluginData.data === undefined || pluginData.data === null) ? ' glimpse-disabled' : '',
                    permanent = pluginData.isPermanent ? ' glimpse-permanent' : '';
            
                return '<li class="glimpse-tab glimpse-tabitem-' + key + disabled + permanent + '" data-glimpseKey="' + key + '">' + pluginData.name + '</li>';
            }
            return '';
        },
        generateHtml = function(pluginDataSet) {
            var html = { instance: '', permanent: '' };
            for (var key in pluginDataSet) {
                var pluginData = pluginDataSet[key], 
                    itemHtml = generateHtmlItem(key, pluginData);
                 
                if (pluginData.isPermanent)
                    html.permanent += itemHtml; 
                else
                    html.instance += itemHtml;
            }
            return html;
        },
        render = function(args) { 
            pubsub.publish('action.tab.inserting.bulk');

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
            
            pubsub.publish('action.tab.inserted.bulk'); 
        },
        selected = function(options) {
            var tabHolder = elements.tabHolder(),
                tab = elements.tab(options.key);

            tabHolder.find('.glimpse-active, .glimpse-hover').removeClass('glimpse-active').removeClass('glimpse-hover');
            tab.addClass('glimpse-active');
        },
        clear = function() {
            elements.tabInstanceHolder().empty();
        },
        insert = function(args) {
            var key = args.key,
                payload = args.payload,
                itemHtml = generateHtmlItem(key, payload);
             
            pubsub.publish('action.tab.inserting.single', { key: key });
            
            if (payload.isPermanent)
                elements.tabPermanentHolder().append(itemHtml);
            else
                elements.tabInstanceHolder().append(itemHtml);
            
            pubsub.publish('action.tab.inserted.single', { key: key }); 
        };
    
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
    pubsub.subscribe('trigger.tab.render', render);
    pubsub.subscribe('trigger.tab.select', selected);
    pubsub.subscribe('trigger.shell.clear', clear);
    pubsub.subscribe('tigger.tab.insert', insert);
})(jQueryGlimpse, glimpse.data, glimpse.elements, glimpse.util, glimpse.pubsub, glimpse.settings);