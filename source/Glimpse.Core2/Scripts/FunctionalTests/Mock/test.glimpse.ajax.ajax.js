ajax = function () {
    var result = [],
        possibleResults = [{ method : 'Get', duration : 213, browser : '', clientName : '', requestTime : '2011/11/09 12:00:12', requestId : 'ajax0', parentId : '1234', isAjax : true, url : '/News'},
            { method : 'Get', duration : 123, browser : '', clientName : '', requestTime : '2011/11/09 12:10:34', requestId : 'ajax1', parentId : '1234', isAjax : true, url : '/Shares'},
            { method : 'Get', duration : 234, browser : '', clientName : '', requestTime : '2011/11/09 12:12:23', requestId : 'ajax2', parentId : '1234', isAjax : true, url : '/Order/230'},
            { method : 'Post', duration : 342, browser : '', clientName : '', requestTime : '2011/11/09 12:17:52', requestId : 'ajax3', parentId : '1234', isAjax : true, url : '/Order/Add'},
            { method : 'Post', duration : 211, browser : '', clientName : '', requestTime : '2011/11/24 12:00:35', requestId : 'ajax4', parentId : '1234', isAjax : true, url : '/History/Results'},
            { method : 'Post', duration : 242, browser : '', clientName : '', requestTime : '2011/11/09 12:27:23', requestId : 'ajax5', parentId : '1234', isAjax : true, url : '/News/List'},
            { method : 'Get', duration : 1234, browser : '', clientName : '', requestTime : '2011/11/09 12:29:14', requestId : 'ajax6', parentId : '1234', isAjax : true, url : '/News'}],
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
} ()