<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NerdDinner.Models.DinnerFormViewModel>" %>

<%= Html.ValidationSummary("Please correct the errors and try again.") %>

<% using (Html.BeginForm()) { %>

    <fieldset>

        <div id="dinnerDiv">

        <p>
            <label for="Title">Dinner Title:</label>
            <%= Html.TextBox("Title", Model.Dinner.Title) %>
            <%= Html.ValidationMessage("Title", "*") %>
        </p>
        <p>
            <label for="EventDate">Event Date:</label>
            <%= Html.TextBox("EventDate", String.Format("{0:g}", Model.Dinner.EventDate)) %>
            <%= Html.ValidationMessage("EventDate", "*") %>
        </p>
        <p>
            <label for="Description">Description:</label>
            <%= Html.TextArea("Description", Model.Dinner.Description) %>
            <%= Html.ValidationMessage("Description", "*")%>
        </p>
        <p>
            <label for="Address">Address:</label>
            <%= Html.TextBox("Address", Model.Dinner.Address) %>
            <%= Html.ValidationMessage("Address", "*") %>
        </p>
        <p>
            <label for="Country">Country:</label>
            <%= Html.DropDownList("Country", Model.Countries) %>                
            <%= Html.ValidationMessage("Country", "*") %>
        </p>
        <p>
            <label for="ContactPhone">Contact Info:</label>
            <%= Html.TextBox("ContactPhone", Model.Dinner.ContactPhone) %>
            <%= Html.ValidationMessage("ContactPhone", "*") %>
        </p>
        <p>
            <%= Html.Hidden("Latitude", Model.Dinner.Latitude)%>
            <%= Html.Hidden("Longitude", Model.Dinner.Longitude)%>
        </p>                 
        <p>
            <input type="submit" value="Save" />
        </p>
            
        </div>
        
        <div id="mapDiv">    
            <% Html.RenderPartial("Map", Model.Dinner); %>
        </div> 
            
    </fieldset>

    <script type="text/javascript">
    //<![CDATA[
        $(document).ready(function() {
            $("#Address").blur(function(evt) {
                //If it's time to look for an address, 
                // clear out the Lat and Lon
                $("#Latitude").val("0");
                $("#Longitude").val("0");

                var address = jQuery.trim($("#Address").val());
                if (address.length < 1)
                    return;

                NerdDinner.FindAddressOnMap(address);
            });
        });
    //]]>
    </script>

<% } %>
