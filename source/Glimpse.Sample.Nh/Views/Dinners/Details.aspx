<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<NerdDinner.Models.Dinner>" MasterPageFile="~/Views/Shared/Site.Master"  %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Html.Encode(Model.Title) %>
</asp:Content>

<asp:Content ID="details" ContentPlaceHolderID="MainContent" runat="server">

    <div id="dinnerDiv" class="vevent">

        <h2 class="summary"><%= Html.Encode(Model.Title) %></h2>
        
        <p>
            <a href="http://feeds.technorati.com/events/<%= Url.AbsoluteAction("Details", new { id = Model.DinnerID }) %>">
                Add event to your calendar (iCal)
            </a>
        </p>
        
        <p>
            <strong>When:</strong> 
            <abbr class="dtstart" title="<%= Model.EventDate.ToString("s") %>">
                <%= Model.EventDate.ToString("MMM dd, yyyy") %> 
                <strong>@</strong>
                <%= Model.EventDate.ToShortTimeString() %>
            </abbr>
        </p>
        
        <p>
            <strong>Where:</strong>
            <span class="location adr">
                <span class="entended-address"><%= Html.Encode(Model.Address) %></span>, 
                <span class="country-name"><%= Html.Encode(Model.Country) %></span>
                <abbr class="geo" title="<%= Model.Latitude %>;<%= Model.Longitude %>" style="display: none;">Geolocation for hCalendar</abbr>
            </span>
        </p>
        
        <p>
            <strong>Description:</strong> 
            <span class="description"><%= Html.Encode(Model.Description) %></span>
            <span style="display: none;">
                <%= Html.ActionLink("URL for hCalendar", "Details", new { id = Model.DinnerID }, new { @class = "url" })%>
            </span>
        </p>
            
        <p>
            <strong>Organizer:</strong>
            <span class="organizer">
                <span class="vcard">
                    <span class="fn nickname"><%= Html.Encode(Model.HostedBy) %></span>
                    <span class="tel"> <%= Html.Encode(Model.ContactPhone) %></span>
                </span>                
            </span>
        </p>
        
        <% Html.RenderPartial("RSVPStatus"); %>
        
        <p id="whoscoming">
            <strong>Who's Coming:</strong>
            <%if (Model.RSVPs.Count == 0){%>
                  No one has registered.
            <% } %>
        </p>
        
        <%if(Model.RSVPs.Count > 0) {%>
					<div id="whoscomingDiv">
            <ul class="attendees">
                <%foreach (var RSVP in Model.RSVPs){%>
                  <li class="attendee">
                    <span class="vcard">
                        <span class="fn nickname"><%=Html.Encode(RSVP.AttendeeName.Replace("@"," at ")) %></span>
                    </span>
                  </li>
                <% } %>
            </ul>
          </div>
        <%} %>
        
        <% Html.RenderPartial("EditAndDeleteLinks"); %>    
        
    </div>
    
    <div id="mapDiv">
        <% Html.RenderPartial("map"); %>    
        <p>
					<img src="/content/img/microformat_hcalendar.png" alt="hCalendar"/>
        </p>
    </div>   
         
</asp:Content> 

