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
} ()