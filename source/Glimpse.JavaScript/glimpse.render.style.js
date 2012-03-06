style = function () {
    var //Support 
        codeProcess = function(items) {
            $.each(items, function() {
                var item = $(this).addClass('prettyprint'), 
                    codeType = item.hasClass('glimpse-code') ? item.attr('data-codeType') : item.closest('.glimpse-code').attr('data-codeType');  

                item.html(prettyPrintOne(item.html(), codeType));
            });
        },

        //Main
        apply = function (scope) { 
            var start = new Date().getTime();
            //Expand collapse  
            scope.find('.glimpse-expand').click(function () {
                var toggle = $(this).toggleClass('glimpse-collapse'),
                    hasClass = toggle.hasClass('glimpse-collapse'); 
                toggle.parent().next().children().first().toggle(!hasClass).next().toggle(hasClass);
            });

            //Alert state
            scope.find('.info, .warn, .error, .fail, .loading, .ms')
                .find('> td:first-child, > tr:first-child > td:first-child:not(:has(div.glimpse-cell)), > tr:first-child > td:first-child > div.glimpse-cell:first-child')
                .not(':has(.icon)').prepend('<div class="icon"></div>');

            //Code formatting
            codeProcess(scope.find('.glimpse-code:not(:has(table)), .glimpse-code > table:not(:has(thead)) .glimpse-preview-show'));
            
            //Open state
            scope.find('.glimpse-start-open > td > .glimpse-expand:first-child').click();

            var end = new Date().getTime(); 
            console.log('Total style time for "' + scope.attr('data-glimpseKey') + '": ' + (end - start));
        };
    
    return { 
            apply : apply
        };
} ()