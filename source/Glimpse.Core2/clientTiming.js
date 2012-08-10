var glimpseClientTimelinePlugin = (function ($, glimpse) {
    var //Support
        wireListener = function () {
            glimpse.pubsub.subscribe('state.timeline.build.prerender', function (topic, payload) { timelinePrerender(payload); }); //Bind to the Timeline 'prerender' event
        },

    //Just before the Timeline renders
        timelinePrerender = function (data) {
            var 
                t = window.performance.timing, //Timing API
                nav = window.performance.navigation, //Navigation API
                navType; //To store the string which describes the user's method of navigation to the page later on

            if (t == null || nav == null) //W3 Spec: "When these [Navigation & Timing API] interfaces are disabled, both ... must return a null value."
                return;

            if (t.domComplete === 0) //DOM isn't ready - we can't do our job properly, so it's better to do nothing than screw up
                return;

            switch (nav.type) { //How did the user get to the page?
                case 0: //"Navigation started by clicking on a link, or entering the URL in the user agent's address bar, or form submission..."
                    navType = "Regular User Action";
                    break;
                case 1: //"Navigation through the reload operation"
                    navType = "Page Reload";
                    break;
                case 2: //"Navigation through a history traversal operation"
                    navType = "History Navigation";
                    break;
                default:
                    navType = null;
            }

            var 
                browser = { //JSON structure that stores all our data
                    "duration": (t.loadEventEnd - t.navigationStart),
                    "category": {
                        "Browser": {
                            "eventColorHighlight": "#636148",
                            "eventColor": "#737154"
                        },
                        "Client Network": {
                            "eventColorHighlight": "#A4F9AE",
                            "eventColor": "#BCFBC3"
                        },
                        "Server Network": {
                            "eventColorHighlight": "#D7C387",
                            "eventColor": "#DECD9C"
                        }
                    },
                    "events": [
                        {
                            "category": "Browser",
                            "startTime": t.navigationStart,
                            "startPoint": 0,
                            "duration": (t.redirectStart ? (t.redirectStart - t.navigationStart) : 0),
                            "title": "Navigation",
                            "subText": navType != null ? ("Via " + navType) : null,
                            "details": null
                        },
                        {
                            "category": "Client Network",
                            "startTime": t.redirectStart,
                            "startPoint": (t.redirectStart ? (t.redirectStart - t.navigationStart) : 0),
                            "duration": (t.redirectEnd - t.redirectStart),
                            "title": "Redirection",
                            "subText": (nav.redirectCount + " Times"),
                            "details": null
                        },
                        {
                            "category": "Browser",
                            "startTime": t.fetchStart,
                            "startPoint": (t.fetchStart - t.navigationStart),
                            "duration": (t.domainLookupStart - t.fetchStart),
                            "title": "Cache",
                            "subText": null,
                            "details": null
                        },
                        {
                            "category": "Client Network",
                            "startTime": t.domainLookupStart,
                            "startPoint": (t.domainLookupStart - t.navigationStart),
                            "duration": (t.domainLookupEnd - t.domainLookupStart),
                            "title": "DNS",
                            "subText": null,
                            "details": null
                        },
                        {
                            "category": "Server Network",
                            "startTime": t.connectStart,
                            "startPoint": (t.connectStart - t.navigationStart),
                            "duration": (t.connectEnd - t.connectStart),
                            "title": "Connection",
                            "subText": null,
                            "details": null
                        },
                        {
                            "category": "Browser",
                            "startTime": t.requestStart,
                            "startPoint": (t.requestStart - t.navigationStart),
                            "duration": (t.responseStart - t.requestStart),
                            "title": "Request",
                            "subText": null,
                            "details": null
                        },
                        {
                            "category": "Server Network",
                            "startTime": t.responseStart,
                            "startPoint": (t.responseStart - t.navigationStart),
                            "duration": (t.responseEnd - t.responseStart),
                            "title": "Response",
                            "subText": null,
                            "details": null
                        },
                        {
                            "category": "Browser",
                            "startTime": t.domLoading,
                            "startPoint": (t.domLoading - t.navigationStart),
                            "duration": (t.domComplete - t.domLoading),
                            "title": "Processing",
                            "subText": null,
                            "details": null
                        },
                        {
                            "category": "Browser",
                            "startTime": t.loadEventStart,
                            "startPoint": (t.loadEventStart - t.navigationStart),
                            "duration": (t.loadEventEnd - t.loadEventStart),
                            "title": "DOM Load",
                            "subText": null,
                            "details": null
                        }
                    ]
                },
                i = 0, //Variable for iteration
                eventsBeforeServer = [], eventsAfterServer = [], //Arrays to hold our events before and after the server stuff
                serverDuration = parseInt(data.duration, 10), //The duration of the server-side information
                sufficientRequest = false; //Whether the client request time is long enough to display at the same time as the server-side information to show a better view of the whole process


            if (browser.events[5].duration > serverDuration) {
                sufficientRequest = true;
                data.duration = browser.duration; //The total duration will only be as long as the client events
            }
            else
                data.duration = serverDuration + browser.duration; //The total duration will be the client and the server events, as they'll be seperately displayed

            for (i in browser.category) //Loop through our categories
            {
                if (browser.category.hasOwnProperty(i))
                    data.category[i] = browser.category[i];
            }

            for (i = 0; i < data.events.length; i++) //Loop through pre-existing events to push forward the "startPoint"s as appropriate
            {
                if (browser.events[5].startPoint > 0)
                    data.events[i].startPoint += ((sufficientRequest) ? (browser.events[5].duration - serverDuration) : (browser.events[5].startPoint));
            }

            var serverEnd = data.events[data.events.length - 1].startPoint + data.events[data.events.length - 1].duration; //Time of the end of server-side information

            for (i = 0; i < browser.events.length; i++) //Loop through our events
            {
                if (browser.events[i].duration > 0 || (browser.events[i].title === 'DOM Load' || browser.events[i].title === 'Navigation')) { //TODO: Clean this up a little, comparing to titles isn't ideal
                    if (i >= 6) //If we're after the request, the event occurs after the server stuff
                    {
                        if (!sufficientRequest)
                            browser.events[i].startPoint += serverEnd;

                        if (browser.events[i].startPoint < serverEnd)
                            browser.events[i].startPoint = serverEnd;

                        eventsAfterServer.push(browser.events[i]);
                    }
                    else //If we're before the request, the event occurs before the server stuff
                    {
                        if (browser.events[i].startPoint < 0) //Sometimes things are negative -- make them 0
                            browser.events[i].startPoint = 0;

                        eventsBeforeServer.push(browser.events[i]);
                    }
                }
            }

            [ ].push.apply(data.events, eventsAfterServer); //Add our events before the server stuff
            [ ].unshift.apply(data.events, eventsBeforeServer); //Add our events after the server stuff
        },

    //Main 
        init = function () {
            wireListener();
        };

    init();
} ($Glimpse, glimpse));