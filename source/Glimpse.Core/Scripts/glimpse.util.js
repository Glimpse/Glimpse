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
            }
        }