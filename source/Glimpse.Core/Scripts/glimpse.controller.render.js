renderController = function () {
    var //Support
        wireListeners = function () {
            pubsub.subscribe('state.render', renderLayout); 
            pubsub.subscribe('data.elements.processed', wireDomListeners); 
            pubsub.subscribe('action.tab.select', function(subject, payload) { selectedItem(payload); }); 
            pubsub.subscribe('action.data.update', dataUpdate);
        },
        wireDomListeners = function () {
            elements.tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('click', function () { pubsub.publish('action.tab.select', $(this).attr('data-glimpseKey')); return false; });
            elements.tabHolder.find('li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function (e) {
                var pluginData = $(this);
                if (e.type == 'mouseover') { pluginData.addClass('glimpse-hover'); } else { pluginData.removeClass('glimpse-hover'); }
            }); 
        },
 
        selectedTab = function (key) {
            var tab = elements.tabHolder.find('.glimpse-tab[data-glimpseKey="' + key + '"]');

            //Switch style states
            elements.tabHolder.find('.glimpse-active, .glimpse-hover').removeClass('glimpse-active').removeClass('glimpse-hover'); 
            tab.addClass('glimpse-active');
        },
        renderTabs = function (pluginDataSet) {
            elements.tabHolder.append(constructTabs(pluginDataSet)); 
            util.sortElements(elements.tabHolder, elements.tabHolder.find('li'));
        },
        constructTabs = function (pluginDataSet) {
            var html = '', key, disabled, pluginData;
            for (key in pluginDataSet) {
                pluginData = pluginDataSet[key];
                disabled = (pluginData.data === undefined || pluginData.data === null) ? ' glimpse-disabled' : '',
                permanent = pluginData.isPermanent ? ' glimpse-permanent' : '';

                if (!pluginData.isPermanent || (pluginData.isPermanent && elements.tabHolder.find('.glimpse-tab[data-glimpseKey="' + key + '"]').length == 0))
                    html += '<li class="glimpse-tab glimpse-tabitem-' + key + disabled + permanent + '" data-glimpseKey="' + key + '">' + pluginData.name + '</li>';
            }
            return html;
        },
         
        selectedPanel = function (key) {
            var panel = elements.panelHolder.find('.glimpse-panel[data-glimpseKey="' + key + '"]');  
            if (panel.length == 0) {
                panel = renderPanel(key, data.current().data[key], data.currentMetadata().plugins[key]);   
                pubsub.publish('action.plugin.created', key); 
            }
            
            //Switch style states
            elements.panelHolder.find('.glimpse-active').removeClass('glimpse-active'); 
            panel.addClass('glimpse-active');
        },
        renderPanel = function (key, pluginData, pluginMetadata) { 
            var start = new Date().getTime();
            
            var metadata = pluginMetadata.structure,  
                html = '<div class="glimpse-panel glimpse-panelitem-' + key + '" data-glimpseKey="' + key + '"><div class="glimpse-panel-message">Loading data, please wait...</div></div>',
                panel = $(html).appendTo(elements.panelHolder);

            if (!pluginData.isLazy && pluginData.data)
                renderEngine.insert(panel, pluginData.data, metadata);
            else
                pubsub.publishAsync('action.plugin.lazyload', key);

            var end = new Date().getTime(); 
            console.log('Total render time for "' + key + '": ' + (end - start));

            return panel;
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


        //Main
        dataUpdate = function () {
            pubsub.publish('state.render');  
        },
        renderLayout = function () { 
            clearPreviousLayout();
            buildNewLayout();
            
            pubsub.publish('state.build.rendered');
        }, 
        clearPreviousLayout = function () {
            elements.tabHolder.find('.glimpse-tab:not(.glimpse-permanent)').remove();
            elements.panelHolder.find('.glimpse-panel:not(.glimpse-permanent)').remove(); 
        },
        buildNewLayout = function () {
            renderTabs(data.current().data);
        },
        init = function () {
            wireListeners();  
        };
    
    init();  
} ()