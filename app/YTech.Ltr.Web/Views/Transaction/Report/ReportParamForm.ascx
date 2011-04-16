<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<YTech.Ltr.Web.Controllers.ViewModel.ReportParamViewModel>" %>
<%@ Import Namespace="YTech.Ltr.Web.Controllers.Helper" %>
<%--<% using (Html.BeginForm())
   { %>--%>
     <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
                       {%>
<%= Html.AntiForgeryToken() %>
<table>
  <%--  <tr>
        <td>
            <label for="ExportFormat">
                Format Laporan :</label>
        </td>
        <td>
            <%= Html.DropDownList("ExportFormat")%>
        </td>
    </tr>--%>
    <% if (ViewData.Model.ShowDateFrom)
       {	%>
    <tr>
        <td>
            <label for="DateFrom">
                Tanggal :</label>
        </td>
        <td>
            <%= Html.TextBox("DateFrom", (Model.DateFrom.HasValue) ? Model.DateFrom.Value.ToString(CommonHelper.DateFormat) : "")%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowDateTo)
       {	%>
    <tr>
        <td>
            <label for="DateTo">
                Sampai Tanggal :</label>
        </td>
        <td>
            <%= Html.TextBox("DateTo", (Model.DateTo.HasValue) ? Model.DateTo.Value.ToString(CommonHelper.DateFormat) : "")%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowAgent)
       {	%>
    <tr>
        <td>
            <label for="AgenId">
                Agen :</label>
        </td>
        <td>
            <%= Html.DropDownList("AgenId", Model.AgentList)%>
        </td>
    </tr>
    <% } %>
    <tr>
        <td colspan="2" align="center">
            <button id="Save" type="submit" name="Save">
                Lihat Laporan</button>
        </td>
    </tr>
</table>
<% } %>
<script language="javascript" type="text/javascript">
    function onSavedSuccess(e) {
        var json = e.get_response().get_object();
        var urlreport ='<%= ResolveUrl("~/ReportViewer.aspx?rpt=") %>' + json.UrlReport;
        //alert(urlreport);
        window.open(urlreport);
    }
    
    $(document).ready(function () {
       // $("#Save").button();
        $("#DateFrom").datepicker({ dateFormat: "dd-M-yy" });
        $("#DateTo").datepicker({ dateFormat: "dd-M-yy" });
    });
</script>
