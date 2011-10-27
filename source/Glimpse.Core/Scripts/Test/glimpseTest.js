
var glimpseTest = (function ($) {
    var //Support
        testHandlers = {},
        
        pager = function () {
            var generate = function (data) {
                    var pagingInfo = { pageSize : 5, pageIndex : 1, totalNumberOfRecords : 31 },
                        result = [['Title 1', 'Title 2', 'Title 3']],
                        start = data.pageIndex * pagingInfo.pageSize,
                        end = start + pagingInfo.pageSize,
                        random = start;
                        
                    if (end > pagingInfo.totalNumberOfRecords)
                        end = pagingInfo.totalNumberOfRecords;
        
                    for (var i = start; i < end; i++) 
                        result.push([random++, random++, random++]);
        
                    return result;
                },
                trigger = function (param) {
                    param.complete();
        
                    setTimeout(function () {
                        param.success(generate(param.data));
                    }, 300);
                };
        
            return {
                trigger : trigger
            };
        } (),
        ajax = function () {
            var result = [['Request URL', 'Method', 'Status', 'Date/Time', 'Inspect']],
                possibleResults = [['/News', 'Get', '200', '2011/11/09 12:00:12', ''],
                                    ['/Shares', 'Get', '200', '2011/11/09 12:10:34', ''],
                                    ['/Order/230', 'Get', '200', '2011/11/09 12:12:23', ''],
                                    ['/Order/Add', 'Get', '200', '2011/11/09 12:17:52', ''],
                                    ['/History/Results', 'Get', '200', '2011/11/24 12:00:35', ''],
                                    ['/News/List', 'Get', '200', '2011/11/09 12:27:23', ''],
                                    ['/News', 'Post', '200', '2011/11/09 12:29:14', '']],
                generate = function (data) { 
                    if (Math.floor(Math.random() * 4) == 3) 
                        result.push(possibleResults[Math.floor(Math.random() * 6)]);  
                    return result;
                },
                trigger = function (param) { 
                    setTimeout(function () {
                        var success = (Math.floor(Math.random() * 11) != 10);
                        param.complete(null, (success ? 'Success' : 'Fail'))
                        if (success)
                            param.success(generate(param.data));
                    }, 300);
                };
        
            return {
                trigger : trigger
            };
        } (),
        
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