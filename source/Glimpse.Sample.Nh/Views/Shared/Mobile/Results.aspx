<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Mobile/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NerdDinner.Models.Dinner>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Nerd Dinners</h2>
		<ul class="dinners">
    <% foreach (var dinner in Model) { %>
        <li>     
            <ul>
            <li>
	            <a href="<%= Url.RouteUrl("PrettyDetails", new { Id = dinner.DinnerID }) %>"><%=Html.Encode(dinner.Title) %></a>
            </li>
            <li>
		          <%= Html.Encode(dinner.EventDate.ToString("yyyy-MMM-dd"))%> 
							@
							<%= Html.Encode(dinner.EventDate.ToString("h:mm tt"))%>
            </li>
						</ul>

        </li>
        <% } %>
      <% if (Model.Count() == 0) { %>
       <li>No Nerd Dinners found!</li>
      <% } %>

		</ul>
    <p>
        <%= Html.ActionLink("Back", "Index", "Home") %>
    </p>

</asp:Content>

