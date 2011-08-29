
    //#region jQueryGlimpse
/*!
 * jQuery JavaScript Library v1.6.2
 * http://jquery.com/
 *
 * Copyright 2011, John Resig
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * Includes Sizzle.js
 * http://sizzlejs.com/
 * Copyright 2011, The Dojo Foundation
 * Released under the MIT, BSD, and GPL Licenses.
 *
 * Date: Thu Jun 30 14:16:56 2011 -0400
 */
(function(a,b){function cv(a){return f.isWindow(a)?a:a.nodeType===9?a.defaultView||a.parentWindow:!1}function cs(a){if(!cg[a]){var b=c.body,d=f("<"+a+">").appendTo(b),e=d.css("display");d.remove();if(e==="none"||e===""){ch||(ch=c.createElement("iframe"),ch.frameBorder=ch.width=ch.height=0),b.appendChild(ch);if(!ci||!ch.createElement)ci=(ch.contentWindow||ch.contentDocument).document,ci.write((c.compatMode==="CSS1Compat"?"<!doctype html>":"")+"<html><body>"),ci.close();d=ci.createElement(a),ci.body.appendChild(d),e=f.css(d,"display"),b.removeChild(ch)}cg[a]=e}return cg[a]}function cr(a,b){var c={};f.each(cm.concat.apply([],cm.slice(0,b)),function(){c[this]=a});return c}function cq(){cn=b}function cp(){setTimeout(cq,0);return cn=f.now()}function cf(){try{return new a.ActiveXObject("Microsoft.XMLHTTP")}catch(b){}}function ce(){try{return new a.XMLHttpRequest}catch(b){}}function b$(a,c){a.dataFilter&&(c=a.dataFilter(c,a.dataType));var d=a.dataTypes,e={},g,h,i=d.length,j,k=d[0],l,m,n,o,p;for(g=1;g<i;g++){if(g===1)for(h in a.converters)typeof h=="string"&&(e[h.toLowerCase()]=a.converters[h]);l=k,k=d[g];if(k==="*")k=l;else if(l!=="*"&&l!==k){m=l+" "+k,n=e[m]||e["* "+k];if(!n){p=b;for(o in e){j=o.split(" ");if(j[0]===l||j[0]==="*"){p=e[j[1]+" "+k];if(p){o=e[o],o===!0?n=p:p===!0&&(n=o);break}}}}!n&&!p&&f.error("No conversion from "+m.replace(" "," to ")),n!==!0&&(c=n?n(c):p(o(c)))}}return c}function bZ(a,c,d){var e=a.contents,f=a.dataTypes,g=a.responseFields,h,i,j,k;for(i in g)i in d&&(c[g[i]]=d[i]);while(f[0]==="*")f.shift(),h===b&&(h=a.mimeType||c.getResponseHeader("content-type"));if(h)for(i in e)if(e[i]&&e[i].test(h)){f.unshift(i);break}if(f[0]in d)j=f[0];else{for(i in d){if(!f[0]||a.converters[i+" "+f[0]]){j=i;break}k||(k=i)}j=j||k}if(j){j!==f[0]&&f.unshift(j);return d[j]}}function bY(a,b,c,d){if(f.isArray(b))f.each(b,function(b,e){c||bC.test(a)?d(a,e):bY(a+"["+(typeof e=="object"||f.isArray(e)?b:"")+"]",e,c,d)});else if(!c&&b!=null&&typeof b=="object")for(var e in b)bY(a+"["+e+"]",b[e],c,d);else d(a,b)}function bX(a,c,d,e,f,g){f=f||c.dataTypes[0],g=g||{},g[f]=!0;var h=a[f],i=0,j=h?h.length:0,k=a===bR,l;for(;i<j&&(k||!l);i++)l=h[i](c,d,e),typeof l=="string"&&(!k||g[l]?l=b:(c.dataTypes.unshift(l),l=bX(a,c,d,e,l,g)));(k||!l)&&!g["*"]&&(l=bX(a,c,d,e,"*",g));return l}function bW(a){return function(b,c){typeof b!="string"&&(c=b,b="*");if(f.isFunction(c)){var d=b.toLowerCase().split(bN),e=0,g=d.length,h,i,j;for(;e<g;e++)h=d[e],j=/^\+/.test(h),j&&(h=h.substr(1)||"*"),i=a[h]=a[h]||[],i[j?"unshift":"push"](c)}}}function bA(a,b,c){var d=b==="width"?a.offsetWidth:a.offsetHeight,e=b==="width"?bv:bw;if(d>0){c!=="border"&&f.each(e,function(){c||(d-=parseFloat(f.css(a,"padding"+this))||0),c==="margin"?d+=parseFloat(f.css(a,c+this))||0:d-=parseFloat(f.css(a,"border"+this+"Width"))||0});return d+"px"}d=bx(a,b,b);if(d<0||d==null)d=a.style[b]||0;d=parseFloat(d)||0,c&&f.each(e,function(){d+=parseFloat(f.css(a,"padding"+this))||0,c!=="padding"&&(d+=parseFloat(f.css(a,"border"+this+"Width"))||0),c==="margin"&&(d+=parseFloat(f.css(a,c+this))||0)});return d+"px"}function bm(a,b){b.src?f.ajax({url:b.src,async:!1,dataType:"script"}):f.globalEval((b.text||b.textContent||b.innerHTML||"").replace(be,"/*$0*/")),b.parentNode&&b.parentNode.removeChild(b)}function bl(a){f.nodeName(a,"input")?bk(a):"getElementsByTagName"in a&&f.grep(a.getElementsByTagName("input"),bk)}function bk(a){if(a.type==="checkbox"||a.type==="radio")a.defaultChecked=a.checked}function bj(a){return"getElementsByTagName"in a?a.getElementsByTagName("*"):"querySelectorAll"in a?a.querySelectorAll("*"):[]}function bi(a,b){var c;if(b.nodeType===1){b.clearAttributes&&b.clearAttributes(),b.mergeAttributes&&b.mergeAttributes(a),c=b.nodeName.toLowerCase();if(c==="object")b.outerHTML=a.outerHTML;else if(c!=="input"||a.type!=="checkbox"&&a.type!=="radio"){if(c==="option")b.selected=a.defaultSelected;else if(c==="input"||c==="textarea")b.defaultValue=a.defaultValue}else a.checked&&(b.defaultChecked=b.checked=a.checked),b.value!==a.value&&(b.value=a.value);b.removeAttribute(f.expando)}}function bh(a,b){if(b.nodeType===1&&!!f.hasData(a)){var c=f.expando,d=f.data(a),e=f.data(b,d);if(d=d[c]){var g=d.events;e=e[c]=f.extend({},d);if(g){delete e.handle,e.events={};for(var h in g)for(var i=0,j=g[h].length;i<j;i++)f.event.add(b,h+(g[h][i].namespace?".":"")+g[h][i].namespace,g[h][i],g[h][i].data)}}}}function bg(a,b){return f.nodeName(a,"table")?a.getElementsByTagName("tbody")[0]||a.appendChild(a.ownerDocument.createElement("tbody")):a}function W(a,b,c){b=b||0;if(f.isFunction(b))return f.grep(a,function(a,d){var e=!!b.call(a,d,a);return e===c});if(b.nodeType)return f.grep(a,function(a,d){return a===b===c});if(typeof b=="string"){var d=f.grep(a,function(a){return a.nodeType===1});if(R.test(b))return f.filter(b,d,!c);b=f.filter(b,d)}return f.grep(a,function(a,d){return f.inArray(a,b)>=0===c})}function V(a){return!a||!a.parentNode||a.parentNode.nodeType===11}function N(a,b){return(a&&a!=="*"?a+".":"")+b.replace(z,"`").replace(A,"&")}function M(a){var b,c,d,e,g,h,i,j,k,l,m,n,o,p=[],q=[],r=f._data(this,"events");if(!(a.liveFired===this||!r||!r.live||a.target.disabled||a.button&&a.type==="click")){a.namespace&&(n=new RegExp("(^|\\.)"+a.namespace.split(".").join("\\.(?:.*\\.)?")+"(\\.|$)")),a.liveFired=this;var s=r.live.slice(0);for(i=0;i<s.length;i++)g=s[i],g.origType.replace(x,"")===a.type?q.push(g.selector):s.splice(i--,1);e=f(a.target).closest(q,a.currentTarget);for(j=0,k=e.length;j<k;j++){m=e[j];for(i=0;i<s.length;i++){g=s[i];if(m.selector===g.selector&&(!n||n.test(g.namespace))&&!m.elem.disabled){h=m.elem,d=null;if(g.preType==="mouseenter"||g.preType==="mouseleave")a.type=g.preType,d=f(a.relatedTarget).closest(g.selector)[0],d&&f.contains(h,d)&&(d=h);(!d||d!==h)&&p.push({elem:h,handleObj:g,level:m.level})}}}for(j=0,k=p.length;j<k;j++){e=p[j];if(c&&e.level>c)break;a.currentTarget=e.elem,a.data=e.handleObj.data,a.handleObj=e.handleObj,o=e.handleObj.origHandler.apply(e.elem,arguments);if(o===!1||a.isPropagationStopped()){c=e.level,o===!1&&(b=!1);if(a.isImmediatePropagationStopped())break}}return b}}function K(a,c,d){var e=f.extend({},d[0]);e.type=a,e.originalEvent={},e.liveFired=b,f.event.handle.call(c,e),e.isDefaultPrevented()&&d[0].preventDefault()}function E(){return!0}function D(){return!1}function m(a,c,d){var e=c+"defer",g=c+"queue",h=c+"mark",i=f.data(a,e,b,!0);i&&(d==="queue"||!f.data(a,g,b,!0))&&(d==="mark"||!f.data(a,h,b,!0))&&setTimeout(function(){!f.data(a,g,b,!0)&&!f.data(a,h,b,!0)&&(f.removeData(a,e,!0),i.resolve())},0)}function l(a){for(var b in a)if(b!=="toJSON")return!1;return!0}function k(a,c,d){if(d===b&&a.nodeType===1){var e="data-"+c.replace(j,"$1-$2").toLowerCase();d=a.getAttribute(e);if(typeof d=="string"){try{d=d==="true"?!0:d==="false"?!1:d==="null"?null:f.isNaN(d)?i.test(d)?f.parseJSON(d):d:parseFloat(d)}catch(g){}f.data(a,c,d)}else d=b}return d}var c=a.document,d=a.navigator,e=a.location,f=function(){function J(){if(!e.isReady){try{c.documentElement.doScroll("left")}catch(a){setTimeout(J,1);return}e.ready()}}var e=function(a,b){return new e.fn.init(a,b,h)},f=a.jQueryGlimpse,g=a.$Glimpse,h,i=/^(?:[^<]*(<[\w\W]+>)[^>]*$|#([\w\-]*)$)/,j=/\S/,k=/^\s+/,l=/\s+$/,m=/\d/,n=/^<(\w+)\s*\/?>(?:<\/\1>)?$/,o=/^[\],:{}\s]*$/,p=/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g,q=/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,r=/(?:^|:|,)(?:\s*\[)+/g,s=/(webkit)[ \/]([\w.]+)/,t=/(opera)(?:.*version)?[ \/]([\w.]+)/,u=/(msie) ([\w.]+)/,v=/(mozilla)(?:.*? rv:([\w.]+))?/,w=/-([a-z])/ig,x=function(a,b){return b.toUpperCase()},y=d.userAgent,z,A,B,C=Object.prototype.toString,D=Object.prototype.hasOwnProperty,E=Array.prototype.push,F=Array.prototype.slice,G=String.prototype.trim,H=Array.prototype.indexOf,I={};e.fn=e.prototype={constructor:e,init:function(a,d,f){var g,h,j,k;if(!a)return this;if(a.nodeType){this.context=this[0]=a,this.length=1;return this}if(a==="body"&&!d&&c.body){this.context=c,this[0]=c.body,this.selector=a,this.length=1;return this}if(typeof a=="string"){a.charAt(0)!=="<"||a.charAt(a.length-1)!==">"||a.length<3?g=i.exec(a):g=[null,a,null];if(g&&(g[1]||!d)){if(g[1]){d=d instanceof e?d[0]:d,k=d?d.ownerDocument||d:c,j=n.exec(a),j?e.isPlainObject(d)?(a=[c.createElement(j[1])],e.fn.attr.call(a,d,!0)):a=[k.createElement(j[1])]:(j=e.buildFragment([g[1]],[k]),a=(j.cacheable?e.clone(j.fragment):j.fragment).childNodes);return e.merge(this,a)}h=c.getElementById(g[2]);if(h&&h.parentNode){if(h.id!==g[2])return f.find(a);this.length=1,this[0]=h}this.context=c,this.selector=a;return this}return!d||d.jquery?(d||f).find(a):this.constructor(d).find(a)}if(e.isFunction(a))return f.ready(a);a.selector!==b&&(this.selector=a.selector,this.context=a.context);return e.makeArray(a,this)},selector:"",jquery:"1.6.2",length:0,size:function(){return this.length},toArray:function(){return F.call(this,0)},get:function(a){return a==null?this.toArray():a<0?this[this.length+a]:this[a]},pushStack:function(a,b,c){var d=this.constructor();e.isArray(a)?E.apply(d,a):e.merge(d,a),d.prevObject=this,d.context=this.context,b==="find"?d.selector=this.selector+(this.selector?" ":"")+c:b&&(d.selector=this.selector+"."+b+"("+c+")");return d},each:function(a,b){return e.each(this,a,b)},ready:function(a){e.bindReady(),A.done(a);return this},eq:function(a){return a===-1?this.slice(a):this.slice(a,+a+1)},first:function(){return this.eq(0)},last:function(){return this.eq(-1)},slice:function(){return this.pushStack(F.apply(this,arguments),"slice",F.call(arguments).join(","))},map:function(a){return this.pushStack(e.map(this,function(b,c){return a.call(b,c,b)}))},end:function(){return this.prevObject||this.constructor(null)},push:E,sort:[].sort,splice:[].splice},e.fn.init.prototype=e.fn,e.extend=e.fn.extend=function(){var a,c,d,f,g,h,i=arguments[0]||{},j=1,k=arguments.length,l=!1;typeof i=="boolean"&&(l=i,i=arguments[1]||{},j=2),typeof i!="object"&&!e.isFunction(i)&&(i={}),k===j&&(i=this,--j);for(;j<k;j++)if((a=arguments[j])!=null)for(c in a){d=i[c],f=a[c];if(i===f)continue;l&&f&&(e.isPlainObject(f)||(g=e.isArray(f)))?(g?(g=!1,h=d&&e.isArray(d)?d:[]):h=d&&e.isPlainObject(d)?d:{},i[c]=e.extend(l,h,f)):f!==b&&(i[c]=f)}return i},e.extend({noConflict:function(b){a.$Glimpse===e&&(a.$Glimpse=g),b&&a.jQueryGlimpse===e&&(a.jQueryGlimpse=f);return e},isReady:!1,readyWait:1,holdReady:function(a){a?e.readyWait++:e.ready(!0)},ready:function(a){if(a===!0&&!--e.readyWait||a!==!0&&!e.isReady){if(!c.body)return setTimeout(e.ready,1);e.isReady=!0;if(a!==!0&&--e.readyWait>0)return;A.resolveWith(c,[e]),e.fn.trigger&&e(c).trigger("ready").unbind("ready")}},bindReady:function(){if(!A){A=e._Deferred();if(c.readyState==="complete")return setTimeout(e.ready,1);if(c.addEventListener)c.addEventListener("DOMContentLoaded",B,!1),a.addEventListener("load",e.ready,!1);else if(c.attachEvent){c.attachEvent("onreadystatechange",B),a.attachEvent("onload",e.ready);var b=!1;try{b=a.frameElement==null}catch(d){}c.documentElement.doScroll&&b&&J()}}},isFunction:function(a){return e.type(a)==="function"},isArray:Array.isArray||function(a){return e.type(a)==="array"},isWindow:function(a){return a&&typeof a=="object"&&"setInterval"in a},isNaN:function(a){return a==null||!m.test(a)||isNaN(a)},type:function(a){return a==null?String(a):I[C.call(a)]||"object"},isPlainObject:function(a){if(!a||e.type(a)!=="object"||a.nodeType||e.isWindow(a))return!1;if(a.constructor&&!D.call(a,"constructor")&&!D.call(a.constructor.prototype,"isPrototypeOf"))return!1;var c;for(c in a);return c===b||D.call(a,c)},isEmptyObject:function(a){for(var b in a)return!1;return!0},error:function(a){throw a},parseJSON:function(b){if(typeof b!="string"||!b)return null;b=e.trim(b);if(a.JSON&&a.JSON.parse)return a.JSON.parse(b);if(o.test(b.replace(p,"@").replace(q,"]").replace(r,"")))return(new Function("return "+b))();e.error("Invalid JSON: "+b)},parseXML:function(b,c,d){a.DOMParser?(d=new DOMParser,c=d.parseFromString(b,"text/xml")):(c=new ActiveXObject("Microsoft.XMLDOM"),c.async="false",c.loadXML(b)),d=c.documentElement,(!d||!d.nodeName||d.nodeName==="parsererror")&&e.error("Invalid XML: "+b);return c},noop:function(){},globalEval:function(b){b&&j.test(b)&&(a.execScript||function(b){a.eval.call(a,b)})(b)},camelCase:function(a){return a.replace(w,x)},nodeName:function(a,b){return a.nodeName&&a.nodeName.toUpperCase()===b.toUpperCase()},each:function(a,c,d){var f,g=0,h=a.length,i=h===b||e.isFunction(a);if(d){if(i){for(f in a)if(c.apply(a[f],d)===!1)break}else for(;g<h;)if(c.apply(a[g++],d)===!1)break}else if(i){for(f in a)if(c.call(a[f],f,a[f])===!1)break}else for(;g<h;)if(c.call(a[g],g,a[g++])===!1)break;return a},trim:G?function(a){return a==null?"":G.call(a)}:function(a){return a==null?"":(a+"").replace(k,"").replace(l,"")},makeArray:function(a,b){var c=b||[];if(a!=null){var d=e.type(a);a.length==null||d==="string"||d==="function"||d==="regexp"||e.isWindow(a)?E.call(c,a):e.merge(c,a)}return c},inArray:function(a,b){if(H)return H.call(b,a);for(var c=0,d=b.length;c<d;c++)if(b[c]===a)return c;return-1},merge:function(a,c){var d=a.length,e=0;if(typeof c.length=="number")for(var f=c.length;e<f;e++)a[d++]=c[e];else while(c[e]!==b)a[d++]=c[e++];a.length=d;return a},grep:function(a,b,c){var d=[],e;c=!!c;for(var f=0,g=a.length;f<g;f++)e=!!b(a[f],f),c!==e&&d.push(a[f]);return d},map:function(a,c,d){var f,g,h=[],i=0,j=a.length,k=a instanceof e||j!==b&&typeof j=="number"&&(j>0&&a[0]&&a[j-1]||j===0||e.isArray(a));if(k)for(;i<j;i++)f=c(a[i],i,d),f!=null&&(h[h.length]=f);else for(g in a)f=c(a[g],g,d),f!=null&&(h[h.length]=f);return h.concat.apply([],h)},guid:1,proxy:function(a,c){if(typeof c=="string"){var d=a[c];c=a,a=d}if(!e.isFunction(a))return b;var f=F.call(arguments,2),g=function(){return a.apply(c,f.concat(F.call(arguments)))};g.guid=a.guid=a.guid||g.guid||e.guid++;return g},access:function(a,c,d,f,g,h){var i=a.length;if(typeof c=="object"){for(var j in c)e.access(a,j,c[j],f,g,d);return a}if(d!==b){f=!h&&f&&e.isFunction(d);for(var k=0;k<i;k++)g(a[k],c,f?d.call(a[k],k,g(a[k],c)):d,h);return a}return i?g(a[0],c):b},now:function(){return(new Date).getTime()},uaMatch:function(a){a=a.toLowerCase();var b=s.exec(a)||t.exec(a)||u.exec(a)||a.indexOf("compatible")<0&&v.exec(a)||[];return{browser:b[1]||"",version:b[2]||"0"}},sub:function(){function a(b,c){return new a.fn.init(b,c)}e.extend(!0,a,this),a.superclass=this,a.fn=a.prototype=this(),a.fn.constructor=a,a.sub=this.sub,a.fn.init=function(d,f){f&&f instanceof e&&!(f instanceof a)&&(f=a(f));return e.fn.init.call(this,d,f,b)},a.fn.init.prototype=a.fn;var b=a(c);return a},browser:{}}),e.each("Boolean Number String Function Array Date RegExp Object".split(" "),function(a,b){I["[object "+b+"]"]=b.toLowerCase()}),z=e.uaMatch(y),z.browser&&(e.browser[z.browser]=!0,e.browser.version=z.version),e.browser.webkit&&(e.browser.safari=!0),j.test(" ")&&(k=/^[\s\xA0]+/,l=/[\s\xA0]+$/),h=e(c),c.addEventListener?B=function(){c.removeEventListener("DOMContentLoaded",B,!1),e.ready()}:c.attachEvent&&(B=function(){c.readyState==="complete"&&(c.detachEvent("onreadystatechange",B),e.ready())});return e}(),g="done fail isResolved isRejected promise then always pipe".split(" "),h=[].slice;f.extend({_Deferred:function(){var a=[],b,c,d,e={done:function(){if(!d){var c=arguments,g,h,i,j,k;b&&(k=b,b=0);for(g=0,h=c.length;g<h;g++)i=c[g],j=f.type(i),j==="array"?e.done.apply(e,i):j==="function"&&a.push(i);k&&e.resolveWith(k[0],k[1])}return this},resolveWith:function(e,f){if(!d&&!b&&!c){f=f||[],c=1;try{while(a[0])a.shift().apply(e,f)}finally{b=[e,f],c=0}}return this},resolve:function(){e.resolveWith(this,arguments);return this},isResolved:function(){return!!c||!!b},cancel:function(){d=1,a=[];return this}};return e},Deferred:function(a){var b=f._Deferred(),c=f._Deferred(),d;f.extend(b,{then:function(a,c){b.done(a).fail(c);return this},always:function(){return b.done.apply(b,arguments).fail.apply(this,arguments)},fail:c.done,rejectWith:c.resolveWith,reject:c.resolve,isRejected:c.isResolved,pipe:function(a,c){return f.Deferred(function(d){f.each({done:[a,"resolve"],fail:[c,"reject"]},function(a,c){var e=c[0],g=c[1],h;f.isFunction(e)?b[a](function(){h=e.apply(this,arguments),h&&f.isFunction(h.promise)?h.promise().then(d.resolve,d.reject):d[g](h)}):b[a](d[g])})}).promise()},promise:function(a){if(a==null){if(d)return d;d=a={}}var c=g.length;while(c--)a[g[c]]=b[g[c]];return a}}),b.done(c.cancel).fail(b.cancel),delete b.cancel,a&&a.call(b,b);return b},when:function(a){function i(a){return function(c){b[a]=arguments.length>1?h.call(arguments,0):c,--e||g.resolveWith(g,h.call(b,0))}}var b=arguments,c=0,d=b.length,e=d,g=d<=1&&a&&f.isFunction(a.promise)?a:f.Deferred();if(d>1){for(;c<d;c++)b[c]&&f.isFunction(b[c].promise)?b[c].promise().then(i(c),g.reject):--e;e||g.resolveWith(g,b)}else g!==a&&g.resolveWith(g,d?[a]:[]);return g.promise()}}),f.support=function(){var a=c.createElement("div"),b=c.documentElement,d,e,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u;a.setAttribute("className","t"),a.innerHTML="   <link/><table></table><a href='/a' style='top:1px;float:left;opacity:.55;'>a</a><input type='checkbox'/>",d=a.getElementsByTagName("*"),e=a.getElementsByTagName("a")[0];if(!d||!d.length||!e)return{};g=c.createElement("select"),h=g.appendChild(c.createElement("option")),i=a.getElementsByTagName("input")[0],k={leadingWhitespace:a.firstChild.nodeType===3,tbody:!a.getElementsByTagName("tbody").length,htmlSerialize:!!a.getElementsByTagName("link").length,style:/top/.test(e.getAttribute("style")),hrefNormalized:e.getAttribute("href")==="/a",opacity:/^0.55$/.test(e.style.opacity),cssFloat:!!e.style.cssFloat,checkOn:i.value==="on",optSelected:h.selected,getSetAttribute:a.className!=="t",submitBubbles:!0,changeBubbles:!0,focusinBubbles:!1,deleteExpando:!0,noCloneEvent:!0,inlineBlockNeedsLayout:!1,shrinkWrapBlocks:!1,reliableMarginRight:!0},i.checked=!0,k.noCloneChecked=i.cloneNode(!0).checked,g.disabled=!0,k.optDisabled=!h.disabled;try{delete a.test}catch(v){k.deleteExpando=!1}!a.addEventListener&&a.attachEvent&&a.fireEvent&&(a.attachEvent("onclick",function(){k.noCloneEvent=!1}),a.cloneNode(!0).fireEvent("onclick")),i=c.createElement("input"),i.value="t",i.setAttribute("type","radio"),k.radioValue=i.value==="t",i.setAttribute("checked","checked"),a.appendChild(i),l=c.createDocumentFragment(),l.appendChild(a.firstChild),k.checkClone=l.cloneNode(!0).cloneNode(!0).lastChild.checked,a.innerHTML="",a.style.width=a.style.paddingLeft="1px",m=c.getElementsByTagName("body")[0],o=c.createElement(m?"div":"body"),p={visibility:"hidden",width:0,height:0,border:0,margin:0},m&&f.extend(p,{position:"absolute",left:-1e3,top:-1e3});for(t in p)o.style[t]=p[t];o.appendChild(a),n=m||b,n.insertBefore(o,n.firstChild),k.appendChecked=i.checked,k.boxModel=a.offsetWidth===2,"zoom"in a.style&&(a.style.display="inline",a.style.zoom=1,k.inlineBlockNeedsLayout=a.offsetWidth===2,a.style.display="",a.innerHTML="<div style='width:4px;'></div>",k.shrinkWrapBlocks=a.offsetWidth!==2),a.innerHTML="<table><tr><td style='padding:0;border:0;display:none'></td><td>t</td></tr></table>",q=a.getElementsByTagName("td"),u=q[0].offsetHeight===0,q[0].style.display="",q[1].style.display="none",k.reliableHiddenOffsets=u&&q[0].offsetHeight===0,a.innerHTML="",c.defaultView&&c.defaultView.getComputedStyle&&(j=c.createElement("div"),j.style.width="0",j.style.marginRight="0",a.appendChild(j),k.reliableMarginRight=(parseInt((c.defaultView.getComputedStyle(j,null)||{marginRight:0}).marginRight,10)||0)===0),o.innerHTML="",n.removeChild(o);if(a.attachEvent)for(t in{submit:1,change:1,focusin:1})s="on"+t,u=s in a,u||(a.setAttribute(s,"return;"),u=typeof a[s]=="function"),k[t+"Bubbles"]=u;o=l=g=h=m=j=a=i=null;return k}(),f.boxModel=f.support.boxModel;var i=/^(?:\{.*\}|\[.*\])$/,j=/([a-z])([A-Z])/g;f.extend({cache:{},uuid:0,expando:"jQuery"+(f.fn.jquery+Math.random()).replace(/\D/g,""),noData:{embed:!0,object:"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000",applet:!0},hasData:function(a){a=a.nodeType?f.cache[a[f.expando]]:a[f.expando];return!!a&&!l(a)},data:function(a,c,d,e){if(!!f.acceptData(a)){var g=f.expando,h=typeof c=="string",i,j=a.nodeType,k=j?f.cache:a,l=j?a[f.expando]:a[f.expando]&&f.expando;if((!l||e&&l&&!k[l][g])&&h&&d===b)return;l||(j?a[f.expando]=l=++f.uuid:l=f.expando),k[l]||(k[l]={},j||(k[l].toJSON=f.noop));if(typeof c=="object"||typeof c=="function")e?k[l][g]=f.extend(k[l][g],c):k[l]=f.extend(k[l],c);i=k[l],e&&(i[g]||(i[g]={}),i=i[g]),d!==b&&(i[f.camelCase(c)]=d);if(c==="events"&&!i[c])return i[g]&&i[g].events;return h?i[f.camelCase(c)]||i[c]:i}},removeData:function(b,c,d){if(!!f.acceptData(b)){var e=f.expando,g=b.nodeType,h=g?f.cache:b,i=g?b[f.expando]:f.expando;if(!h[i])return;if(c){var j=d?h[i][e]:h[i];if(j){delete j[c];if(!l(j))return}}if(d){delete h[i][e];if(!l(h[i]))return}var k=h[i][e];f.support.deleteExpando||h!=a?delete h[i]:h[i]=null,k?(h[i]={},g||(h[i].toJSON=f.noop),h[i][e]=k):g&&(f.support.deleteExpando?delete b[f.expando]:b.removeAttribute?b.removeAttribute(f.expando):b[f.expando]=null)}},_data:function(a,b,c){return f.data(a,b,c,!0)},acceptData:function(a){if(a.nodeName){var b=f.noData[a.nodeName.toLowerCase()];if(b)return b!==!0&&a.getAttribute("classid")===b}return!0}}),f.fn.extend({data:function(a,c){var d=null;if(typeof a=="undefined"){if(this.length){d=f.data(this[0]);if(this[0].nodeType===1){var e=this[0].attributes,g;for(var h=0,i=e.length;h<i;h++)g=e[h].name,g.indexOf("data-")===0&&(g=f.camelCase(g.substring(5)),k(this[0],g,d[g]))}}return d}if(typeof a=="object")return this.each(function(){f.data(this,a)});var j=a.split(".");j[1]=j[1]?"."+j[1]:"";if(c===b){d=this.triggerHandler("getData"+j[1]+"!",[j[0]]),d===b&&this.length&&(d=f.data(this[0],a),d=k(this[0],a,d));return d===b&&j[1]?this.data(j[0]):d}return this.each(function(){var b=f(this),d=[j[0],c];b.triggerHandler("setData"+j[1]+"!",d),f.data(this,a,c),b.triggerHandler("changeData"+j[1]+"!",d)})},removeData:function(a){return this.each(function(){f.removeData(this,a)})}}),f.extend({_mark:function(a,c){a&&(c=(c||"fx")+"mark",f.data(a,c,(f.data(a,c,b,!0)||0)+1,!0))},_unmark:function(a,c,d){a!==!0&&(d=c,c=a,a=!1);if(c){d=d||"fx";var e=d+"mark",g=a?0:(f.data(c,e,b,!0)||1)-1;g?f.data(c,e,g,!0):(f.removeData(c,e,!0),m(c,d,"mark"))}},queue:function(a,c,d){if(a){c=(c||"fx")+"queue";var e=f.data(a,c,b,!0);d&&(!e||f.isArray(d)?e=f.data(a,c,f.makeArray(d),!0):e.push(d));return e||[]}},dequeue:function(a,b){b=b||"fx";var c=f.queue(a,b),d=c.shift(),e;d==="inprogress"&&(d=c.shift()),d&&(b==="fx"&&c.unshift("inprogress"),d.call(a,function(){f.dequeue(a,b)})),c.length||(f.removeData(a,b+"queue",!0),m(a,b,"queue"))}}),f.fn.extend({queue:function(a,c){typeof a!="string"&&(c=a,a="fx");if(c===b)return f.queue(this[0],a);return this.each(function(){var b=f.queue(this,a,c);a==="fx"&&b[0]!=="inprogress"&&f.dequeue(this,a)})},dequeue:function(a){return this.each(function(){f.dequeue(this,a)})},delay:function(a,b){a=f.fx?f.fx.speeds[a]||a:a,b=b||"fx";return this.queue(b,function(){var c=this;setTimeout(function(){f.dequeue(c,b)},a)})},clearQueue:function(a){return this.queue(a||"fx",[])},promise:function(a,c){function m(){--h||d.resolveWith(e,[e])}typeof a!="string"&&(c=a,a=b),a=a||"fx";var d=f.Deferred(),e=this,g=e.length,h=1,i=a+"defer",j=a+"queue",k=a+"mark",l;while(g--)if(l=f.data(e[g],i,b,!0)||(f.data(e[g],j,b,!0)||f.data(e[g],k,b,!0))&&f.data(e[g],i,f._Deferred(),!0))h++,l.done(m);m();return d.promise()}});var n=/[\n\t\r]/g,o=/\s+/,p=/\r/g,q=/^(?:button|input)$/i,r=/^(?:button|input|object|select|textarea)$/i,s=/^a(?:rea)?$/i,t=/^(?:autofocus|autoplay|async|checked|controls|defer|disabled|hidden|loop|multiple|open|readonly|required|scoped|selected)$/i,u=/\:|^on/,v,w;f.fn.extend({attr:function(a,b){return f.access(this,a,b,!0,f.attr)},removeAttr:function(a){return this.each(function(){f.removeAttr(this,a)})},prop:function(a,b){return f.access(this,a,b,!0,f.prop)},removeProp:function(a){a=f.propFix[a]||a;return this.each(function(){try{this[a]=b,delete this[a]}catch(c){}})},addClass:function(a){var b,c,d,e,g,h,i;if(f.isFunction(a))return this.each(function(b){f(this).addClass(a.call(this,b,this.className))});if(a&&typeof a=="string"){b=a.split(o);for(c=0,d=this.length;c<d;c++){e=this[c];if(e.nodeType===1)if(!e.className&&b.length===1)e.className=a;else{g=" "+e.className+" ";for(h=0,i=b.length;h<i;h++)~g.indexOf(" "+b[h]+" ")||(g+=b[h]+" ");e.className=f.trim(g)}}}return this},removeClass:function(a){var c,d,e,g,h,i,j;if(f.isFunction(a))return this.each(function(b){f(this).removeClass(a.call(this,b,this.className))});if(a&&typeof a=="string"||a===b){c=(a||"").split(o);for(d=0,e=this.length;d<e;d++){g=this[d];if(g.nodeType===1&&g.className)if(a){h=(" "+g.className+" ").replace(n," ");for(i=0,j=c.length;i<j;i++)h=h.replace(" "+c[i]+" "," ");g.className=f.trim(h)}else g.className=""}}return this},toggleClass:function(a,b){var c=typeof a,d=typeof b=="boolean";if(f.isFunction(a))return this.each(function(c){f(this).toggleClass(a.call(this,c,this.className,b),b)});return this.each(function(){if(c==="string"){var e,g=0,h=f(this),i=b,j=a.split(o);while(e=j[g++])i=d?i:!h.hasClass(e),h[i?"addClass":"removeClass"](e)}else if(c==="undefined"||c==="boolean")this.className&&f._data(this,"__className__",this.className),this.className=this.className||a===!1?"":f._data(this,"__className__")||""})},hasClass:function(a){var b=" "+a+" ";for(var c=0,d=this.length;c<d;c++)if((" "+this[c].className+" ").replace(n," ").indexOf(b)>-1)return!0;return!1},val:function(a){var c,d,e=this[0];if(!arguments.length){if(e){c=f.valHooks[e.nodeName.toLowerCase()]||f.valHooks[e.type];if(c&&"get"in c&&(d=c.get(e,"value"))!==b)return d;d=e.value;return typeof d=="string"?d.replace(p,""):d==null?"":d}return b}var g=f.isFunction(a);return this.each(function(d){var e=f(this),h;if(this.nodeType===1){g?h=a.call(this,d,e.val()):h=a,h==null?h="":typeof h=="number"?h+="":f.isArray(h)&&(h=f.map(h,function(a){return a==null?"":a+""})),c=f.valHooks[this.nodeName.toLowerCase()]||f.valHooks[this.type];if(!c||!("set"in c)||c.set(this,h,"value")===b)this.value=h}})}}),f.extend({valHooks:{option:{get:function(a){var b=a.attributes.value;return!b||b.specified?a.value:a.text}},select:{get:function(a){var b,c=a.selectedIndex,d=[],e=a.options,g=a.type==="select-one";if(c<0)return null;for(var h=g?c:0,i=g?c+1:e.length;h<i;h++){var j=e[h];if(j.selected&&(f.support.optDisabled?!j.disabled:j.getAttribute("disabled")===null)&&(!j.parentNode.disabled||!f.nodeName(j.parentNode,"optgroup"))){b=f(j).val();if(g)return b;d.push(b)}}if(g&&!d.length&&e.length)return f(e[c]).val();return d},set:function(a,b){var c=f.makeArray(b);f(a).find("option").each(function(){this.selected=f.inArray(f(this).val(),c)>=0}),c.length||(a.selectedIndex=-1);return c}}},attrFn:{val:!0,css:!0,html:!0,text:!0,data:!0,width:!0,height:!0,offset:!0},attrFix:{tabindex:"tabIndex"},attr:function(a,c,d,e){var g=a.nodeType;if(!a||g===3||g===8||g===2)return b;if(e&&c in f.attrFn)return f(a)[c](d);if(!("getAttribute"in a))return f.prop(a,c,d);var h,i,j=g!==1||!f.isXMLDoc(a);j&&(c=f.attrFix[c]||c,i=f.attrHooks[c],i||(t.test(c)?i=w:v&&c!=="className"&&(f.nodeName(a,"form")||u.test(c))&&(i=v)));if(d!==b){if(d===null){f.removeAttr(a,c);return b}if(i&&"set"in i&&j&&(h=i.set(a,d,c))!==b)return h;a.setAttribute(c,""+d);return d}if(i&&"get"in i&&j&&(h=i.get(a,c))!==null)return h;h=a.getAttribute(c);return h===null?b:h},removeAttr:function(a,b){var c;a.nodeType===1&&(b=f.attrFix[b]||b,f.support.getSetAttribute?a.removeAttribute(b):(f.attr(a,b,""),a.removeAttributeNode(a.getAttributeNode(b))),t.test(b)&&(c=f.propFix[b]||b)in a&&(a[c]=!1))},attrHooks:{type:{set:function(a,b){if(q.test(a.nodeName)&&a.parentNode)f.error("type property can't be changed");else if(!f.support.radioValue&&b==="radio"&&f.nodeName(a,"input")){var c=a.value;a.setAttribute("type",b),c&&(a.value=c);return b}}},tabIndex:{get:function(a){var c=a.getAttributeNode("tabIndex");return c&&c.specified?parseInt(c.value,10):r.test(a.nodeName)||s.test(a.nodeName)&&a.href?0:b}},value:{get:function(a,b){if(v&&f.nodeName(a,"button"))return v.get(a,b);return b in a?a.value:null},set:function(a,b,c){if(v&&f.nodeName(a,"button"))return v.set(a,b,c);a.value=b}}},propFix:{tabindex:"tabIndex",readonly:"readOnly","for":"htmlFor","class":"className",maxlength:"maxLength",cellspacing:"cellSpacing",cellpadding:"cellPadding",rowspan:"rowSpan",colspan:"colSpan",usemap:"useMap",frameborder:"frameBorder",contenteditable:"contentEditable"},prop:function(a,c,d){var e=a.nodeType;if(!a||e===3||e===8||e===2)return b;var g,h,i=e!==1||!f.isXMLDoc(a);i&&(c=f.propFix[c]||c,h=f.propHooks[c]);return d!==b?h&&"set"in h&&(g=h.set(a,d,c))!==b?g:a[c]=d:h&&"get"in h&&(g=h.get(a,c))!==b?g:a[c]},propHooks:{}}),w={get:function(a,c){return f.prop(a,c)?c.toLowerCase():b},set:function(a,b,c){var d;b===!1?f.removeAttr(a,c):(d=f.propFix[c]||c,d in a&&(a[d]=!0),a.setAttribute(c,c.toLowerCase()));return c}},f.support.getSetAttribute||(f.attrFix=f.propFix,v=f.attrHooks.name=f.attrHooks.title=f.valHooks.button={get:function(a,c){var d;d=a.getAttributeNode(c);return d&&d.nodeValue!==""?d.nodeValue:b},set:function(a,b,c){var d=a.getAttributeNode(c);if(d){d.nodeValue=b;return b}}},f.each(["width","height"],function(a,b){f.attrHooks[b]=f.extend(f.attrHooks[b],{set:function(a,c){if(c===""){a.setAttribute(b,"auto");return c}}})})),f.support.hrefNormalized||f.each(["href","src","width","height"],function(a,c){f.attrHooks[c]=f.extend(f.attrHooks[c],{get:function(a){var d=a.getAttribute(c,2);return d===null?b:d}})}),f.support.style||(f.attrHooks.style={get:function(a){return a.style.cssText.toLowerCase()||b},set:function(a,b){return a.style.cssText=""+b}}),f.support.optSelected||(f.propHooks.selected=f.extend(f.propHooks.selected,{get:function(a){var b=a.parentNode;b&&(b.selectedIndex,b.parentNode&&b.parentNode.selectedIndex)}})),f.support.checkOn||f.each(["radio","checkbox"],function(){f.valHooks[this]={get:function(a){return a.getAttribute("value")===null?"on":a.value}}}),f.each(["radio","checkbox"],function(){f.valHooks[this]=f.extend(f.valHooks[this],{set:function(a,b){if(f.isArray(b))return a.checked=f.inArray(f(a).val(),b)>=0}})});var x=/\.(.*)$/,y=/^(?:textarea|input|select)$/i,z=/\./g,A=/ /g,B=/[^\w\s.|`]/g,C=function(a){return a.replace(B,"\\$&")};f.event={add:function(a,c,d,e){if(a.nodeType!==3&&a.nodeType!==8){if(d===!1)d=D;else if(!d)return;var g,h;d.handler&&(g=d,d=g.handler),d.guid||(d.guid=f.guid++);var i=f._data(a);if(!i)return;var j=i.events,k=i.handle;j||(i.events=j={}),k||(i.handle=k=function(a){return typeof f!="undefined"&&(!a||f.event.triggered!==a.type)?f.event.handle.apply(k.elem,arguments):b}),k.elem=a,c=c.split(" ");var l,m=0,n;while(l=c[m++]){h=g?f.extend({},g):{handler:d,data:e},l.indexOf(".")>-1?(n=l.split("."),l=n.shift(),h.namespace=n.slice(0).sort().join(".")):(n=[],h.namespace=""),h.type=l,h.guid||(h.guid=d.guid);var o=j[l],p=f.event.special[l]||{};if(!o){o=j[l]=[];if(!p.setup||p.setup.call(a,e,n,k)===!1)a.addEventListener?a.addEventListener(l,k,!1):a.attachEvent&&a.attachEvent("on"+l,k)}p.add&&(p.add.call(a,h),h.handler.guid||(h.handler.guid=d.guid)),o.push(h),f.event.global[l]=!0}a=null}},global:{},remove:function(a,c,d,e){if(a.nodeType!==3&&a.nodeType!==8){d===!1&&(d=D);var g,h,i,j,k=0,l,m,n,o,p,q,r,s=f.hasData(a)&&f._data(a),t=s&&s.events;if(!s||!t)return;c&&c.type&&(d=c.handler,c=c.type);if(!c||typeof c=="string"&&c.charAt(0)==="."){c=c||"";for(h in t)f.event.remove(a,h+c);return}c=c.split(" ");while(h=c[k++]){r=h,q=null,l=h.indexOf(".")<0,m=[],l||(m=h.split("."),h=m.shift(),n=new RegExp("(^|\\.)"+f.map(m.slice(0).sort(),C).join("\\.(?:.*\\.)?")+"(\\.|$)")),p=t[h];if(!p)continue;if(!d){for(j=0;j<p.length;j++){q=p[j];if(l||n.test(q.namespace))f.event.remove(a,r,q.handler,j),p.splice(j--,1)}continue}o=f.event.special[h]||{};for(j=e||0;j<p.length;j++){q=p[j];if(d.guid===q.guid){if(l||n.test(q.namespace))e==null&&p.splice(j--,1),o.remove&&o.remove.call(a,q);if(e!=null)break}}if(p.length===0||e!=null&&p.length===1)(!o.teardown||o.teardown.call(a,m)===!1)&&f.removeEvent(a,h,s.handle),g=null,delete t[h]}if(f.isEmptyObject(t)){var u=s.handle;u&&(u.elem=null),delete s.events,delete s.handle,f.isEmptyObject(s)&&f.removeData(a,b,!0)}}},customEvent:{getData:!0,setData:!0,changeData:!0},trigger:function(c,d,e,g){var h=c.type||c,i=[],j;h.indexOf("!")>=0&&(h=h.slice(0,-1),j=!0),h.indexOf(".")>=0&&(i=h.split("."),h=i.
shift(),i.sort());if(!!e&&!f.event.customEvent[h]||!!f.event.global[h]){c=typeof c=="object"?c[f.expando]?c:new f.Event(h,c):new f.Event(h),c.type=h,c.exclusive=j,c.namespace=i.join("."),c.namespace_re=new RegExp("(^|\\.)"+i.join("\\.(?:.*\\.)?")+"(\\.|$)");if(g||!e)c.preventDefault(),c.stopPropagation();if(!e){f.each(f.cache,function(){var a=f.expando,b=this[a];b&&b.events&&b.events[h]&&f.event.trigger(c,d,b.handle.elem)});return}if(e.nodeType===3||e.nodeType===8)return;c.result=b,c.target=e,d=d!=null?f.makeArray(d):[],d.unshift(c);var k=e,l=h.indexOf(":")<0?"on"+h:"";do{var m=f._data(k,"handle");c.currentTarget=k,m&&m.apply(k,d),l&&f.acceptData(k)&&k[l]&&k[l].apply(k,d)===!1&&(c.result=!1,c.preventDefault()),k=k.parentNode||k.ownerDocument||k===c.target.ownerDocument&&a}while(k&&!c.isPropagationStopped());if(!c.isDefaultPrevented()){var n,o=f.event.special[h]||{};if((!o._default||o._default.call(e.ownerDocument,c)===!1)&&(h!=="click"||!f.nodeName(e,"a"))&&f.acceptData(e)){try{l&&e[h]&&(n=e[l],n&&(e[l]=null),f.event.triggered=h,e[h]())}catch(p){}n&&(e[l]=n),f.event.triggered=b}}return c.result}},handle:function(c){c=f.event.fix(c||a.event);var d=((f._data(this,"events")||{})[c.type]||[]).slice(0),e=!c.exclusive&&!c.namespace,g=Array.prototype.slice.call(arguments,0);g[0]=c,c.currentTarget=this;for(var h=0,i=d.length;h<i;h++){var j=d[h];if(e||c.namespace_re.test(j.namespace)){c.handler=j.handler,c.data=j.data,c.handleObj=j;var k=j.handler.apply(this,g);k!==b&&(c.result=k,k===!1&&(c.preventDefault(),c.stopPropagation()));if(c.isImmediatePropagationStopped())break}}return c.result},props:"altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode layerX layerY metaKey newValue offsetX offsetY pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "),fix:function(a){if(a[f.expando])return a;var d=a;a=f.Event(d);for(var e=this.props.length,g;e;)g=this.props[--e],a[g]=d[g];a.target||(a.target=a.srcElement||c),a.target.nodeType===3&&(a.target=a.target.parentNode),!a.relatedTarget&&a.fromElement&&(a.relatedTarget=a.fromElement===a.target?a.toElement:a.fromElement);if(a.pageX==null&&a.clientX!=null){var h=a.target.ownerDocument||c,i=h.documentElement,j=h.body;a.pageX=a.clientX+(i&&i.scrollLeft||j&&j.scrollLeft||0)-(i&&i.clientLeft||j&&j.clientLeft||0),a.pageY=a.clientY+(i&&i.scrollTop||j&&j.scrollTop||0)-(i&&i.clientTop||j&&j.clientTop||0)}a.which==null&&(a.charCode!=null||a.keyCode!=null)&&(a.which=a.charCode!=null?a.charCode:a.keyCode),!a.metaKey&&a.ctrlKey&&(a.metaKey=a.ctrlKey),!a.which&&a.button!==b&&(a.which=a.button&1?1:a.button&2?3:a.button&4?2:0);return a},guid:1e8,proxy:f.proxy,special:{ready:{setup:f.bindReady,teardown:f.noop},live:{add:function(a){f.event.add(this,N(a.origType,a.selector),f.extend({},a,{handler:M,guid:a.handler.guid}))},remove:function(a){f.event.remove(this,N(a.origType,a.selector),a)}},beforeunload:{setup:function(a,b,c){f.isWindow(this)&&(this.onbeforeunload=c)},teardown:function(a,b){this.onbeforeunload===b&&(this.onbeforeunload=null)}}}},f.removeEvent=c.removeEventListener?function(a,b,c){a.removeEventListener&&a.removeEventListener(b,c,!1)}:function(a,b,c){a.detachEvent&&a.detachEvent("on"+b,c)},f.Event=function(a,b){if(!this.preventDefault)return new f.Event(a,b);a&&a.type?(this.originalEvent=a,this.type=a.type,this.isDefaultPrevented=a.defaultPrevented||a.returnValue===!1||a.getPreventDefault&&a.getPreventDefault()?E:D):this.type=a,b&&f.extend(this,b),this.timeStamp=f.now(),this[f.expando]=!0},f.Event.prototype={preventDefault:function(){this.isDefaultPrevented=E;var a=this.originalEvent;!a||(a.preventDefault?a.preventDefault():a.returnValue=!1)},stopPropagation:function(){this.isPropagationStopped=E;var a=this.originalEvent;!a||(a.stopPropagation&&a.stopPropagation(),a.cancelBubble=!0)},stopImmediatePropagation:function(){this.isImmediatePropagationStopped=E,this.stopPropagation()},isDefaultPrevented:D,isPropagationStopped:D,isImmediatePropagationStopped:D};var F=function(a){var b=a.relatedTarget,c=!1,d=a.type;a.type=a.data,b!==this&&(b&&(c=f.contains(this,b)),c||(f.event.handle.apply(this,arguments),a.type=d))},G=function(a){a.type=a.data,f.event.handle.apply(this,arguments)};f.each({mouseenter:"mouseover",mouseleave:"mouseout"},function(a,b){f.event.special[a]={setup:function(c){f.event.add(this,b,c&&c.selector?G:F,a)},teardown:function(a){f.event.remove(this,b,a&&a.selector?G:F)}}}),f.support.submitBubbles||(f.event.special.submit={setup:function(a,b){if(!f.nodeName(this,"form"))f.event.add(this,"click.specialSubmit",function(a){var b=a.target,c=b.type;(c==="submit"||c==="image")&&f(b).closest("form").length&&K("submit",this,arguments)}),f.event.add(this,"keypress.specialSubmit",function(a){var b=a.target,c=b.type;(c==="text"||c==="password")&&f(b).closest("form").length&&a.keyCode===13&&K("submit",this,arguments)});else return!1},teardown:function(a){f.event.remove(this,".specialSubmit")}});if(!f.support.changeBubbles){var H,I=function(a){var b=a.type,c=a.value;b==="radio"||b==="checkbox"?c=a.checked:b==="select-multiple"?c=a.selectedIndex>-1?f.map(a.options,function(a){return a.selected}).join("-"):"":f.nodeName(a,"select")&&(c=a.selectedIndex);return c},J=function(c){var d=c.target,e,g;if(!!y.test(d.nodeName)&&!d.readOnly){e=f._data(d,"_change_data"),g=I(d),(c.type!=="focusout"||d.type!=="radio")&&f._data(d,"_change_data",g);if(e===b||g===e)return;if(e!=null||g)c.type="change",c.liveFired=b,f.event.trigger(c,arguments[1],d)}};f.event.special.change={filters:{focusout:J,beforedeactivate:J,click:function(a){var b=a.target,c=f.nodeName(b,"input")?b.type:"";(c==="radio"||c==="checkbox"||f.nodeName(b,"select"))&&J.call(this,a)},keydown:function(a){var b=a.target,c=f.nodeName(b,"input")?b.type:"";(a.keyCode===13&&!f.nodeName(b,"textarea")||a.keyCode===32&&(c==="checkbox"||c==="radio")||c==="select-multiple")&&J.call(this,a)},beforeactivate:function(a){var b=a.target;f._data(b,"_change_data",I(b))}},setup:function(a,b){if(this.type==="file")return!1;for(var c in H)f.event.add(this,c+".specialChange",H[c]);return y.test(this.nodeName)},teardown:function(a){f.event.remove(this,".specialChange");return y.test(this.nodeName)}},H=f.event.special.change.filters,H.focus=H.beforeactivate}f.support.focusinBubbles||f.each({focus:"focusin",blur:"focusout"},function(a,b){function e(a){var c=f.event.fix(a);c.type=b,c.originalEvent={},f.event.trigger(c,null,c.target),c.isDefaultPrevented()&&a.preventDefault()}var d=0;f.event.special[b]={setup:function(){d++===0&&c.addEventListener(a,e,!0)},teardown:function(){--d===0&&c.removeEventListener(a,e,!0)}}}),f.each(["bind","one"],function(a,c){f.fn[c]=function(a,d,e){var g;if(typeof a=="object"){for(var h in a)this[c](h,d,a[h],e);return this}if(arguments.length===2||d===!1)e=d,d=b;c==="one"?(g=function(a){f(this).unbind(a,g);return e.apply(this,arguments)},g.guid=e.guid||f.guid++):g=e;if(a==="unload"&&c!=="one")this.one(a,d,e);else for(var i=0,j=this.length;i<j;i++)f.event.add(this[i],a,g,d);return this}}),f.fn.extend({unbind:function(a,b){if(typeof a=="object"&&!a.preventDefault)for(var c in a)this.unbind(c,a[c]);else for(var d=0,e=this.length;d<e;d++)f.event.remove(this[d],a,b);return this},delegate:function(a,b,c,d){return this.live(b,c,d,a)},undelegate:function(a,b,c){return arguments.length===0?this.unbind("live"):this.die(b,null,c,a)},trigger:function(a,b){return this.each(function(){f.event.trigger(a,b,this)})},triggerHandler:function(a,b){if(this[0])return f.event.trigger(a,b,this[0],!0)},toggle:function(a){var b=arguments,c=a.guid||f.guid++,d=0,e=function(c){var e=(f.data(this,"lastToggle"+a.guid)||0)%d;f.data(this,"lastToggle"+a.guid,e+1),c.preventDefault();return b[e].apply(this,arguments)||!1};e.guid=c;while(d<b.length)b[d++].guid=c;return this.click(e)},hover:function(a,b){return this.mouseenter(a).mouseleave(b||a)}});var L={focus:"focusin",blur:"focusout",mouseenter:"mouseover",mouseleave:"mouseout"};f.each(["live","die"],function(a,c){f.fn[c]=function(a,d,e,g){var h,i=0,j,k,l,m=g||this.selector,n=g?this:f(this.context);if(typeof a=="object"&&!a.preventDefault){for(var o in a)n[c](o,d,a[o],m);return this}if(c==="die"&&!a&&g&&g.charAt(0)==="."){n.unbind(g);return this}if(d===!1||f.isFunction(d))e=d||D,d=b;a=(a||"").split(" ");while((h=a[i++])!=null){j=x.exec(h),k="",j&&(k=j[0],h=h.replace(x,""));if(h==="hover"){a.push("mouseenter"+k,"mouseleave"+k);continue}l=h,L[h]?(a.push(L[h]+k),h=h+k):h=(L[h]||h)+k;if(c==="live")for(var p=0,q=n.length;p<q;p++)f.event.add(n[p],"live."+N(h,m),{data:d,selector:m,handler:e,origType:h,origHandler:e,preType:l});else n.unbind("live."+N(h,m),e)}return this}}),f.each("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error".split(" "),function(a,b){f.fn[b]=function(a,c){c==null&&(c=a,a=null);return arguments.length>0?this.bind(b,a,c):this.trigger(b)},f.attrFn&&(f.attrFn[b]=!0)}),function(){function u(a,b,c,d,e,f){for(var g=0,h=d.length;g<h;g++){var i=d[g];if(i){var j=!1;i=i[a];while(i){if(i.sizcache===c){j=d[i.sizset];break}if(i.nodeType===1){f||(i.sizcache=c,i.sizset=g);if(typeof b!="string"){if(i===b){j=!0;break}}else if(k.filter(b,[i]).length>0){j=i;break}}i=i[a]}d[g]=j}}}function t(a,b,c,d,e,f){for(var g=0,h=d.length;g<h;g++){var i=d[g];if(i){var j=!1;i=i[a];while(i){if(i.sizcache===c){j=d[i.sizset];break}i.nodeType===1&&!f&&(i.sizcache=c,i.sizset=g);if(i.nodeName.toLowerCase()===b){j=i;break}i=i[a]}d[g]=j}}}var a=/((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^\[\]]*\]|['"][^'"]*['"]|[^\[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g,d=0,e=Object.prototype.toString,g=!1,h=!0,i=/\\/g,j=/\W/;[0,0].sort(function(){h=!1;return 0});var k=function(b,d,f,g){f=f||[],d=d||c;var h=d;if(d.nodeType!==1&&d.nodeType!==9)return[];if(!b||typeof b!="string")return f;var i,j,n,o,q,r,s,t,u=!0,w=k.isXML(d),x=[],y=b;do{a.exec(""),i=a.exec(y);if(i){y=i[3],x.push(i[1]);if(i[2]){o=i[3];break}}}while(i);if(x.length>1&&m.exec(b))if(x.length===2&&l.relative[x[0]])j=v(x[0]+x[1],d);else{j=l.relative[x[0]]?[d]:k(x.shift(),d);while(x.length)b=x.shift(),l.relative[b]&&(b+=x.shift()),j=v(b,j)}else{!g&&x.length>1&&d.nodeType===9&&!w&&l.match.ID.test(x[0])&&!l.match.ID.test(x[x.length-1])&&(q=k.find(x.shift(),d,w),d=q.expr?k.filter(q.expr,q.set)[0]:q.set[0]);if(d){q=g?{expr:x.pop(),set:p(g)}:k.find(x.pop(),x.length===1&&(x[0]==="~"||x[0]==="+")&&d.parentNode?d.parentNode:d,w),j=q.expr?k.filter(q.expr,q.set):q.set,x.length>0?n=p(j):u=!1;while(x.length)r=x.pop(),s=r,l.relative[r]?s=x.pop():r="",s==null&&(s=d),l.relative[r](n,s,w)}else n=x=[]}n||(n=j),n||k.error(r||b);if(e.call(n)==="[object Array]")if(!u)f.push.apply(f,n);else if(d&&d.nodeType===1)for(t=0;n[t]!=null;t++)n[t]&&(n[t]===!0||n[t].nodeType===1&&k.contains(d,n[t]))&&f.push(j[t]);else for(t=0;n[t]!=null;t++)n[t]&&n[t].nodeType===1&&f.push(j[t]);else p(n,f);o&&(k(o,h,f,g),k.uniqueSort(f));return f};k.uniqueSort=function(a){if(r){g=h,a.sort(r);if(g)for(var b=1;b<a.length;b++)a[b]===a[b-1]&&a.splice(b--,1)}return a},k.matches=function(a,b){return k(a,null,null,b)},k.matchesSelector=function(a,b){return k(b,null,null,[a]).length>0},k.find=function(a,b,c){var d;if(!a)return[];for(var e=0,f=l.order.length;e<f;e++){var g,h=l.order[e];if(g=l.leftMatch[h].exec(a)){var j=g[1];g.splice(1,1);if(j.substr(j.length-1)!=="\\"){g[1]=(g[1]||"").replace(i,""),d=l.find[h](g,b,c);if(d!=null){a=a.replace(l.match[h],"");break}}}}d||(d=typeof b.getElementsByTagName!="undefined"?b.getElementsByTagName("*"):[]);return{set:d,expr:a}},k.filter=function(a,c,d,e){var f,g,h=a,i=[],j=c,m=c&&c[0]&&k.isXML(c[0]);while(a&&c.length){for(var n in l.filter)if((f=l.leftMatch[n].exec(a))!=null&&f[2]){var o,p,q=l.filter[n],r=f[1];g=!1,f.splice(1,1);if(r.substr(r.length-1)==="\\")continue;j===i&&(i=[]);if(l.preFilter[n]){f=l.preFilter[n](f,j,d,i,e,m);if(!f)g=o=!0;else if(f===!0)continue}if(f)for(var s=0;(p=j[s])!=null;s++)if(p){o=q(p,f,s,j);var t=e^!!o;d&&o!=null?t?g=!0:j[s]=!1:t&&(i.push(p),g=!0)}if(o!==b){d||(j=i),a=a.replace(l.match[n],"");if(!g)return[];break}}if(a===h)if(g==null)k.error(a);else break;h=a}return j},k.error=function(a){throw"Syntax error, unrecognized expression: "+a};var l=k.selectors={order:["ID","NAME","TAG"],match:{ID:/#((?:[\w\u00c0-\uFFFF\-]|\\.)+)/,CLASS:/\.((?:[\w\u00c0-\uFFFF\-]|\\.)+)/,NAME:/\[name=['"]*((?:[\w\u00c0-\uFFFF\-]|\\.)+)['"]*\]/,ATTR:/\[\s*((?:[\w\u00c0-\uFFFF\-]|\\.)+)\s*(?:(\S?=)\s*(?:(['"])(.*?)\3|(#?(?:[\w\u00c0-\uFFFF\-]|\\.)*)|)|)\s*\]/,TAG:/^((?:[\w\u00c0-\uFFFF\*\-]|\\.)+)/,CHILD:/:(only|nth|last|first)-child(?:\(\s*(even|odd|(?:[+\-]?\d+|(?:[+\-]?\d*)?n\s*(?:[+\-]\s*\d+)?))\s*\))?/,POS:/:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^\-]|$)/,PSEUDO:/:((?:[\w\u00c0-\uFFFF\-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/},leftMatch:{},attrMap:{"class":"className","for":"htmlFor"},attrHandle:{href:function(a){return a.getAttribute("href")},type:function(a){return a.getAttribute("type")}},relative:{"+":function(a,b){var c=typeof b=="string",d=c&&!j.test(b),e=c&&!d;d&&(b=b.toLowerCase());for(var f=0,g=a.length,h;f<g;f++)if(h=a[f]){while((h=h.previousSibling)&&h.nodeType!==1);a[f]=e||h&&h.nodeName.toLowerCase()===b?h||!1:h===b}e&&k.filter(b,a,!0)},">":function(a,b){var c,d=typeof b=="string",e=0,f=a.length;if(d&&!j.test(b)){b=b.toLowerCase();for(;e<f;e++){c=a[e];if(c){var g=c.parentNode;a[e]=g.nodeName.toLowerCase()===b?g:!1}}}else{for(;e<f;e++)c=a[e],c&&(a[e]=d?c.parentNode:c.parentNode===b);d&&k.filter(b,a,!0)}},"":function(a,b,c){var e,f=d++,g=u;typeof b=="string"&&!j.test(b)&&(b=b.toLowerCase(),e=b,g=t),g("parentNode",b,f,a,e,c)},"~":function(a,b,c){var e,f=d++,g=u;typeof b=="string"&&!j.test(b)&&(b=b.toLowerCase(),e=b,g=t),g("previousSibling",b,f,a,e,c)}},find:{ID:function(a,b,c){if(typeof b.getElementById!="undefined"&&!c){var d=b.getElementById(a[1]);return d&&d.parentNode?[d]:[]}},NAME:function(a,b){if(typeof b.getElementsByName!="undefined"){var c=[],d=b.getElementsByName(a[1]);for(var e=0,f=d.length;e<f;e++)d[e].getAttribute("name")===a[1]&&c.push(d[e]);return c.length===0?null:c}},TAG:function(a,b){if(typeof b.getElementsByTagName!="undefined")return b.getElementsByTagName(a[1])}},preFilter:{CLASS:function(a,b,c,d,e,f){a=" "+a[1].replace(i,"")+" ";if(f)return a;for(var g=0,h;(h=b[g])!=null;g++)h&&(e^(h.className&&(" "+h.className+" ").replace(/[\t\n\r]/g," ").indexOf(a)>=0)?c||d.push(h):c&&(b[g]=!1));return!1},ID:function(a){return a[1].replace(i,"")},TAG:function(a,b){return a[1].replace(i,"").toLowerCase()},CHILD:function(a){if(a[1]==="nth"){a[2]||k.error(a[0]),a[2]=a[2].replace(/^\+|\s*/g,"");var b=/(-?)(\d*)(?:n([+\-]?\d*))?/.exec(a[2]==="even"&&"2n"||a[2]==="odd"&&"2n+1"||!/\D/.test(a[2])&&"0n+"+a[2]||a[2]);a[2]=b[1]+(b[2]||1)-0,a[3]=b[3]-0}else a[2]&&k.error(a[0]);a[0]=d++;return a},ATTR:function(a,b,c,d,e,f){var g=a[1]=a[1].replace(i,"");!f&&l.attrMap[g]&&(a[1]=l.attrMap[g]),a[4]=(a[4]||a[5]||"").replace(i,""),a[2]==="~="&&(a[4]=" "+a[4]+" ");return a},PSEUDO:function(b,c,d,e,f){if(b[1]==="not")if((a.exec(b[3])||"").length>1||/^\w/.test(b[3]))b[3]=k(b[3],null,null,c);else{var g=k.filter(b[3],c,d,!0^f);d||e.push.apply(e,g);return!1}else if(l.match.POS.test(b[0])||l.match.CHILD.test(b[0]))return!0;return b},POS:function(a){a.unshift(!0);return a}},filters:{enabled:function(a){return a.disabled===!1&&a.type!=="hidden"},disabled:function(a){return a.disabled===!0},checked:function(a){return a.checked===!0},selected:function(a){a.parentNode&&a.parentNode.selectedIndex;return a.selected===!0},parent:function(a){return!!a.firstChild},empty:function(a){return!a.firstChild},has:function(a,b,c){return!!k(c[3],a).length},header:function(a){return/h\d/i.test(a.nodeName)},text:function(a){var b=a.getAttribute("type"),c=a.type;return a.nodeName.toLowerCase()==="input"&&"text"===c&&(b===c||b===null)},radio:function(a){return a.nodeName.toLowerCase()==="input"&&"radio"===a.type},checkbox:function(a){return a.nodeName.toLowerCase()==="input"&&"checkbox"===a.type},file:function(a){return a.nodeName.toLowerCase()==="input"&&"file"===a.type},password:function(a){return a.nodeName.toLowerCase()==="input"&&"password"===a.type},submit:function(a){var b=a.nodeName.toLowerCase();return(b==="input"||b==="button")&&"submit"===a.type},image:function(a){return a.nodeName.toLowerCase()==="input"&&"image"===a.type},reset:function(a){var b=a.nodeName.toLowerCase();return(b==="input"||b==="button")&&"reset"===a.type},button:function(a){var b=a.nodeName.toLowerCase();return b==="input"&&"button"===a.type||b==="button"},input:function(a){return/input|select|textarea|button/i.test(a.nodeName)},focus:function(a){return a===a.ownerDocument.activeElement}},setFilters:{first:function(a,b){return b===0},last:function(a,b,c,d){return b===d.length-1},even:function(a,b){return b%2===0},odd:function(a,b){return b%2===1},lt:function(a,b,c){return b<c[3]-0},gt:function(a,b,c){return b>c[3]-0},nth:function(a,b,c){return c[3]-0===b},eq:function(a,b,c){return c[3]-0===b}},filter:{PSEUDO:function(a,b,c,d){var e=b[1],f=l.filters[e];if(f)return f(a,c,b,d);if(e==="contains")return(a.textContent||a.innerText||k.getText([a])||"").indexOf(b[3])>=0;if(e==="not"){var g=b[3];for(var h=0,i=g.length;h<i;h++)if(g[h]===a)return!1;return!0}k.error(e)},CHILD:function(a,b){var c=b[1],d=a;switch(c){case"only":case"first":while(d=d.previousSibling)if(d.nodeType===1)return!1;if(c==="first")return!0;d=a;case"last":while(d=d.nextSibling)if(d.nodeType===1)return!1;return!0;case"nth":var e=b[2],f=b[3];if(e===1&&f===0)return!0;var g=b[0],h=a.parentNode;if(h&&(h.sizcache!==g||!a.nodeIndex)){var i=0;for(d=h.firstChild;d;d=d.nextSibling)d.nodeType===1&&(d.nodeIndex=++i);h.sizcache=g}var j=a.nodeIndex-f;return e===0?j===0:j%e===0&&j/e>=0}},ID:function(a,b){return a.nodeType===1&&a.getAttribute("id")===b},TAG:function(a,b){return b==="*"&&a.nodeType===1||a.nodeName.toLowerCase()===b},CLASS:function(a,b){return(" "+(a.className||a.getAttribute("class"))+" ").indexOf(b)>-1},ATTR:function(a,b){var c=b[1],d=l.attrHandle[c]?l.attrHandle[c](a):a[c]!=null?a[c]:a.getAttribute(c),e=d+"",f=b[2],g=b[4];return d==null?f==="!=":f==="="?e===g:f==="*="?e.indexOf(g)>=0:f==="~="?(" "+e+" ").indexOf(g)>=0:g?f==="!="?e!==g:f==="^="?e.indexOf(g)===0:f==="$="?e.substr(e.length-g.length)===g:f==="|="?e===g||e.substr(0,g.length+1)===g+"-":!1:e&&d!==!1},POS:function(a,b,c,d){var e=b[2],f=l.setFilters[e];if(f)return f(a,c,b,d)}}},m=l.match.POS,n=function(a,b){return"\\"+(b-0+1)};for(var o in l.match)l.match[o]=new RegExp(l.match[o].source+/(?![^\[]*\])(?![^\(]*\))/.source),l.leftMatch[o]=new RegExp(/(^(?:.|\r|\n)*?)/.source+l.match[o].source.replace(/\\(\d+)/g,n));var p=function(a,b){a=Array.prototype.slice.call(a,0);if(b){b.push.apply(b,a);return b}return a};try{Array.prototype.slice.call(c.documentElement.childNodes,0)[0].nodeType}catch(q){p=function(a,b){var c=0,d=b||[];if(e.call(a)==="[object Array]")Array.prototype.push.apply(d,a);else if(typeof a.length=="number")for(var f=a.length;c<f;c++)d.push(a[c]);else for(;a[c];c++)d.push(a[c]);return d}}var r,s;c.documentElement.compareDocumentPosition?r=function(a,b){if(a===b){g=!0;return 0}if(!a.compareDocumentPosition||!b.compareDocumentPosition)return a.compareDocumentPosition?-1:1;return a.compareDocumentPosition(b)&4?-1:1}:(r=function(a,b){if(a===b){g=!0;return 0}if(a.sourceIndex&&b.sourceIndex)return a.sourceIndex-b.sourceIndex;var c,d,e=[],f=[],h=a.parentNode,i=b.parentNode,j=h;if(h===i)return s(a,b);if(!h)return-1;if(!i)return 1;while(j)e.unshift(j),j=j.parentNode;j=i;while(j)f.unshift(j),j=j.parentNode;c=e.length,d=f.length;for(var k=0;k<c&&k<d;k++)if(e[k]!==f[k])return s(e[k],f[k]);return k===c?s(a,f[k],-1):s(e[k],b,1)},s=function(a,b,c){if(a===b)return c;var d=a.nextSibling;while(d){if(d===b)return-1;d=d.nextSibling}return 1}),k.getText=function(a){var b="",c;for(var d=0;a[d];d++)c=a[d],c.nodeType===3||c.nodeType===4?b+=c.nodeValue:c.nodeType!==8&&(b+=k.getText(c.childNodes));return b},function(){var a=c.createElement("div"),d="script"+(new Date).getTime(),e=c.documentElement;a.innerHTML="<a name='"+d+"'/>",e.insertBefore(a,e.firstChild),c.getElementById(d)&&(l.find.ID=function(a,c,d){if(typeof c.getElementById!="undefined"&&!d){var e=c.getElementById(a[1]);return e?e.id===a[1]||typeof e.getAttributeNode!="undefined"&&e.getAttributeNode("id").nodeValue===a[1]?[e]:b:[]}},l.filter.ID=function(a,b){var c=typeof a.getAttributeNode!="undefined"&&a.getAttributeNode("id");return a.nodeType===1&&c&&c.nodeValue===b}),e.removeChild(a),e=a=null}(),function(){var a=c.createElement("div");a.appendChild(c.createComment("")),a.getElementsByTagName("*").length>0&&(l.find.TAG=function(a,b){var c=b.getElementsByTagName(a[1]);if(a[1]==="*"){var d=[];for(var e=0;c[e];e++)c[e].nodeType===1&&d.push(c[e]);c=d}return c}),a.innerHTML="<a href='#'></a>",a.firstChild&&typeof a.firstChild.getAttribute!="undefined"&&a.firstChild.getAttribute("href")!=="#"&&(l.attrHandle.href=function(a){return a.getAttribute("href",2)}),a=null}(),c.querySelectorAll&&function(){var a=k,b=c.createElement("div"),d="__sizzle__";b.innerHTML="<p class='TEST'></p>";if(!b.querySelectorAll||b.querySelectorAll(".TEST").length!==0){k=function(b,e,f,g){e=e||c;if(!g&&!k.isXML(e)){var h=/^(\w+$)|^\.([\w\-]+$)|^#([\w\-]+$)/.exec(b);if(h&&(e.nodeType===1||e.nodeType===9)){if(h[1])return p(e.getElementsByTagName(b),f);if(h[2]&&l.find.CLASS&&e.getElementsByClassName)return p(e.getElementsByClassName(h[2]),f)}if(e.nodeType===9){if(b==="body"&&e.body)return p([e.body],f);if(h&&h[3]){var i=e.getElementById(h[3]);if(!i||!i.parentNode)return p([],f);if(i.id===h[3])return p([i],f)}try{return p(e.querySelectorAll(b),f)}catch(j){}}else if(e.nodeType===1&&e.nodeName.toLowerCase()!=="object"){var m=e,n=e.getAttribute("id"),o=n||d,q=e.parentNode,r=/^\s*[+~]/.test(b);n?o=o.replace(/'/g,"\\$&"):e.setAttribute("id",o),r&&q&&(e=e.parentNode);try{if(!r||q)return p(e.querySelectorAll("[id='"+o+"'] "+b),f)}catch(s){}finally{n||m.removeAttribute("id")}}}return a(b,e,f,g)};for(var e in a)k[e]=a[e];b=null}}(),function(){var a=c.documentElement,b=a.matchesSelector||a.mozMatchesSelector||a.webkitMatchesSelector||a.msMatchesSelector;if(b){var d=!b.call(c.createElement("div"),"div"),e=!1;try{b.call(c.documentElement,"[test!='']:sizzle")}catch(f){e=!0}k.matchesSelector=function(a,c){c=c.replace(/\=\s*([^'"\]]*)\s*\]/g,"='$1']");if(!k.isXML(a))try{if(e||!l.match.PSEUDO.test(c)&&!/!=/.test(c)){var f=b.call(a,c);if(f||!d||a.document&&a.document.nodeType!==11)return f}}catch(g){}return k(c,null,null,[a]).length>0}}}(),function(){var a=c.createElement("div");a.innerHTML="<div class='test e'></div><div class='test'></div>";if(!!a.getElementsByClassName&&a.getElementsByClassName("e").length!==0){a.lastChild.className="e";if(a.getElementsByClassName("e").length===1)return;l.order.splice(1,0,"CLASS"),l.find.CLASS=function(a,b,c){if(typeof b.getElementsByClassName!="undefined"&&!c)return b.getElementsByClassName(a[1])},a=null}}(),c.documentElement.contains?k.contains=function(a,b){return a!==b&&(a.contains?a.contains(b):!0)}:c.documentElement.compareDocumentPosition?k.contains=function(a,b){return!!(a.compareDocumentPosition(b)&16)}:k.contains=function(){return!1},k.isXML=function(a){var b=(a?a.ownerDocument||a:0).documentElement;return b?b.nodeName!=="HTML":!1};var v=function(a,b){var c,d=[],e="",f=b.nodeType?[b]:b;while(c=l.match.PSEUDO.exec(a))e+=c[0],a=a.replace(l.match.PSEUDO,"");a=l.relative[a]?a+"*":a;for(var g=0,h=f.length;g<h;g++)k(a,f[g],d);return k.filter(e,d)};f.find=k,f.expr=k.selectors,f.expr[":"]=f.expr.filters,f.unique=k.uniqueSort,f.text=k.getText,f.isXMLDoc=k.isXML,f.contains=k.contains}();var O=/Until$/,P=/^(?:parents|prevUntil|prevAll)/,Q=/,/,R=/^.[^:#\[\.,]*$/,S=Array.prototype.slice,T=f.expr.match.POS,U={children:!0,contents:!0,next:!0,prev:!0};f.fn.extend({find:function(a){var b=this,c,d;if(typeof a!="string")return f(a).filter(function(){for(c=0,d=b.length;c<d;c++)if(f.contains(b[c],this))return!0});var e=this.pushStack("","find",a),g,h,i;for(c=0,d=this.length;c<d;c++){g=e.length,f.find(a,this[c],e);if(c>0)for(h=g;h<e.length;h++)for(i=0;i<g;i++)if(e[i]===e[h]){e.splice(h--,1);break}}return e},has:function(a){var b=f(a);return this.filter(function(){for(var a=0,c=b.length;a<c;a++)if(f.contains(this,b[a]))return!0})},not:function(a){return this.pushStack(W(this,a,!1),"not",a)},filter:function(a){return this.pushStack(W(this,a,!0),"filter",a)},is:function(a){return!!a&&(typeof a=="string"?f.filter(a,this).length>0:this.filter(a).length>0)},closest:function(a,b){var c=[],d,e,g=this[0];if(f.isArray(a)){var h,i,j={},k=1;if(g&&a.length){for(d=0,e=a.length;d<e;d++)i=a[d],j[i]||(j[i]=T.test(i)?f(i,b||this.context):i);while(g&&g.ownerDocument&&g!==b){for(i in j)h=j[i],(h.jquery?h.index(g)>-1:f(g).is(h))&&c.push({selector:i,elem:g,level:k});g=g.parentNode,k++}}return c}var l=T.test(a)||typeof a!="string"?f(a,b||this.context):0;for(d=0,e=this.length;d<e;d++){g=this[d];while(g){if(l?l.index(g)>-1:f.find.matchesSelector(g,a)){c.push(g);break}g=g.parentNode;if(!g||!g.ownerDocument||g===b||g.nodeType===11)break}}c=c.length>1?f.unique(c):c;return this.pushStack(c,"closest",a)},index:function(a){if(!a||typeof a=="string")return f.inArray(this[0],a?f(a):this.parent().children());return f.inArray(a.jquery?a[0]:a,this)},add:function(a,b){var c=typeof a=="string"?f(a,b):f.makeArray(a&&a.nodeType?[a]:a),d=f.merge(this.get(),c);return this.pushStack(V(c[0])||V(d[0])?d:f.unique(d))},andSelf:function(){return this.add(this.prevObject)}}),f.each({parent:function(a){var b=a.parentNode;return b&&b.nodeType!==11?b:null},parents:function(a){return f.dir(a,"parentNode")},parentsUntil:function(a,b,c){return f.dir(a,"parentNode",c)},next:function(a){return f.nth(a,2,"nextSibling")},prev:function(a){return f.nth(a,2,"previousSibling")},nextAll:function(a){return f.dir(a,"nextSibling")},prevAll:function(a){return f.dir(a,"previousSibling")},nextUntil:function(a,b,c){return f.dir(a,"nextSibling",c)},prevUntil:function(a,b,c){return f.dir(a,"previousSibling",c)},siblings:function(a){return f.sibling(a.parentNode.firstChild,a)},children:function(a){return f.sibling(a.firstChild)},contents:function(a){return f.nodeName(a,"iframe")?a.contentDocument||a.contentWindow.document:f.makeArray(a.childNodes)}},function(a,b){f.fn[a]=function(c,d){var e=f.map(this,b,c),g=S.call(arguments);O.test(a)||(d=c),d&&typeof d=="string"&&(e=f.filter(d,e)),e=this.length>1&&!U[a]?f.unique(e):e,(this.length>1||Q.test(d))&&P.test(a)&&(e=e.reverse());return this.pushStack(e,a,g.join(","))}}),f.extend({filter:function(a,b,c){c&&(a=":not("+a+")");return b.length===1?f.find.matchesSelector(b[0],a)?[b[0]]:[]:f.find.matches(a,b)},dir:function(a,c,d){var e=[],g=a[c];while(g&&g.nodeType!==9&&(d===b||g.nodeType!==1||!f(g).is(d)))g.nodeType===1&&e.push(g),g=g[c];return e},nth:function(a,b,c,d){b=b||1;var e=0;for(;a;a=a[c])if(a.nodeType===1&&++e===b)break;return a},sibling:function(a,b){var c=[];for(;a;a=a.nextSibling)a.nodeType===1&&a!==b&&c.push(a);return c}});var X=/ jQuery\d+="(?:\d+|null)"/g,Y=/^\s+/,Z=/<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/ig,$=/<([\w:]+)/,_=/<tbody/i,ba=/<|&#?\w+;/,bb=/<(?:script|object|embed|option|style)/i,bc=/checked\s*(?:[^=]|=\s*.checked.)/i,bd=/\/(java|ecma)script/i,be=/^\s*<!(?:\[CDATA\[|\-\-)/,bf={option:[1,"<select multiple='multiple'>","</select>"],legend:[1,"<fieldset>","</fieldset>"],thead:[1,"<table>","</table>"],tr:[2,"<table><tbody>","</tbody></table>"],td:[3,"<table><tbody><tr>","</tr></tbody></table>"],col:[2,"<table><tbody></tbody><colgroup>","</colgroup></table>"],area:[1,"<map>","</map>"],_default:[0,"",""]};bf.optgroup=bf.option,bf.tbody=bf.tfoot=bf.colgroup=bf.caption=bf.thead,bf.th=bf.td,f.support.htmlSerialize||(bf._default=[1,"div<div>","</div>"]),f.fn.extend({text:function(a){if(f.isFunction(a))return this.each(function(b){var c=f(this);c.text(a.call(this,b,c.text()))});if(typeof a!="object"&&a!==b)return this.empty().append((this[0]&&this[0].ownerDocument||c).createTextNode(a));return f.text(this)},wrapAll:function(a){if(f.isFunction(a))return this.each(function(b){f(this).wrapAll(a.call(this,b))});if(this[0]){var b=f(a,this[0].ownerDocument).eq(0).clone(!0);this[0].parentNode&&b.insertBefore(this[0]),b.map(function(){var a=this;while(a.firstChild&&a.firstChild.nodeType===1)a=a.firstChild;return a}).append(this)}return this},wrapInner:function(a){if(f.isFunction(a))return this.each(function(b){f(this).wrapInner(a.call(this,b))});return this.each(function(){var b=f(this),c=b.contents();c.length?c.wrapAll(a):b.append(a)})},wrap:function(a){return this.each(function(){f(this).wrapAll(a)})},unwrap:function(){return this.parent().each(function(){f.nodeName(this,"body")||f(this).replaceWith(this.childNodes)}).end()},append:function(){return this.domManip(arguments,!0,function(a){this.nodeType===1&&this.appendChild(a)})},prepend:function(){return this.domManip(arguments,!0,function(a){this.nodeType===1&&this.insertBefore(a,this.firstChild)})},before:function(){if(this[0]&&this[0].parentNode)return this.domManip(arguments,!1,function(a){this.parentNode.insertBefore(a,this)});if(arguments.length){var a=f(arguments[0]);a.push.apply(a,this.toArray());return this.pushStack(a,"before",arguments)}},after:function(){if(this[0]&&this[0].parentNode)return this.domManip(arguments,!1,function(a){this.parentNode.insertBefore(a,this.nextSibling)});if(arguments.length){var a=this.pushStack(this,"after",arguments);a.push.apply(a,f(arguments[0]).toArray());return a}},remove:function(a,b){for(var c=0,d;(d=this[c])!=null;c++)if(!a||f.filter(a,[d]).length)!b&&d.nodeType===1&&(f.cleanData(d.getElementsByTagName("*")),f.cleanData([d])),d.parentNode&&d.parentNode.removeChild(d);return this},empty:function(){for(var a=0,b;(b=this[a])!=null;a++){b.nodeType===1&&f.cleanData(b.getElementsByTagName("*"));while(b.firstChild)b.removeChild(b.firstChild)}return this},clone:function(a,b){a=a==null?!1:a,b=b==null?a:b;return this.map(function(){return f.clone(this,a,b)})},html:function(a){if(a===b)return this[0]&&this[0].nodeType===1?this[0].innerHTML.replace(X,""):null;if(typeof a=="string"&&!bb.test(a)&&(f.support.leadingWhitespace||!Y.test(a))&&!bf[($.exec(a)||["",""])[1].toLowerCase()]){a=a.replace(Z,"<$1></$2>");try{for(var c=0,d=this.length;c<d;c++)this[c].nodeType===1&&(f.cleanData(this[c].getElementsByTagName("*")),this[c].innerHTML=a)}catch(e){this.empty().append(a)}}else f.isFunction(a)?this.each(function(b){var c=f(this);c.html(a.call(this,b,c.html()))}):this.empty().append(a);return this},replaceWith:function(a){if(this[0]&&this[0].parentNode){if(f.isFunction(a))return this.each(function(b){var c=f(this),d=c.html();c.replaceWith(a.call(this,b,d))});typeof a!="string"&&(a=f(a).detach());return this.each(function(){var b=this.nextSibling,c=this.parentNode;f(this).remove(),b?f(b).before(a):f(c).append(a)})}return this.length?this.pushStack(f(f.isFunction(a)?a():a),"replaceWith",a):this},detach:function(a){return this.remove(a,!0)},domManip:function(a,c,d){var e,g,h,i,j=a[0],k=[];if(!f.support.checkClone&&arguments.length===3&&typeof j=="string"&&bc.test(j))return this.each(function(){f(this).domManip(a,c,d,!0)});if(f.isFunction(j))return this.each(function(e){var g=f(this);a[0]=j.call(this,e,c?g.html():b),g.domManip(a,c,d)});if(this[0]){i=j&&j.parentNode,f.support.parentNode&&i&&i.nodeType===11&&i.childNodes.length===this.length?e={fragment:i}:e=f.buildFragment(a,this,k),h=e.fragment,h.childNodes.length===1?g=h=h.firstChild:g=h.firstChild;if(g){c=c&&f.nodeName(g,"tr");for(var l=0,m=this.length,n=m-1;l<m;l++)d.call(c?bg(this[l],g):this[l],e.cacheable||m>1&&l<n?f.clone(h,!0,!0):h)}k.length&&f.each(k,bm)}return this}}),f.buildFragment=function(a,b,d){var e,g,h,i;b&&b[0]&&(i=b[0].ownerDocument||b[0]),i.createDocumentFragment||(i=c),a.length===1&&typeof a[0]=="string"&&a[0].length<512&&i===c&&a[0].charAt(0)==="<"&&!bb.test(a[0])&&(f.support.checkClone||!bc.test(a[0]))&&(g=!0,h=f.fragments[a[0]],h&&h!==1&&(e=h)),e||(e=i.createDocumentFragment(),f.clean(a,i,e,d)),g&&(f.fragments[a[0]]=h?e:1);return{fragment:e,cacheable:g}},f.fragments={},f.each({appendTo:"append",prependTo:"prepend",insertBefore:"before",insertAfter:"after",replaceAll:"replaceWith"},function(a,b){f.fn[a]=function(c){var d=[],e=f(c),g=this.length===1&&this[0].parentNode;if(g&&g.nodeType===11&&g.childNodes.length===1&&e.length===1){e[b](this[0]);return this}for(var h=0,i=e.length;h<i;h++){var j=(h>0?this.clone(!0):this).get();f(e[h])[b](j),d=d.concat(j
)}return this.pushStack(d,a,e.selector)}}),f.extend({clone:function(a,b,c){var d=a.cloneNode(!0),e,g,h;if((!f.support.noCloneEvent||!f.support.noCloneChecked)&&(a.nodeType===1||a.nodeType===11)&&!f.isXMLDoc(a)){bi(a,d),e=bj(a),g=bj(d);for(h=0;e[h];++h)bi(e[h],g[h])}if(b){bh(a,d);if(c){e=bj(a),g=bj(d);for(h=0;e[h];++h)bh(e[h],g[h])}}e=g=null;return d},clean:function(a,b,d,e){var g;b=b||c,typeof b.createElement=="undefined"&&(b=b.ownerDocument||b[0]&&b[0].ownerDocument||c);var h=[],i;for(var j=0,k;(k=a[j])!=null;j++){typeof k=="number"&&(k+="");if(!k)continue;if(typeof k=="string")if(!ba.test(k))k=b.createTextNode(k);else{k=k.replace(Z,"<$1></$2>");var l=($.exec(k)||["",""])[1].toLowerCase(),m=bf[l]||bf._default,n=m[0],o=b.createElement("div");o.innerHTML=m[1]+k+m[2];while(n--)o=o.lastChild;if(!f.support.tbody){var p=_.test(k),q=l==="table"&&!p?o.firstChild&&o.firstChild.childNodes:m[1]==="<table>"&&!p?o.childNodes:[];for(i=q.length-1;i>=0;--i)f.nodeName(q[i],"tbody")&&!q[i].childNodes.length&&q[i].parentNode.removeChild(q[i])}!f.support.leadingWhitespace&&Y.test(k)&&o.insertBefore(b.createTextNode(Y.exec(k)[0]),o.firstChild),k=o.childNodes}var r;if(!f.support.appendChecked)if(k[0]&&typeof (r=k.length)=="number")for(i=0;i<r;i++)bl(k[i]);else bl(k);k.nodeType?h.push(k):h=f.merge(h,k)}if(d){g=function(a){return!a.type||bd.test(a.type)};for(j=0;h[j];j++)if(e&&f.nodeName(h[j],"script")&&(!h[j].type||h[j].type.toLowerCase()==="text/javascript"))e.push(h[j].parentNode?h[j].parentNode.removeChild(h[j]):h[j]);else{if(h[j].nodeType===1){var s=f.grep(h[j].getElementsByTagName("script"),g);h.splice.apply(h,[j+1,0].concat(s))}d.appendChild(h[j])}}return h},cleanData:function(a){var b,c,d=f.cache,e=f.expando,g=f.event.special,h=f.support.deleteExpando;for(var i=0,j;(j=a[i])!=null;i++){if(j.nodeName&&f.noData[j.nodeName.toLowerCase()])continue;c=j[f.expando];if(c){b=d[c]&&d[c][e];if(b&&b.events){for(var k in b.events)g[k]?f.event.remove(j,k):f.removeEvent(j,k,b.handle);b.handle&&(b.handle.elem=null)}h?delete j[f.expando]:j.removeAttribute&&j.removeAttribute(f.expando),delete d[c]}}}});var bn=/alpha\([^)]*\)/i,bo=/opacity=([^)]*)/,bp=/([A-Z]|^ms)/g,bq=/^-?\d+(?:px)?$/i,br=/^-?\d/,bs=/^[+\-]=/,bt=/[^+\-\.\de]+/g,bu={position:"absolute",visibility:"hidden",display:"block"},bv=["Left","Right"],bw=["Top","Bottom"],bx,by,bz;f.fn.css=function(a,c){if(arguments.length===2&&c===b)return this;return f.access(this,a,c,!0,function(a,c,d){return d!==b?f.style(a,c,d):f.css(a,c)})},f.extend({cssHooks:{opacity:{get:function(a,b){if(b){var c=bx(a,"opacity","opacity");return c===""?"1":c}return a.style.opacity}}},cssNumber:{fillOpacity:!0,fontWeight:!0,lineHeight:!0,opacity:!0,orphans:!0,widows:!0,zIndex:!0,zoom:!0},cssProps:{"float":f.support.cssFloat?"cssFloat":"styleFloat"},style:function(a,c,d,e){if(!!a&&a.nodeType!==3&&a.nodeType!==8&&!!a.style){var g,h,i=f.camelCase(c),j=a.style,k=f.cssHooks[i];c=f.cssProps[i]||i;if(d===b){if(k&&"get"in k&&(g=k.get(a,!1,e))!==b)return g;return j[c]}h=typeof d;if(h==="number"&&isNaN(d)||d==null)return;h==="string"&&bs.test(d)&&(d=+d.replace(bt,"")+parseFloat(f.css(a,c)),h="number"),h==="number"&&!f.cssNumber[i]&&(d+="px");if(!k||!("set"in k)||(d=k.set(a,d))!==b)try{j[c]=d}catch(l){}}},css:function(a,c,d){var e,g;c=f.camelCase(c),g=f.cssHooks[c],c=f.cssProps[c]||c,c==="cssFloat"&&(c="float");if(g&&"get"in g&&(e=g.get(a,!0,d))!==b)return e;if(bx)return bx(a,c)},swap:function(a,b,c){var d={};for(var e in b)d[e]=a.style[e],a.style[e]=b[e];c.call(a);for(e in b)a.style[e]=d[e]}}),f.curCSS=f.css,f.each(["height","width"],function(a,b){f.cssHooks[b]={get:function(a,c,d){var e;if(c){if(a.offsetWidth!==0)return bA(a,b,d);f.swap(a,bu,function(){e=bA(a,b,d)});return e}},set:function(a,b){if(!bq.test(b))return b;b=parseFloat(b);if(b>=0)return b+"px"}}}),f.support.opacity||(f.cssHooks.opacity={get:function(a,b){return bo.test((b&&a.currentStyle?a.currentStyle.filter:a.style.filter)||"")?parseFloat(RegExp.$1)/100+"":b?"1":""},set:function(a,b){var c=a.style,d=a.currentStyle;c.zoom=1;var e=f.isNaN(b)?"":"alpha(opacity="+b*100+")",g=d&&d.filter||c.filter||"";c.filter=bn.test(g)?g.replace(bn,e):g+" "+e}}),f(function(){f.support.reliableMarginRight||(f.cssHooks.marginRight={get:function(a,b){var c;f.swap(a,{display:"inline-block"},function(){b?c=bx(a,"margin-right","marginRight"):c=a.style.marginRight});return c}})}),c.defaultView&&c.defaultView.getComputedStyle&&(by=function(a,c){var d,e,g;c=c.replace(bp,"-$1").toLowerCase();if(!(e=a.ownerDocument.defaultView))return b;if(g=e.getComputedStyle(a,null))d=g.getPropertyValue(c),d===""&&!f.contains(a.ownerDocument.documentElement,a)&&(d=f.style(a,c));return d}),c.documentElement.currentStyle&&(bz=function(a,b){var c,d=a.currentStyle&&a.currentStyle[b],e=a.runtimeStyle&&a.runtimeStyle[b],f=a.style;!bq.test(d)&&br.test(d)&&(c=f.left,e&&(a.runtimeStyle.left=a.currentStyle.left),f.left=b==="fontSize"?"1em":d||0,d=f.pixelLeft+"px",f.left=c,e&&(a.runtimeStyle.left=e));return d===""?"auto":d}),bx=by||bz,f.expr&&f.expr.filters&&(f.expr.filters.hidden=function(a){var b=a.offsetWidth,c=a.offsetHeight;return b===0&&c===0||!f.support.reliableHiddenOffsets&&(a.style.display||f.css(a,"display"))==="none"},f.expr.filters.visible=function(a){return!f.expr.filters.hidden(a)});var bB=/%20/g,bC=/\[\]$/,bD=/\r?\n/g,bE=/#.*$/,bF=/^(.*?):[ \t]*([^\r\n]*)\r?$/mg,bG=/^(?:color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week)$/i,bH=/^(?:about|app|app\-storage|.+\-extension|file|widget):$/,bI=/^(?:GET|HEAD)$/,bJ=/^\/\//,bK=/\?/,bL=/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,bM=/^(?:select|textarea)/i,bN=/\s+/,bO=/([?&])_=[^&]*/,bP=/^([\w\+\.\-]+:)(?:\/\/([^\/?#:]*)(?::(\d+))?)?/,bQ=f.fn.load,bR={},bS={},bT,bU;try{bT=e.href}catch(bV){bT=c.createElement("a"),bT.href="",bT=bT.href}bU=bP.exec(bT.toLowerCase())||[],f.fn.extend({load:function(a,c,d){if(typeof a!="string"&&bQ)return bQ.apply(this,arguments);if(!this.length)return this;var e=a.indexOf(" ");if(e>=0){var g=a.slice(e,a.length);a=a.slice(0,e)}var h="GET";c&&(f.isFunction(c)?(d=c,c=b):typeof c=="object"&&(c=f.param(c,f.ajaxSettings.traditional),h="POST"));var i=this;f.ajax({url:a,type:h,dataType:"html",data:c,complete:function(a,b,c){c=a.responseText,a.isResolved()&&(a.done(function(a){c=a}),i.html(g?f("<div>").append(c.replace(bL,"")).find(g):c)),d&&i.each(d,[c,b,a])}});return this},serialize:function(){return f.param(this.serializeArray())},serializeArray:function(){return this.map(function(){return this.elements?f.makeArray(this.elements):this}).filter(function(){return this.name&&!this.disabled&&(this.checked||bM.test(this.nodeName)||bG.test(this.type))}).map(function(a,b){var c=f(this).val();return c==null?null:f.isArray(c)?f.map(c,function(a,c){return{name:b.name,value:a.replace(bD,"\r\n")}}):{name:b.name,value:c.replace(bD,"\r\n")}}).get()}}),f.each("ajaxStart ajaxStop ajaxComplete ajaxError ajaxSuccess ajaxSend".split(" "),function(a,b){f.fn[b]=function(a){return this.bind(b,a)}}),f.each(["get","post"],function(a,c){f[c]=function(a,d,e,g){f.isFunction(d)&&(g=g||e,e=d,d=b);return f.ajax({type:c,url:a,data:d,success:e,dataType:g})}}),f.extend({getScript:function(a,c){return f.get(a,b,c,"script")},getJSON:function(a,b,c){return f.get(a,b,c,"json")},ajaxSetup:function(a,b){b?f.extend(!0,a,f.ajaxSettings,b):(b=a,a=f.extend(!0,f.ajaxSettings,b));for(var c in{context:1,url:1})c in b?a[c]=b[c]:c in f.ajaxSettings&&(a[c]=f.ajaxSettings[c]);return a},ajaxSettings:{url:bT,isLocal:bH.test(bU[1]),global:!0,type:"GET",contentType:"application/x-www-form-urlencoded",processData:!0,async:!0,accepts:{xml:"application/xml, text/xml",html:"text/html",text:"text/plain",json:"application/json, text/javascript","*":"*/*"},contents:{xml:/xml/,html:/html/,json:/json/},responseFields:{xml:"responseXML",text:"responseText"},converters:{"* text":a.String,"text html":!0,"text json":f.parseJSON,"text xml":f.parseXML}},ajaxPrefilter:bW(bR),ajaxTransport:bW(bS),ajax:function(a,c){function w(a,c,l,m){if(s!==2){s=2,q&&clearTimeout(q),p=b,n=m||"",v.readyState=a?4:0;var o,r,u,w=l?bZ(d,v,l):b,x,y;if(a>=200&&a<300||a===304){if(d.ifModified){if(x=v.getResponseHeader("Last-Modified"))f.lastModified[k]=x;if(y=v.getResponseHeader("Etag"))f.etag[k]=y}if(a===304)c="notmodified",o=!0;else try{r=b$(d,w),c="success",o=!0}catch(z){c="parsererror",u=z}}else{u=c;if(!c||a)c="error",a<0&&(a=0)}v.status=a,v.statusText=c,o?h.resolveWith(e,[r,c,v]):h.rejectWith(e,[v,c,u]),v.statusCode(j),j=b,t&&g.trigger("ajax"+(o?"Success":"Error"),[v,d,o?r:u]),i.resolveWith(e,[v,c]),t&&(g.trigger("ajaxComplete",[v,d]),--f.active||f.event.trigger("ajaxStop"))}}typeof a=="object"&&(c=a,a=b),c=c||{};var d=f.ajaxSetup({},c),e=d.context||d,g=e!==d&&(e.nodeType||e instanceof f)?f(e):f.event,h=f.Deferred(),i=f._Deferred(),j=d.statusCode||{},k,l={},m={},n,o,p,q,r,s=0,t,u,v={readyState:0,setRequestHeader:function(a,b){if(!s){var c=a.toLowerCase();a=m[c]=m[c]||a,l[a]=b}return this},getAllResponseHeaders:function(){return s===2?n:null},getResponseHeader:function(a){var c;if(s===2){if(!o){o={};while(c=bF.exec(n))o[c[1].toLowerCase()]=c[2]}c=o[a.toLowerCase()]}return c===b?null:c},overrideMimeType:function(a){s||(d.mimeType=a);return this},abort:function(a){a=a||"abort",p&&p.abort(a),w(0,a);return this}};h.promise(v),v.success=v.done,v.error=v.fail,v.complete=i.done,v.statusCode=function(a){if(a){var b;if(s<2)for(b in a)j[b]=[j[b],a[b]];else b=a[v.status],v.then(b,b)}return this},d.url=((a||d.url)+"").replace(bE,"").replace(bJ,bU[1]+"//"),d.dataTypes=f.trim(d.dataType||"*").toLowerCase().split(bN),d.crossDomain==null&&(r=bP.exec(d.url.toLowerCase()),d.crossDomain=!(!r||r[1]==bU[1]&&r[2]==bU[2]&&(r[3]||(r[1]==="http:"?80:443))==(bU[3]||(bU[1]==="http:"?80:443)))),d.data&&d.processData&&typeof d.data!="string"&&(d.data=f.param(d.data,d.traditional)),bX(bR,d,c,v);if(s===2)return!1;t=d.global,d.type=d.type.toUpperCase(),d.hasContent=!bI.test(d.type),t&&f.active++===0&&f.event.trigger("ajaxStart");if(!d.hasContent){d.data&&(d.url+=(bK.test(d.url)?"&":"?")+d.data),k=d.url;if(d.cache===!1){var x=f.now(),y=d.url.replace(bO,"$1_="+x);d.url=y+(y===d.url?(bK.test(d.url)?"&":"?")+"_="+x:"")}}(d.data&&d.hasContent&&d.contentType!==!1||c.contentType)&&v.setRequestHeader("Content-Type",d.contentType),d.ifModified&&(k=k||d.url,f.lastModified[k]&&v.setRequestHeader("If-Modified-Since",f.lastModified[k]),f.etag[k]&&v.setRequestHeader("If-None-Match",f.etag[k])),v.setRequestHeader("Accept",d.dataTypes[0]&&d.accepts[d.dataTypes[0]]?d.accepts[d.dataTypes[0]]+(d.dataTypes[0]!=="*"?", */*; q=0.01":""):d.accepts["*"]);for(u in d.headers)v.setRequestHeader(u,d.headers[u]);if(d.beforeSend&&(d.beforeSend.call(e,v,d)===!1||s===2)){v.abort();return!1}for(u in{success:1,error:1,complete:1})v[u](d[u]);p=bX(bS,d,c,v);if(!p)w(-1,"No Transport");else{v.readyState=1,t&&g.trigger("ajaxSend",[v,d]),d.async&&d.timeout>0&&(q=setTimeout(function(){v.abort("timeout")},d.timeout));try{s=1,p.send(l,w)}catch(z){status<2?w(-1,z):f.error(z)}}return v},param:function(a,c){var d=[],e=function(a,b){b=f.isFunction(b)?b():b,d[d.length]=encodeURIComponent(a)+"="+encodeURIComponent(b)};c===b&&(c=f.ajaxSettings.traditional);if(f.isArray(a)||a.jquery&&!f.isPlainObject(a))f.each(a,function(){e(this.name,this.value)});else for(var g in a)bY(g,a[g],c,e);return d.join("&").replace(bB,"+")}}),f.extend({active:0,lastModified:{},etag:{}});var b_=f.now(),ca=/(\=)\?(&|$)|\?\?/i;f.ajaxSetup({jsonp:"callback",jsonpCallback:function(){return f.expando+"_"+b_++}}),f.ajaxPrefilter("json jsonp",function(b,c,d){var e=b.contentType==="application/x-www-form-urlencoded"&&typeof b.data=="string";if(b.dataTypes[0]==="jsonp"||b.jsonp!==!1&&(ca.test(b.url)||e&&ca.test(b.data))){var g,h=b.jsonpCallback=f.isFunction(b.jsonpCallback)?b.jsonpCallback():b.jsonpCallback,i=a[h],j=b.url,k=b.data,l="$1"+h+"$2";b.jsonp!==!1&&(j=j.replace(ca,l),b.url===j&&(e&&(k=k.replace(ca,l)),b.data===k&&(j+=(/\?/.test(j)?"&":"?")+b.jsonp+"="+h))),b.url=j,b.data=k,a[h]=function(a){g=[a]},d.always(function(){a[h]=i,g&&f.isFunction(i)&&a[h](g[0])}),b.converters["script json"]=function(){g||f.error(h+" was not called");return g[0]},b.dataTypes[0]="json";return"script"}}),f.ajaxSetup({accepts:{script:"text/javascript, application/javascript, application/ecmascript, application/x-ecmascript"},contents:{script:/javascript|ecmascript/},converters:{"text script":function(a){f.globalEval(a);return a}}}),f.ajaxPrefilter("script",function(a){a.cache===b&&(a.cache=!1),a.crossDomain&&(a.type="GET",a.global=!1)}),f.ajaxTransport("script",function(a){if(a.crossDomain){var d,e=c.head||c.getElementsByTagName("head")[0]||c.documentElement;return{send:function(f,g){d=c.createElement("script"),d.async="async",a.scriptCharset&&(d.charset=a.scriptCharset),d.src=a.url,d.onload=d.onreadystatechange=function(a,c){if(c||!d.readyState||/loaded|complete/.test(d.readyState))d.onload=d.onreadystatechange=null,e&&d.parentNode&&e.removeChild(d),d=b,c||g(200,"success")},e.insertBefore(d,e.firstChild)},abort:function(){d&&d.onload(0,1)}}}});var cb=a.ActiveXObject?function(){for(var a in cd)cd[a](0,1)}:!1,cc=0,cd;f.ajaxSettings.xhr=a.ActiveXObject?function(){return!this.isLocal&&ce()||cf()}:ce,function(a){f.extend(f.support,{ajax:!!a,cors:!!a&&"withCredentials"in a})}(f.ajaxSettings.xhr()),f.support.ajax&&f.ajaxTransport(function(c){if(!c.crossDomain||f.support.cors){var d;return{send:function(e,g){var h=c.xhr(),i,j;c.username?h.open(c.type,c.url,c.async,c.username,c.password):h.open(c.type,c.url,c.async);if(c.xhrFields)for(j in c.xhrFields)h[j]=c.xhrFields[j];c.mimeType&&h.overrideMimeType&&h.overrideMimeType(c.mimeType),!c.crossDomain&&!e["X-Requested-With"]&&(e["X-Requested-With"]="XMLHttpRequest");try{for(j in e)h.setRequestHeader(j,e[j])}catch(k){}h.send(c.hasContent&&c.data||null),d=function(a,e){var j,k,l,m,n;try{if(d&&(e||h.readyState===4)){d=b,i&&(h.onreadystatechange=f.noop,cb&&delete cd[i]);if(e)h.readyState!==4&&h.abort();else{j=h.status,l=h.getAllResponseHeaders(),m={},n=h.responseXML,n&&n.documentElement&&(m.xml=n),m.text=h.responseText;try{k=h.statusText}catch(o){k=""}!j&&c.isLocal&&!c.crossDomain?j=m.text?200:404:j===1223&&(j=204)}}}catch(p){e||g(-1,p)}m&&g(j,k,m,l)},!c.async||h.readyState===4?d():(i=++cc,cb&&(cd||(cd={},f(a).unload(cb)),cd[i]=d),h.onreadystatechange=d)},abort:function(){d&&d(0,1)}}}});var cg={},ch,ci,cj=/^(?:toggle|show|hide)$/,ck=/^([+\-]=)?([\d+.\-]+)([a-z%]*)$/i,cl,cm=[["height","marginTop","marginBottom","paddingTop","paddingBottom"],["width","marginLeft","marginRight","paddingLeft","paddingRight"],["opacity"]],cn,co=a.webkitRequestAnimationFrame||a.mozRequestAnimationFrame||a.oRequestAnimationFrame;f.fn.extend({show:function(a,b,c){var d,e;if(a||a===0)return this.animate(cr("show",3),a,b,c);for(var g=0,h=this.length;g<h;g++)d=this[g],d.style&&(e=d.style.display,!f._data(d,"olddisplay")&&e==="none"&&(e=d.style.display=""),e===""&&f.css(d,"display")==="none"&&f._data(d,"olddisplay",cs(d.nodeName)));for(g=0;g<h;g++){d=this[g];if(d.style){e=d.style.display;if(e===""||e==="none")d.style.display=f._data(d,"olddisplay")||""}}return this},hide:function(a,b,c){if(a||a===0)return this.animate(cr("hide",3),a,b,c);for(var d=0,e=this.length;d<e;d++)if(this[d].style){var g=f.css(this[d],"display");g!=="none"&&!f._data(this[d],"olddisplay")&&f._data(this[d],"olddisplay",g)}for(d=0;d<e;d++)this[d].style&&(this[d].style.display="none");return this},_toggle:f.fn.toggle,toggle:function(a,b,c){var d=typeof a=="boolean";f.isFunction(a)&&f.isFunction(b)?this._toggle.apply(this,arguments):a==null||d?this.each(function(){var b=d?a:f(this).is(":hidden");f(this)[b?"show":"hide"]()}):this.animate(cr("toggle",3),a,b,c);return this},fadeTo:function(a,b,c,d){return this.filter(":hidden").css("opacity",0).show().end().animate({opacity:b},a,c,d)},animate:function(a,b,c,d){var e=f.speed(b,c,d);if(f.isEmptyObject(a))return this.each(e.complete,[!1]);a=f.extend({},a);return this[e.queue===!1?"each":"queue"](function(){e.queue===!1&&f._mark(this);var b=f.extend({},e),c=this.nodeType===1,d=c&&f(this).is(":hidden"),g,h,i,j,k,l,m,n,o;b.animatedProperties={};for(i in a){g=f.camelCase(i),i!==g&&(a[g]=a[i],delete a[i]),h=a[g],f.isArray(h)?(b.animatedProperties[g]=h[1],h=a[g]=h[0]):b.animatedProperties[g]=b.specialEasing&&b.specialEasing[g]||b.easing||"swing";if(h==="hide"&&d||h==="show"&&!d)return b.complete.call(this);c&&(g==="height"||g==="width")&&(b.overflow=[this.style.overflow,this.style.overflowX,this.style.overflowY],f.css(this,"display")==="inline"&&f.css(this,"float")==="none"&&(f.support.inlineBlockNeedsLayout?(j=cs(this.nodeName),j==="inline"?this.style.display="inline-block":(this.style.display="inline",this.style.zoom=1)):this.style.display="inline-block"))}b.overflow!=null&&(this.style.overflow="hidden");for(i in a)k=new f.fx(this,b,i),h=a[i],cj.test(h)?k[h==="toggle"?d?"show":"hide":h]():(l=ck.exec(h),m=k.cur(),l?(n=parseFloat(l[2]),o=l[3]||(f.cssNumber[i]?"":"px"),o!=="px"&&(f.style(this,i,(n||1)+o),m=(n||1)/k.cur()*m,f.style(this,i,m+o)),l[1]&&(n=(l[1]==="-="?-1:1)*n+m),k.custom(m,n,o)):k.custom(m,h,""));return!0})},stop:function(a,b){a&&this.queue([]),this.each(function(){var a=f.timers,c=a.length;b||f._unmark(!0,this);while(c--)a[c].elem===this&&(b&&a[c](!0),a.splice(c,1))}),b||this.dequeue();return this}}),f.each({slideDown:cr("show",1),slideUp:cr("hide",1),slideToggle:cr("toggle",1),fadeIn:{opacity:"show"},fadeOut:{opacity:"hide"},fadeToggle:{opacity:"toggle"}},function(a,b){f.fn[a]=function(a,c,d){return this.animate(b,a,c,d)}}),f.extend({speed:function(a,b,c){var d=a&&typeof a=="object"?f.extend({},a):{complete:c||!c&&b||f.isFunction(a)&&a,duration:a,easing:c&&b||b&&!f.isFunction(b)&&b};d.duration=f.fx.off?0:typeof d.duration=="number"?d.duration:d.duration in f.fx.speeds?f.fx.speeds[d.duration]:f.fx.speeds._default,d.old=d.complete,d.complete=function(a){f.isFunction(d.old)&&d.old.call(this),d.queue!==!1?f.dequeue(this):a!==!1&&f._unmark(this)};return d},easing:{linear:function(a,b,c,d){return c+d*a},swing:function(a,b,c,d){return(-Math.cos(a*Math.PI)/2+.5)*d+c}},timers:[],fx:function(a,b,c){this.options=b,this.elem=a,this.prop=c,b.orig=b.orig||{}}}),f.fx.prototype={update:function(){this.options.step&&this.options.step.call(this.elem,this.now,this),(f.fx.step[this.prop]||f.fx.step._default)(this)},cur:function(){if(this.elem[this.prop]!=null&&(!this.elem.style||this.elem.style[this.prop]==null))return this.elem[this.prop];var a,b=f.css(this.elem,this.prop);return isNaN(a=parseFloat(b))?!b||b==="auto"?0:b:a},custom:function(a,b,c){function h(a){return d.step(a)}var d=this,e=f.fx,g;this.startTime=cn||cp(),this.start=a,this.end=b,this.unit=c||this.unit||(f.cssNumber[this.prop]?"":"px"),this.now=this.start,this.pos=this.state=0,h.elem=this.elem,h()&&f.timers.push(h)&&!cl&&(co?(cl=!0,g=function(){cl&&(co(g),e.tick())},co(g)):cl=setInterval(e.tick,e.interval))},show:function(){this.options.orig[this.prop]=f.style(this.elem,this.prop),this.options.show=!0,this.custom(this.prop==="width"||this.prop==="height"?1:0,this.cur()),f(this.elem).show()},hide:function(){this.options.orig[this.prop]=f.style(this.elem,this.prop),this.options.hide=!0,this.custom(this.cur(),0)},step:function(a){var b=cn||cp(),c=!0,d=this.elem,e=this.options,g,h;if(a||b>=e.duration+this.startTime){this.now=this.end,this.pos=this.state=1,this.update(),e.animatedProperties[this.prop]=!0;for(g in e.animatedProperties)e.animatedProperties[g]!==!0&&(c=!1);if(c){e.overflow!=null&&!f.support.shrinkWrapBlocks&&f.each(["","X","Y"],function(a,b){d.style["overflow"+b]=e.overflow[a]}),e.hide&&f(d).hide();if(e.hide||e.show)for(var i in e.animatedProperties)f.style(d,i,e.orig[i]);e.complete.call(d)}return!1}e.duration==Infinity?this.now=b:(h=b-this.startTime,this.state=h/e.duration,this.pos=f.easing[e.animatedProperties[this.prop]](this.state,h,0,1,e.duration),this.now=this.start+(this.end-this.start)*this.pos),this.update();return!0}},f.extend(f.fx,{tick:function(){for(var a=f.timers,b=0;b<a.length;++b)a[b]()||a.splice(b--,1);a.length||f.fx.stop()},interval:13,stop:function(){clearInterval(cl),cl=null},speeds:{slow:600,fast:200,_default:400},step:{opacity:function(a){f.style(a.elem,"opacity",a.now)},_default:function(a){a.elem.style&&a.elem.style[a.prop]!=null?a.elem.style[a.prop]=(a.prop==="width"||a.prop==="height"?Math.max(0,a.now):a.now)+a.unit:a.elem[a.prop]=a.now}}}),f.expr&&f.expr.filters&&(f.expr.filters.animated=function(a){return f.grep(f.timers,function(b){return a===b.elem}).length});var ct=/^t(?:able|d|h)$/i,cu=/^(?:body|html)$/i;"getBoundingClientRect"in c.documentElement?f.fn.offset=function(a){var b=this[0],c;if(a)return this.each(function(b){f.offset.setOffset(this,a,b)});if(!b||!b.ownerDocument)return null;if(b===b.ownerDocument.body)return f.offset.bodyOffset(b);try{c=b.getBoundingClientRect()}catch(d){}var e=b.ownerDocument,g=e.documentElement;if(!c||!f.contains(g,b))return c?{top:c.top,left:c.left}:{top:0,left:0};var h=e.body,i=cv(e),j=g.clientTop||h.clientTop||0,k=g.clientLeft||h.clientLeft||0,l=i.pageYOffset||f.support.boxModel&&g.scrollTop||h.scrollTop,m=i.pageXOffset||f.support.boxModel&&g.scrollLeft||h.scrollLeft,n=c.top+l-j,o=c.left+m-k;return{top:n,left:o}}:f.fn.offset=function(a){var b=this[0];if(a)return this.each(function(b){f.offset.setOffset(this,a,b)});if(!b||!b.ownerDocument)return null;if(b===b.ownerDocument.body)return f.offset.bodyOffset(b);f.offset.initialize();var c,d=b.offsetParent,e=b,g=b.ownerDocument,h=g.documentElement,i=g.body,j=g.defaultView,k=j?j.getComputedStyle(b,null):b.currentStyle,l=b.offsetTop,m=b.offsetLeft;while((b=b.parentNode)&&b!==i&&b!==h){if(f.offset.supportsFixedPosition&&k.position==="fixed")break;c=j?j.getComputedStyle(b,null):b.currentStyle,l-=b.scrollTop,m-=b.scrollLeft,b===d&&(l+=b.offsetTop,m+=b.offsetLeft,f.offset.doesNotAddBorder&&(!f.offset.doesAddBorderForTableAndCells||!ct.test(b.nodeName))&&(l+=parseFloat(c.borderTopWidth)||0,m+=parseFloat(c.borderLeftWidth)||0),e=d,d=b.offsetParent),f.offset.subtractsBorderForOverflowNotVisible&&c.overflow!=="visible"&&(l+=parseFloat(c.borderTopWidth)||0,m+=parseFloat(c.borderLeftWidth)||0),k=c}if(k.position==="relative"||k.position==="static")l+=i.offsetTop,m+=i.offsetLeft;f.offset.supportsFixedPosition&&k.position==="fixed"&&(l+=Math.max(h.scrollTop,i.scrollTop),m+=Math.max(h.scrollLeft,i.scrollLeft));return{top:l,left:m}},f.offset={initialize:function(){var a=c.body,b=c.createElement("div"),d,e,g,h,i=parseFloat(f.css(a,"marginTop"))||0,j="<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";f.extend(b.style,{position:"absolute",top:0,left:0,margin:0,border:0,width:"1px",height:"1px",visibility:"hidden"}),b.innerHTML=j,a.insertBefore(b,a.firstChild),d=b.firstChild,e=d.firstChild,h=d.nextSibling.firstChild.firstChild,this.doesNotAddBorder=e.offsetTop!==5,this.doesAddBorderForTableAndCells=h.offsetTop===5,e.style.position="fixed",e.style.top="20px",this.supportsFixedPosition=e.offsetTop===20||e.offsetTop===15,e.style.position=e.style.top="",d.style.overflow="hidden",d.style.position="relative",this.subtractsBorderForOverflowNotVisible=e.offsetTop===-5,this.doesNotIncludeMarginInBodyOffset=a.offsetTop!==i,a.removeChild(b),f.offset.initialize=f.noop},bodyOffset:function(a){var b=a.offsetTop,c=a.offsetLeft;f.offset.initialize(),f.offset.doesNotIncludeMarginInBodyOffset&&(b+=parseFloat(f.css(a,"marginTop"))||0,c+=parseFloat(f.css(a,"marginLeft"))||0);return{top:b,left:c}},setOffset:function(a,b,c){var d=f.css(a,"position");d==="static"&&(a.style.position="relative");var e=f(a),g=e.offset(),h=f.css(a,"top"),i=f.css(a,"left"),j=(d==="absolute"||d==="fixed")&&f.inArray("auto",[h,i])>-1,k={},l={},m,n;j?(l=e.position(),m=l.top,n=l.left):(m=parseFloat(h)||0,n=parseFloat(i)||0),f.isFunction(b)&&(b=b.call(a,c,g)),b.top!=null&&(k.top=b.top-g.top+m),b.left!=null&&(k.left=b.left-g.left+n),"using"in b?b.using.call(a,k):e.css(k)}},f.fn.extend({position:function(){if(!this[0])return null;var a=this[0],b=this.offsetParent(),c=this.offset(),d=cu.test(b[0].nodeName)?{top:0,left:0}:b.offset();c.top-=parseFloat(f.css(a,"marginTop"))||0,c.left-=parseFloat(f.css(a,"marginLeft"))||0,d.top+=parseFloat(f.css(b[0],"borderTopWidth"))||0,d.left+=parseFloat(f.css(b[0],"borderLeftWidth"))||0;return{top:c.top-d.top,left:c.left-d.left}},offsetParent:function(){return this.map(function(){var a=this.offsetParent||c.body;while(a&&!cu.test(a.nodeName)&&f.css(a,"position")==="static")a=a.offsetParent;return a})}}),f.each(["Left","Top"],function(a,c){var d="scroll"+c;f.fn[d]=function(c){var e,g;if(c===b){e=this[0];if(!e)return null;g=cv(e);return g?"pageXOffset"in g?g[a?"pageYOffset":"pageXOffset"]:f.support.boxModel&&g.document.documentElement[d]||g.document.body[d]:e[d]}return this.each(function(){g=cv(this),g?g.scrollTo(a?f(g).scrollLeft():c,a?c:f(g).scrollTop()):this[d]=c})}}),f.each(["Height","Width"],function(a,c){var d=c.toLowerCase();f.fn["inner"+c]=function(){var a=this[0];return a&&a.style?parseFloat(f.css(a,d,"padding")):null},f.fn["outer"+c]=function(a){var b=this[0];return b&&b.style?parseFloat(f.css(b,d,a?"margin":"border")):null},f.fn[d]=function(a){var e=this[0];if(!e)return a==null?null:this;if(f.isFunction(a))return this.each(function(b){var c=f(this);c[d](a.call(this,b,c[d]()))});if(f.isWindow(e)){var g=e.document.documentElement["client"+c];return e.document.compatMode==="CSS1Compat"&&g||e.document.body["client"+c]||g}if(e.nodeType===9)return Math.max(e.documentElement["client"+c],e.body["scroll"+c],e.documentElement["scroll"+c],e.body["offset"+c],e.documentElement["offset"+c]);if(a===b){var h=f.css(e,d),i=parseFloat(h);return f.isNaN(i)?h:i}return this.css(d,typeof a=="string"?a:a+"px")}}),a.jQueryGlimpse=a.$Glimpse=f})(window);


    //#endregion

