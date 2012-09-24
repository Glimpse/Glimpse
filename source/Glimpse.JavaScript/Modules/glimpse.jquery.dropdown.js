(function ($) { 
    $.fn.dropdown = function() {
        var scope = $(this);
        
        scope.live('mouseenter', function() { 
            $(this).next().css('left', $(this).position().left).show(); 
        }); 
        scope('mouseleave', function() {
            $(this).fadeOut(100);  
        }); 
        
        return scope;
    };
})(jQueryGlimpse);