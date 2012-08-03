
var glimpseTest = (function ($) {
    var //Support 
        testHandlers = {},
        pager = function () {
            var generate = function (data) {
                    var pagingInfo = { pageSize : 5, pageIndex : 1, totalNumberOfRecords : 31 },
                        result = [['Title 1', 'Title 2', 'Title 3']],
                        start = data.pageIndex * pagingInfo.pageSize,
                        end = start + pagingInfo.pageSize,
                        random = start;
                        
                    if (end > pagingInfo.totalNumberOfRecords)
                        end = pagingInfo.totalNumberOfRecords;
        
                    for (var i = start; i < end; i++) 
                        result.push([random++, random++, random++]);
        
                    return result;
                },
                trigger = function (param, data) {
                    param.complete();
        
                    setTimeout(function () {
                        param.success(generate(data));
                    }, 300);
                };
        
            return {
                trigger : trigger
            };
        } (), 
        data = function () {
            var 
                metadata = {
                    "environmentUrls":{"Dev":"http://localhost/","QA":"http://qa.getglimpse.com/","Prod":"http://getglimpse.com/"},
                    "version":[{name: 'core', current: '0.85', channel: 'dev'}, {name: 'glimpse.mvc', current: '0.85', channel: 'dev'}, {name: 'glimpse.webforms', current: '0.85', channel: 'dev'}],
                    "plugins":{"Paging":{"pagingInfo":{ pagerType : 'continuous', pageSize : 5, pageIndex : 0, totalNumberOfRecords : 31 }}, "Lazy":{},"Server":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Server"},"Session":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Session"},"Request":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Request"},"Trace":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Trace"},"Config":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Config"},"Environment":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Environment"},"SQL":{"documentationUri":"http://getGlimpse.com/Help/Plugin/EF","structure":[[{"forceFull":true,"data":0,"structure":[[{"span":6,"forceFull":true,"minDisplay":true,"data":0,"structure":[[{"width":"150px","key":true,"data":0},{"data":1}]]}],[{"width":"55px","data":1},{"isCode":true,"codeType":"sql","data":2},{"width":"25%","data":3},{"width":"60px","data":4},{"width":"100px","post":" ms","className":"mono","data":5},{"width":"70px","pre":"T+ ","post":" ms","className":"mono","data":6}],[{"span":6,"forceFull":true,"minDisplay":true,"data":8,"structure":[[{"width":"20%","data":0},{"className":"mono","data":1}]]}],[{"span":6,"forceFull":true,"minDisplay":true,"data":7,"structure":[[{"width":"150px","key":true,"data":0},{"data":1}]]}]]},{"width":"75px","post":" ms","className":"mono","data":1}]]},"Routes":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Routes"},"Binding":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Binders"},"Views":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Views"},"Execution":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Execution"},"MetaData":{"documentationUri":"http://getGlimpse.com/Help/Plugin/MetaData","structure":[[{"width":"150px","data":0},{"width":"25%","data":1},{"data":2}]]}}
                },
                data = {
                    "Session":{ name : 'Session', data : [["Key","Value","Type"],["CartId","2dc2f24f-5816-4e8e-bc70-2438ce628be8","string"],["__ControllerTempData",{"Test":"A bit of temp"},"System.Collections.Generic.Dictionary<string,object>"]] },
                    "Server":{ name : 'Server', data : {"ALL_HTTP":"HTTP_CONNECTION:keep-alive\r\nHTTP_ACCEPT:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\nHTTP_ACCEPT_CHARSET:ISO-8859-1,utf-8;q=0.7,*;q=0.3\r\nHTTP_ACCEPT_ENCODING:gzip,deflate,sdch\r\nHTTP_ACCEPT_LANGUAGE:en-US,en;q=0.8\r\nHTTP_COOKIE:glimpseClientName=null; glimpseState=On; ASP.NET_SessionId=1k33gr33dxqvzw3tm41koizh; prgLocation=/StoreManager/; prgLocationRedirect=/Account/LogOn?ReturnUrl=%2fStoreManager%2f; prgLocationId=25620064-8eb8-4879-842d-092a99b2b0ef; prgLocationMethod=GET; glimpseOptions=%7B%220%22%3A%22n%22%2C%221%22%3A%22u%22%2C%222%22%3A%22l%22%2C%223%22%3A%22l%22%2C%22open%22%3Atrue%2C%22height%22%3A447%2C%22activeTab%22%3A%22Views%22%2C%22popupOn%22%3Afalse%2C%22firstPopup%22%3Atrue%2C%22timeView%22%3Afalse%7D\r\nHTTP_HOST:localhost:33333\r\nHTTP_USER_AGENT:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.83 Safari/535.2\r\n","ALL_RAW":"Connection: keep-alive\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\nAccept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3\r\nAccept-Encoding: gzip,deflate,sdch\r\nAccept-Language: en-US,en;q=0.8\r\nCookie: glimpseClientName=null; glimpseState=On; ASP.NET_SessionId=1k33gr33dxqvzw3tm41koizh; prgLocation=/StoreManager/; prgLocationRedirect=/Account/LogOn?ReturnUrl=%2fStoreManager%2f; prgLocationId=25620064-8eb8-4879-842d-092a99b2b0ef; prgLocationMethod=GET; glimpseOptions=%7B%220%22%3A%22n%22%2C%221%22%3A%22u%22%2C%222%22%3A%22l%22%2C%223%22%3A%22l%22%2C%22open%22%3Atrue%2C%22height%22%3A447%2C%22activeTab%22%3A%22Views%22%2C%22popupOn%22%3Afalse%2C%22firstPopup%22%3Atrue%2C%22timeView%22%3Afalse%7D\r\nHost: localhost:33333\r\nUser-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.83 Safari/535.2\r\n","APPL_MD_PATH":"","APPL_PHYSICAL_PATH":"C:\\Users\\avanderhoorn\\Glimpse\\source\\Glimpse.Sample.MVC3\\","AUTH_TYPE":"","AUTH_USER":"","AUTH_PASSWORD":"","LOGON_USER":"CORP\\AVanDerHoorn","REMOTE_USER":"","CERT_COOKIE":"","CERT_FLAGS":"","CERT_ISSUER":"","CERT_KEYSIZE":"","CERT_SECRETKEYSIZE":"","CERT_SERIALNUMBER":"","CERT_SERVER_ISSUER":"","CERT_SERVER_SUBJECT":"","CERT_SUBJECT":"","CONTENT_LENGTH":"0","CONTENT_TYPE":"","GATEWAY_INTERFACE":"","HTTPS":"","HTTPS_KEYSIZE":"","HTTPS_SECRETKEYSIZE":"","HTTPS_SERVER_ISSUER":"","HTTPS_SERVER_SUBJECT":"","INSTANCE_ID":"","INSTANCE_META_PATH":"","LOCAL_ADDR":"127.0.0.1","PATH_INFO":"/","PATH_TRANSLATED":"C:\\Users\\avanderhoorn\\Glimpse\\source\\Glimpse.Sample.MVC3","QUERY_STRING":"","REMOTE_ADDR":"127.0.0.1","REMOTE_HOST":"127.0.0.1","REMOTE_PORT":"","REQUEST_METHOD":"GET","SCRIPT_NAME":"/","SERVER_NAME":"localhost","SERVER_PORT":"33333","SERVER_PORT_SECURE":"0","SERVER_PROTOCOL":"HTTP/1.1","SERVER_SOFTWARE":"","URL":"/","HTTP_CONNECTION":"keep-alive","HTTP_ACCEPT":"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8","HTTP_ACCEPT_CHARSET":"ISO-8859-1,utf-8;q=0.7,*;q=0.3","HTTP_ACCEPT_ENCODING":"gzip,deflate,sdch","HTTP_ACCEPT_LANGUAGE":"en-US,en;q=0.8","HTTP_COOKIE":"glimpseClientName=null; glimpseState=On; ASP.NET_SessionId=1k33gr33dxqvzw3tm41koizh; prgLocation=/StoreManager/; prgLocationRedirect=/Account/LogOn?ReturnUrl=%2fStoreManager%2f; prgLocationId=25620064-8eb8-4879-842d-092a99b2b0ef; prgLocationMethod=GET; glimpseOptions=%7B%220%22%3A%22n%22%2C%221%22%3A%22u%22%2C%222%22%3A%22l%22%2C%223%22%3A%22l%22%2C%22open%22%3Atrue%2C%22height%22%3A447%2C%22activeTab%22%3A%22Views%22%2C%22popupOn%22%3Afalse%2C%22firstPopup%22%3Atrue%2C%22timeView%22%3Afalse%7D","HTTP_HOST":"localhost:33333","HTTP_USER_AGENT":"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.83 Safari/535.2"} },
                    "Request":{ name : 'Request', data : {"Cookies":[["Name","Path","Secure","Value"],["glimpseClientName","/","False","null"],["glimpseState","/","False","On"],["ASP.NET_SessionId","/","False","1k33gr33dxqvzw3tm41koizh"],["prgLocation","/","False","/StoreManager/"],["prgLocationRedirect","/","False","/Account/LogOn?ReturnUrl=/StoreManager/"],["prgLocationId","/","False","25620064-8eb8-4879-842d-092a99b2b0ef"],["prgLocationMethod","/","False","GET"],["glimpseOptions","/","False","{\"0\":\"n\",\"1\":\"u\",\"2\":\"l\",\"3\":\"l\",\"open\":true,\"height\":447,\"activeTab\":\"Views\",\"popupOn\":false,\"firstPopup\":true,\"timeView\":false}"]],"Form":null,"QueryString":null,"InputStream":null,"CurrentUICulture":"en-US","ApplicationPath":"/","AppRelativeCurrentExecutionFilePath":"~/","CurrentExecutionFilePath":"/","FilePath":"/","Path":"/","PathInfo":"","PhysicalApplicationPath":"C:\\Users\\avanderhoorn\\Glimpse\\source\\Glimpse.Sample.MVC3\\","PhysicalPath":"C:\\Users\\avanderhoorn\\Glimpse\\source\\Glimpse.Sample.MVC3","RawUrl":"/","Url":"http://localhost:33333/","UrlReferrer":null,"UserAgent":"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.83 Safari/535.2","UserHostAddress":"127.0.0.1","UserHostName":"127.0.0.1"} },
                    "Trace":{ name : 'Trace', data : [["Category","Message","From First","From Last"],["Info","IDependencyResolver.GetService<System.Web.Mvc.IControllerActivator>() = null",null,null,"info"],["Info","CreateControllerInterceptor.CreateController(requestContext, \"Home\") = MvcMusicStore.Controllers.HomeController","86 ms","86 ms","info"],["Info","IDependencyResolver.GetServices<System.Web.Mvc.IFilterProvider>() = null","99 ms","12 ms","info"],["Info","Got top 5 albums","1818 ms","1719 ms","info"],["Info","This is info from Glimpse","1820 ms","1 ms","info"],["Warn","This is warn from Glimpse at 10/9/2011 1:20:08 PM","1821 ms","0 ms","warn"],["Error","This is error from MvcMusicStore.Controllers.HomeController","1822 ms","0 ms","error"],["Fail","This is Fail from Glimpse","1822 ms","0 ms","fail"],["Warning","Test TraceWarning;","1824 ms","1 ms","warn"],["Error","Test TraceError;","1825 ms","1 ms","error"],["Information","Test TraceInformation;","1826 ms","1 ms","info"],["Info","IDependencyResolver.GetServices<System.Web.Mvc.IViewEngine>() = null","1829 ms","3 ms","info"],["Info","IDependencyResolver.GetService<System.Web.Mvc.IViewPageActivator>() = null","2335 ms","506 ms","info"],["Info","IDependencyResolver.GetService<ASP._Page_Views_Home_Index_cshtml>() = ASP._Page_Views_Home_Index_cshtml","2338 ms","3 ms","info"],["Info","CreateControllerInterceptor.CreateController(requestContext, \"ShoppingCart\") = MvcMusicStore.Controllers.ShoppingCartController","3498 ms","1159 ms","info"],["Info","IDependencyResolver.GetService<ASP._Page_Views_ShoppingCart_CartSummary_cshtml>() = ASP._Page_Views_ShoppingCart_CartSummary_cshtml","3964 ms","465 ms","info"],["Info","CreateControllerInterceptor.CreateController(requestContext, \"Store\") = MvcMusicStore.Controllers.StoreController","4007 ms","42 ms","info"],["Info","IDependencyResolver.GetService<ASP._Page_Views_Store_GenreMenu_cshtml>() = ASP._Page_Views_Store_GenreMenu_cshtml","4859 ms","851 ms","info"],["Info","IDependencyResolver.GetService<System.Web.Mvc.ModelMetadataProvider>() = null","5247 ms","388 ms","info"]],"Config":{"AppSettings":{"ClientValidationEnabled":"true","UnobtrusiveJavaScriptEnabled":"true"},"ConnectionStrings":{"LocalSqlServer":"data source=.\\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true","MusicStoreEntities":"Data Source=|DataDirectory|MvcMusicStore.sdf"},"CustomErrors":{"Mode":"RemoteOnly","RedirectMode":"ResponseRedirect","DefaultRedirect":"/Opps/Problem/","Errors":{"404":"/Opps/NotFound/","401":"/Opps/NotAllowed/","500":"/Opps/BadProgram/"}},"Authentication":{"Mode":"Forms","Cookieless":"UseDeviceProfile","DefaultUrl":"default.aspx","Domain":null,"EnableCrossAppRedirects":"False","LoginUrl":"~/Account/LogOn","Name":".ASPXAUTH","Path":"/","Protection":"All","RequireSSL":"False","SlidingExpiration":"True","TicketCompatibilityMode":"Framework20","Timeout":"2.00:00:00"}} },
                    "Environment":{ name : 'Environment', data : {"Environment Name":"Dev","Machine":[["Name","Operating System","Start Time"],["NN-AVANDERHOORN (4 processors)","Microsoft Windows NT 6.1.7600.0 (64 bit)",new Date(1318007390840)]],"Web Server":[["Type","Integrated Pipeline"],["Visual Studio Web Development Server","False"]],"Framework":[[".NET Framework","Debugging","Server Culture","Current Trust Level"],[".NET 4.0.30319.225 (32 bit)","True","en-US","Unrestricted"]],"Process":[["Worker Process","Process ID","Start Time"],["WebDev.WebServer40.exe",6372,new Date(1318180783512)]],"Timezone":[["Current","Is Daylight Saving","UtcOffset w/DLS"],["(UTC-05:00) Eastern Time (US & Canada)","True",-4]],"Application Assemblies":[["Name","Version","Culture","From GAC","Full Trust"],["Anonymously Hosted DynamicMethods Assembly","0.0.0.0","_neutral_","False","True"],["App_global.asax.aomqkliw","0.0.0.0","_neutral_","False","True"],["App_Web_fv3jvfpv","0.0.0.0","_neutral_","False","True"],["App_Web_mwskk53m","0.0.0.0","_neutral_","False","True"],["App_Web_nbkzhuxw","0.0.0.0","_neutral_","False","True"],["App_Web_twmmx5az","0.0.0.0","_neutral_","False","True"],["App_Web_ugeopa2n","0.0.0.0","_neutral_","False","True"],["Castle.Core","2.5.1.0","_neutral_","False","True"],["CppCodeProvider","10.0.0.0","_neutral_","True","True"],["DynamicProxyGenAssembly2","0.0.0.0","_neutral_","False","True"],["DynamicProxyGenAssembly2","0.0.0.0","_neutral_","False","True"],["DynamicProxyGenAssembly2","0.0.0.0","_neutral_","False","True"],["DynamicProxyGenAssembly2","0.0.0.0","_neutral_","False","True"],["DynamicProxyGenAssembly2","0.0.0.0","_neutral_","False","True"],["EntityFramework","4.1.0.0","_neutral_","False","True"],["EntityFrameworkDynamicProxies-EntityFramework","1.0.0.0","_neutral_","False","True"],["EntityFrameworkDynamicProxies-MvcMusicStore","1.0.0.0","_neutral_","False","True"],["Glimpse.Core","0.85.0.0","_neutral_","False","True"],["Glimpse.Ef","0.0.5.0","_neutral_","False","True"],["Glimpse.Mvc3","0.85.0.0","_neutral_","False","True"],["LukeSkywalker.IPNetwork","1.3.1.0","_neutral_","False","True"],["MetadataViewProxies_89c1cd79-c764-49ee-94b0-36eaf451d055","0.0.0.0","_neutral_","False","True"],["mnml2iog","0.0.0.0","_neutral_","False","True"],["mscorlib","4.0.0.0","_neutral_","True","True"],["MvcMusicStore","1.0.0.0","_neutral_","False","True"],["Newtonsoft.Json.Net35","4.0.2.0","_neutral_","False","True"],["NLog","2.0.0.0","_neutral_","False","True"],["NLog.Extended","2.0.0.0","_neutral_","False","True"],["SMDiagnostics","4.0.0.0","_neutral_","True","True"],["WebDev.WebHost40","10.0.0.0","_neutral_","True","True"]],"System Assemblies":[["Name","Version","Culture","From GAC","Full Trust"],["Microsoft.Build.Framework","4.0.0.0","_neutral_","True","True"],["Microsoft.Build.Utilities.v4.0","4.0.0.0","_neutral_","True","True"],["Microsoft.CSharp","4.0.0.0","_neutral_","True","True"],["Microsoft.JScript","10.0.0.0","_neutral_","True","True"],["Microsoft.VisualBasic.Activities.Compiler","10.0.0.0","_neutral_","True","True"],["Microsoft.Web.Infrastructure","1.0.0.0","_neutral_","True","True"],["System","4.0.0.0","_neutral_","True","True"],["System.Activities","4.0.0.0","_neutral_","True","True"],["System.ComponentModel.Composition","4.0.0.0","_neutral_","True","True"],["System.ComponentModel.DataAnnotations","4.0.0.0","_neutral_","True","True"],["System.Configuration","4.0.0.0","_neutral_","True","True"],["System.Core","4.0.0.0","_neutral_","True","True"],["System.Data","4.0.0.0","_neutral_","True","True"],["System.Data.DataSetExtensions","4.0.0.0","_neutral_","True","True"],["System.Data.Entity","4.0.0.0","_neutral_","True","True"],["System.Data.Linq","4.0.0.0","_neutral_","True","True"],["System.Data.OracleClient","4.0.0.0","_neutral_","True","True"],["System.Data.Services.Design","4.0.0.0","_neutral_","True","True"],["System.Data.SqlServerCe","3.5.1.0","_neutral_","True","True"],["System.Data.SqlServerCe","4.0.0.0","_neutral_","True","True"],["System.Data.SqlServerCe.Entity","4.0.0.0","_neutral_","True","True"],["System.Data.SqlXml","4.0.0.0","_neutral_","True","True"],["System.DirectoryServices","4.0.0.0","_neutral_","True","True"],["System.DirectoryServices.Protocols","4.0.0.0","_neutral_","True","True"],["System.Drawing","4.0.0.0","_neutral_","True","True"],["System.Dynamic","4.0.0.0","_neutral_","True","True"],["System.EnterpriseServices","4.0.0.0","_neutral_","True","True"],["System.IdentityModel","4.0.0.0","_neutral_","True","True"],["System.Messaging","4.0.0.0","_neutral_","True","True"],["System.Numerics","4.0.0.0","_neutral_","True","True"],["System.Runtime.Caching","4.0.0.0","_neutral_","True","True"],["System.Runtime.DurableInstancing","4.0.0.0","_neutral_","True","True"],["System.Runtime.Serialization","4.0.0.0","_neutral_","True","True"],["System.Security","4.0.0.0","_neutral_","True","True"],["System.ServiceModel","4.0.0.0","_neutral_","True","True"],["System.ServiceModel.Activation","4.0.0.0","_neutral_","True","True"],["System.ServiceModel.Activities","4.0.0.0","_neutral_","True","True"],["System.ServiceModel.Web","4.0.0.0","_neutral_","True","True"],["System.Transactions","4.0.0.0","_neutral_","True","True"],["System.Web","4.0.0.0","_neutral_","True","True"],["System.Web.Abstractions","4.0.0.0","_neutral_","True","True"],["System.Web.ApplicationServices","4.0.0.0","_neutral_","True","True"],["System.Web.DynamicData","4.0.0.0","_neutral_","True","True"],["System.Web.Extensions","4.0.0.0","_neutral_","True","True"],["System.Web.Helpers","1.0.0.0","_neutral_","True","True"],["System.Web.Mobile","4.0.0.0","_neutral_","True","True"],["System.Web.Mvc","3.0.0.0","_neutral_","True","True"],["System.Web.Razor","1.0.0.0","_neutral_","True","True"],["System.Web.RegularExpressions","4.0.0.0","_neutral_","True","True"],["System.Web.Routing","4.0.0.0","_neutral_","True","True"],["System.Web.Services","4.0.0.0","_neutral_","True","True"],["System.Web.WebPages","1.0.0.0","_neutral_","True","True"],["System.Web.WebPages.Deployment","1.0.0.0","_neutral_","True","True"],["System.Web.WebPages.Razor","1.0.0.0","_neutral_","True","True"],["System.Windows.Forms","4.0.0.0","_neutral_","True","True"],["System.Workflow.Activities","4.0.0.0","_neutral_","True","True"],["System.Workflow.ComponentModel","4.0.0.0","_neutral_","True","True"],["System.Workflow.Runtime","4.0.0.0","_neutral_","True","True"],["System.WorkflowServices","4.0.0.0","_neutral_","True","True"],["System.Xaml","4.0.0.0","_neutral_","True","True"],["System.Xml","4.0.0.0","_neutral_","True","True"],["System.Xml.Linq","4.0.0.0","_neutral_","True","True"]]} },
                    "SQL":{ name : 'SQL', data : [["Commands per Connection","Open Time"],[[["Transaction Start","Ordinal","Command","Parameters","Records","Command Time","From First","Transaction End","Errors"],[null,1,"SELECT TOP (1) \r\n[Extent1].[Id] AS [Id], \r\n[Extent1].[ModelHash] AS [ModelHash]\r\nFROM [EdmMetadata] AS [Extent1]\r\nORDER BY [Extent1].[Id] DESC",null,1,30,"0",null,null,""],[null,2,"SELECT TOP (5) \r\n[Project1].[AlbumId] AS [AlbumId], \r\n[Project1].[GenreId] AS [GenreId], \r\n[Project1].[ArtistId] AS [ArtistId], \r\n[Project1].[Title] AS [Title], \r\n[Project1].[Price] AS [Price], \r\n[Project1].[AlbumArtUrl] AS [AlbumArtUrl]\r\nFROM ( SELECT \r\n\t[Extent1].[AlbumId] AS [AlbumId], \r\n\t[Extent1].[GenreId] AS [GenreId], \r\n\t[Extent1].[ArtistId] AS [ArtistId], \r\n\t[Extent1].[Title] AS [Title], \r\n\t[Extent1].[Price] AS [Price], \r\n\t[Extent1].[AlbumArtUrl] AS [AlbumArtUrl], \r\n\t[SSQTAB1].[A1] AS [C1]\r\n\tFROM [Albums] AS [Extent1]\r\n\t OUTER APPLY\r\n\t(SELECT \r\n\t\tCOUNT(1) AS [A1]\r\n\t\tFROM [OrderDetails] AS [Extent2]\r\n\t\tWHERE [Extent1].[AlbumId] = [Extent2].[AlbumId]) AS [SSQTAB1]\r\n)  AS [Project1]\r\nORDER BY [Project1].[C1] DESC",null,5,3,"0",null,null,""]],1536.0],[[["Transaction Start","Ordinal","Command","Parameters","Records","Command Time","From First","Transaction End","Errors"],[null,1,"SELECT \r\n[GroupBy1].[A1] AS [C1]\r\nFROM ( SELECT \r\n\tSUM([Extent1].[Count]) AS [A1]\r\n\tFROM [Carts] AS [Extent1]\r\n\tWHERE [Extent1].[CartId] = '2dc2f24f-5816-4e8e-bc70-2438ce628be8' /* @p__linq__0 */\r\n)  AS [GroupBy1]",[["Name","Value","Type","Size"],["@p__linq__0","2dc2f24f-5816-4e8e-bc70-2438ce628be8","String",0]],1,10,"0",null,null,""]],43.0],[[["Transaction Start","Ordinal","Command","Parameters","Records","Command Time","From First","Transaction End","Errors"],[null,1,"SELECT \r\n[Extent1].[GenreId] AS [GenreId], \r\n[Extent1].[Name] AS [Name], \r\n[Extent1].[Description] AS [Description]\r\nFROM [Genres] AS [Extent1]",null,10,394,"0",null,null,""]],414.0]] },
                    "Routes":{ name : 'Routes', data : [["Match","Area","Url","Data","Constraints","DataTokens"],["False","Test","Test/{controller}/{action}/{id}",null,null,{"Namespaces":["MvcMusicStore.Areas.Test.*"],"area":"Test","UseNamespaceFallback":false},""],["False","Test","Test/{controller}/NeverUsed/{action}/{id}",null,null,{"Namespaces":["MvcMusicStore.Areas.Test.*"],"area":"Test","UseNamespaceFallback":false},""],["False","_Root_","{resource}.axd/{*pathInfo}",null,null,null,""],["False","_Root_","{*favicon}",null,{"favicon":"(.*/)?favicon.ico(/.*)?"},null,""],["True","_Root_","{controller}/{action}/{id}",[["Placeholder","Default Value","Actual Value"],["controller","Home","Home"],["action","Index","Index"],["id","_Optional_","_Optional_"]],null,null,"selected"],["False","_Root_","Never/Used/Route",null,null,null,""]] }
                },
                lazyData = {
                    'Lazy Name' : 'Snoopy',
                    'Lazy Description' : 'Because I said so'
                },
        
                requestData = {
                        'PRG': {
                        method : 'Post',  
                        browser : '',
                        clientName : '',
                        requestTime : '',
                        requestId : 'PRG',
                        isAjax : false,
                        url : '/Help/Feature/Add',  
                        metadata : {
                            "environmentUrls":{"Dev":"http://localhost/","QA":"http://qa.getglimpse.com/","Prod":"http://getglimpse.com/"},
                            "version":[{name: 'core', current: '0.85', channel: 'dev'}, {name: 'glimpse.mvc', current: '0.85', channel: 'dev'}, {name: 'glimpse.webforms', current: '0.85', channel: 'dev'}],
                            "plugins": {
                                "Server":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Server"},
                                "Session":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Session"}, 
                                "Views":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Views"},
                                "Execution":{"documentationUri":"http://getGlimpse.com/Help/Plugin/Execution"},
                                "MetaData":{"documentationUri":"http://getGlimpse.com/Help/Plugin/MetaData"}
                            }
                        },
                        data : {
                                "Session":{ name : 'Session', data : [["Key","Value","Type"],["CartId","2dc2f24f-5816-4e8e-bc70-2438ce628be8","string"],["__ControllerTempData",{"Test":"A bit of temp"},"System.Collections.Generic.Dictionary<string,object>"]] },
                                "Server":{ name : 'Server', data : {"ALL_HTTP":"HTTP_CONNECTION:keep-alive\r\nHTTP_ACCEPT:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\nHTTP_ACCEPT_CHARSET:ISO-8859-1,utf-8;q=0.7,*;q=0.3\r\nHTTP_ACCEPT_ENCODING:gzip,deflate,sdch\r\nHTTP_ACCEPT_LANGUAGE:en-US,en;q=0.8\r\nHTTP_COOKIE:glimpseClientName=null; glimpseState=On; ASP.NET_SessionId=1k33gr33dxqvzw3tm41koizh; prgLocation=/StoreManager/; prgLocationRedirect=/Account/LogOn?ReturnUrl=%2fStoreManager%2f; prgLocationId=25620064-8eb8-4879-842d-092a99b2b0ef; prgLocationMethod=GET; glimpseOptions=%7B%220%22%3A%22n%22%2C%221%22%3A%22u%22%2C%222%22%3A%22l%22%2C%223%22%3A%22l%22%2C%22open%22%3Atrue%2C%22height%22%3A447%2C%22activeTab%22%3A%22Views%22%2C%22popupOn%22%3Afalse%2C%22firstPopup%22%3Atrue%2C%22timeView%22%3Afalse%7D\r\nHTTP_HOST:localhost:33333\r\nHTTP_USER_AGENT:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.83 Safari/535.2\r\n","ALL_RAW":"Connection: keep-alive\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\nAccept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3\r\nAccept-Encoding: gzip,deflate,sdch\r\nAccept-Language: en-US,en;q=0.8\r\nCookie: glimpseClientName=null; glimpseState=On; ASP.NET_SessionId=1k33gr33dxqvzw3tm41koizh; prgLocation=/StoreManager/; prgLocationRedirect=/Account/LogOn?ReturnUrl=%2fStoreManager%2f; prgLocationId=25620064-8eb8-4879-842d-092a99b2b0ef; prgLocationMethod=GET; glimpseOptions=%7B%220%22%3A%22n%22%2C%221%22%3A%22u%22%2C%222%22%3A%22l%22%2C%223%22%3A%22l%22%2C%22open%22%3Atrue%2C%22height%22%3A447%2C%22activeTab%22%3A%22Views%22%2C%22popupOn%22%3Afalse%2C%22firstPopup%22%3Atrue%2C%22timeView%22%3Afalse%7D\r\nHost: localhost:33333\r\nUser-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.83 Safari/535.2\r\n","APPL_MD_PATH":"","APPL_PHYSICAL_PATH":"C:\\Users\\avanderhoorn\\Glimpse\\source\\Glimpse.Sample.MVC3\\","AUTH_TYPE":"","AUTH_USER":"","AUTH_PASSWORD":"","LOGON_USER":"CORP\\AVanDerHoorn","REMOTE_USER":"","CERT_COOKIE":"","CERT_FLAGS":"","CERT_ISSUER":"","CERT_KEYSIZE":"","CERT_SECRETKEYSIZE":"","CERT_SERIALNUMBER":"","CERT_SERVER_ISSUER":"","CERT_SERVER_SUBJECT":"","CERT_SUBJECT":"","CONTENT_LENGTH":"0","CONTENT_TYPE":"","GATEWAY_INTERFACE":"","HTTPS":"","HTTPS_KEYSIZE":"","HTTPS_SECRETKEYSIZE":"","HTTPS_SERVER_ISSUER":"","HTTPS_SERVER_SUBJECT":"","INSTANCE_ID":"","INSTANCE_META_PATH":"","LOCAL_ADDR":"127.0.0.1","PATH_INFO":"/","PATH_TRANSLATED":"C:\\Users\\avanderhoorn\\Glimpse\\source\\Glimpse.Sample.MVC3","QUERY_STRING":"","REMOTE_ADDR":"127.0.0.1","REMOTE_HOST":"127.0.0.1","REMOTE_PORT":"","REQUEST_METHOD":"GET","SCRIPT_NAME":"/","SERVER_NAME":"localhost","SERVER_PORT":"33333","SERVER_PORT_SECURE":"0","SERVER_PROTOCOL":"HTTP/1.1","SERVER_SOFTWARE":"","URL":"/","HTTP_CONNECTION":"keep-alive","HTTP_ACCEPT":"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8","HTTP_ACCEPT_CHARSET":"ISO-8859-1,utf-8;q=0.7,*;q=0.3","HTTP_ACCEPT_ENCODING":"gzip,deflate,sdch","HTTP_ACCEPT_LANGUAGE":"en-US,en;q=0.8","HTTP_COOKIE":"glimpseClientName=null; glimpseState=On; ASP.NET_SessionId=1k33gr33dxqvzw3tm41koizh; prgLocation=/StoreManager/; prgLocationRedirect=/Account/LogOn?ReturnUrl=%2fStoreManager%2f; prgLocationId=25620064-8eb8-4879-842d-092a99b2b0ef; prgLocationMethod=GET; glimpseOptions=%7B%220%22%3A%22n%22%2C%221%22%3A%22u%22%2C%222%22%3A%22l%22%2C%223%22%3A%22l%22%2C%22open%22%3Atrue%2C%22height%22%3A447%2C%22activeTab%22%3A%22Views%22%2C%22popupOn%22%3Afalse%2C%22firstPopup%22%3Atrue%2C%22timeView%22%3Afalse%7D","HTTP_HOST":"localhost:33333","HTTP_USER_AGENT":"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.83 Safari/535.2"} },
                                "Views":{ name : 'Views', data : [["Ordinal","Requested View","Master Override","Partial","View Engine","Check Cache","Found","Details"],[1,"Index","","False","WebFormViewEngine","True","False",[["Not Found In"],["_WebFormViewEngine cache_"]]],[2,"Index","","False","RazorViewEngine","True","False",[["Not Found In"],["_RazorViewEngine cache_"]]],[3,"Index","","False","WebFormViewEngine","False","False",[["Not Found In"],["~/Views/Home/Index.aspx"],["~/Views/Home/Index.ascx"],["~/Views/Shared/Index.aspx"],["~/Views/Shared/Index.ascx"]]],[4,"Index","","False","RazorViewEngine","False","True",{"ViewData":null,"Model":{"ModelType":"System.Collections.Generic.List<MvcMusicStore.Models.Album>","Value":{"0":{"Artist":"Men At Work","Genre":"Rock","Price":"$8.99","Title":"The Best Of Men At Work"},"1":{"Artist":"AC/DC","Genre":"Rock","Price":"$8.99","Title":"For Those About To Rock We Salute You"},"2":{"Artist":"AC/DC","Genre":"Rock","Price":"$8.99","Title":"Let There Be Rock"},"3":{"Artist":"Accept","Genre":"Rock","Price":"$8.99","Title":"Balls to the Wall"},"4":{"Artist":"Accept","Genre":"Rock","Price":"$8.99","Title":"Restless and Wild"}}},"TempData":{"Test":"A bit of temp"}},"selected"],[5,"CartSummary","","True","WebFormViewEngine","True","False",[["Not Found In"],["_WebFormViewEngine cache_"]]],[6,"CartSummary","","True","RazorViewEngine","True","False",[["Not Found In"],["_RazorViewEngine cache_"]]],[7,"CartSummary","","True","WebFormViewEngine","False","False",[["Not Found In"],["~/Views/ShoppingCart/CartSummary.aspx"],["~/Views/ShoppingCart/CartSummary.ascx"],["~/Views/Shared/CartSummary.aspx"],["~/Views/Shared/CartSummary.ascx"]]],[8,"CartSummary","","True","RazorViewEngine","False","True",{"ViewData":{"CartCount":"0"},"Model":null,"TempData":{"Test":"A bit of temp"}},"selected"],[9,"GenreMenu","","True","WebFormViewEngine","True","False",[["Not Found In"],["_WebFormViewEngine cache_"]]],[10,"GenreMenu","","True","RazorViewEngine","True","False",[["Not Found In"],["_RazorViewEngine cache_"]]],[11,"GenreMenu","","True","WebFormViewEngine","False","False",[["Not Found In"],["~/Views/Store/GenreMenu.aspx"],["~/Views/Store/GenreMenu.ascx"],["~/Views/Shared/GenreMenu.aspx"],["~/Views/Shared/GenreMenu.ascx"]]],[12,"GenreMenu","","True","RazorViewEngine","False","True",{"ViewData":null,"Model":{"ModelType":"System.Collections.Generic.List<MvcMusicStore.Models.Genre>","Value":{"0":{"Name":"Rock","Id":"1","Description":null},"1":{"Name":"Classical","Id":"2","Description":null},"2":{"Name":"Jazz","Id":"3","Description":null},"3":{"Name":"Pop","Id":"4","Description":null},"4":{"Name":"Disco","Id":"5","Description":null},"5":{"Name":"Latin","Id":"6","Description":null},"6":{"Name":"Metal","Id":"7","Description":null},"7":{"Name":"Alternative","Id":"8","Description":null},"8":{"Name":"Reggae","Id":"9","Description":null},"9":{"Name":"Blues","Id":"10","Description":null}}},"TempData":{"Test":"A bit of temp"}},"selected"]] },
                                "Execution":{ name : 'Execution', data : {"ExecutedMethods":[["Ordinal","Child","Category","Type","Method","Time Elapsed","Order","Scope","Details"],[0,"False","Authorization","HomeController","OnAuthorization()","1 ms","int.MinValue","First",null],[1,"False","Action","HomeController","OnActionExecuting()","~ 0 ms","int.MinValue","First",null],[2,"False","","HomeController","Index()","1706 ms",null,"",null,"selected"],[3,"False","Action","HomeController","OnActionExecuted()","~ 0 ms","int.MinValue","First",null],[4,"False","Result","HomeController","OnResultExecuting()","~ 0 ms","int.MinValue","First",null],[5,"False","","ViewResult","ExecuteResult(ControllerContext context)","3039 ms",null,"",null,"selected"],[6,"True","Authorization","ShoppingCartController","OnAuthorization()","~ 0 ms","int.MinValue","First",null],[7,"True","Authorization","ChildActionOnlyAttribute","OnAuthorization()","~ 0 ms",-1,"Action",null],[8,"True","Action","ShoppingCartController","OnActionExecuting()","~ 0 ms","int.MinValue","First",null],[9,"True","","ShoppingCartController","CartSummary()","47 ms",null,"",null,"selected"],[10,"True","Action","ShoppingCartController","OnActionExecuted()","~ 0 ms","int.MinValue","First",null],[11,"True","Result","ShoppingCartController","OnResultExecuting()","~ 0 ms","int.MinValue","First",null],[12,"True","","PartialViewResult","ExecuteResult(ControllerContext context)","419 ms",null,"",null,"selected"],[13,"True","Result","ShoppingCartController","OnResultExecuted()","~ 0 ms","int.MinValue","First",null],[14,"True","Authorization","StoreController","OnAuthorization()","~ 0 ms","int.MinValue","First",null],[15,"True","Authorization","ChildActionOnlyAttribute","OnAuthorization()","~ 0 ms",-1,"Action",null],[16,"True","Action","StoreController","OnActionExecuting()","~ 0 ms","int.MinValue","First",null],[17,"True","","StoreController","GenreMenu()","416 ms",null,"",null,"selected"],[18,"True","Action","StoreController","OnActionExecuted()","~ 0 ms","int.MinValue","First",null],[19,"True","Result","StoreController","OnResultExecuting()","~ 0 ms","int.MinValue","First",null],[20,"True","","PartialViewResult","ExecuteResult(ControllerContext context)","436 ms",null,"",null,"selected"],[21,"True","Result","StoreController","OnResultExecuted()","~ 0 ms","int.MinValue","First",null],[22,"False","Result","HomeController","OnResultExecuted()","~ 0 ms","int.MinValue","First",null]],"UnExecutedMethods":[["Child","Category","Type","Method","Order","Scope","Details"],["False","Exception","HomeController","OnException()","int.MinValue","First",null,"quiet"],["False","Exception","HandleErrorAttribute","OnException()",-1,"Global",{"ExceptionType":"System.Exception","Master":"","View":"Error"},"quiet"],["True","Exception","ShoppingCartController","OnException()","int.MinValue","First",null,"quiet"],["True","Exception","HandleErrorAttribute","OnException()",-1,"Global",{"ExceptionType":"System.Exception","Master":"","View":"Error"},"quiet"],["True","Exception","StoreController","OnException()","int.MinValue","First",null,"quiet"],["True","Exception","HandleErrorAttribute","OnException()",-1,"Global",{"ExceptionType":"System.Exception","Master":"","View":"Error"},"quiet"]]} },
                                "MetaData":{ name : 'MetaData', data : [["Registration","Type","Details"],["Primary View Model","System.Collections.Generic.List<MvcMusicStore.Models.Album>",{"ConvertEmptyStringToNull":"True","DataTypeName":null,"Description":null,"DisplayFormatString":null,"DisplayName":null,"EditFormatString":null,"HideSurroundingHtml":"False","IsComplexType":true,"IsNullableValueType":false,"IsReadOnly":"False","IsRequired":"False","NullDisplayText":null,"ShortDisplayName":null,"ShowForDisplay":"True","ShowForEdit":"True","SimpleDisplayText":"System.Collections.Generic.List<MvcMusicStore.Models.Album>","TemplateHint":null,"Watermark":null},""],["View Model Properties",null,{"Capacity":{"ConvertEmptyStringToNull":"True","DataTypeName":null,"Description":null,"DisplayFormatString":null,"DisplayName":null,"EditFormatString":null,"HideSurroundingHtml":"False","IsComplexType":false,"IsNullableValueType":false,"IsReadOnly":"False","IsRequired":"*True*","NullDisplayText":null,"ShortDisplayName":null,"ShowForDisplay":"True","ShowForEdit":"True","SimpleDisplayText":"8","TemplateHint":null,"Watermark":null},"Count":{"ConvertEmptyStringToNull":"True","DataTypeName":null,"Description":null,"DisplayFormatString":null,"DisplayName":null,"EditFormatString":null,"HideSurroundingHtml":"False","IsComplexType":false,"IsNullableValueType":false,"IsReadOnly":"*True*","IsRequired":"*True*","NullDisplayText":null,"ShortDisplayName":null,"ShowForDisplay":"True","ShowForEdit":"True","SimpleDisplayText":"5","TemplateHint":null,"Watermark":null}},"glimpse-start-open"]] }
                        }
                    }
                },
        
                requests1 = [
                    { type : 'Session', method : 'Get', duration : 213, browser : 'Chrome 16.0', clientName : '', requestTime : '2011/11/09 12:00:12', requestId : 'ajax0', parentId : '1234', isAjax : true, url : '/Product'},
                    { type : 'Server', method : 'Get', duration : 123, browser : 'Chrome 16.0', clientName : '', requestTime : '2011/11/09 12:10:34', requestId : 'ajax1', parentId : '1234', isAjax : true, url : '/Product/Trip'},
                    { type : 'Request', method : 'Get', duration : 234, browser : 'Chrome 16.0', clientName : '', requestTime : '2011/11/09 12:12:23', requestId : 'ajax2', parentId : '1234', isAjax : true, url : '/Product/230'},
                    { type : 'Trace', method : 'Post', duration : 342, browser : 'Chrome 16.0', clientName : '', requestTime : '2011/11/09 12:17:52', requestId : 'ajax3', parentId : '1234', isAjax : true, url : '/Product/Add'},
                    { type : 'Environment', method : 'Post', duration : 211, browser : 'Chrome 16.0', clientName : '', requestTime : '2011/11/24 12:00:35', requestId : 'ajax4', parentId : '1234', isAjax : true, url : '/Product/Results'},
                    { type : 'SQL', method : 'Post', duration : 242, browser : 'Chrome 16.0', clientName : '', requestTime : '2011/11/09 12:27:23', requestId : 'ajax5', parentId : '1234', isAjax : true, url : '/Product/List'},
                    { type : 'Routes', method : 'Get', duration : 1234, browser : 'Chrome 16.0', clientName : '', requestTime : '2011/11/09 12:29:14', requestId : 'ajax6', parentId : '1234', isAjax : true, url : '/Product'}
                ],
                requests2 = [ 
                    { type : 'Session', method : 'Post', duration : 213, browser : 'iPhone 1', clientName : 'iPhone', requestTime : '2011/11/09 12:00:12', requestId : 'iPhone1Ajax', parentId : 'iPhone1', isAjax : true, url : '/Product'},
                    { type : 'Server', method : 'Post', duration : 123, browser : 'iPhone 1', clientName : 'iPhone', requestTime : '2011/11/09 12:10:34', requestId : 'iPhone1', isAjax : false, url : '/Product'},
                    { type : 'Request', method : 'Post', duration : 234, browser : 'iPhone 1', clientName : 'iPhone', requestTime : '2011/11/09 12:12:23', requestId : 'iPhone2', isAjax : false, url : '/Product/230'},
                    { type : 'Trace', method : 'Post', duration : 342, browser : 'iPhone 1', clientName : 'iPhone', requestTime : '2011/11/09 12:17:52', requestId : 'iPhone3', isAjax : false, url : '/Product/Add'}
                ],
                requests3 = [ 
                    { type : 'Environment', method : 'Get', duration : 211, browser : 'IE6', clientName : 'Remote', requestTime : '2011/11/24 12:00:35', requestId : '2ajax4', isAjax : false, url : '/Product/Results'},
                    { type : 'SQL', method : 'Get', duration : 242, browser : 'IE6', clientName : 'Remote', requestTime : '2011/11/09 12:27:23', requestId : '2ajax5', isAjax : false, url : '/Product/List'},
                    { type : 'Routes', method : 'Get', duration : 1234, browser : 'IE6', clientName : 'Remote', requestTime : '2011/11/09 12:29:14', requestId : '2ajax6', isAjax : false, url : '/Product'}
                ],
                requests = {
                    '' : requests1,
                    'iPhone' : requests2,
                    'Remote' : requests3 
                },  
                
                requestTracker = [ 
                    { name : '', count : 0, max : 7 },
                    { name : 'iPhone', count : 0, max : 4 },
                    { name : 'Remote', count : 0, max : 3 } 
                ],  
                requestTrackerResults = {},
                
                ajaxRequestTracker = {
                    index: 0,
                    lastId: 0
                },
                ajaxRequestTrackerResults = [],
        
                
                generateAjaxResults = function (requestId) { 
                    if (requestId != ajaxRequestTracker.lastId) {
                        ajaxRequestTracker.index = 0;
                        ajaxRequestTracker.lastId = requestId;
                        ajaxRequestTrackerResults = [];
                    }
        
                    if (random(2) == 1 && (ajaxRequestTracker.index < 6 && requestId == 1234) || ajaxRequestTracker.index < 3)
                        ajaxRequestTrackerResults.push(requests1[ajaxRequestTracker.index++]);
                    
                    return ajaxRequestTrackerResults;
                },
        
                findRequest = function (requestId) {
                    var response;
                    for (var name in requests) {
                        var nameValue = requests[name];
                        for (var i = 0; i < nameValue.length ; i++) {
                            if (nameValue[i].requestId == requestId)
                                response = nameValue[i];
                        }
                    }
                     
                    if (response) {
                        response = $.extend(true, {}, response);
                        
                        response.data = {};
                        response.data[response.type] = data[response.type];
                        response.metadata = metadata; 
                    } 
        
                    return response;
                },
        
                radomResponse = function() { 
                    if (random(2) == 1) {
                        var mainIndex = random(5);
                        if (mainIndex < 3) {
                            var tackedItem = requestTracker[mainIndex];
                            if (tackedItem.count < tackedItem.max) { 
                                if (tackedItem.count == 0) 
                                    requestTrackerResults[tackedItem.name] = []; 
                                requestTrackerResults[tackedItem.name].push(requests[tackedItem.name][tackedItem.count]);
                                tackedItem.count++;
                            }
                        }
                    }
                    return requestTrackerResults;
                },
                
                trigger = function (param, data) { 
                    setTimeout(function () { 
                        
                        var response, 
                            success = 'success';
                        
                        if (param.url.indexOf("History") == 0) {
                            // History
                            success = random(11) != 10 ? 'success' : 'Fail';
                            if (success == 'success')
                                response = radomResponse();
                        }
                        else if (data) { 
                            if (data.requestId) {
                                // Request
                                if (requestData[data.requestId])
                                    response = requestData[data.requestId];   // Find Exact Request
                                else 
                                    response = findRequest(data.requestId);   // Generate Partical Request
                                
                                // Tab Request
                                if (data.pluginKey) 
                                    response = data.pluginKey != "Lazy" ? response.data[data.pluginKey] : lazyData;
                            }            
                            else if (data.parentRequestId) { 
                                // Ajax Requests 
                                success = random(11) != 10 ? 'success' : 'Fail';
                                if (success == 'success') 
                                    response = generateAjaxResults(data.parentRequestId);  
                            }
                        } 
                        
                        if (response)
                            response = $.extend(true, $.isArray(response) ? [] : {}, response);
        
                        if (param.complete)
                            param.complete(null, success);
                        if (param.success && success == 'success')
                            param.success(response);
                        
                    }, random(6) * 100 + 1);
                };
        
            return {
                trigger : trigger
            };
        } (),
        
        //Main
        queryStringToObject = function (uri) {
            var queryString = {},
                matched = 0;
            uri.replace(
                new RegExp("([^?=&]+)(=([^&]*))?", "g"),
                function($0, $1, $2, $3) {
                    if (matched > 0)
                        queryString[$1] = $3;
                    matched++;
                }
            );
            return matched > 1 ? queryString : null;
        },

        random = function (length) {
            return Math.floor(Math.random() * length);
        },
        retrieve = function (url) {
            var parts = /(\S+)\?/ig.exec(url); 
            if (parts.length == 2 && testHandlers[parts[1]]) 
                return testHandlers[parts[1]];
            return null;
        },
        register = function (name, callback) { 
            testHandlers[name] = callback; 
        },
        init = function () { 
            register("Pager", pager.trigger);
            register("Ajax", data.trigger); 
            register("History", data.trigger); 
            register("Request", data.trigger); 
            register("Tab", data.trigger); 

            //http://stackoverflow.com/questions/5272698/how-to-fake-jquery-ajax-response
            var original = $.ajax;
            $.ajax = function (param) { 
                var callback = retrieve(param.url);
                if (callback) 
                    callback(param, queryStringToObject(param.url));
                else 
                    original(param); 
            };
        };

    init();
}($Glimpse));