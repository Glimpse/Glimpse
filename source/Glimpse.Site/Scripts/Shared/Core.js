
var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-22715154-1']);
_gaq.push(['_trackPageview']);

(function() {
var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})(); 





var uservoiceOptions = {
    /* required */
    key: 'getglimpse',
    host: 'getglimpse.uservoice.com', 
    forum: '1',
    showTab: true,  
    /* optional */
    alignment: 'right',
    background_color:'#267A9D', 
    text_color: 'white',
    hover_color: '#1A4157',
    lang: 'en'
};

function _loadUserVoice() {
    var s = document.createElement('script');
    s.setAttribute('type', 'text/javascript');
    s.setAttribute('src', ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.uservoice.com/javascripts/widgets/tab.js");
    document.getElementsByTagName('head')[0].appendChild(s);
}
_loadSuper = window.onload;
window.onload = (typeof window.onload != 'function') ? _loadUserVoice : function() { _loadSuper(); _loadUserVoice(); };