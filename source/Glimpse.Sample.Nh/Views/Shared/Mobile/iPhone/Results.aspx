<%@ Page Inherits="System.Web.Mvc.ViewPage<NerdDinner.Helpers.PaginatedList<NerdDinner.Models.Dinner>>" Language="C#"  %>
<ul title="Results">
      <% foreach (var dinner in Model) { %>
					<li><a href="<%=Url.RouteUrl("PrettyDetails", new { Id = dinner.DinnerID } ) %>">
						<%= dinner.EventDate.ToString("MMM dd")%> <%= HttpUtility.HtmlEncode(dinner.Description) %>
					</a></li> 
      <% } %>    
      <% if (Model.Count == 0) { %>
       <li>No Nerd Dinners found!</li>
      <% } %>
</ul>
