glimpse.util = (function($) {
    return {
        htmlEncode: function (value) {
            return !(value == undefined || value == null) ? value.replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;') : '';
        },
        preserveWhitespace: function (value) {
            return value.replace(/\r\n/g, '<br />').replace(/\n/g, '<br />').replace(/\t/g, '&nbsp; &nbsp; ').replace(/  /g, '&nbsp; ');
        },
        lengthJson: function (data) {
            var count = 0;
            if ($.isPlainObject(data))
                $.each(data, function (k, v) { count++; });
            return count;
        }, 
        uriTemplate: function (url, data) {
            return UriTemplate.parse(url).expand(data || {});
        },
        sortTabs: function (data) {
            var sorted = {},
                i, temp = [];
            
            for (i in data)
                temp.push({ id: i, name: data[i].name });
            temp.sort(function(a, b) {
                return a.name < b.name ? -1 : a.name > b.name ? 1 : 0;
            });
            for (i = 0; i < temp.length; i++)
                sorted[temp[i].id] = data[temp[i].id];
            return sorted;
        }, 
        getTokens: function(formatString) { 
            var count = 0, working = '', result = [];
            for (var i = 0; i < formatString.length; i++) {
                var x = formatString[i];
                
                if (count <= 2) { 
                    if (x == '{')
                        count++;
                    else if (x == '}' && count > 0)
                        count--;
                    else if (count == 2) {
                        if (!$.isNumeric(x)) {
                            count = 0;
                            working = '';
                        }
                        else 
                            working += '' + x;
                    }
                    else {
                        count = 0;
                        working = '';
                    }

                    if (count == 0 && working != '') {
                        result.push(working);
                        working = '';
                    }
                } 
            }
            return result;
        }
    };
})(jQueryGlimpse);