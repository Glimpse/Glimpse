renderController = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('state.render', renderLayout); 
            pubsub.subscribe('data.elements.processed', wireDomListeners); 
            pubsub.subscribe('action.tab.select', function(subject, payload) { selectedTab(payload); }); 
        },
        wireDomListeners = function() {
            elements.tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('click', function () { pubsub.publish('action.tab.select', $(this)); return false; });
            elements.tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function (e) {
                var pluginData = $(this);
                if (e.type == 'mouseover') { pluginData.addClass('glimpse-hover'); } else { pluginData.removeClass('glimpse-hover'); }
            });
        },

        cleanPreRender = function() {
            elements.tabHolder.find('.glimpse-tab:not(.glimpse-permanent)').remove();
            elements.panelHolder.find('.glimpse-panel:not(.glimpse-permanent)').remove(); 
        },

        constructTabs = function (pluginDataCollection) {
            var html = '', key, disabled, pluginData;
            for (key in pluginDataCollection) {
                pluginData = pluginDataCollection[key];
                disabled = (pluginData.data === undefined || pluginData.data === null) ? ' glimpse-disabled' : '';

                html += '<li class="glimpse-tab glimpse-tabitem-' + key + disabled + '" data-glimpseKey="' + key + '">' + pluginData.name + '</li>';
            }
            return html;
        },
        renderTabs = function (pluginDataCollection) {
            elements.tabHolder.append(constructTabs(pluginDataCollection)); 
            util.sortElements(elements.tabHolder, elements.tabHolder.find('li'));
        },
        selectedTab = function (selectedItem) {
            var key = selectedItem.attr('data-glimpseKey');

            //Switch style states
            elements.tabHolder.find('.glimpse-active, .glimpse-hover').removeClass('glimpse-active').removeClass('glimpse-hover'); 
            selectedItem.addClass('glimpse-active');

            //Make sure Panel assoc panel is shown 
            selectedPanel(key);
        },

        constuctPanel = function (key, pluginData, pluginMetadata) {
            var start = new Date().getTime();
            
            var metadata = pluginMetadata == undefined ? pluginMetadata.structure : undefined;
                html = '<div class="glimpse-panel glimpse-panelitem-' + key + '" data-glimpseKey="' + key + '">' + renderEngine.build(pluginData.data, metadata) + '</div>'
                
            var end = new Date().getTime(); 
            console.log('Total render time for ' + key + ': ' + (end - start));

            return html;
        },
        renderPanel = function (key, pluginData, pluginMetadata) {
            elements.panelHolder.append(constuctPanel(key, pluginData, pluginMetadata)); 
        },
        selectedPanel = function (key) {
            var panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]'); 

            //Create panel if needed
            if (panel.length == 0) {
                renderPanel(key, data.getCurrent()[key], data.getCurrentMeta().plugins[key]);  
                panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]');
            }

            //Switch display states
            elements.panelHolder.find('.glimpse-panel:visible').hide();
            panel.show();
        },


        //Main
        renderLayout = function () { 
            cleanPreRender();
            renderTabs(data.getCurrent());
        },
        init = function () {
            wireListeners(); 
        };
    
    init();  
} ()