<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NerdDinner.Models.Dinner>" %>

<script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>    

<script type="text/javascript">

    function AnimateRSVPMessage() {
        $("#rsvpmsg").animate({ fontSize: "1.5em" }, 400);
    }

</script>
    
<div id="rsvpmsg">

<% if (Request.IsAuthenticated) { %>

    <% if (Model.IsUserRegistered(Context.User.Identity.Name)) { %>        
    
        <p>You are registered for this event!</p>
    
    <% } else { %>  
    
        <%= Ajax.ActionLink( "RSVP for this event",
                             "Register", "RSVP",
                             new { id=Model.DinnerID }, 
                             new AjaxOptions { UpdateTargetId="rsvpmsg", OnSuccess="AnimateRSVPMessage" }) %>         
    <% } %>
    
<% } else { %>

    <strong><%= Html.ActionLink("Log On", "LogOn", new { controller = "Account", returnUrl = HttpContext.Current.Request.RawUrl }) %> to RSVP for this event.</strong>

<% } %>
    
</div>    