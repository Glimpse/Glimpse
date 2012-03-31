templateProcess = function () {
    var //Support
        wireListeners = function () {
            pubsub.subscribe('state.build.template', processData);  
        },
        processData = function () {  
            var metadata = data.currentMetadata();
            template.css = '/*(import:glimpse.view.shell.css)*/';
            template.html = '/*(import:glimpse.view.shell.html)*/';
            template.metadata = '/*(import:glimpse.view.metadata.html)*/';

            pubsub.publish('data.template.processed'); 
        }, 
        
        //Main
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()