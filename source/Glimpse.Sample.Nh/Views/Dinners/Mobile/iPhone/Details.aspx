<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<NerdDinner.Models.Dinner>" %>

<ul id="Details">
    <li>Title: <%= Html.Encode(Model.Title) %></li>
        <li>When: <%= Model.EventDate.ToShortDateString() %> @ <%= Model.EventDate.ToShortTimeString() %></li>
    <li><a target="_self" href="<%=String.Format("http://maps.google.com/maps?q={0},{1}",Url.Encode(Model.Address),Url.Encode(Model.Country)) %>">Where: <%= Html.Encode(Model.Address) %>, <%= Html.Encode(Model.Country) %></a></li>
    <li>Description: <%= Html.Encode(Model.Description) %></li>
    <li><a target="_self" href="tel://<%= Html.Encode(Model.ContactPhone) %>">Organizer: <%= Html.Encode(Model.HostedBy) %> (<%= Html.Encode(Model.ContactPhone) %>)</a></li>
    <li><a href="#whoscoming">Who's Coming?</a></li>
</ul>

<div id="whoscoming" title="<%= Html.Encode(Model.Title) %>" class="panel">
    <h2>Who's Coming?</h2>
            <%if (Model.RSVPs.Count == 0){%>
                  <ul><li>No one has registered.</li></ul>
            <% } %>

        <%if(Model.RSVPs.Count > 0) {%>
        <ul id="attendees">
            <%foreach (var RSVP in Model.RSVPs){%>
              <li><%=Html.Encode(RSVP.AttendeeName.Replace("@"," at ")) %></li>    
            <% } %>
        </ul>
        <%} %>
</div>