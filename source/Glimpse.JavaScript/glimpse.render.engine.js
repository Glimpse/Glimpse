glimpse.render.engine = (function($, pubsub) {
    var providers = {},
        addition = function (scope, data, metadata, isAppend) {
            var insertType = isAppend ? 'insertAfter' : 'insertBefore',
                html = $('<div>' + build(data, metadata) + '</div>').find('.glimpse-row-holder:first > .glimpse-row'),
                rowHolder = scope.find('.glimpse-row-holder:first'),
                scopeTarget = isAppend ? rowHolder.find('> .glimpse-row:last') : rowHolder.find('> .glimpse-row:first');
            
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
            addition(scope, data, metadata, true);
        },
        prepend = function(scope, data, metadata) {
            addition(scope, data, metadata, false);
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