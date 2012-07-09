titleController = function () {
    var //Support    
        wireListeners = function () {
            pubsub.subscribe('state.render', setup);  
            pubsub.subscribe('data.elements.processed', wireDomListeners); 
        },
        wireDomListeners = function() {
            elements.title.find('.glimpse-url a').live('click', function() { switchContext($(this).attr('data-requestId')); return false; });
            elements.title.find('.glimpse-snapshot-path a').live('click', function() { switchContext($(this).attr('data-requestId')); return false; });
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
            start : function () { elements.title.find('.glimpse-url .loading').fadeIn(); }, 
            complete: function () { elements.title.find('.glimpse-url .loading').fadeOut(); },
            success: function (requestId, newResult, oldResult) {
                newResult.metadata.correlation = oldResult.metadata.correlation;
            }
        },
        switchContext = function (requestId) { 
            glimpse.pubsub.publish('action.data.context.reset', 'Title');
            data.retrieve(requestId, switchContextFunc);
        },
        buildEnvironment = function (requestMetadata) {
            var urls = requestMetadata.environmentUrls, 
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
            var correlation = requestMetadata.correlation, 
                html = request.url; 

            if (correlation) { 
                var currentUrl = request.url, 
                    currentLeg; 

                html = '<div>' + correlation.title + '</div>'; 
                for (var i = 0; i < correlation.legs.length; i++) {
                    var leg = correlation.legs[i];
                    if (leg.url == currentUrl) {
                        currentLeg = leg.url;
                        html += currentLeg + ' - <strong>' + leg.method + '</strong> (Current)<br />';
                    }
                    else
                        html += '<a title="Go to ' + leg.url + '" href="#" data-requestId="' + leg.glimpseId + '" data-url="' + leg.url + '">' + leg.url + '</a> - <strong>' + leg.method + '</strong><br />';
                }
                html = '<span class="glimpse-drop">' + currentLeg + '<span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over">' + html + '<div class="loading"><span class="icon"></span><span>Loaded...</span></div></div>'; 
            }
            return html;
        },
        buildTypes = function (types) {
            var payload = data.current(),
                basePayload = data.base(),
                ajax = payload.isAjax && payload.requestId,
                history = (payload.isAjax && basePayload.requestId != payload.parentId && payload.parentId) || (!payload.isAjax && basePayload.requestId != payload.requestId && payload.requestId),
                home = basePayload.requestId,
                html = '';
            
            if (ajax)
                html = ' &gt; Ajax';
            if (history) {
                if (html) 
                    html = ' &gt; <a data-requestId="' + history + '">History</a>' + html;
                else
                    html = ' &gt; History';    
            }
            if (html)
                html = ' <span class="glimpse-snapshot-path">(<a data-requestId="' + home + '">Home</a>' + html + ')</span>';

             return html; 
        },
        buildName = function (name) {
            if (name)
                return '"' + name + '"';
            return name;
        },
        
        //Main
        setup = function () { 
            var request = data.current(),
                requestMetadata = data.currentMetadata(); 
            
            elements.title.find('.glimpse-snapshot-type').text(buildName(request.clientName)).append(buildTypes()).append('&nbsp;');
            elements.title.find('.glimpse-enviro').html(buildEnvironment(requestMetadata));
            elements.title.find('.glimpse-url').html(buildCorrelation(request, requestMetadata));

            dropFunction(elements.title);
        }, 
        init = function () {
            wireListeners();
        };
    
    init(); 
} ()