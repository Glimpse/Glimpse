var XHRSpy = function()
{
    this.requestHeaders = [];
    this.responseHeaders = [];
};

XHRSpy.prototype = 
{
    method: null,
    url: null,
    href: null, 
    async: null, 
    xhrRequest: null, 
    loaded: false, 
    success: false,
    status: null,
    statusText: null,
    responseText: null, 
    requestHeaders: null,
    responseHeaders: null, 
    startTime: null,
    duration: null, 
    logRow: null,
    send: function() {
        $.glimpseAjax.callStarted(this);
    },
    finish: function() { 
        $.glimpseAjax.callFinished(this); 
    }
};

var XMLHttpRequestWrapper = function(activeXObject)
{
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper internal variables
    
    var xhrRequest = (typeof activeXObject != "undefined" ? activeXObject : new _XMLHttpRequest()), 
        spy = new XHRSpy(), that = this;

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper internal methods
    
    var finishXHR = function() {
        var duration = new Date().getTime() - spy.startTime;
        var success = xhrRequest.status == 200;
        
        //Pull out the header information
        var responseHeadersText = xhrRequest.getAllResponseHeaders();
        var responses = responseHeadersText ? responseHeadersText.split(/[\n\r]/) : [];
        var reHeader = /^(\S+):\s*(.*)/; 
        for (var i = 0, l=responses.length; i<l; i++)
        {
            var text = responses[i];
            var match = text.match(reHeader);
            if (match)
            {
                spy.responseHeaders.push({
                   name: [match[1]],
                   value: [match[2]]
                });
            }
        }
        
        //Trigger the finish a bit latter
        setTimeout(function(){ spy.finish(); }, 200);
        
        //Get the rest of the information
        spy.success = success;
        spy.loaded = true;
        spy.status = xhrRequest.status;
        spy.statusText = xhrRequest.statusText;
        spy.responseText = xhrRequest.responseText; 
        spy.duration = duration;
    };
    
    var handleStateChange = function() { 
        that.readyState = xhrRequest.readyState;
        
        if (xhrRequest.readyState == 4)
        {
            finishXHR(); 
            xhrRequest.onreadystatechange = function(){};
        } 
        that.onreadystatechange();
    };
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper public properties and handlers
    
    this.readyState = 0;
    
    this.onreadystatechange = function(){};
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper public methods
    
    this.open = function(method, url, async) {
        console.log("xhrRequest open");
        
        if (spy.loaded)
            spy = new XHRSpy(); 
        spy.method = method;
        spy.url = url;
        spy.href = url;
        spy.async = async;
        spy.xhrRequest = xhrRequest;
        //spy.urlParams = parseURLParamsArray(url);
        
        if (!$.browser.msie && async)                                                 //TODO: Change over to jQuery
            xhrRequest.onreadystatechange = handleStateChange;
        
        // xhr.open.apply not available in IE
        if (xhrRequest.open.apply)                                              //TODO: Need to see if this applies
            xhrRequest.open.apply(xhrRequest, arguments)
        else 
            xhrRequest.open(method, url, async);
        
        if ($.browser.msie && async)                                                 //TODO: Change over to jQuery
            xhrRequest.onreadystatechange = handleStateChange;
        
    };
     
    this.send = function(data) {
        console.log("xhrRequest send");
        
        spy.data = data; 
        spy.startTime = new Date().getTime();
        
        try {
            xhrRequest.send(data);
        }
        catch(e) {
            throw e;
        }
        finally {
            spy.send(); 
            if (!spy.async) {
                that.readyState = xhrRequest.readyState; 
                finishXHR();
            }
        }
    };
     
    this.setRequestHeader = function(header, value) {
        spy.requestHeaders.push({name: [header], value: [value]});
        xhrRequest.setRequestHeader(header, value);
    };
     
    this.getResponseHeader = function(header) {
        return xhrRequest.getResponseHeader(header);
    };
     
    this.getAllResponseHeaders = function() {
        return xhrRequest.getAllResponseHeaders();
    };
     
    this.abort = function() {
        return xhrRequest.abort();
    };
    
    return this;
};

// ************************************************************************************************
// Reguster XMLHttpRequest Wrapper / ActiveXObject Wrapper (IE6 only)

var _ActiveXObject; 
if ($.browser.msie && $.browser.version=="6.0") {
    window._ActiveXObject = window.ActiveXObject; 
    window.ActiveXObject = function(name)
    {
        var error = null;
        
        try {
            var activeXObject = new window._ActiveXObject(name);
        }
        catch(e) {
            error = e;
        }
        finally {
            if (!error) {
                var xhrObjects = " MSXML2.XMLHTTP.5.0 MSXML2.XMLHTTP.4.0 MSXML2.XMLHTTP.3.0 MSXML2.XMLHTTP Microsoft.XMLHTTP ";
                if (xhrObjects.indexOf(" " + name + " ") != -1)
                    return new XMLHttpRequestWrapper(activeXObject);
                else
                    return activeXObject;
            }
            else
                throw error.message;
        }
    };
} 
else {
    var _XMLHttpRequest = XMLHttpRequest;
    window.XMLHttpRequest = function() {
        return new XMLHttpRequestWrapper();
    }
}