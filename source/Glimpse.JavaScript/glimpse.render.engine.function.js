(function($, util, engine, engineUtil) {
    var build = function (data, level, forceFull) { 
            if (!forceFull)
                return buildPreview(data, level); 
            return buildOnly(data, level);
        }, 
        buildOnly = function (data, level) {
            return data.toString();
        },
        buildPreview = function (data, level) { 
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + buildPreviewOnly(data, level) + '</div><div class="glimpse-preview-show glimpse-code" data-codeType="js">' + buildOnly(data, level) + '</div></td></tr></table>';
        },
        buildPreviewOnly = function (data, level) {
            data = data.toString();
            
            var name = data.substring(data.indexOf(' ') + 1, data.indexOf('(')),
                args = data.substring(data.indexOf('(') + 1, data.indexOf(')')).split(', ').join('<span class="rspace">,</span>');
             
            return  '<span class="start">function</span>' + name + '<span>(</span>' + args + '<span>) { ... }</span>'; 
        },
        provider = {
            build : build,
            buildOnly : buildOnly,
            buildPreview : buildPreview,
            buildPreviewOnly : buildPreviewOnly
        }; 

    engine.register('function', provider);
})(jQueryGlimpse, glimpse.util, glimpse.render.engine, glimpse.render.engine.util);
