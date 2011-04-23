<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" Inherits="System.Web.Mvc.ViewPage<AgentCommViewModel>" %>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
  <% foreach (MGame game in Model.ListGames )
{ %>

  <%= game.Id%>
  <%= game.GameName%>
<% } %>
</asp:Content>
