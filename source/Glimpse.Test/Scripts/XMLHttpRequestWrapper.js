var XHRSpy = function()
{
    this.requestHeaders = [];
    this.responseHeaders = [];
};

XHRSpy.prototype = 
{
    method: null,
    url: null,
    async: null, 
    xhrRequest: null, 
    href: null, 
    loaded: false, 
    logRow: null,  
    status: null,
    statusText: null,
    responseText: null, 
    requestHeaders: null,
    responseHeaders: null, 
    time: null,
    sourceLink: null, // {href:"file.html", line: 22} 
    getURL: function()
    {
        return this.href;
    },
    send: function() {
        var row = Firebug.Console.log(spy, null, "spy", Firebug.Spy.XHR); 
        if (row)
        {
            setClass(row, "loading");
            spy.logRow = row;
        }
    },
    finish: function() { 
        // update row information to avoid "ethernal spinning gif" bug in IE 
        row = row || spy.logRow;
                
        // if chrome document is not loaded, there will be no row yet, so just ignore
        if (!row) return;
                
        $(row).removeClass("loading"); 
        if (!success)
            $(row).addClass("error");
                
        var item = $(".spyStatus", row).text(status); 
        var item = $(".spyTime", row).text(time + "ms");
    }
};

var XMLHttpRequestWrapper = function(activeXObject)
{
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper internal variables
    
    var xhrRequest = (typeof activeXObject != "undefined" ? activeXObject : new _XMLHttpRequest()), 
        spy = new XHRSpy(), that = this, reqType, reqUrl, reqStartTS, handler = new glimpseXhrHandler(spy);

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper internal methods
    
    var finishXHR = function() 
    {
        var duration = new Date().getTime() - reqStartTS;
        var success = xhrRequest.status == 200;
        
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
       
        setTimeout(function(){ spy.finish(); }, 200);
        
        spy.loaded = true;
        spy.status = xhrRequest.status;
        spy.statusText = xhrRequest.statusText;
        spy.responseText = xhrRequest.responseText; 
        spy.time: duration;
    };
    
    var handleStateChange = function()
    {
        console.log("onreadystatechange");
        
        that.readyState = xhrRequest.readyState;
        
        if (xhrRequest.readyState == 4)
        {
            finishXHR(); 
            xhrRequest.onreadystatechange = function(){};
        }
        
        console.log(spy.url + ": " + xhrRequest.readyState);
        that.onreadystatechange();
    };
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper public properties and handlers
    
    this.readyState = 0;
    
    this.onreadystatechange = function(){};
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // XMLHttpRequestWrapper public methods
    
    this.open = function(method, url, async)
    {
        console.log("xhrRequest open");
        
        if (spy.loaded)
            spy = new XHRSpy();
        
        spy.method = method;
        spy.url = url;
        spy.async = async;
        spy.href = url;
        spy.xhrRequest = xhrRequest;
        spy.urlParams = parseURLParamsArray(url);
        
        if (!FBL.isIE && async)                                                 //TODO: Change over to jQuery
            xhrRequest.onreadystatechange = handleStateChange;
        
        // xhr.open.apply not available in IE
        if (xhrRequest.open.apply)                                              //TODO: Need to see if this applies
            xhrRequest.open.apply(xhrRequest, arguments)
        else
            // TODO: xxxpedro user and pass parameters?
            xhrRequest.open(method, url, async);
        
        if (FBL.isIE && async)                                                 //TODO: Change over to jQuery
            xhrRequest.onreadystatechange = handleStateChange;
        
    };
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    
    this.send = function(data)
    {
        //Firebug.Console.log("xhrRequest send");
        
        spy.data = data;
        
        reqStartTS = new Date().getTime();
        
        try
        {
            xhrRequest.send(data);
        }
        catch(e)
        {
            throw e;
        }
        finally
        {
            spy.send(); 
            if (!spy.async)
            {
                that.readyState = xhrRequest.readyState; 
                finishXHR();
            }
        }
    };
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    
    this.setRequestHeader = function(header, value)
    {
        spy.requestHeaders.push({name: [header], value: [value]});
        xhrRequest.setRequestHeader(header, value);
    };
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    
    this.getResponseHeader = function(header)
    {
        return xhrRequest.getResponseHeader(header);
    };
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    
    this.getAllResponseHeaders = function()
    {
        return xhrRequest.getAllResponseHeaders();
    };
    
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    
    this.abort = function()
    {
        return xhrRequest.abort();
    };
    
    return this;
};

// ************************************************************************************************
// Reguster XMLHttpRequest Wrapper / ActiveXObject Wrapper (IE6 only)

var _ActiveXObject;
var isIE6 =  /msie 6/i.test(navigator.appVersion);

if (isIE6) {
    window._ActiveXObject = window.ActiveXObject;
    
    var xhrObjects = " MSXML2.XMLHTTP.5.0 MSXML2.XMLHTTP.4.0 MSXML2.XMLHTTP.3.0 MSXML2.XMLHTTP Microsoft.XMLHTTP ";
    
    window.ActiveXObject = function(name)
    {
        var error = null;
        
        try
        {
            var activeXObject = new window._ActiveXObject(name);
        }
        catch(e)
        {
            error = e;
        }
        finally
        {
            if (!error)
            {
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
    window.XMLHttpRequest = function()
    {
        return new XMLHttpRequestWrapper();
    }
}