(function($, util, engine, engineUtil) {
    var provider = {
            build: function (data, level, forceLimit) { 
                if (data == undefined || data == null)
                    return '--';
                if ($.isArray(data))
                    return '[ ... ]';
                if ($.isPlainObject(data))
                    return '{ ... }';

                var charMax = $.isNumeric(forceLimit) ? forceLimit : (level > 1 ? 80 : 150),
                    charOuterMax = (charMax * 1.2),
                    content = engineUtil.raw.process(data, charMax, charOuterMax, true);

                if (data.length > charOuterMax) {
                    content = '<span class="glimpse-preview-string" title="' + engineUtil.raw.process(data, charMax * 2, charMax * 2.1, false, true) + '">' + content + '</span>';
                    if (charMax >= 15)
                        content = '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td>' + content + '<span class="glimpse-preview-show">' + util.preserveWhitespace(engineUtil.raw.process(data)) + '</span></td></tr></table>';
                }
                else 
                    content = util.preserveWhitespace(content);  
              
                return content;
            }
        };

    engine.register('string', provider);
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util);
