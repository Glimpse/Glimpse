renderController = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('state.render', render); 
        },
        constructTabs = function (items) {
            var html = '', itemKey, disabled, item;
            for (itemKey in items) {
                item = items[itemKey];
                disabled = (item.data === undefined || item.data === null) ? ' glimpse-disabled' : '';

                html += '<li class="glimpse-tabitem-' + itemKey + disabled + '" data-sort="' + itemKey + '">' + item.name + '</li>';
            }
            return html;
        },
        renderTabs = function (items) {
            elements.tabHolder.find('li:not(.glimpse-permanent)').remove();
            elements.tabHolder.append(constructTabs(items)); 
            util.sortElements(elements.tabHolder, elements.tabHolder.find('li'));
        },

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