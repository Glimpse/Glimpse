(function($) {
  $.fn.ContentSlider = function(options)
  {
    var defaults = {
      leftBtn : 'Content/cs_leftImg.png',
      rightBtn : 'Content/cs_rightImg.png',
      width : '900px',
      height : '450px',
      speed : 400,
      easing : 'easeOutQuad',
      textResize : false,
      IE_h2 : '26px',
      IE_p : '11px'
    }
    var defaultWidth = defaults.width;
    var o = $.extend(defaults, options);
    var w = parseInt(o.width);
    var n = this.children('.cs_wrapper').children('.cs_slider').children('.cs_article').length;
    var x = -1*w*n+w; // Minimum left value
    var p = parseInt(o.width)/parseInt(defaultWidth);
    var thisInstance = this.attr('id');
    var inuse = false; // Prevents colliding animations

    function moveSlider(d, b)
    {
      var l = parseInt(b.siblings('.cs_wrapper').children('.cs_slider').css('left'));
      if(isNaN(l)) {
        var l = 0;
      }
      var m = (d=='left') ? l-w : l+w;
      if(m<=0&&m>=x) {
        b
          .siblings('.cs_wrapper')
            .children('.cs_slider')
              .animate({ 'left':m+'px' }, o.speed, o.easing, function() {
                inuse=false;
              });
               
        if(b.attr('class')=='cs_leftBtn') {
          var thisBtn = $('#'+thisInstance+' .cs_leftBtn');
          var otherBtn = $('#'+thisInstance+' .cs_rightBtn');
        } else {
          var thisBtn = $('#'+thisInstance+' .cs_rightBtn');
          var otherBtn = $('#'+thisInstance+' .cs_leftBtn');
        }
        if(m==0||m==x) {
          thisBtn.animate({ 'opacity':'0' }, o.speed, o.easing, function() { thisBtn.hide(); });
        }
        if(otherBtn.css('opacity')=='0') {
          otherBtn.show().animate({ 'opacity':'1' }, { duration:o.speed, easing:o.easing });
        }
      }
    }

    function vCenterBtns(b)
    {
      // Safari and IE don't seem to like the CSS used to vertically center
      // the buttons, so we'll force it with this function
      var mid = parseInt(o.height)/2;
      b
        .find('.cs_leftBtn img').css({ 'top':mid+'px', 'padding':0 }).end()
        .find('.cs_rightBtn img').css({ 'top':mid+'px', 'padding':0 });
    }

    return this.each(function() { 
      $(this)
        // Set the width and height of the div to the defined size
        .css({
          width:o.width,
          height:o.height
        }) 
        // Add the buttons to move left and right
        .prepend('<a href="#" class="cs_leftBtn"><img src="'+o.leftBtn+'" /></a>')
        .append('<a href="#" class="cs_rightBtn"><img src="'+o.rightBtn+'" /></a>')
        // Dig down to the article div elements
        .find('.cs_article')
          // Set the width and height of the div to the defined size
          .css({
            width:o.width,
            height:o.height
          })
          .end()
        // Animate the entrance of the buttons
        .find('.cs_leftBtn')
          .css("opacity",0)
          .hide()
          .end()
        .find('.cs_rightBtn')
          .hide()
          .animate({ 'width':'show' });

      // Resize the font to match the bounding box
      if(o.textResize===true) {
        var h2FontSize = $(this).find('h2').css('font-size');
        var pFontSize = $(this).find('p').css('font-size');
        $.each(jQuery.browser, function(i) {
          if($.browser.msie) {
             h2FontSize = o.IE_h2;
             pFontSize = o.IE_p;
          }
        });
        $(this).find('h2').css({ 'font-size' : parseFloat(h2FontSize)*p+'px', 'margin-left' : '66%' });
        $(this).find('p').css({ 'font-size' : parseFloat(pFontSize)*p+'px', 'margin-left' : '66%' });
        $(this).find('.readmore').css({ 'font-size' : parseFloat(pFontSize)*p+'px', 'margin-left' : '66%' });
      }

      // Store a copy of the button in a variable to pass to moveSlider()
      var leftBtn = $(this).children('.cs_leftBtn');
      leftBtn.bind('click', function() {
        if(inuse===false) {
          inuse = true;
          moveSlider('right', leftBtn);
        }
        return false; // Keep the link from firing
      });

      // Store a copy of the button in a variable to pass to moveSlider()
      var rightBtn = $(this).children('.cs_rightBtn');
      rightBtn.bind('click', function() {
        if(inuse===false) {
          inuse=true;
          moveSlider('left', rightBtn);
        }
        return false; // Keep the link from firing
      });

      vCenterBtns($(this)); // This is a CSS fix function.
    });
  }
})(jQuery)