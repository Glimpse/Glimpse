Thank your get NuGetting Glimpse!

Glimpse is currently in Beta.  

If you experience any issues, or have feature requests, please report them to https://github.com/Glimpse/Glimpse/issues

To get started with Glimpse, visit [http://yoursite.com]/Glimpse/Config/ This page hosts the Glimpse bookmarklets, useful for quickly turning on Glimpse.

The following configuration values are allowed for Glimpse in your web.config:

<glimpse on="true" saveRequestCount="5"> <!-- set on to false to completly turn off Glimpse. saveRequestCount specifies the max number of requests Glimpse will save -->
    <ipAddresses> <!-- List of IP addresses allowed to get Glimpse data.  localhost (IPv4 & IPv6) by default -->
        <add address="127.0.0.1" />
        <add address="::1" />
    </ipAddresses>
    <contentTypes>
        <add contentType="text/html"/>
    </contentTypes>
</glimpse>


For more info, visit the homepage at https://github.com/Glimpse/