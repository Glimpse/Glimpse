
var glimpseTest = (function ($) {
    var //Support
        testHandlers = {},
/*(import:test.glimpse.ajax.pager.js|2)*/,
/*(import:test.glimpse.ajax.ajax.js|2)*/,
        
        //Main
        retrieve = function (name) {
            return testHandlers[name];
        },
        register = function (name, engine) {
            testHandlers[name] = engine;
        },
        init = function () { 
            register("Pager", pager);
            register("Ajax", ajax);
            //http://stackoverflow.com/questions/5272698/how-to-fake-jquery-ajax-response
            var original = $.ajax;
            $.ajax = function (param) { 
                var handel = retrieve(param.url);
                if (handel)
                    handel.trigger(param);
                else 
                    $.ajax(param); 
            };
        };

    init();
}($Glimpse));