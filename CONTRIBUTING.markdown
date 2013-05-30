# How to Contribute
This is a general guide about how to contribute to Glimpse. It is not a set of hard and fast rules. Any questions, concerns or suggestions should be raised on the [Glimpse Developers List](https://groups.google.com/forum/?fromgroups#!forum/getglimpse-dev).

## Submit Bugs
Bugs should be reported in the [GitHub Issue](https://github.com/glimpse/glimpse/issues) tracker if they have not been previously submitted. 

When reporting a bug or issue, please include all pertinent information. This typically includes:

* Glimpse package(s) installed _(Example: Glimpse.Mvc3.0.87  and FluentSecurity.Glimpse.1.1)_
* Development platform, including .NET version and web server _(Example: Mvc3 with .NET version 4.0 on IIS Express)_
* If the problem is UI related include the browser and its version _(Example: IE9)_
* Steps to reproduce the bug/example code

It is also quite helpful to include the relevant portions of Glimpse's log file. You can enable Glimpse logging by adding the `<logging>` tag in web.config. Additional information can be found in [the configuration page](https://github.com/Glimpse/Glimpse/wiki/Configuration#logging).

````
	<glimpse enabled="true">
		<logging level="Trace" />
		<!-- ... -->
	</glimpse>
````

Bugs will be addressed as soon as humanly possible, but please allow ample time. For quicker responses, you may also choose to implement and contribute the bug fix. 

## Update Documentation

The [Glimpse website](http://getglimpse.com/) itself is also open source. As such, any of the documentation contained within it can be improved via contributions from the community. Feel free to submit a pull request to improve/add documentation.

## Contribute Code

Any medium or large contribution should begin by sending a message to the [Glimpse Developers List](https://groups.google.com/forum/?fromgroups#!forum/getglimpse-dev).

The message should describe the contribution you are interested in making, and any initial thoughts on implementation. This will allow the community to discuss and become involved with you from the get go. If you receive positive feedback on the mailing list, go ahead and start implementation! You should also sign and return the Contributor License agreement, which is required for the Glimpse team to accept your contribution.

The Glimpse team maintains a [backlog of feature ideas on Trello](https://trello.com/board/glimpse/4fb1bbcc8166822f2218b6c8) which is good to look through if you want to get involved but aren't quite sure how.

### Code Conventions

Glimpse follows a loose set of coding conventions. Chiefly among them:

* Ensure all unit tests pass successfully
* Cover additional code with passing unit tests
* Try not to add any additional StyleCop warnings to the compilation process
* Ensure your Git autocrlf setting is true to avoid "whole file" diffs.

# Additional Resources

* [General GitHub documentation](http://help.github.com/)
* [GitHub pull request documentation](http://help.github.com/send-pull-requests/)
* [Jabbr Chatroom](http://jabbr.net/#/rooms/Glimpse)
* [Nightly NuGet Feed](http://www.myget.org/F/glimpsenightly/)
* [Milestone NuGet Feed](http://www.myget.org/F/glimpsemilestone/)
* [Production NuGet Feed](https://nuget.org/api/v2/)
