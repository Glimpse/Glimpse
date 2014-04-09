# How to Contribute
Open source projects thrive on contributions from the developer community. Would like to get involved? There is plenty that you can do to help!

This is a general guide about how to contribute to Glimpse. It is not a set of hard and fast rules. Any questions, concerns or suggestions should be raised on the [Glimpse Developers List](https://groups.google.com/forum/?fromgroups#!forum/getglimpse-dev).

## Submitting Issues

Bugs should be reported in the [GitHub Issue](https://github.com/glimpse/glimpse/issues) tracker if they have not been previously submitted.

When reporting a bug or issue, please include all pertinent information. This typically includes:

* Glimpse package(s) installed _(Example: Glimpse.Mvc3.1.8.4  and FluentSecurity.Glimpse.1.1)_
* Development platform, including .NET version and web server _(Example: Mvc3 with .NET version 4.0 on IIS Express)_
* If the problem is UI related include the browser and its version _(Example: IE9)_
* Steps to reproduce the bug/example code
* Any error messages and stack trace

It is also quite helpful to include the relevant portions of Glimpse’s log file. You can enable Glimpse logging by adding the `loggingEnabled` attribute in web.config.
 
    <glimpse defaultRuntimePolicy="On">
        <logging level="Trace" />
    </glimpse> 

Bugs will be addressed as soon as humanly possible, but please allow ample time. For quicker responses, you may also choose to implement and contribute the bug fix.

## Fixing Issues

Glimpse maintains several issues that are good for first-timers [tagged as Jump In on GitHub](https://github.com/Glimpse/Glimpse/issues?labels=Jump+In&milestone=&page=1&sort=updated&state=open). If one peaks your interest, feel free to work on it and let us know if you need any help doing so.

 - [Learn more about how "Jump In" issues work](http://nikcodes.com/2013/05/10/new-contributor-jump-in/)

## New Features

For those looking to get more deeply involved, [reach out](/Community) to find out about our current efforts and how you can help. Medium or large contribution should begin by sending a message to the [Glimpse Developers List](https://groups.google.com/forum/?fromgroups#!forum/getglimpse-dev) or should start as a basic pull request so that a discussion can be started.

The message should describe the contribution you are interested in making, and any initial thoughts on implementation. This will allow the community to discuss and become involved with you from the get go. If you receive positive feedback on the mailing list, go ahead and start implementation! You should also sign and return the Contributor License agreement, which is required for the Glimpse team to accept your contribution.

The Glimpse team uses [the issue tracking features of GitHub](https://github.com/Glimpse/Glimpse/issues) which is a good place to look through if you want to get involved but aren't quite sure how.

## Share Glimpse

If you love Glimpse, tell others about it! Present Glimpse at a company tech talk, your local user group or submit a proposal to a conference about how you are using Glimpse or any extensions you may have written.</p>

 - [Need inspiration? Watch some of the talks that we have given](More-information)

## Create an Extension

Get the best out of Glimpse by writing your own extension to expose diagnostic data that is meaningful for your applications. Creating extensions is easy, check [the docs](Custom-Tabs) or reference an [open source extension](/Extensions) to get started.

 - [Creating your own custom extensions and tabs](Custom-Tabs)
 - [Creating your own custom runtime policies](Custom-Runtime-Policy)

## Documentation

Documentation is a key differentiator between good projects and _great_ ones. Whether you’re a first time OSS contributor or a veteran, documentation is a great stepping stone to learn our contribution process.

Contributing to Glimpse documentation is dead simple. To make it so easy, we're using Glimpse’s [GitHub Wiki](https://github.com/Glimpse/Glimpse/wiki) as the entry point for documentation - each page within the docs section of the site has a link to take you straight to the page where you can make changes directly. GitHub Wikis provide an online WYSIWYG interface for adding and editing the docs, completely in browser, using [Markdown](https://daringfireball.net/projects/markdown/).

## Code Conventions

Glimpse follows a loose set of coding conventions. Chiefly among them:

* Ensure all unit tests pass successfully
* Cover additional code with passing unit tests
* Try not to add any additional StyleCop warnings to the compilation process
* Ensure your [Git autocrlf setting](https://help.github.com/articles/dealing-with-line-endings) is true to avoid "whole file" diffs.

## Additional Resources

* [General GitHub documentation](http://help.github.com/)
* [GitHub pull request documentation](http://help.github.com/send-pull-requests/)
* [Nightly NuGet Feed](http://www.myget.org/F/glimpsenightly/)
* [Milestone NuGet Feed](http://www.myget.org/F/glimpsemilestone/)
* [Production NuGet Feed](https://nuget.org/api/v2/)
