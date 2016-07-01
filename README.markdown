#NOTE: Work on v2 of Glimpse has shifted to [Glimpse/Glimpse.Prototype](https://github.com/Glimpse/Glimpse.Prototype)

##The Diagnostics platform of the web [![Build Status](http://img.shields.io/teamcity/codebetter/bt428.svg)](http://teamcity.codebetter.com/viewType.html?buildTypeId=bt428&guest=1)

Providing real time diagnostics & insights to the fingertips of hundreds of thousands of developers daily

##How does Glimpse help?
Once installed, Glimpse inspects web requests as they happen, providing insights and tooling that reduce debugging time and empower every developer to improve their web applications.

 - **Visual Profiling** - Glimpse profiles key server side activities and displays the timing of each in an easy to understand Gantt chart.
 - **Transparent Data Access** - Out of process database calls are expensive. Glimpse lists each of them, so excessive or under-performant queries can be reigned in.
 - **View Rendering & Resolution** - Glimpse provides complete visibility into ASP.NET MVC's view resolution process, including file access paths.
 - **Route Debugging** - ASP.NET routing is a powerful, and sometimes maddening, feature. Glimpse cracks into the black box and exposes how routes are matched.
 - **Server Configuration** - Know everything necessary about a request's origin server including: timezone, patch version, process ID and pertinent web.config entries.
 - **Works For All Requests** - Glimpse provides insights for not only the originating request, but also for AJAX requests, historical requests from the past and even requests made from other users.
 - **ASP.NET WebForms Too** - Glimpse ❤'s ASP.NET WebForms with the best View State decoder available embedded directly into the server control tree.
 - **Improved Tracing** - Glimpse automatically displays trace statements, eliminating the headache of digging through log files. Popular logging frameworks can be integrated with some slight configuration as well!


##Where Does it Fit
Glimpse works where you need it to, how you want it to. Insights are presented in digestable summaries, with the ability to drill down to tackle tough problems.

 - **Install via NuGet** - Glimpse is installed with one simple NuGet command (Install-Package Glimpse) or with Visual Studio's manager package dialog.
 - **Extensible & Configurable** - Extend Glimpse via simple APIs and our 3rd party NuGet package ecosystem. Configure it on a whim with the ~/Glimpse.axd configuration builder and web.config.
 - **Web Native** - Glimpse is built with web technologies that you love: HTML, CSS & JS. It requires no proprietary browser plugin and works everywhere you do.
 - **Hardened Security** - Hardened by default, only you get to choose who can see what Glimpse data and when. Check out GlimpseSecurityPolicy.cs, already in your project, for more information and samples.
 - **Lightweight & Fast** - Glimpse's unique architecture makes it faster and less resource intensive than traditional profilers so you never have to sacrifice performance.
 - **Unique Perspective** - While F12 tools like Firebug and proxy debuggers like Fiddler are extremely useful, only Glimpse provides diagnostics from the perspective of your server.

##Free & Open Source
Thanks to community contributions, Glimpse is thriving and growing. Glimpse is free and available under the Apache 2.0 license. The source code is available, so feel free to jump in and contribute!

###Getting involved
Glimpse wouldn't be what it is today without the love and support of some awesome people from around the world. These contributions have ranged from simple bug fixes to fully fledged features, and from as far afield as South Africa and South East Asia.

If you would like to get involved, there are plenty of things that you can do. There are issues that are good for first-timers ringfenced and tagged Jump In in GitHub. If you'd rather start off on something more self-contained, why not write an extension?

 - **Issues** - Glimpse maintains several issues that are good for first-timers [tagged as Jump In on GitHub](https://github.com/Glimpse/Glimpse/issues?labels=Jump+In&milestone=&page=1&sort=updated&state=open). If one piques your interest, feel free to work on it and let us know if you need any help doing so.
    - [Learn more about how "Jump In" issues work](http://nikcodes.com/2013/05/10/new-contributor-jump-in/)
 - **New Features** - For those looking to get more deeply involved, reach out to find out about our current efforts and how you can help.
    - [Learn more about contributing to Glimpse core](http://getglimpse.com/Docs/Contributing)
 - **Share Glimpse** - If you love Glimpse, tell others about it! Present Glimpse at a company tech talk, your local user group or submit a proposal to a conference about how you are using Glimpse or any extensions you may have written.
    - [Need inspiration? Watch some of the talks that we have given](http://getglimpse.com/Docs/More-information)
 - **Create an Extension** - Get the best out of Glimpse by writing your own extension to expose diagnostic data that is meaningful for your applications. Creating extensions is easy, check [the docs](http://getglimpse.com/Docs/Custom-Tabs) or reference an [open source extension](http://getglimpse.com//Extensions) to get started.
 - **Documentation** - Documentation is a key differentiator between good projects and <em>great</em> ones. Whether you’re a first time OSS contributor or a veteran, documentation is a great stepping stone to learn our contribution process.
    - Contributing to Glimpse documentation is dead simple. To make it so easy, we're using Glimpse’s [GitHub Wiki](https://github.com/Glimpse/Glimpse/wiki) as the entry point for documentation - each page within the docs section of the site has a link to take you straight to the page where you can make changes directly. GitHub Wikis provide an online WYSIWYG interface for adding and editing the docs, completely in browser, using [Markdown](https://daringfireball.net/projects/markdown/).

For more on getting involved see our [contributor guidelines](https://github.com/Glimpse/Glimpse/blob/master/contributing.md).

##Need Help?

 - **Issue List** - Have you found a bug or something misbehaving? Tell us about it on our [GitHub issue list](https://github.com/glimpse/glimpse/issues) and someone will help you as soon as possible.
 - **Stack Overflow** - Got general questions or just need a little support? Anything related to using, configuring or extending Glimpse can be asked at [StackOverflow](http://stackoverflow.com). View [already answered](http://stackoverflow.com/questions/tagged/glimpse) questions for even faster support.
 - **Mailing List** - Got questions about how to extend or work on Glimpse, or just want to discuss a new feature idea? Shoot it through to the [developers mailing list](https://groups.google.com/forum/#!forum/getglimpse-dev).

More information about Glimpse can be found at [getGlimpse.com](http://getGlimpse.com)

---

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
