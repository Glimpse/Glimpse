
    $.extend($.glimpseContent, {
        formatStringTypes: {
            italics: {
                match: function (d) { return d.match(/^\_[\w\D]+\_$/) != null; },
                replace: function (d) { return '<u>' + $.glimpse.util.htmlEncode($.glimpseContent.scrub(d)) + '</u>'; },
                trimmable: true
            },
            underline: {
                match: function (d) { return d.match(/^\\[\w\D]+\\$/) != null; },
                replace: function (d) { return '<em>' + $.glimpse.util.htmlEncode($.glimpseContent.scrub(d)) + '</em>'; },
                trimmable: true
            },
            strong: {
                match: function (d) { return d.match(/^\*[\w\D]+\*$/) != null; },
                replace: function (d) { return '<strong>' + $.glimpse.util.htmlEncode($.glimpseContent.scrub(d)) + '</strong>'; },
                trimmable: true
            },
            raw: {
                match: function (d) { return d.match(/^\![\w\D]+\!$/) != null; },
                replace: function (d) { return $.glimpseContent.scrub(d); },
                trimmable: false
            }
        },
        scrub: function (d) {
            return d.substr(1, d.length - 2);
        },
        formatString: function (data) {
            var that = this;
            return that.trimFormatString(data);
        },
        trimFormatString: function (data, charMax, charOuterMax, wrapEllipsis, skipEncoding) {
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
    });
