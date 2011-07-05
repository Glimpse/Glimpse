
    //#region jQueryGlimpse

/*!
 * Note: While Microsoft is not the author of this file, Microsoft is
 * offering you a license subject to the terms of the Microsoft Software
 * License Terms for Microsoft ASP.NET Model View Controller 3.
 * Microsoft reserves all other rights. The notices below are provided
 * for informational purposes only and are not the license terms under
 * which Microsoft distributed this file.
 *
 * jQuery JavaScript Library v1.4.4
 * http://jquery.com/
 *
 * Copyright 2010, John Resig
 *
 * Includes Sizzle.js
 * http://sizzlejs.com/
 * Copyright 2010, The Dojo Foundation
 *
 * Date: Thu Nov 11 19:04:53 2010 -0500
 */
(function(E,B){function ka(a,b,d){if(d===B&&a.nodeType===1){d=a.getAttribute("data-"+b);if(typeof d==="string"){try{d=d==="true"?true:d==="false"?false:d==="null"?null:!c.isNaN(d)?parseFloat(d):Ja.test(d)?c.parseJSON(d):d}catch(e){}c.data(a,b,d)}else d=B}return d}function U(){return false}function ca(){return true}function la(a,b,d){d[0].type=a;return c.event.handle.apply(b,d)}function Ka(a){var b,d,e,f,h,l,k,o,x,r,A,C=[];f=[];h=c.data(this,this.nodeType?"events":"__events__");if(typeof h==="function")h=
h.events;if(!(a.liveFired===this||!h||!h.live||a.button&&a.type==="click")){if(a.namespace)A=RegExp("(^|\\.)"+a.namespace.split(".").join("\\.(?:.*\\.)?")+"(\\.|$)");a.liveFired=this;var J=h.live.slice(0);for(k=0;k<J.length;k++){h=J[k];h.origType.replace(X,"")===a.type?f.push(h.selector):J.splice(k--,1)}f=c(a.target).closest(f,a.currentTarget);o=0;for(x=f.length;o<x;o++){r=f[o];for(k=0;k<J.length;k++){h=J[k];if(r.selector===h.selector&&(!A||A.test(h.namespace))){l=r.elem;e=null;if(h.preType==="mouseenter"||
h.preType==="mouseleave"){a.type=h.preType;e=c(a.relatedTarget).closest(h.selector)[0]}if(!e||e!==l)C.push({elem:l,handleObj:h,level:r.level})}}}o=0;for(x=C.length;o<x;o++){f=C[o];if(d&&f.level>d)break;a.currentTarget=f.elem;a.data=f.handleObj.data;a.handleObj=f.handleObj;A=f.handleObj.origHandler.apply(f.elem,arguments);if(A===false||a.isPropagationStopped()){d=f.level;if(A===false)b=false;if(a.isImmediatePropagationStopped())break}}return b}}function Y(a,b){return(a&&a!=="*"?a+".":"")+b.replace(La,
"`").replace(Ma,"&")}function ma(a,b,d){if(c.isFunction(b))return c.grep(a,function(f,h){return!!b.call(f,h,f)===d});else if(b.nodeType)return c.grep(a,function(f){return f===b===d});else if(typeof b==="string"){var e=c.grep(a,function(f){return f.nodeType===1});if(Na.test(b))return c.filter(b,e,!d);else b=c.filter(b,e)}return c.grep(a,function(f){return c.inArray(f,b)>=0===d})}function na(a,b){var d=0;b.each(function(){if(this.nodeName===(a[d]&&a[d].nodeName)){var e=c.data(a[d++]),f=c.data(this,
e);if(e=e&&e.events){delete f.handle;f.events={};for(var h in e)for(var l in e[h])c.event.add(this,h,e[h][l],e[h][l].data)}}})}function Oa(a,b){b.src?c.ajax({url:b.src,async:false,dataType:"script"}):c.globalEval(b.text||b.textContent||b.innerHTML||"");b.parentNode&&b.parentNode.removeChild(b)}function oa(a,b,d){var e=b==="width"?a.offsetWidth:a.offsetHeight;if(d==="border")return e;c.each(b==="width"?Pa:Qa,function(){d||(e-=parseFloat(c.css(a,"padding"+this))||0);if(d==="margin")e+=parseFloat(c.css(a,
"margin"+this))||0;else e-=parseFloat(c.css(a,"border"+this+"Width"))||0});return e}function da(a,b,d,e){if(c.isArray(b)&&b.length)c.each(b,function(f,h){d||Ra.test(a)?e(a,h):da(a+"["+(typeof h==="object"||c.isArray(h)?f:"")+"]",h,d,e)});else if(!d&&b!=null&&typeof b==="object")c.isEmptyObject(b)?e(a,""):c.each(b,function(f,h){da(a+"["+f+"]",h,d,e)});else e(a,b)}function S(a,b){var d={};c.each(pa.concat.apply([],pa.slice(0,b)),function(){d[this]=a});return d}function qa(a){if(!ea[a]){var b=c("<"+
a+">").appendTo("body"),d=b.css("display");b.remove();if(d==="none"||d==="")d="block";ea[a]=d}return ea[a]}function fa(a){return c.isWindow(a)?a:a.nodeType===9?a.defaultView||a.parentWindow:false}var t=E.document,c=function(){function a(){if(!b.isReady){try{t.documentElement.doScroll("left")}catch(j){setTimeout(a,1);return}b.ready()}}var b=function(j,s){return new b.fn.init(j,s)},d=E.jQueryGlimpse,e=E.$Glimpse,f,h=/^(?:[^<]*(<[\w\W]+>)[^>]*$|#([\w\-]+)$)/,l=/\S/,k=/^\s+/,o=/\s+$/,x=/\W/,r=/\d/,A=/^<(\w+)\s*\/?>(?:<\/\1>)?$/,
C=/^[\],:{}\s]*$/,J=/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g,w=/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,I=/(?:^|:|,)(?:\s*\[)+/g,L=/(webkit)[ \/]([\w.]+)/,g=/(opera)(?:.*version)?[ \/]([\w.]+)/,i=/(msie) ([\w.]+)/,n=/(mozilla)(?:.*? rv:([\w.]+))?/,m=navigator.userAgent,p=false,q=[],u,y=Object.prototype.toString,F=Object.prototype.hasOwnProperty,M=Array.prototype.push,N=Array.prototype.slice,O=String.prototype.trim,D=Array.prototype.indexOf,R={};b.fn=b.prototype={init:function(j,
s){var v,z,H;if(!j)return this;if(j.nodeType){this.context=this[0]=j;this.length=1;return this}if(j==="body"&&!s&&t.body){this.context=t;this[0]=t.body;this.selector="body";this.length=1;return this}if(typeof j==="string")if((v=h.exec(j))&&(v[1]||!s))if(v[1]){H=s?s.ownerDocument||s:t;if(z=A.exec(j))if(b.isPlainObject(s)){j=[t.createElement(z[1])];b.fn.attr.call(j,s,true)}else j=[H.createElement(z[1])];else{z=b.buildFragment([v[1]],[H]);j=(z.cacheable?z.fragment.cloneNode(true):z.fragment).childNodes}return b.merge(this,
j)}else{if((z=t.getElementById(v[2]))&&z.parentNode){if(z.id!==v[2])return f.find(j);this.length=1;this[0]=z}this.context=t;this.selector=j;return this}else if(!s&&!x.test(j)){this.selector=j;this.context=t;j=t.getElementsByTagName(j);return b.merge(this,j)}else return!s||s.jquery?(s||f).find(j):b(s).find(j);else if(b.isFunction(j))return f.ready(j);if(j.selector!==B){this.selector=j.selector;this.context=j.context}return b.makeArray(j,this)},selector:"",jquery:"1.4.4",length:0,size:function(){return this.length},
toArray:function(){return N.call(this,0)},get:function(j){return j==null?this.toArray():j<0?this.slice(j)[0]:this[j]},pushStack:function(j,s,v){var z=b();b.isArray(j)?M.apply(z,j):b.merge(z,j);z.prevObject=this;z.context=this.context;if(s==="find")z.selector=this.selector+(this.selector?" ":"")+v;else if(s)z.selector=this.selector+"."+s+"("+v+")";return z},each:function(j,s){return b.each(this,j,s)},ready:function(j){b.bindReady();if(b.isReady)j.call(t,b);else q&&q.push(j);return this},eq:function(j){return j===
-1?this.slice(j):this.slice(j,+j+1)},first:function(){return this.eq(0)},last:function(){return this.eq(-1)},slice:function(){return this.pushStack(N.apply(this,arguments),"slice",N.call(arguments).join(","))},map:function(j){return this.pushStack(b.map(this,function(s,v){return j.call(s,v,s)}))},end:function(){return this.prevObject||b(null)},push:M,sort:[].sort,splice:[].splice};b.fn.init.prototype=b.fn;b.extend=b.fn.extend=function(){var j,s,v,z,H,G=arguments[0]||{},K=1,Q=arguments.length,ga=false;
if(typeof G==="boolean"){ga=G;G=arguments[1]||{};K=2}if(typeof G!=="object"&&!b.isFunction(G))G={};if(Q===K){G=this;--K}for(;K<Q;K++)if((j=arguments[K])!=null)for(s in j){v=G[s];z=j[s];if(G!==z)if(ga&&z&&(b.isPlainObject(z)||(H=b.isArray(z)))){if(H){H=false;v=v&&b.isArray(v)?v:[]}else v=v&&b.isPlainObject(v)?v:{};G[s]=b.extend(ga,v,z)}else if(z!==B)G[s]=z}return G};b.extend({noConflict:function(j){E.$Glimpse=e;if(j)E.jQueryGlimpse=d;return b},isReady:false,readyWait:1,ready:function(j){j===true&&b.readyWait--;
if(!b.readyWait||j!==true&&!b.isReady){if(!t.body)return setTimeout(b.ready,1);b.isReady=true;if(!(j!==true&&--b.readyWait>0))if(q){var s=0,v=q;for(q=null;j=v[s++];)j.call(t,b);b.fn.trigger&&b(t).trigger("ready").unbind("ready")}}},bindReady:function(){if(!p){p=true;if(t.readyState==="complete")return setTimeout(b.ready,1);if(t.addEventListener){t.addEventListener("DOMContentLoaded",u,false);E.addEventListener("load",b.ready,false)}else if(t.attachEvent){t.attachEvent("onreadystatechange",u);E.attachEvent("onload",
b.ready);var j=false;try{j=E.frameElement==null}catch(s){}t.documentElement.doScroll&&j&&a()}}},isFunction:function(j){return b.type(j)==="function"},isArray:Array.isArray||function(j){return b.type(j)==="array"},isWindow:function(j){return j&&typeof j==="object"&&"setInterval"in j},isNaN:function(j){return j==null||!r.test(j)||isNaN(j)},type:function(j){return j==null?String(j):R[y.call(j)]||"object"},isPlainObject:function(j){if(!j||b.type(j)!=="object"||j.nodeType||b.isWindow(j))return false;if(j.constructor&&
!F.call(j,"constructor")&&!F.call(j.constructor.prototype,"isPrototypeOf"))return false;for(var s in j);return s===B||F.call(j,s)},isEmptyObject:function(j){for(var s in j)return false;return true},error:function(j){throw j;},parseJSON:function(j){if(typeof j!=="string"||!j)return null;j=b.trim(j);if(C.test(j.replace(J,"@").replace(w,"]").replace(I,"")))return E.JSON&&E.JSON.parse?E.JSON.parse(j):(new Function("return "+j))();else b.error("Invalid JSON: "+j)},noop:function(){},globalEval:function(j){if(j&&
l.test(j)){var s=t.getElementsByTagName("head")[0]||t.documentElement,v=t.createElement("script");v.type="text/javascript";if(b.support.scriptEval)v.appendChild(t.createTextNode(j));else v.text=j;s.insertBefore(v,s.firstChild);s.removeChild(v)}},nodeName:function(j,s){return j.nodeName&&j.nodeName.toUpperCase()===s.toUpperCase()},each:function(j,s,v){var z,H=0,G=j.length,K=G===B||b.isFunction(j);if(v)if(K)for(z in j){if(s.apply(j[z],v)===false)break}else for(;H<G;){if(s.apply(j[H++],v)===false)break}else if(K)for(z in j){if(s.call(j[z],
z,j[z])===false)break}else for(v=j[0];H<G&&s.call(v,H,v)!==false;v=j[++H]);return j},trim:O?function(j){return j==null?"":O.call(j)}:function(j){return j==null?"":j.toString().replace(k,"").replace(o,"")},makeArray:function(j,s){var v=s||[];if(j!=null){var z=b.type(j);j.length==null||z==="string"||z==="function"||z==="regexp"||b.isWindow(j)?M.call(v,j):b.merge(v,j)}return v},inArray:function(j,s){if(s.indexOf)return s.indexOf(j);for(var v=0,z=s.length;v<z;v++)if(s[v]===j)return v;return-1},merge:function(j,
s){var v=j.length,z=0;if(typeof s.length==="number")for(var H=s.length;z<H;z++)j[v++]=s[z];else for(;s[z]!==B;)j[v++]=s[z++];j.length=v;return j},grep:function(j,s,v){var z=[],H;v=!!v;for(var G=0,K=j.length;G<K;G++){H=!!s(j[G],G);v!==H&&z.push(j[G])}return z},map:function(j,s,v){for(var z=[],H,G=0,K=j.length;G<K;G++){H=s(j[G],G,v);if(H!=null)z[z.length]=H}return z.concat.apply([],z)},guid:1,proxy:function(j,s,v){if(arguments.length===2)if(typeof s==="string"){v=j;j=v[s];s=B}else if(s&&!b.isFunction(s)){v=
s;s=B}if(!s&&j)s=function(){return j.apply(v||this,arguments)};if(j)s.guid=j.guid=j.guid||s.guid||b.guid++;return s},access:function(j,s,v,z,H,G){var K=j.length;if(typeof s==="object"){for(var Q in s)b.access(j,Q,s[Q],z,H,v);return j}if(v!==B){z=!G&&z&&b.isFunction(v);for(Q=0;Q<K;Q++)H(j[Q],s,z?v.call(j[Q],Q,H(j[Q],s)):v,G);return j}return K?H(j[0],s):B},now:function(){return(new Date).getTime()},uaMatch:function(j){j=j.toLowerCase();j=L.exec(j)||g.exec(j)||i.exec(j)||j.indexOf("compatible")<0&&n.exec(j)||
[];return{browser:j[1]||"",version:j[2]||"0"}},browser:{}});b.each("Boolean Number String Function Array Date RegExp Object".split(" "),function(j,s){R["[object "+s+"]"]=s.toLowerCase()});m=b.uaMatch(m);if(m.browser){b.browser[m.browser]=true;b.browser.version=m.version}if(b.browser.webkit)b.browser.safari=true;if(D)b.inArray=function(j,s){return D.call(s,j)};if(!/\s/.test("\u00a0")){k=/^[\s\xA0]+/;o=/[\s\xA0]+$/}f=b(t);if(t.addEventListener)u=function(){t.removeEventListener("DOMContentLoaded",u,
false);b.ready()};else if(t.attachEvent)u=function(){if(t.readyState==="complete"){t.detachEvent("onreadystatechange",u);b.ready()}};return E.jQueryGlimpse=E.$Glimpse=b}();(function(){c.support={};var a=t.documentElement,b=t.createElement("script"),d=t.createElement("div"),e="script"+c.now();d.style.display="none";d.innerHTML="   <link/><table></table><a href='/a' style='color:red;float:left;opacity:.55;'>a</a><input type='checkbox'/>";var f=d.getElementsByTagName("*"),h=d.getElementsByTagName("a")[0],l=t.createElement("select"),
k=l.appendChild(t.createElement("option"));if(!(!f||!f.length||!h)){c.support={leadingWhitespace:d.firstChild.nodeType===3,tbody:!d.getElementsByTagName("tbody").length,htmlSerialize:!!d.getElementsByTagName("link").length,style:/red/.test(h.getAttribute("style")),hrefNormalized:h.getAttribute("href")==="/a",opacity:/^0.55$/.test(h.style.opacity),cssFloat:!!h.style.cssFloat,checkOn:d.getElementsByTagName("input")[0].value==="on",optSelected:k.selected,deleteExpando:true,optDisabled:false,checkClone:false,
scriptEval:false,noCloneEvent:true,boxModel:null,inlineBlockNeedsLayout:false,shrinkWrapBlocks:false,reliableHiddenOffsets:true};l.disabled=true;c.support.optDisabled=!k.disabled;b.type="text/javascript";try{b.appendChild(t.createTextNode("window."+e+"=1;"))}catch(o){}a.insertBefore(b,a.firstChild);if(E[e]){c.support.scriptEval=true;delete E[e]}try{delete b.test}catch(x){c.support.deleteExpando=false}a.removeChild(b);if(d.attachEvent&&d.fireEvent){d.attachEvent("onclick",function r(){c.support.noCloneEvent=
false;d.detachEvent("onclick",r)});d.cloneNode(true).fireEvent("onclick")}d=t.createElement("div");d.innerHTML="<input type='radio' name='radiotest' checked='checked'/>";a=t.createDocumentFragment();a.appendChild(d.firstChild);c.support.checkClone=a.cloneNode(true).cloneNode(true).lastChild.checked;c(function(){var r=t.createElement("div");r.style.width=r.style.paddingLeft="1px";t.body.appendChild(r);c.boxModel=c.support.boxModel=r.offsetWidth===2;if("zoom"in r.style){r.style.display="inline";r.style.zoom=
1;c.support.inlineBlockNeedsLayout=r.offsetWidth===2;r.style.display="";r.innerHTML="<div style='width:4px;'></div>";c.support.shrinkWrapBlocks=r.offsetWidth!==2}r.innerHTML="<table><tr><td style='padding:0;display:none'></td><td>t</td></tr></table>";var A=r.getElementsByTagName("td");c.support.reliableHiddenOffsets=A[0].offsetHeight===0;A[0].style.display="";A[1].style.display="none";c.support.reliableHiddenOffsets=c.support.reliableHiddenOffsets&&A[0].offsetHeight===0;r.innerHTML="";t.body.removeChild(r).style.display=
"none"});a=function(r){var A=t.createElement("div");r="on"+r;var C=r in A;if(!C){A.setAttribute(r,"return;");C=typeof A[r]==="function"}return C};c.support.submitBubbles=a("submit");c.support.changeBubbles=a("change");a=b=d=f=h=null}})();var ra={},Ja=/^(?:\{.*\}|\[.*\])$/;c.extend({cache:{},uuid:0,expando:"jQuery"+c.now(),noData:{embed:true,object:"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000",applet:true},data:function(a,b,d){if(c.acceptData(a)){a=a==E?ra:a;var e=a.nodeType,f=e?a[c.expando]:null,h=
c.cache;if(!(e&&!f&&typeof b==="string"&&d===B)){if(e)f||(a[c.expando]=f=++c.uuid);else h=a;if(typeof b==="object")if(e)h[f]=c.extend(h[f],b);else c.extend(h,b);else if(e&&!h[f])h[f]={};a=e?h[f]:h;if(d!==B)a[b]=d;return typeof b==="string"?a[b]:a}}},removeData:function(a,b){if(c.acceptData(a)){a=a==E?ra:a;var d=a.nodeType,e=d?a[c.expando]:a,f=c.cache,h=d?f[e]:e;if(b){if(h){delete h[b];d&&c.isEmptyObject(h)&&c.removeData(a)}}else if(d&&c.support.deleteExpando)delete a[c.expando];else if(a.removeAttribute)a.removeAttribute(c.expando);
else if(d)delete f[e];else for(var l in a)delete a[l]}},acceptData:function(a){if(a.nodeName){var b=c.noData[a.nodeName.toLowerCase()];if(b)return!(b===true||a.getAttribute("classid")!==b)}return true}});c.fn.extend({data:function(a,b){var d=null;if(typeof a==="undefined"){if(this.length){var e=this[0].attributes,f;d=c.data(this[0]);for(var h=0,l=e.length;h<l;h++){f=e[h].name;if(f.indexOf("data-")===0){f=f.substr(5);ka(this[0],f,d[f])}}}return d}else if(typeof a==="object")return this.each(function(){c.data(this,
a)});var k=a.split(".");k[1]=k[1]?"."+k[1]:"";if(b===B){d=this.triggerHandler("getData"+k[1]+"!",[k[0]]);if(d===B&&this.length){d=c.data(this[0],a);d=ka(this[0],a,d)}return d===B&&k[1]?this.data(k[0]):d}else return this.each(function(){var o=c(this),x=[k[0],b];o.triggerHandler("setData"+k[1]+"!",x);c.data(this,a,b);o.triggerHandler("changeData"+k[1]+"!",x)})},removeData:function(a){return this.each(function(){c.removeData(this,a)})}});c.extend({queue:function(a,b,d){if(a){b=(b||"fx")+"queue";var e=
c.data(a,b);if(!d)return e||[];if(!e||c.isArray(d))e=c.data(a,b,c.makeArray(d));else e.push(d);return e}},dequeue:function(a,b){b=b||"fx";var d=c.queue(a,b),e=d.shift();if(e==="inprogress")e=d.shift();if(e){b==="fx"&&d.unshift("inprogress");e.call(a,function(){c.dequeue(a,b)})}}});c.fn.extend({queue:function(a,b){if(typeof a!=="string"){b=a;a="fx"}if(b===B)return c.queue(this[0],a);return this.each(function(){var d=c.queue(this,a,b);a==="fx"&&d[0]!=="inprogress"&&c.dequeue(this,a)})},dequeue:function(a){return this.each(function(){c.dequeue(this,
a)})},delay:function(a,b){a=c.fx?c.fx.speeds[a]||a:a;b=b||"fx";return this.queue(b,function(){var d=this;setTimeout(function(){c.dequeue(d,b)},a)})},clearQueue:function(a){return this.queue(a||"fx",[])}});var sa=/[\n\t]/g,ha=/\s+/,Sa=/\r/g,Ta=/^(?:href|src|style)$/,Ua=/^(?:button|input)$/i,Va=/^(?:button|input|object|select|textarea)$/i,Wa=/^a(?:rea)?$/i,ta=/^(?:radio|checkbox)$/i;c.props={"for":"htmlFor","class":"className",readonly:"readOnly",maxlength:"maxLength",cellspacing:"cellSpacing",rowspan:"rowSpan",
colspan:"colSpan",tabindex:"tabIndex",usemap:"useMap",frameborder:"frameBorder"};c.fn.extend({attr:function(a,b){return c.access(this,a,b,true,c.attr)},removeAttr:function(a){return this.each(function(){c.attr(this,a,"");this.nodeType===1&&this.removeAttribute(a)})},addClass:function(a){if(c.isFunction(a))return this.each(function(x){var r=c(this);r.addClass(a.call(this,x,r.attr("class")))});if(a&&typeof a==="string")for(var b=(a||"").split(ha),d=0,e=this.length;d<e;d++){var f=this[d];if(f.nodeType===
1)if(f.className){for(var h=" "+f.className+" ",l=f.className,k=0,o=b.length;k<o;k++)if(h.indexOf(" "+b[k]+" ")<0)l+=" "+b[k];f.className=c.trim(l)}else f.className=a}return this},removeClass:function(a){if(c.isFunction(a))return this.each(function(o){var x=c(this);x.removeClass(a.call(this,o,x.attr("class")))});if(a&&typeof a==="string"||a===B)for(var b=(a||"").split(ha),d=0,e=this.length;d<e;d++){var f=this[d];if(f.nodeType===1&&f.className)if(a){for(var h=(" "+f.className+" ").replace(sa," "),
l=0,k=b.length;l<k;l++)h=h.replace(" "+b[l]+" "," ");f.className=c.trim(h)}else f.className=""}return this},toggleClass:function(a,b){var d=typeof a,e=typeof b==="boolean";if(c.isFunction(a))return this.each(function(f){var h=c(this);h.toggleClass(a.call(this,f,h.attr("class"),b),b)});return this.each(function(){if(d==="string")for(var f,h=0,l=c(this),k=b,o=a.split(ha);f=o[h++];){k=e?k:!l.hasClass(f);l[k?"addClass":"removeClass"](f)}else if(d==="undefined"||d==="boolean"){this.className&&c.data(this,
"__className__",this.className);this.className=this.className||a===false?"":c.data(this,"__className__")||""}})},hasClass:function(a){a=" "+a+" ";for(var b=0,d=this.length;b<d;b++)if((" "+this[b].className+" ").replace(sa," ").indexOf(a)>-1)return true;return false},val:function(a){if(!arguments.length){var b=this[0];if(b){if(c.nodeName(b,"option")){var d=b.attributes.value;return!d||d.specified?b.value:b.text}if(c.nodeName(b,"select")){var e=b.selectedIndex;d=[];var f=b.options;b=b.type==="select-one";
if(e<0)return null;var h=b?e:0;for(e=b?e+1:f.length;h<e;h++){var l=f[h];if(l.selected&&(c.support.optDisabled?!l.disabled:l.getAttribute("disabled")===null)&&(!l.parentNode.disabled||!c.nodeName(l.parentNode,"optgroup"))){a=c(l).val();if(b)return a;d.push(a)}}return d}if(ta.test(b.type)&&!c.support.checkOn)return b.getAttribute("value")===null?"on":b.value;return(b.value||"").replace(Sa,"")}return B}var k=c.isFunction(a);return this.each(function(o){var x=c(this),r=a;if(this.nodeType===1){if(k)r=
a.call(this,o,x.val());if(r==null)r="";else if(typeof r==="number")r+="";else if(c.isArray(r))r=c.map(r,function(C){return C==null?"":C+""});if(c.isArray(r)&&ta.test(this.type))this.checked=c.inArray(x.val(),r)>=0;else if(c.nodeName(this,"select")){var A=c.makeArray(r);c("option",this).each(function(){this.selected=c.inArray(c(this).val(),A)>=0});if(!A.length)this.selectedIndex=-1}else this.value=r}})}});c.extend({attrFn:{val:true,css:true,html:true,text:true,data:true,width:true,height:true,offset:true},
attr:function(a,b,d,e){if(!a||a.nodeType===3||a.nodeType===8)return B;if(e&&b in c.attrFn)return c(a)[b](d);e=a.nodeType!==1||!c.isXMLDoc(a);var f=d!==B;b=e&&c.props[b]||b;var h=Ta.test(b);if((b in a||a[b]!==B)&&e&&!h){if(f){b==="type"&&Ua.test(a.nodeName)&&a.parentNode&&c.error("type property can't be changed");if(d===null)a.nodeType===1&&a.removeAttribute(b);else a[b]=d}if(c.nodeName(a,"form")&&a.getAttributeNode(b))return a.getAttributeNode(b).nodeValue;if(b==="tabIndex")return(b=a.getAttributeNode("tabIndex"))&&
b.specified?b.value:Va.test(a.nodeName)||Wa.test(a.nodeName)&&a.href?0:B;return a[b]}if(!c.support.style&&e&&b==="style"){if(f)a.style.cssText=""+d;return a.style.cssText}f&&a.setAttribute(b,""+d);if(!a.attributes[b]&&a.hasAttribute&&!a.hasAttribute(b))return B;a=!c.support.hrefNormalized&&e&&h?a.getAttribute(b,2):a.getAttribute(b);return a===null?B:a}});var X=/\.(.*)$/,ia=/^(?:textarea|input|select)$/i,La=/\./g,Ma=/ /g,Xa=/[^\w\s.|`]/g,Ya=function(a){return a.replace(Xa,"\\$&")},ua={focusin:0,focusout:0};
c.event={add:function(a,b,d,e){if(!(a.nodeType===3||a.nodeType===8)){if(c.isWindow(a)&&a!==E&&!a.frameElement)a=E;if(d===false)d=U;else if(!d)return;var f,h;if(d.handler){f=d;d=f.handler}if(!d.guid)d.guid=c.guid++;if(h=c.data(a)){var l=a.nodeType?"events":"__events__",k=h[l],o=h.handle;if(typeof k==="function"){o=k.handle;k=k.events}else if(!k){a.nodeType||(h[l]=h=function(){});h.events=k={}}if(!o)h.handle=o=function(){return typeof c!=="undefined"&&!c.event.triggered?c.event.handle.apply(o.elem,
arguments):B};o.elem=a;b=b.split(" ");for(var x=0,r;l=b[x++];){h=f?c.extend({},f):{handler:d,data:e};if(l.indexOf(".")>-1){r=l.split(".");l=r.shift();h.namespace=r.slice(0).sort().join(".")}else{r=[];h.namespace=""}h.type=l;if(!h.guid)h.guid=d.guid;var A=k[l],C=c.event.special[l]||{};if(!A){A=k[l]=[];if(!C.setup||C.setup.call(a,e,r,o)===false)if(a.addEventListener)a.addEventListener(l,o,false);else a.attachEvent&&a.attachEvent("on"+l,o)}if(C.add){C.add.call(a,h);if(!h.handler.guid)h.handler.guid=
d.guid}A.push(h);c.event.global[l]=true}a=null}}},global:{},remove:function(a,b,d,e){if(!(a.nodeType===3||a.nodeType===8)){if(d===false)d=U;var f,h,l=0,k,o,x,r,A,C,J=a.nodeType?"events":"__events__",w=c.data(a),I=w&&w[J];if(w&&I){if(typeof I==="function"){w=I;I=I.events}if(b&&b.type){d=b.handler;b=b.type}if(!b||typeof b==="string"&&b.charAt(0)==="."){b=b||"";for(f in I)c.event.remove(a,f+b)}else{for(b=b.split(" ");f=b[l++];){r=f;k=f.indexOf(".")<0;o=[];if(!k){o=f.split(".");f=o.shift();x=RegExp("(^|\\.)"+
c.map(o.slice(0).sort(),Ya).join("\\.(?:.*\\.)?")+"(\\.|$)")}if(A=I[f])if(d){r=c.event.special[f]||{};for(h=e||0;h<A.length;h++){C=A[h];if(d.guid===C.guid){if(k||x.test(C.namespace)){e==null&&A.splice(h--,1);r.remove&&r.remove.call(a,C)}if(e!=null)break}}if(A.length===0||e!=null&&A.length===1){if(!r.teardown||r.teardown.call(a,o)===false)c.removeEvent(a,f,w.handle);delete I[f]}}else for(h=0;h<A.length;h++){C=A[h];if(k||x.test(C.namespace)){c.event.remove(a,r,C.handler,h);A.splice(h--,1)}}}if(c.isEmptyObject(I)){if(b=
w.handle)b.elem=null;delete w.events;delete w.handle;if(typeof w==="function")c.removeData(a,J);else c.isEmptyObject(w)&&c.removeData(a)}}}}},trigger:function(a,b,d,e){var f=a.type||a;if(!e){a=typeof a==="object"?a[c.expando]?a:c.extend(c.Event(f),a):c.Event(f);if(f.indexOf("!")>=0){a.type=f=f.slice(0,-1);a.exclusive=true}if(!d){a.stopPropagation();c.event.global[f]&&c.each(c.cache,function(){this.events&&this.events[f]&&c.event.trigger(a,b,this.handle.elem)})}if(!d||d.nodeType===3||d.nodeType===
8)return B;a.result=B;a.target=d;b=c.makeArray(b);b.unshift(a)}a.currentTarget=d;(e=d.nodeType?c.data(d,"handle"):(c.data(d,"__events__")||{}).handle)&&e.apply(d,b);e=d.parentNode||d.ownerDocument;try{if(!(d&&d.nodeName&&c.noData[d.nodeName.toLowerCase()]))if(d["on"+f]&&d["on"+f].apply(d,b)===false){a.result=false;a.preventDefault()}}catch(h){}if(!a.isPropagationStopped()&&e)c.event.trigger(a,b,e,true);else if(!a.isDefaultPrevented()){var l;e=a.target;var k=f.replace(X,""),o=c.nodeName(e,"a")&&k===
"click",x=c.event.special[k]||{};if((!x._default||x._default.call(d,a)===false)&&!o&&!(e&&e.nodeName&&c.noData[e.nodeName.toLowerCase()])){try{if(e[k]){if(l=e["on"+k])e["on"+k]=null;c.event.triggered=true;e[k]()}}catch(r){}if(l)e["on"+k]=l;c.event.triggered=false}}},handle:function(a){var b,d,e,f;d=[];var h=c.makeArray(arguments);a=h[0]=c.event.fix(a||E.event);a.currentTarget=this;b=a.type.indexOf(".")<0&&!a.exclusive;if(!b){e=a.type.split(".");a.type=e.shift();d=e.slice(0).sort();e=RegExp("(^|\\.)"+
d.join("\\.(?:.*\\.)?")+"(\\.|$)")}a.namespace=a.namespace||d.join(".");f=c.data(this,this.nodeType?"events":"__events__");if(typeof f==="function")f=f.events;d=(f||{})[a.type];if(f&&d){d=d.slice(0);f=0;for(var l=d.length;f<l;f++){var k=d[f];if(b||e.test(k.namespace)){a.handler=k.handler;a.data=k.data;a.handleObj=k;k=k.handler.apply(this,h);if(k!==B){a.result=k;if(k===false){a.preventDefault();a.stopPropagation()}}if(a.isImmediatePropagationStopped())break}}}return a.result},props:"altKey attrChange attrName bubbles button cancelable charCode clientX clientY ctrlKey currentTarget data detail eventPhase fromElement handler keyCode layerX layerY metaKey newValue offsetX offsetY pageX pageY prevValue relatedNode relatedTarget screenX screenY shiftKey srcElement target toElement view wheelDelta which".split(" "),
fix:function(a){if(a[c.expando])return a;var b=a;a=c.Event(b);for(var d=this.props.length,e;d;){e=this.props[--d];a[e]=b[e]}if(!a.target)a.target=a.srcElement||t;if(a.target.nodeType===3)a.target=a.target.parentNode;if(!a.relatedTarget&&a.fromElement)a.relatedTarget=a.fromElement===a.target?a.toElement:a.fromElement;if(a.pageX==null&&a.clientX!=null){b=t.documentElement;d=t.body;a.pageX=a.clientX+(b&&b.scrollLeft||d&&d.scrollLeft||0)-(b&&b.clientLeft||d&&d.clientLeft||0);a.pageY=a.clientY+(b&&b.scrollTop||
d&&d.scrollTop||0)-(b&&b.clientTop||d&&d.clientTop||0)}if(a.which==null&&(a.charCode!=null||a.keyCode!=null))a.which=a.charCode!=null?a.charCode:a.keyCode;if(!a.metaKey&&a.ctrlKey)a.metaKey=a.ctrlKey;if(!a.which&&a.button!==B)a.which=a.button&1?1:a.button&2?3:a.button&4?2:0;return a},guid:1E8,proxy:c.proxy,special:{ready:{setup:c.bindReady,teardown:c.noop},live:{add:function(a){c.event.add(this,Y(a.origType,a.selector),c.extend({},a,{handler:Ka,guid:a.handler.guid}))},remove:function(a){c.event.remove(this,
Y(a.origType,a.selector),a)}},beforeunload:{setup:function(a,b,d){if(c.isWindow(this))this.onbeforeunload=d},teardown:function(a,b){if(this.onbeforeunload===b)this.onbeforeunload=null}}}};c.removeEvent=t.removeEventListener?function(a,b,d){a.removeEventListener&&a.removeEventListener(b,d,false)}:function(a,b,d){a.detachEvent&&a.detachEvent("on"+b,d)};c.Event=function(a){if(!this.preventDefault)return new c.Event(a);if(a&&a.type){this.originalEvent=a;this.type=a.type}else this.type=a;this.timeStamp=
c.now();this[c.expando]=true};c.Event.prototype={preventDefault:function(){this.isDefaultPrevented=ca;var a=this.originalEvent;if(a)if(a.preventDefault)a.preventDefault();else a.returnValue=false},stopPropagation:function(){this.isPropagationStopped=ca;var a=this.originalEvent;if(a){a.stopPropagation&&a.stopPropagation();a.cancelBubble=true}},stopImmediatePropagation:function(){this.isImmediatePropagationStopped=ca;this.stopPropagation()},isDefaultPrevented:U,isPropagationStopped:U,isImmediatePropagationStopped:U};
var va=function(a){var b=a.relatedTarget;try{for(;b&&b!==this;)b=b.parentNode;if(b!==this){a.type=a.data;c.event.handle.apply(this,arguments)}}catch(d){}},wa=function(a){a.type=a.data;c.event.handle.apply(this,arguments)};c.each({mouseenter:"mouseover",mouseleave:"mouseout"},function(a,b){c.event.special[a]={setup:function(d){c.event.add(this,b,d&&d.selector?wa:va,a)},teardown:function(d){c.event.remove(this,b,d&&d.selector?wa:va)}}});if(!c.support.submitBubbles)c.event.special.submit={setup:function(){if(this.nodeName.toLowerCase()!==
"form"){c.event.add(this,"click.specialSubmit",function(a){var b=a.target,d=b.type;if((d==="submit"||d==="image")&&c(b).closest("form").length){a.liveFired=B;return la("submit",this,arguments)}});c.event.add(this,"keypress.specialSubmit",function(a){var b=a.target,d=b.type;if((d==="text"||d==="password")&&c(b).closest("form").length&&a.keyCode===13){a.liveFired=B;return la("submit",this,arguments)}})}else return false},teardown:function(){c.event.remove(this,".specialSubmit")}};if(!c.support.changeBubbles){var V,
xa=function(a){var b=a.type,d=a.value;if(b==="radio"||b==="checkbox")d=a.checked;else if(b==="select-multiple")d=a.selectedIndex>-1?c.map(a.options,function(e){return e.selected}).join("-"):"";else if(a.nodeName.toLowerCase()==="select")d=a.selectedIndex;return d},Z=function(a,b){var d=a.target,e,f;if(!(!ia.test(d.nodeName)||d.readOnly)){e=c.data(d,"_change_data");f=xa(d);if(a.type!=="focusout"||d.type!=="radio")c.data(d,"_change_data",f);if(!(e===B||f===e))if(e!=null||f){a.type="change";a.liveFired=
B;return c.event.trigger(a,b,d)}}};c.event.special.change={filters:{focusout:Z,beforedeactivate:Z,click:function(a){var b=a.target,d=b.type;if(d==="radio"||d==="checkbox"||b.nodeName.toLowerCase()==="select")return Z.call(this,a)},keydown:function(a){var b=a.target,d=b.type;if(a.keyCode===13&&b.nodeName.toLowerCase()!=="textarea"||a.keyCode===32&&(d==="checkbox"||d==="radio")||d==="select-multiple")return Z.call(this,a)},beforeactivate:function(a){a=a.target;c.data(a,"_change_data",xa(a))}},setup:function(){if(this.type===
"file")return false;for(var a in V)c.event.add(this,a+".specialChange",V[a]);return ia.test(this.nodeName)},teardown:function(){c.event.remove(this,".specialChange");return ia.test(this.nodeName)}};V=c.event.special.change.filters;V.focus=V.beforeactivate}t.addEventListener&&c.each({focus:"focusin",blur:"focusout"},function(a,b){function d(e){e=c.event.fix(e);e.type=b;return c.event.trigger(e,null,e.target)}c.event.special[b]={setup:function(){ua[b]++===0&&t.addEventListener(a,d,true)},teardown:function(){--ua[b]===
0&&t.removeEventListener(a,d,true)}}});c.each(["bind","one"],function(a,b){c.fn[b]=function(d,e,f){if(typeof d==="object"){for(var h in d)this[b](h,e,d[h],f);return this}if(c.isFunction(e)||e===false){f=e;e=B}var l=b==="one"?c.proxy(f,function(o){c(this).unbind(o,l);return f.apply(this,arguments)}):f;if(d==="unload"&&b!=="one")this.one(d,e,f);else{h=0;for(var k=this.length;h<k;h++)c.event.add(this[h],d,l,e)}return this}});c.fn.extend({unbind:function(a,b){if(typeof a==="object"&&!a.preventDefault)for(var d in a)this.unbind(d,
a[d]);else{d=0;for(var e=this.length;d<e;d++)c.event.remove(this[d],a,b)}return this},delegate:function(a,b,d,e){return this.live(b,d,e,a)},undelegate:function(a,b,d){return arguments.length===0?this.unbind("live"):this.die(b,null,d,a)},trigger:function(a,b){return this.each(function(){c.event.trigger(a,b,this)})},triggerHandler:function(a,b){if(this[0]){var d=c.Event(a);d.preventDefault();d.stopPropagation();c.event.trigger(d,b,this[0]);return d.result}},toggle:function(a){for(var b=arguments,d=
1;d<b.length;)c.proxy(a,b[d++]);return this.click(c.proxy(a,function(e){var f=(c.data(this,"lastToggle"+a.guid)||0)%d;c.data(this,"lastToggle"+a.guid,f+1);e.preventDefault();return b[f].apply(this,arguments)||false}))},hover:function(a,b){return this.mouseenter(a).mouseleave(b||a)}});var ya={focus:"focusin",blur:"focusout",mouseenter:"mouseover",mouseleave:"mouseout"};c.each(["live","die"],function(a,b){c.fn[b]=function(d,e,f,h){var l,k=0,o,x,r=h||this.selector;h=h?this:c(this.context);if(typeof d===
"object"&&!d.preventDefault){for(l in d)h[b](l,e,d[l],r);return this}if(c.isFunction(e)){f=e;e=B}for(d=(d||"").split(" ");(l=d[k++])!=null;){o=X.exec(l);x="";if(o){x=o[0];l=l.replace(X,"")}if(l==="hover")d.push("mouseenter"+x,"mouseleave"+x);else{o=l;if(l==="focus"||l==="blur"){d.push(ya[l]+x);l+=x}else l=(ya[l]||l)+x;if(b==="live"){x=0;for(var A=h.length;x<A;x++)c.event.add(h[x],"live."+Y(l,r),{data:e,selector:r,handler:f,origType:l,origHandler:f,preType:o})}else h.unbind("live."+Y(l,r),f)}}return this}});
c.each("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error".split(" "),function(a,b){c.fn[b]=function(d,e){if(e==null){e=d;d=null}return arguments.length>0?this.bind(b,d,e):this.trigger(b)};if(c.attrFn)c.attrFn[b]=true});E.attachEvent&&!E.addEventListener&&c(E).bind("unload",function(){for(var a in c.cache)if(c.cache[a].handle)try{c.event.remove(c.cache[a].handle.elem)}catch(b){}});
(function(){function a(g,i,n,m,p,q){p=0;for(var u=m.length;p<u;p++){var y=m[p];if(y){var F=false;for(y=y[g];y;){if(y.sizcache===n){F=m[y.sizset];break}if(y.nodeType===1&&!q){y.sizcache=n;y.sizset=p}if(y.nodeName.toLowerCase()===i){F=y;break}y=y[g]}m[p]=F}}}function b(g,i,n,m,p,q){p=0;for(var u=m.length;p<u;p++){var y=m[p];if(y){var F=false;for(y=y[g];y;){if(y.sizcache===n){F=m[y.sizset];break}if(y.nodeType===1){if(!q){y.sizcache=n;y.sizset=p}if(typeof i!=="string"){if(y===i){F=true;break}}else if(k.filter(i,
[y]).length>0){F=y;break}}y=y[g]}m[p]=F}}}var d=/((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^\[\]]*\]|['"][^'"]*['"]|[^\[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g,e=0,f=Object.prototype.toString,h=false,l=true;[0,0].sort(function(){l=false;return 0});var k=function(g,i,n,m){n=n||[];var p=i=i||t;if(i.nodeType!==1&&i.nodeType!==9)return[];if(!g||typeof g!=="string")return n;var q,u,y,F,M,N=true,O=k.isXML(i),D=[],R=g;do{d.exec("");if(q=d.exec(R)){R=q[3];D.push(q[1]);if(q[2]){F=q[3];
break}}}while(q);if(D.length>1&&x.exec(g))if(D.length===2&&o.relative[D[0]])u=L(D[0]+D[1],i);else for(u=o.relative[D[0]]?[i]:k(D.shift(),i);D.length;){g=D.shift();if(o.relative[g])g+=D.shift();u=L(g,u)}else{if(!m&&D.length>1&&i.nodeType===9&&!O&&o.match.ID.test(D[0])&&!o.match.ID.test(D[D.length-1])){q=k.find(D.shift(),i,O);i=q.expr?k.filter(q.expr,q.set)[0]:q.set[0]}if(i){q=m?{expr:D.pop(),set:C(m)}:k.find(D.pop(),D.length===1&&(D[0]==="~"||D[0]==="+")&&i.parentNode?i.parentNode:i,O);u=q.expr?k.filter(q.expr,
q.set):q.set;if(D.length>0)y=C(u);else N=false;for(;D.length;){q=M=D.pop();if(o.relative[M])q=D.pop();else M="";if(q==null)q=i;o.relative[M](y,q,O)}}else y=[]}y||(y=u);y||k.error(M||g);if(f.call(y)==="[object Array]")if(N)if(i&&i.nodeType===1)for(g=0;y[g]!=null;g++){if(y[g]&&(y[g]===true||y[g].nodeType===1&&k.contains(i,y[g])))n.push(u[g])}else for(g=0;y[g]!=null;g++)y[g]&&y[g].nodeType===1&&n.push(u[g]);else n.push.apply(n,y);else C(y,n);if(F){k(F,p,n,m);k.uniqueSort(n)}return n};k.uniqueSort=function(g){if(w){h=
l;g.sort(w);if(h)for(var i=1;i<g.length;i++)g[i]===g[i-1]&&g.splice(i--,1)}return g};k.matches=function(g,i){return k(g,null,null,i)};k.matchesSelector=function(g,i){return k(i,null,null,[g]).length>0};k.find=function(g,i,n){var m;if(!g)return[];for(var p=0,q=o.order.length;p<q;p++){var u,y=o.order[p];if(u=o.leftMatch[y].exec(g)){var F=u[1];u.splice(1,1);if(F.substr(F.length-1)!=="\\"){u[1]=(u[1]||"").replace(/\\/g,"");m=o.find[y](u,i,n);if(m!=null){g=g.replace(o.match[y],"");break}}}}m||(m=i.getElementsByTagName("*"));
return{set:m,expr:g}};k.filter=function(g,i,n,m){for(var p,q,u=g,y=[],F=i,M=i&&i[0]&&k.isXML(i[0]);g&&i.length;){for(var N in o.filter)if((p=o.leftMatch[N].exec(g))!=null&&p[2]){var O,D,R=o.filter[N];D=p[1];q=false;p.splice(1,1);if(D.substr(D.length-1)!=="\\"){if(F===y)y=[];if(o.preFilter[N])if(p=o.preFilter[N](p,F,n,y,m,M)){if(p===true)continue}else q=O=true;if(p)for(var j=0;(D=F[j])!=null;j++)if(D){O=R(D,p,j,F);var s=m^!!O;if(n&&O!=null)if(s)q=true;else F[j]=false;else if(s){y.push(D);q=true}}if(O!==
B){n||(F=y);g=g.replace(o.match[N],"");if(!q)return[];break}}}if(g===u)if(q==null)k.error(g);else break;u=g}return F};k.error=function(g){throw"Syntax error, unrecognized expression: "+g;};var o=k.selectors={order:["ID","NAME","TAG"],match:{ID:/#((?:[\w\u00c0-\uFFFF\-]|\\.)+)/,CLASS:/\.((?:[\w\u00c0-\uFFFF\-]|\\.)+)/,NAME:/\[name=['"]*((?:[\w\u00c0-\uFFFF\-]|\\.)+)['"]*\]/,ATTR:/\[\s*((?:[\w\u00c0-\uFFFF\-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\]/,TAG:/^((?:[\w\u00c0-\uFFFF\*\-]|\\.)+)/,CHILD:/:(only|nth|last|first)-child(?:\((even|odd|[\dn+\-]*)\))?/,
POS:/:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^\-]|$)/,PSEUDO:/:((?:[\w\u00c0-\uFFFF\-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/},leftMatch:{},attrMap:{"class":"className","for":"htmlFor"},attrHandle:{href:function(g){return g.getAttribute("href")}},relative:{"+":function(g,i){var n=typeof i==="string",m=n&&!/\W/.test(i);n=n&&!m;if(m)i=i.toLowerCase();m=0;for(var p=g.length,q;m<p;m++)if(q=g[m]){for(;(q=q.previousSibling)&&q.nodeType!==1;);g[m]=n||q&&q.nodeName.toLowerCase()===
i?q||false:q===i}n&&k.filter(i,g,true)},">":function(g,i){var n,m=typeof i==="string",p=0,q=g.length;if(m&&!/\W/.test(i))for(i=i.toLowerCase();p<q;p++){if(n=g[p]){n=n.parentNode;g[p]=n.nodeName.toLowerCase()===i?n:false}}else{for(;p<q;p++)if(n=g[p])g[p]=m?n.parentNode:n.parentNode===i;m&&k.filter(i,g,true)}},"":function(g,i,n){var m,p=e++,q=b;if(typeof i==="string"&&!/\W/.test(i)){m=i=i.toLowerCase();q=a}q("parentNode",i,p,g,m,n)},"~":function(g,i,n){var m,p=e++,q=b;if(typeof i==="string"&&!/\W/.test(i)){m=
i=i.toLowerCase();q=a}q("previousSibling",i,p,g,m,n)}},find:{ID:function(g,i,n){if(typeof i.getElementById!=="undefined"&&!n)return(g=i.getElementById(g[1]))&&g.parentNode?[g]:[]},NAME:function(g,i){if(typeof i.getElementsByName!=="undefined"){for(var n=[],m=i.getElementsByName(g[1]),p=0,q=m.length;p<q;p++)m[p].getAttribute("name")===g[1]&&n.push(m[p]);return n.length===0?null:n}},TAG:function(g,i){return i.getElementsByTagName(g[1])}},preFilter:{CLASS:function(g,i,n,m,p,q){g=" "+g[1].replace(/\\/g,
"")+" ";if(q)return g;q=0;for(var u;(u=i[q])!=null;q++)if(u)if(p^(u.className&&(" "+u.className+" ").replace(/[\t\n]/g," ").indexOf(g)>=0))n||m.push(u);else if(n)i[q]=false;return false},ID:function(g){return g[1].replace(/\\/g,"")},TAG:function(g){return g[1].toLowerCase()},CHILD:function(g){if(g[1]==="nth"){var i=/(-?)(\d*)n((?:\+|-)?\d*)/.exec(g[2]==="even"&&"2n"||g[2]==="odd"&&"2n+1"||!/\D/.test(g[2])&&"0n+"+g[2]||g[2]);g[2]=i[1]+(i[2]||1)-0;g[3]=i[3]-0}g[0]=e++;return g},ATTR:function(g,i,n,
m,p,q){i=g[1].replace(/\\/g,"");if(!q&&o.attrMap[i])g[1]=o.attrMap[i];if(g[2]==="~=")g[4]=" "+g[4]+" ";return g},PSEUDO:function(g,i,n,m,p){if(g[1]==="not")if((d.exec(g[3])||"").length>1||/^\w/.test(g[3]))g[3]=k(g[3],null,null,i);else{g=k.filter(g[3],i,n,true^p);n||m.push.apply(m,g);return false}else if(o.match.POS.test(g[0])||o.match.CHILD.test(g[0]))return true;return g},POS:function(g){g.unshift(true);return g}},filters:{enabled:function(g){return g.disabled===false&&g.type!=="hidden"},disabled:function(g){return g.disabled===
true},checked:function(g){return g.checked===true},selected:function(g){return g.selected===true},parent:function(g){return!!g.firstChild},empty:function(g){return!g.firstChild},has:function(g,i,n){return!!k(n[3],g).length},header:function(g){return/h\d/i.test(g.nodeName)},text:function(g){return"text"===g.type},radio:function(g){return"radio"===g.type},checkbox:function(g){return"checkbox"===g.type},file:function(g){return"file"===g.type},password:function(g){return"password"===g.type},submit:function(g){return"submit"===
g.type},image:function(g){return"image"===g.type},reset:function(g){return"reset"===g.type},button:function(g){return"button"===g.type||g.nodeName.toLowerCase()==="button"},input:function(g){return/input|select|textarea|button/i.test(g.nodeName)}},setFilters:{first:function(g,i){return i===0},last:function(g,i,n,m){return i===m.length-1},even:function(g,i){return i%2===0},odd:function(g,i){return i%2===1},lt:function(g,i,n){return i<n[3]-0},gt:function(g,i,n){return i>n[3]-0},nth:function(g,i,n){return n[3]-
0===i},eq:function(g,i,n){return n[3]-0===i}},filter:{PSEUDO:function(g,i,n,m){var p=i[1],q=o.filters[p];if(q)return q(g,n,i,m);else if(p==="contains")return(g.textContent||g.innerText||k.getText([g])||"").indexOf(i[3])>=0;else if(p==="not"){i=i[3];n=0;for(m=i.length;n<m;n++)if(i[n]===g)return false;return true}else k.error("Syntax error, unrecognized expression: "+p)},CHILD:function(g,i){var n=i[1],m=g;switch(n){case "only":case "first":for(;m=m.previousSibling;)if(m.nodeType===1)return false;if(n===
"first")return true;m=g;case "last":for(;m=m.nextSibling;)if(m.nodeType===1)return false;return true;case "nth":n=i[2];var p=i[3];if(n===1&&p===0)return true;var q=i[0],u=g.parentNode;if(u&&(u.sizcache!==q||!g.nodeIndex)){var y=0;for(m=u.firstChild;m;m=m.nextSibling)if(m.nodeType===1)m.nodeIndex=++y;u.sizcache=q}m=g.nodeIndex-p;return n===0?m===0:m%n===0&&m/n>=0}},ID:function(g,i){return g.nodeType===1&&g.getAttribute("id")===i},TAG:function(g,i){return i==="*"&&g.nodeType===1||g.nodeName.toLowerCase()===
i},CLASS:function(g,i){return(" "+(g.className||g.getAttribute("class"))+" ").indexOf(i)>-1},ATTR:function(g,i){var n=i[1];n=o.attrHandle[n]?o.attrHandle[n](g):g[n]!=null?g[n]:g.getAttribute(n);var m=n+"",p=i[2],q=i[4];return n==null?p==="!=":p==="="?m===q:p==="*="?m.indexOf(q)>=0:p==="~="?(" "+m+" ").indexOf(q)>=0:!q?m&&n!==false:p==="!="?m!==q:p==="^="?m.indexOf(q)===0:p==="$="?m.substr(m.length-q.length)===q:p==="|="?m===q||m.substr(0,q.length+1)===q+"-":false},POS:function(g,i,n,m){var p=o.setFilters[i[2]];
if(p)return p(g,n,i,m)}}},x=o.match.POS,r=function(g,i){return"\\"+(i-0+1)},A;for(A in o.match){o.match[A]=RegExp(o.match[A].source+/(?![^\[]*\])(?![^\(]*\))/.source);o.leftMatch[A]=RegExp(/(^(?:.|\r|\n)*?)/.source+o.match[A].source.replace(/\\(\d+)/g,r))}var C=function(g,i){g=Array.prototype.slice.call(g,0);if(i){i.push.apply(i,g);return i}return g};try{Array.prototype.slice.call(t.documentElement.childNodes,0)}catch(J){C=function(g,i){var n=0,m=i||[];if(f.call(g)==="[object Array]")Array.prototype.push.apply(m,
g);else if(typeof g.length==="number")for(var p=g.length;n<p;n++)m.push(g[n]);else for(;g[n];n++)m.push(g[n]);return m}}var w,I;if(t.documentElement.compareDocumentPosition)w=function(g,i){if(g===i){h=true;return 0}if(!g.compareDocumentPosition||!i.compareDocumentPosition)return g.compareDocumentPosition?-1:1;return g.compareDocumentPosition(i)&4?-1:1};else{w=function(g,i){var n,m,p=[],q=[];n=g.parentNode;m=i.parentNode;var u=n;if(g===i){h=true;return 0}else if(n===m)return I(g,i);else if(n){if(!m)return 1}else return-1;
for(;u;){p.unshift(u);u=u.parentNode}for(u=m;u;){q.unshift(u);u=u.parentNode}n=p.length;m=q.length;for(u=0;u<n&&u<m;u++)if(p[u]!==q[u])return I(p[u],q[u]);return u===n?I(g,q[u],-1):I(p[u],i,1)};I=function(g,i,n){if(g===i)return n;for(g=g.nextSibling;g;){if(g===i)return-1;g=g.nextSibling}return 1}}k.getText=function(g){for(var i="",n,m=0;g[m];m++){n=g[m];if(n.nodeType===3||n.nodeType===4)i+=n.nodeValue;else if(n.nodeType!==8)i+=k.getText(n.childNodes)}return i};(function(){var g=t.createElement("div"),
i="script"+(new Date).getTime(),n=t.documentElement;g.innerHTML="<a name='"+i+"'/>";n.insertBefore(g,n.firstChild);if(t.getElementById(i)){o.find.ID=function(m,p,q){if(typeof p.getElementById!=="undefined"&&!q)return(p=p.getElementById(m[1]))?p.id===m[1]||typeof p.getAttributeNode!=="undefined"&&p.getAttributeNode("id").nodeValue===m[1]?[p]:B:[]};o.filter.ID=function(m,p){var q=typeof m.getAttributeNode!=="undefined"&&m.getAttributeNode("id");return m.nodeType===1&&q&&q.nodeValue===p}}n.removeChild(g);
n=g=null})();(function(){var g=t.createElement("div");g.appendChild(t.createComment(""));if(g.getElementsByTagName("*").length>0)o.find.TAG=function(i,n){var m=n.getElementsByTagName(i[1]);if(i[1]==="*"){for(var p=[],q=0;m[q];q++)m[q].nodeType===1&&p.push(m[q]);m=p}return m};g.innerHTML="<a href='#'></a>";if(g.firstChild&&typeof g.firstChild.getAttribute!=="undefined"&&g.firstChild.getAttribute("href")!=="#")o.attrHandle.href=function(i){return i.getAttribute("href",2)};g=null})();t.querySelectorAll&&
function(){var g=k,i=t.createElement("div");i.innerHTML="<p class='TEST'></p>";if(!(i.querySelectorAll&&i.querySelectorAll(".TEST").length===0)){k=function(m,p,q,u){p=p||t;m=m.replace(/\=\s*([^'"\]]*)\s*\]/g,"='$1']");if(!u&&!k.isXML(p))if(p.nodeType===9)try{return C(p.querySelectorAll(m),q)}catch(y){}else if(p.nodeType===1&&p.nodeName.toLowerCase()!=="object"){var F=p.getAttribute("id"),M=F||"__sizzle__";F||p.setAttribute("id",M);try{return C(p.querySelectorAll("#"+M+" "+m),q)}catch(N){}finally{F||
p.removeAttribute("id")}}return g(m,p,q,u)};for(var n in g)k[n]=g[n];i=null}}();(function(){var g=t.documentElement,i=g.matchesSelector||g.mozMatchesSelector||g.webkitMatchesSelector||g.msMatchesSelector,n=false;try{i.call(t.documentElement,"[test!='']:sizzle")}catch(m){n=true}if(i)k.matchesSelector=function(p,q){q=q.replace(/\=\s*([^'"\]]*)\s*\]/g,"='$1']");if(!k.isXML(p))try{if(n||!o.match.PSEUDO.test(q)&&!/!=/.test(q))return i.call(p,q)}catch(u){}return k(q,null,null,[p]).length>0}})();(function(){var g=
t.createElement("div");g.innerHTML="<div class='test e'></div><div class='test'></div>";if(!(!g.getElementsByClassName||g.getElementsByClassName("e").length===0)){g.lastChild.className="e";if(g.getElementsByClassName("e").length!==1){o.order.splice(1,0,"CLASS");o.find.CLASS=function(i,n,m){if(typeof n.getElementsByClassName!=="undefined"&&!m)return n.getElementsByClassName(i[1])};g=null}}})();k.contains=t.documentElement.contains?function(g,i){return g!==i&&(g.contains?g.contains(i):true)}:t.documentElement.compareDocumentPosition?
function(g,i){return!!(g.compareDocumentPosition(i)&16)}:function(){return false};k.isXML=function(g){return(g=(g?g.ownerDocument||g:0).documentElement)?g.nodeName!=="HTML":false};var L=function(g,i){for(var n,m=[],p="",q=i.nodeType?[i]:i;n=o.match.PSEUDO.exec(g);){p+=n[0];g=g.replace(o.match.PSEUDO,"")}g=o.relative[g]?g+"*":g;n=0;for(var u=q.length;n<u;n++)k(g,q[n],m);return k.filter(p,m)};c.find=k;c.expr=k.selectors;c.expr[":"]=c.expr.filters;c.unique=k.uniqueSort;c.text=k.getText;c.isXMLDoc=k.isXML;
c.contains=k.contains})();var Za=/Until$/,$a=/^(?:parents|prevUntil|prevAll)/,ab=/,/,Na=/^.[^:#\[\.,]*$/,bb=Array.prototype.slice,cb=c.expr.match.POS;c.fn.extend({find:function(a){for(var b=this.pushStack("","find",a),d=0,e=0,f=this.length;e<f;e++){d=b.length;c.find(a,this[e],b);if(e>0)for(var h=d;h<b.length;h++)for(var l=0;l<d;l++)if(b[l]===b[h]){b.splice(h--,1);break}}return b},has:function(a){var b=c(a);return this.filter(function(){for(var d=0,e=b.length;d<e;d++)if(c.contains(this,b[d]))return true})},
not:function(a){return this.pushStack(ma(this,a,false),"not",a)},filter:function(a){return this.pushStack(ma(this,a,true),"filter",a)},is:function(a){return!!a&&c.filter(a,this).length>0},closest:function(a,b){var d=[],e,f,h=this[0];if(c.isArray(a)){var l,k={},o=1;if(h&&a.length){e=0;for(f=a.length;e<f;e++){l=a[e];k[l]||(k[l]=c.expr.match.POS.test(l)?c(l,b||this.context):l)}for(;h&&h.ownerDocument&&h!==b;){for(l in k){e=k[l];if(e.jquery?e.index(h)>-1:c(h).is(e))d.push({selector:l,elem:h,level:o})}h=
h.parentNode;o++}}return d}l=cb.test(a)?c(a,b||this.context):null;e=0;for(f=this.length;e<f;e++)for(h=this[e];h;)if(l?l.index(h)>-1:c.find.matchesSelector(h,a)){d.push(h);break}else{h=h.parentNode;if(!h||!h.ownerDocument||h===b)break}d=d.length>1?c.unique(d):d;return this.pushStack(d,"closest",a)},index:function(a){if(!a||typeof a==="string")return c.inArray(this[0],a?c(a):this.parent().children());return c.inArray(a.jquery?a[0]:a,this)},add:function(a,b){var d=typeof a==="string"?c(a,b||this.context):
c.makeArray(a),e=c.merge(this.get(),d);return this.pushStack(!d[0]||!d[0].parentNode||d[0].parentNode.nodeType===11||!e[0]||!e[0].parentNode||e[0].parentNode.nodeType===11?e:c.unique(e))},andSelf:function(){return this.add(this.prevObject)}});c.each({parent:function(a){return(a=a.parentNode)&&a.nodeType!==11?a:null},parents:function(a){return c.dir(a,"parentNode")},parentsUntil:function(a,b,d){return c.dir(a,"parentNode",d)},next:function(a){return c.nth(a,2,"nextSibling")},prev:function(a){return c.nth(a,
2,"previousSibling")},nextAll:function(a){return c.dir(a,"nextSibling")},prevAll:function(a){return c.dir(a,"previousSibling")},nextUntil:function(a,b,d){return c.dir(a,"nextSibling",d)},prevUntil:function(a,b,d){return c.dir(a,"previousSibling",d)},siblings:function(a){return c.sibling(a.parentNode.firstChild,a)},children:function(a){return c.sibling(a.firstChild)},contents:function(a){return c.nodeName(a,"iframe")?a.contentDocument||a.contentWindow.document:c.makeArray(a.childNodes)}},function(a,
b){c.fn[a]=function(d,e){var f=c.map(this,b,d);Za.test(a)||(e=d);if(e&&typeof e==="string")f=c.filter(e,f);f=this.length>1?c.unique(f):f;if((this.length>1||ab.test(e))&&$a.test(a))f=f.reverse();return this.pushStack(f,a,bb.call(arguments).join(","))}});c.extend({filter:function(a,b,d){if(d)a=":not("+a+")";return b.length===1?c.find.matchesSelector(b[0],a)?[b[0]]:[]:c.find.matches(a,b)},dir:function(a,b,d){var e=[];for(a=a[b];a&&a.nodeType!==9&&(d===B||a.nodeType!==1||!c(a).is(d));){a.nodeType===1&&
e.push(a);a=a[b]}return e},nth:function(a,b,d){b=b||1;for(var e=0;a;a=a[d])if(a.nodeType===1&&++e===b)break;return a},sibling:function(a,b){for(var d=[];a;a=a.nextSibling)a.nodeType===1&&a!==b&&d.push(a);return d}});var za=/ jQuery\d+="(?:\d+|null)"/g,$=/^\s+/,Aa=/<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/ig,Ba=/<([\w:]+)/,db=/<tbody/i,eb=/<|&#?\w+;/,Ca=/<(?:script|object|embed|option|style)/i,Da=/checked\s*(?:[^=]|=\s*.checked.)/i,fb=/\=([^="'>\s]+\/)>/g,P={option:[1,
"<select multiple='multiple'>","</select>"],legend:[1,"<fieldset>","</fieldset>"],thead:[1,"<table>","</table>"],tr:[2,"<table><tbody>","</tbody></table>"],td:[3,"<table><tbody><tr>","</tr></tbody></table>"],col:[2,"<table><tbody></tbody><colgroup>","</colgroup></table>"],area:[1,"<map>","</map>"],_default:[0,"",""]};P.optgroup=P.option;P.tbody=P.tfoot=P.colgroup=P.caption=P.thead;P.th=P.td;if(!c.support.htmlSerialize)P._default=[1,"div<div>","</div>"];c.fn.extend({text:function(a){if(c.isFunction(a))return this.each(function(b){var d=
c(this);d.text(a.call(this,b,d.text()))});if(typeof a!=="object"&&a!==B)return this.empty().append((this[0]&&this[0].ownerDocument||t).createTextNode(a));return c.text(this)},wrapAll:function(a){if(c.isFunction(a))return this.each(function(d){c(this).wrapAll(a.call(this,d))});if(this[0]){var b=c(a,this[0].ownerDocument).eq(0).clone(true);this[0].parentNode&&b.insertBefore(this[0]);b.map(function(){for(var d=this;d.firstChild&&d.firstChild.nodeType===1;)d=d.firstChild;return d}).append(this)}return this},
wrapInner:function(a){if(c.isFunction(a))return this.each(function(b){c(this).wrapInner(a.call(this,b))});return this.each(function(){var b=c(this),d=b.contents();d.length?d.wrapAll(a):b.append(a)})},wrap:function(a){return this.each(function(){c(this).wrapAll(a)})},unwrap:function(){return this.parent().each(function(){c.nodeName(this,"body")||c(this).replaceWith(this.childNodes)}).end()},append:function(){return this.domManip(arguments,true,function(a){this.nodeType===1&&this.appendChild(a)})},
prepend:function(){return this.domManip(arguments,true,function(a){this.nodeType===1&&this.insertBefore(a,this.firstChild)})},before:function(){if(this[0]&&this[0].parentNode)return this.domManip(arguments,false,function(b){this.parentNode.insertBefore(b,this)});else if(arguments.length){var a=c(arguments[0]);a.push.apply(a,this.toArray());return this.pushStack(a,"before",arguments)}},after:function(){if(this[0]&&this[0].parentNode)return this.domManip(arguments,false,function(b){this.parentNode.insertBefore(b,
this.nextSibling)});else if(arguments.length){var a=this.pushStack(this,"after",arguments);a.push.apply(a,c(arguments[0]).toArray());return a}},remove:function(a,b){for(var d=0,e;(e=this[d])!=null;d++)if(!a||c.filter(a,[e]).length){if(!b&&e.nodeType===1){c.cleanData(e.getElementsByTagName("*"));c.cleanData([e])}e.parentNode&&e.parentNode.removeChild(e)}return this},empty:function(){for(var a=0,b;(b=this[a])!=null;a++)for(b.nodeType===1&&c.cleanData(b.getElementsByTagName("*"));b.firstChild;)b.removeChild(b.firstChild);
return this},clone:function(a){var b=this.map(function(){if(!c.support.noCloneEvent&&!c.isXMLDoc(this)){var d=this.outerHTML,e=this.ownerDocument;if(!d){d=e.createElement("div");d.appendChild(this.cloneNode(true));d=d.innerHTML}return c.clean([d.replace(za,"").replace(fb,'="$1">').replace($,"")],e)[0]}else return this.cloneNode(true)});if(a===true){na(this,b);na(this.find("*"),b.find("*"))}return b},html:function(a){if(a===B)return this[0]&&this[0].nodeType===1?this[0].innerHTML.replace(za,""):null;
else if(typeof a==="string"&&!Ca.test(a)&&(c.support.leadingWhitespace||!$.test(a))&&!P[(Ba.exec(a)||["",""])[1].toLowerCase()]){a=a.replace(Aa,"<$1></$2>");try{for(var b=0,d=this.length;b<d;b++)if(this[b].nodeType===1){c.cleanData(this[b].getElementsByTagName("*"));this[b].innerHTML=a}}catch(e){this.empty().append(a)}}else c.isFunction(a)?this.each(function(f){var h=c(this);h.html(a.call(this,f,h.html()))}):this.empty().append(a);return this},replaceWith:function(a){if(this[0]&&this[0].parentNode){if(c.isFunction(a))return this.each(function(b){var d=
c(this),e=d.html();d.replaceWith(a.call(this,b,e))});if(typeof a!=="string")a=c(a).detach();return this.each(function(){var b=this.nextSibling,d=this.parentNode;c(this).remove();b?c(b).before(a):c(d).append(a)})}else return this.pushStack(c(c.isFunction(a)?a():a),"replaceWith",a)},detach:function(a){return this.remove(a,true)},domManip:function(a,b,d){var e,f,h,l=a[0],k=[];if(!c.support.checkClone&&arguments.length===3&&typeof l==="string"&&Da.test(l))return this.each(function(){c(this).domManip(a,
b,d,true)});if(c.isFunction(l))return this.each(function(x){var r=c(this);a[0]=l.call(this,x,b?r.html():B);r.domManip(a,b,d)});if(this[0]){e=l&&l.parentNode;e=c.support.parentNode&&e&&e.nodeType===11&&e.childNodes.length===this.length?{fragment:e}:c.buildFragment(a,this,k);h=e.fragment;if(f=h.childNodes.length===1?h=h.firstChild:h.firstChild){b=b&&c.nodeName(f,"tr");f=0;for(var o=this.length;f<o;f++)d.call(b?c.nodeName(this[f],"table")?this[f].getElementsByTagName("tbody")[0]||this[f].appendChild(this[f].ownerDocument.createElement("tbody")):
this[f]:this[f],f>0||e.cacheable||this.length>1?h.cloneNode(true):h)}k.length&&c.each(k,Oa)}return this}});c.buildFragment=function(a,b,d){var e,f,h;b=b&&b[0]?b[0].ownerDocument||b[0]:t;if(a.length===1&&typeof a[0]==="string"&&a[0].length<512&&b===t&&!Ca.test(a[0])&&(c.support.checkClone||!Da.test(a[0]))){f=true;if(h=c.fragments[a[0]])if(h!==1)e=h}if(!e){e=b.createDocumentFragment();c.clean(a,b,e,d)}if(f)c.fragments[a[0]]=h?e:1;return{fragment:e,cacheable:f}};c.fragments={};c.each({appendTo:"append",
prependTo:"prepend",insertBefore:"before",insertAfter:"after",replaceAll:"replaceWith"},function(a,b){c.fn[a]=function(d){var e=[];d=c(d);var f=this.length===1&&this[0].parentNode;if(f&&f.nodeType===11&&f.childNodes.length===1&&d.length===1){d[b](this[0]);return this}else{f=0;for(var h=d.length;f<h;f++){var l=(f>0?this.clone(true):this).get();c(d[f])[b](l);e=e.concat(l)}return this.pushStack(e,a,d.selector)}}});c.extend({clean:function(a,b,d,e){b=b||t;if(typeof b.createElement==="undefined")b=b.ownerDocument||
b[0]&&b[0].ownerDocument||t;for(var f=[],h=0,l;(l=a[h])!=null;h++){if(typeof l==="number")l+="";if(l){if(typeof l==="string"&&!eb.test(l))l=b.createTextNode(l);else if(typeof l==="string"){l=l.replace(Aa,"<$1></$2>");var k=(Ba.exec(l)||["",""])[1].toLowerCase(),o=P[k]||P._default,x=o[0],r=b.createElement("div");for(r.innerHTML=o[1]+l+o[2];x--;)r=r.lastChild;if(!c.support.tbody){x=db.test(l);k=k==="table"&&!x?r.firstChild&&r.firstChild.childNodes:o[1]==="<table>"&&!x?r.childNodes:[];for(o=k.length-
1;o>=0;--o)c.nodeName(k[o],"tbody")&&!k[o].childNodes.length&&k[o].parentNode.removeChild(k[o])}!c.support.leadingWhitespace&&$.test(l)&&r.insertBefore(b.createTextNode($.exec(l)[0]),r.firstChild);l=r.childNodes}if(l.nodeType)f.push(l);else f=c.merge(f,l)}}if(d)for(h=0;f[h];h++)if(e&&c.nodeName(f[h],"script")&&(!f[h].type||f[h].type.toLowerCase()==="text/javascript"))e.push(f[h].parentNode?f[h].parentNode.removeChild(f[h]):f[h]);else{f[h].nodeType===1&&f.splice.apply(f,[h+1,0].concat(c.makeArray(f[h].getElementsByTagName("script"))));
d.appendChild(f[h])}return f},cleanData:function(a){for(var b,d,e=c.cache,f=c.event.special,h=c.support.deleteExpando,l=0,k;(k=a[l])!=null;l++)if(!(k.nodeName&&c.noData[k.nodeName.toLowerCase()]))if(d=k[c.expando]){if((b=e[d])&&b.events)for(var o in b.events)f[o]?c.event.remove(k,o):c.removeEvent(k,o,b.handle);if(h)delete k[c.expando];else k.removeAttribute&&k.removeAttribute(c.expando);delete e[d]}}});var Ea=/alpha\([^)]*\)/i,gb=/opacity=([^)]*)/,hb=/-([a-z])/ig,ib=/([A-Z])/g,Fa=/^-?\d+(?:px)?$/i,
jb=/^-?\d/,kb={position:"absolute",visibility:"hidden",display:"block"},Pa=["Left","Right"],Qa=["Top","Bottom"],W,Ga,aa,lb=function(a,b){return b.toUpperCase()};c.fn.css=function(a,b){if(arguments.length===2&&b===B)return this;return c.access(this,a,b,true,function(d,e,f){return f!==B?c.style(d,e,f):c.css(d,e)})};c.extend({cssHooks:{opacity:{get:function(a,b){if(b){var d=W(a,"opacity","opacity");return d===""?"1":d}else return a.style.opacity}}},cssNumber:{zIndex:true,fontWeight:true,opacity:true,
zoom:true,lineHeight:true},cssProps:{"float":c.support.cssFloat?"cssFloat":"styleFloat"},style:function(a,b,d,e){if(!(!a||a.nodeType===3||a.nodeType===8||!a.style)){var f,h=c.camelCase(b),l=a.style,k=c.cssHooks[h];b=c.cssProps[h]||h;if(d!==B){if(!(typeof d==="number"&&isNaN(d)||d==null)){if(typeof d==="number"&&!c.cssNumber[h])d+="px";if(!k||!("set"in k)||(d=k.set(a,d))!==B)try{l[b]=d}catch(o){}}}else{if(k&&"get"in k&&(f=k.get(a,false,e))!==B)return f;return l[b]}}},css:function(a,b,d){var e,f=c.camelCase(b),
h=c.cssHooks[f];b=c.cssProps[f]||f;if(h&&"get"in h&&(e=h.get(a,true,d))!==B)return e;else if(W)return W(a,b,f)},swap:function(a,b,d){var e={},f;for(f in b){e[f]=a.style[f];a.style[f]=b[f]}d.call(a);for(f in b)a.style[f]=e[f]},camelCase:function(a){return a.replace(hb,lb)}});c.curCSS=c.css;c.each(["height","width"],function(a,b){c.cssHooks[b]={get:function(d,e,f){var h;if(e){if(d.offsetWidth!==0)h=oa(d,b,f);else c.swap(d,kb,function(){h=oa(d,b,f)});if(h<=0){h=W(d,b,b);if(h==="0px"&&aa)h=aa(d,b,b);
if(h!=null)return h===""||h==="auto"?"0px":h}if(h<0||h==null){h=d.style[b];return h===""||h==="auto"?"0px":h}return typeof h==="string"?h:h+"px"}},set:function(d,e){if(Fa.test(e)){e=parseFloat(e);if(e>=0)return e+"px"}else return e}}});if(!c.support.opacity)c.cssHooks.opacity={get:function(a,b){return gb.test((b&&a.currentStyle?a.currentStyle.filter:a.style.filter)||"")?parseFloat(RegExp.$1)/100+"":b?"1":""},set:function(a,b){var d=a.style;d.zoom=1;var e=c.isNaN(b)?"":"alpha(opacity="+b*100+")",f=
d.filter||"";d.filter=Ea.test(f)?f.replace(Ea,e):d.filter+" "+e}};if(t.defaultView&&t.defaultView.getComputedStyle)Ga=function(a,b,d){var e;d=d.replace(ib,"-$1").toLowerCase();if(!(b=a.ownerDocument.defaultView))return B;if(b=b.getComputedStyle(a,null)){e=b.getPropertyValue(d);if(e===""&&!c.contains(a.ownerDocument.documentElement,a))e=c.style(a,d)}return e};if(t.documentElement.currentStyle)aa=function(a,b){var d,e,f=a.currentStyle&&a.currentStyle[b],h=a.style;if(!Fa.test(f)&&jb.test(f)){d=h.left;
e=a.runtimeStyle.left;a.runtimeStyle.left=a.currentStyle.left;h.left=b==="fontSize"?"1em":f||0;f=h.pixelLeft+"px";h.left=d;a.runtimeStyle.left=e}return f===""?"auto":f};W=Ga||aa;if(c.expr&&c.expr.filters){c.expr.filters.hidden=function(a){var b=a.offsetHeight;return a.offsetWidth===0&&b===0||!c.support.reliableHiddenOffsets&&(a.style.display||c.css(a,"display"))==="none"};c.expr.filters.visible=function(a){return!c.expr.filters.hidden(a)}}var mb=c.now(),nb=/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,
ob=/^(?:select|textarea)/i,pb=/^(?:color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week)$/i,qb=/^(?:GET|HEAD)$/,Ra=/\[\]$/,T=/\=\?(&|$)/,ja=/\?/,rb=/([?&])_=[^&]*/,sb=/^(\w+:)?\/\/([^\/?#]+)/,tb=/%20/g,ub=/#.*$/,Ha=c.fn.load;c.fn.extend({load:function(a,b,d){if(typeof a!=="string"&&Ha)return Ha.apply(this,arguments);else if(!this.length)return this;var e=a.indexOf(" ");if(e>=0){var f=a.slice(e,a.length);a=a.slice(0,e)}e="GET";if(b)if(c.isFunction(b)){d=b;b=null}else if(typeof b===
"object"){b=c.param(b,c.ajaxSettings.traditional);e="POST"}var h=this;c.ajax({url:a,type:e,dataType:"html",data:b,complete:function(l,k){if(k==="success"||k==="notmodified")h.html(f?c("<div>").append(l.responseText.replace(nb,"")).find(f):l.responseText);d&&h.each(d,[l.responseText,k,l])}});return this},serialize:function(){return c.param(this.serializeArray())},serializeArray:function(){return this.map(function(){return this.elements?c.makeArray(this.elements):this}).filter(function(){return this.name&&
!this.disabled&&(this.checked||ob.test(this.nodeName)||pb.test(this.type))}).map(function(a,b){var d=c(this).val();return d==null?null:c.isArray(d)?c.map(d,function(e){return{name:b.name,value:e}}):{name:b.name,value:d}}).get()}});c.each("ajaxStart ajaxStop ajaxComplete ajaxError ajaxSuccess ajaxSend".split(" "),function(a,b){c.fn[b]=function(d){return this.bind(b,d)}});c.extend({get:function(a,b,d,e){if(c.isFunction(b)){e=e||d;d=b;b=null}return c.ajax({type:"GET",url:a,data:b,success:d,dataType:e})},
getScript:function(a,b){return c.get(a,null,b,"script")},getJSON:function(a,b,d){return c.get(a,b,d,"json")},post:function(a,b,d,e){if(c.isFunction(b)){e=e||d;d=b;b={}}return c.ajax({type:"POST",url:a,data:b,success:d,dataType:e})},ajaxSetup:function(a){c.extend(c.ajaxSettings,a)},ajaxSettings:{url:location.href,global:true,type:"GET",contentType:"application/x-www-form-urlencoded",processData:true,async:true,xhr:function(){return new E.XMLHttpRequest},accepts:{xml:"application/xml, text/xml",html:"text/html",
script:"text/javascript, application/javascript",json:"application/json, text/javascript",text:"text/plain",_default:"*/*"}},ajax:function(a){var b=c.extend(true,{},c.ajaxSettings,a),d,e,f,h=b.type.toUpperCase(),l=qb.test(h);b.url=b.url.replace(ub,"");b.context=a&&a.context!=null?a.context:b;if(b.data&&b.processData&&typeof b.data!=="string")b.data=c.param(b.data,b.traditional);if(b.dataType==="jsonp"){if(h==="GET")T.test(b.url)||(b.url+=(ja.test(b.url)?"&":"?")+(b.jsonp||"callback")+"=?");else if(!b.data||
!T.test(b.data))b.data=(b.data?b.data+"&":"")+(b.jsonp||"callback")+"=?";b.dataType="json"}if(b.dataType==="json"&&(b.data&&T.test(b.data)||T.test(b.url))){d=b.jsonpCallback||"jsonp"+mb++;if(b.data)b.data=(b.data+"").replace(T,"="+d+"$1");b.url=b.url.replace(T,"="+d+"$1");b.dataType="script";var k=E[d];E[d]=function(m){if(c.isFunction(k))k(m);else{E[d]=B;try{delete E[d]}catch(p){}}f=m;c.handleSuccess(b,w,e,f);c.handleComplete(b,w,e,f);r&&r.removeChild(A)}}if(b.dataType==="script"&&b.cache===null)b.cache=
false;if(b.cache===false&&l){var o=c.now(),x=b.url.replace(rb,"$1_="+o);b.url=x+(x===b.url?(ja.test(b.url)?"&":"?")+"_="+o:"")}if(b.data&&l)b.url+=(ja.test(b.url)?"&":"?")+b.data;b.global&&c.active++===0&&c.event.trigger("ajaxStart");o=(o=sb.exec(b.url))&&(o[1]&&o[1].toLowerCase()!==location.protocol||o[2].toLowerCase()!==location.host);if(b.dataType==="script"&&h==="GET"&&o){var r=t.getElementsByTagName("head")[0]||t.documentElement,A=t.createElement("script");if(b.scriptCharset)A.charset=b.scriptCharset;
A.src=b.url;if(!d){var C=false;A.onload=A.onreadystatechange=function(){if(!C&&(!this.readyState||this.readyState==="loaded"||this.readyState==="complete")){C=true;c.handleSuccess(b,w,e,f);c.handleComplete(b,w,e,f);A.onload=A.onreadystatechange=null;r&&A.parentNode&&r.removeChild(A)}}}r.insertBefore(A,r.firstChild);return B}var J=false,w=b.xhr();if(w){b.username?w.open(h,b.url,b.async,b.username,b.password):w.open(h,b.url,b.async);try{if(b.data!=null&&!l||a&&a.contentType)w.setRequestHeader("Content-Type",
b.contentType);if(b.ifModified){c.lastModified[b.url]&&w.setRequestHeader("If-Modified-Since",c.lastModified[b.url]);c.etag[b.url]&&w.setRequestHeader("If-None-Match",c.etag[b.url])}o||w.setRequestHeader("X-Requested-With","XMLHttpRequest");w.setRequestHeader("Accept",b.dataType&&b.accepts[b.dataType]?b.accepts[b.dataType]+", */*; q=0.01":b.accepts._default)}catch(I){}if(b.beforeSend&&b.beforeSend.call(b.context,w,b)===false){b.global&&c.active--===1&&c.event.trigger("ajaxStop");w.abort();return false}b.global&&
c.triggerGlobal(b,"ajaxSend",[w,b]);var L=w.onreadystatechange=function(m){if(!w||w.readyState===0||m==="abort"){J||c.handleComplete(b,w,e,f);J=true;if(w)w.onreadystatechange=c.noop}else if(!J&&w&&(w.readyState===4||m==="timeout")){J=true;w.onreadystatechange=c.noop;e=m==="timeout"?"timeout":!c.httpSuccess(w)?"error":b.ifModified&&c.httpNotModified(w,b.url)?"notmodified":"success";var p;if(e==="success")try{f=c.httpData(w,b.dataType,b)}catch(q){e="parsererror";p=q}if(e==="success"||e==="notmodified")d||
c.handleSuccess(b,w,e,f);else c.handleError(b,w,e,p);d||c.handleComplete(b,w,e,f);m==="timeout"&&w.abort();if(b.async)w=null}};try{var g=w.abort;w.abort=function(){w&&Function.prototype.call.call(g,w);L("abort")}}catch(i){}b.async&&b.timeout>0&&setTimeout(function(){w&&!J&&L("timeout")},b.timeout);try{w.send(l||b.data==null?null:b.data)}catch(n){c.handleError(b,w,null,n);c.handleComplete(b,w,e,f)}b.async||L();return w}},param:function(a,b){var d=[],e=function(h,l){l=c.isFunction(l)?l():l;d[d.length]=
encodeURIComponent(h)+"="+encodeURIComponent(l)};if(b===B)b=c.ajaxSettings.traditional;if(c.isArray(a)||a.jquery)c.each(a,function(){e(this.name,this.value)});else for(var f in a)da(f,a[f],b,e);return d.join("&").replace(tb,"+")}});c.extend({active:0,lastModified:{},etag:{},handleError:function(a,b,d,e){a.error&&a.error.call(a.context,b,d,e);a.global&&c.triggerGlobal(a,"ajaxError",[b,a,e])},handleSuccess:function(a,b,d,e){a.success&&a.success.call(a.context,e,d,b);a.global&&c.triggerGlobal(a,"ajaxSuccess",
[b,a])},handleComplete:function(a,b,d){a.complete&&a.complete.call(a.context,b,d);a.global&&c.triggerGlobal(a,"ajaxComplete",[b,a]);a.global&&c.active--===1&&c.event.trigger("ajaxStop")},triggerGlobal:function(a,b,d){(a.context&&a.context.url==null?c(a.context):c.event).trigger(b,d)},httpSuccess:function(a){try{return!a.status&&location.protocol==="file:"||a.status>=200&&a.status<300||a.status===304||a.status===1223}catch(b){}return false},httpNotModified:function(a,b){var d=a.getResponseHeader("Last-Modified"),
e=a.getResponseHeader("Etag");if(d)c.lastModified[b]=d;if(e)c.etag[b]=e;return a.status===304},httpData:function(a,b,d){var e=a.getResponseHeader("content-type")||"",f=b==="xml"||!b&&e.indexOf("xml")>=0;a=f?a.responseXML:a.responseText;f&&a.documentElement.nodeName==="parsererror"&&c.error("parsererror");if(d&&d.dataFilter)a=d.dataFilter(a,b);if(typeof a==="string")if(b==="json"||!b&&e.indexOf("json")>=0)a=c.parseJSON(a);else if(b==="script"||!b&&e.indexOf("javascript")>=0)c.globalEval(a);return a}});
if(E.ActiveXObject)c.ajaxSettings.xhr=function(){if(E.location.protocol!=="file:")try{return new E.XMLHttpRequest}catch(a){}try{return new E.ActiveXObject("Microsoft.XMLHTTP")}catch(b){}};c.support.ajax=!!c.ajaxSettings.xhr();var ea={},vb=/^(?:toggle|show|hide)$/,wb=/^([+\-]=)?([\d+.\-]+)(.*)$/,ba,pa=[["height","marginTop","marginBottom","paddingTop","paddingBottom"],["width","marginLeft","marginRight","paddingLeft","paddingRight"],["opacity"]];c.fn.extend({show:function(a,b,d){if(a||a===0)return this.animate(S("show",
3),a,b,d);else{d=0;for(var e=this.length;d<e;d++){a=this[d];b=a.style.display;if(!c.data(a,"olddisplay")&&b==="none")b=a.style.display="";b===""&&c.css(a,"display")==="none"&&c.data(a,"olddisplay",qa(a.nodeName))}for(d=0;d<e;d++){a=this[d];b=a.style.display;if(b===""||b==="none")a.style.display=c.data(a,"olddisplay")||""}return this}},hide:function(a,b,d){if(a||a===0)return this.animate(S("hide",3),a,b,d);else{a=0;for(b=this.length;a<b;a++){d=c.css(this[a],"display");d!=="none"&&c.data(this[a],"olddisplay",
d)}for(a=0;a<b;a++)this[a].style.display="none";return this}},_toggle:c.fn.toggle,toggle:function(a,b,d){var e=typeof a==="boolean";if(c.isFunction(a)&&c.isFunction(b))this._toggle.apply(this,arguments);else a==null||e?this.each(function(){var f=e?a:c(this).is(":hidden");c(this)[f?"show":"hide"]()}):this.animate(S("toggle",3),a,b,d);return this},fadeTo:function(a,b,d,e){return this.filter(":hidden").css("opacity",0).show().end().animate({opacity:b},a,d,e)},animate:function(a,b,d,e){var f=c.speed(b,
d,e);if(c.isEmptyObject(a))return this.each(f.complete);return this[f.queue===false?"each":"queue"](function(){var h=c.extend({},f),l,k=this.nodeType===1,o=k&&c(this).is(":hidden"),x=this;for(l in a){var r=c.camelCase(l);if(l!==r){a[r]=a[l];delete a[l];l=r}if(a[l]==="hide"&&o||a[l]==="show"&&!o)return h.complete.call(this);if(k&&(l==="height"||l==="width")){h.overflow=[this.style.overflow,this.style.overflowX,this.style.overflowY];if(c.css(this,"display")==="inline"&&c.css(this,"float")==="none")if(c.support.inlineBlockNeedsLayout)if(qa(this.nodeName)===
"inline")this.style.display="inline-block";else{this.style.display="inline";this.style.zoom=1}else this.style.display="inline-block"}if(c.isArray(a[l])){(h.specialEasing=h.specialEasing||{})[l]=a[l][1];a[l]=a[l][0]}}if(h.overflow!=null)this.style.overflow="hidden";h.curAnim=c.extend({},a);c.each(a,function(A,C){var J=new c.fx(x,h,A);if(vb.test(C))J[C==="toggle"?o?"show":"hide":C](a);else{var w=wb.exec(C),I=J.cur()||0;if(w){var L=parseFloat(w[2]),g=w[3]||"px";if(g!=="px"){c.style(x,A,(L||1)+g);I=(L||
1)/J.cur()*I;c.style(x,A,I+g)}if(w[1])L=(w[1]==="-="?-1:1)*L+I;J.custom(I,L,g)}else J.custom(I,C,"")}});return true})},stop:function(a,b){var d=c.timers;a&&this.queue([]);this.each(function(){for(var e=d.length-1;e>=0;e--)if(d[e].elem===this){b&&d[e](true);d.splice(e,1)}});b||this.dequeue();return this}});c.each({slideDown:S("show",1),slideUp:S("hide",1),slideToggle:S("toggle",1),fadeIn:{opacity:"show"},fadeOut:{opacity:"hide"},fadeToggle:{opacity:"toggle"}},function(a,b){c.fn[a]=function(d,e,f){return this.animate(b,
d,e,f)}});c.extend({speed:function(a,b,d){var e=a&&typeof a==="object"?c.extend({},a):{complete:d||!d&&b||c.isFunction(a)&&a,duration:a,easing:d&&b||b&&!c.isFunction(b)&&b};e.duration=c.fx.off?0:typeof e.duration==="number"?e.duration:e.duration in c.fx.speeds?c.fx.speeds[e.duration]:c.fx.speeds._default;e.old=e.complete;e.complete=function(){e.queue!==false&&c(this).dequeue();c.isFunction(e.old)&&e.old.call(this)};return e},easing:{linear:function(a,b,d,e){return d+e*a},swing:function(a,b,d,e){return(-Math.cos(a*
Math.PI)/2+0.5)*e+d}},timers:[],fx:function(a,b,d){this.options=b;this.elem=a;this.prop=d;if(!b.orig)b.orig={}}});c.fx.prototype={update:function(){this.options.step&&this.options.step.call(this.elem,this.now,this);(c.fx.step[this.prop]||c.fx.step._default)(this)},cur:function(){if(this.elem[this.prop]!=null&&(!this.elem.style||this.elem.style[this.prop]==null))return this.elem[this.prop];var a=parseFloat(c.css(this.elem,this.prop));return a&&a>-1E4?a:0},custom:function(a,b,d){function e(l){return f.step(l)}
var f=this,h=c.fx;this.startTime=c.now();this.start=a;this.end=b;this.unit=d||this.unit||"px";this.now=this.start;this.pos=this.state=0;e.elem=this.elem;if(e()&&c.timers.push(e)&&!ba)ba=setInterval(h.tick,h.interval)},show:function(){this.options.orig[this.prop]=c.style(this.elem,this.prop);this.options.show=true;this.custom(this.prop==="width"||this.prop==="height"?1:0,this.cur());c(this.elem).show()},hide:function(){this.options.orig[this.prop]=c.style(this.elem,this.prop);this.options.hide=true;
this.custom(this.cur(),0)},step:function(a){var b=c.now(),d=true;if(a||b>=this.options.duration+this.startTime){this.now=this.end;this.pos=this.state=1;this.update();this.options.curAnim[this.prop]=true;for(var e in this.options.curAnim)if(this.options.curAnim[e]!==true)d=false;if(d){if(this.options.overflow!=null&&!c.support.shrinkWrapBlocks){var f=this.elem,h=this.options;c.each(["","X","Y"],function(k,o){f.style["overflow"+o]=h.overflow[k]})}this.options.hide&&c(this.elem).hide();if(this.options.hide||
this.options.show)for(var l in this.options.curAnim)c.style(this.elem,l,this.options.orig[l]);this.options.complete.call(this.elem)}return false}else{a=b-this.startTime;this.state=a/this.options.duration;b=this.options.easing||(c.easing.swing?"swing":"linear");this.pos=c.easing[this.options.specialEasing&&this.options.specialEasing[this.prop]||b](this.state,a,0,1,this.options.duration);this.now=this.start+(this.end-this.start)*this.pos;this.update()}return true}};c.extend(c.fx,{tick:function(){for(var a=
c.timers,b=0;b<a.length;b++)a[b]()||a.splice(b--,1);a.length||c.fx.stop()},interval:13,stop:function(){clearInterval(ba);ba=null},speeds:{slow:600,fast:200,_default:400},step:{opacity:function(a){c.style(a.elem,"opacity",a.now)},_default:function(a){if(a.elem.style&&a.elem.style[a.prop]!=null)a.elem.style[a.prop]=(a.prop==="width"||a.prop==="height"?Math.max(0,a.now):a.now)+a.unit;else a.elem[a.prop]=a.now}}});if(c.expr&&c.expr.filters)c.expr.filters.animated=function(a){return c.grep(c.timers,function(b){return a===
b.elem}).length};var xb=/^t(?:able|d|h)$/i,Ia=/^(?:body|html)$/i;c.fn.offset="getBoundingClientRect"in t.documentElement?function(a){var b=this[0],d;if(a)return this.each(function(l){c.offset.setOffset(this,a,l)});if(!b||!b.ownerDocument)return null;if(b===b.ownerDocument.body)return c.offset.bodyOffset(b);try{d=b.getBoundingClientRect()}catch(e){}var f=b.ownerDocument,h=f.documentElement;if(!d||!c.contains(h,b))return d||{top:0,left:0};b=f.body;f=fa(f);return{top:d.top+(f.pageYOffset||c.support.boxModel&&
h.scrollTop||b.scrollTop)-(h.clientTop||b.clientTop||0),left:d.left+(f.pageXOffset||c.support.boxModel&&h.scrollLeft||b.scrollLeft)-(h.clientLeft||b.clientLeft||0)}}:function(a){var b=this[0];if(a)return this.each(function(x){c.offset.setOffset(this,a,x)});if(!b||!b.ownerDocument)return null;if(b===b.ownerDocument.body)return c.offset.bodyOffset(b);c.offset.initialize();var d,e=b.offsetParent,f=b.ownerDocument,h=f.documentElement,l=f.body;d=(f=f.defaultView)?f.getComputedStyle(b,null):b.currentStyle;
for(var k=b.offsetTop,o=b.offsetLeft;(b=b.parentNode)&&b!==l&&b!==h;){if(c.offset.supportsFixedPosition&&d.position==="fixed")break;d=f?f.getComputedStyle(b,null):b.currentStyle;k-=b.scrollTop;o-=b.scrollLeft;if(b===e){k+=b.offsetTop;o+=b.offsetLeft;if(c.offset.doesNotAddBorder&&!(c.offset.doesAddBorderForTableAndCells&&xb.test(b.nodeName))){k+=parseFloat(d.borderTopWidth)||0;o+=parseFloat(d.borderLeftWidth)||0}e=b.offsetParent}if(c.offset.subtractsBorderForOverflowNotVisible&&d.overflow!=="visible"){k+=
parseFloat(d.borderTopWidth)||0;o+=parseFloat(d.borderLeftWidth)||0}d=d}if(d.position==="relative"||d.position==="static"){k+=l.offsetTop;o+=l.offsetLeft}if(c.offset.supportsFixedPosition&&d.position==="fixed"){k+=Math.max(h.scrollTop,l.scrollTop);o+=Math.max(h.scrollLeft,l.scrollLeft)}return{top:k,left:o}};c.offset={initialize:function(){var a=t.body,b=t.createElement("div"),d,e,f,h=parseFloat(c.css(a,"marginTop"))||0;c.extend(b.style,{position:"absolute",top:0,left:0,margin:0,border:0,width:"1px",
height:"1px",visibility:"hidden"});b.innerHTML="<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";a.insertBefore(b,a.firstChild);d=b.firstChild;e=d.firstChild;f=d.nextSibling.firstChild.firstChild;this.doesNotAddBorder=e.offsetTop!==5;this.doesAddBorderForTableAndCells=
f.offsetTop===5;e.style.position="fixed";e.style.top="20px";this.supportsFixedPosition=e.offsetTop===20||e.offsetTop===15;e.style.position=e.style.top="";d.style.overflow="hidden";d.style.position="relative";this.subtractsBorderForOverflowNotVisible=e.offsetTop===-5;this.doesNotIncludeMarginInBodyOffset=a.offsetTop!==h;a.removeChild(b);c.offset.initialize=c.noop},bodyOffset:function(a){var b=a.offsetTop,d=a.offsetLeft;c.offset.initialize();if(c.offset.doesNotIncludeMarginInBodyOffset){b+=parseFloat(c.css(a,
"marginTop"))||0;d+=parseFloat(c.css(a,"marginLeft"))||0}return{top:b,left:d}},setOffset:function(a,b,d){var e=c.css(a,"position");if(e==="static")a.style.position="relative";var f=c(a),h=f.offset(),l=c.css(a,"top"),k=c.css(a,"left"),o=e==="absolute"&&c.inArray("auto",[l,k])>-1;e={};var x={};if(o)x=f.position();l=o?x.top:parseInt(l,10)||0;k=o?x.left:parseInt(k,10)||0;if(c.isFunction(b))b=b.call(a,d,h);if(b.top!=null)e.top=b.top-h.top+l;if(b.left!=null)e.left=b.left-h.left+k;"using"in b?b.using.call(a,
e):f.css(e)}};c.fn.extend({position:function(){if(!this[0])return null;var a=this[0],b=this.offsetParent(),d=this.offset(),e=Ia.test(b[0].nodeName)?{top:0,left:0}:b.offset();d.top-=parseFloat(c.css(a,"marginTop"))||0;d.left-=parseFloat(c.css(a,"marginLeft"))||0;e.top+=parseFloat(c.css(b[0],"borderTopWidth"))||0;e.left+=parseFloat(c.css(b[0],"borderLeftWidth"))||0;return{top:d.top-e.top,left:d.left-e.left}},offsetParent:function(){return this.map(function(){for(var a=this.offsetParent||t.body;a&&!Ia.test(a.nodeName)&&
c.css(a,"position")==="static";)a=a.offsetParent;return a})}});c.each(["Left","Top"],function(a,b){var d="scroll"+b;c.fn[d]=function(e){var f=this[0],h;if(!f)return null;if(e!==B)return this.each(function(){if(h=fa(this))h.scrollTo(!a?e:c(h).scrollLeft(),a?e:c(h).scrollTop());else this[d]=e});else return(h=fa(f))?"pageXOffset"in h?h[a?"pageYOffset":"pageXOffset"]:c.support.boxModel&&h.document.documentElement[d]||h.document.body[d]:f[d]}});c.each(["Height","Width"],function(a,b){var d=b.toLowerCase();
c.fn["inner"+b]=function(){return this[0]?parseFloat(c.css(this[0],d,"padding")):null};c.fn["outer"+b]=function(e){return this[0]?parseFloat(c.css(this[0],d,e?"margin":"border")):null};c.fn[d]=function(e){var f=this[0];if(!f)return e==null?null:this;if(c.isFunction(e))return this.each(function(l){var k=c(this);k[d](e.call(this,l,k[d]()))});if(c.isWindow(f))return f.document.compatMode==="CSS1Compat"&&f.document.documentElement["client"+b]||f.document.body["client"+b];else if(f.nodeType===9)return Math.max(f.documentElement["client"+
b],f.body["scroll"+b],f.documentElement["scroll"+b],f.body["offset"+b],f.documentElement["offset"+b]);else if(e===B){f=c.css(f,d);var h=parseFloat(f);return c.isNaN(h)?f:h}else return this.css(d,typeof e==="string"?e:e+"px")}})})(window);

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
    var glimpseCss = '.glimpse, .glimpse *, .glimpse a, .glimpse td, .glimpse th, .glimpse table { /*font-family:Lucida Grande,Tahoma,sans-serif;*/ font-family: Helvetica, Arial, sans-serif; background-color:transparent; font-size:11px; line-height:14px; border:0px; color:#232323; text-align:left; }.glimpse a, .glimpse a:hover, .glimpse a:visited { color:#2200C1; text-decoration:underline; font-weight:normal; }.glimpse a:active { color:#c11; text-decoration:underline; font-weight:normal; }.glimpse th { font-weight:bold; }.glimpse-open { z-index: 100010; position:fixed; right:0; bottom:0; height:27px; width:28px; border-left: 1px solid #ACA899; border-top: 1px solid #ACA899; background:#EEE; background:-moz-linear-gradient(top, #FFFFFF 0%, #EEEEEE 4%, #F3F5F7 8%, #E9E8DD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#FFFFFF), color-stop(4%,#EEEEEE), color-stop(8%,#F3F5F7), color-stop(100%,#E9E8DD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#FFFFFF\', endColorstr=\'#E9E8DD\',GradientType=0 ); }.glimpse-icon { background:url() 0px -16px; height:20px; width:20px; margin: 3px 4px 0; cursor:pointer; }.glimpse-holder { display:none; z-index:100010 !important; height:0; position:fixed; bottom:0; left:0; width:100%; background-color:#fff; }.glimpse-bar { height:27px; border-top:1px solid #ACA899; background:#FFFFFF; background:-moz-linear-gradient(top, #FFFFFF 0%, #EEEEEE 4%, #F3F5F7 8%, #E9E8DD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#FFFFFF), color-stop(4%,#EEEEEE), color-stop(8%,#F3F5F7), color-stop(100%,#E9E8DD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#FFFFFF\', endColorstr=\'#E9E8DD\',GradientType=0 ); }.glimpse-bar .glimpse-icon { margin-top:4px; float:left; } .glimpse-buttons { text-align:right; float:right; height:17px; width:150px; padding:6px; }.glimpse-title { margin:0 0 0 15px; padding-top:5px; font-weight:bold; display:inline-block; width:75%; overflow:hidden; }.glimpse-title span:first-child { display:inline-block; height:20px; }.glimpse-title span:last-child { font-weight:normal; font-style:italic; padding-left:10px; width:60%; white-space:nowrap; display:inline-block; height:20px; } .glimpse-title span:last-child .glimpse-switch { -webkit-border-radius: 3px; -moz-border-radius: 3px; border-radius: 3px; border: 1px solid #CCC; padding: 1px 10px 4px 8px; margin: 0 5px 0 0; background:#FFFFFF; background:-moz-linear-gradient(top, #E9E8DD 0%, #F3F5F7 4%, #EEE 8%, #DDD 100%); background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#E9E8DD), color-stop(4%,#F3F5F7), color-stop(8%,#EEE), color-stop(100%,#DDD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#E9E8DD\', endColorstr=\'#FFFFFF\',GradientType=0 ) } .glimpse-title span:last-child .glimpse-switch-over { position:absolute; display:none; top:4px; padding-bottom:10px; z-index:100; } .glimpse-title span:last-child .glimpse-switch-over div { text-align:center; font-weight:bold; margin:5px 0; } .glimpse-button, .glimpse-button:hover { cursor:pointer; background-image:url(); background-repeat:no-repeat; height:14px; width:14px; margin-left:2px; display:inline-block; }.glimpse-meta-warning { background-position:-168px -1px; display:none; }.glimpse-meta-warning:hover { background-position:-183px -1px; } .glimpse-meta-help { background-position:-138px -1px; margin-right:15px; }.glimpse-meta-help:hover { background-position:-153px -1px; margin-right:15px; }.glimpse-meta-update { background-position:-198px -1px; display:none; }.glimpse-meta-update:hover { background-position:-213px -1px; }.glimpse-close { background-position:-1px -1px; }.glimpse-close:hover { background-position:-17px -1px; }.glimpse-terminate { background-position:-65px -1px; }.glimpse-terminate:hover { background-position:-81px -1px; } .glimpse-popout { background-position:-96px -1px; }.glimpse-popout:hover { background-position: -111px -1px; } .glimpse-tabs { height:24px; font-weight:bold; border-bottom:1px solid #ACA899; border-top:1px solid #CDCABB; background:#B9B7AF; background:-moz-linear-gradient(top, #B9B7AF 0%, #DAD8C8 4%, #D7D4C5 10%, #E9E6D5 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#B9B7AF), color-stop(4%,#DAD8C8), color-stop(10%,#D7D4C5), color-stop(100%,#E9E6D5)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#B9B7AF\', endColorstr=\'#E9E6D5\',GradientType=0 );}.glimpse-tabs ul { margin:4px 0px 0 0; padding:0px; }.glimpse-tabs li { display:inline; margin:0 2px 3px 2px; height:22px; padding:4px 9px 3px; color:#565656; cursor:pointer; border-radius: 0px 0px 3px 3px; -moz-border-radius: 0px 0px 3px 3px; -webkit-border-bottom-right-radius: 3px; -webkit-border-bottom-left-radius: 3px; }.glimpse-tabs li.glimpse-active { padding:4px 8px 3px; color:#000; border-left:1px solid #A4A4A4; border-bottom:1px solid #A4A4A4; border-right:1px solid #A4A4A4; background:#F7F6F1; background:-moz-linear-gradient(top, #F2F1EC 0%, #F2F1EC 3%, #EFEEE9 7%, #E8E7E1 51%, #F7F6F1 92%, #F1F0EB 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#F2F1EC), color-stop(3%,#F2F1EC), color-stop(7%,#EFEEE9), color-stop(51%,#E8E7E1), color-stop(92%,#F7F6F1), color-stop(100%,#F1F0EB)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#EFEEE9\', endColorstr=\'#F7F6F1\',GradientType=0 ); } .glimpse-tabs li.glimpse-hover { padding:4px 8px 3px; border-left:1px solid #BFBDB1; border-bottom:1px solid #BFBDB1; border-right:1px solid #BFBDB1; background:#EEECE3; background:-moz-linear-gradient(top, #BFBDB1 0%, #DAD9CB 4%, #D8D5C9 8%, #E8E7E1 51%, #F0EEE4 92%, #EDEBE1 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#BFBDB1), color-stop(4%,#DAD9CB), color-stop(8%,#D8D5C9), color-stop(51%,#E8E7E1), color-stop(92%,#F0EEE4), color-stop(100%,#EDEBE1)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#D8D5C9\', endColorstr=\'#F0EEE4\',GradientType=0 ); }.glimpse-tabs li.glimpse-disabled { color:#AAA; cursor:default; }.glimpse-panel-holder {}.glimpse-panel { display:none; overflow:auto; position:relative; } .glimpse-panel-message { text-align:center; padding-top:40px; font-size:1.1em; color:#AAA; }.glimpse-panel table { border-spacing:0; width:100%; }.glimpse-panel table td, .glimpse-panel table th { padding:3px 4px; text-align:left; vertical-align:top; } .glimpse-panel table td .glimpse-cell { vertical-align:top; } .glimpse-panel tbody .mono { font-family:Consolas, monospace, serif; font-size: 1.2em; } .glimpse-panel tr.glimpse-row-header-0 { height:19px; } .glimpse-panel .glimpse-row-header-0 { font-weight:bold; border-bottom:1px solid #9C9C9C; background-repeat:no-repeat; background:#C6C6C6; background:-moz-linear-gradient(top, #DEDEDE 0%, #BDBDBD 80%, #BBB 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#DEDEDE), color-stop(80%,#BDBDBD), color-stop(100%,#BBB)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#DEDEDE\', endColorstr=\'#BBB\',GradientType=0 ); }.glimpse-panel .glimpse-row-header-0 th { border-left:1px solid #D9D9D9; border-right:1px solid #9C9C9C; } .glimpse-panel .glimpse-soft { color:#999; } .glimpse-panel .glimpse-cell-key { font-weight:bold; } .glimpse-panel th.glimpse-cell-key { width:30%; max-width:150px; } .glimpse-panel table table { border:1px solid #D9D9D9; } .glimpse-panel table table thead tr { height:17px; border-bottom:1px solid #9C9C9C; background:#C6C6C6; background:-moz-linear-gradient(top, #F1F1F1 0%, #DFDFDF 80%, #DDD 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#F1F1F1), color-stop(80%,#DFDFDF), color-stop(100%,#DDD)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#F1F1F1\', endColorstr=\'#DDD\',GradientType=0 ); }.glimpse-panel table table thead tr th { border-left:1px solid #C6C6C6; border-right:1px solid #D9D9D9; padding:1px 4px 2px 4px; }.glimpse-panel table table thead tr th:first-child { border-left:0px; }.glimpse-panel table table thead tr th:last-child { border-right:0px; }.glimpse-panel .even { background:#F4F4F4; }.glimpse-panel .odd { background:#F9F9F9; }.glimpse-panel table table tbody th { font-weight:normal; font-style:italic; }.glimpse-panel table table thead th { font-weight:bold; font-style:normal; }.glimpse-panel .glimpse-side-sub-panel { right:0; z-index:10; background-color:#F5F5F5; height:100%; width:25%; border-left:1px solid #ACA899; position:absolute; }.glimpse-panel .glimpse-side-main-panel { position:relative; height:100%; width:75%; float:left; } .glimpse-panel-holder .glimpse-active { display:block; }.glimpse-resizer { height:4px; cursor:n-resize; width:100%; position:absolute; top:-1px; }li.glimpse-permanent { font-style:italic; padding:4px 8px 3px; border-bottom:1px solid #ACA899; border-left:1px solid #CDCABB; border-right:1px solid #CDCABB; background:#B9B7AF; background:-moz-linear-gradient(top, #B9B7AF 0%, #DAD8C8 4%, #D7D4C5 10%, #E9E6D5 100%); background:-webkit-gradient(linear, left top, left bottom, color-stop(0%,#B9B7AF), color-stop(4%,#DAD8C8), color-stop(10%,#D7D4C5), color-stop(100%,#E9E6D5)); filter:progid:DXImageTransform.Microsoft.gradient( startColorstr=\'#B9B7AF\', endColorstr=\'#E9E6D5\',GradientType=0 ); }.glimpse-preview-object { color:#006400; } .glimpse-preview-string { color:#006400 !important; font-weight:normal !important; } .glimpse-preview-string span { padding-left:1px; }.glimpse-preview-object span { font-weight:bold; color:#444; } .glimpse-preview-object span.start { margin-right:5px; } .glimpse-preview-object span.end { margin-left:5px; }.glimpse-preview-object span.rspace { margin-right:4px; }.glimpse-preview-object span.mspace { margin:0 4px; }.glimpse-preview-object span.small { font-size:0.95em; } .glimpse-panel .glimpse-preview-table { border:0; } .glimpse-panel .glimpse-preview-table .glimpse-preview-cell { padding-left:0; padding-right:2px; width:11px; } .glimpse-expand { height:11px; width:11px; display:inline-block; float:left; margin:1px 0 0 0; cursor:pointer; background-image:url(); background-repeat:no-repeat; background-position:-126px 0; }.glimpse-collapse { background-position:-126px -11px; }.glimpse-preview-show { display:none; font-weight:normal !important; }.glimpse-panel .quiet *, .glimpse-panel .ms * { color:#AAA; }.glimpse-panel .suppress { text-decoration:line-through; }.glimpse-panel .suppress * { color:#AAA; }.glimpse-panel .selected { background-color:#FFFF99; }.glimpse-panel .selected * { color:#409B3B; }.glimpse-panel .info .icon, .glimpse-panel .warn .icon, .glimpse-panel .loading .icon, .glimpse-panel .error .icon, .glimpse-panel .fail .icon, .glimpse-panel .ms .icon { width:14px; height:14px; background-image:url(); background-repeat:no-repeat; display:inline-block; margin-right: 5px; } .glimpse-panel .info .icon { background-position: -22px -22px; }.glimpse-panel .warn .icon { background-position:-36px -22px; }.glimpse-panel .loading .icon { background-position:-78px -22px; }.glimpse-panel .error .icon { background-position:-50px -22px; }.glimpse-panel .ms .icon { background-position:-181px -22px; } .glimpse-panel .fail .icon { background-position:-64px -22px; }.glimpse-panel .info * { color:#067CE5; }.glimpse-panel .warn * { color:#FE850C; } .glimpse-panel .error * { color:#B40000; }.glimpse-panel .fail * { color:#B40000; font-weight:bold; }.glimpse-panelitem-Ajax .loading .icon { float:right; }.glimpse-panelitem-Remote .glimpse-side-sub-panel .loading, .glimpse-panelitem-Remote .glimpse-side-main-panel .loading, .glimpse-clear { position:fixed; bottom:5px; right:10px; color:#777; } .glimpse-panelitem-Remote .glimpse-side-main-panel .loading { right:27%; } .glimpse-clear { background-color:white; padding:0.3em 1em 0.5em 1em; border:#CCC solid 1px; bottom:25px; -webkit-border-radius:3px; -moz-border-radius:3px; border-radius:3px; } .glimpse-panel table .glimpse-head-message td { text-align:center; background-color:#DDD; } .glimpse-panelitem-GlimpseInformation div { text-align:center; } .glimpse-panelitem-GlimpseInformation .glimpse-panel-message { padding-top:5px; } .glimpse-panelitem-GlimpseInformation strong { font-weight:bold; } .glimpse-panelitem-GlimpseInformation .glimpse-info-more { font-size:1.5em; margin:1em 0; } .glimpse-panelitem-GlimpseInformation .glimpse-info-quote { font-style:italic; margin:0.75em 0 3em; }';
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
        resizer: function () {
            return this.each(function () {
                var gr = $.glimpseResize;
                gr.static.anchor = $(this).bind("mousedown", { el: $(this).parent() }, gr.startDrag);
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
                    that.addTabBody(panelHolder, that.build(data, 0, metadata, 1), key);
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
                    var item = $(this).addClass('prettyprint'), codeType = item.hasClass('glimpse-code') ? item.data('codeType') : item.closest('.glimpse-code').data('codeType');  
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
        build: function (data, level, metadata, tolerance) {
            var that = this, result = '';

            if ($.isArray(data)) {
                if (metadata)
                    result = that.buildStructuredTable(data, level, false, metadata, tolerance);
                else
                    result = that.buildCustomTable(data, level, false, tolerance);
            }
            else if ($.isPlainObject(data))
                result = that.buildKeyValueTable(data, level, false, tolerance);
            else if (level == 0) {
                if (data === undefined || data === null || data === '')
                    result = '';
                else
                    result = '<div class="glimpse-panel-message">' + data + '</div>';
            }
            else
                result = that.buildString(data, level, tolerance);

            return result;
        },
        buildHeading: function (url, clientName, type) {
            var clean = function (data) {
                return (data === undefined || data === null || data === "null") ? '' : data;
            }
            type = clean(type);
            clientName = clean(clientName);
            return '<span>' + clientName + ((type.length > 0) ? ' (' + type + ')' : '') + '&nbsp;</span><span>' + url + '</span>';
        },
        buildKeyValueTable: function (data, level, forceFull, tolerance) {
            var that = this, limit = 3 * tolerance; 
            if (((level > 0 && $.glimpse.util.lengthJson(data) > (limit + 1)) || level > 1) && !forceFull)
                return that.buildKeyValuePreview(data, level, tolerance);

            var i = 1, html = '<table><thead><tr class="glimpse-row-header-' + level + '"><th class="glimpse-cell-key">Key</th><th class="glimpse-cell-value">Value</th></tr></thead>';
            for (var key in data)
                html += '<tr class="' + (i++ % 2 ? 'odd' : 'even') + '"><th width="30%">' + $.glimpseContent.formatString(key) + '</th><td width="70%"> ' + that.build(data[key], level + 1, undefined, 1) + '</td></tr>';
            html += '</table>';
            return html;
        },
        buildCustomTable: function (data, level, forceFull, tolerance) {
            var that = this, limit = 3 * tolerance; 
            if (((level > 0 && data.length > (limit + 1)) || level > 1) && !forceFull)
                return that.buildCustomPreview(data, level, tolerance);

            var html = '<table><thead><tr class="glimpse-row-header-' + level + '">';
            if ($.isArray(data[0])) {
                for (var x = 0; x < data[0].length; x++)
                    html += '<th>' + $.glimpseContent.formatString(data[0][x]) + '</th>';
                html += '</tr></thead>';
                for (var i = 1; i < data.length; i++) {
                    html += '<tr class="' + (i % 2 ? 'odd' : 'even') + (data[i].length > data[0].length ? ' ' + data[i][data[i].length - 1] : '') + '">';
                    for (var x = 0; x < data[0].length; x++)
                        html += '<td>' + that.build(data[i][x], level + 1, undefined, 1) + '</td>';
                    html += '</tr>';
                }
                html += '</table>';
            }
            else {
                if (data.length > 1) {
                    html += '<th>Values</th></tr></thead>';
                    for (var i = 0; i < data.length; i++)
                        html += '<tr class="' + (i % 2 ? 'odd' : 'even') + '"><td>' + that.build(data[i], level + 1, undefined, 1) + '</td></tr>';
                    html += '</table>';
                }
                else
                    html = that.build(data[0], level + 1, undefined, 1)
            }
            return html;
        },
        buildStructuredTable: function (data, level, forceFull, metadata, tolerance) {
            var that = this, limit = 3 * tolerance; 
            if (((level > 0 && data.length > (limit + 1)) || level > 1) && !forceFull)
                return that.buildCustomPreview(data, level, tolerance);

            var html = '<table>', rowClass = '', newTolerance = 1;
            for (var i = 0; i < data.length; i++) {
                rowClass = data[i].length > data[0].length ? (' ' + data[i][data[i].length - 1]) : '';
                html += (i == 0) ? '<thead class="glimpse-row-header-' + level + '">' : '<tbody class="' + (i % 2 ? 'odd' : 'even') + rowClass + '">';
                for (var x = 0; x < metadata.length; x++) { 
                    html += '<tr>';

                    //TODO Review for Performance 
                    for (var y = 0; y < metadata[x].length; y++) {
                        if ($.isArray(metadata[x][y].data) && metadata[x][y].data.length > newTolerance)
                            newTolerance = metadata[x][y].data.length;
                    }

                    for (var y = 0; y < metadata[x].length; y++) {
                        var metadataItem = metadata[x][y], cellType = (i == 0 ? 'th' : 'td'); 
                        html += that.buildStructuredTableCell(data[i], metadataItem, level, newTolerance, cellType);
                    }

                    html += '</tr>';
                }
                html += (i == 0) ? '</thead>' : '</tbody>';
            }
            html += '</table>'; 

            return html;
        },
        buildStructuredTableCell : function(data, metadataItem, level, tolerance, cellType) {
            var that = this, html = '', cellContent = '', cellClass = '', cellStyle = '', cellAttr = '';
                
            //Cell Content
            if ($.isArray(metadataItem.data)) {
                for (var i = 0; i < metadataItem.data.length; i++) 
                    cellContent += that.buildStructuredTableCell(data, metadataItem.data[i], level, 1, 'div');
            }
            else { 
                if (!metadataItem.indexs && $.isNaN(metadataItem.data)) 
                    metadataItem.indexs = $.glimpse.util.getTokens(metadataItem.data, data); 
                    
                cellContent = metadataItem.indexs ? that.buildFormatString(metadataItem.data, data, metadataItem.indexs) : data[metadataItem.data];
                cellContent = that.build(cellContent, level + 1, metadataItem.structure, tolerance);

                //Content pre/post
                if (metadataItem.pre) { cellContent = '<span class="glimpse-soft">' + metadataItem.pre + '</span>' + cellContent; }
                if (metadataItem.post) { cellContent = cellContent + '<span class="glimpse-soft">' + metadataItem.post + '</span>'; }
            }

            if (cellType != 'th') {
                cellClass = 'glimpse-cell';
                //Cell Class
                if (metadataItem.key === true) { cellClass += ' glimpse-cell-key'; }
                if (metadataItem.isCode === true) { cellClass += ' glimpse-code'; }
                if (metadataItem.className) { cellClass += ' ' + metadataItem.className; }
                //Cell Code 
                if (metadataItem.codeType) { cellAttr += ' data-codeType="' + metadataItem.codeType + '"'; };
            }
            if (cellClass) { cellAttr += ' class="' + cellClass + '"' }; 
            //Cell Style  
            if (metadataItem.width) { cellStyle += 'width:' + metadataItem.width + ';' };
            if (metadataItem.align) { cellStyle += 'text-align:' + metadataItem.align + ';' };
            if (cellStyle) { cellAttr += ' style="' + cellStyle + '"' };
            //Cell Span
            if (metadataItem.span) { cellAttr += ' colspan="' + metadataItem.span + '"' };

            html += '<' + cellType + cellAttr + '>' + cellContent + '</' + cellType + '>';
            
            return html;
        },
        buildFormatString : function(formatString, data, indexs) { 
            for (var i = 0; i < indexs.length; i++) {
                var pattern = "\\\{\\\{" + indexs[i] + "\\\}\\\}", regex = new RegExp(pattern, "g"); 
                formatString = formatString.replace(regex, data[indexs[i]]);
            } 
            return formatString;
        }, 
        buildString: function (data, level, tolerance) {
            return this.buildStringPreview(data, level, tolerance);
        },
        buildKeyValuePreview: function (data, level, tolerance) {
            var that = this, length = $.glimpse.util.lengthJson(data), rowMax = 2 * tolerance, rowLimit = (rowMax < length ? rowMax : length), i = 1, html = '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object"><span class="start">{</span>';
            for (var key in data) {
                html += that.newItemSpacer(i, rowLimit, length);
                if (i > length || i++ > rowLimit)
                    break;
                html += '<span>\'</span>' + that.buildStringPreview(key, level + 1, 1) + '<span>\'</span><span class="mspace">:</span><span>\'</span>' + that.buildStringPreview(data[key], level + 99, 1) + '<span>\'</span>';
            }
            html += '<span class="end">}</span></div><div class="glimpse-preview-show">' + that.buildKeyValueTable(data, level, true, tolerance) + '</div></td></tr></table>';
            return html;
        },
        buildCustomPreview: function (data, level, tolerance) {
            var that = this, appendTail = true, isComplex = ($.isArray(data[0]) || $.isPlainObject(data[0])), length = (isComplex ? data.length - 1 : data.length), rowMax = 2 * tolerance, columnMax = 3, columnLimit = 1, rowLimit = (rowMax < length ? rowMax : length), html = '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td><div class="glimpse-preview-object"><span class="start">[</span>';

            if (isComplex) {
                columnLimit = ((data[0].length > columnMax) ? columnMax : data[0].length);
                for (var i = 1; i <= rowLimit + 1; i++) {
                    html += that.newItemSpacer(i, rowLimit, length);
                    if (i > length || i > rowLimit)
                        break;

                    html += '<span class="start">[</span>';
                    var spacer = '';
                    for (var x = 0; x < columnLimit; x++) {
                        html += spacer + '<span>\'</span>' + that.buildStringPreview(data[i][x], level + 99, 1) + '<span>\'</span>';
                        spacer = '<span class="rspace">,</span>';
                    }
                    if (x < data[0].length)
                        html += spacer + '<span>...</span>'
                    html += '<span class="end">]</span>';
                }
            }
            else {
                if (data.length > 1) {
                    for (var i = 0; i <= rowLimit; i++) {
                        html += that.newItemSpacer(i + 1, rowLimit, length);
                        if (i >= length || i >= rowLimit)
                            break;
                        html += '<span>\'</span>' + that.buildStringPreview(data[i], level + 99, 1) + '<span>\'</span>';
                    }
                }
                else {
                    appendTail = false;
                    html = that.buildStringPreview(data[0], level + 1, 1);
                }
            }

            if (appendTail)
                html += '<span class="end">]</span></div><div class="glimpse-preview-show">' + that.buildCustomTable(data, level, true, tolerance) + '</div></td></tr></table>';
            return html;
        },
        buildStringPreview: function (data, level, tolerance) {
            if (data == undefined || data == null)
                return '--';
            if ($.isArray(data))
                return "[ ... ]";
            if ($.isPlainObject(data))
                return "{ ... }";

            var that = this,
                charMax = (level > 100 ? 12 : ((level > 1 ? 80 : 150) * tolerance)),
                charOuterMax = (charMax * 1.2),
                content = $.glimpseContent.trimFormatString(data, charMax, charOuterMax, true);

            if (data.length > charOuterMax) {
                content = '<span class="glimpse-preview-string" title="' + $.glimpseContent.trimFormatString(data, charMax * 2, charMax * 2.1, false, true) + '">' + content + '</span>';
                if (level < 100)
                    content = '<table class="glimpse-preview-table"><tr><td class="glimpse-preview-cell"><div class="glimpse-expand"></div></td><td>' + content + '<span class="glimpse-preview-show">' + $.glimpse.util.preserveWhitespace($.glimpseContent.formatString(data)) + '</span></td></tr></table>';
            }
            else 
                content = $.glimpse.util.preserveWhitespace(content);  
              
            return content;
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
                    listener.callback(data)
            }
        },
        _executeLayoutListeners: function (g, isInit) {
            var i = 0, listeners = g.plugins.layoutListeners, static = g.static, tabStrip = static.tabStrip(), panelHolder = static.panelHolder();
            for (; i < listeners.length; i++) {
                var listener = listeners[i];
                if (isInit || !listener.onInitOnly)
                    listener.callback(tabStrip, panelHolder)
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

                //Save selection
                settings.activeTab = item.data('sort');
                g.persistState();
            });

            //Resize
            $('.glimpse-resizer').resizer(settings.height);

            g._wireCommonPluginEvents(g);

            //Resize panels if we are in popup
            if (static.isPopup) {
                $(window).resize(function () {
                    $('.glimpse-holder .glimpse-panel').height($(window).height() - 54);
                });
            }
            $(window).unload(function () {
                g.popup.close();
            })
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
        _wireCallback: function (g) {
            //Remember height 
            $.glimpseResize.static.endDragCallback = function () {
                g.settings.height = $('.glimpse-holder').height();
                g.persistState();

                $('.glimpse-spacer').height(g.settings.height);
                $('.glimpse-holder .glimpse-panel').height(g.settings.height - 54);
            }
        },
        _adjustLayout: function (g) {
            $('.glimpse-spacer').height(g.settings.height);
            $('.glimpse-holder .glimpse-panel').height(g.settings.height - 54);
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

            static.isPopup = window.location.href.indexOf(static.popupUrl) > -1;

            if (!data) {
                if (static.isPopup && window.opener.glimpse) {
                    $.glimpse.util.cookie('glimpseKeepPopup', '');
                    $.glimpse.static.url = window.opener.jQueryGlimpse.glimpse.static.url;
                    glimpse = data = JSON.parse(window.opener.jQueryGlimpse.glimpse.static.dataString)
                }
                else
                    return;
            }

            g.static.data = data;
            g.settings = $.extend(g.settings, $.glimpse.util.cookie('glimpseOptions'));

            g._executeProtocolListeners(g, true);

            $('body').append(static.html.plugin);

            g._wireEvents(g);
            g._wireCallback(g);

            $.glimpseProcessor.layout(g, $.glimpseProcessor.buildHeading(static.url, static.clientName, ''));

            g._executeLayoutListeners(g, true);

            if (!static.isPopup)
                $('body').append('<div class="glimpse-spacer"></div>');

            g.restoreState();

            if (!static.isPopup && g.settings.popupOn)
                g.popup.open();
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
            var padding = function (t) { return t < 10 ? '0' + t : t; }
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
        send: function () {
            $.glimpseAjax.callStarted(this);
        },
        finish: function () {
            $.glimpseAjax.callFinished(this);
        }
    };

    var XMLHttpRequestWrapper = function (activeXObject) {
        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        // XMLHttpRequestWrapper internal variables

        var xhrRequest = (typeof activeXObject != "undefined" ? activeXObject : new _XMLHttpRequest()),
            spy = new XHRSpy(), that = this;

        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        // XMLHttpRequestWrapper internal methods

        var finishXHR = function () {
            var duration = new Date().getTime() - spy.startTime;
            var success = xhrRequest.status == 200;

            //Pull out the header information
            var responseHeadersText = xhrRequest.getAllResponseHeaders();
            var responses = responseHeadersText ? responseHeadersText.split(/[\n\r]/) : [];
            var reHeader = /^(\S+):\s*(.*)/;
            for (var i = 0, l = responses.length; i < l; i++) {
                var match = responses[i].match(reHeader);
                if (match)
                    spy.responseHeaders[match[1]] = match[2];
            }

            //Trigger the finish a bit latter
            setTimeout(function () { spy.finish(); }, 200);

            //Get the rest of the information
            spy.success = success;
            spy.loaded = true;
            spy.status = xhrRequest.status;
            spy.statusText = xhrRequest.statusText;
            spy.responseText = xhrRequest.responseText;
            spy.duration = duration;
        };

        var handleStateChange = function () {
            that.readyState = xhrRequest.readyState;

            if (xhrRequest.readyState == 4) {
                that.statusText = xhrRequest.statusText;
                that.status = xhrRequest.status;
                that.response = xhrRequest.response;
                that.responseText = xhrRequest.responseText;
                that.responseType = xhrRequest.responseType;
                that.responseXML = xhrRequest.responseXML;

                finishXHR();
                xhrRequest.onreadystatechange = function () { };
            }
            that.onreadystatechange();
        };

        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        // XMLHttpRequestWrapper public properties and handlers

        this.readyState = 0;

        this.onreadystatechange = function () { };

        this.response = null;
        this.responseText = null;
        this.responseType = null;
        this.responseXML = null;
        this.status = null;
        this.statusText = null;

        // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        // XMLHttpRequestWrapper public methods

        this.open = function (method, url, async) {
            if (spy.loaded)
                spy = new XHRSpy();
            spy.method = method;
            spy.url = url;
            spy.href = url;
            spy.async = async;
            spy.xhrRequest = xhrRequest;
            //spy.urlParams = parseURLParamsArray(url);

            if (!$.browser.msie && async)
                xhrRequest.onreadystatechange = handleStateChange;

            // xhr.open.apply not available in IE
            if (xhrRequest.open.apply)                                              //TODO: Need to see if this applies
                xhrRequest.open.apply(xhrRequest, arguments)
            else
                xhrRequest.open(method, url, async);

            if ($.browser.msie && async)
                xhrRequest.onreadystatechange = handleStateChange;

        };

        this.send = function (data) {
            spy.data = data;
            spy.startTime = new Date().getTime();

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
                    finishXHR();
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
            return xhrRequest.abort();
        };

        return this;
    };

    var _ActiveXObject;
    if ($.browser.msie && $.browser.version == "6.0") {
        window._ActiveXObject = window.ActiveXObject;
        window.ActiveXObject = function (name) {
            var error = null;

            try {
                var activeXObject = new window._ActiveXObject(name);
            }
            catch (e) {
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
        window.XMLHttpRequest = function () {
            return new XMLHttpRequestWrapper();
        }
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

            data[ga.static.key] = ''

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
            var gm = this;

            //Info tab
            data[gm.static.key.info] = '<div class="glimpse-info-title"><a href="http://getGlimpse.com/" target="_blank"><img border="0" src="' + glimpsePath + 'logo.png" /></a></div><div>v0.83</div><div class="glimpse-info-quote">"What Firebug is for the client, Glimpse is for the server"</div><div class="glimpse-info-more">Go to your Glimpse Config page <a href="' + glimpsePath + 'Config" target="_blank">Glimpse.axd</a></div><div class="glimpse-info-more">For more info see <a href="http://getGlimpse.com" target="_blank">http://getGlimpse.com</a></div><div style="margin:1.5em 0 0.5em;">Created by <strong>Anthony van der Hoorn</strong> (<a href="http://twitter.com/anthony_vdh" target="_blank">@anthony_vdh</a>) and <strong>Nik Molnar</strong> (<a href="http://twitter.com/nikmd23" target="_blank">@nikmd23</a>) - &copy; getglimpse.com 2011</div><div>Have a <em>feature</em> request? <a href="http://getglimpse.uservoice.com" target="_blank">Submit the idea</a>. &nbsp; &nbsp; Found an <em>error</em>? <a href="https://github.com/glimpse/glimpse/issues" target="_blank">Help us improve</a>. &nbsp; &nbsp; Have a <em>question</em>? <a href="http://twitter.com/#search?q=%23glimpse" target="_blank">Tweet us using #glimpse</a>.</div>';
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

    //#region $.glimpseServerSwitcher

    $.glimpseServerSwitcher = {};
    $.extend($.glimpseServerSwitcher, {
        init: function () {
            var gs = this;

            //Wire up plugin  
            $.glimpse.addLayoutListener(function (tabStrip, panelHolder) { gs.adjustLayout(); }, true);
            $('.glimpse').live('glimpse.request.change', function (ev, type) { if (type != 'ajax') { gs.adjustLayout(); } });
        },
        adjustLayout: function () {
            var g = $.glimpse, mainHolder = g.static.mainHolder(), environments = ((metadata = g.static.data._metadata) && (metadata = metadata.request) && (metadata = metadata.environmentUrls));

            if (environments) {
                var currentEnvironment, environmentsList = '';
                for (name in environments) {
                    if (environments[name] === unescape(window.location.href)) {
                        currentEnvironment = name;
                        environmentsList += ' - ' + name + ' (Current)<br />';
                    }
                    else
                        environmentsList += ' - <a title="Go to - ' + environments[name] + '" href="' + environments[name] + '">' + name + ' - ' + environments[name] + '</a><br />';
                }

                if (currentEnvironment) {
                    $('.glimpse-title span:last', mainHolder)
                        .prepend('<a class="glimpse-switch">' + currentEnvironment + '</a><div class="glimpse-switch glimpse-switch-over"><div>Switch Servers</div>' + environmentsList + '</div>');
                    $('.glimpse-title span:last .glimpse-switch-over', mainHolder).mouseleave(function () {
                        $('.glimpse-switch-over', mainHolder).hide();
                    });
                    $('.glimpse-title span:last a.glimpse-switch', mainHolder).mouseover(function () {
                        $('.glimpse-switch-over', mainHolder).show();
                    });
                }
            }
        }
    });

    //Wireup glimpse offical plugins
    $.glimpseServerSwitcher.init();

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

    //#region Ready

    //Run glimpse
    $(document).ready(function () {
        $.glimpse.init(glimpse);
    });

    //#endregion

})(jQueryGlimpse); }   