//#region JSON

//TODO remove parse method as jquery already has it 

var JSON; if (!JSON) { JSON = {}; } (function () { "use strict"; function f(n) { return n < 10 ? '0' + n : n; } if (typeof Date.prototype.toJSON !== 'function') { Date.prototype.toJSON = function (key) { return isFinite(this.valueOf()) ? this.getUTCFullYear() + '-' + f(this.getUTCMonth() + 1) + '-' + f(this.getUTCDate()) + 'T' + f(this.getUTCHours()) + ':' + f(this.getUTCMinutes()) + ':' + f(this.getUTCSeconds()) + 'Z' : null; }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) { return this.valueOf(); }; } var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { '\b': '\\b', '\t': '\\t', '\n': '\\n', '\f': '\\f', '\r': '\\r', '"': '\\"', '\\': '\\\\' }, rep; function quote(string) { escapable.lastIndex = 0; return escapable.test(string) ? '"' + string.replace(escapable, function (a) { var c = meta[a]; return typeof c === 'string' ? c : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4); }) + '"' : '"' + string + '"'; } function str(key, holder) { var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === 'object' && typeof value.toJSON === 'function') { value = value.toJSON(key); } if (typeof rep === 'function') { value = rep.call(holder, key, value); } switch (typeof value) { case 'string': return quote(value); case 'number': return isFinite(value) ? String(value) : 'null'; case 'boolean': case 'null': return String(value); case 'object': if (!value) { return 'null'; } gap += indent; partial = []; if (Object.prototype.toString.apply(value) === '[object Array]') { length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || 'null'; } v = partial.length === 0 ? '[]' : gap ? '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' : '[' + partial.join(',') + ']'; gap = mind; return v; } if (rep && typeof rep === 'object') { length = rep.length; for (i = 0; i < length; i += 1) { if (typeof rep[i] === 'string') { k = rep[i]; v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ': ' : ':') + v); } } } } else { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ': ' : ':') + v); } } } } v = partial.length === 0 ? '{}' : gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' : '{' + partial.join(',') + '}'; gap = mind; return v; } } if (typeof JSON.stringify !== 'function') { JSON.stringify = function (value, replacer, space) { var i; gap = ''; indent = ''; if (typeof space === 'number') { for (i = 0; i < space; i += 1) { indent += ' '; } } else if (typeof space === 'string') { indent = space; } rep = replacer; if (replacer && typeof replacer !== 'function' && (typeof replacer !== 'object' || typeof replacer.length !== 'number')) { throw new Error('JSON.stringify'); } return str('', { '': value }); }; } if (typeof JSON.parse !== 'function') { JSON.parse = function (text, reviver) { var j; function walk(holder, key) { var k, v, value = holder[key]; if (value && typeof value === 'object') { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v; } else { delete value[k]; } } } } return reviver.call(holder, key, value); } text = String(text); cx.lastIndex = 0; if (cx.test(text)) { text = text.replace(cx, function (a) { return '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4); }); } if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@').replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) { j = eval('(' + text + ')'); return typeof reviver === 'function' ? walk({ '': j }, '') : j; } throw new SyntaxError('JSON.parse'); }; } } ());

