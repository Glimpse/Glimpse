renderController = function () {
    var //Support
        wireListeners = function() {
            pubsub.subscribe('state.render', renderLayout); 
            pubsub.subscribe('data.elements.processed', wireDomListeners); 
            pubsub.subscribe('action.tab.select', function(subject, payload) { selectedItem(payload); }); 
        },
        wireDomListeners = function() {
            elements.tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('click', function () { pubsub.publish('action.tab.select', $(this).attr('data-glimpseKey')); return false; });
            elements.tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function (e) {
                var pluginData = $(this);
                if (e.type == 'mouseover') { pluginData.addClass('glimpse-hover'); } else { pluginData.removeClass('glimpse-hover'); }
            });
        },

        clearPreviousLayout = function () {
            elements.tabHolder.find('.glimpse-tab:not(.glimpse-permanent)').remove();
            elements.panelHolder.find('.glimpse-panel:not(.glimpse-permanent)').remove(); 
        },
        buildNewLayout = function () {
            renderTabs(data.getCurrent());
        },

        renderTabs = function (pluginDataSet) {
            elements.tabHolder.append(constructTabs(pluginDataSet)); 
            util.sortElements(elements.tabHolder, elements.tabHolder.find('li'));
        },
        constructTabs = function (pluginDataSet) {
            var html = '', key, disabled, pluginData;
            for (key in pluginDataSet) {
                pluginData = pluginDataSet[key];
                disabled = (pluginData.data === undefined || pluginData.data === null) ? ' glimpse-disabled' : '';

                html += '<li class="glimpse-tab glimpse-tabitem-' + key + disabled + '" data-glimpseKey="' + key + '">' + pluginData.name + '</li>';
            }
            return html;
        },
         
        renderPanel = function (key, pluginData, pluginMetadata) { 
            var start = new Date().getTime();
            
            var metadata = pluginMetadata == undefined ? pluginMetadata.structure : undefined;
                html = '<div class="glimpse-panel glimpse-panelitem-' + key + '" data-glimpseKey="' + key + '"><div class="glimpse-panel-message">Loading data, please wait...</div></div>',
                panel = $(html).appendTo(elements.panelHolder);

            renderEngine.insert(panel, pluginData.data, metadata); 

            var end = new Date().getTime(); 
            console.log('Total render time for "' + key + '": ' + (end - start));

            return html;
        },

        
        selectedItem = function (key) {
            var oldItem = elements.tabHolder.find('.glimpse-active');
             
            if (oldItem.length > 0) { pubsub.publish('action.plugin.deactive', oldItem.attr('data-glimpseKey')); } 

            selectedTab(key);
            selectedPanel(key);

            settings.activeTab = key;
            pubsub.publish('state.persist');
             
            pubsub.publish('action.plugin.active', key); 
        }, 
        selectedTab = function (key) {
            var tab = elements.tabHolder.find('.glimpse-tab[data-glimpseKey="' + key + '"]');

            //Switch style states
            elements.tabHolder.find('.glimpse-active, .glimpse-hover').removeClass('glimpse-active').removeClass('glimpse-hover'); 
            tab.addClass('glimpse-active');
        },
        selectedPanel = function (key) {
            var panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]'); 

            //Create panel if needed
            if (panel.length == 0) {
                renderPanel(key, data.getCurrent()[key], data.getCurrentMeta().plugins[key]);  
                panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]');
                
                pubsub.publish('action.plugin.created', key); 
            }
            
            //Switch style states
            elements.panelHolder.find('.glimpse-active').removeClass('glimpse-active'); 
            panel.addClass('glimpse-active');
        },


        //Main
        renderLayout = function () { 
            clearPreviousLayout();
            buildNewLayout();
            
            pubsub.publish('state.build.rendered');
        },
        init = function () {
            wireListeners(); 
        };
    
    init();  
} ()