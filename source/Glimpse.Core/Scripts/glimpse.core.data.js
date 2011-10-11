data = (function () {
    var //Support
        current = {},
    
        //Main
        getCurrent = function () {
            return current;
        },
        init = function () {
            current = glimpseData; 
        };
        
    init(); 
    
    return {
        getCurrent : getCurrent
    };
}())