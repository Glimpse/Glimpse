glimpse.render.engine.util.table = (function($) {
    var factories = {
            array: {
                isHandled: function(data) {
                    return $.isArray(data[0]);
                },
                getHeader: function(data) {
                    return data[0];
                },
                getRowClass: function(data, rowIndex) {
                    return data[rowIndex].length > data[0].length ? ' ' + data[rowIndex][data[rowIndex].length - 1] : '';
                },
                getRowValue: function(dataRow, fieldIndex, header) {
                    return dataRow[fieldIndex];
                },
                startingIndex: function() {
                    return 1;
                }
            },
            object: {
                isHandled: function(data) {
                    return data[0] === Object(data[0]);
                },
                getHeader: function(data) {
                    var result = [];
                    for (var key in data[0]) {
                        if (key != "_metadata")
                            result.push(key);
                    }
                    return result;
                },
                getRowClass: function(data, rowIndex) {
                    return data[rowIndex]._metadata && data[rowIndex]._metadata.style ? ' ' + data[rowIndex]._metadata.style : '';
                },
                getRowValue: function(dataRow, fieldIndex, header) {
                    return dataRow[header[fieldIndex]];
                },
                startingIndex: function() {
                    return 0;
                }
            },
            other: {
                isHandled: function(data) {
                    return true;
                },
                getHeader: function(data) {
                    return ["Values"];
                },
                getRowClass: function(data, rowIndex) {
                    return '';
                },
                getRowValue: function(dataRow, fieldIndex, header) {
                    return dataRow;
                },
                startingIndex: function() {
                    return 0;
                }
            }
        };

    return {
        findFactory: function(data) {
                var match = null;
                for (var key in factories) {
                    if (factories[key].isHandled(data)) {
                        match = factories[key];
                        break;
                    }
                }
                return match;
            }
        };
})(jQueryGlimpse);