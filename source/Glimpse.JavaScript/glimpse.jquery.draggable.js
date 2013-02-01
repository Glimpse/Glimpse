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
             
            if ($.isFunction(settings.dragging))
                settings.dragging(settings);
        
            settings._min = $.isFunction(settings.min) ? settings.min(settings) : settings.min;
            settings._max = $.isFunction(settings.max) ? settings.max(settings) : settings.max;
            settings._startMousePosition = mousePosition(e);
            settings._startDimention = settings.getDimention.call(settings);
            settings.opacityScope.css('opacity', 0.50);  
             
            $(document).bind('mousemove', { settings: settings }, performDrag).bind('mouseup', { settings: settings }, endDrag); 
            
            return false;
        },
        endDrag = function(e) {
            var settings = e.data.settings;
            
            settings.opacityScope.css('opacity', 1);
             
            $(document).unbind('mousemove', performDrag).unbind('mouseup', endDrag);

            if ($.isFunction(settings.dragged))
                settings.dragged(settings, settings.getDimention.call(settings)); 
            
            return false;
        },
        performDrag = function (e) {
            var settings = e.data.settings, 
                newDimention = settings._startDimention + ((mousePosition(e) - settings._startMousePosition) * settings.offset);
            
            if (settings._min != null) 
                newDimention = Math.max(settings._min, newDimention); 
            if (settings._max != null) 
                newDimention = Math.min(settings._max, newDimention); 

            settings.setDimention.call(settings, newDimention); 
            
            return false;
        };

    $.draggable = function(settings) {
        settings = $.extend(true, {}, defaults, settings);
         
        settings.handelScope.bind("mousedown", { settings: settings }, startDrag);
        
        return this;
    };
})(jQueryGlimpse);