//#endregion

//#region google-code-prettify
 
if (!window.PR_SHOULD_USE_CONTINUATION) {
var q=null;window.PR_SHOULD_USE_CONTINUATION=!0;
(function(){function L(a){function m(a){var f=a.charCodeAt(0);if(f!==92)return f;var b=a.charAt(1);return(f=r[b])?f:"0"<=b&&b<="7"?parseInt(a.substring(1),8):b==="u"||b==="x"?parseInt(a.substring(2),16):a.charCodeAt(1)}function e(a){if(a<32)return(a<16?"\\x0":"\\x")+a.toString(16);a=String.fromCharCode(a);if(a==="\\"||a==="-"||a==="["||a==="]")a="\\"+a;return a}function h(a){for(var f=a.substring(1,a.length-1).match(/\\u[\dA-Fa-f]{4}|\\x[\dA-Fa-f]{2}|\\[0-3][0-7]{0,2}|\\[0-7]{1,2}|\\[\S\s]|[^\\]/g),a=
[],b=[],o=f[0]==="^",c=o?1:0,i=f.length;c<i;++c){var j=f[c];if(/\\[bdsw]/i.test(j))a.push(j);else{var j=m(j),d;c+2<i&&"-"===f[c+1]?(d=m(f[c+2]),c+=2):d=j;b.push([j,d]);d<65||j>122||(d<65||j>90||b.push([Math.max(65,j)|32,Math.min(d,90)|32]),d<97||j>122||b.push([Math.max(97,j)&-33,Math.min(d,122)&-33]))}}b.sort(function(a,f){return a[0]-f[0]||f[1]-a[1]});f=[];j=[NaN,NaN];for(c=0;c<b.length;++c)i=b[c],i[0]<=j[1]+1?j[1]=Math.max(j[1],i[1]):f.push(j=i);b=["["];o&&b.push("^");b.push.apply(b,a);for(c=0;c<
f.length;++c)i=f[c],b.push(e(i[0])),i[1]>i[0]&&(i[1]+1>i[0]&&b.push("-"),b.push(e(i[1])));b.push("]");return b.join("")}function y(a){for(var f=a.source.match(/\[(?:[^\\\]]|\\[\S\s])*]|\\u[\dA-Fa-f]{4}|\\x[\dA-Fa-f]{2}|\\\d+|\\[^\dux]|\(\?[!:=]|[()^]|[^()[\\^]+/g),b=f.length,d=[],c=0,i=0;c<b;++c){var j=f[c];j==="("?++i:"\\"===j.charAt(0)&&(j=+j.substring(1))&&j<=i&&(d[j]=-1)}for(c=1;c<d.length;++c)-1===d[c]&&(d[c]=++t);for(i=c=0;c<b;++c)j=f[c],j==="("?(++i,d[i]===void 0&&(f[c]="(?:")):"\\"===j.charAt(0)&&
(j=+j.substring(1))&&j<=i&&(f[c]="\\"+d[i]);for(i=c=0;c<b;++c)"^"===f[c]&&"^"!==f[c+1]&&(f[c]="");if(a.ignoreCase&&s)for(c=0;c<b;++c)j=f[c],a=j.charAt(0),j.length>=2&&a==="["?f[c]=h(j):a!=="\\"&&(f[c]=j.replace(/[A-Za-z]/g,function(a){a=a.charCodeAt(0);return"["+String.fromCharCode(a&-33,a|32)+"]"}));return f.join("")}for(var t=0,s=!1,l=!1,p=0,d=a.length;p<d;++p){var g=a[p];if(g.ignoreCase)l=!0;else if(/[a-z]/i.test(g.source.replace(/\\u[\da-f]{4}|\\x[\da-f]{2}|\\[^UXux]/gi,""))){s=!0;l=!1;break}}for(var r=
{b:8,t:9,n:10,v:11,f:12,r:13},n=[],p=0,d=a.length;p<d;++p){g=a[p];if(g.global||g.multiline)throw Error(""+g);n.push("(?:"+y(g)+")")}return RegExp(n.join("|"),l?"gi":"g")}function M(a){function m(a){switch(a.nodeType){case 1:if(e.test(a.className))break;for(var g=a.firstChild;g;g=g.nextSibling)m(g);g=a.nodeName;if("BR"===g||"LI"===g)h[s]="\n",t[s<<1]=y++,t[s++<<1|1]=a;break;case 3:case 4:g=a.nodeValue,g.length&&(g=p?g.replace(/\r\n?/g,"\n"):g.replace(/[\t\n\r ]+/g," "),h[s]=g,t[s<<1]=y,y+=g.length,
t[s++<<1|1]=a)}}var e=/(?:^|\s)nocode(?:\s|$)/,h=[],y=0,t=[],s=0,l;a.currentStyle?l=a.currentStyle.whiteSpace:window.getComputedStyle&&(l=document.defaultView.getComputedStyle(a,q).getPropertyValue("white-space"));var p=l&&"pre"===l.substring(0,3);m(a);return{a:h.join("").replace(/\n$/,""),c:t}}function B(a,m,e,h){m&&(a={a:m,d:a},e(a),h.push.apply(h,a.e))}function x(a,m){function e(a){for(var l=a.d,p=[l,"pln"],d=0,g=a.a.match(y)||[],r={},n=0,z=g.length;n<z;++n){var f=g[n],b=r[f],o=void 0,c;if(typeof b===
"string")c=!1;else{var i=h[f.charAt(0)];if(i)o=f.match(i[1]),b=i[0];else{for(c=0;c<t;++c)if(i=m[c],o=f.match(i[1])){b=i[0];break}o||(b="pln")}if((c=b.length>=5&&"lang-"===b.substring(0,5))&&!(o&&typeof o[1]==="string"))c=!1,b="src";c||(r[f]=b)}i=d;d+=f.length;if(c){c=o[1];var j=f.indexOf(c),k=j+c.length;o[2]&&(k=f.length-o[2].length,j=k-c.length);b=b.substring(5);B(l+i,f.substring(0,j),e,p);B(l+i+j,c,C(b,c),p);B(l+i+k,f.substring(k),e,p)}else p.push(l+i,b)}a.e=p}var h={},y;(function(){for(var e=a.concat(m),
l=[],p={},d=0,g=e.length;d<g;++d){var r=e[d],n=r[3];if(n)for(var k=n.length;--k>=0;)h[n.charAt(k)]=r;r=r[1];n=""+r;p.hasOwnProperty(n)||(l.push(r),p[n]=q)}l.push(/[\S\s]/);y=L(l)})();var t=m.length;return e}function u(a){var m=[],e=[];a.tripleQuotedStrings?m.push(["str",/^(?:'''(?:[^'\\]|\\[\S\s]|''?(?=[^']))*(?:'''|$)|"""(?:[^"\\]|\\[\S\s]|""?(?=[^"]))*(?:"""|$)|'(?:[^'\\]|\\[\S\s])*(?:'|$)|"(?:[^"\\]|\\[\S\s])*(?:"|$))/,q,"'\""]):a.multiLineStrings?m.push(["str",/^(?:'(?:[^'\\]|\\[\S\s])*(?:'|$)|"(?:[^"\\]|\\[\S\s])*(?:"|$)|`(?:[^\\`]|\\[\S\s])*(?:`|$))/,
q,"'\"`"]):m.push(["str",/^(?:'(?:[^\n\r'\\]|\\.)*(?:'|$)|"(?:[^\n\r"\\]|\\.)*(?:"|$))/,q,"\"'"]);a.verbatimStrings&&e.push(["str",/^@"(?:[^"]|"")*(?:"|$)/,q]);var h=a.hashComments;h&&(a.cStyleComments?(h>1?m.push(["com",/^#(?:##(?:[^#]|#(?!##))*(?:###|$)|.*)/,q,"#"]):m.push(["com",/^#(?:(?:define|elif|else|endif|error|ifdef|include|ifndef|line|pragma|undef|warning)\b|[^\n\r]*)/,q,"#"]),e.push(["str",/^<(?:(?:(?:\.\.\/)*|\/?)(?:[\w-]+(?:\/[\w-]+)+)?[\w-]+\.h|[a-z]\w*)>/,q])):m.push(["com",/^#[^\n\r]*/,
q,"#"]));a.cStyleComments&&(e.push(["com",/^\/\/[^\n\r]*/,q]),e.push(["com",/^\/\*[\S\s]*?(?:\*\/|$)/,q]));a.regexLiterals&&e.push(["lang-regex",/^(?:^^\.?|[!+-]|!=|!==|#|%|%=|&|&&|&&=|&=|\(|\*|\*=|\+=|,|-=|->|\/|\/=|:|::|;|<|<<|<<=|<=|=|==|===|>|>=|>>|>>=|>>>|>>>=|[?@[^]|\^=|\^\^|\^\^=|{|\||\|=|\|\||\|\|=|~|break|case|continue|delete|do|else|finally|instanceof|return|throw|try|typeof)\s*(\/(?=[^*/])(?:[^/[\\]|\\[\S\s]|\[(?:[^\\\]]|\\[\S\s])*(?:]|$))+\/)/]);(h=a.types)&&e.push(["typ",h]);a=(""+a.keywords).replace(/^ | $/g,
"");a.length&&e.push(["kwd",RegExp("^(?:"+a.replace(/[\s,]+/g,"|")+")\\b"),q]);m.push(["pln",/^\s+/,q," \r\n\t\xa0"]);e.push(["lit",/^@[$_a-z][\w$@]*/i,q],["typ",/^(?:[@_]?[A-Z]+[a-z][\w$@]*|\w+_t\b)/,q],["pln",/^[$_a-z][\w$@]*/i,q],["lit",/^(?:0x[\da-f]+|(?:\d(?:_\d+)*\d*(?:\.\d*)?|\.\d\+)(?:e[+-]?\d+)?)[a-z]*/i,q,"0123456789"],["pln",/^\\[\S\s]?/,q],["pun",/^.[^\s\w"-$'./@\\`]*/,q]);return x(m,e)}function D(a,m){function e(a){switch(a.nodeType){case 1:if(k.test(a.className))break;if("BR"===a.nodeName)h(a),
a.parentNode&&a.parentNode.removeChild(a);else for(a=a.firstChild;a;a=a.nextSibling)e(a);break;case 3:case 4:if(p){var b=a.nodeValue,d=b.match(t);if(d){var c=b.substring(0,d.index);a.nodeValue=c;(b=b.substring(d.index+d[0].length))&&a.parentNode.insertBefore(s.createTextNode(b),a.nextSibling);h(a);c||a.parentNode.removeChild(a)}}}}function h(a){function b(a,d){var e=d?a.cloneNode(!1):a,f=a.parentNode;if(f){var f=b(f,1),g=a.nextSibling;f.appendChild(e);for(var h=g;h;h=g)g=h.nextSibling,f.appendChild(h)}return e}
for(;!a.nextSibling;)if(a=a.parentNode,!a)return;for(var a=b(a.nextSibling,0),e;(e=a.parentNode)&&e.nodeType===1;)a=e;d.push(a)}var k=/(?:^|\s)nocode(?:\s|$)/,t=/\r\n?|\n/,s=a.ownerDocument,l;a.currentStyle?l=a.currentStyle.whiteSpace:window.getComputedStyle&&(l=s.defaultView.getComputedStyle(a,q).getPropertyValue("white-space"));var p=l&&"pre"===l.substring(0,3);for(l=s.createElement("LI");a.firstChild;)l.appendChild(a.firstChild);for(var d=[l],g=0;g<d.length;++g)e(d[g]);m===(m|0)&&d[0].setAttribute("value",
m);var r=s.createElement("OL");r.className="linenums";for(var n=Math.max(0,m-1|0)||0,g=0,z=d.length;g<z;++g)l=d[g],l.className="L"+(g+n)%10,l.firstChild||l.appendChild(s.createTextNode("\xa0")),r.appendChild(l);a.appendChild(r)}function k(a,m){for(var e=m.length;--e>=0;){var h=m[e];A.hasOwnProperty(h)?window.console&&console.warn("cannot override language handler %s",h):A[h]=a}}function C(a,m){if(!a||!A.hasOwnProperty(a))a=/^\s*</.test(m)?"default-markup":"default-code";return A[a]}function E(a){var m=
a.g;try{var e=M(a.h),h=e.a;a.a=h;a.c=e.c;a.d=0;C(m,h)(a);var k=/\bMSIE\b/.test(navigator.userAgent),m=/\n/g,t=a.a,s=t.length,e=0,l=a.c,p=l.length,h=0,d=a.e,g=d.length,a=0;d[g]=s;var r,n;for(n=r=0;n<g;)d[n]!==d[n+2]?(d[r++]=d[n++],d[r++]=d[n++]):n+=2;g=r;for(n=r=0;n<g;){for(var z=d[n],f=d[n+1],b=n+2;b+2<=g&&d[b+1]===f;)b+=2;d[r++]=z;d[r++]=f;n=b}for(d.length=r;h<p;){var o=l[h+2]||s,c=d[a+2]||s,b=Math.min(o,c),i=l[h+1],j;if(i.nodeType!==1&&(j=t.substring(e,b))){k&&(j=j.replace(m,"\r"));i.nodeValue=
j;var u=i.ownerDocument,v=u.createElement("SPAN");v.className=d[a+1];var x=i.parentNode;x.replaceChild(v,i);v.appendChild(i);e<o&&(l[h+1]=i=u.createTextNode(t.substring(b,o)),x.insertBefore(i,v.nextSibling))}e=b;e>=o&&(h+=2);e>=c&&(a+=2)}}catch(w){"console"in window&&console.log(w&&w.stack?w.stack:w)}}var v=["break,continue,do,else,for,if,return,while"],w=[[v,"auto,case,char,const,default,double,enum,extern,float,goto,int,long,register,short,signed,sizeof,static,struct,switch,typedef,union,unsigned,void,volatile"],
"catch,class,delete,false,import,new,operator,private,protected,public,this,throw,true,try,typeof"],F=[w,"alignof,align_union,asm,axiom,bool,concept,concept_map,const_cast,constexpr,decltype,dynamic_cast,explicit,export,friend,inline,late_check,mutable,namespace,nullptr,reinterpret_cast,static_assert,static_cast,template,typeid,typename,using,virtual,where"],G=[w,"abstract,boolean,byte,extends,final,finally,implements,import,instanceof,null,native,package,strictfp,super,synchronized,throws,transient"],
H=[G,"as,base,by,checked,decimal,delegate,descending,dynamic,event,fixed,foreach,from,group,implicit,in,interface,internal,into,is,lock,object,out,override,orderby,params,partial,readonly,ref,sbyte,sealed,stackalloc,string,select,uint,ulong,unchecked,unsafe,ushort,var"],w=[w,"debugger,eval,export,function,get,null,set,undefined,var,with,Infinity,NaN"],I=[v,"and,as,assert,class,def,del,elif,except,exec,finally,from,global,import,in,is,lambda,nonlocal,not,or,pass,print,raise,try,with,yield,False,True,None"],
J=[v,"alias,and,begin,case,class,def,defined,elsif,end,ensure,false,in,module,next,nil,not,or,redo,rescue,retry,self,super,then,true,undef,unless,until,when,yield,BEGIN,END"],v=[v,"case,done,elif,esac,eval,fi,function,in,local,set,then,until"],K=/^(DIR|FILE|vector|(de|priority_)?queue|list|stack|(const_)?iterator|(multi)?(set|map)|bitset|u?(int|float)\d*)/,N=/\S/,O=u({keywords:[F,H,w,"caller,delete,die,do,dump,elsif,eval,exit,foreach,for,goto,if,import,last,local,my,next,no,our,print,package,redo,require,sub,undef,unless,until,use,wantarray,while,BEGIN,END"+
I,J,v],hashComments:!0,cStyleComments:!0,multiLineStrings:!0,regexLiterals:!0}),A={};k(O,["default-code"]);k(x([],[["pln",/^[^<?]+/],["dec",/^<!\w[^>]*(?:>|$)/],["com",/^<\!--[\S\s]*?(?:--\>|$)/],["lang-",/^<\?([\S\s]+?)(?:\?>|$)/],["lang-",/^<%([\S\s]+?)(?:%>|$)/],["pun",/^(?:<[%?]|[%?]>)/],["lang-",/^<xmp\b[^>]*>([\S\s]+?)<\/xmp\b[^>]*>/i],["lang-js",/^<script\b[^>]*>([\S\s]*?)(<\/script\b[^>]*>)/i],["lang-css",/^<style\b[^>]*>([\S\s]*?)(<\/style\b[^>]*>)/i],["lang-in.tag",/^(<\/?[a-z][^<>]*>)/i]]),
["default-markup","htm","html","mxml","xhtml","xml","xsl"]);k(x([["pln",/^\s+/,q," \t\r\n"],["atv",/^(?:"[^"]*"?|'[^']*'?)/,q,"\"'"]],[["tag",/^^<\/?[a-z](?:[\w-.:]*\w)?|\/?>$/i],["atn",/^(?!style[\s=]|on)[a-z](?:[\w:-]*\w)?/i],["lang-uq.val",/^=\s*([^\s"'>]*(?:[^\s"'/>]|\/(?=\s)))/],["pun",/^[/<->]+/],["lang-js",/^on\w+\s*=\s*"([^"]+)"/i],["lang-js",/^on\w+\s*=\s*'([^']+)'/i],["lang-js",/^on\w+\s*=\s*([^\s"'>]+)/i],["lang-css",/^style\s*=\s*"([^"]+)"/i],["lang-css",/^style\s*=\s*'([^']+)'/i],["lang-css",
/^style\s*=\s*([^\s"'>]+)/i]]),["in.tag"]);k(x([],[["atv",/^[\S\s]+/]]),["uq.val"]);k(u({keywords:F,hashComments:!0,cStyleComments:!0,types:K}),["c","cc","cpp","cxx","cyc","m"]);k(u({keywords:"null,true,false"}),["json"]);k(u({keywords:H,hashComments:!0,cStyleComments:!0,verbatimStrings:!0,types:K}),["cs"]);k(u({keywords:G,cStyleComments:!0}),["java"]);k(u({keywords:v,hashComments:!0,multiLineStrings:!0}),["bsh","csh","sh"]);k(u({keywords:I,hashComments:!0,multiLineStrings:!0,tripleQuotedStrings:!0}),
["cv","py"]);k(u({keywords:"caller,delete,die,do,dump,elsif,eval,exit,foreach,for,goto,if,import,last,local,my,next,no,our,print,package,redo,require,sub,undef,unless,until,use,wantarray,while,BEGIN,END",hashComments:!0,multiLineStrings:!0,regexLiterals:!0}),["perl","pl","pm"]);k(u({keywords:J,hashComments:!0,multiLineStrings:!0,regexLiterals:!0}),["rb"]);k(u({keywords:w,cStyleComments:!0,regexLiterals:!0}),["js"]);k(u({keywords:"all,and,by,catch,class,else,extends,false,finally,for,if,in,is,isnt,loop,new,no,not,null,of,off,on,or,return,super,then,true,try,unless,until,when,while,yes",
hashComments:3,cStyleComments:!0,multilineStrings:!0,tripleQuotedStrings:!0,regexLiterals:!0}),["coffee"]);k(x([],[["str",/^[\S\s]+/]]),["regex"]);window.prettyPrintOne=function(a,m,e){var h=document.createElement("PRE");h.innerHTML=a;e&&D(h,e);E({g:m,i:e,h:h});return h.innerHTML};window.prettyPrint=function(a){function m(){for(var e=window.PR_SHOULD_USE_CONTINUATION?l.now()+250:Infinity;p<h.length&&l.now()<e;p++){var n=h[p],k=n.className;if(k.indexOf("prettyprint")>=0){var k=k.match(g),f,b;if(b=
!k){b=n;for(var o=void 0,c=b.firstChild;c;c=c.nextSibling)var i=c.nodeType,o=i===1?o?b:c:i===3?N.test(c.nodeValue)?b:o:o;b=(f=o===b?void 0:o)&&"CODE"===f.tagName}b&&(k=f.className.match(g));k&&(k=k[1]);b=!1;for(o=n.parentNode;o;o=o.parentNode)if((o.tagName==="pre"||o.tagName==="code"||o.tagName==="xmp")&&o.className&&o.className.indexOf("prettyprint")>=0){b=!0;break}b||((b=(b=n.className.match(/\blinenums\b(?::(\d+))?/))?b[1]&&b[1].length?+b[1]:!0:!1)&&D(n,b),d={g:k,h:n,i:b},E(d))}}p<h.length?setTimeout(m,
250):a&&a()}for(var e=[document.getElementsByTagName("pre"),document.getElementsByTagName("code"),document.getElementsByTagName("xmp")],h=[],k=0;k<e.length;++k)for(var t=0,s=e[k].length;t<s;++t)h.push(e[k][t]);var e=q,l=Date;l.now||(l={now:function(){return+new Date}});var p=0,d,g=/\blang(?:uage)?-([\w.]+)(?!\S)/;m()};window.PR={createSimpleLexer:x,registerLangHandler:k,sourceDecorator:u,PR_ATTRIB_NAME:"atn",PR_ATTRIB_VALUE:"atv",PR_COMMENT:"com",PR_DECLARATION:"dec",PR_KEYWORD:"kwd",PR_LITERAL:"lit",
PR_NOCODE:"nocode",PR_PLAIN:"pln",PR_PUNCTUATION:"pun",PR_SOURCE:"src",PR_STRING:"str",PR_TAG:"tag",PR_TYPE:"typ"}})();

var prettifyCss = '.glimpse .pln{color:#000}.glimpse .str{color:#080}.glimpse .kwd{color:#008}.glimpse .com{color:#800}.glimpse .typ{color:#606}.glimpse .lit{color:#066}.glimpse .pun,.glimpse .opn,.glimpse .clo{color:#660}.glimpse .tag{color:#008}.glimpse .atn{color:#606}.glimpse .atv{color:#080}.glimpse .dec,.glimpse .var{color:#606}.glimpse .fun{color:red}.glimpse .prettyprint span{font-family:Consolas, monospace, serif; font-size:1.1em;}.glimpse ol.linenums{margin-top:0;margin-bottom:0}.glimpse li.L0,.glimpse li.L1,.glimpse li.L2,.glimpse li.L3,.glimpse li.L5,.glimpse li.L6,.glimpse li.L7,.glimpse li.L8{list-style-type:none}.glimpse li.L1,.glimpse li.L3,.glimpse li.L5,.glimpse li.L7,.glimpse li.L9{background:#eee}'
$Glimpse('<style type="text/css"> ' + prettifyCss + ' </style>').appendTo("head");
 
}

PR.registerLangHandler(PR.createSimpleLexer([["pln",/^[\t\n\r \xa0]+/,null,"\t\n\r Â\xa0"],["str",/^(?:"(?:[^"\\]|\\.)*"|'(?:[^'\\]|\\.)*')/,null,"\"'"]],[["com",/^(?:--[^\n\r]*|\/\*[\S\s]*?(?:\*\/|$))/],["kwd",/^(?:add|all|alter|and|any|as|asc|authorization|backup|begin|between|break|browse|bulk|by|cascade|case|check|checkpoint|close|clustered|coalesce|collate|column|commit|compute|constraint|contains|containstable|continue|convert|create|cross|current|current_date|current_time|current_timestamp|current_user|cursor|database|dbcc|deallocate|declare|default|delete|deny|desc|disk|distinct|distributed|double|drop|dummy|dump|else|end|errlvl|escape|except|exec|execute|exists|exit|fetch|file|fillfactor|for|foreign|freetext|freetexttable|from|full|function|goto|grant|group|having|holdlock|identity|identitycol|identity_insert|if|in|index|inner|insert|intersect|into|is|join|key|kill|left|like|lineno|load|match|merge|national|nocheck|nonclustered|not|null|nullif|of|off|offsets|on|open|opendatasource|openquery|openrowset|openxml|option|or|order|outer|over|percent|plan|precision|primary|print|proc|procedure|public|raiserror|read|readtext|reconfigure|references|replication|restore|restrict|return|revoke|right|rollback|rowcount|rowguidcol|rule|save|schema|select|session_user|set|setuser|shutdown|some|statistics|system_user|table|textsize|then|to|top|tran|transaction|trigger|truncate|tsequal|union|unique|update|updatetext|use|user|using|values|varying|view|waitfor|when|where|while|with|writetext)(?=[^\w-]|$)/i,
null],["lit",/^[+-]?(?:0x[\da-f]+|(?:\.\d+|\d+(?:\.\d*)?)(?:e[+-]?\d+)?)/i],["pln",/^[_a-z][\w-]*/i],["pun",/^[^\w\t\n\r "'\xa0][^\w\t\n\r "'+\xa0-]*/]]),["sql"]);


//#endregion

var glimpse, glimpsePath;
if (window.jQueryGlimpse) { (function ($) {

    //#region Setup
    var glimpseCss = '.glimpse, .glimpse *, .glimpse a, .glimpse td, .glimpse th, .glimpse table { font-family: Helvetica, Arial, sans-serif; background-color:transparent; font-size:11px; line-height:14px; border:0px; color:#232323; text-align:left; } .glimpse table { min-width:0; } .glimpse a, .glimpse a:hover, .glimpse a:visited { color:#2200C1; text-decoration:underline; font-weight:normal; }.glimpse a:active { color:#c11; text-decoration:underline; font-weight:normal; }.glimpse th { font-weight:bold; }.glimpse-open { z-index: 100010; position:fixed; right:0; bottom:0; height:27px; width:28px; background: #cfcfcf; background: -moz-linear-gradient(top, #cfcfcf 0%, #dddddd 100%); background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#cfcfcf), color-stop(100%,#dddddd)); background: -webkit-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); background: -o-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); background: -ms-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#cfcfcf\', endColorstr=\'#dddddd\',GradientType=0 ); background: linear-gradient(top, #cfcfcf 0%,#dddddd 100%); -webkit-box-shadow: inset 0px 1px 0px 0px #E2E2E2; -moz-box-shadow: inset 0px 1px 0px 0px #E2E2E2; box-shadow: inset 0px 1px 0px 0px #E2E2E2; border-top: 1px solid #7A7A7A; border-left: 1px solid #7A7A7A; }.glimpse-icon { background:url() 0px -16px; height:20px; width:20px; margin: 3px 4px 0; cursor:pointer; }.glimpse-holder { display:none; z-index:100010 !important; height:0; position:fixed; bottom:0; left:0; width:100%; background-color:#fff; }.glimpse-bar { background: #cfcfcf; background: -moz-linear-gradient(top, #cfcfcf 0%, #dddddd 100%); background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#cfcfcf), color-stop(100%,#dddddd)); background: -webkit-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); background: -o-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); background: -ms-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#cfcfcf\', endColorstr=\'#dddddd\',GradientType=0 ); background: linear-gradient(top, #cfcfcf 0%,#dddddd 100%); -webkit-box-shadow: inset 0px 1px 0px 0px #E2E2E2; -moz-box-shadow: inset 0px 1px 0px 0px #E2E2E2; box-shadow: inset 0px 1px 0px 0px #E2E2E2; border-top: 1px solid #7A7A7A; height: 27px; }.glimpse-bar .glimpse-icon { margin-top:4px; float:left; } .glimpse-buttons { text-align:right; float:right; height:17px; width:150px; padding:6px; }.glimpse-title { margin:0 0 0 15px; padding-top:5px; font-weight:bold; display:inline-block; width:75%; overflow:hidden; }.glimpse-title .glimpse-snapshot-type { display:inline-block; height:20px; }.glimpse-title .glimpse-enviro { padding-left:10px; white-space:nowrap; height:20px; } .glimpse-title .glimpse-url .glimpse-drop { padding-left:10px; } .glimpse-title .glimpse-url .loading { margin:5px 0 0; font-weight:normal; display:none; } .glimpse-title .glimpse-url .glimpse-drop-over { padding-left:20px; padding-right:20px; text-align: center; } .glimpse .glimpse-drop { padding:1px 1px 1px 8px; height:14px; font-size:0.9em; } .glimpse .glimpse-drop, .glimpse .glimpse-drop-over { font-weight:normal; font-weight:normal; background: #f7f7f7; background: -moz-linear-gradient(top, #f7f7f7 0%, #e6e6e6 29%, #e2e2e2 31%, #c9c9c9 100%);background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#f7f7f7), color-stop(29%,#e6e6e6), color-stop(31%,#e2e2e2), color-stop(100%,#c9c9c9));background: -webkit-linear-gradient(top, #f7f7f7 0%,#e6e6e6 29%,#e2e2e2 31%,#c9c9c9 100%);background: -o-linear-gradient(top, #f7f7f7 0%,#e6e6e6 29%,#e2e2e2 31%,#c9c9c9 100%);background: -ms-linear-gradient(top, #f7f7f7 0%,#e6e6e6 29%,#e2e2e2 31%,#c9c9c9 100%);filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#f7f7f7\', endColorstr=\'#c9c9c9\',GradientType=0 );background: linear-gradient(top, #f7f7f7 0%,#e6e6e6 29%,#e2e2e2 31%,#c9c9c9 100%); -webkit-box-shadow: inset 0px 1px 0px 0px #F9F9F9; -moz-box-shadow: inset 0px 1px 0px 0px #F9F9F9; box-shadow: inset 0px 1px 0px 0px #F9F9F9; border: 1px solid #A7A7A7; -webkit-border-radius: 3px; -moz-border-radius: 3px; border-radius: 3px; margin: 0 5px 0 0; } .glimpse .glimpse-drop-over { position:absolute; display:none; top:4px; padding: 1px 10px 10px 10px; z-index:100; -webkit-box-shadow: 0px 0px 8px 0px #696969; -moz-box-shadow: 0px 0px 8px 0px #696969; box-shadow: 0px 0px 8px 0px #696969; } .glimpse .glimpse-drop-over div { text-align:center; font-weight:bold; margin:5px 0; }  .glimpse .glimpse-drop-arrow-holder { margin: 3px 3px 3px 5px; padding-left: 3px; border-left: 1px solid #A7A7A7; font-size: 9px; height: 9px; width: 10px; } .glimpse .glimpse-drop-arrow { background: url() no-repeat -22px -18px; width: 7px;  height: 4px; display: inline-block; } .glimpse-button, .glimpse-button:hover { cursor:pointer; background-image:url(); background-repeat:no-repeat; height:14px; width:14px; margin-left:2px; display:inline-block; }.glimpse-meta-warning { background-position:-168px -1px; display:none; }.glimpse-meta-warning:hover { background-position:-183px -1px; } .glimpse-meta-help { background-position:-138px -1px; margin-right:15px; }.glimpse-meta-help:hover { background-position:-153px -1px; margin-right:15px; }.glimpse-meta-update { background-position:-198px -1px; display:none; }.glimpse-meta-update:hover { background-position:-213px -1px; }.glimpse-close { background-position:-1px -1px; }.glimpse-close:hover { background-position:-17px -1px; }.glimpse-terminate { background-position:-65px -1px; }.glimpse-terminate:hover { background-position:-81px -1px; } .glimpse-popout { background-position:-96px -1px; }.glimpse-popout:hover { background-position: -111px -1px; } .glimpse-tabs { background: #afafaf; background: -moz-linear-gradient(top, #afafaf 0%, #cfcfcf 100%); background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#afafaf), color-stop(100%,#cfcfcf)); background: -webkit-linear-gradient(top, #afafaf 0%,#cfcfcf 100%); background: -o-linear-gradient(top, #afafaf 0%,#cfcfcf 100%); background: -ms-linear-gradient(top, #afafaf 0%,#cfcfcf 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#afafaf\', endColorstr=\'#cfcfcf\',GradientType=0 ); background: linear-gradient(top, #afafaf 0%,#cfcfcf 100%); border-bottom: 1px solid #A4A4A4; border-top: 1px solid #F9F9F9; -webkit-box-shadow: inset 0px 1px 0px 0px #8b8b8b;-moz-box-shadow: inset 0px 1px 0px 0px #8b8b8b;box-shadow: inset 0px 1px 0px 0px #8b8b8b; font-weight: bold; height: 24px; }.glimpse-tabs ul { margin:4px 0px 0 0; padding:0px; }.glimpse-tabs li { display:inline; margin:0 2px 3px 2px; height:22px; padding:4px 9px 3px; color:#565656; cursor:pointer; border-radius: 0px 0px 3px 3px; -moz-border-radius: 0px 0px 3px 3px; -webkit-border-bottom-right-radius: 3px; -webkit-border-bottom-left-radius: 3px; -webkit-transition: color 0.3s ease;-moz-transition: color 0.3s ease;-o-transition: color 0.3s ease;transition: color 0.3s ease; } .glimpse-tabs li.glimpse-hover { padding:4px 8px 3px; background: #dddddd;background: -moz-linear-gradient(top, #dddddd 0%, #ffffff 100%);background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#dddddd), color-stop(100%,#ffffff));background: -webkit-linear-gradient(top, #dddddd 0%,#ffffff 100%);background: -o-linear-gradient(top, #dddddd 0%,#ffffff 100%);background: -ms-linear-gradient(top, #dddddd 0%,#ffffff 100%);filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#dddddd\', endColorstr=\'#ffffff\',GradientType=0 );background: linear-gradient(top, #dddddd 0%,#ffffff 100%);-webkit-box-shadow: inset 1px -1px 0px #F9F9F9, inset -1px 0px 0px #F9F9F9;-moz-box-shadow: inset 1px -1px 0px #F9F9F9, inset -1px 0px 0px #F9F9F9;box-shadow: inset 1px -1px 0px #F9F9F9, inset -1px 0px 0px #F9F9F9; border-bottom: 1px solid #8B8B8B; border-left: 1px solid #8B8B8B; border-right: 1px solid #8B8B8B; border-top: 2px solid #DDD; }.glimpse-tabs li.glimpse-active { background: #dddddd;background: -moz-linear-gradient(top, #dddddd 0%, #efefef 100%);background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#dddddd), color-stop(100%,#efefef));background: -webkit-linear-gradient(top, #dddddd 0%,#efefef 100%);background: -o-linear-gradient(top, #dddddd 0%,#efefef 100%);background: -ms-linear-gradient(top, #dddddd 0%,#efefef 100%);filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#dddddd\', endColorstr=\'#efefef\',GradientType=0 );background: linear-gradient(top, #dddddd 0%,#efefef 100%);-webkit-box-shadow: inset 1px -1px 0px #F9F9F9, inset -1px 0px 0px #F9F9F9;-moz-box-shadow: inset 1px -1px 0px #F9F9F9, inset -1px 0px 0px #F9F9F9;box-shadow: inset 1px -1px 0px #F9F9F9, inset -1px 0px 0px #F9F9F9; border-bottom: 1px solid #8b8b8b; border-left: 1px solid #8b8b8b; border-right: 1px solid #8b8b8b; border-top: 2px solid #DDD; color: #000; padding: 4px 8px 3px; }.glimpse-tabs li.glimpse-disabled { color:#AAA; cursor:default; }.glimpse-panel-holder {}.glimpse-panel { display:none; overflow:auto; position:relative; } .glimpse-panel-message { text-align:center; padding-top:40px; font-size:1.1em; color:#AAA; }.glimpse-panel table { border-spacing:0; width:100%; }.glimpse-panel table td, .glimpse-panel table th { padding:3px 4px; text-align:left; vertical-align:top; } .glimpse-panel table td .glimpse-cell { vertical-align:top; } .glimpse-panel tbody .mono { font-family:Consolas, monospace, serif; font-size: 1.1em; } .glimpse-panel tr.glimpse-row-header-0 { height:19px; } .glimpse-panel .glimpse-row-header-0 th { background: #DFDFDF; background: -moz-linear-gradient(top, #f3f3f3 0%, #f3f3f3 5%, #e6e6e6 6%, #d1d1d1 100%); background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#f3f3f3), color-stop(5%,#f3f3f3), color-stop(6%,#e6e6e6), color-stop(100%,#d1d1d1)); background: -webkit-linear-gradient(top, #f3f3f3 0%,#f3f3f3 5%,#e6e6e6 6%,#d1d1d1 100%); background: -o-linear-gradient(top, #f3f3f3 0%,#f3f3f3 5%,#e6e6e6 6%,#d1d1d1 100%); background: -ms-linear-gradient(top, #f3f3f3 0%,#f3f3f3 5%,#e6e6e6 6%,#d1d1d1 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#f3f3f3\', endColorstr=\'#d1d1d1\',GradientType=0 ); background: linear-gradient(top, #f3f3f3 0%,#f3f3f3 5%,#e6e6e6 6%,#d1d1d1 100%); border-bottom: 1px solid #9C9C9C; font-weight: bold; } .glimpse-panel .glimpse-row-header-0 th { border-left:1px solid #D9D9D9; border-right:1px solid #9C9C9C; } .glimpse-panel .glimpse-soft { color:#999; } .glimpse-panel .glimpse-cell-key { font-weight:bold; } .glimpse-panel th.glimpse-cell-key { width:30%; max-width:150px; } .glimpse-panel table table { border:1px solid #D9D9D9; } .glimpse-panel table table thead th { background: #f3f3f3; background: -moz-linear-gradient(top, #f3f3f3 0%, #e6e6e6 100%); background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#f3f3f3), color-stop(100%,#e6e6e6)); background: -webkit-linear-gradient(top, #f3f3f3 0%,#e6e6e6 100%); background: -o-linear-gradient(top, #f3f3f3 0%,#e6e6e6 100%); background: -ms-linear-gradient(top, #f3f3f3 0%,#e6e6e6 100%); filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#f3f3f3\', endColorstr=\'#e6e6e6\',GradientType=0 ); background: linear-gradient(top, #f3f3f3 0%,#e6e6e6 100%); border-bottom: 1px solid #9C9C9C; } .glimpse-panel table table thead tr th { border-left:1px solid #C6C6C6; border-right:1px solid #D9D9D9; padding:1px 4px 2px 4px; }.glimpse-panel table table thead tr th:first-child { border-left:0px; }.glimpse-panel table table thead tr th:last-child { border-right:0px; }.glimpse-panel .even, .glimpse-panel .even > td, .glimpse-panel .even > th, .glimpse-panel .even > tr > td, .glimpse-panel .even > tr > th, .even > td > .glimpse-preview-table > tbody > tr > td, .even > tr > td > .glimpse-preview-table > tbody > tr > td { background-color:#F2F5F9; } .glimpse-panel .odd, .glimpse-panel .odd > td, .glimpse-panel .odd > th, .glimpse-panel .odd > tr > td, .glimpse-panel .odd > tr > th, .odd > td > .glimpse-preview-table > tbody > tr > td, .odd > tr > td > .glimpse-preview-table > tbody > tr > td { background-color:#FEFFFF; }.glimpse-panel table table tbody th { font-weight:normal; font-style:italic; }.glimpse-panel table table thead th { font-weight:bold; font-style:normal; }.glimpse-panel .glimpse-side-sub-panel { right:0; z-index:10; background-color:#FAFCFC; height:100%; width:25%; border-left:1px solid #ACA899; position:absolute; }.glimpse-panel .glimpse-side-main-panel { position:relative; height:100%; width:75%; float:left; } .glimpse-panel-holder .glimpse-active { display:block; }.glimpse-resizer { height:4px; cursor:n-resize; width:100%; position:absolute; top:-1px; }li.glimpse-permanent { font-style:italic; }.glimpse-preview-object { color:#006400; } .glimpse-preview-string, .glimpse-preview-object .glimpse-preview-string { color:#006400; font-weight:normal !important; } .glimpse-preview-string span { padding-left:1px; }.glimpse-preview-object span { font-weight:bold; color:#444; } .glimpse-preview-object span.start { margin-right:5px; } .glimpse-preview-object span.end { margin-left:5px; }.glimpse-preview-object span.rspace { margin-right:4px; }.glimpse-preview-object span.mspace { margin:0 4px; }.glimpse-preview-object span.small { font-size:0.95em; } .glimpse-panel .glimpse-preview-table { border:0; } .glimpse-panel .glimpse-preview-table .glimpse-preview-cell { padding-left:0; padding-right:2px; width:11px; } .glimpse-expand { height:11px; width:11px; display:inline-block; float:left; margin:1px 0 0 0; cursor:pointer; background-image:url(); background-repeat:no-repeat; background-position:-126px 0; }.glimpse-collapse { background-position:-126px -11px; }.glimpse-preview-show { display:none; font-weight:normal !important; }.glimpse-panel .quiet *, .glimpse-panel .ms * { color:#AAA; }.glimpse-panel .suppress { text-decoration:line-through; }.glimpse-panel .suppress * { color:#AAA; } .glimpse-panel .selected, .glimpse-panel .selected > td, .glimpse-panel .selected > th, .glimpse-panel .selected > tr > td, .glimpse-panel .selected > tr > th, .selected > td > .glimpse-preview-table > tbody > tr > td, .selected > tr > td > .glimpse-preview-table > tbody > tr > td { background-color:#FFFF99; }.glimpse-panel .selected * { color:#409B3B; }.glimpse .info .icon, .glimpse .warn .icon, .glimpse .loading .icon, .glimpse .error .icon, .glimpse .fail .icon, .glimpse .ms .icon { width:14px; height:14px; background-image:url(); background-repeat:no-repeat; display:inline-block; margin-right: 5px; } .glimpse .info .icon { background-position: -22px -22px; }.glimpse .warn .icon { background-position:-36px -22px; }.glimpse .loading .icon { background-position:-78px -22px; }.glimpse .error .icon { background-position:-50px -22px; }.glimpse .ms .icon { background-position:-181px -22px; } .glimpse .fail .icon { background-position:-64px -22px; }.glimpse .info * { color:#067CE5; }.glimpse .warn * { color:#FE850C; } .glimpse .error * { color:#B40000; }.glimpse .fail * { color:#B40000; font-weight:bold; }.glimpse-panelitem-Ajax .loading .icon { float:right; }.glimpse-panelitem-Remote .glimpse-side-sub-panel .loading, .glimpse-panelitem-Remote .glimpse-side-main-panel .loading, .glimpse-clear { position:fixed; bottom:5px; right:10px; color:#777; } .glimpse-panelitem-Remote .glimpse-side-main-panel .loading { right:27%; } .glimpse-clear { background-color:white; padding:0.3em 1em 0.5em 1em; border:#CCC solid 1px; bottom:25px; -webkit-border-radius:3px; -moz-border-radius:3px; border-radius:3px; } .glimpse-panel table .glimpse-head-message td { text-align:center; background-color:#DDD; } .glimpse-panelitem-GlimpseInformation div { text-align:center; } .glimpse-panelitem-GlimpseInformation .glimpse-panel-message { padding-top:5px; } .glimpse-panelitem-GlimpseInformation strong { font-weight:bold; } .glimpse-panelitem-GlimpseInformation .glimpse-info-more { font-size:1.5em; margin:1em 0; } .glimpse-panelitem-GlimpseInformation .glimpse-info-quote { font-style:italic; margin:0.75em 0 3em; } @media screen and (-webkit-min-device-pixel-ratio:0) { .glimpse-tabs li.glimpse-hover, .glimpse-tabs li.glimpse-active { border-top:1px solid #DDD; } }';
    glimpseCss += '.glimpse-panel .glimpse-tl-resizer { position:absolute; width:4px; height:100%; cursor:col-resize; } .glimpse-panel .glimpse-tl-row-summary { position:relative; height:100px; } .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-resizer-bar { background-color:#404040; width:1px; height:100%; margin-left:2px; } .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-resizer-handle { background-color:#404040; width:5px; height:20px; top:0; position:absolute; -webkit-border-radius:2px; -moz-border-radius:2px; border-radius:2px; } .glimpse-panel .glimpse-tl-row-spacer { background:#cfcfcf; background:-moz-linear-gradient(top, #cfcfcf 0%, #dddddd 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#cfcfcf), color-stop(100%,#dddddd)); background:-webkit-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); background:-o-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); background:-ms-linear-gradient(top, #cfcfcf 0%,#dddddd 100%); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#cfcfcf\', endColorstr=\'#dddddd\',GradientType=0 ); background:linear-gradient(top, #cfcfcf 0%,#dddddd 100%); -webkit-box-shadow:inset 0px 1px 0px 0px #E2E2E2; -moz-box-shadow:inset 0px 1px 0px 0px #E2E2E2; box-shadow:inset 0px 1px 0px 0px #E2E2E2; border-top:1px solid #7A7A7A; height:7px; } .glimpse-panel .glimpse-tl-col-side { position:absolute;width:200px; height:100%; left:0px; } .glimpse-panel .glimpse-tl-col-main { position:absolute; left:200px; right:0px; top:0px; } .glimpse-panel .glimpse-tl-row-content .glimpse-tl-col-side { border-right:1px solid #404040; } .glimpse-panel .glimpse-tl-row-content { position:relative; height:400px; } .glimpse-panel .glimpse-tl-row-content .glimpse-tl-resizer { left:200px; } .glimpse-panel .glimpse-tl-row-content .glimpse-tl-resizer div { background-color:#404040; width:1px; height:100%; } .glimpse-panel .glimpse-tl-band{ padding-top:2px; padding-bottom:2px; } .glimpse-panel .glimpse-tl-col-side .glimpse-tl-band { padding-top:2px; -webkit-box-sizing:border-box; -moz-box-sizing:border-box; box-sizing:border-box; padding-left:5px; white-space:nowrap; text-overflow:ellipsis; overflow:hidden; } .glimpse-panel .glimpse-tl-band { position:relative; height:18px; padding:0px; } .glimpse-panel .glimpse-tl-col-main .glimpse-tl-event { position:absolute; top:4px; margin-left:-2px; } .glimpse-panel .glimpse-tl-band-title { height:20px !important; -moz-box-sizing:border-box; -webkit-box-sizing:border-box; box-sizing:border-box; font-weight:bold; padding-top:4px; } .glimpse-panel .glimpse-tl-event { border-radius:4px; width:7px; height:7px; display:inline-block; margin:0 5px 0 2px; background:-moz-linear-gradient(top, rgba(255,255,255,0.7) 0%, rgba(255,255,255,0) 40%, rgba(255,255,255,0) 70%, rgba(0,0,0,0.5) 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(255,255,255,0.7)), color-stop(40%,rgba(255,255,255,0)), color-stop(70%,rgba(255,255,255,0)), color-stop(100%,rgba(0,0,0,0.5))); background:-webkit-linear-gradient(top, rgba(255,255,255,0.7) 0%,rgba(255,255,255,0) 40%,rgba(255,255,255,0) 70%,rgba(0,0,0,0.5) 100%); background:-o-linear-gradient(top, rgba(255,255,255,0.7) 0%,rgba(255,255,255,0) 40%,rgba(255,255,255,0) 70%,rgba(0,0,0,0.5) 100%); background:-ms-linear-gradient(top, rgba(255,255,255,0.7) 0%,rgba(255,255,255,0) 40%,rgba(255,255,255,0) 70%,rgba(0,0,0,0.5) 100%); background:linear-gradient(top, rgba(255,255,255,0.7) 0%,rgba(255,255,255,0) 40%,rgba(255,255,255,0) 70%,rgba(0,0,0,0.5) 100%); } .glimpse-panel .glimpse-tl-col-main .glimpse-tl-event { width:1%; min-width:3px; } .glimpse-panel .glimpse-tl-event-info { position:absolute; top:1px; padding:0.75em; border:1px solid rgba(0, 0, 0, 0.3); background-color:#FCF7BD; display:none; -webkit-border-radius:15px; -moz-border-radius:15px; border-radius:15px; -webkit-box-shadow:0px 0px 8px 0px #696969; -moz-box-shadow:0px 0px 8px 0px #696969; box-shadow:0px 0px 8px 0px #696969; } .glimpse-panel .glimpse-tl-event-info th { font-weight:bold; text-align:right; } .glimpse-panel .glimpse-tl-event-info .glimpse-tl-event-info-title { text-align:left; border-bottom:1px solid rgba(0, 0, 0, 0.3); } .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-event-holder { margin-left:3px; } .glimpse-panel .glimpse-tl-row-content .glimpse-tl-event-holder { margin-left:15px; } .glimpse-panel .glimpse-tl-event-holder-inner { position:absolute; left:0px; right:0px; margin-left:4px; } .glimpse-panel .glimpse-tl-event-desc-sub { color:#AAA; font-size:0.9em; margin-left:5px; } .glimpse-panel .glimpse-tl-event-overlay { display:none; position:absolute; height:18px; width:7px; } .glimpse-panel .glimpse-tl-event-overlay-lh { position:absolute; left:0; width:1px; } .glimpse-panel .glimpse-tl-event-overlay-li { position:absolute; right:-2px; font-size:0.8em; top:7px; background:url() no-repeat -31px -17px; width:11px; height:3px; } .glimpse-panel .glimpse-tl-event-overlay-lt { position:absolute; right:15px; font-size:0.8em; top:2px; color:rgba(0, 0, 0, 0.75); } .glimpse-panel .glimpse-tl-event-overlay-rh { position:absolute; right:0; width:1px; } .glimpse-panel .glimpse-tl-event-overlay-ri { position:absolute; left:-4px; font-size:0.8em; top:7px; background:url() no-repeat -44px -17px; width:11px; height:3px; } .glimpse-panel .glimpse-tl-event-overlay-rt { font-size:0.8em; position:absolute; top:2px; left:11px; color:rgba(0, 0, 0, 0.75); } .glimpse-panel .glimpse-tl-event-overlay-c { font-size:0.9em; text-align:center; padding-top:1px; color:rgba(0, 0, 0, 0.75); font-weight:bold; } .glimpse-panel .glimpse-tl-content-scroll { overflow-y:scroll; overflow-x:hidden; width:100%; position:absolute; height:100%; } .glimpse-panel .glimpse-tl-padding-holder { right:18px; } .glimpse-panel .glimpse-tl-padding { position:absolute; width:0; top:0; bottom:0; background-color:rgba(0, 0, 0, 0.3); } .glimpse-panel .glimpse-tl-padding-l { left:0; border-left:1px solid #555; } .glimpse-panel .glimpse-tl-padding-r { right:0; border-right:1px solid #555; } .glimpse-panel .glimpse-tl-divider-line-holder { position:absolute; height:100%; top:0; right:0; } .glimpse-panel .glimpse-tl-divider { position:absolute; width:1px; top:0; bottom:0; background-color:rgba(0, 0, 0, 0.1); } .glimpse-panel .glimpse-tl-divider div { position:absolute; top:4px; right:5px; font-size:9px; color:#323232; white-space:nowrap; } .glimpse-panel .glimpse-tl-divider-title-bar { width:100%; background-color:rgba(255, 255, 255, 0.8); border-bottom:1px solid rgba(0, 0, 0, 0.3); height:20px; } .glimpse-panel .glimpse-tl-divider-zero-holder { position:absolute; height:100%; top:0; right:0; left:0; } .glimpse-panel .glimpse-tl-divider-zero-holder .glimpse-tl-divider { left:15px } .glimpse-panel .glimpse-tl-content-scroll .glimpse-tl-divider div { display:none; }  .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-divider-holder { right:19px; height:20px; } .glimpse-panel .glimpse-tl-row-content .glimpse-tl-content-scroll .glimpse-tl-divider-holder { right:-1px; margin-left:1px; height:100%; } .glimpse-panel .glimpse-tl-row-content .glimpse-tl-content-overlay .glimpse-tl-divider-holder { right:16px; height:20px; border-left:1px solid #404040; } .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-divider-line-holder { left:0; } .glimpse-panel .glimpse-tl-row-content .glimpse-tl-divider-line-holder { left:15px; } .glimpse-panel .glimpse-tl-resizer-holder { right:17px; } .glimpse-panel .glimpse-tl-resizer-l { left:0px; margin-left:-2px; } .glimpse-panel .glimpse-tl-resizer-r { right:0px; } .glimpse-panel .glimpse-tl-col-side { background-color:#F2F5F7; } .glimpse-panel .glimpse-tl-col-side .odd, .glimpse-panel .glimpse-tl-col-side .odd > td { background-color:#F2F5F7; } .glimpse-panel .glimpse-tl-col-side .even, .glimpse-panel .glimpse-tl-col-side .even > td { background-color:#E1E7F0; } .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-band-title { opacity:0.9; } .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-event-desc-group .glimpse-tl-band { opacity:0.95; }  .glimpse-panel .glimpse-tl-row-summary .glimpse-tl-event-desc-group input { margin-right:5px; float:right; display:none; }';
    glimpseCss = glimpseCss.replace(/url\(\)/gi, 'url(' + glimpsePath + 'sprite.png)'); 
    $('<style type="text/css"> ' + glimpseCss + ' </style>').appendTo("head");      //http://stackoverflow.com/questions/1212500/jquery-create-css-rule-class-runtime

    $.glimpse = {};
    $.glimpseProcessor = {};
    $.glimpseContent = {};
    $.glimpseResize = {};

    //#endregion

    //#region $.fn

    //TODO: Shift to $.glimpse.util instead of $.fn

    $.extend($.fn, { 
        resizer : function (settings) {
            return this.each(function() {
                $.glimpseResize.init($(this), settings);
            });
        },
        sortElements: (function () {
            var sort = [].sort;
            return function (comparator, getSortable) {
                getSortable = getSortable || function () { return this; };
                comparator = comparator || function (a, b) { return $(a).data('sort') > $(b).data('sort') ? 1 : -1; };
                var placements = this.map(function () {
                    var sortElement = getSortable.call(this), parentNode = sortElement.parentNode, nextSibling = parentNode.insertBefore(document.createTextNode(''), sortElement.nextSibling);
                    return function () {
                        if (parentNode === this) {
                            throw new Error("You can't sort elements if any one is a descendant of another.");
                        }
                        parentNode.insertBefore(this, nextSibling);
                        parentNode.removeChild(nextSibling);
                    };
                });
                return sort.call(this, comparator).each(function (i) {
                    placements[i].call(getSortable.call(this));
                });
            };
        }
        )()
    });
    //#endregion

    //#region $.glimpseResize

    //TODO: Shift to $.glimpse.resize instead of $.glimpseResizer

    $.extend($.glimpseResize, {
        defaults : {
            staticOffset : null, 
            lastMousePos : 0,
            min : function() { return 50; },  
            max : undefined,
            preDragCallback : function(postion) { }, 
            endDragCallback : function(postion) { },
            isUpDown : false, 
            valueStyle : 'left', 
            offset : 1,
            getValue : function(s) { return parseInt(s.resizeScope.css(s.valueStyle)); },
            setValue : function(s, value) { return s.resizeScope.css(s.valueStyle, value + 'px'); }
        }, 
        init : function (scope, s) { 
            var gr = $.glimpseResize,  
                settings = $.extend(true, { opacityScope : scope, anchorScope : scope, resizeScope : scope }, gr.defaults, s);   
                 
            settings.mousePosition = s.isUpDown ? function(e) { return e.clientY + document.documentElement.scrollTop; } : function(e) { return e.clientX + document.documentElement.scrollLeft; };

            scope.bind("mousedown", { settings : settings }, gr.startDrag); 
        },
        startDrag : function (e) {
            var gr = $.glimpseResize, 
                s = e.data.settings;  

            gr.min = s.min();
            gr.max = s.max != undefined ? s.max() : undefined;
             
            s.preDragCallback();

            s.lastMousePos = s.mousePosition(e);
            s.staticOffset = s.getValue(s) + (s.lastMousePos * -1 * s.offset);  

            s.opacityScope.css('opacity', 0.50);

            $(document).bind('mousemove', { settings : s }, gr.performDrag).bind('mouseup', { settings : s }, gr.endDrag);

            return false;
        },
        performDrag : function (e) {
            var gr = $.glimpseResize, 
                settings = e.data.settings, 
                mousePos = settings.mousePosition(e), 
                offsetMousePos = gr.calculateOffset(mousePos, settings); 
                 
            settings.setValue(settings, offsetMousePos);  
            settings.lastMousePos = mousePos;

            if (offsetMousePos < gr.min || (gr.max && offsetMousePos > gr.max)) { gr.endDrag(e); }

            return false;
        },
        endDrag : function (e) {
            var gr = $.glimpseResize, 
                settings = e.data.settings;

            $(document).unbind('mousemove', gr.performDrag).unbind('mouseup', gr.endDrag);

            settings.opacityScope.css('opacity', 1);  

            settings.endDragCallback(settings.getValue(settings));
        },
        calculateOffset : function (mousePos, settings) {
            var gr = $.glimpseResize, 
                offsetMousePos = settings.staticOffset + (mousePos * 1 * settings.offset); 

            if (settings.lastMousePos >= mousePos) { offsetMousePos += 4; } 
            if (gr.min != undefined) { offsetMousePos = Math.max(gr.min, offsetMousePos); }
            if (gr.max != undefined) { offsetMousePos = Math.min(gr.max, offsetMousePos); } 

            return offsetMousePos;
        }
    });
    
    /*
    $.extend($.glimpseResize, {
        static: {
            anchor: null,
            staticOffset: null,
            lastMousePos: 0,
            min: 50,
            endDragCallback: function (height) { }
        },
        startDrag: function (e) {
            var gr = $.glimpseResize, o = gr.static;
            o.anchor = $(e.data.el);
            o.lastMousePos = gr.mousePosition(e).y;
            o.staticOffset = o.anchor.height() + o.lastMousePos;
            o.anchor.css('opacity', 0.50);
            $(document).mousemove(gr.performDrag).mouseup(gr.endDrag);
            return false;
        },
        performDrag: function (e) {
            var gr = $.glimpseResize, o = gr.static;
            var mousePos = gr.mousePosition(e).y;
            var offsetMousePos = o.staticOffset - mousePos;
            if (o.lastMousePos >= mousePos) {
                offsetMousePos += 4;
            }
            offsetMousePos = Math.max(o.min, offsetMousePos);
            o.anchor.height(offsetMousePos + 'px');
            o.lastMousePos = mousePos;
            if (offsetMousePos < o.min) {
                gr.endDrag(e);
            }
            return false;
        },
        endDrag: function (e) {
            var gr = $.glimpseResize, o = gr.static;
            $(document).unbind('mousemove', gr.performDrag).unbind('mouseup', gr.endDrag);
            o.anchor.css('opacity', 1);
            o.anchor = null;
            o.staticOffset = null;
            o.lastMousePos = 0;
            o.endDragCallback();
        },
        mousePosition: function (e) {
            var d = document.documentElement;
            return { x: e.clientX + d.scrollLeft, y: e.clientY + d.scrollTop };
        }
    });
    */

    //#endregion

    //#region $.glimpseProcessor

    //TODO: Shift to $.glimpse.processor instead of $.glimpseProcessor

    $.extend($.glimpseProcessor, {
        layout: function (g, title) {
            var that = this, static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder(), data, metadata;
            
            //Build Dynamic HTML
            for (var key in static.data) {
                if ($('.glimpse-tabitem-' + key, tabStrip).length == 0) {
                    data = static.data[key];
                    metadata = ((metadata = static.data._metadata) && (metadata = metadata.plugins[key]) && (metadata = metadata.structure));

                    that.addTab(tabStrip, data, key);
                    that.addTabBody(panelHolder, that.build(data, 0, true, metadata, 1), key);
                }
            }
             
            //Sort Elemetns
            $('li', tabStrip).sortElements();
            $('.glimpse-panel', panelHolder).sortElements();

            //Adjust render
            that.applyPostRenderTransforms(panelHolder);

            //Select tab
            that.restoreTab(g);

            $('.glimpse-title').html(title);
        },
        applyPostRenderTransforms: function (scope) {
            //Alert state
            $('.info, .warn, .error, .fail, .loading, .ms', scope).find('> td:first-child, > tr:first-child > td:first-child:not(:has(div.glimpse-cell)), > tr:first-child > td:first-child > div.glimpse-cell:first-child').not(':has(.icon)').prepend('<div class="icon"></div>');
               
            //Code formatting
            var codeProcess = function(items) {
                $.each(items, function() {
                    var item = $(this).addClass('prettyprint'), codeType = item.hasClass('glimpse-code') ? item.attr('data-codeType') : item.closest('.glimpse-code').attr('data-codeType');  
                    item.html(prettyPrintOne(item.html(), codeType));
                });
            };
            codeProcess($('.glimpse-code:not(:has(table)), .glimpse-code > table:not(:has(thead)) .glimpse-preview-show', scope));
            
            //Open state
            setTimeout(function () {
                $('.glimpse-start-open > td > .glimpse-expand:first-child', scope).click();
            }, 500);
        },
        addTab: function (container, data, key) {
            var disabled = (data === undefined || data === null) ? ' glimpse-disabled' : '';
            container.append('<li class="glimpse-tabitem-' + key + disabled + '" data-sort="' + key + '">' + key + '</li>');
        },
        addTabBody: function (container, content, key) {
            container.append('<div class="glimpse-panel glimpse-panelitem-' + key + '" data-sort="' + key + '">' + content + '</div>');
        },
        restoreTab: function (g) {
            var static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder(), activeTab = g.settings.activeTab;

            $('.glimpse-active', tabStrip).removeClass('glimpse-active').removeClass('glimpse-hover');
            $((activeTab ? '.glimpse-tabitem-' + activeTab : 'li:first'), tabStrip).addClass('glimpse-active');

            $('.glimpse-active', panelHolder).removeClass('glimpse-active');
            $((activeTab ? '.glimpse-panelitem-' + activeTab : '.glimpse-panel:first'), panelHolder).addClass('glimpse-active');
             
            $('.glimpse').trigger('glimpse.tabchanged', tabStrip.find('.glimpse-active').attr('data-sort'));
        },
        clearLayout: function (g) {
            var that = this, static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder();

            that.removeTabs(tabStrip);
            that.removeTabBodies(panelHolder);
        },
        removeTabs: function (container) {
            $('li:not(.glimpse-permanent)', container).remove();
        },
        removeTabBodies: function (container) {
            $('.glimpse-panel:not(.glimpse-permanent)', container).remove();
        },
        build: function (data, level, forceFull, metadata, forceLimit) {
            var that = this, result = '';

            if ($.isArray(data)) {
                if (metadata)
                    result = that.buildStructuredTable(data, level, forceFull, metadata, forceLimit);
                else
                    result = that.buildCustomTable(data, level, forceFull, forceLimit);
            }
            else if ($.isPlainObject(data))
                result = that.buildKeyValueTable(data, level, forceFull, forceLimit);
            else if (level == 0) {
                if (data === undefined || data === null || data === '')
                    result = '';
                else {
                    var attr = '';
                    if (data.indexOf('http://') == 0) {
                        attr = ' data-glimpse-lazy-url="' + data + '"';
                        data = 'Loading data, please wait...';
                    } 
                    result = '<div class="glimpse-panel-message"' + attr + '>' + data + '</div>';
                }
            }
            else
                result = that.buildString(data, level, forceLimit);

            return result;
        },
        buildHeading: function (url, clientName, type) {
            var clean = function(data) {
                return (data === undefined || data === null || data === "null") ? '' : data;
            };
            type = clean(type);
            clientName = clean(clientName);
            return '<span class="glimpse-snapshot-type" data-clientName="' + clientName + '">' + clientName + ((type.length > 0) ? ' (' + type + ')' : '') + '&nbsp;</span><span><span class="glimpse-enviro"></span><span class="glimpse-url">' + url + '</span></span>';
        },
        buildKeyValueTable: function (data, level, forceFull, forceLimit) {  
            var that = this, limit = $.isNaN(forceLimit) ? 3 : forceLimit;
            if (that.shouldUsePreview($.glimpse.util.lengthJson(data), level, forceFull, limit, forceLimit, 1))
                return that.buildKeyValuePreview(data, level);
                
            var i = 1, html = '<table><thead><tr class="glimpse-row-header-' + level + '"><th class="glimpse-cell-key">Key</th><th class="glimpse-cell-value">Value</th></tr></thead>';
            for (var key in data)
                html += '<tr class="' + (i++ % 2 ? 'odd' : 'even') + '"><th width="30%">' + $.glimpseContent.formatString(key) + '</th><td width="70%"> ' + that.build(data[key], level + 1) + '</td></tr>';
            html += '</table>';
            return html;
        },
        buildCustomTable: function (data, level, forceFull, forceLimit) { 
            var that = this, limit = $.isNaN(forceLimit) ? 3 : forceLimit;
            if (that.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return that.buildCustomPreview(data, level);

            var html = '<table><thead><tr class="glimpse-row-header-' + level + '">';
            if ($.isArray(data[0])) {
                for (var x = 0; x < data[0].length; x++)
                    html += '<th>' + $.glimpseContent.formatString(data[0][x]) + '</th>';
                html += '</tr></thead>';
                for (var i = 1; i < data.length; i++) {
                    html += '<tr class="' + (i % 2 ? 'odd' : 'even') + (data[i].length > data[0].length ? ' ' + data[i][data[i].length - 1] : '') + '">';
                    for (var x = 0; x < data[0].length; x++)
                        html += '<td>' + that.build(data[i][x], level + 1) + '</td>';
                    html += '</tr>';
                }
                html += '</table>';
            }
            else {
                if (data.length > 1) {
                    html += '<th>Values</th></tr></thead>';
                    for (var i = 0; i < data.length; i++)
                        html += '<tr class="' + (i % 2 ? 'odd' : 'even') + '"><td>' + that.build(data[i], level + 1) + '</td></tr>';
                    html += '</table>';
                }
                else
                    html = that.build(data[0], level + 1);
            }
            return html;
        },
        buildStructuredTable: function (data, level, forceFull, metadata, forceLimit) { 
            var that = this, limit = $.isNaN(forceLimit) ? 3 : forceLimit;
            if (that.shouldUsePreview(data.length, level, forceFull, limit, forceLimit, 1))
                return that.buildStructuredTablePreview(data, level, metadata);
            
            var html = '<table>', rowClass = '';
            for (var i = 0; i < data.length; i++) {
                rowClass = data[i].length > data[0].length ? (' ' + data[i][data[i].length - 1]) : '';
                html += (i == 0) ? '<thead class="glimpse-row-header-' + level + '">' : '<tbody class="' + (i % 2 ? 'odd' : 'even') + rowClass + '">';
                for (var x = 0; x < metadata.length; x++) { 
                    var rowData = '';
                     
                    for (var y = 0; y < metadata[x].length; y++) {
                        var metadataItem = metadata[x][y], cellType = (i == 0 ? 'th' : 'td'); 
                        rowData += that.buildStructuredTableCell(data[i], metadataItem, level, cellType, i);
                    }
                     
                    if (rowData != '') { html += '<tr>' + rowData + '</tr>'; };
                }
                html += (i == 0) ? '</thead>' : '</tbody>';
            }
            html += '</table>'; 

            return html;
        },
        buildStructuredTableCell : function(data, metadataItem, level, cellType, rowIndex) {
            var that = this, html = '', cellContent = '', cellClass = '', cellStyle = '', cellAttr = '';
                
            //Cell Content
            if ($.isArray(metadataItem.data)) {
                for (var i = 0; i < metadataItem.data.length; i++) 
                    cellContent += that.buildStructuredTableCell(data, metadataItem.data[i], level, 'div', rowIndex);
            }
            else { 
                if (!metadataItem.indexs && $.isNaN(metadataItem.data)) 
                    metadataItem.indexs = $.glimpse.util.getTokens(metadataItem.data, data); 
                
                //Get metadata for the new data 
                var newMetadataItem = metadataItem.structure;
                if ($.isPlainObject(newMetadataItem)) 
                    newMetadataItem = newMetadataItem[rowIndex];
                    
                cellContent = metadataItem.indexs ? that.buildFormatString(metadataItem.data, data, metadataItem.indexs) : data[metadataItem.data];
                
                //If minDisplay and we are in header or there is no data, we don't want to render anything 
                if (metadataItem.minDisplay && (rowIndex == 0 || cellContent == undefined || cellContent == null))
                    return ""; 
                     
                cellContent = that.build(cellContent, level + 1, metadataItem.forceFull, newMetadataItem, rowIndex == 0 ? undefined : metadataItem.limit);

                //Content pre/post
                if (rowIndex != 0) {
                    if (metadataItem.pre) { cellContent = '<span class="glimpse-soft">' + metadataItem.pre + '</span>' + cellContent; }
                    if (metadataItem.post) { cellContent = cellContent + '<span class="glimpse-soft">' + metadataItem.post + '</span>'; }
                }
            }
            
            if (rowIndex != 0) {
                cellClass = 'glimpse-cell';
                //Cell Class
                if (metadataItem.key === true) { cellClass += ' glimpse-cell-key'; }
                if (metadataItem.isCode === true) { cellClass += ' glimpse-code'; }
                if (metadataItem.className) { cellClass += ' ' + metadataItem.className; }
                //Cell Code 
                if (metadataItem.codeType) { cellAttr += ' data-codeType="' + metadataItem.codeType + '"'; };
            }
            if (cellClass) { cellAttr += ' class="' + cellClass + '"'; }; 
            //Cell Style  
            if (metadataItem.width) { cellStyle += 'width:' + metadataItem.width + ';'; };
            if (metadataItem.align) { cellStyle += 'text-align:' + metadataItem.align + ';'; };
            if (cellStyle) { cellAttr += ' style="' + cellStyle + '"'; };
            //Cell Span
            if (metadataItem.span) { cellAttr += ' colspan="' + metadataItem.span + '"'; };
             
            html += '<' + cellType + cellAttr + '>' + cellContent + '</' + cellType + '>';
            
            return html;
        }, 
        buildString: function (data, level, forceLimit) {
            return this.buildStringPreview(data, level, forceLimit);
        },
        buildKeyValuePreview: function (data, level) {
            var that = this;
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + that.buildKeyValuePreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + that.buildKeyValueTable(data, level, true) + '</div></td></tr></table>';
        },
        buildKeyValuePreviewOnly: function (data, level) {
            var that = this, length = $.glimpse.util.lengthJson(data), rowMax = 2, rowLimit = (rowMax < length ? rowMax : length), i = 1, html = '<span class="start">{</span>';

            for (var key in data) {
                html += that.newItemSpacer(i, rowLimit, length);
                if (i > length || i++ > rowLimit)
                    break;
                html += '<span>\'</span>' + that.buildStringPreview(key, level + 1) + '<span>\'</span><span class="mspace">:</span><span>\'</span>' + that.buildStringPreview(data[key], level, 12) + '<span>\'</span>';
            }
            html += '<span class="end">}</span>';

            return html;
        },
        buildCustomPreview: function (data, level) {
            var that = this, isComplex = ($.isArray(data[0]) || $.isPlainObject(data[0]));
            
            if (isComplex && data.length == 1)
                return that.build(data[0], level);
            if (isComplex || data.length > 1) 
                return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + that.buildCustomPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + that.buildCustomTable(data, level, true) + '</div></td></tr></table>';
            return that.buildStringPreview(data[0], level + 1); 
        },
        buildCustomPreviewOnly: function (data, level) { 
            var that = this, isComplex = $.isArray(data[0]), length = (isComplex ? data.length - 1 : data.length), rowMax = 2, columnMax = 3, columnLimit = 1, rowLimit = (rowMax < length ? rowMax : length), html = '<span class="start">[</span>';

            if (isComplex) {
                columnLimit = ((data[0].length > columnMax) ? columnMax : data[0].length);
                for (var i = 1; i <= rowLimit + 1; i++) {
                    html += that.newItemSpacer(i, rowLimit, length);
                    if (i > length || i > rowLimit)
                        break;

                    html += '<span class="start">[</span>';
                    var spacer = '';
                    for (var x = 0; x < columnLimit; x++) {
                        html += spacer + '<span>\'</span>' + that.buildStringPreview(data[i][x], level, 12) + '<span>\'</span>';
                        spacer = '<span class="rspace">,</span>';
                    }
                    if (x < data[0].length)
                        html += spacer + '<span>...</span>';
                    html += '<span class="end">]</span>';
                }
            }
            else { 
                for (var i = 0; i <= rowLimit; i++) {
                    html += that.newItemSpacer(i + 1, rowLimit, length);
                    if (i >= length || i >= rowLimit)
                        break;
                    html += '<span>\'</span>' + that.buildStringPreview(data[i], level, 12) + '<span>\'</span>';
                } 
            }

            html += '<span class="end">]</span>';

            return html;
        },
        buildStructuredTablePreview : function(data, level, metadata) {
            var that = this;
            return '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object">' + that.buildCustomPreviewOnly(data, level) + '</div><div class="glimpse-preview-show">' + that.buildStructuredTable(data, level, true, metadata) + '</div></td></tr></table>';
        },
        buildStringPreview: function (data, level, forceLimit) {
            if (data == undefined || data == null)
                return '--';
            if ($.isArray(data))
                return '[ ... ]';
            if ($.isPlainObject(data))
                return '{ ... }';

            var that = this,
                charMax = !$.isNaN(forceLimit) ? forceLimit : (level > 1 ? 80 : 150),
                charOuterMax = (charMax * 1.2),
                content = $.glimpseContent.trimFormatString(data, charMax, charOuterMax, true);

            if (data.length > charOuterMax) {
                content = '<span class="glimpse-preview-string" title="' + $.glimpseContent.trimFormatString(data, charMax * 2, charMax * 2.1, false, true) + '">' + content + '</span>';
                if (charMax >= 15)
                    content = '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td>' + content + '<span class="glimpse-preview-show">' + $.glimpse.util.preserveWhitespace($.glimpseContent.formatString(data)) + '</span></td></tr></table>';
            }
            else 
                content = $.glimpse.util.preserveWhitespace(content);  
              
            return content;
        },
        //TODO: these needs to be moved out 
        shouldUsePreview: function(length, level, forceFull, limit, forceLimit, tolerance) {
            if (!$.isNaN(forceLimit)) { limit = forceLimit;}
            return !forceFull && ((level == 1 && length > (limit + tolerance)) || (level > 1 && (!forceLimit || length > (limit + tolerance))));
        }, 
        buildFormatString : function(formatString, data, indexs) { //TODO: this needs to be moved out 
            for (var i = 0; i < indexs.length; i++) {
                var pattern = "\\\{\\\{" + indexs[i] + "\\\}\\\}", regex = new RegExp(pattern, "g"); 
                formatString = formatString.replace(regex, data[indexs[i]]);
            }
            return formatString;
        },
        newItemSpacer: function (currentRow, rowLimit, dataLength) { 
            var html = '';
            if (currentRow > 1 && (currentRow <= rowLimit || dataLength > rowLimit)) { html += '<span class="rspace">,</span>'; }
            if (currentRow > rowLimit && dataLength > rowLimit) { html += '<span class="small">length=' + dataLength + '</span>'; }
            return html;
        }
    });

    //#endregion

    //#region $.glimpseContent

    //TODO: Shift to $.glimpse.content instead of $.glimpseContent

    $.extend($.glimpseContent, {
        formatStringTypes: {
            italics: {
                match: function (d) { return d.match(/^\_[\w\D]+\_$/) != null; },
                replace: function (d) { return '<u>' + $.glimpse.util.htmlEncode($.glimpseContent.scrub(d)) + '</u>'; },
                trimmable: true
            },
            underline: {
                match: function (d) { return d.match(/^\\[\w\D]+\\$/) != null; },
                replace: function (d) { return '<em>' + $.glimpse.util.htmlEncode($.glimpseContent.scrub(d)) + '</em>'; },
                trimmable: true
            },
            strong: {
                match: function (d) { return d.match(/^\*[\w\D]+\*$/) != null; },
                replace: function (d) { return '<strong>' + $.glimpse.util.htmlEncode($.glimpseContent.scrub(d)) + '</strong>'; },
                trimmable: true
            },
            raw: {
                match: function (d) { return d.match(/^\![\w\D]+\!$/) != null; },
                replace: function (d) { return $.glimpseContent.scrub(d); },
                trimmable: false
            }
        },
        scrub: function (d) {
            return d.substr(1, d.length - 2);
        },
        formatString: function (data) {
            var that = this;
            return that.trimFormatString(data);
        },
        trimFormatString: function (data, charMax, charOuterMax, wrapEllipsis, skipEncoding) {
            var that = this, trimmable = true, replace = function (d) { return $.glimpse.util.htmlEncode(d); };

            if (data == undefined || data == null)
                return '--';
            if (typeof data != 'string')
                data = data + '';
            //data = $.trim(data);

            if (!skipEncoding) {
                for (var typeKey in that.formatStringTypes) {
                    var type = that.formatStringTypes[typeKey];
                    if (type.match(data)) {
                        replace = type.replace;
                        trimmable = type.trimmable;
                        break;
                    }
                }
            }

            if (trimmable && charOuterMax && data.length > charOuterMax)
                return replace(data.substr(0, charMax)) + (wrapEllipsis ? '<span>...</span>' : '...');
            return replace(data);
        }
    });

    //#endregion

    //#region $.glimpse

    $.extend($.glimpse, {
        _executeProtocolListeners: function (g, isInit) {
            var i = 0, listeners = g.plugins.protocolListeners, data = g.static.data;
            for (; i < listeners.length; i++) {
                var listener = listeners[i];
                if (isInit || !listener.onInitOnly)
                    listener.callback(data);
            }
        },
        _executeLayoutListeners: function (g, isInit) {
            var i = 0, listeners = g.plugins.layoutListeners, static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder();
            for (; i < listeners.length; i++) {
                var listener = listeners[i];
                if (isInit || !listener.onInitOnly)
                    listener.callback(tabStrip, panelHolder);
            }
        },
        _wireEvents: function (g) {
            var static = g.static, settings = g.settings;

            //Open/Close Holder
            $('.glimpse-open').live('click', function (ev) { ev.preventDefault(); g.open(); return false; });
            $('.glimpse-close').live('click', function (ev) { ev.preventDefault(); g.close(); return false; });
            $('.glimpse-terminate').live('click', function (ev) { ev.preventDefault(); g.terminate(); return false; });
            $('.glimpse-popout').live('click', function (ev) { ev.preventDefault(); g.popup.open(); return false; });

            //Tab Switching 
            $('.glimpse-tabs li:not(.glimpse-active, .glimpse-disabled)').live('mouseover mouseout', function (e) {
                var item = $(this);
                if (e.type == 'mouseover') { item.addClass('glimpse-hover'); } else { item.removeClass('glimpse-hover'); }
            });
            $('.glimpse-tabs li:not(.glimpse-active, .glimpse-disabled)').live('click', function () {
                var item = $(this);

                //Setup Tabs
                $('.glimpse-tabs .glimpse-active').removeClass('glimpse-active').removeClass('glimpse-hover');
                item.addClass('glimpse-active');

                //Setup Panels 
                $('.glimpse-panel-holder .glimpse-active').removeClass('glimpse-active');
                $('.glimpse-panel-holder .glimpse-panel:eq(' + $('.glimpse-tabs ul li').index(item) + ')').addClass('glimpse-active');

                //Trigger event
                $('.glimpse').trigger('glimpse.tabchanged', item.attr('data-sort'));

                //Save selection
                settings.activeTab = item.data('sort');
                g.persistState();
            });

            //Resize  
            var parentScope = $('.glimpse-resizer').parent();
            $('.glimpse-resizer').resizer({
                getValue : function(s) { return s.resizeScope.height(); },
                setValue : function(s, value) { return s.resizeScope.height(value + 'px'); },
                resizeScope : parentScope,
                opacityScope : parentScope,
                isUpDown : true, 
                offset : -1,
                endDragCallback: function () {
                    g.settings.height = $('.glimpse-holder').height();
                    g.persistState();

                    $('.glimpse-spacer').height(g.settings.height);
                    $('.glimpse-holder .glimpse-panel').height(g.settings.height - 54);
                    
                    $('.glimpse').trigger('glimpse.resize', g.settings.height - 54);
                }
            });
              
            g._wireCommonPluginEvents(g);

            //Resize panels if we are in popup
            if (static.isPopup) {
                $(window).resize(function () {
                    $('.glimpse-holder .glimpse-panel').height($(window).height() - 54);
                });
            }
            $(window).unload(function() {
                g.popup.close();
            });
        },
        _wireCommonPluginEvents: function (g) {
            //Exspand/Collapse
            $('.glimpse-expand').live('click', function () {
                var button = $(this).toggleClass('glimpse-collapse');
                if (button.hasClass('glimpse-collapse'))
                    button.parent().next().children().first().hide().next().show();
                else
                    button.parent().next().children().first().show().next().hide();
            });
        }, 
        _adjustLayout: function (g) {
            $('.glimpse-spacer').height(g.settings.height);
            $('.glimpse-holder .glimpse-panel').height(g.settings.height - 54);
            $('.glimpse').trigger('glimpse.resize', g.settings.height - 54);
        },
        addProtocolListener: function (callback, onInitOnly) {
            $.glimpse.plugins.protocolListeners.push({ 'callback': callback, 'onInitOnly': onInitOnly });
        },
        addLayoutListener: function (callback, onInitOnly) {
            $.glimpse.plugins.layoutListeners.push({ 'callback': callback, 'onInitOnly': onInitOnly });
        },
        open: function (speed, dontPersist) {
            var g = $.glimpse;

            if (!dontPersist) {
                g.settings.open = true;
                g.persistState();
            }

            $('.glimpse-open').hide();
            $('.glimpse-holder').show().animate({ 'height': g.settings.height }, (speed === undefined ? 'fast' : speed));
            g._adjustLayout(g);
        },
        close: function (speed) {
            var g = $.glimpse;

            g.settings.open = false;
            g.persistState();

            $('.glimpse-holder').animate({ 'height': '0' }, (speed === undefined ? 'fast' : speed), function () {
                $(this).hide();
                $('.glimpse-open').show();
            });
            $('.glimpse-spacer').height('0');
        },
        terminate: function () {
            $('.glimpse-open, .glimpse-spacer').remove();
            $('.glimpse-holder').animate({ 'height': '0' }, 'fast', function () {
                $(this).remove();
            });

            $.glimpse.util.cookie('glimpseState', null);
            $.glimpse.util.cookie('glimpseClientName', null);
            $.glimpse.util.cookie('glimpseOptions', null);
        },
        persistState: function () {
            var g = $.glimpse;
            $.glimpse.util.cookie('glimpseOptions', g.settings);
        },
        restoreState: function () {
            var g = $.glimpse;
            if (g.settings.open)
                g.open(0);
        },
        refresh: function (data, title) {
            if (!data) return;

            var g = $.glimpse, static = g.static;
            static.data = data;

            g._executeProtocolListeners(g, false);

            $.glimpseProcessor.clearLayout(g);
            $.glimpseProcessor.layout(g, title);
            g._adjustLayout(g);

            g._executeLayoutListeners(g, false);
        },
        reset: function () {
            var g = this, static = g.static;

            this.refresh(glimpse, $.glimpseProcessor.buildHeading(static.url, static.clientName, ''));
            $('.glimpse').trigger('glimpse.request.refresh');
        },
        init: function (data) {
            var g = $.glimpse, static = g.static;
            g.clientName = $.glimpse.util.cookie('glimpseClientName');

            var start = (new Date).getTime();

            static.isPopup = window.location.href.indexOf(static.popupUrl) > -1;

            if (!data) {
                if (static.isPopup && window.opener.glimpse) {
                    $.glimpse.util.cookie('glimpseKeepPopup', '');
                    $.glimpse.static.url = window.opener.jQueryGlimpse.glimpse.static.url;
                    glimpse = data = JSON.parse(window.opener.jQueryGlimpse.glimpse.static.dataString);
                }
                else
                    return;
            }

//            var urlTest = static.popupUrl + '&glimpseRequestID=' + $('#glimpseData').data('glimpse-requestID');
//            static.popup = window.open(urlTest, 'GlimpsePopup', 'width=1100,height=600,status=no,toolbar=no,menubar=no,location=no,resizable=yes,scrollbars=yes');

            g.static.data = data;
            g.settings = $.extend(g.settings, $.glimpse.util.cookie('glimpseOptions'));

            g._executeProtocolListeners(g, true);

            $('body').append(static.html.plugin);

            g._wireEvents(g); 

            $.glimpseProcessor.layout(g, $.glimpseProcessor.buildHeading(static.url, static.clientName, ''));

            g._executeLayoutListeners(g, true);

            if (!static.isPopup)
                $('body').append('<div class="glimpse-spacer"></div>');

            g.restoreState();

            if (!static.isPopup && g.settings.popupOn)
                g.popup.open();

                
            /* Run a test. */
            var diff = (new Date).getTime() - start;
            console.log(diff);
        },
        plugins: {
            protocolListeners: [],
            layoutListeners: []
        },
        settings: {
            open: false,
            height: 300,
            activeTab: 'Routes',
            popupOn: false,
            firstPopup: true
        },
        static: {
            data: null,
            url: window.location.href.replace(window.location.protocol + '//' + window.location.host, ''),
            clientName: '',
            html: { plugin: '<div class="glimpse-open"><div class="glimpse-icon"></div></div><div class="glimpse-holder glimpse"><div class="glimpse-resizer"></div><div class="glimpse-bar"><div class="glimpse-icon" title="About Glimpse?"></div><div class="glimpse-title"></div><div class="glimpse-buttons"><a href="#" class="glimpse-meta-warning glimpse-button" title="Glimpse has some warnings!"></a><a href="http://www.nuget.org/List/Packages/Glimpse" class="glimpse-meta-update glimpse-button" title="New version of Glimpse available" target="_blank"></a><a href="#" class="glimpse-meta-help glimpse-button"></a><a href="#" title="Close/Minimize" class="glimpse-close glimpse-button"></a><a href="#" title="Pop Out" class="glimpse-popout glimpse-button"></a><a href="#" title="Shutdown/Terminate" class="glimpse-terminate glimpse-button"></a></div></div><div class="glimpse-content"><div class="glimpse-tabs"><ul></ul></div><div class="glimpse-panel-holder"></div></div></div>' },
            tabStrip: function () { return $('.glimpse-tabs ul'); },
            panelHolder: function () { return $('.glimpse-panel-holder'); },
            mainHolder: function () { return $('.glimpse-holder'); },
            isPopup: false,
            popupUrl: glimpsePath + 'popup',
            popup: null
        },
        popup: {},
        util: {}
    });

    //#endregion

    //#region $.glimpse.util

    $.extend($.glimpse.util, {
        htmlEncode: function (value) {
            return !(value == undefined || value == null) ? $('<div/>').text(value).html() : '';
        },
        htmlDecode: function (value) {
            return !(value == undefined || value == null) ? $('<div/>').html(value).text() : '';
        },
        preserveWhitespace: function (value) {
            return value.replace(/\r\n/g, '<br />').replace(/\n/g, '<br />').replace(/\t/g, '&nbsp; &nbsp; ').replace(/  /g, '&nbsp; ');
        },
        lengthJson: function (data) {
            var count = 0;
            if ($.isPlainObject(data))
                $.each(data, function (k, v) { count++; });
            return count;
        },
        formatTime: function (d) {
            if (typeof d === 'number')
                d = new Date(d);
            var padding = function(t) { return t < 10 ? '0' + t : t; };
            return d.getHours() + ':' + padding(d.getMinutes()) + ':' + padding(d.getSeconds()) + ' ' + d.getMilliseconds();
        },
        cookie: function (key, value, expiresIn) {
            key = encodeURIComponent(key);
            //Set Cookie
            if (arguments.length > 1) {
                var t = new Date();
                t.setDate(t.getDate() + expiresIn || 1000);

                value = $.isPlainObject(value) ? JSON.stringify(value) : String(value);
                return (document.cookie = key + '=' + encodeURIComponent(value) + '; expires=' + t.toUTCString() + '; path=/');
            }

            //Get cookie 
            var result = new RegExp("(?:^|; )" + key + "=([^;]*)").exec(document.cookie);
            if (result) {
                result = decodeURIComponent(result[1]);
                if (result.substr(0, 1) == '{') {
                    result = JSON.parse(result);
                }
                return result;
            }
            return null;
        },
        getTokens: function(formatString, data) { 
            var count = 0, working = '', result = [];
            for (var i = 0; i < formatString.length; i++) {
                var x = formatString[i];
                
                if (count <= 2) { 
                    if (x == '{')
                        count++;
                    else if (x == '}' && count > 0)
                        count--;
                    else if (count == 2) {
                        if ($.isNaN(x)) {
                            count = 0;
                            working = '';
                        }
                        else 
                            working += '' + x;
                    }
                    else {
                        count = 0;
                        working = '';
                    }

                    if (count == 0 && working != '') {
                        result.push(working);
                        working = '';
                    }
                } 
            }
            return result;
        },
        getDomain: function(url) {
            if (url.indexOf('://') > -1)
                url = url.split('://')[1];
            return url.split('/')[0];
        },
        timeConvert:function(value) {
            if (value < 1000)
                return value + 'ms';
            return Math.round(value / 10) / 100 + 's';
        }
    });

    //#endregion

    //#region $.glimpse.popup

    $.extend($.glimpse.popup, {
        open: function () {
            var gp = this, g = $.glimpse, static = g.static;

            if (!static.popup || static.popup.closed) {
                if (g.settings.firstPopup)
                    alert('Glimpse Message: Glimpse may get blocked by your popup blocker, if this is the case make sure you set up and exception for this domain.')

                g.settings.firstPopup = false;
                g.settings.popupOn = true;
                g.persistState();

                static.dataString = JSON.stringify(static.data);

                var url = static.popupUrl + '&glimpseRequestID=' + $('#glimpseData').data('glimpse-requestID');
                static.popup = window.open(url, 'GlimpsePopup', 'width=1100,height=600,status=no,toolbar=no,menubar=no,location=no,resizable=yes,scrollbars=yes');

                if (gp.popupWorked(static.popup))
                    g.close(undefined, true);
            }
        },
        close: function () {
            var gp = this, g = $.glimpse;

            if (g.settings.popupOn) {
                if (g.static.isPopup && !$.glimpse.util.cookie('glimpseKeepPopup')) {
                    g.static.popup = null;
                    g.settings.popupOn = false;
                    g.persistState();
                }
                else
                    $.glimpse.util.cookie('glimpseKeepPopup', '1');
            }
        },
        popupWorked: function (popup) {
            var successfull = (popup && !popup.closed && typeof popup.closed != 'undefined');
            if (!successfull)
                alert("Glimpse Error: Glimpse popup was blocked.");
            return successfull
        }
    });

    //#endregion

    //#region $.glimpseAjax

    //#region XHRSpy
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
            numberOfXHRProperties = 0;
             
        // XMLHttpRequestWrapper internal methods
        var updateSelfPropertiesIgnore = {
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
        };
         
        var updateSelfProperties = function() {
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
        };
        
        var updateXHRPropertiesIgnore = {
            channel: 1,
            onreadystatechange: 1,
            readyState: 1,
            responseBody: 1,
            responseText: 1,
            responseXML: 1,
            status: 1,
            statusText: 1,
            upload: 1
        };
         
        var updateXHRProperties = function() {
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
        };

        var finished = false;
        var finishXHR = function () {
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
        };
         
        var handleStateChange = function () {
            that.readyState = xhrRequest.readyState;

            if (xhrRequest.readyState == 4) { 
                finishXHR();
                xhrRequest.onreadystatechange = function () { };
            }
            that.onreadystatechange();
        };

        // XMLHttpRequestWrapper public properties and handlers 
        this.readyState = 0; 
        this.onreadystatechange = function () { };
 
        // XMLHttpRequestWrapper public methods 
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
    //#endregion

    $.glimpseAjax = {};
    $.extend($.glimpseAjax, {
        init: function () {
            var ga = this, static = ga.static;

            //Wire up plugin
            $.glimpse.addProtocolListener(function (data) { ga.adjustProtocol(data); }, true);
            $.glimpse.addLayoutListener(function (tabStrip, panelHolder) { ga.adjustLayout(tabStrip, panelHolder); }, true);
        },
        adjustProtocol: function (data) {
            var ga = this, metadata;

            data[ga.static.key] = '';

            if ((metadata = data._metadata) && (metadata = metadata.plugins))
                (metadata[ga.static.key] = {})['helpUrl'] = 'http://getglimpse.com/Help/Plugin/Ajax'; 
        },
        adjustLayout: function (tabStrip, panelHolder) {
            var ga = this, static = ga.static;

            //Setup layout
            static.tab = $('.glimpse-tabitem-' + static.key, tabStrip);
            static.panel = $('.glimpse-panelitem-' + static.key, panelHolder);
            static.tab.addClass('glimpse-permanent').text(static.key);
            static.panel.addClass('glimpse-permanent');

            //Wire up events 
            $('.glimpse-clear', static.panel).live('click', function (ev) { ga.removeRequests(); return false; });
            $('thead .glimpse-head-message a', static.panel).live('click', function (ev) { ga.resetContext(); return false; });
            $('.glimpse').live('glimpse.request.refresh', function () { ga.globalResetCallback(); })
                         .live('glimpse.request.change', function (ev, type) { if (type != 'ajax') { ga.globalResetCallback(); } });

            //Reset to start things off 
            ga.removeRequests(true);
        },
        shouldMakePopoutCall: function () {
            var g = $.glimpse;
            return (!g.static.isPopup && g.settings.popupOn && g.static.popup && !g.static.popup.closed)
        },
        removeRequests: function (isInit) {
            var ga = this;

            //Clear out anything inside the panel
            ga.static.panel.html('<div class="glimpse-panel-message">No ajax calls have yet been detected</div>');
            ga.resetContext(isInit);
        },
        resetContext: function (isInit) {
            if (!isInit)
                $.glimpse.reset();
        },
        requestSelected: function (link) {
            var ga = this, panelHead = $('table thead', ga.static.panel);

            $('.glimpse').trigger('glimpse.request.change', ['ajax']);

            //Adjust styles 
            $('.glimpse-head-message', panelHead).fadeIn();
            $('.selected', link.parents('table:first')).removeClass('selected');
            link.parents('tr:first').addClass('selected');
        },
        globalResetCallback: function () {
            var ga = this, panel = ga.static.panel;

            //Adjust styles
            $('thead .glimpse-head-message', panel).fadeOut();
            $('tbody .selected', panel).removeClass('selected');
        },
        callStarted: function (ajaxSpy) {
            var g = $.glimpse, ga = this, static = ga.static, panelHolder = g.static.panelHolder(), panelItem = static.panel;

            if ((ajaxSpy.url && ajaxSpy.url.length > 9 && ajaxSpy.url.indexOf('Glimpse.axd') != -1) || !panelItem || panelItem.length == 0)
                return;

            //Make this exact same call in the popout window
            if (!g.static.isPopup)
                ajaxSpy.logRow = ++static.index;
            if (ga.shouldMakePopoutCall()) {
                g.static.popup.jQueryGlimpse.glimpseAjax.static.index = ajaxSpy.logRow;
                g.static.popup.jQueryGlimpse.glimpseAjax.callStarted(ajaxSpy);
            }

            //First time round we need to set everything up
            if ($('.glimpse-panel-message', panelItem).length > 0) {
                panelItem.html($.glimpseProcessor.build([
                    ['Request URL', 'Status', 'Date/Time', 'Duration', 'Is Async', 'Inspect']
                ], 0)).append('<div class="glimpse-clear"><a href="#">Clear</a></div>');

                //Manually alter the render
                $('table thead', panelItem).append('<tr class="glimpse-head-message" style="display:none"><td colspan="6"><a href="#">Reset context back to starting page</a></td></tr>');
                $('table', panelItem).append('<tbody></tbody>');
            }
            ajaxSpy.clientName = $.glimpse.util.cookie('glimpseClientName');

            //Add new row
            $('table tbody', panelItem).prepend('<tr class="loading" data-index="' + static.index + '"><th><div class="icon"></div>' + ajaxSpy.url + '</th><td class="glimpse-ajax-status">Loading...</td><td>' + $.glimpse.util.formatTime(ajaxSpy.startTime) + '</td><td class="glimpse-ajax-duration">--</td><td>' + ajaxSpy.async + '</td><td class="glimpse-ajax-inspect">N/A</td></tr>');

            //In theory I wouldn't need to do this every time but wanting to make sure that all rows are kept in sync 
            $('table tbody tr:odd', panelItem).removeClass('odd').addClass('even');
            $('table tbody tr:even', panelItem).removeClass('even').addClass('odd');
        },
        callFinished: function (ajaxSpy) {
            var g = $.glimpse, ga = this, static = ga.static, glimpseRequestId = ajaxSpy.responseHeaders['X-Glimpse-RequestID'],
                    panelHolder = g.static.panelHolder(), panelItem = $('.glimpse-panelitem-' + static.key, panelHolder), row = $("tr[data-index='" + ajaxSpy.logRow + "']", panelItem);

            if ((ajaxSpy.url && ajaxSpy.url.length > 9 && ajaxSpy.url.indexOf('Glimpse/') != -1) || !panelItem || panelItem.length == 0)
                return;

            //Make this exact same call in the popout window
            if (ga.shouldMakePopoutCall())
                g.static.popup.jQueryGlimpse.glimpseAjax.callFinished(ajaxSpy);

            //Adjust layout
            row.removeClass('loading').addClass(!ajaxSpy.success ? 'error' : glimpseRequestId ? 'ajax-loaded' : 'suppress');
            $('.glimpse-ajax-status', row).text(ajaxSpy.status);
            $('.glimpse-ajax-duration', row).text(ajaxSpy.duration + 'ms');
            if (glimpseRequestId) {
                var linkPlaceholder = $('.glimpse-ajax-inspect', row).html('<div class="icon"></div>Loading...').addClass('loading');

                $.ajax({
                    url: static.historyLink,
                    type: 'GET',
                    data: { 'ClientRequestID': glimpseRequestId },
                    contentType: 'application/json',
                    success: function (result) {
                        linkPlaceholder.html('<a href="#">Launch</a>').children('a').click(function () {
                            $.glimpse.refresh(eval('(' + result.Data[glimpseRequestId].Data + ')'), $.glimpseProcessor.buildHeading(ajaxSpy.url, ajaxSpy.clientName, ga.static.key));
                            ga.requestSelected($(this));
                            return false;
                        });
                    }
                });
            }
        },
        static: {
            key: 'Ajax',
            tab: null,
            panel: null,
            historyLink: glimpsePath + 'History',
            index: 0
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseAjax.init();

    //#endregion

    //#region $.glimpseRemote

    $.glimpseRemote = {};
    $.extend($.glimpseRemote, {
        init: function () {
            var gr = this;

            //Wire up plugin
            $.glimpse.addProtocolListener(function (data) { gr.adjustProtocol(data); }, true);
            $.glimpse.addLayoutListener(function (tabStrip, panelHolder) { gr.adjustLayout(tabStrip, panelHolder); }, true);
        },
        adjustProtocol: function (data) {
            var gr = this, metadata;

            data[gr.static.key] = ''

            if ((metadata = data._metadata) && (metadata = metadata.plugins))
                (metadata[gr.static.key] = {})['helpUrl'] = 'http://getglimpse.com/Help/Plugin/Remote'; 
        },
        adjustLayout: function (tabStrip, panelHolder) {
            var gr = this, static = gr.static;

            //Setup layout
            static.tab = $('.glimpse-tabitem-' + static.key, tabStrip);
            static.panel = $('.glimpse-panelitem-' + static.key, panelHolder);

            //Make sure we stick round 
            static.tab.addClass('glimpse-permanent').text(static.key);
            static.panel.addClass('glimpse-permanent');
            static.panel.prepend('<div class="glimpse-side-sub-panel"><div class="loading"><div class="icon"></div><span>Refreshing...</span></div><div class="glimpse-content"></div></div><div class="glimpse-side-main-panel"><div class="glimpse-initial glimpse-panel-message">No remote calls have yet been detected</div><div class="loading" style="display:none"><div class="icon"></div><span>Refreshing...</span></div><div class="glimpse-content"></div></div>');

            static.subPanel = $('.glimpse-side-sub-panel', static.panel);
            static.mainPanel = $('.glimpse-side-main-panel', static.panel);

            //Wireevents 
            $('a', static.panel).live('click', function () {
                $('.selected', $(this).parents('table:first')).removeClass('selected');
                $(this).parents('tr:first').addClass('selected');
            });
            $('a.glimpse-orignal', static.subPanel).live('click', function () { gr.resetContext(); return false; });
            $('a.glimpse-trigger', static.subPanel).live('click', function () { gr.sessionSelected($(this).data('client')); return false; });
            $('a', static.mainPanel).live('click', function () { gr.requestSelected($(this).data('client'), $(this).data('request')); return false; });
            static.tab.click(function () { gr.refreshSessionList(); });
            $('.glimpse-clear', static.subPanel).live('click', function (ev) { gr.removeRequests(); return false; });
            $('thead .glimpse-head-message a', static.subPanel).live('click', function (ev) { gr.resetContext(); return false; });
            $('.glimpse').live('glimpse.request.refresh', function () { gr.globalResetCallback(); })
                         .live('glimpse.request.change', function (ev, type) { if (type != 'remote') { gr.globalResetCallback(); } });

            if ($.glimpse.settings.activeTab == static.key)
                gr.refreshSessionList();
        },
        removeRequests: function () {
            var gr = this, static = gr.static;

            static.result = {};
            $('.loading', static.subPanel).show().find('span').text('Loaded...');
            $('.glimpse-content', static.subPanel).html('');

            gr.resetContext();
        },
        resetContext: function () {
            var gr = this, static = gr.static;

            $.glimpse.reset();

            $('.glimpse-initial', static.mainPanel).show();
            $('.loading', static.mainPanel).hide();
            $('.glimpse-content', static.mainPanel).empty();
        },
        globalResetCallback: function () {
            var ga = this, static = ga.static;

            //Adjust styles
            $('thead .glimpse-head-message', static.subPanel).fadeOut();
            $('tbody .selected', static.mainPanel).removeClass('selected');
        },
        sessionSelected: function (clientId) {
            var gr = this;
            gr.refreshRequestList(clientId);
        },
        requestSelected: function (clientId, requestId) {
            var gr = this, static = gr.static, request = static.result.Data[clientId][requestId];
            if (request.Data) {
                $('.glimpse-head-message', static.subPanel).fadeIn(); 
                $.glimpse.refresh(eval('(' + request.Data + ')'), $.glimpseProcessor.buildHeading(request.Url, clientId, static.key)); 
                $('.glimpse').trigger('glimpse.request.change', ['remote']);
            }
        },
        refreshSessionList: function () {
            var gr = this, static = gr.static, loading = $('.loading', static.subPanel);

            $.ajax({
                url: static.clientLink,
                type: 'GET',
                contentType: 'application/json',
                beforeSend: function () {
                    $('span', loading).text('Refreshing...').parent().fadeIn();
                },
                success: function (result) {
                    $('span', loading).text('Loaded...').parent().delay(1500).fadeOut();
                    gr.processSessionList(result);
                }
            });
        },
        refreshRequestList: function (clientId) {
            var gr = this, static = gr.static, loading = $('.loading', static.mainPanel);

            $.ajax({
                url: static.historyLink,
                type: 'GET',
                data: { 'ClientName': clientId },
                contentType: 'application/json',
                beforeSend: function () {
                    $('span', loading).text('Refreshing...').parent().fadeIn();
                },
                success: function (result) {
                    $('span', loading).text('Loaded...').parent().delay(1500).fadeOut();
                    gr.processRequestList(result);
                }
            });
        },
        processSessionList: function (result) {
            var gr = this, static = gr.static;

            //Create the table we need first time round 
            if ($('table', static.subPanel).length == 0) {
                $('.glimpse-content', static.subPanel).html($.glimpseProcessor.build(
                    [['Client', 'Count', 'Launch']], 0)).append('<div class="glimpse-clear"><a href="#">Clear</a></div>');

                //Manually alter the render
                $('table thead', static.subPanel).append('<tr class="glimpse-head-message" style="display:none"><td colspan="3"><a href="#">Reset context back to starting page</a></td></tr>');
                $('table', static.subPanel).append('<tbody></tbody>');
            }

            if (static.result && result) {
                var shouldTriggerHistoryRequest = false, selectedClientName = $(".selected a", static.subPanel).data('client');

                //Adjusts the returned data
                $.extend(true, static.result, result);

                //Need to do some work on this data 
                var ldata = static.result.Data || {}, rdata = result.Data || {};
                for (var lclientToken in ldata) {
                    var lclient = ldata[lclientToken], rclient = rdata[lclientToken], count = 0;

                    //Lets go through the client requests 
                    for (var lclientRequestToken in lclient) {
                        var lclientRequest = lclient[lclientRequestToken];

                        //Remove any tokens that we don't have cached locally and the server does't have 
                        if (!(lclientRequest.Data || (rclient && rclient[lclientRequestToken])))
                            delete lclient[lclientRequestToken];
                        else
                            count++;

                        //Detect if any new requests new requests have arrived, since this client was selected
                        if (selectedClientName == lclientToken && !lclientRequest.Detected)
                            shouldTriggerHistoryRequest = true;
                        lclientRequest.Detected = true;
                    }

                    //Lets update the UI, by updateing the counter, adding a now or removing a row
                    var clientRow = $("tr:has(a[data-client='" + lclientToken + "'])", static.subPanel);
                    if (clientRow.length > 0) {
                        //If the client count is 0 then the server has no data and neither does the client
                        if (count == 0) {
                            delete ldata[lclientToken];
                            clientRow.remove();
                        }
                        else
                            $('td:nth-child(2)', clientRow).text(count);
                    }
                    else
                        $('table', static.subPanel).append('<tr><td>' + lclientToken + '</td><td>' + count + '</td><td><a href="#" class="glimpse-trigger" data-client="' + lclientToken + '">Launch</a></td></tr>')
                }

                //Trigger a history request if we need to 
                if (shouldTriggerHistoryRequest)
                    gr.sessionSelected(selectedClientName);

                //In theory I wouldn't need to do this every time but wanting to make sure that all rows are kept in sync
                $('table tbody tr', static.subPanel).removeClass('even').removeClass('odd');
                $('table tbody tr:odd', static.subPanel).addClass('even');
                $('table tbody tr:even', static.subPanel).addClass('odd');
            }

            //Trigger new fetch
            if (static.tab.hasClass('glimpse-active'))
                setTimeout(function () { gr.refreshSessionList(); }, 5000);
        },
        processRequestList: function (result) {
            var gr = this, static = gr.static;

            if (static.result && result) {

                $.extend(true, static.result, result);

                //Pull out the name of the client
                var ldata = static.result.Data || {}, rdata = result.Data || {}, rclientToken = '';
                for (var key in rdata) {
                    rclientToken = key;
                    break;
                }

                //As long as the client  
                if ($(".selected a[data-client='" + rclientToken + "']", static.subPanel).length > 0) {
                    var lclient = ldata[rclientToken], data = [['Client Name', 'Method', 'Request Url', 'Browser', 'Date/Time', 'Is Ajax', 'Launch']];

                    for (var lclientRequestToken in lclient) {
                        var lclientRequest = lclient[lclientRequestToken];
                        if (lclientRequest.Data)
                            data.push([rclientToken, lclientRequest.Method, lclientRequest.Url, lclientRequest.Browser, lclientRequest.RequestTime, lclientRequest.IsAjax, '!<a href="#" data-request="' + lclientRequestToken + '" data-client="' + rclientToken + '">Launch</a>!']);
                    }

                    $('.glimpse-initial', static.mainPanel).hide();
                    $('.glimpse-content', static.mainPanel).html($.glimpseProcessor.build(data, 0));
                }
            }
        },
        static: {
            key: 'Remote',
            tab: null,
            panel: null,
            subPanel: null,
            mainPanel: null,
            historyLink: glimpsePath + 'History',
            clientLink: glimpsePath + 'Clients',
            result: {},
            _count: 0
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseRemote.init();

    //#endregion

    //#region $.glimpseMeta

    $.glimpseMeta = {};
    $.extend($.glimpseMeta, {
        init: function () {
            var gm = this;

            //Wire up plugin 
            $.glimpse.addProtocolListener(function (data) { gm.adjustProtocol(data); }, true);
            $.glimpse.addLayoutListener(function (tabStrip, panelHolder) { gm.adjustLayout(tabStrip, panelHolder); }, true);

            $('.glimpse').live('glimpse.request.refresh glimpse.request.change', function () { gm.globalResetCallback(); });
        },
        adjustProtocol: function (data) {
            var gm = this, g = $.glimpse, metadata, version = ((metadata = g.static.data._metadata) && (metadata = metadata.request) && (metadata = metadata.runningVersion));
            
            //Info tab
            data[gm.static.key.info] = '<div class="glimpse-info-title"><a href="http://getGlimpse.com/" target="_blank"><img border="0" src="' + glimpsePath + 'logo.png" /></a></div><div>v' + version + '</div><div class="glimpse-info-quote">"What Firebug is for the client, Glimpse is for the server"</div><div class="glimpse-info-more">Go to your Glimpse Config page <a href="' + glimpsePath + 'Config" target="_blank">Glimpse.axd</a></div><div class="glimpse-info-more">For more info see <a href="http://getGlimpse.com" target="_blank">http://getGlimpse.com</a></div><div style="margin:1.5em 0 0.5em;">Created by <strong>Anthony van der Hoorn</strong> (<a href="http://twitter.com/anthony_vdh" target="_blank">@anthony_vdh</a>) and <strong>Nik Molnar</strong> (<a href="http://twitter.com/nikmd23" target="_blank">@nikmd23</a>) - &copy; getglimpse.com 2011</div><div>Have a <em>feature</em> request? <a href="http://getglimpse.uservoice.com" target="_blank">Submit the idea</a>. &nbsp; &nbsp; Found an <em>error</em>? <a href="https://github.com/glimpse/glimpse/issues" target="_blank">Help us improve</a>. &nbsp; &nbsp; Have a <em>question</em>? <a href="http://twitter.com/#search?q=%23glimpse" target="_blank">Tweet us using #glimpse</a>.</div>';
        },
        adjustLayout: function (tabStrip, panelHolder) {
            var gm = this, g = $.glimpse, mainHolder = g.static.mainHolder();

            //Warn tab
            var warnTab = $('.glimpse-tabitem-' + gm.static.key.warn, tabStrip).hide();
            if (warnTab.length > 0) {
                $('.glimpse-meta-warning', mainHolder).css('display', 'inline-block').live('click', function () {
                    warnTab.click();
                    return false;
                });
            }

            //Info tab
            var infoTab = $('.glimpse-tabitem-' + gm.static.key.info, tabStrip).hide();
            if (infoTab.length > 0) {
                $('.glimpse-bar .glimpse-icon').live('click', function () {
                    infoTab.click();
                    return false;
                });
            }

            //Help setup
            $('li', tabStrip).live('click', function () { gm.changeHelp($(this)); });
            gm.changeHelp($('.glimpse-active', tabStrip));

            //Metadata tab
            var metaTab = $('.glimpse-tabitem-' + gm.static.key.meta, tabStrip).hide();
        },
        globalResetCallback: function () {
            var gm = this;
            for (var key in gm.static.key)
                var test = $('.glimpse-tabitem-' + gm.static.key[key]).hide();
        },
        changeHelp: function (item) {
            if (item.hasClass('glimpse-disabled')) return;

            var g = $.glimpse, mainHolder = g.static.mainHolder(), key = item.data('sort'), metaData = g.static.data._metadata, url = '', icon = $('.glimpse-meta-help', mainHolder);
            if (metaData != undefined && (metaData = metaData.plugins[key]) != undefined && (url = metaData.helpUrl) != undefined && url.length > 0)
                icon.show().attr('href', url);
            else
                icon.hide();
        },
        static: {
            key: {
                warn: 'GlimpseWarnings',
                info: 'GlimpseInformation',
                help: 'GlimpseHelp',
                meta: '_metadata'
            }
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseMeta.init();

    //#endregion

    //#region $.glimpseSwitcher

    $.glimpseSwitcher = {}; 
    $.extend($.glimpseSwitcher, {
        init: function () {
            var gs = this;

            //Wire up plugin  
            $.glimpse.addLayoutListener(function(tabStrip, panelHolder) { gs.adjustLayout(); }, true);
            $('.glimpse').live('glimpse.request.change', function (ev, type) { if (type != 'ajax') { gs.adjustLayout(); } });
            $('.glimpse').live('glimpse.request.refresh', function (ev, type) { gs.adjustLayout(); });
            
            //Wireup click listeners
            $('.glimpse-url .glimpse-drop-over a').live('click', function() {
                gs.switchContext($(this).attr('data-requestId'), $(this).attr('data-url'), $('.glimpse .glimpse-snapshot-type').attr('data-clientName'));
                return false;
            });
        },
        adjustLayout: function () {
            var g = $.glimpse, mainHolder = g.static.mainHolder(), metadata = g.static.data._metadata, request = (metadata && metadata.request), environments = (request && request.environmentUrls), correlation = (request && request.correlation);

            var dropFunction = function(scope) {
                $('.glimpse-drop', scope).mouseover(function() { $('.glimpse-drop-over', scope).css('left', $(this).position().left).fadeIn('fast'); }); 
                $('.glimpse-drop-over', scope).mouseleave(function() { $('.glimpse-drop-over', scope).fadeOut(100); }); 
            };
            //Setup environements
            if (environments) {
                var currentEnvironment, environmentsList = '', domain = $.glimpse.util.getDomain(unescape(window.location.href));
                for (name in environments) {
                    if ($.glimpse.util.getDomain(environments[name]) === domain) {
                        currentEnvironment = name;
                        environmentsList += ' - ' + name + ' (Current)<br />';
                    }
                    else
                        environmentsList += ' - <a title="Go to - ' + environments[name] + '" href="' + environments[name] + '">' + name + '</a><br />';
                }

                if (currentEnvironment) {
                    var envHolder = $('.glimpse-title .glimpse-enviro', mainHolder);
                    envHolder.prepend('<span class="glimpse-drop">' + currentEnvironment + '<span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over"><div>Switch Servers</div>' + environmentsList + '</div>');
                    dropFunction(envHolder);
                }
            }
             
            //Setup correlation
            if (correlation) { 
                var urlHolder = $('.glimpse-title .glimpse-url', mainHolder), currentUrl = urlHolder.text(), currentLeg, correlationList = '<div>' + correlation.title + '</div>';
                for (var i = 0; i < correlation.legs.length; i++) {
                    var leg = correlation.legs[i];
                    if (leg.url == currentUrl) {
                        currentLeg = leg;
                        correlationList += currentLeg.url;
                    }
                    else
                        correlationList += '<a title="Go to ' + leg.url + '" href="#" data-requestId="' + leg.glimpseId + '" data-url="' + leg.url + '">' + leg.url + '</a>'; 
                    correlationList += ' <strong>(' + leg.method + ')</strong><br />';
                }
                
                if (currentLeg) {
                    urlHolder.html($('<span class="glimpse-drop">' + currentLeg.url + ' <strong>(' + currentLeg.method + ')</strong><span class="glimpse-drop-arrow-holder"><span class="glimpse-drop-arrow"></span></span></span><div class="glimpse-drop-over">' + correlationList + '<div class="loading"><span class="icon"></span><span>Loaded...</span></div></div>'));
                    dropFunction(urlHolder);
                }
            }
        },
        switchContext: function(glimpseRequestId, glimpseRequestUrl, glimpseClientName) {
            var gs = this, loading = $('.glimpse-url .glimpse-drop-over .loading').fadeIn();
            
            $.ajax({
                url: gs.defaults.historyLink,
                type: 'GET',
                data: { 'ClientRequestID': glimpseRequestId },
                contentType: 'application/json',
                success: function (result) {
                    loading.fadeOut();
                    $.glimpse.refresh(eval('(' + result.Data[glimpseRequestId].Data + ')'), $.glimpseProcessor.buildHeading(glimpseRequestUrl, glimpseClientName, gs.defaults.key));
                    $('.glimpse').trigger('glimpse.request.change', ['correlation']);
                }
            });
        },
        defaults: {
            key: 'PRG',
            historyLink: glimpsePath + 'History'
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseSwitcher.init();

    //#endregion

    //#region $.glimpseUpdateNotification

    $.glimpseUpdateNotification = {};
    $.extend($.glimpseUpdateNotification, {
        init: function () {
            var gu = this;

            //Wire up plugin  
            $.glimpse.addLayoutListener(function (tabStrip, panelHolder) { gu.adjustLayout(tabStrip, panelHolder); }, true);
        },
        adjustLayout: function (tabStrip, panelHolder) {
            var g = $.glimpse, mainHolder = g.static.mainHolder(), newestVersion = $.glimpse.util.cookie('glimpseLatestVersion'), currentVersion = '';

            if (newestVersion) {
                currentVersion = ((currentVersion = g.static.data._metadata) && (currentVersion = currentVersion.request) && (currentVersion = currentVersion.runningVersion));
                if (currentVersion && currentVersion < newestVersion)
                    $('.glimpse-meta-update', mainHolder).attr('title', 'Update: Glimpse ' + parseFloat(newestVersion).toFixed(2) + ' now available on nuget.org').css('display', 'inline-block');

                return;
            }

            $.glimpse.util.cookie('glimpseLatestVersion', -1, 1);

            $.ajax({
                dataType: 'jsonp',
                url: 'http://getglimpse.com/Glimpse/CurrentVersion/',
                success: function (data) {
                    $.glimpse.util.cookie('glimpseLatestVersion', data, 3);
                }
            });
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseUpdateNotification.init();

    //#endregion 

//#region $.glimpseLazyLoad

    $.glimpseLazyLoad = {};
    $.extend($.glimpseLazyLoad, {
        init: function () { 
            var gll = this;

            //Wire up plugin   
            $('.glimpse').live('glimpse.tabchanged', function(ev, type) { gll.loadResource(type); });
        },
        loadResource: function (key) {
            var g = $.glimpse, static = g.static, mainHolder = static.mainHolder(), panel = mainHolder.find('.glimpse-panelitem-' + key), subPanel = panel.find('.glimpse-panel-message[data-glimpse-lazy-url]');
            
            if (subPanel.length > 0) { 
                var url = subPanel.attr('data-glimpse-lazy-url');
                subPanel.attr('data-glimpse-lazy-url', '');

                $.ajax({ 
                    url: url,
                    success: function (data) { 
                        var metadata = ((metadata = static.data._metadata) && (metadata = metadata.plugins[key]) && (metadata = metadata.structure)); 
                        panel.html($.glimpseProcessor.build(data, 0, true, metadata, 1));
                    }
                });
            }
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseLazyLoad.init();

//#endregion

//#region glimpseTimeline

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
                scope.html('<div class="glimpse-timeline"><div class="glimpse-tl-row-summary"><div class="glimpse-tl-content-scroll"><div class="glimpse-tl-event-desc-holder glimpse-tl-col-side"><div class="glimpse-tl-band glimpse-tl-band-title">Categories</div><div class="glimpse-tl-event-desc-group"></div></div><div class="glimpse-tl-band-holder glimpse-tl-col-main"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-band-group"></div></div><div class="glimpse-tl-event-holder glimpse-tl-col-main"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-event-group"></div></div></div><div class="glimpse-tl-padding-holder glimpse-tl-col-main"><div class="glimpse-tl-padding glimpse-tl-padding-l glimpse-tl-summary-height"></div><div class="glimpse-tl-padding glimpse-tl-padding-r glimpse-tl-summary-height"></div></div><div class="glimpse-tl-divider-holder glimpse-tl-col-main"><div class="glimpse-tl-divider-title-bar"></div><div class="glimpse-tl-divider-line-holder"></div></div><div class="glimpse-tl-resizer-holder glimpse-tl-col-main"><div class="glimpse-tl-resizer glimpse-tl-resizer-l glimpse-tl-summary-height"><div class="glimpse-tl-resizer-bar"></div><div class="glimpse-tl-resizer-handle"></div></div><div class="glimpse-tl-resizer glimpse-tl-resizer-r glimpse-tl-summary-height"><div class="glimpse-tl-resizer-bar"></div><div class="glimpse-tl-resizer-handle"></div></div></div></div><div class="glimpse-tl-row-spacer"></div><div class="glimpse-tl-row-content"><div class="glimpse-tl-content-scroll"><div class="glimpse-tl-band-holder glimpse-tl-col-main"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-band-group"></div></div><div class="glimpse-tl-divider-holder glimpse-tl-col-main"><div class="glimpse-tl-divider-zero-holder"><div class="glimpse-tl-divider"></div></div><div class="glimpse-tl-divider-line-holder"></div></div><div class="glimpse-tl-event-holder glimpse-tl-col-main"><div class="glimpse-tl-event-holder-inner"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-event-group"></div></div></div><div class="glimpse-tl-event-desc-holder glimpse-tl-col-side"><div class="glimpse-tl-band glimpse-tl-band-title">Events</div><div class="glimpse-tl-event-desc-group"></div></div></div><div class="glimpse-tl-content-overlay"><div class="glimpse-tl-divider-holder glimpse-tl-col-main"><div class="glimpse-tl-divider-title-bar"></div><div class="glimpse-tl-divider-zero-holder"><div class="glimpse-tl-divider"><div>0</div></div></div><div class="glimpse-tl-divider-line-holder"></div></div></div><div class="glimpse-tl-resizer"><div></div></div></div><div class="glimpse-tl-event-info"></div></div>');
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
                            divider.find('div').text($.glimpse.util.timeConvert(parseInt(time)));
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
                    colorRows();
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
                    for (var i = 0; i < settings.events.length; i += 1) {
                        var event = settings.events[i], 
                            category = settings.category[event.category], 
                            left = (event.startPoint / settings.duration) * 100, 
                            rLeft = Math.round(left),
                            width = (event.duration / settings.duration) * 100, 
                            rWidth = Math.round(width),
                            widthStyle = (width > 0 ? 'width:' + width + '%' : ''),
                            maxStyle = (width <= 0 ? 'max-width:7px;' : ''),
                            subText = (event.subText ? '<span class="glimpse-tl-event-desc-sub">(' + event.subText + ')</span>' : '');

                        //Save calc data
                        event.startPersent = left;
                        event.endPersent = left + width;
                        event.widthPersent = width;

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
                        elements.contentDescHolder.append('<div class="glimpse-tl-band"><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '"></div>' + event.title + subText +'</div>');
                     
                        //Register events for summary  
                        deriveEventSummary(category, left, rLeft, width, rWidth)
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
                colorRows = function () { 
                    //TODO: only do this logic for visable rows
                    elements.contentBandHolder.find('> div:even').addClass('even');
                    elements.contentBandHolder.find('> div:odd').addClass('odd');
                    elements.contentDescHolder.find('> div:even').addClass('even');
                    elements.contentDescHolder.find('> div:odd').addClass('odd');
                    elements.summaryBandHolder.find('> div:even').addClass('even');
                    elements.summaryBandHolder.find('> div:odd').addClass('odd');
                    elements.summaryDescHolder.find('> div:even').addClass('even');
                    elements.summaryDescHolder.find('> div:odd').addClass('odd');
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
                    elements.zoomLeftHandle.resizer({
                        min: function () { return 0; },
                        max: function () { return (elements.zoomRightHandle.position().left - 20); },
                        preDragCallback: function () { elements.zoomLeftHandle.css('left', (elements.zoomLeftHandle.position().left) + 'px'); },
                        endDragCallback: function () { positionLeft(); }
                    });
                    elements.zoomRightHandle.resizer({
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
                    hiddenCategories : {}
                },
                search = function (c) {
                    //Go through each event doing executing search
                    for (var i = 0; i < settings.events.length; i += 1) {
                        var event = settings.events[i],
                            show = !(c.persentLeft > event.endPersent 
                                    || c.persentRightFromLeft < event.startPersent)
                                    && c.hiddenCategories[event.category] != undefined;

                        elements.contentBandHolder.find('.glimpse-tl-band').eq(i).toggle(show);
                        elements.contentEventHolder.find('.glimpse-tl-band').eq(i).toggle(show);
                        elements.contentDescHolder.find('.glimpse-tl-band').eq(i).toggle(show); 
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
                    return '<table><tr><th colspan="2"><div class="glimpse-tl-event-info-title"><div class="glimpse-tl-event" style="background-color:' + category.eventColor + ';border:1px solid ' + category.eventColorHighlight + '"></div>' + event.title + ' - Details</div></th></tr><tr><th>Duration</th><td>' + event.duration + 'ms (at ' + event.startPoint + 'ms' + ( + event.duration > 1 ? (' to ' + (event.startPoint + event.duration) + 'ms') : '' ) +')</td></tr>' + (event.subText ? '<tr><th>Details</th><td>' + event.subText + '</td></tr>' : '' ) + details + '</table>';
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
                    var contentHeight = height - (elements.summaryRow.height() + scope.find('.glimpse-tl-row-spacer').height() + 1);  
                    elements.contentRow.height(contentHeight + 'px');
                    
                    //Render Divers
                    dividerBuilder.render();
                },
                wireEvents = function () { 
                    elements.contentRow.find('.glimpse-tl-resizer').resizer({
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

//TODO this is to  be removed
var glimpseTimelineData = {
    events :[
        { category :'ASPNET', startTime :'', startPoint :0, duration :0, title :'Request Begin', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'ASPNET', startTime :'', startPoint :2, duration :11, title :'Http Handlers', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'ASPNET', startTime :'', startPoint :15, duration :0, title :'Process Pipeline', subText :'', pluginContextId :'', plugin :'', details :{ 'Hello World' :'This is data', 'Jester' :'Hello there' } },
        { category :'Database', startTime :'', startPoint :20, duration :0, title :'Connection', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'MVC', startTime :'', startPoint :25, duration :15, title :'Action', subText :'Person/Add', pluginContextId :'', plugin :'', details :{} },
        { category :'Database', startTime :'', startPoint :55, duration :0, title :'Command', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'MVC', startTime :'', startPoint :60, duration :20, title :'Filter', subText :'Authorization', pluginContextId :'', plugin :'', details :{} },
        { category :'MVC', startTime :'', startPoint :75, duration :35, title :'Filter', subText :'Validation', pluginContextId :'', plugin :'', details :{} },
        { category :'Database', startTime :'', startPoint :80, duration :45, title :'Transaction', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Trace', startTime :'', startPoint :130, duration :0, title :'Logon', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Routes', startTime :'', startPoint :134, duration :0, title :'Resolved Routes', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Trace', startTime :'', startPoint :138, duration :0, title :'Registered', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Trace', startTime :'', startPoint :142, duration :0, title :'Socket Open', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Routes', startTime :'', startPoint :143, duration :5, title :'Partial View', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Database', startTime :'', startPoint :150, duration :0, title :'Command', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Routes', startTime :'', startPoint :152, duration :8, title :'Partial View', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'MVC', startTime :'', startPoint :160, duration :36, title :'View', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Trace', startTime :'', startPoint :195, duration :29, title :'Process Workflow', subText :'', pluginContextId :'', plugin :'', details :{} }
    ],
    category :{
        'ASPNET' : { eventColor : '#FD4545', eventColorHighlight : '#DD3131' },
        'Database' : { eventColor : '#AF78DD', eventColorHighlight : '#823BBE' }, //:{ event:'purple' },
        'MVC' : { eventColor : '#72A3E4', eventColorHighlight : '#5087CF' }, //{ event:'blue' },
        'Trace' : { eventColor : '#FDBF45', eventColorHighlight : '#DDA431' }, //{ event:'orange' },
        'Routes' : { eventColor : '#10E309', eventColorHighlight : '#0EC41D' }, //{ event:'green' }
    },
    duration :'230'
};

var glimpseTimelinePlugin = function () {
    var timeline,
        defaults = {
            key : 'Timeline',
            hasRun : false
        }, 
        timelineData,
        init = function () {
            $.glimpse.addProtocolListener(function(data) { adjustProtocol(data); });
            $.glimpse.addLayoutListener(function(tabStrip, panelHolder) { defaults.hasRun = false; });
            
            $('.glimpse').live('glimpse.tabchanged', function(ev, type) { if (defaults.key == type && !defaults.hasRun) { adjustLayout(); } });
            $('.glimpse').live('glimpse.resize', function(ev, height) { if (timeline || defaults.hasRun) { timeline.support.containerResize(height); } defaults.height = height; });
        },
        adjustProtocol = function (data) {
            //Pull out data and store
            timelineData = data.timeline || glimpseTimelineData;
            //Clear out data
            data[defaults.key] = 'Generating timeline, please wait...';
        },
        adjustLayout = function () {   
            console.log('test');
            timeline = glimpseTimeline($('.glimpse-panelitem-' + defaults.key), timelineData);
            timeline.init();
            if (defaults.height) {
                timeline.support.containerResize(defaults.height);
            }
            defaults.hasRun = true;
        };

    return { 
        init : init 
    };
} ();
    
glimpseTimelinePlugin.init();

//#endregion

    //#region Ready

    //Run glimpse
    $(document).ready(function () {
        $.glimpse.init(glimpse);
    });

    //#endregion

})(jQueryGlimpse); }   
