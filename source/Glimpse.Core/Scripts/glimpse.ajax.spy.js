var XHRSpy = function () {
    this.requestHeaders = {};
    this.responseHeaders = {};
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
    mimeType: null,
    send: function () {
        $.glimpseAjax.callStarted(this);
    },
    finish: function () {
        $.glimpseAjax.callFinished(this);
    }
};

var XMLHttpRequestWrapper = function(activeXObject) { 
    // XMLHttpRequestWrapper internal variables

    var xhrRequest = (typeof activeXObject != "undefined" ? activeXObject : new _XMLHttpRequest()),
        spy = new XHRSpy(), 
        that = this,
        isIE6 = $.browser.msie && $.browser.version == "6.0",
        supportsApply = !isIE6 && xhrRequest && xhrRequest.open && typeof xhrRequest.open.apply != "undefined";
        numberOfXHRProperties = 0,
        updateSelfPropertiesIgnore = {  //XMLHttpRequestWrapper internal methods
            abort: 1,
            channel: 1,
            getAllResponseHeaders: 1,
            getInterface: 1,
            getResponseHeader: 1,
            mozBackgroundRequest: 1,
            multipart: 1,
            onreadystatechange: 1,
            open: 1,
            send: 1,
            setRequestHeader: 1
        },
        updateSelfProperties = function() {
            if (supportsXHRIterator) {
                for (var propName in xhrRequest) {
                    if (propName in updateSelfPropertiesIgnore)
                        continue; 
                    try {
                        var propValue = xhrRequest[propName]; 
                        if (propValue && !$.isFunction(propValue))
                            that[propName] = propValue;
                    }
                    catch(E) {  }
                }
            }
            else { 
                if (xhrRequest.readyState == 4) {
                    that.status = xhrRequest.status;
                    that.statusText = xhrRequest.statusText;
                    that.response = xhrRequest.response;
                    that.responseText = xhrRequest.responseText;
                    that.responseType = xhrRequest.responseType;
                    that.responseXML = xhrRequest.responseXML;  
                }
            }
        },
        updateXHRPropertiesIgnore = {
            channel: 1,
            onreadystatechange: 1,
            readyState: 1,
            responseBody: 1,
            responseText: 1,
            responseXML: 1,
            status: 1,
            statusText: 1,
            upload: 1
        },
        updateXHRProperties = function() {
            for (var propName in that) {
                if (propName in updateXHRPropertiesIgnore)
                    continue; 
                try {
                    var propValue = that[propName]; 
                    if (propValue && !xhrRequest[propName]) 
                        xhrRequest[propName] = propValue; 
                }
                catch(E) {  }
            }
        },
        finished = false,
        finishXHR = function () {
            if (finished) { return; }
            finished = true;
            
            var duration = new Date().getTime() - spy.startTime, 
                success = xhrRequest.status == 200;

            //Pull out the header information
            var responseHeadersText = xhrRequest.getAllResponseHeaders(), 
                responses = responseHeadersText ? responseHeadersText.split(/[\n\r]/) : [],
                regHeader = /^(\S+):\s*(.*)/;
            for (var i = 0, l = responses.length; i < l; i++) { 
                var text = responses[i], match = text.match(regHeader); 
                if (match)
                {
                    var name = match[1], value = match[2]; 
                    if (name == "Content-Type")
                        spy.mimeType = value; 
                    spy.responseHeaders[name] = value;
                }
            }
            spy.success = success;
            spy.loaded = true;
            spy.status = xhrRequest.status;
            spy.statusText = xhrRequest.statusText;
            spy.responseText = xhrRequest.responseText;
            spy.duration = duration; 
  
            //Trigger the finish a bit latter
            setTimeout(function () { spy.finish(); }, 200);
             
            updateSelfProperties();
        },
        handleStateChange = function () {
            that.readyState = xhrRequest.readyState;

            if (xhrRequest.readyState == 4) { 
                finishXHR();
                xhrRequest.onreadystatechange = function () { };
            }
            that.onreadystatechange();
        }; 
     
    this.readyState = 0; 
    this.onreadystatechange = function () { }; 
    this.open = function (method, url, async, user, password) { 
        updateSelfProperties();
            
        if (spy.loaded)
            spy = new XHRSpy();
            
        spy.method = method;
        spy.url = url;
        spy.href = url;
        spy.async = async;
        spy.xhrRequest = xhrRequest; 

        try {
            // xhrRequest.open.apply may not be available in IE
            if (supportsApply)
                xhrRequest.open.apply(xhrRequest, arguments);
            else
                xhrRequest.open(method, url, async, user, password);
        }
        catch(e) {
            throw e;
        }
        
        xhrRequest.onreadystatechange = handleStateChange;
    }; 
    this.send = function (data) {
        spy.data = data;
        spy.startTime = new Date().getTime();
            
        updateXHRProperties();
            
        try {
            xhrRequest.send(data);
        }
        catch (e) { 
            throw e;
        }
        finally {
            spy.send();
            if (!spy.async) {
                that.readyState = xhrRequest.readyState;
                try { finishXHR(); } 
                catch(E) { } 
            }
        }
    }; 
    this.setRequestHeader = function (header, value) {
        spy.requestHeaders[header] = value;
        xhrRequest.setRequestHeader(header, value);
    }; 
    this.getResponseHeader = function (header) {
        return xhrRequest.getResponseHeader(header);
    };   
    this.getAllResponseHeaders = function () {
        return xhrRequest.getAllResponseHeaders();
    }; 
    this.abort = function () {
        xhrRequest.abort();
        updateSelfProperties();
    };
         
    //Clone XHR object
    for (var propName in xhrRequest) {
        numberOfXHRProperties++;
        
        if (propName in updateSelfPropertiesIgnore)
            continue;
        
        try {
            var propValue = xhrRequest[propName];  
            if (isFunction(propValue)) {
                if (typeof that[propName] == "undefined") {
                    this[propName] = (function(name, xhr) { 
                        return supportsApply ? 
                            function() { return xhr[name].apply(xhr, arguments); } :
                            function(a,b,c,d,e) { return xhr[name](a,b,c,d,e); }; 
                    })(propName, xhrRequest);
                } 
            }
            else
                this[propName] = propValue;
        }
        catch(E) { }
    } 
    var supportsXHRIterator = numberOfXHRProperties > 0;
        
    return this;
};

var _ActiveXObject;
if ($.browser.msie && $.browser.version == "6.0") {
    _ActiveXObject = window.ActiveXObject;
    
    var xhrObjects = " MSXML2.XMLHTTP.5.0 MSXML2.XMLHTTP.4.0 MSXML2.XMLHTTP.3.0 MSXML2.XMLHTTP Microsoft.XMLHTTP ";
    
    window.ActiveXObject = function(name) {
        var error = null; 
        try {
            var activeXObject = new _ActiveXObject(name);
        }
        catch(e) {
            error = e;
        }
        finally {
            if (!error) {
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
    };
}