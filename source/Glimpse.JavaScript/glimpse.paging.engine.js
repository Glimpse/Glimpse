pagingEngine = function () {
    var //Engines 
        registeredEngnies = {},
/*(import:glimpse.paging.engine.traditional.js|2)*/,
/*(import:glimpse.paging.engine.continuous.js|2)*/,
/*(import:glimpse.paging.engine.scrolling.js|2)*/,

        //Main  
        retrieve = function (name) {
            return registeredEngnies[name];
        },
        register = function (name, engine) {
            registeredEngnies[name] = engine;
        },
        init = function () {
            register('traditional', traditional);
            register('continuous', continuous);
            register('scrolling', scrolling);
        };

    init();
     
    return { 
        retrieve : retrieve,
        register : register
    };
} ()