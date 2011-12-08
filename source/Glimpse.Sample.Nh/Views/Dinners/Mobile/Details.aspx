<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mobile/Site.Master" Inherits="System.Web.Mvc.ViewPage<NerdDinner.Models.Dinner>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server"><%= Html.Encode(Model.Title) %></asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h1><a href="/" runat="server"><img alt="nerddinner" src="Content/Img/Mobile/logo_medium.jpg"/></a></h1>
    <h2><%= Html.Encode(Model.Title) %></h2>
    <ul id="Details">
        <li><strong>When:</strong> <%= Model.EventDate.ToShortDateString() %> @ <%= Model.EventDate.ToShortTimeString() %></li>
        <li><strong>Where:</strong> <a target="_self" href="<%=String.Format("http://m.live.com/search/LocationPage.aspx?l={0}&amp;q=map&amp;a=changelocation",Url.Encode(Model.Address + " " + Model.Country)) %>"><%= Html.Encode(Model.Address) %>, <%= Html.Encode(Model.Country) %></a></li>
        <li><strong>Description:</strong> <%= Html.Encode(Model.Description) %></li>
        <li><strong>Organizer:</strong> <%= Html.Encode(Model.HostedBy) %> (<%= Html.Encode(Model.ContactPhone) %>)</li>
    </ul>

    <div id="whoscoming" title="<%= Html.Encode(Model.Title) %>" class="panel">
        <h2>Who's Coming?</h2>
                <%if (Model.RSVPs.Count == 0){%>
                      <ul><li>No one has registered.</li></ul>
                <% } %>

            <%if(Model.RSVPs.Count > 0) {%>
            <ul id="attendees">
                <%foreach (var RSVP in Model.RSVPs){%>
                  <li ><%=Html.Encode(RSVP.AttendeeName.Replace("@"," at ")) %></li>    
                <% } %>
            </ul>
            <%} %>
    </div>
</asp:Content>