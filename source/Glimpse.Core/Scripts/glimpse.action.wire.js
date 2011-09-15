        actionWire = function () {
            var //Public 
                bind = function () {

                    elements.scope.find('.glimpse-open').click(function () { pubsub.publish('action.open'); return false; });
                    elements.scope.find('.glimpse-close').click(function () { pubsub.publish('action.close'); return false; });
                    elements.scope.find('.glimpse-terminate').click(function () { pubsub.publish('action.terminate'); return false; });
                    elements.scope.find('.glimpse-popout').click(function () {  return false; });
                }, 
         
                //Private
                init = function () {
                    pubsub.subscribe('state.render', bind); 
                };
    
            init(); 
        } ()