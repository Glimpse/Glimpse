[![Build Status](http://teamcity.codebetter.com/app/rest/builds/buildType:%28id:bt428%29/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=bt428&guest=1)

A client side Glimpse into whats going on in your server 

Overview
--------
At its core Glimpse allows you to debug your Web Service right in the browser. Glimpse allows you to "Glimpse" into what's going on in your web server. In other words what Firebug is to debugging your client side code, Glimpse is to debugging your server within the client.

Fundamentally Glimpse is made up of 3 different parts, all of which are extensible and customizable for any platform:

* Glimpse Server Module 
* Glimpse Client Side Viewer 
* Glimpse Protocol


How it Works
------------
On the Server:

1. Server collects all server side information that will aid in debugging (i.e. application settings, routes, session variables, trace data, etc)
2. It does this by running through a pipeline of server side data providers that can be dynamically controlled and added to under plugin architecture
3. Before the response is send, the server formats this data in accordance with the Glimpse Protocol and serializes it as JSON
4. Depending on whether it is a Ajax request or not, the server embeds the JSON in the HTTP Header or in the content of the page

On the Client:

5. Depending on whether it is a Ajax request or not, it picks up the JSON data and runs through the data set by executing a pipeline of client side data providers that can be dynamically controlled and added to under plugin architecture
6. The client side module then dynamically renders a client side UI (similar to Firebug Lite) that lets you view this data

Glimpse can be turned on or off by a series of different mechanistic, but at its core if the Glimpse cookie is present the server will provide the "debug" data - as a security measure, the request for debug data is "authenticated". Via the plugin model, this authentication check can have any logic that is required by the site to ensure that unauthorized users don't have access to sensitive debug data.

 
Server Implementations 
----------------------
Given the scope of the project and what it can do, the concept isn't restricted to any one platform. Hence, once mature, Glimpse Server Module will be available on all major web platforms. 

Platforms currently supported:

* ASP.Net Web Forms 
* ASP.Net MVC 

Platforms soon to be supported:

* PHP
* Ruby on Rails 

NOTE - If you would like help develop a Glimpse Server Module for a given platform please let us know.


Client Implementations 
----------------------
To start with the Glimpse Client Side Viewer is simply a light weight JavaScript "plugin" that understands the Glimpse Protocol and knows how to render the data. From a technology standpoint we currently use jQuery as the client side framework.


![Glimpse Client](https://raw.github.com/Glimpse/Glimpse/master/Doco/Glimpse.png "Glimpse Client")

Protocol
-------- 
Details coming soon.
