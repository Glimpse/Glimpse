var glimpseTimeline = function (scope, settings) { 
    var elements = {},
        findElements = function () {
            //Main elements
            elements.contentRow = scope.find('.glimpse-tl-row-content');
            elements.summaryRow = scope.find('.glimpse-tl-row-summary');

            //Event elements
            elements.contentBandScroller = elements.contentRow.find('.glimpse-tl-content-scroll');
            elements.contentBandHolder = elements.contentRow.find('.glimpse-tl-band-group');
            elements.contentEventHolder = elements.contentRow.find('.glimpse-tl-event-group');
            elements.contentDescHolder = elements.contentRow.find('.glimpse-tl-event-desc-group');
            elements.contentTableHolder = elements.contentRow.find('.glimpse-tl-table-holder');

            elements.summaryBandHolder = elements.summaryRow.find('.glimpse-tl-band-group');
            elements.summaryEventHolder = elements.summaryRow.find('.glimpse-tl-event-group'); 
            elements.summaryDescHolder = elements.summaryRow.find('.glimpse-tl-event-desc-group');

            //Event info element 
            elements.eventInfo = scope.find('.glimpse-tl-event-info');
             
            //Zoom elements
            elements.zoomHolder = elements.summaryRow.find('.glimpse-tl-resizer-holder');
            elements.zoomLeftHandle = elements.summaryRow.find('.glimpse-tl-resizer:first-child');
            elements.zoomRightHandle = elements.summaryRow.find('.glimpse-tl-resizer:last-child');
            elements.zoomLeftPadding = elements.summaryRow.find('.glimpse-tl-padding:first-child');
            elements.zoomRightPadding = elements.summaryRow.find('.glimpse-tl-padding:last-child');

            //Divider elements
            elements.contentDividerHolder = elements.contentRow.find('.glimpse-tl-divider-line-holder');
            elements.summaryDividerHolder = elements.summaryRow.find('.glimpse-tl-divider-line-holder');
        },
        builder = function () {
            var init = function() {
                scope.html('/*(import:glimpse.plugin.timeline.shell.html)*/');
            };

            return {
                init : init
            };
        }(),
        dividerBuilder = function () {
            var render = function (force) {   
                    //Setup the dividers
                    renderDiverders(elements.summaryDividerHolder, { startTime : 0, endTime : settings.duration}, force);
                    renderDiverders(elements.contentDividerHolder, { startTime : settings.startTime, endTime : settings.endTime }, force);

                    //Fix height
                    adjustHeight();
                },
                renderDiverders = function (scope, range, force) { 
                    var x;
                    for (x = 0; x < scope.length; x += 1) {
                        var holder = $(scope[x]),
                            dividerCount = Math.round(holder.width() / 64),
                            currentDividerCount = holder.find('.glimpse-tl-divider').length;

                        if (!force && currentDividerCount === dividerCount) { return; }

                        var leftOffset = 100 / dividerCount,
                            timeSlice = Math.round((range.endTime - range.startTime) / dividerCount),
                            divider = holder.find('.glimpse-tl-divider:first-child');

                        for (var i = 0; i < dividerCount; i += 1) {
                            //Create divider if needed 
                            if (divider.length == 0) {
                                divider = $('<div class="glimpse-tl-divider"><div></div></div>');
                                holder.append(divider);
                            }
                            //Position divider
                            divider.css('left', (leftOffset * (i + 1)) + '%');
                            //Set label of divider
                            var time = i == (dividerCount - 1) ? range.endTime : (timeSlice * (i + 1)) + range.startTime; 
                            divider.find('div').text(glimpse.util.timeConvert(parseInt(time)));
                            //Move onto next
                            divider = divider.next();
                        }

                        while (divider.length == 1) {
                            var nextDivider = divider.next();
                            divider.remove();
                            divider = nextDivider;
                        }
                    }
                },
                adjustHeight = function () { 
                    //Content row divider height
                    var innerHeight = Math.max(elements.contentBandScroller.height(), elements.contentBandScroller.find('.glimpse-tl-band-holder').height());   
                    elements.contentBandScroller.find('.glimpse-tl-divider-holder').height(innerHeight); 

                    //Summary row divider height
                    elements.summaryRow.find('.glimpse-tl-summary-height').height(elements.summaryRow.height());
                },
                wireEvents = function () {  
                    //Window resize event 
                    $(window).resize(function () { render(); }); 
                },
                init = function () {
                    wireEvents();
                };
                 
            return {
                init : init,
                render : render,
                adjustHeight : adjustHeight
            }; 
        }(),
        eventBuilder = function () { 
            var render = function () {
                    processCategories();
                    processEvents();
                    processEventSummary();
                    processTableData();
                    //colorRows(true);
                    view.start();
                },
                processTableData = function () {
                    var dataResult = [ [ 'Title', 'Description', 'Category', 'Timing', 'Start Point', 'Duration', 'w/out Children' ] ],
                        metadata = [ [ { data : '{{0}} |({{1}})|' }, { data : 2, width : '18%' }, { data : 3, width : '9%' }, { data : 4, align : 'right', pre : 'T+ ', post : ' ms', className : 'mono', width : '100px' }, { data : 5, align : 'right', post : ' ms', className : 'mono', width : '100px' }, { data : 6, align : 'right', post : ' ms', className : 'mono', width : '100px' } ] ];
                    
                    //Massage the data 
                    for (var i = 0; i < settings.events.length; i++) {
                        var event = settings.events[i],
                            data = [ event.title, event.subText, event.category, '', event.startPoint, event.duration, event.childlessDuration ];
                        dataResult.push(data);
                    } 

                    //Insert it into the document
                    var result = glimpse.render.build(dataResult, metadata); 
                    elements.contentTableHolder.append(result);

                    //Update the output 
                    elements.contentTableHolder.find('tbody tr').each(function(i) {
                        var row = $(this),
                            event = settings.events[i],  
                            category = settings.category[event.category];
                             
                        row.find('td:first-child').prepend($('<div class="glimpse-tl-event"></div>').css({ 'backgroundColor' : category.eventColor, marginLeft : (15 * event.nesting) + 'px', 'border' : '1px solid ' + category.eventColorHighlight }));
                        row.find('td:nth-child(3)').css('position', 'relative').prepend($('<div class="glimpse-tl-event"></div>').css({ 'backgroundColor' : category.eventColor, 'border' : '1px solid ' + category.eventColorHighlight, 'margin-left' : event.startPersent + '%', width : event.widthPersent + '%' })); 
                    }); 
                },
                processCategories = function () {
                    for (var categoryName in settings.category) {
                        var category = settings.category[categoryName];

                        elements.summaryDescHolder.append('<div class="glimpse-tl-band glimpse-tl-category-selected"><input type="checkbox" value="' + categoryName +'" checked="checked" /><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '"></div>' + categoryName +'</div>'); 
                        elements.summaryBandHolder.append('<div class="glimpse-tl-band"></div>');
                        category.holder = $('<div class="glimpse-tl-band"></div>').appendTo(elements.summaryEventHolder); 
                        category.events = {};
                    }
                },
                processEvents = function () { 
                    var eventStack = [], lastEvent = { startPoint : 0, duration : 0, childlessDuration : 0 };
                    for (var i = 0; i < settings.events.length; i += 1) {
                        var event = settings.events[i],
                            topEvent = eventStack.length > 0 ? eventStack[eventStack.length - 1] : undefined,
                            category = settings.category[event.category],
                            left = (event.startPoint / settings.duration) * 100,
                            rLeft = Math.round(left),
                            width = (event.duration / settings.duration) * 100,
                            rWidth = Math.round(width),
                            widthStyle = (width > 0 ? 'width:' + width + '%' : ''),
                            maxStyle = (width <= 0 ? 'max-width:7px;' : ''),
                            subTextPre = (event.subText ? '(' + event.subText + ')' : ''),
                            subText = (subTextPre ? '<span class="glimpse-tl-event-desc-sub">' + subTextPre + '</span>' : ''),
                            stackParsed = false;
                             
                        //Derive event nesting  
                        while (!stackParsed) {
                            if (event.startPoint > lastEvent.startPoint && (event.startPoint + event.duration) <= (lastEvent.startPoint + lastEvent.duration)) {
                                eventStack.push(lastEvent); 
                                stackParsed = true;
                            }
                            else if (topEvent != undefined && (topEvent.startPoint + topEvent.duration) < (event.startPoint + event.duration)) {
                                eventStack.pop(); 
                                topEvent = eventStack.length > 0 ? eventStack[eventStack.length - 1] : undefined; 
                                stackParsed = false;
                            }
                            else
                                stackParsed = true;
                        }
                        
                        //Work out childless timings 
                        var temp = eventStack.length > 0 ? eventStack[eventStack.length - 1] : undefined; 
                        if (temp) { temp.childlessDuration -= event.duration; } 

                        //Save calc data
                        event.childlessDuration = event.duration;
                        event.startPersent = left;
                        event.endPersent = left + width;
                        event.widthPersent = width;
                        event.nesting = eventStack.length;

                        //Build up the event decoration
                        var eventDecoration = '';
                        if (width >= 0)
                            eventDecoration = '<div class="glimpse-tl-event-overlay-lh"><div class="glimpse-tl-event-overlay-li"></div><div class="glimpse-tl-event-overlay-lt">' + event.startPoint + 'ms</div></div>';
                        if (width > 0)
                            eventDecoration += '<div class="glimpse-tl-event-overlay-rh"><div class="glimpse-tl-event-overlay-ri"></div><div class="glimpse-tl-event-overlay-rt">' + (event.startPoint + event.duration) + 'ms</div></div><div class="glimpse-tl-event-overlay-c">' + (width < 3.5 ? '...' : (event.duration + 'ms')) + '</div>';
                        eventDecoration = '<div class="glimpse-tl-event-overlay" style="left:' + left + '%;' + widthStyle + maxStyle + '" data-timelineItemIndex="' + i + '">' + eventDecoration + '</div>';

                        //Add main event HTML to DOM
                        elements.contentBandHolder.append('<div class="glimpse-tl-band"></div>');
                        elements.contentEventHolder.append('<div class="glimpse-tl-band"><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + ';left:' + left + '%;' + widthStyle + maxStyle + '"></div>'+ eventDecoration +'</div>');
                        elements.contentDescHolder.append('<div class="glimpse-tl-band" title="' + event.title + ' ' + subTextPre + '"><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '"></div>' + event.title + subText +'</div>');
                     
                        //Register events for summary  
                        deriveEventSummary(category, left, rLeft, width, rWidth);
                        
                        lastEvent = event;
                    }
                },
                deriveEventSummary = function (category, left, rLeft, width, rWidth) {
                    for (var j = rLeft; j <= (rLeft + rWidth); ++j) { 
                        var data = category.events[j], right = left + width;
                        if (data) {
                            data.left = Math.min(left, data.left);
                            data.right = Math.max(right, data.right);
                        }
                        else 
                            category.events[j] = { left : left, right : right } 
                    } 
                },
                processEventSummary = function () { 
                    var addCategoryEvent = function (category, start, finish) {
                        var width = (finish - start), 
                            widthStyle = (width > 0 ? 'width:' + width + '%' : ''),
                            maxStyle = (width <= 0 ? 'max-width:7px;' : ''); 
                        category.holder.append('<div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '; left:' + start + '%;' + widthStyle + maxStyle + '"></div>');
                    };

                    for (var categoryName in settings.category) {
                        var category = settings.category[categoryName], 
                            events = category.events, 
                            startData = null, 
                            next = 0; 

                        for (var currentPoint in events) { 
                            var current = parseInt(currentPoint);

                            if (!startData) {  //TODO: this needs to be cleanned up, duplicate logic here
                                startData = events[currentPoint]; 
                                next = current + 1; 
                            }
                            else if (current != next) {  
                                addCategoryEvent(category, startData.left, events[next - 1].right); 
                                startData = events[currentPoint]; 
                                next = current + 1; 
                            }
                            else 
                                next++; 
                        }
                        if (startData) { addCategoryEvent(category, startData.left, events[next - 1].right); } 
                    }
                },
                colorRows = function (applyAll) { 
                    var filter = applyAll ? '' : ':visible';
                    colorElement(elements.contentBandHolder.find('> div'), filter);
                    colorElement(elements.contentDescHolder.find('> div'), filter);
                    colorElement(elements.summaryBandHolder.find('> div'), filter);
                    colorElement(elements.summaryDescHolder.find('> div'), filter);
                    colorElement(elements.contentTableHolder.find('tbody'), filter); 
                },
                colorElement = function (scope, filter) {
                    scope.removeClass('odd').removeClass('even');
                    scope.filter(filter + ':even').addClass('even');
                    scope.filter(filter + ':odd').addClass('odd');
                },
                categoryEvents = function (item) {
                    //Handel how the UI will look
                    var isChecked = item[0].checked, parent = item.parent();
                    parent.animate({ 'opacity' : (isChecked ? 0.95 : 0.6) }, 200);
                    elements.summaryEventHolder.find('.glimpse-tl-band').eq(parent.index()).animate({ 'opacity' : (isChecked ? 1 : 0.7) }, 200)

                    //Trigger search
                    filter.category()
                },
                _wireEvents = function () {
                    elements.summaryDescHolder.find('input').live('click', function() {
                        categoryEvents($(this));
                    });
                    elements.summaryDescHolder.find('.glimpse-tl-band')
                        .live('mouseenter', function () { if ($(this).has('input:checked').length > 0) { $(this).find('input').stop(true, true).clearQueue().fadeIn(); } })
                        .live('mouseleave', function () { if ($(this).has('input:checked').length > 0) { $(this).find('input').stop(true, true).clearQueue().fadeOut(); } });
                    elements.summaryDescHolder.find('input').click(function() { filter.category(); });
                },
                init = function () { 
                    _wireEvents();
                };
                 
            return {
                init : init,
                colorRows : colorRows,
                render : render
            };
        }(),          
        zoom = function () {
            var positionLeft = function () {
                    var persentLeft = (elements.zoomLeftHandle.position().left / elements.zoomHolder.width()) * 100, 
                        persentRight = (elements.zoomRightPadding.width() / elements.zoomHolder.width()) * 100; 
                 
                    //Set left slider
                    elements.zoomLeftHandle.css('left', persentLeft + '%'); 
                    elements.zoomLeftPadding.css('width', persentLeft + '%');
                
                    settings.startTime = settings.duration * (persentLeft / 100);

                    //Force render
                    dividerBuilder.render(true);

                    //Zoom in on main line items 
                    zoomEvents(persentLeft, persentRight);
                 
                    //Manage zero display
                    toggleZeroLine(persentLeft == 0); 
                 
                    //Hide events that aren't needed
                    filter.zoom(persentLeft, persentRight); 
                },
                positionRight = function () {
                    var persentRight = ((elements.zoomHolder.width() - 4 - elements.zoomRightHandle.position().left) / elements.zoomHolder.width()) * 100, 
                        persentLeft = (elements.zoomLeftPadding.width() / elements.zoomHolder.width()) * 100; 
                 
                    //Set right slider
                    elements.zoomRightHandle.css('right', persentRight + '%'); 
                    elements.zoomRightPadding.css('width', persentRight + '%');

                    settings.endTime = settings.duration - (settings.duration * (persentRight / 100));
                
                    //Force render
                    dividerBuilder.render(true);
                
                    //Zoom in on main line items 
                    zoomEvents(persentLeft, persentRight);
                    
                    //Hide events that aren't needed
                    filter.zoom(persentLeft, persentRight);  
                },
                zoomEvents = function (persentLeft, persentRight) {
                    var offset = (100 / (100 - persentLeft - persentRight)) * -1, 
                        lOffset = offset * persentLeft, 
                        rOffset = offset * persentRight;

                    elements.contentRow.find('.glimpse-tl-event-holder-inner').css({ left : lOffset + '%', right : rOffset + '%' });
                },
                toggleZeroLine = function (show) {
                    elements.contentRow.find('.glimpse-tl-divider-zero-holder').toggle(show);  
                    elements.contentRow.find('.glimpse-tl-divider-line-holder').css('left', (show ? '15' : '0') + 'px');
                    elements.contentRow.find('.glimpse-tl-event-holder').css('marginLeft', (show ? '15' : '0') + 'px');
                }, 
                wireEvents = function () {   
                    glimpse.util.resizer(elements.zoomLeftHandle, {
                        min: function () { return 0; },
                        max: function () { return (elements.zoomRightHandle.position().left - 20); },
                        preDragCallback: function () { elements.zoomLeftHandle.css('left', (elements.zoomLeftHandle.position().left) + 'px'); },
                        endDragCallback: function () { positionLeft(); }
                    });
                    glimpse.util.resizer(elements.zoomRightHandle, {
                        min: function () { return 0; },
                        max: function () { return (elements.zoomHolder.width() - elements.zoomLeftHandle.position().left) - 20; },
                        preDragCallback: function () { elements.zoomRightHandle.css('right', (elements.zoomHolder.width() - elements.zoomRightHandle.position().left) + 'px'); },
                        endDragCallback: function () { positionRight(); },
                        valueStyle: 'right',
                        offset: -1
                    });
                },
                init = function () {
                    wireEvents();
                };
                 
            return {
                init : init
            };
        }(),
        filter = function () {
            var criteria = { 
                    persentLeft : 0, 
                    persentRightFromLeft : 100, 
                    hiddenCategories : undefined
                },
                search = function (c) {
                    //Go through each event doing executing search
                    for (var i = 0; i < settings.events.length; i += 1) {
                        var event = settings.events[i],
                            show = !(c.persentLeft > event.endPersent 
                                    || c.persentRightFromLeft < event.startPersent)
                                    && (c.hiddenCategories == undefined || c.hiddenCategories[event.category] == true);

                        //Timeline elements
                        elements.contentBandHolder.find('.glimpse-tl-band').eq(i).toggle(show);
                        elements.contentEventHolder.find('.glimpse-tl-band').eq(i).toggle(show);
                        elements.contentDescHolder.find('.glimpse-tl-band').eq(i).toggle(show); 
                        //Table elements
                        elements.contentTableHolder.find('tbody').eq(i).toggle(show); 
                    }

                    //Recolourize rows 
                    eventBuilder.colorRows();

                    //Fix height
                    dividerBuilder.adjustHeight();
                },
                zoom = function (persentLeft, persentRight) {
                    //Pull out current search
                    criteria.persentLeft = persentLeft;
                    criteria.persentRightFromLeft = 100 - persentRight; 
                    
                    //Execute search
                    search(criteria);
                },
                category = function () {
                    //Pull out current search
                    var hiddenCategoriesObj = {}, hiddenCategories = elements.summaryDescHolder.find('input:checked').map(function() { return $(this).val() }).get();
                    for (var i = 0; i < hiddenCategories.length; i++)
                        hiddenCategoriesObj[hiddenCategories[i]] = true;
                    criteria.hiddenCategories = hiddenCategoriesObj;
                    
                    //Execute search
                    search(criteria);
                },
                init = function () { 
                };
                 
            return {
                init : init,
                zoom : zoom,
                category : category
            };
        }(),
        info = function () {
            var showTip = function (item) {
                    item.find('.glimpse-tl-event-overlay').stop(true, true).fadeIn(); 
                },
                hideTip = function (item) {
                    item.find('.glimpse-tl-event-overlay').stop(true, true).fadeOut(); 
                },
                buildBubbleDetails = function(event, category) {
                    var details = '', detailKey;
                    for (detailKey in event.details) {
                        details += '<tr><th>' + detailKey + '</th><td>' + event.details[detailKey] + '</td></tr>';
                    }
                    return '<table><tr><th colspan="2"><div class="glimpse-tl-event-info-title"><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '"></div>' + event.title + ' - Details</div></th></tr><tr><th>Duration</th><td>' + event.duration + 'ms (at ' + event.startPoint + 'ms' + ( + event.duration > 1 ? (' to ' + (event.startPoint + event.duration) + 'ms') : '' ) +')</td></tr>' + (event.duration != event.childlessDuration ? '<tr><th>w/out Children</th><td>' + event.childlessDuration + 'ms</td></tr>' : '') + (event.subText ? '<tr><th>Details</th><td>' + event.subText + '</td></tr>' : '' ) + details + '</table>';
                },
                updateBubble = function (item) {
                    var eventOffset = item.offset(), 
                        containerOffset = elements.eventInfo.parent().offset(),
                        eventSize = { height : item.height(), width : item.width() },
                        event = settings.events[item.attr('data-timelineItemIndex')],
                        category = settings.category[event.category],
                        content = buildBubbleDetails(event, category);
                     
                    eventOffset.top -= containerOffset.top;
                    eventOffset.left -= containerOffset.left;

                    elements.eventInfo.html(content)
                     
                    var detailSize = { height : elements.eventInfo.height(), width : elements.eventInfo.width() }, 
                        newDetailLeft = Math.min(Math.max((eventOffset.left + ((eventSize.width - detailSize.width) / 2)) - 15, 5), $(document).width() - detailSize.width - 30),
                        newDetailTop = eventOffset.top - detailSize.height - 20; 
                         
                    elements.eventInfo.css('left', newDetailLeft + 'px');
                    elements.eventInfo.css('top', newDetailTop + 'px');   
                },
                showBubble = function (item) {  
                    elements.eventInfo.stop(true, true).clearQueue().delay(500).queue(function () { updateBubble(item); elements.eventInfo.show(); }); 
                },
                hideBubble = function () { 
                    elements.eventInfo.stop(true, true).clearQueue().delay(500).fadeOut(); 
                },
                wireEvents = function () {
                    elements.eventInfo
                        .live('mouseenter', function () { elements.eventInfo.stop(true, true).clearQueue(); })
                        .live('mouseleave', function () { hideBubble(); });

                    elements.contentRow.find('.glimpse-tl-event-overlay')
                        .live('mouseenter', function () { showBubble($(this)); })
                        .live('mouseleave', function () { hideBubble(); });

                    elements.contentEventHolder.find('.glimpse-tl-band')
                        .live('mouseenter', function () { showTip($(this)); })
                        .live('mouseleave', function () { hideTip($(this)); });
                },
                init = function () {
                    wireEvents();
                };
                 
            return {
                init : init
            };
        }(),
        resize = function () {
            var columnResize = function (position) {
                    scope.find('.glimpse-tl-col-side').width(position + 'px');
                    scope.find('.glimpse-tl-col-main').css('left', position + 'px');
                    
                    dividerBuilder.render(false);
                },
                containerResize = function (height) { 
                    //Work out what heihgt we can work with
                    var contentHeight = height - (elements.summaryRow.height() + scope.find('.glimpse-tl-row-spacer').height());  
                    elements.contentRow.height(contentHeight + 'px');
                    
                    //Render Divers
                    dividerBuilder.render();
                },
                wireEvents = function () { 
                    glimpse.util.resizer(elements.contentRow.find('.glimpse-tl-resizer'), {
                        max: function () { return 300; },
                        endDragCallback: function (position) { columnResize(position); }
                    });
                },
                init = function () {
                    wireEvents(); 
                };
                 
            return {
                containerResize : containerResize,
                init : init
            };
        }(),
        view = function () {
            var apply = function(showTimeline, isFirst) {
                    elements.contentTableHolder.parent().toggle(!showTimeline); 
                    elements.contentRow.find('.glimpse-tl-content-scroll:first-child').toggle(showTimeline);
                    elements.contentRow.find('.glimpse-tl-resizer').toggle(showTimeline); 
                    
                    eventBuilder.colorRows(isFirst); 
                    if (showTimeline) 
                        dividerBuilder.render();
                },
                toggle = function() {
                    var showTimeline = !(glimpse.settings.timeView);

                    apply(showTimeline);
                 
                    glimpse.settings.timeView = showTimeline;
                    glimpse.pubsub.publish('state.persist');
                },
                start = function() {
                    apply(glimpse.settings.timeView, true);
                },
                init = function() { 
                    elements.summaryRow.find('.glimpse-tl-band-title span').click(function () {
                        toggle(); 
                    });
                };

            return { 
                toggle : toggle,
                start : start,
                init : init
            };
        } (),
        init = function () { 
            //Set defaults
            settings.startTime = 0;
            settings.endTime = settings.duration;
            
            //Render main layout
            builder.init();

            //Find rendered elements 
            findElements();

            //Wire events
            dividerBuilder.init();
            eventBuilder.init();
            zoom.init();
            filter.init();
            info.init();
            resize.init();
            view.init();
             
            //Render events 
            eventBuilder.render();  
        };

    return {  
        init : init,
        support : {
            containerResize : resize.containerResize
        }
    };
};