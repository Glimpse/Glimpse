
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
            var result = [],
                possibleResults = [{ method : 'Get', duration : 213, browser : '', clientName : '', requestTime : '2011/11/09 12:00:12', requestId : 'sadsad', isAjax : true, url : '/News'},
                    { method : 'Get', duration : 123, browser : '', clientName : '', requestTime : '2011/11/09 12:10:34', requestId : 'gfdsgf', isAjax : true, url : '/Shares'},
                    { method : 'Get', duration : 234, browser : '', clientName : '', requestTime : '2011/11/09 12:12:23', requestId : '324', isAjax : true, url : '/Order/230'},
                    { method : 'Post', duration : 342, browser : '', clientName : '', requestTime : '2011/11/09 12:17:52', requestId : 'asd', isAjax : true, url : '/Order/Add'},
                    { method : 'Post', duration : 211, browser : '', clientName : '', requestTime : '2011/11/24 12:00:35', requestId : 'kmk', isAjax : true, url : '/History/Results'},
                    { method : 'Post', duration : 242, browser : '', clientName : '', requestTime : '2011/11/09 12:27:23', requestId : 'sdf', isAjax : true, url : '/News/List'},
                    { method : 'Get', duration : 1234, browser : '', clientName : '', requestTime : '2011/11/09 12:29:14', requestId : 'zcxcv', isAjax : true, url : '/News'}],
                index = 0,
                lastId = 0,
                generate = function (data) {  
                    if ((index < 6 && data.glimpseId == 1234) || index < 3)
                        result.push(possibleResults[index++]);
                    return result;
                },
                trigger = function (param) { 
                    if (param.data.glimpseId != lastId) {
                        index = 0;
                        lastId = param.data.glimpseId;
                    }
        
                    setTimeout(function () {
                        var success = (Math.floor(Math.random() * 11) != 10);
                        param.complete(null, (success ? 'Success' : 'Fail'));
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