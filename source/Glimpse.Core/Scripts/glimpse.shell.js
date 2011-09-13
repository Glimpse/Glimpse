        shell = function () {
            var //Private 
                getCss = function() {
                    return template.css.replace(/url\(\)/gi, 'url(' + config.path + 'sprite.png)'); 
                },
                getHtml = function() {
                    return template.html;
                },
        
                //Public
                build = function () {
                    pubsub.publish('state.renderPreview');  
                 
                    $('<style type="text/css"> ' + getCss() + ' </style>').appendTo("head");
                    $('body').append(getHtml());

                    pubsub.publish('state.render'); 
                },
        
                //Private
                init = function () {
                    pubsub.subscribe('state.build', build); 
                };
    
            init(); 
        } ()