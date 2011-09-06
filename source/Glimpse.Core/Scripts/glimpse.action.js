    //settings
    //elements
    (function () {
        var //Public 
            open = function () {
                settings.open = true;
                pubsub.publish('state.persist');

                elements.opener.hide(); 
                $.fn.add.call(elements.holder, elements.spacer).show().animate({ height : settings.height }, 'fast');  
            },
            close = function (remove) {
                settings.open = false;
                pubsub.publish('state.persist');

                var panelElements = $.fn.add.call(elements.holder, elements.spacer).animate({ height : '0' }, 'fast', function() {
                        panelElements.hide();

                        if (remove) {
                            elements.scope.remove();
                            pubsub.publish('state.persist');
                        }
                        else
                            elements.opener.show(); 
                    });
            }, 
            resize = function (height) {
                settings.height = height;
                pubsub.publish('state.persist');
             
                elements.spacer.height(height);
                elements.holder.find('.glimpse-panel').height(height - 54); 
            },
        
            //Private
            _init = function () {
                pubsub.subscribe('action.open', open);
                pubsub.subscribe('action.close', close);
                pubsub.subscribe('action.terminate', function() { close(true); });
                pubsub.subscribe('action.resize', resize);
            };
    
        _init(); 
    }()); 