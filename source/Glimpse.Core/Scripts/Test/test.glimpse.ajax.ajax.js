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
            param.complete();

            setTimeout(function () {
                param.success(generate(param.data));
            }, 300);
        };

    return {
        trigger : trigger
    };
} ()