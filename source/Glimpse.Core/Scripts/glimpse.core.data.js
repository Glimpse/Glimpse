data = (function () {
    var //Support
        current = {},
    
        //Main
        getCurrent = function () {
            return current;
        },
        getCurrentMeta = function () {
            return current.data._metadata;
        },
        init = function () {
            current = glimpseData; 
        };
        
    init(); 
    
    return {
        getCurrent : getCurrent,
        getCurrentMeta : getCurrentMeta
    };
}())