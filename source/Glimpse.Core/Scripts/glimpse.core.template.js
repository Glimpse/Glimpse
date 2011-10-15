templateController = function () {
    var //Support
        wireListeners = function () {
            pubsub.subscribe('state.build.template', processData);  
        },
        processData = function () {  
            template.css = '/*(import:glimpse.view.shell.css)*/';
            template.html = '/*(import:glimpse.view.shell.html)*/';

            pubsub.publish('data.template.processed'); 
        }, 
        
        //Main
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()