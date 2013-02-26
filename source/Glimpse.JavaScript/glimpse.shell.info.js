(function($, pubsub, elements, data, util) {
    var wireListeners = function () { 
            elements.barHolder().find('.glimpse-icon').click(function () { pubsub.publish('trigger.tab.select.info', { key: 'info' }); }); 
        },  
        buildHelp = function(options) { 
            var info = data.currentMetadata().plugins[options.key],
                url = info && info.documentationUri; 
            
            elements.barHolder().find('.glimpse-meta-help').toggle(url != null).attr('href', url); 
        }, 
        buildInfo = function(args) { 
            var metadata = data.currentMetadata(); 
            args.panel.html('/*(import:glimpse.render.shell.info.html)*/');
        },
        setupInfo = function(args) { 
            args.newData.data.info = { data: 'Loading...', suppressTab: true, isPermanent: true };
            args.newData.metadata.plugins.info = {};
        };
    
    pubsub.subscribe('trigger.shell.subscriptions', wireListeners); 
    pubsub.subscribe('action.panel.rendered.info', buildInfo);  
    pubsub.subscribe('action.data.initial.changed', setupInfo); 
    pubsub.subscribe('action.panel.showing', buildHelp); 
})(jQueryGlimpse, glimpse.pubsub, glimpse.elements, glimpse.data, glimpse.util);