(function($, pubsub, data, elements) {
    var wireListeners = function() {
            
        }, 
        buildHtml = function(currentData) {
            var baseData = data.baseData(), 
                html = '';
            
            if (currentData.isAjax)
                html = ' &gt; Ajax';
            if ((currentData.isAjax && baseData.requestId != currentData.parentId) || (!currentData.isAjax && baseData.requestId != currentData.requestId)) {
                if (html) 
                    html = ' &gt; <a data-requestId="' + history + '">History</a>' + html;
                else
                    html = ' &gt; History';    
            }
            if (html)
                html = '(<a data-requestId="' + baseData.requestId + '">Original</a>' + html + ')';

             return html; 
        }, 
        contextSwitch = function(args) { 
            var html =  buildHtml(args.newData);
            
            elements.titleHolder().find('.glimpse-snapshot-path').html(html);
        };
     
    pubsub.subscribe('action.data.refresh.changed', contextSwitch); 
    pubsub.subscribe('trigger.shell.listener.subscriptions', wireListeners);
})(jQueryGlimpse, glimpse.pubsub, glimpse.data, glimpse.elements);