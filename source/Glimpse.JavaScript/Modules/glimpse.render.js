(function($, pubsub, util, data) {
    var templates = {
            css: '/*(import:glimpse.render.shell.css)*/',
            html: '/*(import:glimpse.render.shell.html)*/'
        },
        generateSpriteAddress = function () {
            return util.uriTemplate(data.currentMetadata().resources.glimpse_sprite);
        },
        getCss = function() {
            var content = templates.css.replace(/url\(\)/gi, 'url(' + generateSpriteAddress() + ')');
            
            return '<style type="text/css"> ' + content + ' </style>'; 
        },
        getHtml = function() {
            return templates.html;
        },
        process = function(isInitial, topic) {
            pubsub.publish(topic + '.rendering', { isInitial: isInitial });
            pubsub.publish('action.shell.rendering', { isInitial: isInitial }); 
        
            pubsub.publish('trigger.tab.render', { isInitial: isInitial });
        
            pubsub.publish('action.shell.rendered', { isInitial: isInitial });
            pubsub.publish(topic + '.rendered', { isInitial: isInitial });
        },
        refresh = function() { 
            process(false, 'action.shell.refresh'); 
        },
        render = function() {  
            pubsub.publish('action.template.processing', { templates: templates });
            pubsub.publish('action.shell.loading');
            
            $(getCss()).appendTo('head'); 
            $(getHtml()).appendTo('body');
            
            pubsub.publish('action.shell.loaded');
            pubsub.publish('action.template.processed', { templates: templates });

            pubsub.publish('trigger.shell.listener.subscriptions');

            process(true, 'action.shell.initial'); 
        };
    
    pubsub.subscribe('trigger.shell.refresh', refresh); 
    pubsub.subscribe('trigger.shell.render', render);   
})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.data); 