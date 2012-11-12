(function($, pubsub, settings, util, renderEngine) { 
    var timeline = {};
    
    // Elements
    timeline.elements = (function() { 
        var elements = {},
            find = function() {
                //Main elements
                elements.scope = timeline.scope;
                elements.contentRow = elements.scope.find('.glimpse-tl-row-content');
                elements.summaryRow = elements.scope.find('.glimpse-tl-row-summary');
                elements.resizer = elements.contentRow.find('.glimpse-tl-resizer');

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
                elements.eventInfo = elements.scope.find('.glimpse-tl-event-info');
             
                //Zoom elements
                elements.zoomHolder = elements.summaryRow.find('.glimpse-tl-resizer-holder');
                elements.zoomLeftHandle = elements.summaryRow.find('.glimpse-tl-resizer:first-child');
                elements.zoomRightHandle = elements.summaryRow.find('.glimpse-tl-resizer:last-child');
                elements.zoomLeftPadding = elements.summaryRow.find('.glimpse-tl-padding:first-child');
                elements.zoomRightPadding = elements.summaryRow.find('.glimpse-tl-padding:last-child');

                //Divider elements
                elements.contentDividerHolder = elements.contentRow.find('.glimpse-tl-divider-line-holder');
                elements.summaryDividerHolder = elements.summaryRow.find('.glimpse-tl-divider-line-holder');
            };
            
        pubsub.subscribe('action.timeline.shell.loaded', find);
        
        return elements;
    })();
    
    // Render Shell
    (function() {
        var templates = {
                html: '/*(import:glimpse.timeline.shell.html)*/'
            },
            setup = function() {
                pubsub.publish('action.timeline.template.processing', { templates: templates });
                pubsub.publish('action.timeline.shell.loading');
                        
                timeline.scope.html(templates.html); 
            
                pubsub.publish('action.timeline.shell.loaded');
                pubsub.publish('action.timeline.template.processed', { templates: templates });
            };

        pubsub.subscribe('trigger.timline.shell.init', setup);
    })();
        
    // Render Dividers
    (function(elements) {
        var wireListeners = function() {  
                //Window resize event 
                $(window).resize(function() { render({}); }); 
            },
            render = function(args) {   
                //Setup the dividers
                renderDiverders(elements.summaryDividerHolder, { startTime : 0, endTime : timeline.data.duration}, args.force);
                renderDiverders(elements.contentDividerHolder, { startTime : timeline.data.startTime, endTime : timeline.data.endTime }, args.force);

                //Fix height
                adjustHeight();
            },
            renderDiverders = function(scope, range, force) { 
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
                        divider.find('div').text(util.timeConvert(parseInt(time)));
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
            adjustHeight = function() { 
                //Content row divider height
                var innerHeight = Math.max(elements.contentBandScroller.height(), elements.contentBandScroller.find('.glimpse-tl-band-holder').height());   
                elements.contentBandScroller.find('.glimpse-tl-divider-holder').height(innerHeight); 

                //Summary row divider height
                elements.summaryRow.find('.glimpse-tl-summary-height').height(elements.summaryRow.height());
            };
            
        pubsub.subscribe('trigger.timline.shell.subscriptions', wireListeners); 
        pubsub.subscribe('trigger.timline.divider.render', render);
        pubsub.subscribe('trigger.timline.filtered', adjustHeight);  
    })(timeline.elements);

    // Render Events
    (function(elements) { 
        var wireListeners = function() {
                elements.summaryDescHolder
                    .delegate('.glimpse-tl-band', 'mouseenter', function() {
                         if ($(this).has('input:checked').length > 0) { $(this).find('input').stop(true, true).clearQueue().fadeIn(); }
                    })
                    .delegate('.glimpse-tl-band', 'mouseleave', function() {
                         if ($(this).has('input:checked').length > 0) { $(this).find('input').stop(true, true).clearQueue().fadeOut(); }
                    })
                    .delegate('input', 'click', function() {  categoryEvents($(this)); });
                //elements.summaryDescHolder.find('input').click(function() { /*filter.category();*/ pubsub.publish('trigger.timline.search.category'); });
            },
            render = function() {
                pubsub.publish('action.timline.event.rendering');

                processCategories();
                processEvents();
                processEventSummary();
                processTableData(); 
                
                //view.start();
                pubsub.publish('action.timline.event.rendered');
            },
            processTableData = function() {
                var dataResult = [ [ 'Title', 'Description', 'Category', 'Timing', 'Start Point', 'Duration', 'w/out Children' ] ],
                    metadata = [ [ { data : '{{0}} |({{1}})|' }, { data : 2, width : '18%' }, { data : 3, width : '9%' }, { data : 4, align : 'right', pre : 'T+ ', post : ' ms', className : 'mono', width : '100px' }, { data : 5, align : 'right', post : ' ms', className : 'mono', width : '100px' }, { data : 6, align : 'right', post : ' ms', className : 'mono', width : '100px' } ] ];
                    
                //Massage the data 
                for (var i = 0; i < timeline.data.events.length; i++) {
                    var event = timeline.data.events[i],
                        data = [ event.title, event.subText, event.category, '', event.startPoint, event.duration, event.childlessDuration ];
                    dataResult.push(data);
                } 

                //Insert it into the document
                var result = renderEngine.build(dataResult, metadata); 
                elements.contentTableHolder.append(result);

                //Update the output 
                elements.contentTableHolder.find('tbody tr').each(function(i) {
                    var row = $(this),
                        event = timeline.data.events[i],  
                        category = timeline.data.category[event.category];
                             
                    row.find('td:first-child').prepend($('<div class="glimpse-tl-event"></div>').css({ 'backgroundColor' : category.eventColor, marginLeft : (15 * event.nesting) + 'px', 'border' : '1px solid ' + category.eventColorHighlight }));
                    row.find('td:nth-child(3)').css('position', 'relative').prepend($('<div class="glimpse-tl-event"></div>').css({ 'backgroundColor' : category.eventColor, 'border' : '1px solid ' + category.eventColorHighlight, 'margin-left' : event.startPersent + '%', width : event.widthPersent + '%' })); 
                }); 
            },
            processCategories = function() {
                for (var categoryName in timeline.data.category) {
                    var category = timeline.data.category[categoryName];

                    elements.summaryDescHolder.append('<div class="glimpse-tl-band glimpse-tl-category-selected"><input type="checkbox" value="' + categoryName +'" checked="checked" /><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '"></div>' + categoryName +'</div>'); 
                    elements.summaryBandHolder.append('<div class="glimpse-tl-band"></div>');
                    category.holder = $('<div class="glimpse-tl-band"></div>').appendTo(elements.summaryEventHolder); 
                    category.events = {};
                }
            },
            processEvents = function() { 
                var eventStack = [], lastEvent = { startPoint : 0, duration : 0, childlessDuration : 0, endPoint : 0 };
                for (var i = 0; i < timeline.data.events.length; i += 1) {
                    var event = timeline.data.events[i],
                        topEvent = eventStack.length > 0 ? eventStack[eventStack.length - 1] : undefined,
                        category = timeline.data.category[event.category],
                        left = (event.startPoint / timeline.data.duration) * 100,
                        rLeft = Math.round(left),
                        actualWidth = (event.duration / timeline.data.duration) * 100,
                        width = actualWidth < 1 ? 0 : actualWidth,
                        rWidth = Math.round(width),
                        widthStyle = (width > 0 ? 'width:' + width + '%' : ''),
                        maxStyle = (width <= 0 ? 'max-width:7px;' : ''),
                        subTextPre = (event.subText ? '(' + event.subText + ')' : ''),
                        subText = (subTextPre ? '<span class="glimpse-tl-event-desc-sub">' + subTextPre + '</span>' : ''),
                        stackParsed = false;

                    event.endPoint = (parseFloat(event.startPoint) + parseFloat(event.duration)).toFixed(2);

                    //Derive event nesting  
                    while (!stackParsed) {
                        if (event.startPoint > lastEvent.startPoint && event.endPoint <= lastEvent.endPoint) {
                            eventStack.push(lastEvent); 
                            stackParsed = true;
                        }
                        else if (topEvent != undefined && topEvent.endPoint < event.endPoint) {
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
                        eventDecoration = '<div class="glimpse-tl-event-overlay-lh"><div class="glimpse-tl-event-overlay-li"></div><div class="glimpse-tl-event-overlay-lt">' + event.startPoint + ' ms</div></div>';
                    if (width > 0)
                        eventDecoration += '<div class="glimpse-tl-event-overlay-rh"><div class="glimpse-tl-event-overlay-ri"></div><div class="glimpse-tl-event-overlay-rt">' + event.endPoint + ' ms</div></div><div class="glimpse-tl-event-overlay-c">' + (width < 3.5 ? '...' : (event.duration + ' ms')) + '</div>';
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
            deriveEventSummary = function(category, left, rLeft, width, rWidth) {
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
            processEventSummary = function() { 
                var addCategoryEvent = function(category, start, finish) {
                    var width = (finish - start), 
                        widthStyle = (width > 0 ? 'width:' + width + '%' : ''),
                        maxStyle = (width <= 0 ? 'max-width:7px;' : ''); 
                    category.holder.append('<div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '; left:' + start + '%;' + widthStyle + maxStyle + '"></div>');
                };

                for (var categoryName in timeline.data.category) {
                    var category = timeline.data.category[categoryName], 
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
            colorRows = function(args) { 
                var filter = args.applyAll ? '' : ':visible';
                colorElement(elements.contentBandHolder.find('> div'), filter);
                colorElement(elements.contentDescHolder.find('> div'), filter);
                colorElement(elements.summaryBandHolder.find('> div'), filter);
                colorElement(elements.summaryDescHolder.find('> div'), filter);
                colorElement(elements.contentTableHolder.find('tbody'), filter); 
            },
            colorElement = function(scope, filter) {
                scope.removeClass('odd').removeClass('even');
                scope.filter(filter + ':even').addClass('even');
                scope.filter(filter + ':odd').addClass('odd');
            },
            categoryEvents = function(item) {
                //Handel how the UI will look
                var isChecked = item[0].checked, parent = item.parent();
                parent.animate({ 'opacity' : (isChecked ? 0.95 : 0.6) }, 200);
                elements.summaryEventHolder.find('.glimpse-tl-band').eq(parent.index()).animate({ 'opacity' : (isChecked ? 1 : 0.7) }, 200);

                //Trigger search
                //filter.category();
                pubsub.publish('trigger.timline.search.category'); 
            };
                 
        pubsub.subscribe('trigger.timline.shell.subscriptions', wireListeners); 
        pubsub.subscribe('trigger.timline.event.render', render);
        pubsub.subscribe('trigger.timline.filtered', colorRows); 
        pubsub.subscribe('action.timline.shell.switched', colorRows); 
    })(timeline.elements),  

    // Zoom
    (function(elements) {
        var positionLeft = function() {
                var persentLeft = (elements.zoomLeftHandle.position().left / elements.zoomHolder.width()) * 100, 
                    persentRight = (elements.zoomRightPadding.width() / elements.zoomHolder.width()) * 100; 
                 
                //Set left slider
                elements.zoomLeftHandle.css('left', persentLeft + '%'); 
                elements.zoomLeftPadding.css('width', persentLeft + '%');
                
                timeline.data.startTime = timeline.data.duration * (persentLeft / 100);

                //Force render
                //dividerBuilder.render(true);
                pubsub.publish('trigger.timline.divider.render', { force: true });

                //Zoom in on main line items 
                zoomEvents(persentLeft, persentRight);
                 
                //Manage zero display
                toggleZeroLine(persentLeft == 0); 
                 
                //Hide events that aren't needed
                //filter.zoom(persentLeft, persentRight); 
                pubsub.publish('trigger.timline.search.zoom', { persentLeft: persentLeft, persentRight: persentRight });
            },
            positionRight = function() {
                var persentRight = ((elements.zoomHolder.width() - 4 - elements.zoomRightHandle.position().left) / elements.zoomHolder.width()) * 100, 
                    persentLeft = (elements.zoomLeftPadding.width() / elements.zoomHolder.width()) * 100; 
                 
                //Set right slider
                elements.zoomRightHandle.css('right', persentRight + '%'); 
                elements.zoomRightPadding.css('width', persentRight + '%');

                timeline.data.endTime = timeline.data.duration - (timeline.data.duration * (persentRight / 100));
                
                //Force render
                //dividerBuilder.render(true);
                pubsub.publish('trigger.timline.divider.render', { force: true });
                
                //Zoom in on main line items 
                zoomEvents(persentLeft, persentRight);
                    
                //Hide events that aren't needed
                //filter.zoom(persentLeft, persentRight);  
                pubsub.publish('trigger.timline.search.zoom', { persentLeft: persentLeft, persentRight: persentRight });
            },
            zoomEvents = function(persentLeft, persentRight) {
                var offset = (100 / (100 - persentLeft - persentRight)) * -1, 
                    lOffset = offset * persentLeft, 
                    rOffset = offset * persentRight;

                elements.contentRow.find('.glimpse-tl-event-holder-inner').css({ left : lOffset + '%', right : rOffset + '%' });
            },
            toggleZeroLine = function(show) {
                elements.contentRow.find('.glimpse-tl-divider-zero-holder').toggle(show);  
                elements.contentRow.find('.glimpse-tl-divider-line-holder').css('left', (show ? '15' : '0') + 'px');
                elements.contentRow.find('.glimpse-tl-event-holder').css('marginLeft', (show ? '15' : '0') + 'px');
            }, 
            wireListeners = function() {
                jQueryGlimpse.draggable({
                    handelScope: elements.zoomLeftHandle,
                    opacityScope: elements.zoomLeftHandle,
                    resizeScope: elements.zoomLeftHandle,
                    valueStyle: 'left',
                    isUpDown: false,
                    offset: 1, 
                    min: 0,
                    max: function() { return (elements.zoomRightHandle.position().left - 20); },
                    dragging: function() { elements.zoomLeftHandle.css('left', (elements.zoomLeftHandle.position().left) + 'px'); console.log(elements.zoomLeftHandle.css('left')); },
                    dragged: function() { positionLeft(); }
                }); 
                jQueryGlimpse.draggable({
                    handelScope: elements.zoomRightHandle,
                    opacityScope: elements.zoomRightHandle,
                    resizeScope: elements.zoomRightHandle,
                    valueStyle: 'right',
                    isUpDown: false,
                    min: 0,
                    max: function () { return (elements.zoomHolder.width() - elements.zoomLeftHandle.position().left) - 20; },
                    dragging: function() { elements.zoomRightHandle.css('right', (elements.zoomHolder.width() - elements.zoomRightHandle.position().left) + 'px'); },
                    dragged: function() { positionRight(); }
                }); 
            };
                 
        pubsub.subscribe('trigger.timline.shell.subscriptions', wireListeners);  
    })(timeline.elements);
        
    // Filter
    (function(elements) {
        var criteria = { 
                persentLeft : 0, 
                persentRightFromLeft : 100, 
                hiddenCategories : undefined
            },
            search = function(c) {
                pubsub.publish('trigger.timline.filtering', { criteria: c });

                //Go through each event doing executing search
                for (var i = 0; i < timeline.data.events.length; i += 1) {
                    var event = timeline.data.events[i],
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
                
                pubsub.publish('trigger.timline.filtered', { criteria: c });  
            },
            zoom = function(args) {
                //Pull out current search
                criteria.persentLeft = args.persentLeft;
                criteria.persentRightFromLeft = 100 - args.persentRight; 
                    
                //Execute search
                search(criteria);
            },
            category = function() {
                //Pull out current search
                var hiddenCategoriesObj = {}, hiddenCategories = elements.summaryDescHolder.find('input:checked').map(function() { return $(this).val(); }).get(); //TODO: This shouldn't do this
                for (var i = 0; i < hiddenCategories.length; i++)
                    hiddenCategoriesObj[hiddenCategories[i]] = true;
                criteria.hiddenCategories = hiddenCategoriesObj;
                    
                //Execute search
                search(criteria);
            };
        
        pubsub.subscribe('trigger.timline.search.zoom', zoom);
        pubsub.subscribe('trigger.timline.search.category', category); 
    })(timeline.elements);

    // Info
    (function(elements) {
        var wireListeners = function() {
                elements.scope
                    .delegate('.glimpse-tl-event-info', 'mouseenter', function() { elements.eventInfo.stop(true, true).clearQueue(); })
                    .delegate('.glimpse-tl-event-info', 'mouseleave', function() { hideBubble(); });
            
                elements.contentRow
                    .delegate('.glimpse-tl-event-overlay', 'mouseenter', function() { showBubble($(this)); })
                    .delegate('.glimpse-tl-event-overlay', 'mouseleave', function() { hideBubble($(this)); });
            
                elements.contentEventHolder
                    .delegate('.glimpse-tl-band', 'mouseenter', function() { showTip($(this)); })
                    .delegate('.glimpse-tl-band', 'mouseleave', function() { hideTip($(this)); });
            },
            showTip = function(item) {
                item.find('.glimpse-tl-event-overlay').stop(true, true).fadeIn(); 
            },
            hideTip = function(item) {
                item.find('.glimpse-tl-event-overlay').stop(true, true).fadeOut(); 
            },
            buildBubbleDetails = function(event, category) {
                var details = '', detailKey;
                for (detailKey in event.details) {
                    details += '<tr><th>' + detailKey + '</th><td>' + event.details[detailKey] + '</td></tr>';
                }
                return '<table><tr><th colspan="2"><div class="glimpse-tl-event-info-title"><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '"></div>' + event.title + ' - Details</div></th></tr><tr><th>Duration</th><td>' + event.duration + ' ms (at ' + event.startPoint + ' ms' + ( + event.duration > 1 ? (' to ' + event.endPoint + ' ms') : '' ) +')</td></tr>' + (event.duration != event.childlessDuration ? '<tr><th>w/out Children</th><td>' + event.childlessDuration + ' ms</td></tr>' : '') + (event.subText ? '<tr><th>Details</th><td>' + event.subText + '</td></tr>' : '' ) + details + '</table>';
            },
            updateBubble = function(item) {
                var eventOffset = item.offset(), 
                    containerOffset = elements.eventInfo.parent().offset(),
                    eventSize = { height : item.height(), width : item.width() },
                    event = timeline.data.events[item.attr('data-timelineItemIndex')],
                    category = timeline.data.category[event.category],
                    content = buildBubbleDetails(event, category);
                     
                eventOffset.top -= containerOffset.top;
                eventOffset.left -= containerOffset.left;

                elements.eventInfo.html(content);
                     
                var detailSize = { height : elements.eventInfo.height(), width : elements.eventInfo.width() }, 
                    newDetailLeft = Math.min(Math.max((eventOffset.left + ((eventSize.width - detailSize.width) / 2)) - 15, 5), $(document).width() - detailSize.width - 30),
                    newDetailTop = eventOffset.top - detailSize.height - 20; 
                         
                elements.eventInfo.css('left', newDetailLeft + 'px');
                elements.eventInfo.css('top', newDetailTop + 'px');   
            },
            showBubble = function(item) {  
                elements.eventInfo.stop(true, true).clearQueue().delay(500).queue(function() { updateBubble(item); elements.eventInfo.show(); }); 
            },
            hideBubble = function() { 
                elements.eventInfo.stop(true, true).clearQueue().delay(500).fadeOut(); 
            };
                 
        pubsub.subscribe('trigger.timline.shell.subscriptions', wireListeners); 
    })(timeline.elements);

    // Resize
    (function(elements) {
        var wireListeners = function() { 
                jQueryGlimpse.draggable({
                    handelScope: elements.resizer,
                    opacityScope: elements.resizer,
                    resizeScope: elements.resizer,
                    valueStyle: 'left',
                    isUpDown: false,
                    offset: 1, 
                    min: 50,
                    max: 300,
                    dragged: function(options, position) { columnResize(position); }
                });
            },
            columnResize = function(position) {
                elements.scope.find('.glimpse-tl-col-side').width(position + 'px');
                elements.scope.find('.glimpse-tl-col-main').css('left', position + 'px');
                    
                //dividerBuilder.render(false);
                pubsub.publish('trigger.timline.divider.render', {});
            },
            panelResize = function() { 
                if (elements.scope) {
                    //Work out what heihgt we can work with
                    var contentHeight = settings.local('panelHeight') - (elements.summaryRow.height() + elements.scope.find('.glimpse-tl-row-spacer').height());  
                    elements.contentRow.height(contentHeight + 'px');
                    
                    //Render Divers
                    //dividerBuilder.render();
                    pubsub.publish('trigger.timline.divider.render', {});
                }
            };
        
        pubsub.subscribe('trigger.timline.shell.subscriptions', wireListeners); 
        pubsub.subscribe('trigger.shell.resize', panelResize); 
        pubsub.subscribe('action.panel.showed.Timeline', function() { setTimeout(panelResize, 1); }); 
    })(timeline.elements);

    // View
    (function(elements) {
        var wireListeners = function() { 
                elements.summaryRow.find('.glimpse-tl-band-title span').click(function() {
                    toggle(); 
                });
            },
            apply = function(showTimeline, isFirst) {
                if (showTimeline == undefined || showTimeline == null)
                    showTimeline = false;
                
                pubsub.publish('action.timline.shell.switching', { applyAll: isFirst, showTimeline: showTimeline });

                elements.contentTableHolder.parent().toggle(showTimeline); 
                elements.contentRow.find('.glimpse-tl-content-scroll:first-child').toggle(!showTimeline);
                elements.contentRow.find('.glimpse-tl-resizer').toggle(!showTimeline); 
                    
                
                if (!showTimeline) {
                    //dividerBuilder.render();
                    pubsub.publish('trigger.timline.divider.render', {});
                }
                
                //eventBuilder.colorRows(isFirst); 
                pubsub.publish('action.timline.shell.switched', { applyAll: isFirst, showTimeline: showTimeline });
            },
            toggle = function() {
                var showTimeline = !(settings.local('timelineView'));

                apply(showTimeline);
                 
                //glimpse.timeline.data.timeView = showTimeline;
                //glimpse.pubsub.publish('state.persist');
                settings.local('timelineView', showTimeline);
            },
            start = function() {
                //apply(glimpse.timeline.data.timeView, true);
                apply(settings.local('timelineView'), true);
            };
             
        pubsub.subscribe('trigger.timline.shell.subscriptions', wireListeners); 
        pubsub.subscribe('trigger.timline.shell.toggle', toggle);
        pubsub.subscribe('action.timline.event.rendered', start);
    })(timeline.elements);

    // Timline
    (function() { 
        var init = function() { 
                //Set defaults
                timeline.data.startTime = 0;
                timeline.data.endTime = timeline.data.duration;
                
                pubsub.publish('trigger.timline.shell.init');
                pubsub.publish('trigger.timline.shell.subscriptions');
                pubsub.publish('trigger.timline.event.render');
            },
            modify = function(options) {
                options.templates.css += '/*(import:glimpse.timeline.shell.css)*/';
            },
            prerender = function(args) {
                args.pluginData._data = args.pluginData.data;
                args.pluginData.data = 'Loading data, please wait...';
            },
            postrender = function(args) { 
                args.pluginData.data = args.pluginData._data;
                args.pluginData._data = undefined;
                
                timeline.data = args.pluginData.data;
                timeline.scope = args.panel;
                
                pubsub.publishAsync('trigger.timline.init');
            };
         
        pubsub.subscribe('action.template.processing', modify); 
        pubsub.subscribe('trigger.timline.init', init); 
        pubsub.subscribe('action.panel.rendering.Timeline', prerender);
        pubsub.subscribe('action.panel.rendered.Timeline', postrender);
    })();
})(jQueryGlimpse, glimpse.pubsub, glimpse.settings, glimpse.util, glimpse.render.engine);