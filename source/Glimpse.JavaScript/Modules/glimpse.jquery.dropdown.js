(function ($) { 
    $.fn.dropdown = function() {
        var scope = $(this);
        
        scope.find('.glimpse-drop').live('mouseenter', function() { 
            $(this).next().css('left', $(this).position().left).show(); 
        }); 
        scope.find('.glimpse-drop-over').live('mouseleave', function() {
            $(this).fadeOut(100);  
        }); 
        
        return scope;
    };
})(jQueryGlimpse);