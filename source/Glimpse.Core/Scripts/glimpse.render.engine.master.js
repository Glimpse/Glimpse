master = function () {
    var //Main 
        build = function (data, level, forceFull, metadata, forceLimit) {
            var result = '', attr;

            if ($.isArray(data)) {
                if (metadata)
                    result = structured.build(data, level, forceFull, metadata, forceLimit);
                else
                    result = table.build(data, level, forceFull, forceLimit);
            }
            else if ($.isPlainObject(data))
                result = keyValue.build(data, level, forceFull, forceLimit);
            else if (level == 0) {
                if (data === undefined || data === null || data === '')
                    result = '';
                else {
                    attr = '';
                    if (data.indexOf('http://') == 0) {
                        attr = ' data-glimpse-lazy-url="' + data + '"';
                        data = 'Loading data, please wait...';
                    } 
                    result = '<div class="glimpse-panel-message"' + attr + '>' + data + '</div>';
                }
            }
            else
                result = string.build(data, level, forceLimit);

            return result;
        };

    return {
        build : build
    };
} ()