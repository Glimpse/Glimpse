rawString = function () {
    var //Support 
        types = {
            italics : {
                match: function (d) { return d.match(/^\_[\w\D]+\_$/) != null; },
                replace: function (d) { return '<u>' + util.htmlEncode(scrub(d)) + '</u>'; },
                trimmable: true
            },
            underline : {
                match: function (d) { return d.match(/^\\[\w\D]+\\$/) != null; },
                replace: function (d) { return '<em>' + util.htmlEncode(scrub(d)) + '</em>'; },
                trimmable: true
            },
            strong : {
                match: function (d) { return d. match(/^\*[\w\D]+\*$/) != null; },
                replace: function (d) { return '<strong>' + util.htmlEncode(scrub(d)) + '</strong>'; },
                trimmable: true
            },
            raw : {
                match: function (d) { return d.match(/^\![\w\D]+\!$/) != null; },
                replace: function (d) { return scrub(d); },
                trimmable: false
            }
        }, 
        scrub = function (d) {
            return d.substr(1, d.length - 2);
        },

        //Main 
        process = function (data, charMax, charOuterMax, wrapEllipsis, skipEncoding) {
            var trimmable = true, 
                replace = function (d) { return util.htmlEncode(d); };

            if (data == undefined || data == null)
                return '--';
            if (typeof data != 'string')
                data = data + '';  
            if (!skipEncoding) {
                for (var typeKey in types) {
                    var type = types[typeKey];
                    if (type.match(data)) {
                        replace = type.replace;
                        trimmable = type.trimmable;
                        break;
                    }
                }
            } 
            if (trimmable && charOuterMax && data.length > charOuterMax)
                return replace(data.substr(0, charMax)) + (wrapEllipsis ? '<span>...</span>' : '...');

            return replace(data);
        };
    
    return {
        process : process
        }; 
} ()