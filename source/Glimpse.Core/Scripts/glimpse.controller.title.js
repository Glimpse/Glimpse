sizerController = function () {
    var //Support    
        wireListeners = function () {
            pubsub.subscribe('state.render', setup);  
            pubsub.subscribe('data.elements.processed', wireDomListeners); 
        },
        wireDomListeners = function() {
            elements.title.find('.glimpse-url a').live('click', function() { switchContext($(this).attr('data-requestId')); return false; });
        }, 
        dropFunction = function (scope) { 
            scope.find('.glimpse-drop').mouseenter(function() { 
                $(this).next().css('left', $(this).position().left).show(); 
            }); 
            scope.find('.glimpse-drop-over').mouseleave(function() {
                $(this).fadeOut(100);  
            }); 
        },
        switchContextFunc = {
            start : function () {
                elements.title.find('.glimpse-url .loading').fadeIn();
            }, 
            complete : function () {
                elements.title.find('.glimpse-url .loading').fadeOut();
            }
        },
        switchContext = function (requestId) {
            data.retrieve(requestId, switchContextFunc);
        },
        buildEnvironment = function (requestMetadata) {
            var urls = requestMetadata.request.environmentUrls, 
                html = ''; 

            if (urls) {
                var currentName = 'Enviro', 
                    currentDomain = util.getDomain(unescape(window.location.href));

                for (targetName in urls) {
                    if (util.getDomain(urls[targetName]) === currentDomain) {
                        currentName = targetName;
                        html += ' - ' + targetName + ' (Current)<br />';
                    }
                    else
                        html += ' - <a title="Go to - ' + urls[targetName] + '" href="' + urls[targetName] + '">' + targetName + '</a><br />';
                }
                html = '<span class="glimpse-drop">' + currentName + '<span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over"><div>Switch Servers</div>' + html + '</div>';
            }
            return html;
        },
        buildCorrelation = function (request, requestMetadata) {
            var correlation = requestMetadata.request.correlation, 
                html = ''; 

            if (correlation) { 
                var currentUrl = request.url, 
                    currentLeg; 

                html = '<div>' + correlation.title + '</div>'; 
                for (var i = 0; i < correlation.legs.length; i++) {
                    var leg = correlation.legs[i];
                    if (leg.url == currentUrl) {
                        currentLeg = leg.url;
                        html += currentLeg + ' - <strong>' + leg.method + '</strong> (Current)';
                    }
                    else
                        html += '<a title="Go to ' + leg.url + '" href="#" data-requestId="' + leg.glimpseId + '" data-url="' + leg.url + '">' + leg.url + '</a> - <strong>' + leg.method + '</strong><br />';
                }
                html = '<span class="glimpse-drop">' + currentLeg + '<span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over">' + html + '<div class="loading"><span class="icon"></span><span>Loaded...</span></div></div>'; 
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
            elements.title.find('.glimpse-url').html(buildCorrelation(request, requestMetadata));

            dropFunction(elements.title);
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()