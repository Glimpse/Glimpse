
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
 
    //#region $.glimpse.util

    $.extend($.glimpse.util, {
        formatTime: function (d) {
            if (typeof d === 'number')
                d = new Date(d);
            var padding = function(t) { return t < 10 ? '0' + t : t; };
            return d.getHours() + ':' + padding(d.getMinutes()) + ':' + padding(d.getSeconds()) + ' ' + d.getMilliseconds();
        },  
        timeConvert:function(value) {
            if (value < 1000)
                return value + 'ms';
            return Math.round(value / 10) / 100 + 's';
        }
    });

    //#endregion
 
    //#region $.glimpseAjax
 
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
                scope.html('<div class="glimpse-timeline"><div class="glimpse-tl-row-summary"><div class="glimpse-tl-content-scroll"><div class="glimpse-tl-event-desc-holder glimpse-tl-col-side"><div class="glimpse-tl-band glimpse-tl-band-title">Categories<span>[Switch view]</span></div><div class="glimpse-tl-event-desc-group"></div></div><div class="glimpse-tl-band-holder glimpse-tl-col-main"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-band-group"></div></div><div class="glimpse-tl-event-holder glimpse-tl-col-main"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-event-group"></div></div></div><div class="glimpse-tl-padding-holder glimpse-tl-col-main"><div class="glimpse-tl-padding glimpse-tl-padding-l glimpse-tl-summary-height"></div><div class="glimpse-tl-padding glimpse-tl-padding-r glimpse-tl-summary-height"></div></div><div class="glimpse-tl-divider-holder glimpse-tl-col-main"><div class="glimpse-tl-divider-title-bar"></div><div class="glimpse-tl-divider-line-holder"></div></div><div class="glimpse-tl-resizer-holder glimpse-tl-col-main"><div class="glimpse-tl-resizer glimpse-tl-resizer-l glimpse-tl-summary-height"><div class="glimpse-tl-resizer-bar"></div><div class="glimpse-tl-resizer-handle"></div></div><div class="glimpse-tl-resizer glimpse-tl-resizer-r glimpse-tl-summary-height"><div class="glimpse-tl-resizer-bar"></div><div class="glimpse-tl-resizer-handle"></div></div></div></div><div class="glimpse-tl-row-spacer"></div><div class="glimpse-tl-row-content"><div class="glimpse-tl-content-scroll"><div class="glimpse-tl-band-holder glimpse-tl-col-main"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-band-group"></div></div><div class="glimpse-tl-divider-holder glimpse-tl-col-main"><div class="glimpse-tl-divider-zero-holder"><div class="glimpse-tl-divider"></div></div><div class="glimpse-tl-divider-line-holder"></div></div><div class="glimpse-tl-event-holder glimpse-tl-col-main"><div class="glimpse-tl-event-holder-inner"><div class="glimpse-tl-band glimpse-tl-band-title"></div><div class="glimpse-tl-event-group"></div></div></div><div class="glimpse-tl-event-desc-holder glimpse-tl-col-side"><div class="glimpse-tl-band glimpse-tl-band-title">Events</div><div class="glimpse-tl-event-desc-group"></div></div></div><div class="glimpse-tl-content-overlay"><div class="glimpse-tl-divider-holder glimpse-tl-col-main"><div class="glimpse-tl-divider-title-bar"></div><div class="glimpse-tl-divider-zero-holder"><div class="glimpse-tl-divider"><div>0</div></div></div><div class="glimpse-tl-divider-line-holder"></div></div></div><div class="glimpse-tl-resizer"><div></div></div><div class="glimpse-tl-content-scroll" style="display:none"><div class="glimpse-tl-table-holder"></div></div></div><div class="glimpse-tl-event-info"></div></div>');
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
                    var result = $.glimpseProcessor.build(dataResult, 0, true, metadata, false); 
                    elements.contentTableHolder.append(result);

                    //Update the output 
                    elements.contentTableHolder.find('tbody tr').each(function(i) {
                        var row = $(this),
                            event = settings.events[i],  
                            category = settings.category[event.category];
                             
                        row.find('td:first-child').prepend($('<div class="glimpse-tl-event"></div>').css({ 'backgroundColor' : category.eventColor, marginLeft : (15 * event.nesting) + 'px', 'border' : '1px solid ' + category.eventColorHighlight }));
                        row.find('td:nth-child(3)').css('position', 'relative').prepend($('<div class="glimpse-tl-event"></div>').css({ 'backgroundColor' : category.eventColor, 'border' : '1px solid ' + category.eventColorHighlight, 'left' : event.startPersent + '%', width : event.widthPersent + '%', position : 'absolute', top : '5px' })); 
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
                    var contentHeight = height - (elements.summaryRow.height() + scope.find('.glimpse-tl-row-spacer').height() + 2);  
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
                    var showTimeline = !($.glimpse.settings.timeView);

                    apply(showTimeline);
                 
                    $.glimpse.settings.timeView = showTimeline;
                    $.glimpse.persistState();
                },
                start = function() {
                    apply($.glimpse.settings.timeView, true);
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
        { category :'MVC', startTime :'', startPoint :75, duration :55, title :'Filter', subText :'Validation', pluginContextId :'', plugin :'', details :{} },
        { category :'Database', startTime :'', startPoint :80, duration :45, title :'Transaction', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Trace', startTime :'', startPoint :100, duration :6, title :'Logon', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Routes', startTime :'', startPoint :134, duration :4, title :'Resolved Routes', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Trace', startTime :'', startPoint :138, duration :0, title :'Registered', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Trace', startTime :'', startPoint :142, duration :0, title :'Socket Open', subText :'', pluginContextId :'', plugin :'', details :{} },
        { category :'Routes', startTime :'', startPoint :143, duration :30, title :'Partial View', subText :'', pluginContextId :'', plugin :'', details :{} },
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
        'Routes' : { eventColor : '#10E309', eventColorHighlight : '#0EC41D' } //{ event:'green' }
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
 
})(jQueryGlimpse); }   
