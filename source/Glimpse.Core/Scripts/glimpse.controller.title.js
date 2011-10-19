sizerController = function () {
    var //Support    
        wireListeners = function () {
            pubsub.subscribe('state.render', setup); 
        }, 
        dropFunction = function (scope) { 
            scope.find('.glimpse-drop').mouseenter(function() { 
                $(this).next().css('left', $(this).position().left).show(); 
            }); 
            scope.find('.glimpse-drop-over').mouseleave(function() {
                $(this).fadeOut(100);  
            }); 
        },
        buildEnvironment = function (requestMetadata) {
            var urls = requestMetadata.request.environmentUrls, 
                html = ''; 

            if (urls) {
                var currentName = '&nbsp;', 
                    currentDomain = util.getDomain(unescape(window.location.href));

                for (targetName in urls) {
                    if (util.getDomain(urls[targetName]) === currentDomain) {
                        currentName = targetName;
                        html += ' - ' + targetName + ' (Current)<br />';
                    }
                    else
                        html += ' - <a title="Go to - ' + urls[targetName] + '" href="' + urls[targetName] + '">' + targetName + '</a><br />';
                }

                if (currentName) 
                    html = '<span class="glimpse-drop">' + currentName + '<span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over"><div>Switch Servers</div>' + html + '</div>';
            }
            return html;
        },
        
        //Main
        setup = function () { 
            var request = data.current(),
                requestMetadata = data.currentMetadata(),
                type = ''; //((type.length > 0) ? ' (' + type + ')' : '')
            
            elements.title.find('.glimpse-snapshot-type').text(request.clientName + type).append('&nbsp;');
            elements.title.find('.glimpse-enviro').html(buildEnvironment(requestMetadata));
            elements.title.find('.glimpse-url').text(request.url);

            dropFunction(elements.title);
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()