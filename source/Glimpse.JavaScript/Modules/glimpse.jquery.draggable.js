(function ($) {
    var defaults = { 
            min: 50, 
            isUpDown: true,
            valueStyle: 'height', 
            offset: -1, 
            getDimention: function() { return parseInt(this.resizeScope.css(this.valueStyle)); },
            setDimention: function(value) { return this.resizeScope.css(this.valueStyle, value + 'px'); }
        },
        mousePosition = function(e) { 
            return e.data.settings.isUpDown ? e.clientY : e.clientX;
        },
        startDrag = function(e) {
            var settings = e.data.settings;

            console.log(settings);

            if ($.isFunction(settings.dragging))
                $.isFunction(settings.dragging(settings));
        
            settings._min = $.isFunction(settings.min) ? settings.min(settings) : settings.min;
            settings._max = $.isFunction(settings.max) ? settings.max(settings) : settings.max;
            settings._startMousePosition = mousePosition(e);
            settings._startDimention = settings.getDimention.call(settings);
            settings.opacityScope.css('opacity', 0.50);  
             
            $(document).bind('mousemove', { settings: settings }, performDrag).bind('mouseup', { settings: settings }, endDrag);
            $('body').addClass('glimpse-dragging');
        },
        endDrag = function(e) {
            var settings = e.data.settings;
            
            settings.opacityScope.css('opacity', 1);
            
            $('body').removeClass('glimpse-dragging');
            $(document).unbind('mousemove', performDrag).unbind('mouseup', endDrag);

            if ($.isFunction(settings.dragged))
                $.isFunction(settings.dragged(settings)); 
        },
        performDrag = function (e) {
            var settings = e.data.settings,
                newMousePosition = mousePosition(e),
                differenceMousePosition = (newMousePosition - settings._startMousePosition) * settings.offset,
                newDimention = settings._startDimention + differenceMousePosition;
             
            if (settings._min != undefined) 
                newDimention = Math.max(settings._min, newDimention); 
            if (settings._max != undefined) 
                newDimention = Math.min(settings._max, newDimention); 
            
            settings.setDimention.call(settings, newDimention); 
        };

    $.draggable = function(settings) {
        settings = $.extend(true, {}, defaults, settings);

        console.log(settings);

        settings.handelScope.bind("mousedown", { settings: settings }, startDrag);
        
        return this;
    };
})(jQueryGlimpse);