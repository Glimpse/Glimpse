
var glimpseTest = (function ($) {
    var //Support
        testHandlers = {},
/*(import:test.glimpse.ajax.pager.js|2)*/, 
/*(import:test.glimpse.ajax.data.js|2)*/,
        
        //Main
        random = function (length) {
            return Math.floor(Math.random() * length);
        },
        retrieve = function (name) {
            return testHandlers[name];
        },
        register = function (name, callback) {
            testHandlers[name] = callback;
        },
        init = function () { 
            register("Pager", function(param) { pager.trigger(param); });
            register("Ajax", function(param) { data.trigger(param); }); 
            register("History", function(param) { data.trigger(param); }); 
            register("Request", function(param) { data.trigger(param); }); 
            register("Tab", function(param) { data.trigger(param); }); 

            //http://stackoverflow.com/questions/5272698/how-to-fake-jquery-ajax-response
            var original = $.ajax;
            $.ajax = function (param) { 
                var callback = retrieve(param.url);
                if (callback)
                    callback(param);
                else 
                    original(param); 
            };
        };

    init();
}($Glimpse));