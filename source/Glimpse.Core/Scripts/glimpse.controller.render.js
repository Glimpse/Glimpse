renderController = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('state.render', render); 
            pubsub.subscribe('data.elements.processed', wireDomListeners); 
            pubsub.subscribe('action.tab.select', function(subject, payload) { selectedTab(payload); }); 
        },
        wireDomListeners = function() {
            elements.tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('click', function () { pubsub.publish('action.tab.select', $(this)); return false; });
        },

        constructTabs = function (items) {
            var html = '', itemKey, disabled, item;
            for (itemKey in items) {
                item = items[itemKey];
                disabled = (item.data === undefined || item.data === null) ? ' glimpse-disabled' : '';

                html += '<li class="glimpse-tabitem-' + itemKey + disabled + '" data-glimpseKey="' + itemKey + '">' + item.name + '</li>';
            }
            return html;
        },
        renderTabs = function (items) {
            elements.tabHolder.find('li:not(.glimpse-permanent)').remove();
            elements.tabHolder.append(constructTabs(items)); 
            util.sortElements(elements.tabHolder, elements.tabHolder.find('li'));
        },
        selectedTab = function (selectedItem) {
            var key = selectedItem.data('glimpseKey');

            elements.tabHolder.find('.glimpse-active, .glimpse-hover').removeClass('glimpse-active').removeClass('glimpse-hover'); 
            selectedItem.addClass('glimpse-active');
        }

        //Main
        render = function () {
            var items = data.getCurrent();
            renderTabs(items);
        },
        init = function () {
            wireListeners(); 
        };
    
    init();  
} ()