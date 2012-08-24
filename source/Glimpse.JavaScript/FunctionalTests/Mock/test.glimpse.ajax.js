
var glimpseTest = (function ($) {
    var //Support 
        testHandlers = {},
/*(import:test.glimpse.ajax.pager.js|2)*/, 
/*(import:test.glimpse.ajax.data.js|2)*/,
        
        //Main
        queryStringToObject = function (uri) {
            var queryString = {},
                matched = 0;
            uri.replace(
                new RegExp("([^?=&]+)(=([^&]*))?", "g"),
                function($0, $1, $2, $3) {
                    if (matched > 0)
                        queryString[$1] = $3;
                    matched++;
                }
            );
            return matched > 1 ? queryString : null;
        },

        random = function (length) {
            return Math.floor(Math.random() * length);
        },
        retrieve = function (url) {
            var parts = /(\S+)\?/ig.exec(url); 
            if (parts != null && parts.length == 2 && testHandlers[parts[1]]) 
                return testHandlers[parts[1]];
            return null;
        },
        register = function (name, callback) { 
            testHandlers[name] = callback; 
        },
        init = function () { 
            register("Pager", pager.trigger);
            register("Ajax", data.trigger); 
            register("History", data.trigger); 
            register("Request", data.trigger); 
            register("Tab", data.trigger); 

            //http://stackoverflow.com/questions/5272698/how-to-fake-jquery-ajax-response
            var original = $.ajax;
            $.ajax = function (param) { 
                var callback = retrieve(param.url);
                if (callback) 
                    callback(param, queryStringToObject(param.url));
                else 
                    original(param); 
            };
        };

    init();
}($Glimpse));