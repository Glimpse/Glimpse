shellController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('state.build', build); 
        },
        wireDomListeners = function() {
            elements.scope.find('.glimpse-open').click(function () { pubsub.publish('action.open'); return false; });
            elements.scope.find('.glimpse-minimize').click(function () { pubsub.publish('action.minimize'); return false; });
            elements.scope.find('.glimpse-close').click(function () { pubsub.publish('action.close'); return false; });
            elements.scope.find('.glimpse-popout').click(function () { pubsub.publish('action.popout'); return false; });
        },  
        getCss = function() {
            return '<style type="text/css"> ' + template.css.replace(/url\(\)/gi, 'url(' + config.path + 'sprite.png)') + ' </style>'; 
        },
        getHtml = function() {
            return template.html;
        },
        
        //Main
        build = function () {
            pubsub.publish('state.build.template');  
            pubsub.publish('state.build.template.modify');
                    
            $(getCss()).appendTo('head'); 
            $(getHtml()).appendTo('body');

            pubsub.publish('state.build.shell'); 
            pubsub.publish('state.build.shell.modify');

            wireDomListeners();
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()