<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    About
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>What is NerdDinner.com?</h2>
    <div id="about">
         <p><img src="/Content/nerd.jpg" height="200" style="float:left; padding-right:20px" alt="Picture of a huge nerd."/>Are you a huge nerd? Perhaps a geek? No? Maybe a dork, dweeb or wonk. 
         Quite possibly you're just a normal person. Either way, you're a social being. You need to get out for a bite
         to eat occasionally, preferably with folks that are like you.</p>
         <p>Enter <a href="http://www.nerddinner.com">NerdDinner.com</a>, for all your event planning needs. We're focused on one thing. Organizing the world's
         nerds and helping them eat in packs. </p>
         <p>We're free and fun. <a href="/">Find a dinner near you</a>, or <a href="/dinners/create">host a dinner</a>. Be social.</p>
         <p>We also have blog badges and widgets that you can install on your blog or website so your readers can get 
         involved in the Nerd Dinner movement. Well, it's not really a movement. Mostly just geeks in a food court, but still.</p>
	</div>
</asp:Content>
