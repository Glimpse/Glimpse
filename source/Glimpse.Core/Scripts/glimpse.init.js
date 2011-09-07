    (function () {
        var //Private  
            processData = (function () {  
                data.css = '/*(import:glimpse.shell.css)*/',
                data.html = '/*(import:glimpse.shell.html)*/'
            }),
            findElements = (function () { 
                elements.scope = scope;
                elements.holder = elements.scope.find('.glimpse-holder');
                elements.opener = elements.scope.find('.glimpse-open');
                elements.spacer = elements.scope.find('.glimpse-spacer');  
            }),
        
            //Private
            init = function () {
                pubsub.subscribe('state.renderPreview', processData); 
                pubsub.subscribe('state.render', findElements); 
            };
    
        init(); 
    }());