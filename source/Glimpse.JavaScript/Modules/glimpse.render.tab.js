(function($, data, elements, util, pubsub) {
    var wireListeners = function () {
            var tabHolder = elements.tabHolder();
            
            tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('click', function () {
                var key = $(this).attr('data-glimpseKey');
                pubsub.publish('trigger.tab.select.' + key, key);
            }); 
            tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function (e) {
                e.type == 'mouseover' ? $(this).addClass('glimpse-hover') : $(this).removeClass('glimpse-hover');
            }); 
        },
        generateHtml = function(pluginDataSet) {
            var html = '';
            for (var key in pluginDataSet) {
                var pluginData = pluginDataSet[key],
                    disabled = (pluginData.data === undefined || pluginData.data === null) ? ' glimpse-disabled' : '';

                html += '<li class="glimpse-tab glimpse-tabitem-' + key + disabled + '" data-glimpseKey="' + key + '">' + pluginData.name + '</li>';
            }
            return html;
        },
        render = function() {
            pubsub.publish('action.tab.rendering');
            
            var currentData = data.currentData(),
                tabHolder = elements.tabHolder();

            // Make sure that the tabs are sorted
            currentData.data = util.sortTabs(currentData.data);

            // Add tabs to the dom
            var tabHtml = generateHtml(currentData.data);
            tabHolder.append(tabHtml);
            
            pubsub.publish('action.tab.rendered', tabHolder);
        },
        selected = function(key) {
            var tabHolder = elements.tabHolder(),
                tab = tabHolder.find('.glimpse-tab[data-glimpseKey="' + key + '"]');

            tabHolder.find('.glimpse-active, .glimpse-hover').removeClass('glimpse-active').removeClass('glimpse-hover');
            tab.addClass('glimpse-active');
        };
    
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
    pubsub.subscribe('trigger.tab.render', render);
    pubsub.subscribe('trigger.tab.select', selected);
})(jQueryGlimpse, glimpse.data, glimpse.elements, glimpse.util, glimpse.pubsub);