formatString = function () {
    var //Support 
        types = {
            italics : {
                match: function (d) { return d.match(/^\_[\w\D]+\_$/) != null; },
                replace: function (d) { return '<u>' + $.glimpse.util.htmlEncode(scrub(d)) + '</u>'; },
                trimmable: true
            },
            underline : {
                match: function (d) { return d.match(/^\\[\w\D]+\\$/) != null; },
                replace: function (d) { return '<em>' + $.glimpse.util.htmlEncode(scrub(d)) + '</em>'; },
                trimmable: true
            },
            strong : {
                match: function (d) { return d.match(/^\*[\w\D]+\*$/) != null; },
                replace: function (d) { return '<strong>' + $.glimpse.util.htmlEncode(scrub(d)) + '</strong>'; },
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
        buildFormatString = function(formatString, data, indexs) {  
            for (var i = 0; i < indexs.length; i++) {
                var pattern = "\\\{\\\{" + indexs[i] + "\\\}\\\}", regex = new RegExp(pattern, "g"); 
                formatString = formatString.replace(regex, data[indexs[i]]);
            }
            return formatString;
        },
        formatString = function (data) {
            var that = this;
            return that.trimFormatString(data);
        },
        trimFormatString = function (data, charMax, charOuterMax, wrapEllipsis, skipEncoding) {
            var that = this, trimmable = true, replace = function (d) { return $.glimpse.util.htmlEncode(d); };

            if (data == undefined || data == null)
                return '--';
            if (typeof data != 'string')
                data = data + '';
            //data = $.trim(data);

            if (!skipEncoding) {
                for (var typeKey in that.formatStringTypes) {
                    var type = that.formatStringTypes[typeKey];
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
        }
        
        
        //Main
        init = function () {  
            //TODO: provide injection point for 
        };
    
    init(); 
} ()