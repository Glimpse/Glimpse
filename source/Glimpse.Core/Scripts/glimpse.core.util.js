util = { 
    cookie : function (key, value, expiresIn) {
        key = encodeURIComponent(key);
        //Set Cookie
        if (arguments.length > 1) {
            var t = new Date();
            t.setDate(t.getDate() + expiresIn || 1000);

            value = $.isPlainObject(value) ? JSON.stringify(value) : String(value);
            return (document.cookie = key + '=' + encodeURIComponent(value) + '; expires=' + t.toUTCString() + '; path=/');
        }

        //Get cookie 
        var result = new RegExp("(?:^|; )" + key + "=([^;]*)").exec(document.cookie);
        if (result) {
            result = decodeURIComponent(result[1]);
            if (result.substr(0, 1) == '{') {
                result = JSON.parse(result);
            }
            return result;
        }
        return null;
    },
    htmlEncode: function (value) {
        return !(value == undefined || value == null) ? $('<div/>').text(value).html() : '';
    },
    htmlDecode: function (value) {
        return !(value == undefined || value == null) ? $('<div/>').html(value).text() : '';
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
}