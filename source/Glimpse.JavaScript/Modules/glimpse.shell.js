(function($, glimpse) {
    var pubsub = glimpse.pubsub,
        templates = {
            css: '/*(import:glimpse.view.shell.css)*/',
            html: '/*(import:glimpse.view.shell.html)*/'
        },
        getCss = function() {
            var spriteAddress = util.replaceTokens(data.currentMetadata().resources.glimpse_sprite),
                content = templates.css.replace(/url\(\)/gi, 'url(' + spriteAddress + ')');
            
            return '<style type="text/css"> ' + content + ' </style>'; 
        },
        getHtml = function() {
            return template.html;
        },
        ready = function() {
            pubsub.subscribe('action.template.processing', templates);
            pubsub.subscribe('action.shell.loading');
            
            $(getCss()).appendTo('head'); 
            $(getHtml()).appendTo('body');
            
            pubsub.subscribe('action.template.processed', templates);
            pubsub.subscribe('action.shell.loaded');
        };

    pubsub.subscribe('trigger.shell.load', start); 
})(jQueryGlimpse, glimpse);