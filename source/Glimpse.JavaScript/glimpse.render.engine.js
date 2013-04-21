glimpse.render.engine = (function($, pubsub) {
    var providers = {},
        addition = function (scope, data, metadata, insertType, targetType) {
            var html = $('<div>' + build(data, metadata) + '</div>').find('.glimpse-row-holder:first > .glimpse-row'),
                rowHolder = scope.find('.glimpse-row-holder:first'),
                scopeTarget = rowHolder.find('> .glimpse-row:' + targetType);
            
            // Catch the case when we don't have anything to action the insertType against
            if (scopeTarget.length == 0) {
                scopeTarget = rowHolder;
                insertType = 'appendTo';
            }
            
            var elements = $(html)[insertType](scopeTarget);
            
            pubsub.publish('trigger.panel.render.style', { scope: elements });
        },
        retrieve = function(name) {
            return providers[name];
        },
        register = function(name, engine) {
            providers[name] = engine;
        },
        build = function(data, metadata) {
            return providers.master.build(data, 0, true, metadata, 1);
        },
        insert = function(scope, data, metadata) {
            scope.html(build(data, metadata)); 
            pubsub.publish('trigger.panel.render.style', { scope: scope });
        },
        append = function(scope, data, metadata) {
            addition(scope, data, metadata, 'insertAfter', 'last');
        },
        prepend = function(scope, data, metadata) {
            addition(scope, data, metadata, 'insertBefore', 'first');
        };
   
    return {
        _providers: providers,
        retrieve: retrieve,
        register: register,
        build: build,
        insert: insert,
        append: append,
        prepend: prepend
    };
})(jQueryGlimpse, glimpse.pubsub);