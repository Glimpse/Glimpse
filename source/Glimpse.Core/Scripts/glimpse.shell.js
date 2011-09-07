    (function () {
        var //Private 
            getCss = function() {
                return data.css.replace(/url\(\)/gi, 'url(' + settings.path + 'sprite.png)'); 
            },
            getHtml = function() {
                return data.html;
            },
        
            //Public
            build = function () {
                pubsub.publish('state.renderPreview');  

                //Add css to head
                $('<style type="text/css"> ' + getCss() + ' </style>').appendTo("head");      //http://stackoverflow.com/questions/1212500/jquery-create-css-rule-class-runtime
                
                //Add html to body
                $('body').append(getHtml());

                pubsub.publish('state.render'); 
            },
        
            //Private
            _init = function () {
                pubsub.subscribe('state.build', build); 
            };
    
        _init(); 
    }()); 


