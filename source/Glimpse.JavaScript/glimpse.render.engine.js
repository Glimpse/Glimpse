glimpse.render.engine = (function($, pubsub) {
    var providers = {},
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
            var html = $('<div>' + build(data, metadata) + '</div>').find('.glimpse-row-holder:first')[0].innerHTML;
            var elements = $(html).insertAfter(scope.find('.glimpse-row-holder:first > .glimpse-row:last'));
            
            pubsub.publish('trigger.panel.render.style', { scope: elements });
        },
        prepend = function(scope, data, metadata) {
            var html = $('<div>' + build(data, metadata) + '</div>').find('.glimpse-row-holder:first')[0].innerHTML;
            var elements = $(html).insertBefore(scope.find('.glimpse-row-holder:first > .glimpse-row:first'));
            
            pubsub.publish('trigger.panel.render.style', { scope: elements });
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