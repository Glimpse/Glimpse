shellController = function () {
    var //Support  
        wireListeners = function() {
            pubsub.subscribe('state.build', build); 
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
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()