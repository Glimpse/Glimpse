<%@ Page Inherits="System.Web.Mvc.ViewPage<NerdDinner.Helpers.PaginatedList<NerdDinner.Models.Dinner>>" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
	Upcoming Nerd Dinners
</asp:Content>

<asp:Content ID="Masthead" ContentPlaceHolderID="MastheadContent" runat="server">
	<% Html.RenderPartial("Masthead", false); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>
        Upcoming Dinners
    </h2>

    <ul class="upcomingdinners">
    
        <% foreach (var dinner in Model) { %>
        
            <li>     
                <%= Html.ActionLink(dinner.Title, "Details", new { id=dinner.DinnerID }) %>
                on 
                <%= Html.Encode(dinner.EventDate.ToString("yyyy-MMM-dd"))%> 
                @
                <%= Html.Encode(dinner.EventDate.ToString("HH:mm tt"))%>
            </li>
        
        <% } %>

    </ul>

    <div class="pagination">

        <% if (Model.HasPreviousPage) { %>
        
            <%= Html.RouteLink("<<< Previous Page", 
                               "UpcomingDinners", 
                               new { page=(Model.PageIndex-1) }) %>
        
        <% } %>
        
        <% if (Model.HasNextPage) { %>
        
            <%= Html.RouteLink("Next Page >>>", 
                               "UpcomingDinners", 
                               new { page = (Model.PageIndex + 1) })%>
        
        <% } %>    

    </div>
    
</asp:Content>


