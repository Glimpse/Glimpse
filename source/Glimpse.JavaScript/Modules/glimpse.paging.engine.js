glimpse.paging.engine = (function(pubsub) {
    var providers = {},
        retrieve = function(name) {
            return providers[name];
        },
        register = function(name, engine) {
            providers[name] = engine;
        },
        render = function(args) {
            var pagerEngine = providers[args.pagerType]; 
            pagerEngine.renderControls(args.key, args.pagerContainer, args.pagerKey, args.pagerType, args.pageIndex, args.pageIndexLast);
        };
   
    pubsub.subscribe('trigger.paging.controls.render', render);  
    
    return {
        retrieve: retrieve,
        register: register
    };
})(glimpse.pubsub);