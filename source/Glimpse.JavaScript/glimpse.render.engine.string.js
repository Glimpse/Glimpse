string = function () {
    var //Main
        build = function (data, level, forceLimit) { 
            if (data == undefined || data == null)
                return '--';
            if ($.isArray(data))
                return '[ ... ]';
            if ($.isPlainObject(data))
                return '{ ... }';

            var charMax = !$.isNaN(forceLimit) ? forceLimit : (level > 1 ? 80 : 150),
                charOuterMax = (charMax * 1.2),
                content = rawString.process(data, charMax, charOuterMax, true);

            if (data.length > charOuterMax) {
                content = '<span class="glimpse-preview-string" title="' + rawString.process(data, charMax * 2, charMax * 2.1, false, true) + '">' + content + '</span>';
                if (charMax >= 15)
                    content = '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td>' + content + '<span class="glimpse-preview-show">' + util.preserveWhitespace(rawString.process(data)) + '</span></td></tr></table>';
            }
            else 
                content = util.preserveWhitespace(content);  
              
            return content;
        };

    return {
        build : build
    };
} ()