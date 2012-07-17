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
                    data = 'No data found for this plugin.';
                result = '<div class="glimpse-panel-message">' + data + '</div>';
            }
            else
                result = string.build(data, level, forceLimit);

            return result;
        };

    return {
        build : build
    };
} ()