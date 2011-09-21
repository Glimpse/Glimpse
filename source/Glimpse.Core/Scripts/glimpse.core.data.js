data = function () {
    var //Support
        processData = function () {  
            template.css = '/*(import:glimpse.view.shell.css)*/';
            template.html = '/*(import:glimpse.view.shell.html)*/';

            pubsub.publish('data.template.processed'); 
        },
        findElements = function () { 
            elements.scope = scope;
            elements.holder = elements.scope.find('.glimpse-holder');
            elements.opener = elements.scope.find('.glimpse-open');
            elements.spacer = elements.scope.find('.glimpse-spacer');  

            pubsub.publish('data.elements.processed'); 
        },
        
        //Main
        init = function () {
            pubsub.subscribe('state.renderPreview', processData); 
            pubsub.subscribe('state.render', findElements); 
        };
    
    init(); 
} ()