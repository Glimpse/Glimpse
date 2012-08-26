renderEngine = function () {
    var //Support 
        registeredEngnies = {},
        shouldUsePreview = function (length, level, forceFull, limit, forceLimit, tolerance) {
            if ($.isNumeric(forceLimit))  
                limit = forceLimit; 
            return !forceFull && ((level == 1 && length > (limit + tolerance)) || (level > 1 && (!forceLimit || length > (limit + tolerance))));
        },
        newItemSpacer = function (currentRow, rowLimit, dataLength) { 
            var html = '';
            if (currentRow > 1 && (currentRow <= rowLimit || dataLength > rowLimit)) 
                html += '<span class="rspace">,</span>'; 
            if (currentRow > rowLimit && dataLength > rowLimit) 
                html += '<span class="small">length=' + dataLength + '</span>'; 
            return html;
        },
/*(import:glimpse.render.util.rawString.js|2)*/,
/*(import:glimpse.render.style.js|2)*/,

        //Engines 
/*(import:glimpse.render.engine.master.js|2)*/,
/*(import:glimpse.render.engine.keyValue.js|2)*/,
/*(import:glimpse.render.engine.table.js|2)*/,
/*(import:glimpse.render.engine.structured.js|2)*/,
/*(import:glimpse.render.engine.string.js|2)*/,

        //Main 
        retrieve = function (name) {
            return registeredEngnies[name];
        },
        register = function (name, engine) {
            registeredEngnies[name] = engine;
        },
        build = function (data, metadata) { 
            return master.build(data, 0, true, metadata, 1);
        },
        insert = function (scope, data, metadata) {
            scope.html(build(data, metadata));
            style.apply(scope);
        },
        init = function () {
            register('master', master);
            register('keyvalue', keyValue);
            register('table', table);
            register('structured', structured);
            register('string', string);

        };

    init();
     
    return {
        insert : insert,
        build : build,
        retrieve : retrieve,
        register : register
    };
} ()